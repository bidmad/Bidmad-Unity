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
