# Release Automation Implementation Plan

> **For agentic workers:** REQUIRED SUB-SKILL: Use superpowers:subagent-driven-development (recommended) or superpowers:executing-plans to implement this plan task-by-task. Steps use checkbox (`- [ ]`) syntax for tracking.

**Goal:** Build `scripts/release.sh`, a one-command Mac-local workflow that compiles the Unity plugin, exports a `.unitypackage` matching the 3.9.2 layout, pushes a tag, and creates a GitHub release with auto-extracted notes.

**Architecture:** Bash orchestrator (`scripts/release.sh`) sources pure functions from `scripts/lib/release-functions.sh`. A Unity C# editor script (`Bidmad.Editor.BidmadPackageExporter`) does the actual `.unitypackage` export via batchmode. Tests use bats-core: unit tests for pure functions, integration tests against stubbed `git`/`gh`/`Unity`, and a manual real-Unity dry-run smoke.

**Tech Stack:** Bash 5+, `bats-core` (Homebrew), Unity 2022.3.62f3 (already installed), `gh` CLI (already authenticated).

**Spec:** [`docs/superpowers/specs/2026-05-19-release-automation-design.md`](../specs/2026-05-19-release-automation-design.md)

---

## Task 1: Foundation — directories, Makefile, .gitignore, script skeletons

**Files:**
- Create: `Makefile`
- Create: `tests/run.sh`
- Create: `scripts/release.sh` (skeleton)
- Create: `scripts/lib/release-functions.sh` (skeleton)
- Modify: `.gitignore` (add `releases/`)

- [ ] **Step 1: Verify bats-core is installed**

Run: `bats --version`
Expected: prints `Bats 1.x.x`. If missing, run `brew install bats-core` first.

- [ ] **Step 2: Create the directory layout**

Run:
```bash
mkdir -p scripts/lib tests/unit tests/integration tests/fixtures tests/stubs/bin
```

- [ ] **Step 3: Create `scripts/lib/release-functions.sh` skeleton**

Write `scripts/lib/release-functions.sh`:
```bash
#!/usr/bin/env bash
# Pure functions for release.sh. Source this file; do not execute directly.

parse_latest_version() {
  echo "TODO: parse_latest_version not implemented" >&2
  return 1
}

extract_notes() {
  echo "TODO: extract_notes not implemented" >&2
  return 1
}

read_unity_version() {
  echo "TODO: read_unity_version not implemented" >&2
  return 1
}
```

- [ ] **Step 4: Create `scripts/release.sh` skeleton**

Write `scripts/release.sh`:
```bash
#!/usr/bin/env bash
set -euo pipefail

SCRIPT_DIR="$(cd "$(dirname "${BASH_SOURCE[0]}")" && pwd)"
REPO_ROOT="$(cd "$SCRIPT_DIR/.." && pwd)"
# shellcheck source=lib/release-functions.sh
source "$SCRIPT_DIR/lib/release-functions.sh"

main() {
  echo "release.sh: not yet implemented" >&2
  return 1
}

main "$@"
```

Then make both executable:
```bash
chmod +x scripts/release.sh scripts/lib/release-functions.sh
```

- [ ] **Step 5: Create `tests/run.sh`**

Write `tests/run.sh`:
```bash
#!/usr/bin/env bash
# Runs all bats tests under tests/unit and tests/integration.
set -euo pipefail
HERE="$(cd "$(dirname "${BASH_SOURCE[0]}")" && pwd)"
shopt -s nullglob
files=("$HERE"/unit/*.bats "$HERE"/integration/*.bats)
if [ ${#files[@]} -eq 0 ]; then
  echo "No .bats tests found."
  exit 0
fi
exec bats "${files[@]}"
```

Run: `chmod +x tests/run.sh`

- [ ] **Step 6: Create `Makefile`**

Write `Makefile`:
```makefile
.PHONY: test test-build test-all help

help:
	@echo "Targets:"
	@echo "  test        Run bats unit + integration tests"
	@echo "  test-build  Run real-Unity dry-run smoke test (slow)"
	@echo "  test-all    Run all of the above"

test:
	@./tests/run.sh

test-build:
	@./scripts/release.sh --dry-run

test-all: test test-build
```

- [ ] **Step 7: Update `.gitignore`**

Append to `.gitignore`:
```
# Release automation artifacts
/releases/
```

- [ ] **Step 8: Verify `make test` and `scripts/release.sh` run without crashing**

Run: `make test`
Expected: prints `No .bats tests found.` and exits 0.

Run: `./scripts/release.sh`
Expected: prints `release.sh: not yet implemented` to stderr and exits 1.

- [ ] **Step 9: Commit**

```bash
git add Makefile tests/run.sh scripts/release.sh scripts/lib/release-functions.sh .gitignore
git commit -m "Add release automation scaffolding (Makefile, skeletons, .gitignore)"
```

---

## Task 2: Implement `parse_latest_version` with unit tests (TDD)

**Files:**
- Modify: `scripts/lib/release-functions.sh` — implement `parse_latest_version`
- Create: `tests/unit/parse_version.bats`
- Create: `tests/fixtures/CHANGELOG_normal.md`
- Create: `tests/fixtures/CHANGELOG_malformed_header.md`
- Create: `tests/fixtures/CHANGELOG_empty.md`

- [ ] **Step 1: Create fixture files**

Write `tests/fixtures/CHANGELOG_normal.md`:
```
# Version 3.9.3
#### Changes
- Test bullet
#### Integration Bidmad SDK version
- Android : bidmad-androidx 3.25.0
- iOS : BidmadSDK.framework 6.13.3

# Version 3.9.2
#### Changes
- iOS Build code Bug Fix
#### Integration Bidmad SDK version
- Android : bidmad-androidx 3.25.0
- iOS : BidmadSDK.framework 6.13.3

# Version 3.9.1
#### Changes
- Ad Network SDK Update
- Bug Fix
```

Write `tests/fixtures/CHANGELOG_malformed_header.md`:
```
## Version 3.9.2
This uses ## instead of # — should not be recognized as a version heading.

### Version 3.9.1
Same problem.
```

Write `tests/fixtures/CHANGELOG_empty.md` (must be empty):
```bash
: > tests/fixtures/CHANGELOG_empty.md
```

- [ ] **Step 2: Write the failing tests**

Write `tests/unit/parse_version.bats`:
```bash
#!/usr/bin/env bats

setup() {
  source "$BATS_TEST_DIRNAME/../../scripts/lib/release-functions.sh"
  FIXTURES="$BATS_TEST_DIRNAME/../fixtures"
}

@test "parse_latest_version returns top version from normal changelog" {
  run parse_latest_version "$FIXTURES/CHANGELOG_normal.md"
  [ "$status" -eq 0 ]
  [ "$output" = "3.9.3" ]
}

@test "parse_latest_version fails on missing file" {
  run parse_latest_version "/tmp/definitely-not-a-real-changelog-$$.md"
  [ "$status" -ne 0 ]
}

@test "parse_latest_version fails when no version heading exists" {
  run parse_latest_version "$FIXTURES/CHANGELOG_malformed_header.md"
  [ "$status" -ne 0 ]
}

@test "parse_latest_version fails on empty file" {
  run parse_latest_version "$FIXTURES/CHANGELOG_empty.md"
  [ "$status" -ne 0 ]
}
```

- [ ] **Step 3: Run tests to verify they fail**

Run: `bats tests/unit/parse_version.bats`
Expected: all 4 tests fail (the skeleton returns 1 with a TODO message).

- [ ] **Step 4: Implement `parse_latest_version`**

In `scripts/lib/release-functions.sh`, replace the `parse_latest_version` stub with:
```bash
parse_latest_version() {
  local changelog="${1:-}"
  if [[ -z "$changelog" || ! -f "$changelog" ]]; then
    echo "Error: changelog file not found: $changelog" >&2
    return 1
  fi
  local version
  version="$(grep -E '^# Version [0-9]+\.[0-9]+\.[0-9]+$' "$changelog" | head -n1 | sed 's/^# Version //')"
  if [[ -z "$version" ]]; then
    echo "Error: no '# Version X.Y.Z' heading found in $changelog" >&2
    return 1
  fi
  printf '%s\n' "$version"
}
```

- [ ] **Step 5: Run tests to verify they pass**

Run: `bats tests/unit/parse_version.bats`
Expected: all 4 tests pass.

- [ ] **Step 6: Commit**

```bash
git add scripts/lib/release-functions.sh tests/unit/parse_version.bats tests/fixtures/CHANGELOG_*.md
git commit -m "Implement parse_latest_version with bats unit tests"
```

---

## Task 3: Implement `extract_notes` with unit tests (TDD)

**Files:**
- Modify: `scripts/lib/release-functions.sh` — implement `extract_notes`
- Create: `tests/unit/extract_notes.bats`
- Create: `tests/fixtures/CHANGELOG_version_at_end.md`

- [ ] **Step 1: Create the additional fixture**

Write `tests/fixtures/CHANGELOG_version_at_end.md`:
```
# Version 3.9.3
#### Changes
- Top entry
- More content

# Version 3.9.2
#### Changes
- Last entry, no following version
- Final line of file
```

(No trailing newline — this stresses the EOF-handling branch.)

- [ ] **Step 2: Write failing tests**

Write `tests/unit/extract_notes.bats`:
```bash
#!/usr/bin/env bats

setup() {
  source "$BATS_TEST_DIRNAME/../../scripts/lib/release-functions.sh"
  FIXTURES="$BATS_TEST_DIRNAME/../fixtures"
}

@test "extract_notes returns body of top version" {
  run extract_notes "$FIXTURES/CHANGELOG_normal.md" "3.9.3"
  [ "$status" -eq 0 ]
  [[ "$output" == *"Test bullet"* ]]
  [[ "$output" != *"# Version 3.9.2"* ]]
  [[ "$output" != *"iOS Build code Bug Fix"* ]]
}

@test "extract_notes returns body of middle version, bounded by next heading" {
  run extract_notes "$FIXTURES/CHANGELOG_normal.md" "3.9.2"
  [ "$status" -eq 0 ]
  [[ "$output" == *"iOS Build code Bug Fix"* ]]
  [[ "$output" != *"Ad Network SDK Update"* ]]
}

@test "extract_notes returns body of last version (no following heading)" {
  run extract_notes "$FIXTURES/CHANGELOG_version_at_end.md" "3.9.2"
  [ "$status" -eq 0 ]
  [[ "$output" == *"Last entry, no following version"* ]]
  [[ "$output" == *"Final line of file"* ]]
}

@test "extract_notes strips trailing blank lines" {
  run extract_notes "$FIXTURES/CHANGELOG_normal.md" "3.9.3"
  [ "$status" -eq 0 ]
  last_line="$(printf '%s' "$output" | tail -n1)"
  [ -n "$last_line" ]
}

@test "extract_notes fails when version is not present" {
  run extract_notes "$FIXTURES/CHANGELOG_normal.md" "9.9.9"
  [ "$status" -ne 0 ]
}

@test "extract_notes fails on missing file" {
  run extract_notes "/tmp/nope-$$.md" "3.9.3"
  [ "$status" -ne 0 ]
}
```

- [ ] **Step 3: Run tests to verify they fail**

Run: `bats tests/unit/extract_notes.bats`
Expected: all 6 tests fail.

- [ ] **Step 4: Implement `extract_notes`**

In `scripts/lib/release-functions.sh`, replace the `extract_notes` stub with:
```bash
extract_notes() {
  local changelog="${1:-}"
  local version="${2:-}"
  if [[ -z "$changelog" || ! -f "$changelog" ]]; then
    echo "Error: changelog file not found: $changelog" >&2
    return 1
  fi
  if [[ -z "$version" ]]; then
    echo "Error: version argument required" >&2
    return 1
  fi
  local body
  body="$(awk -v target="# Version $version" '
    $0 == target { capture = 1; next }
    capture && /^# Version / { exit }
    capture
  ' "$changelog")"
  # Strip leading and trailing blank lines and detect empty
  body="$(printf '%s' "$body" | awk '
    NF { if (!started) started = 1 }
    started { lines[++n] = $0 }
    END {
      while (n > 0 && lines[n] ~ /^[[:space:]]*$/) n--
      for (i = 1; i <= n; i++) print lines[i]
    }
  ')"
  if [[ -z "$body" ]]; then
    echo "Error: no notes found for version $version in $changelog" >&2
    return 1
  fi
  printf '%s\n' "$body"
}
```

- [ ] **Step 5: Run tests to verify they pass**

Run: `bats tests/unit/extract_notes.bats`
Expected: all 6 tests pass.

- [ ] **Step 6: Commit**

```bash
git add scripts/lib/release-functions.sh tests/unit/extract_notes.bats tests/fixtures/CHANGELOG_version_at_end.md
git commit -m "Implement extract_notes with bats unit tests"
```

---

## Task 4: Implement `read_unity_version` with unit tests (TDD)

**Files:**
- Modify: `scripts/lib/release-functions.sh` — implement `read_unity_version`
- Create: `tests/unit/read_unity_version.bats`
- Create: `tests/fixtures/ProjectSettings_good/ProjectVersion.txt`
- Create: `tests/fixtures/ProjectSettings_bad/ProjectVersion.txt`

- [ ] **Step 1: Create the fixtures**

Run:
```bash
mkdir -p tests/fixtures/ProjectSettings_good tests/fixtures/ProjectSettings_bad
```

Write `tests/fixtures/ProjectSettings_good/ProjectVersion.txt`:
```
m_EditorVersion: 2022.3.62f3
m_EditorVersionWithRevision: 2022.3.62f3 (96770f904ca7)
```

Write `tests/fixtures/ProjectSettings_bad/ProjectVersion.txt`:
```
unrelated_field: 2022.3.62f3
no_m_EditorVersion_line_at_all: oops
```

- [ ] **Step 2: Write failing tests**

Write `tests/unit/read_unity_version.bats`:
```bash
#!/usr/bin/env bats

setup() {
  source "$BATS_TEST_DIRNAME/../../scripts/lib/release-functions.sh"
  FIXTURES="$BATS_TEST_DIRNAME/../fixtures"
}

@test "read_unity_version returns m_EditorVersion value" {
  # The function takes a project path; it looks for ProjectSettings/ProjectVersion.txt.
  # The good fixture lives at tests/fixtures/ProjectSettings_good/ProjectVersion.txt
  # so we pass tests/fixtures and the function reads from ProjectSettings_good as the
  # project root would normally. To make this work cleanly we pretend the project root
  # IS the fixture dir whose ProjectSettings subdir is the fixture itself:
  tmp="$(mktemp -d)"
  mkdir -p "$tmp/ProjectSettings"
  cp "$FIXTURES/ProjectSettings_good/ProjectVersion.txt" "$tmp/ProjectSettings/"
  run read_unity_version "$tmp"
  [ "$status" -eq 0 ]
  [ "$output" = "2022.3.62f3" ]
  rm -rf "$tmp"
}

@test "read_unity_version fails when ProjectVersion.txt is missing" {
  tmp="$(mktemp -d)"
  run read_unity_version "$tmp"
  [ "$status" -ne 0 ]
  rm -rf "$tmp"
}

@test "read_unity_version fails when m_EditorVersion line is absent" {
  tmp="$(mktemp -d)"
  mkdir -p "$tmp/ProjectSettings"
  cp "$FIXTURES/ProjectSettings_bad/ProjectVersion.txt" "$tmp/ProjectSettings/"
  run read_unity_version "$tmp"
  [ "$status" -ne 0 ]
  rm -rf "$tmp"
}
```

- [ ] **Step 3: Run tests to verify they fail**

Run: `bats tests/unit/read_unity_version.bats`
Expected: all 3 tests fail.

- [ ] **Step 4: Implement `read_unity_version`**

In `scripts/lib/release-functions.sh`, replace the `read_unity_version` stub with:
```bash
read_unity_version() {
  local project_path="${1:-}"
  local pv_file="$project_path/ProjectSettings/ProjectVersion.txt"
  if [[ -z "$project_path" || ! -f "$pv_file" ]]; then
    echo "Error: $pv_file not found" >&2
    return 1
  fi
  local version
  version="$(grep -E '^m_EditorVersion:' "$pv_file" | head -n1 | sed 's/^m_EditorVersion:[[:space:]]*//')"
  if [[ -z "$version" ]]; then
    echo "Error: m_EditorVersion not found in $pv_file" >&2
    return 1
  fi
  printf '%s\n' "$version"
}
```

- [ ] **Step 5: Run tests to verify they pass**

Run: `bats tests/unit/read_unity_version.bats`
Expected: all 3 tests pass.

- [ ] **Step 6: Run the full unit suite as a regression check**

Run: `make test`
Expected: 13 tests pass total (4 + 6 + 3).

- [ ] **Step 7: Commit**

```bash
git add scripts/lib/release-functions.sh tests/unit/read_unity_version.bats tests/fixtures/ProjectSettings_*
git commit -m "Implement read_unity_version with bats unit tests"
```

---

## Task 5: Add `BidmadPackageExporter.cs` Unity editor script

**Files:**
- Create: `BidmadPluginSample/Assets/Bidmad/Editor/BidmadPackageExporter.cs`

This script is invoked by Unity batchmode. There is no bats-testable surface; Tier 3 (real Unity dry-run) is the verification.

- [ ] **Step 1: Create the editor script**

Write `BidmadPluginSample/Assets/Bidmad/Editor/BidmadPackageExporter.cs`:
```csharp
using System;
using UnityEditor;
using UnityEngine;

namespace Bidmad.Editor
{
    public static class BidmadPackageExporter
    {
        public static void Export()
        {
            try
            {
                string outPath = Environment.GetEnvironmentVariable("BIDMAD_PACKAGE_OUTPUT");
                if (string.IsNullOrEmpty(outPath))
                {
                    Debug.LogError("BIDMAD_PACKAGE_OUTPUT env var not set");
                    EditorApplication.Exit(2);
                    return;
                }

                string[] folders = new[]
                {
                    "Assets/Bidmad/Editor",
                    "Assets/Bidmad/Scripts/Bidmad",
                    "Assets/ExternalDependencyManager",
                    "Assets/Plugins/iOS/Bidmad",
                };

                AssetDatabase.ExportPackage(folders, outPath, ExportPackageOptions.Recurse);
                Debug.Log("Bidmad package exported to: " + outPath);
                EditorApplication.Exit(0);
            }
            catch (Exception ex)
            {
                Debug.LogError("BidmadPackageExporter failed: " + ex);
                EditorApplication.Exit(1);
            }
        }
    }
}
```

- [ ] **Step 2: Commit**

The `.cs.meta` file will be generated by Unity on first open and committed in Task 12. For now only the `.cs` file is committed.

```bash
git add BidmadPluginSample/Assets/Bidmad/Editor/BidmadPackageExporter.cs
git commit -m "Add BidmadPackageExporter editor script for batch export"
```

---

## Task 6: Stub binaries for integration tests

**Files:**
- Create: `tests/stubs/bin/gh`
- Create: `tests/stubs/bin/git`
- Create: `tests/stubs/bin/Unity`

Stubs log every invocation to `$STUB_LOG` and exit per env vars. The `git` stub passes through to the real `git` for everything except network-affecting subcommands (`push`, `ls-remote`).

- [ ] **Step 1: Create the `gh` stub**

Write `tests/stubs/bin/gh`:
```bash
#!/usr/bin/env bash
# Stub for the gh CLI. Logs args; exits per env-var overrides.
echo "gh $*" >> "${STUB_LOG:-/dev/null}"

case "${1:-}" in
  auth)
    exit "${STUB_GH_AUTH_EXIT:-0}"
    ;;
  release)
    case "${2:-}" in
      view)
        # Default: not found (exit 1) so release.sh's "release does not exist" check passes
        exit "${STUB_GH_RELEASE_VIEW_EXIT:-1}"
        ;;
      create)
        exit "${STUB_GH_RELEASE_CREATE_EXIT:-0}"
        ;;
    esac
    ;;
esac
exit 0
```

- [ ] **Step 2: Create the `git` stub**

Write `tests/stubs/bin/git`:
```bash
#!/usr/bin/env bash
# Stub for git. Network-affecting subcommands are intercepted; everything else
# passes through to the real git binary at $REAL_GIT.
echo "git $*" >> "${STUB_LOG:-/dev/null}"

real_git="${REAL_GIT:-/usr/bin/git}"

case "${1:-}" in
  push)
    exit "${STUB_GIT_PUSH_EXIT:-0}"
    ;;
  ls-remote)
    if [[ -n "${STUB_GIT_LSREMOTE_OUTPUT:-}" ]]; then
      printf '%s\n' "$STUB_GIT_LSREMOTE_OUTPUT"
    fi
    exit "${STUB_GIT_LSREMOTE_EXIT:-0}"
    ;;
  *)
    exec "$real_git" "$@"
    ;;
esac
```

- [ ] **Step 3: Create the `Unity` stub**

Write `tests/stubs/bin/Unity`:
```bash
#!/usr/bin/env bash
# Stub for Unity batchmode. Optionally writes a fake .unitypackage and exits.
echo "Unity $*" >> "${STUB_LOG:-/dev/null}"

if [[ -n "${BIDMAD_PACKAGE_OUTPUT:-}" && "${STUB_UNITY_WRITE_OUTPUT:-1}" = "1" ]]; then
  mkdir -p "$(dirname "$BIDMAD_PACKAGE_OUTPUT")"
  size_bytes="${STUB_UNITY_OUTPUT_SIZE:-100000}"  # default 100 KB so size check passes
  dd if=/dev/zero of="$BIDMAD_PACKAGE_OUTPUT" bs="$size_bytes" count=1 status=none
fi

exit "${STUB_UNITY_EXIT:-0}"
```

- [ ] **Step 4: Make stubs executable**

Run:
```bash
chmod +x tests/stubs/bin/gh tests/stubs/bin/git tests/stubs/bin/Unity
```

- [ ] **Step 5: Sanity-check the stubs**

Run:
```bash
STUB_LOG=/tmp/stub_sanity.log tests/stubs/bin/gh auth status
cat /tmp/stub_sanity.log
```
Expected: prints `gh auth status`.

```bash
REAL_GIT="$(command -v git)" STUB_LOG=/tmp/stub_sanity.log tests/stubs/bin/git rev-parse --show-toplevel
```
Expected: prints the current repo's top-level path (pass-through works).

```bash
rm /tmp/stub_sanity.log
```

- [ ] **Step 6: Commit**

```bash
git add tests/stubs/bin/gh tests/stubs/bin/git tests/stubs/bin/Unity
git commit -m "Add stub binaries (gh, git, Unity) for integration tests"
```

---

## Task 7: Integration test harness + Phase 1 (preflight) implementation

**Files:**
- Create: `tests/fixtures/sample_project/CHANGELOG.md`
- Create: `tests/fixtures/sample_project/BidmadPluginSample/ProjectSettings/ProjectVersion.txt`
- Create: `tests/integration/preflight.bats`
- Modify: `scripts/release.sh` — implement Phase 1

- [ ] **Step 1: Create the sample-project fixture**

Run:
```bash
mkdir -p tests/fixtures/sample_project/BidmadPluginSample/ProjectSettings
```

Write `tests/fixtures/sample_project/CHANGELOG.md`:
```
# Version 3.9.3
#### Changes
- Test release notes line one
- Test release notes line two
#### Integration Bidmad SDK version
- Android : bidmad-androidx 3.25.0
- iOS : BidmadSDK.framework 6.13.3

# Version 3.9.2
#### Changes
- Previous release
```

Write `tests/fixtures/sample_project/BidmadPluginSample/ProjectSettings/ProjectVersion.txt`:
```
m_EditorVersion: 2022.3.62f3
m_EditorVersionWithRevision: 2022.3.62f3 (96770f904ca7)
```

- [ ] **Step 2: Write the integration test harness (failing preflight tests)**

Write `tests/integration/preflight.bats`:
```bash
#!/usr/bin/env bats

setup() {
  TMPDIR_ROOT="$(mktemp -d)"
  REPO_DIR="$TMPDIR_ROOT/repo"
  ORIGIN_DIR="$TMPDIR_ROOT/origin.git"
  STUB_LOG="$TMPDIR_ROOT/stub.log"
  : > "$STUB_LOG"

  cp -R "$BATS_TEST_DIRNAME/../fixtures/sample_project/." "$REPO_DIR"
  # Pre-create the scripts dir layout in the temp repo by symlinking the project's scripts
  ln -s "$BATS_TEST_DIRNAME/../../scripts" "$REPO_DIR/scripts"

  cd "$REPO_DIR"
  git init --quiet --initial-branch=master
  git -c user.email=test@test.local -c user.name=test add -A
  git -c user.email=test@test.local -c user.name=test commit --quiet -m "initial"
  git init --bare --quiet "$ORIGIN_DIR"
  git remote add origin "$ORIGIN_DIR"
  git push --quiet origin master

  export REAL_GIT="$(command -v git)"
  export STUB_LOG
  export PATH="$BATS_TEST_DIRNAME/../stubs/bin:$PATH"
  export RELEASE_REPO="bidmad/Bidmad-Unity"  # default — overridable per test
}

teardown() {
  rm -rf "$TMPDIR_ROOT"
}

@test "preflight: refuses to run on dirty working tree" {
  cd "$REPO_DIR"
  echo "untracked" > newfile.txt
  run ./scripts/release.sh 3.9.3
  [ "$status" -ne 0 ]
  [[ "${output:-}${stderr:-}" == *"dirty"* || "$output" == *"clean"* ]]
}

@test "preflight: refuses when version is missing from CHANGELOG" {
  cd "$REPO_DIR"
  run ./scripts/release.sh 9.9.9
  [ "$status" -ne 0 ]
  ! grep -q "Unity" "$STUB_LOG"
}

@test "preflight: refuses when Unity version mismatches" {
  cd "$REPO_DIR"
  cat > BidmadPluginSample/ProjectSettings/ProjectVersion.txt <<'EOF'
m_EditorVersion: 2021.3.0f1
m_EditorVersionWithRevision: 2021.3.0f1 (xxxxxxxxxxxx)
EOF
  git -c user.email=t@t -c user.name=t commit -am "bad unity version" --quiet
  run ./scripts/release.sh 3.9.3
  [ "$status" -ne 0 ]
  ! grep -q "^Unity " "$STUB_LOG"
}

@test "preflight: refuses when tag already exists locally" {
  cd "$REPO_DIR"
  git tag 3.9.3
  run ./scripts/release.sh 3.9.3
  [ "$status" -ne 0 ]
  ! grep -q "^Unity " "$STUB_LOG"
}

@test "preflight: refuses when tag exists on remote" {
  cd "$REPO_DIR"
  export STUB_GIT_LSREMOTE_OUTPUT="abcdef1234567890 refs/tags/3.9.3"
  run ./scripts/release.sh 3.9.3
  [ "$status" -ne 0 ]
  ! grep -q "^Unity " "$STUB_LOG"
}

@test "preflight: refuses when gh release already exists" {
  cd "$REPO_DIR"
  export STUB_GH_RELEASE_VIEW_EXIT=0  # exit 0 = release exists
  run ./scripts/release.sh 3.9.3
  [ "$status" -ne 0 ]
  ! grep -q "^Unity " "$STUB_LOG"
}
```

- [ ] **Step 3: Run integration tests, verify they fail**

Run: `bats tests/integration/preflight.bats`
Expected: all 6 tests fail (`main()` still prints "not yet implemented").

- [ ] **Step 4: Implement Phase 1 in `scripts/release.sh`**

Replace `scripts/release.sh` with:
```bash
#!/usr/bin/env bash
set -euo pipefail

SCRIPT_DIR="$(cd "$(dirname "${BASH_SOURCE[0]}")" && pwd)"
REPO_ROOT="$(cd "$SCRIPT_DIR/.." && pwd)"
# shellcheck source=lib/release-functions.sh
source "$SCRIPT_DIR/lib/release-functions.sh"

# Defaults (overridable via env).
UNITY_BIN="${UNITY_BIN:-/Applications/Unity/Hub/Editor/2022.3.62f3/Unity.app/Contents/MacOS/Unity}"
UNITY_VERSION="${UNITY_VERSION:-2022.3.62f3}"
RELEASE_REPO="${RELEASE_REPO:-bidmad/Bidmad-Unity}"
PROJECT_PATH="$REPO_ROOT/BidmadPluginSample"
CHANGELOG="$REPO_ROOT/CHANGELOG.md"
RELEASES_DIR="$REPO_ROOT/releases"

DRY_RUN=0
VERSION=""

usage() {
  cat <<EOF
Usage: scripts/release.sh [<version>] [--dry-run] [--help]

Arguments:
  <version>       Override CHANGELOG-detected version (e.g., 3.9.3).

Flags:
  --dry-run       Run phases 1-3; skip git push, tag, and gh release.
  --help          Print usage.

Environment overrides:
  UNITY_BIN, UNITY_VERSION, RELEASE_REPO
EOF
}

parse_args() {
  while [[ $# -gt 0 ]]; do
    case "$1" in
      --dry-run) DRY_RUN=1; shift ;;
      --help|-h) usage; exit 0 ;;
      --*) echo "Unknown flag: $1" >&2; usage; exit 2 ;;
      *) VERSION="$1"; shift ;;
    esac
  done
}

phase1_preflight() {
  cd "$REPO_ROOT"

  if [[ -z "$VERSION" ]]; then
    VERSION="$(parse_latest_version "$CHANGELOG")"
  fi
  if ! [[ "$VERSION" =~ ^[0-9]+\.[0-9]+\.[0-9]+$ ]]; then
    echo "Error: invalid version '$VERSION'" >&2
    return 1
  fi

  # CHANGELOG section must exist
  if ! grep -qE "^# Version $VERSION$" "$CHANGELOG"; then
    echo "Error: CHANGELOG.md has no '# Version $VERSION' section" >&2
    return 1
  fi

  # Unity version match
  local detected
  detected="$(read_unity_version "$PROJECT_PATH")"
  if [[ "$detected" != "$UNITY_VERSION" ]]; then
    echo "Error: Unity version mismatch — project is $detected, expected $UNITY_VERSION" >&2
    return 1
  fi

  # Unity binary exists
  if [[ ! -x "$UNITY_BIN" ]] && ! command -v Unity >/dev/null 2>&1; then
    echo "Error: Unity binary not found at $UNITY_BIN" >&2
    return 1
  fi

  # gh auth
  if ! gh auth status >/dev/null 2>&1; then
    echo "Error: gh CLI is not authenticated" >&2
    return 1
  fi

  # Clean working tree
  if [[ -n "$(git status --porcelain)" ]]; then
    echo "Error: working tree is not clean; commit or stash before releasing" >&2
    return 1
  fi

  # Branch check
  local branch
  branch="$(git rev-parse --abbrev-ref HEAD)"
  if [[ "$branch" != "master" ]]; then
    echo "Warning: not on master (current branch: $branch); proceeding anyway" >&2
  fi

  # Tag must not exist locally
  if git rev-parse --verify --quiet "refs/tags/$VERSION" >/dev/null 2>&1; then
    echo "Error: tag '$VERSION' already exists locally" >&2
    return 1
  fi

  # Tag must not exist on origin
  if [[ -n "$(git ls-remote --tags origin "$VERSION" 2>/dev/null)" ]]; then
    echo "Error: tag '$VERSION' already exists on origin" >&2
    return 1
  fi

  # Release must not already exist
  if gh release view "$VERSION" --repo "$RELEASE_REPO" >/dev/null 2>&1; then
    echo "Error: GitHub release '$VERSION' already exists on $RELEASE_REPO" >&2
    return 1
  fi

  echo "Preflight OK: version=$VERSION, branch=$branch, repo=$RELEASE_REPO" >&2
}

main() {
  parse_args "$@"
  phase1_preflight
  # Phase 2+ implemented in later tasks. For now stop after preflight.
  echo "Preflight complete (later phases not yet implemented)" >&2
  return 0
}

main "$@"
```

- [ ] **Step 5: Run integration tests, verify they pass**

Run: `bats tests/integration/preflight.bats`
Expected: all 6 tests pass.

Run: `make test`
Expected: all unit + integration tests pass (13 unit + 6 integration = 19).

- [ ] **Step 6: Commit**

```bash
git add scripts/release.sh tests/fixtures/sample_project tests/integration/preflight.bats
git commit -m "Implement Phase 1 (preflight) + integration tests"
```

---

## Task 8: Phase 2 (compile + export) + integration tests

**Files:**
- Modify: `scripts/release.sh` — add `phase2_export`
- Create: `tests/integration/export.bats`

- [ ] **Step 1: Write failing integration tests for Phase 2**

Write `tests/integration/export.bats`:
```bash
#!/usr/bin/env bats

setup() {
  TMPDIR_ROOT="$(mktemp -d)"
  REPO_DIR="$TMPDIR_ROOT/repo"
  ORIGIN_DIR="$TMPDIR_ROOT/origin.git"
  STUB_LOG="$TMPDIR_ROOT/stub.log"
  : > "$STUB_LOG"

  cp -R "$BATS_TEST_DIRNAME/../fixtures/sample_project/." "$REPO_DIR"
  ln -s "$BATS_TEST_DIRNAME/../../scripts" "$REPO_DIR/scripts"

  cd "$REPO_DIR"
  git init --quiet --initial-branch=master
  git -c user.email=t@t -c user.name=t add -A
  git -c user.email=t@t -c user.name=t commit --quiet -m "init"
  git init --bare --quiet "$ORIGIN_DIR"
  git remote add origin "$ORIGIN_DIR"
  git push --quiet origin master

  export REAL_GIT="$(command -v git)"
  export STUB_LOG
  export PATH="$BATS_TEST_DIRNAME/../stubs/bin:$PATH"
  export RELEASE_REPO="bidmad/Bidmad-Unity"
  # Point UNITY_BIN at the stub so the executable check passes
  export UNITY_BIN="$BATS_TEST_DIRNAME/../stubs/bin/Unity"
}

teardown() {
  rm -rf "$TMPDIR_ROOT"
}

@test "phase2: invokes Unity with the expected args" {
  cd "$REPO_DIR"
  run ./scripts/release.sh 3.9.3 --dry-run
  [ "$status" -eq 0 ]
  grep -q "^Unity .*-batchmode" "$STUB_LOG"
  grep -q "^Unity .*-projectPath" "$STUB_LOG"
  grep -q "^Unity .*Bidmad.Editor.BidmadPackageExporter.Export" "$STUB_LOG"
}

@test "phase2: produces the unitypackage at the expected path" {
  cd "$REPO_DIR"
  ./scripts/release.sh 3.9.3 --dry-run
  [ -f "$REPO_DIR/releases/BidmadUnityPlugin_3.9.3.unitypackage" ]
}

@test "phase2: fails when Unity exits non-zero" {
  cd "$REPO_DIR"
  export STUB_UNITY_EXIT=1
  run ./scripts/release.sh 3.9.3 --dry-run
  [ "$status" -ne 0 ]
}

@test "phase2: fails when output file is too small" {
  cd "$REPO_DIR"
  export STUB_UNITY_OUTPUT_SIZE=10  # 10 bytes — below 50KB threshold
  run ./scripts/release.sh 3.9.3 --dry-run
  [ "$status" -ne 0 ]
}
```

- [ ] **Step 2: Run tests, verify they fail**

Run: `bats tests/integration/export.bats`
Expected: all 4 fail (`phase2_export` doesn't exist yet).

- [ ] **Step 3: Implement `phase2_export` in `scripts/release.sh`**

Add the following function to `scripts/release.sh`, just below `phase1_preflight`:
```bash
phase2_export() {
  cd "$REPO_ROOT"
  mkdir -p "$RELEASES_DIR"
  local output_path="$RELEASES_DIR/BidmadUnityPlugin_$VERSION.unitypackage"
  rm -f "$output_path"

  export BIDMAD_PACKAGE_OUTPUT="$output_path"

  # Pick the Unity invocation: explicit UNITY_BIN if it's executable, otherwise PATH lookup
  local unity_cmd="$UNITY_BIN"
  if [[ ! -x "$unity_cmd" ]]; then
    unity_cmd="$(command -v Unity)"
  fi

  local log_file="$RELEASES_DIR/unity_${VERSION}.log"
  set +e
  "$unity_cmd" \
    -batchmode -nographics -quit \
    -projectPath "$PROJECT_PATH" \
    -executeMethod Bidmad.Editor.BidmadPackageExporter.Export \
    -logFile - | tee "$log_file"
  local unity_exit=${PIPESTATUS[0]}
  set -e

  if [[ "$unity_exit" -ne 0 ]]; then
    echo "Error: Unity batchmode exited $unity_exit (see $log_file)" >&2
    return 1
  fi

  if [[ ! -f "$output_path" ]]; then
    echo "Error: expected output not produced: $output_path" >&2
    return 1
  fi

  local size
  size=$(wc -c <"$output_path" | tr -d ' ')
  if [[ "$size" -lt 50000 ]]; then
    echo "Error: output package suspiciously small ($size bytes); minimum 50000" >&2
    return 1
  fi

  echo "Export OK: $output_path ($size bytes)" >&2
}
```

And replace the `main()` body to call Phase 2:
```bash
main() {
  parse_args "$@"
  phase1_preflight
  phase2_export
  echo "Phases 1-2 complete (later phases not yet implemented)" >&2
  return 0
}
```

- [ ] **Step 4: Run tests, verify they pass**

Run: `bats tests/integration/export.bats`
Expected: 4 tests pass.

Run: `make test`
Expected: 23 tests pass (13 unit + 10 integration).

- [ ] **Step 5: Commit**

```bash
git add scripts/release.sh tests/integration/export.bats
git commit -m "Implement Phase 2 (Unity export) + integration tests"
```

---

## Task 9: Phase 3 (notes extraction) + `--dry-run` short-circuit + tests

**Files:**
- Modify: `scripts/release.sh` — add `phase3_notes` and dry-run gate
- Create: `tests/integration/dry_run.bats`

- [ ] **Step 1: Write failing tests for Phase 3 + dry-run**

Write `tests/integration/dry_run.bats`:
```bash
#!/usr/bin/env bats

setup() {
  TMPDIR_ROOT="$(mktemp -d)"
  REPO_DIR="$TMPDIR_ROOT/repo"
  ORIGIN_DIR="$TMPDIR_ROOT/origin.git"
  STUB_LOG="$TMPDIR_ROOT/stub.log"
  : > "$STUB_LOG"

  cp -R "$BATS_TEST_DIRNAME/../fixtures/sample_project/." "$REPO_DIR"
  ln -s "$BATS_TEST_DIRNAME/../../scripts" "$REPO_DIR/scripts"

  cd "$REPO_DIR"
  git init --quiet --initial-branch=master
  git -c user.email=t@t -c user.name=t add -A
  git -c user.email=t@t -c user.name=t commit --quiet -m "init"
  git init --bare --quiet "$ORIGIN_DIR"
  git remote add origin "$ORIGIN_DIR"
  git push --quiet origin master

  export REAL_GIT="$(command -v git)"
  export STUB_LOG
  export PATH="$BATS_TEST_DIRNAME/../stubs/bin:$PATH"
  export RELEASE_REPO="bidmad/Bidmad-Unity"
  export UNITY_BIN="$BATS_TEST_DIRNAME/../stubs/bin/Unity"
}

teardown() {
  rm -rf "$TMPDIR_ROOT"
}

@test "dry-run writes notes file with extracted CHANGELOG body" {
  cd "$REPO_DIR"
  run ./scripts/release.sh 3.9.3 --dry-run
  [ "$status" -eq 0 ]
  [ -f "$REPO_DIR/releases/notes_3.9.3.md" ]
  grep -q "Test release notes line one" "$REPO_DIR/releases/notes_3.9.3.md"
  ! grep -q "^# Version " "$REPO_DIR/releases/notes_3.9.3.md"
}

@test "dry-run never invokes git push or gh release create" {
  cd "$REPO_DIR"
  ./scripts/release.sh 3.9.3 --dry-run
  ! grep -q "^git push" "$STUB_LOG"
  ! grep -q "^gh release create" "$STUB_LOG"
}
```

- [ ] **Step 2: Run, verify failures**

Run: `bats tests/integration/dry_run.bats`
Expected: both tests fail (notes file not produced yet).

- [ ] **Step 3: Implement `phase3_notes` and the `--dry-run` gate**

Add the following function to `scripts/release.sh`, below `phase2_export`:
```bash
phase3_notes() {
  cd "$REPO_ROOT"
  mkdir -p "$RELEASES_DIR"
  local notes_path="$RELEASES_DIR/notes_$VERSION.md"
  extract_notes "$CHANGELOG" "$VERSION" > "$notes_path"
  echo "Notes written: $notes_path" >&2
}
```

Replace `main()` with:
```bash
main() {
  parse_args "$@"
  phase1_preflight
  phase2_export
  phase3_notes
  if [[ "$DRY_RUN" -eq 1 ]]; then
    echo "Dry run complete. Artifacts under $RELEASES_DIR/" >&2
    return 0
  fi
  echo "Phases 4-5 not yet implemented" >&2
  return 0
}
```

- [ ] **Step 4: Run, verify success**

Run: `bats tests/integration/dry_run.bats`
Expected: 2 tests pass.

Run: `make test`
Expected: 25 tests pass.

- [ ] **Step 5: Commit**

```bash
git add scripts/release.sh tests/integration/dry_run.bats
git commit -m "Implement Phase 3 (notes extraction) and --dry-run gate"
```

---

## Task 10: Phase 4 + 5 (push, tag, gh release create) + tests

**Files:**
- Modify: `scripts/release.sh` — add `phase4_push_tag`, `phase5_release`
- Create: `tests/integration/full_flow.bats`

- [ ] **Step 1: Write failing tests for the full flow**

Write `tests/integration/full_flow.bats`:
```bash
#!/usr/bin/env bats

setup() {
  TMPDIR_ROOT="$(mktemp -d)"
  REPO_DIR="$TMPDIR_ROOT/repo"
  ORIGIN_DIR="$TMPDIR_ROOT/origin.git"
  STUB_LOG="$TMPDIR_ROOT/stub.log"
  : > "$STUB_LOG"

  cp -R "$BATS_TEST_DIRNAME/../fixtures/sample_project/." "$REPO_DIR"
  ln -s "$BATS_TEST_DIRNAME/../../scripts" "$REPO_DIR/scripts"

  cd "$REPO_DIR"
  git init --quiet --initial-branch=master
  git -c user.email=t@t -c user.name=t add -A
  git -c user.email=t@t -c user.name=t commit --quiet -m "init"
  git init --bare --quiet "$ORIGIN_DIR"
  git remote add origin "$ORIGIN_DIR"
  git push --quiet origin master

  export REAL_GIT="$(command -v git)"
  export STUB_LOG
  export PATH="$BATS_TEST_DIRNAME/../stubs/bin:$PATH"
  export RELEASE_REPO="bidmad/Bidmad-Unity"
  export UNITY_BIN="$BATS_TEST_DIRNAME/../stubs/bin/Unity"
}

teardown() {
  rm -rf "$TMPDIR_ROOT"
}

@test "full flow on master: pushes branch, tag, and creates gh release" {
  cd "$REPO_DIR"
  run ./scripts/release.sh 3.9.3
  [ "$status" -eq 0 ]
  grep -qE "^git push origin refs/heads/master" "$STUB_LOG"
  grep -qE "^git push origin refs/tags/3\.9\.3" "$STUB_LOG"
  grep -qE "^gh release create 3.9.3" "$STUB_LOG"
  grep -qE "BidmadUnityPlugin 3.9.3 Update" "$STUB_LOG"
  # Tag was created locally
  git rev-parse --verify refs/tags/3.9.3 >/dev/null
}

@test "full flow honors RELEASE_REPO override" {
  cd "$REPO_DIR"
  export RELEASE_REPO="sandbox/test-repo"
  run ./scripts/release.sh 3.9.3
  [ "$status" -eq 0 ]
  grep -qE "^gh release create 3.9.3 --repo sandbox/test-repo" "$STUB_LOG"
}

@test "full flow warns when off master but still proceeds" {
  cd "$REPO_DIR"
  git checkout -q -b feature/foo
  run ./scripts/release.sh 3.9.3
  [ "$status" -eq 0 ]
  [[ "${stderr:-}${output:-}" == *"Warning"* || "$output" == *"not on master"* ]]
  grep -qE "^git push origin refs/heads/feature/foo" "$STUB_LOG"
}

@test "full flow stops if git push fails" {
  cd "$REPO_DIR"
  export STUB_GIT_PUSH_EXIT=1
  run ./scripts/release.sh 3.9.3
  [ "$status" -ne 0 ]
  ! grep -q "^gh release create" "$STUB_LOG"
}
```

- [ ] **Step 2: Run, verify failures**

Run: `bats tests/integration/full_flow.bats`
Expected: 4 tests fail.

- [ ] **Step 3: Implement Phase 4 + 5**

Add to `scripts/release.sh`, below `phase3_notes`:
```bash
phase4_push_tag() {
  cd "$REPO_ROOT"
  local branch
  branch="$(git rev-parse --abbrev-ref HEAD)"
  git push origin "refs/heads/$branch:refs/heads/$branch"
  git tag "$VERSION"
  git push origin "refs/tags/$VERSION:refs/tags/$VERSION"
  echo "Pushed branch $branch and tag $VERSION" >&2
}

phase5_release() {
  cd "$REPO_ROOT"
  local pkg="$RELEASES_DIR/BidmadUnityPlugin_$VERSION.unitypackage"
  local notes="$RELEASES_DIR/notes_$VERSION.md"
  gh release create "$VERSION" \
    --repo "$RELEASE_REPO" \
    --title "BidmadUnityPlugin $VERSION Update" \
    --notes-file "$notes" \
    "$pkg"
  echo "Release $VERSION created on $RELEASE_REPO" >&2
}
```

Replace `main()`:
```bash
main() {
  parse_args "$@"
  phase1_preflight
  phase2_export
  phase3_notes
  if [[ "$DRY_RUN" -eq 1 ]]; then
    echo "Dry run complete. Artifacts under $RELEASES_DIR/" >&2
    return 0
  fi
  phase4_push_tag
  phase5_release
  echo "Release $VERSION complete." >&2
}
```

- [ ] **Step 4: Run tests, verify success**

Run: `bats tests/integration/full_flow.bats`
Expected: 4 tests pass.

Run: `make test`
Expected: 29 tests pass (13 unit + 16 integration).

- [ ] **Step 5: Commit**

```bash
git add scripts/release.sh tests/integration/full_flow.bats
git commit -m "Implement Phase 4-5 (push, tag, gh release) + full-flow tests"
```

---

## Task 11: `expected_paths_3.9.x.txt` fixture + README docs

**Files:**
- Create: `tests/fixtures/expected_paths_3.9.x.txt`
- Modify: `README.md` (add a "Releasing" section)
- Modify: `README-EN.md` (add a "Releasing" section)

- [ ] **Step 1: Create the expected-paths fixture**

Write `tests/fixtures/expected_paths_3.9.x.txt`:
```
Assets/Bidmad
Assets/Bidmad/Editor
Assets/Bidmad/Editor/BidmadPostProcessBuild.cs
Assets/Bidmad/Editor/OBHDependencies.xml
Assets/Bidmad/Editor/Unity.iOS.Extensions.Xcode.dll
Assets/Bidmad/Scripts
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
Assets/Plugins
Assets/Plugins/iOS
Assets/Plugins/iOS/Bidmad
Assets/Plugins/iOS/Bidmad/OpenBiddingHelperUnityBridge.h
Assets/Plugins/iOS/Bidmad/OpenBiddingHelperUnityBridge.mm
Assets/Plugins/iOS/Bidmad/SwiftEnabler.swift
```

Verify sorted: `LC_ALL=C sort -c tests/fixtures/expected_paths_3.9.x.txt && echo "sorted OK"`
Expected: prints `sorted OK`. If not, run `LC_ALL=C sort -o tests/fixtures/expected_paths_3.9.x.txt tests/fixtures/expected_paths_3.9.x.txt`.

- [ ] **Step 2: Add a "Releasing" section to `README.md`**

Append to `README.md` (Korean repo readme):
```markdown

## Releasing a new version

릴리즈는 `scripts/release.sh`로 자동화되어 있습니다.

1. `CHANGELOG.md` 상단에 새 `# Version X.Y.Z` 섹션을 추가하고 커밋합니다.
2. 워킹 트리가 깨끗하고 `master` 브랜치에 있는지 확인합니다.
3. 다음을 실행합니다:
   ```bash
   ./scripts/release.sh            # CHANGELOG 최상단 버전 사용
   ./scripts/release.sh 3.9.3      # 명시적 버전
   ./scripts/release.sh --dry-run  # 푸시 없이 unitypackage만 생성
   ```

테스트:
```bash
make test        # bats 유닛 + 인테그레이션
make test-build  # 실제 Unity로 --dry-run smoke test
```

Unity 2022.3.62f3과 `gh` CLI가 설치/인증되어 있어야 합니다.
`bats-core`는 `brew install bats-core`로 설치하세요.
```

- [ ] **Step 3: Add a "Releasing" section to `README-EN.md`**

Append to `README-EN.md`:
```markdown

## Releasing a new version

Releases are automated via `scripts/release.sh`.

1. Add a new `# Version X.Y.Z` section at the top of `CHANGELOG.md` and commit.
2. Confirm a clean working tree on `master`.
3. Run:
   ```bash
   ./scripts/release.sh            # auto-detects version from CHANGELOG
   ./scripts/release.sh 3.9.3      # explicit version
   ./scripts/release.sh --dry-run  # build .unitypackage without pushing
   ```

Test suite:
```bash
make test        # bats unit + integration
make test-build  # real-Unity --dry-run smoke test
```

Requirements: Unity 2022.3.62f3, `gh` CLI authenticated, `bats-core`
(`brew install bats-core`).
```

- [ ] **Step 4: Commit**

```bash
git add tests/fixtures/expected_paths_3.9.x.txt README.md README-EN.md
git commit -m "Add expected paths fixture and Releasing docs in both READMEs"
```

---

## Task 12: Tier-3 manual dry-run smoke test with real Unity

This task is **manual** — it requires running real Unity locally. The
result is a `.unitypackage` whose contents are diff'd against the
fixture from Task 11.

**Files:**
- Create (Unity auto-generates): `BidmadPluginSample/Assets/Bidmad/Editor/BidmadPackageExporter.cs.meta`

- [ ] **Step 1: Prepare a temporary CHANGELOG entry**

The script's preflight requires a CHANGELOG section that matches the
version under test. Pick a version that does not already exist as a
release and that won't be used for a real release.

```bash
# Make a backup so we can revert
cp CHANGELOG.md /tmp/CHANGELOG.md.bak
```

Edit `CHANGELOG.md` and insert at the top:
```
# Version 999.0.0
#### Changes
- Dry-run smoke test (not a real release)
```

- [ ] **Step 2: Commit the temporary entry locally (preflight requires a clean tree)**

```bash
git add CHANGELOG.md
git commit -m "TEMP: dry-run smoke test entry — do not push"
```

- [ ] **Step 3: Run the dry-run**

```bash
make test-build
# equivalently: ./scripts/release.sh 999.0.0 --dry-run
```

Expected: exit code 0; messages "Preflight OK", "Export OK", "Notes
written", and "Dry run complete". Takes 2-4 minutes.

If Unity errors about `BidmadPackageExporter.cs` being missing or not
compiling, the `.cs.meta` may not have been generated yet — open the
project once in the Unity editor to force meta generation, then re-run.

- [ ] **Step 4: Verify the output structure matches the fixture**

```bash
tmp=$(mktemp -d)
tar -xzf releases/BidmadUnityPlugin_999.0.0.unitypackage -C "$tmp"
for f in "$tmp"/*/pathname; do head -n1 "$f"; done | LC_ALL=C sort > "$tmp/actual.txt"
diff -u tests/fixtures/expected_paths_3.9.x.txt "$tmp/actual.txt"
echo "Exit: $?"
```

Expected: `Exit: 0` (no diff). If there's a diff, decide whether the
fixture or the export folder list is wrong, fix in a follow-up commit,
and re-run.

- [ ] **Step 5: Verify the customer-side directory layout (spot check)**

```bash
ls "$tmp"/*/pathname | wc -l
```

Expected: matches the line count of `expected_paths_3.9.x.txt` (33 entries as of this writing).

- [ ] **Step 6: Revert the temporary CHANGELOG entry**

```bash
# Undo the temp commit
git reset --hard HEAD~1
# Sanity check
head -3 CHANGELOG.md
```

Expected: shows the real top entry (e.g., `# Version 3.9.2`), not the
temporary one.

- [ ] **Step 7: Commit the `.cs.meta` Unity generated**

Unity will have created `BidmadPluginSample/Assets/Bidmad/Editor/BidmadPackageExporter.cs.meta` during the dry-run.

```bash
git status
# Should show the .cs.meta as new (untracked)
git add BidmadPluginSample/Assets/Bidmad/Editor/BidmadPackageExporter.cs.meta
git commit -m "Add Unity-generated meta for BidmadPackageExporter"
```

- [ ] **Step 8: Clean up dry-run artifacts**

```bash
rm -rf releases/
```

(They're gitignored, but tidy is nice.)

- [ ] **Step 9: Final regression run**

```bash
make test
```

Expected: 29 tests pass.

---

## Done

After Task 12, the repo is ready to cut a real release. Typical usage:

```bash
# Edit CHANGELOG.md to add `# Version X.Y.Z` at the top, commit.
./scripts/release.sh
# Browse the URL printed at the end.
```

If something goes wrong mid-flow (e.g., `gh release create` fails after
the tag is pushed), the `.unitypackage` and notes file are already in
`releases/`. You can finish manually with:

```bash
gh release create <version> \
  --repo bidmad/Bidmad-Unity \
  --title "BidmadUnityPlugin <version> Update" \
  --notes-file releases/notes_<version>.md \
  releases/BidmadUnityPlugin_<version>.unitypackage
```
