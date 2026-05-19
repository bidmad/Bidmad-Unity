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
