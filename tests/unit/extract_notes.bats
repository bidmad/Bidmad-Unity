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
