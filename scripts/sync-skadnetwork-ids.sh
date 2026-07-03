#!/usr/bin/env bash
#
# sync-skadnetwork-ids.sh
#
# Compares the SKAdNetworkIdentifier list embedded in BidmadPostProcessBuild.cs
# against the golden-standard list published in the Bidmad-iOS wiki:
#   https://github.com/bidmad/Bidmad-iOS/wiki/Preparing-for-iOS-14[ENG]#2-setting-skadnetwork
#
# Usage:
#   scripts/sync-skadnetwork-ids.sh              # report missing / extra (diff mode)
#   scripts/sync-skadnetwork-ids.sh --emit-cs    # print the C# AddDict lines for the golden list
#   scripts/sync-skadnetwork-ids.sh --wiki-file <path>   # use a local wiki .md instead of downloading
#
# Exit status (diff mode): 0 if every golden id is present in the .cs, 1 otherwise.
#
set -euo pipefail

WIKI_URL="https://raw.githubusercontent.com/wiki/bidmad/Bidmad-iOS/Preparing-for-iOS-14%5BENG%5D.md"
SCRIPT_DIR="$(cd "$(dirname "${BASH_SOURCE[0]}")" && pwd)"
REPO_ROOT="$(cd "$SCRIPT_DIR/.." && pwd)"
CS_FILE="${CS_FILE:-$REPO_ROOT/BidmadPluginSample/Assets/Bidmad/Editor/BidmadPostProcessBuild.cs}"
INDENT="                "   # 16 spaces — matches the existing AddDict lines in the .cs

EMIT=0
WIKI_FILE=""

usage() { sed -n '2,20p' "$0" | sed 's/^# \{0,1\}//'; }

while [[ $# -gt 0 ]]; do
  case "$1" in
    --emit-cs) EMIT=1; shift ;;
    --wiki-file) WIKI_FILE="${2:-}"; shift 2 ;;
    --help|-h) usage; exit 0 ;;
    *) echo "Unknown argument: $1" >&2; usage; exit 2 ;;
  esac
done

# --- Obtain the wiki markdown -------------------------------------------------
cleanup=""
if [[ -n "$WIKI_FILE" ]]; then
  [[ -f "$WIKI_FILE" ]] || { echo "Error: wiki file not found: $WIKI_FILE" >&2; exit 1; }
  src="$WIKI_FILE"
else
  src="$(mktemp)"; cleanup="$src"
  if ! curl -fsSL "$WIKI_URL" -o "$src"; then
    echo "Error: failed to download wiki from $WIKI_URL" >&2
    rm -f "$cleanup"; exit 1
  fi
fi
trap '[[ -n "$cleanup" ]] && rm -f "$cleanup"' EXIT

# --- Extract identifiers ------------------------------------------------------
# Golden: the values inside <string>...</string>, preserving wiki order, de-duplicated
# keeping first occurrence. Covers BOTH attribution domains the wiki lists in the same
# SKAdNetworkItems array: *.skadnetwork (SKAdNetwork) and *.adattributionkit
# (AdAttributionKit). Apple treats ids case-insensitively, but we preserve the exact
# spelling the wiki publishes.
golden="$(grep -oE '<string>[A-Za-z0-9]+\.(skadnetwork|adattributionkit)</string>' "$src" \
          | sed -E 's#</?string>##g' | awk '!seen[$0]++')"

if [[ -z "$golden" ]]; then
  echo "Error: no <string>*.skadnetwork</string> entries found in wiki source" >&2
  exit 1
fi

# --emit-cs: print ready-to-paste C# lines for the full golden list, in wiki order.
if [[ "$EMIT" -eq 1 ]]; then
  while IFS= read -r id; do
    printf '%sskAdNetworkArray.AddDict().SetString("SKAdNetworkIdentifier", "%s");\n' "$INDENT" "$id"
  done <<< "$golden"
  exit 0
fi

# Current: identifiers already present in the .cs.
[[ -f "$CS_FILE" ]] || { echo "Error: .cs not found: $CS_FILE" >&2; exit 1; }
current="$(grep -oE '[A-Za-z0-9]+\.(skadnetwork|adattributionkit)' "$CS_FILE" | awk '!seen[$0]++')"

# --- Compare ------------------------------------------------------------------
missing="$(comm -23 <(printf '%s\n' "$golden" | sort) <(printf '%s\n' "$current" | sort))"
extra="$(comm -13 <(printf '%s\n' "$golden" | sort) <(printf '%s\n' "$current" | sort))"

golden_n="$(printf '%s\n' "$golden" | grep -c . || true)"
current_n="$(printf '%s\n' "$current" | grep -c . || true)"
missing_n="$(printf '%s' "$missing" | grep -c . || true)"
extra_n="$(printf '%s' "$extra" | grep -c . || true)"

echo "golden (wiki):   $golden_n"
echo "current (.cs):   $current_n"
echo
echo "MISSING from .cs ($missing_n):"
[[ -n "$missing" ]] && printf '  %s\n' $missing || echo "  (none)"
echo
echo "EXTRA in .cs, not in wiki ($extra_n):"
[[ -n "$extra" ]] && printf '  %s\n' $extra || echo "  (none)"

# Fail if the .cs is missing any golden id (usable as a CI gate).
[[ "$missing_n" -eq 0 ]]
