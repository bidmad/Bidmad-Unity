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
