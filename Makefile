.PHONY: test test-build test-all help

help:
	@echo "Targets:"
	@echo "  test        Run bats unit + integration tests"
	@echo "  test-build  Run real-Unity dry-run smoke test (slow)"
	@echo "  test-all    Run all of the above"

test:
	@./tests/run.sh

test-build:
	@./scripts/release.sh --dry-run

test-all: test test-build
