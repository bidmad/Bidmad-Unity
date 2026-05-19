#!/usr/bin/env bash
# Pure functions for release.sh. Source this file; do not execute directly.

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

extract_notes() {
  echo "TODO: extract_notes not implemented" >&2
  return 1
}

read_unity_version() {
  echo "TODO: read_unity_version not implemented" >&2
  return 1
}
