#!/usr/bin/env bats

setup() {
  source "$BATS_TEST_DIRNAME/../../scripts/lib/release-functions.sh"
  FIXTURES="$BATS_TEST_DIRNAME/../fixtures"
}

@test "read_unity_version returns m_EditorVersion value" {
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
