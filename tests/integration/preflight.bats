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
  git -c user.email=test@test.local -c user.name=test add -A
  git -c user.email=test@test.local -c user.name=test commit --quiet -m "initial"
  git init --bare --quiet "$ORIGIN_DIR"
  git remote add origin "$ORIGIN_DIR"
  git push --quiet origin master

  export REAL_GIT="$(command -v git)"
  export STUB_LOG
  export PATH="$BATS_TEST_DIRNAME/../stubs/bin:$PATH"
  export RELEASE_REPO="bidmad/Bidmad-Unity"
}

teardown() {
  rm -rf "$TMPDIR_ROOT"
}

@test "preflight: refuses to run on dirty working tree" {
  cd "$REPO_DIR"
  echo "untracked" > newfile.txt
  run ./scripts/release.sh 3.9.3
  [ "$status" -ne 0 ]
}

@test "preflight: refuses when version is missing from CHANGELOG" {
  cd "$REPO_DIR"
  run ./scripts/release.sh 9.9.9
  [ "$status" -ne 0 ]
  ! grep -q "^Unity " "$STUB_LOG"
}

@test "preflight: refuses when Unity version mismatches" {
  cd "$REPO_DIR"
  cat > BidmadPluginSample/ProjectSettings/ProjectVersion.txt <<'EOF'
m_EditorVersion: 2021.3.0f1
m_EditorVersionWithRevision: 2021.3.0f1 (xxxxxxxxxxxx)
EOF
  git -c user.email=t@t -c user.name=t commit -am "bad unity version" --quiet
  run ./scripts/release.sh 3.9.3
  [ "$status" -ne 0 ]
  ! grep -q "^Unity " "$STUB_LOG"
}

@test "preflight: refuses when tag already exists locally" {
  cd "$REPO_DIR"
  git tag 3.9.3
  run ./scripts/release.sh 3.9.3
  [ "$status" -ne 0 ]
  ! grep -q "^Unity " "$STUB_LOG"
}

@test "preflight: refuses when tag exists on remote" {
  cd "$REPO_DIR"
  export STUB_GIT_LSREMOTE_OUTPUT="abcdef1234567890 refs/tags/3.9.3"
  run ./scripts/release.sh 3.9.3
  [ "$status" -ne 0 ]
  ! grep -q "^Unity " "$STUB_LOG"
}

@test "preflight: refuses when gh release already exists" {
  cd "$REPO_DIR"
  export STUB_GH_RELEASE_VIEW_EXIT=0
  run ./scripts/release.sh 3.9.3
  [ "$status" -ne 0 ]
  ! grep -q "^Unity " "$STUB_LOG"
}
