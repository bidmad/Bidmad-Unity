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
