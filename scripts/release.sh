#!/usr/bin/env bash
set -euo pipefail

SCRIPT_DIR="$(cd "$(dirname "${BASH_SOURCE[0]}")" && pwd)"
REPO_ROOT="$(cd "$SCRIPT_DIR/.." && pwd)"
# shellcheck source=lib/release-functions.sh
source "$SCRIPT_DIR/lib/release-functions.sh"

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

  if ! grep -qE "^# Version $VERSION$" "$CHANGELOG"; then
    echo "Error: CHANGELOG.md has no '# Version $VERSION' section" >&2
    return 1
  fi

  local detected
  detected="$(read_unity_version "$PROJECT_PATH")"
  if [[ "$detected" != "$UNITY_VERSION" ]]; then
    echo "Error: Unity version mismatch — project is $detected, expected $UNITY_VERSION" >&2
    return 1
  fi

  if [[ ! -x "$UNITY_BIN" ]] && ! command -v Unity >/dev/null 2>&1; then
    echo "Error: Unity binary not found at $UNITY_BIN" >&2
    return 1
  fi

  if ! gh auth status >/dev/null 2>&1; then
    echo "Error: gh CLI is not authenticated" >&2
    return 1
  fi

  if [[ -n "$(git status --porcelain)" ]]; then
    echo "Error: working tree is not clean; commit or stash before releasing" >&2
    return 1
  fi

  local branch
  branch="$(git rev-parse --abbrev-ref HEAD)"
  if [[ "$branch" != "master" ]]; then
    echo "Warning: not on master (current branch: $branch); proceeding anyway" >&2
  fi

  if git rev-parse --verify --quiet "refs/tags/$VERSION" >/dev/null 2>&1; then
    echo "Error: tag '$VERSION' already exists locally" >&2
    return 1
  fi

  if [[ -n "$(git ls-remote --tags origin "$VERSION" 2>/dev/null)" ]]; then
    echo "Error: tag '$VERSION' already exists on origin" >&2
    return 1
  fi

  if gh release view "$VERSION" --repo "$RELEASE_REPO" >/dev/null 2>&1; then
    echo "Error: GitHub release '$VERSION' already exists on $RELEASE_REPO" >&2
    return 1
  fi

  echo "Preflight OK: version=$VERSION, branch=$branch, repo=$RELEASE_REPO" >&2
}

phase2_export() {
  cd "$REPO_ROOT"
  mkdir -p "$RELEASES_DIR"
  local output_path="$RELEASES_DIR/BidmadUnityPlugin_$VERSION.unitypackage"
  rm -f "$output_path"

  export BIDMAD_PACKAGE_OUTPUT="$output_path"

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

phase3_notes() {
  cd "$REPO_ROOT"
  mkdir -p "$RELEASES_DIR"
  local notes_path="$RELEASES_DIR/notes_$VERSION.md"
  extract_notes "$CHANGELOG" "$VERSION" > "$notes_path"
  echo "Notes written: $notes_path" >&2
}

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

main "$@"
