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

@test "full flow on master: pushes branch, tag, and creates gh release" {
  cd "$REPO_DIR"
  run ./scripts/release.sh 3.9.3
  [ "$status" -eq 0 ]
  grep -qE "^git push origin refs/heads/master" "$STUB_LOG"
  grep -qE "^git push origin refs/tags/3\.9\.3" "$STUB_LOG"
  grep -qE "^gh release create 3.9.3" "$STUB_LOG"
  grep -qE "BidmadUnityPlugin 3.9.3 Update" "$STUB_LOG"
  git rev-parse --verify refs/tags/3.9.3 >/dev/null
}

@test "full flow honors RELEASE_REPO override" {
  cd "$REPO_DIR"
  export RELEASE_REPO="sandbox/test-repo"
  run ./scripts/release.sh 3.9.3
  [ "$status" -eq 0 ]
  grep -qE "^gh release create 3.9.3 --repo sandbox/test-repo" "$STUB_LOG"
}

@test "full flow warns when off master but still proceeds" {
  cd "$REPO_DIR"
  git checkout -q -b feature/foo
  run ./scripts/release.sh 3.9.3
  [ "$status" -eq 0 ]
  grep -qE "^git push origin refs/heads/feature/foo" "$STUB_LOG"
}

@test "full flow stops if git push fails" {
  cd "$REPO_DIR"
  export STUB_GIT_PUSH_EXIT=1
  run ./scripts/release.sh 3.9.3
  [ "$status" -ne 0 ]
  ! grep -q "^gh release create" "$STUB_LOG"
}
