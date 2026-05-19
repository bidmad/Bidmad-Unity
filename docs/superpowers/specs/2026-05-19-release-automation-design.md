# Release Automation Design — `scripts/release.sh`

**Date:** 2026-05-19
**Status:** Draft, awaiting review
**Repo:** `bidmad/Bidmad-Unity`

## Goal

Provide a one-command, locally-runnable workflow that produces a GitHub
release page equivalent to
[3.9.2](https://github.com/bidmad/Bidmad-Unity/releases/tag/3.9.2) — a tag,
a release with auto-generated notes, and a `.unitypackage` asset built
from the current source tree.

The workflow must verify that the source compiles, the changelog is
updated, and the toolchain matches the project before pushing anything
remote.

## Non-Goals

- No CI / GitHub Actions setup. Local Mac execution only.
- No support for prerelease channels, beta tags, or multi-asset releases.
- No automated CHANGELOG editing — the changelog is hand-edited by the
  releaser before invoking the script.

## Ground Truth From Release 3.9.2

These values are fixed inputs to the design, derived by inspecting the
existing 3.9.2 release and the project layout:

| Field | Value |
| --- | --- |
| Tag format | `<major>.<minor>.<patch>` (no `v` prefix) |
| Release title | `BidmadUnityPlugin <version> Update` |
| Asset filename | `BidmadUnityPlugin_<version>.unitypackage` |
| Unity editor version | `2022.3.62f3` (from `BidmadPluginSample/ProjectSettings/ProjectVersion.txt`) |
| Unity binary path (Mac) | `/Applications/Unity/Hub/Editor/2022.3.62f3/Unity.app/Contents/MacOS/Unity` |
| Export folders | `Assets/Bidmad/Editor`, `Assets/Bidmad/Scripts/Bidmad`, `Assets/ExternalDependencyManager`, `Assets/Plugins/iOS/Bidmad` |
| Reference asset size | ~380 KB (sanity bound: built asset must be > 50 KB) |
| Target repo | `bidmad/Bidmad-Unity` |

The four export folders were confirmed by extracting
`BidmadUnityPlugin_3.9.2.unitypackage` and listing every `pathname` file
inside.

**Important: do not pass `Assets/Bidmad` as a top-level export root.**
The current `BidmadPluginSample` project includes sample/demo code under
`Assets/Bidmad/Scripts/AdSample/`, `Assets/Bidmad/Scripts/ButtonEvent.cs`,
and `Assets/Bidmad/Scripts/gameobject.cs`. These are intentionally part of
the in-repo sample but must **not** ship in the redistributable. Naming
the two specific subfolders `Assets/Bidmad/Editor` and
`Assets/Bidmad/Scripts/Bidmad` selects the redistributable surface
without sweeping in samples.

`Assets/Plugins/Android/`, `Assets/Resources/`, and `Assets/Scenes/` are
also intentionally excluded — the first contains project-specific gradle
templates, the latter two are sample content.

## Pipeline

The script executes five phases. Each phase fails fast with a clear,
single-line error message. No phase has side effects on the remote until
phase 4, and `--dry-run` stops execution after phase 3.

### Phase 1 — Preflight

1. Resolve target version:
   - If `--version <ver>` is passed (or positional arg given), use it.
   - Else parse the first `# Version X.Y.Z` heading in `CHANGELOG.md`.
2. Validate version matches `^[0-9]+\.[0-9]+\.[0-9]+$`.
3. Confirm a `# Version <ver>` section exists in `CHANGELOG.md`.
4. Read `BidmadPluginSample/ProjectSettings/ProjectVersion.txt` and confirm
   `m_EditorVersion` equals `2022.3.62f3` (or value of `UNITY_VERSION`
   env override if set).
5. Confirm Unity binary exists at the expected path (or `UNITY_BIN`
   env override if set) and is executable.
6. Confirm `gh auth status` succeeds.
7. Confirm `git status --porcelain` is empty. **Refuse on dirty tree.**
8. Confirm current branch.
   - If `master`, proceed silently.
   - If not, emit a warning to stderr and proceed. (Allow via design;
     no `--force-branch` flag needed.)
9. Confirm tag `<version>` does not exist locally
   (`git rev-parse --verify "refs/tags/<version>"` non-zero) **and**
   does not exist on origin
   (`git ls-remote --tags origin "<version>"` empty).
10. Confirm release `<version>` does not already exist on the target repo
    (`gh release view <version> --repo "${RELEASE_REPO:-bidmad/Bidmad-Unity}"`
    returns non-zero).

### Phase 2 — Compile and Export

1. Ensure `releases/` directory exists.
2. Set env var `BIDMAD_PACKAGE_OUTPUT=releases/BidmadUnityPlugin_<version>.unitypackage`
   (absolute path).
3. Invoke Unity:
   ```
   "$UNITY_BIN" \
     -batchmode -nographics -quit \
     -projectPath BidmadPluginSample \
     -executeMethod Bidmad.Editor.BidmadPackageExporter.Export \
     -logFile - \
     | tee "releases/unity_<version>.log"
   ```
4. Capture Unity's exit code from `${PIPESTATUS[0]}`. Non-zero → fail.
5. Confirm the output `.unitypackage` exists and is greater than 50 KB.
   (Sanity check against a silent export-no-files bug.)

### Phase 3 — Release Notes Extraction

1. Extract the lines between `# Version <version>` and the next
   `# Version ` header (exclusive of both markers).
2. Strip leading/trailing blank lines.
3. Write to `releases/notes_<version>.md`.

(If `--dry-run`, stop here and print where to find the artifacts.)

### Phase 4 — Push and Tag

1. Push the current branch:
   ```
   branch="$(git rev-parse --abbrev-ref HEAD)"
   git push origin "refs/heads/$branch:refs/heads/$branch"
   ```
   Explicit refspec form avoids any reliance on upstream tracking config.
2. `git tag <version>` (lightweight tag — GitHub creates the release from
   it identically to an annotated tag, and lightweight matches the
   simpler intent).
3. `git push origin "refs/tags/<version>:refs/tags/<version>"`.

**Assumption:** the `origin` remote points at the same GitHub repo as
`RELEASE_REPO`. If `RELEASE_REPO` is overridden for a sandbox test, the
caller is responsible for updating `origin` (or using a separate clone).
Mismatch would push the tag to one repo while creating the release in
another. This is acceptable because the override is a developer-only
seam.

### Phase 5 — Publish Release

```
gh release create <version> \
  --repo "${RELEASE_REPO:-bidmad/Bidmad-Unity}" \
  --title "BidmadUnityPlugin <version> Update" \
  --notes-file "releases/notes_<version>.md" \
  "releases/BidmadUnityPlugin_<version>.unitypackage"
```

Print the URL of the created release on success.

## CLI Surface

```
Usage: scripts/release.sh [<version>] [--dry-run] [--help]

Arguments:
  <version>       Override CHANGELOG-detected version (e.g., 3.9.3).

Flags:
  --dry-run       Run phases 1-3; skip git push, tag, and gh release.
  --help          Print usage.

Environment overrides (rarely needed):
  UNITY_BIN       Override Unity binary path.
  UNITY_VERSION   Override expected Unity version string.
  RELEASE_REPO    Override target GitHub repo (used by gh --repo).
                  Default: bidmad/Bidmad-Unity.
```

`RELEASE_REPO` is the seam used by the Tier-4 sandbox-repo test path,
even though that tier is out of scope for the initial implementation.

## Components

### `scripts/release.sh`

A Bash script (`#!/usr/bin/env bash`, `set -euo pipefail`).

Decomposed into named functions to allow Tier-1 unit testing:

| Function | Pure? | Purpose |
| --- | --- | --- |
| `parse_latest_version <changelog>` | yes | Return first `# Version X.Y.Z` |
| `extract_notes <changelog> <ver>` | yes | Return section body |
| `read_unity_version <project_path>` | yes | Return `m_EditorVersion` value |
| `is_clean_tree` | no (calls `git`) | Stub-tested in Tier 2 |
| `tag_exists_locally <ver>` | no | Stub-tested in Tier 2 |
| `tag_exists_remotely <ver>` | no | Stub-tested in Tier 2 |
| `current_branch` | no | Stub-tested in Tier 2 |

`main()` orchestrates the five phases and is the only function with
side effects beyond filesystem writes under `releases/`.

### `BidmadPluginSample/Assets/Editor/BidmadPackageExporter.cs`

The script lives under a **top-level** `Assets/Editor/` folder — not
`Assets/Bidmad/Editor/`. Unity treats any folder named `Editor` as
editor-only regardless of position, but the export folder list
includes `Assets/Bidmad/Editor/` recursively, so a script placed
there would leak into the redistributable .unitypackage. Putting it
at the top level keeps it editor-only AND out of the export scope.

```csharp
namespace Bidmad.Editor {
    public static class BidmadPackageExporter {
        public static void Export() {
            try {
                string outPath = System.Environment.GetEnvironmentVariable(
                    "BIDMAD_PACKAGE_OUTPUT");
                if (string.IsNullOrEmpty(outPath)) {
                    UnityEngine.Debug.LogError(
                        "BIDMAD_PACKAGE_OUTPUT env var not set");
                    UnityEditor.EditorApplication.Exit(2);
                    return;
                }
                string[] folders = new[] {
                    "Assets/Bidmad/Editor",
                    "Assets/Bidmad/Scripts/Bidmad",
                    "Assets/ExternalDependencyManager",
                    "Assets/Plugins/iOS/Bidmad",
                };
                UnityEditor.AssetDatabase.ExportPackage(
                    folders, outPath,
                    UnityEditor.ExportPackageOptions.Recurse);
                UnityEditor.EditorApplication.Exit(0);
            } catch (System.Exception ex) {
                UnityEngine.Debug.LogError(
                    "BidmadPackageExporter failed: " + ex);
                UnityEditor.EditorApplication.Exit(1);
            }
        }
    }
}
```

Notes:
- `ExportPackageOptions.Recurse` is sufficient; the folders already
  carry their own `.meta` files and dependencies (the iOS native files
  reference no Unity assets, EDM4U is self-contained).
- A C# compile error anywhere in the project causes Unity batchmode to
  exit non-zero before reaching this method, which is exactly the
  syntax-check semantics we want.

### `releases/`

Output directory for `.unitypackage`, `notes_<version>.md`, and
`unity_<version>.log`. Added to `.gitignore`. Not committed.

### Tests

Layout:

```
tests/
├── unit/
│   ├── parse_version.bats
│   ├── extract_notes.bats
│   └── read_unity_version.bats
├── integration/
│   ├── preflight.bats
│   ├── dry_run.bats
│   └── full_flow.bats
├── fixtures/
│   ├── CHANGELOG_normal.md
│   ├── CHANGELOG_missing_version.md
│   ├── CHANGELOG_version_at_end.md
│   ├── CHANGELOG_malformed_header.md
│   ├── ProjectVersion.txt
│   └── expected_paths_3.9.x.txt
└── stubs/
    └── bin/
        ├── git
        ├── gh
        └── Unity
```

A `tests/run.sh` script discovers and runs `*.bats` files; a top-level
`Makefile` exposes `make test`, `make test-build`, and `make test-all`
for ergonomic invocation.

## Testing Strategy

Three tiers, scoped per the user's approval:

### Tier 1 — Pure-function unit tests (bats-core)

Test the three pure parsing functions against fixture inputs. Total
expected runtime: < 2 seconds.

`parse_latest_version`:
- Normal CHANGELOG → returns `3.9.2`
- Version at end of file (no following header) → still returns it
- Empty file → exits non-zero
- File with only malformed `## 3.9.2` headers → exits non-zero

`extract_notes`:
- Asks for version present in middle of file → returns just that section
- Asks for top version → returns section, no preceding garbage
- Asks for bottom version → returns section, stops at EOF
- Asks for missing version → exits non-zero
- Section with blank lines → preserved internally, stripped at edges

`read_unity_version`:
- Normal `ProjectVersion.txt` → `2022.3.62f3`
- File missing → exits non-zero
- Malformed file (no `m_EditorVersion:` line) → exits non-zero

### Tier 2 — Integration tests with stubbed `git` / `gh` / `Unity`

Each `.bats` test:
1. Creates a `mktemp -d` working dir.
2. Copies the project skeleton (just `CHANGELOG.md` + the
   `ProjectSettings/ProjectVersion.txt` fixture).
3. Initializes a git repo, sets up a fake `origin` remote (also a local
   `mktemp -d` bare repo).
4. Prepends `tests/stubs/bin` to `PATH`. Stubs log every invocation to
   `$STUB_LOG` and exit per `$STUB_<name>_EXIT` env vars.
5. The `Unity` stub additionally `touch`es `$BIDMAD_PACKAGE_OUTPUT` and
   `dd`s 100 KB of zeros into it, so the > 50 KB sanity check passes.
6. Runs `release.sh` and asserts on exit code + `$STUB_LOG` contents.

Cases:

| Test | Setup | Assert |
| --- | --- | --- |
| Happy path on `master` | Clean tree, valid CHANGELOG, no existing tag | Exit 0; `STUB_LOG` shows `gh release create 3.9.3 --title ... --notes-file ... releases/BidmadUnityPlugin_3.9.3.unitypackage`; tag pushed |
| Happy path on feature branch | Same, on `claude/foo` branch | Exit 0; stderr contains `warning: not on master`; rest as above |
| Dirty tree | Unstaged change | Exit non-zero; `STUB_LOG` shows no `gh` calls, no `Unity` invocation |
| Tag already exists locally | Pre-create the tag | Exit non-zero; no `Unity` invocation |
| Tag already exists on origin | Push tag to fake origin first | Exit non-zero; no `Unity` invocation |
| Version missing from CHANGELOG | Pass `--version 9.9.9` | Exit non-zero before Unity |
| Wrong Unity version in ProjectVersion | Fixture with `2021.3.0f1` | Exit non-zero before Unity |
| Unity compile fail | `STUB_Unity_EXIT=1` | Exit non-zero; no git push, no gh release |
| Unity exports empty package | Stub creates 10-byte file | Exit non-zero on size check; no git push |
| `--dry-run` | Clean state | Exit 0; `STUB_LOG` shows Unity call but no git push, no gh release; `releases/notes_<version>.md` exists |
| Custom `RELEASE_REPO` | Set env var | `gh` invocation contains `--repo <custom>` |

Total expected runtime: < 10 seconds.

### Tier 3 — Real Unity dry-run smoke test

Manual one-liner:
```
./scripts/release.sh --dry-run
```
- Uses a freshly-bumped CHANGELOG entry (local-only, never committed).
- Real Unity, real compile (catches actual C# errors in the codebase).
- Inspect output:
  ```
  tar tzf releases/BidmadUnityPlugin_<ver>.unitypackage | \
    while read d; do cat extracted/$d/pathname 2>/dev/null; done
  ```
- Should match the three expected top-level folders.

Expected runtime: 2–4 minutes (Unity startup + import dominates).

This tier is run before the first real release after the script is
written, and again any time `BidmadPackageExporter.cs` or the export
folder list changes.

## Expected Output Structure

A `.unitypackage` is a gzipped tar archive where each asset lives in a
GUID-named directory with three sibling files: `asset` (the file
contents), `asset.meta` (the Unity meta), and `pathname` (the target
import path). The package format also includes folder-marker entries
for each containing directory, so Unity can re-create the parent folder
structure when the user imports.

The script's output package, **after extraction**, must contain exactly
the following `pathname` entries (and nothing else):

```
Assets/Bidmad/Editor
Assets/Bidmad/Editor/BidmadPostProcessBuild.cs
Assets/Bidmad/Editor/OBHDependencies.xml
Assets/Bidmad/Editor/Unity.iOS.Extensions.Xcode.dll
Assets/Bidmad/Scripts/Bidmad
Assets/Bidmad/Scripts/Bidmad/BidmadBanner.cs
Assets/Bidmad/Scripts/Bidmad/BidmadCommon.cs
Assets/Bidmad/Scripts/Bidmad/BidmadGoogleGDPR.cs
Assets/Bidmad/Scripts/Bidmad/BidmadInterstitial.cs
Assets/Bidmad/Scripts/Bidmad/BidmadManager.cs
Assets/Bidmad/Scripts/Bidmad/BidmadReward.cs
Assets/ExternalDependencyManager
Assets/ExternalDependencyManager/Editor
Assets/ExternalDependencyManager/Editor/1.2.186
Assets/ExternalDependencyManager/Editor/1.2.186/Google.IOSResolver.dll
Assets/ExternalDependencyManager/Editor/1.2.186/Google.JarResolver.dll
Assets/ExternalDependencyManager/Editor/1.2.186/Google.PackageManagerResolver.dll
Assets/ExternalDependencyManager/Editor/1.2.186/Google.VersionHandlerImpl.dll
Assets/ExternalDependencyManager/Editor/CHANGELOG.md
Assets/ExternalDependencyManager/Editor/Google.VersionHandler.dll
Assets/ExternalDependencyManager/Editor/LICENSE
Assets/ExternalDependencyManager/Editor/README.md
Assets/ExternalDependencyManager/Editor/external-dependency-manager_version-1.2.186_manifest.txt
Assets/Plugins/iOS/Bidmad
Assets/Plugins/iOS/Bidmad/OpenBiddingHelperUnityBridge.h
Assets/Plugins/iOS/Bidmad/OpenBiddingHelperUnityBridge.mm
Assets/Plugins/iOS/Bidmad/SwiftEnabler.swift
```

**Note on parent folder markers.** The historical 3.9.2 release also
listed parent-folder entries like `Assets/Bidmad`,
`Assets/Bidmad/Scripts`, `Assets/Plugins`, and `Assets/Plugins/iOS`.
`AssetDatabase.ExportPackage` does **not** include parent folder
markers for parents that lie outside the export folder list. Children
and descendants of named folders are included (so
`Assets/ExternalDependencyManager/Editor/1.2.186` shows up because it
is below the named `Assets/ExternalDependencyManager`), but parents of
named folders are not. The customer experience is identical either
way — Unity's importer auto-creates any missing parent folders on
import, with no GUID conflict because the parent folders don't yet
exist in the destination project.

When a customer imports this `.unitypackage` into a fresh Unity project,
the resulting project's `Assets/` tree looks like this:

```
Assets/
├── Bidmad/
│   ├── Editor/
│   │   ├── BidmadPostProcessBuild.cs
│   │   ├── OBHDependencies.xml
│   │   └── Unity.iOS.Extensions.Xcode.dll
│   └── Scripts/
│       └── Bidmad/
│           ├── BidmadBanner.cs
│           ├── BidmadCommon.cs
│           ├── BidmadGoogleGDPR.cs
│           ├── BidmadInterstitial.cs
│           ├── BidmadManager.cs
│           └── BidmadReward.cs
├── ExternalDependencyManager/
│   └── Editor/
│       ├── 1.2.186/
│       │   ├── Google.IOSResolver.dll
│       │   ├── Google.JarResolver.dll
│       │   ├── Google.PackageManagerResolver.dll
│       │   └── Google.VersionHandlerImpl.dll
│       ├── CHANGELOG.md
│       ├── Google.VersionHandler.dll
│       ├── LICENSE
│       ├── README.md
│       └── external-dependency-manager_version-1.2.186_manifest.txt
└── Plugins/
    └── iOS/
        └── Bidmad/
            ├── OpenBiddingHelperUnityBridge.h
            ├── OpenBiddingHelperUnityBridge.mm
            └── SwiftEnabler.swift
```

Plus a `.meta` file alongside every file and folder (Unity generates
these from the import; the script does not need to manage them).

### Output Structure Verification

The Tier-3 dry-run smoke test asserts this structure. Concretely:

```bash
# After ./scripts/release.sh --dry-run produces the .unitypackage:
tmp=$(mktemp -d)
tar -xzf releases/BidmadUnityPlugin_<ver>.unitypackage -C "$tmp"

# Each GUID dir has a `pathname` file containing the asset's target path.
# head -n1 guards against trailing newlines / multi-line pathname files.
for f in "$tmp"/*/pathname; do
  head -n1 "$f"
done | sort > "$tmp/actual_paths.txt"

diff -u tests/fixtures/expected_paths_3.9.x.txt "$tmp/actual_paths.txt"
```

Both `expected_paths_3.9.x.txt` and the runtime output are sorted, so
`diff -u` exits zero only on an exact match. Any addition or removal in
the redistributable surface produces a contextual diff that points at
the specific path.

The fixture file `tests/fixtures/expected_paths_3.9.x.txt` contains the
pathname list above. When the redistributable surface intentionally
changes (e.g., a new `Bidmad*.cs` file lands under
`Assets/Bidmad/Scripts/Bidmad/`), update the fixture in the same commit
as the source change.

The Tier-2 stub `Unity` does not need to produce real package contents —
it only needs to write a > 50 KB file at `$BIDMAD_PACKAGE_OUTPUT` so the
size-sanity check passes. Pathname-set verification is exclusively a
Tier-3 concern, because only a real Unity export produces real package
contents.

## Error Handling and Idempotency

- Every error exits with a unique non-zero code and a single stderr line
  identifying which check failed. No silent partial failures.
- The script never amends or rewrites history. Any tag-creation failure
  leaves the local repo untouched.
- If phase 4 succeeds but phase 5 fails: tag is pushed but release is
  not created. The user can re-run `gh release create` manually with the
  already-built artifact under `releases/`. The script does not attempt
  to roll back a pushed tag.
- Re-running after a successful release: preflight refuses because the
  tag and release already exist. Tag deletion is intentionally a manual
  operation.

## Files Added / Modified

**Added:**
- `scripts/release.sh`
- `BidmadPluginSample/Assets/Editor/BidmadPackageExporter.cs`
- `BidmadPluginSample/Assets/Editor/BidmadPackageExporter.cs.meta`
  (Unity will auto-generate this `.meta` the first time the project is
  opened in the editor; commit it alongside the `.cs` file so other
  developers don't get a meta-regeneration diff on next checkout)
- `tests/run.sh`
- `tests/unit/*.bats`
- `tests/integration/*.bats`
- `tests/fixtures/*` (CHANGELOG variants, ProjectVersion sample, and
  `expected_paths_3.9.x.txt` — the canonical pathname list verified
  against `.unitypackage` output)
- `tests/stubs/bin/{git,gh,Unity}`
- `Makefile`
- `docs/superpowers/specs/2026-05-19-release-automation-design.md` (this file)

**Modified:**
- `.gitignore` — add `releases/` entry

## Open Questions

None at design time. Resolved during brainstorming:
- Execution environment → local Mac script.
- Export folders → three folders matching 3.9.2 contents.
- Working tree policy → refuse if dirty.
- Branch policy → warn but allow off-master.
- Notes source → auto-extracted CHANGELOG section.
- Testing scope → Tiers 1, 2, 3 (skip Tier 4 sandbox e2e).
