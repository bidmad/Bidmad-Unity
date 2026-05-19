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
