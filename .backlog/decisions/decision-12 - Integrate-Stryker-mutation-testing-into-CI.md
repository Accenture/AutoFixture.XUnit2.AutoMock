---
id: decision-12
title: Integrate Stryker mutation testing into CI
date: '2023-09-30'
status: accepted
---
## Context

Traditional code coverage metrics verify that tests execute code paths, not that they detect
bugs. A test suite with 100% coverage can still pass while missing every meaningful
regression. The project needed a higher-confidence quality gate.

## Decision

Integrate Stryker.NET mutation testing into the CI pipeline as a separate, slow-running
stage. Configuration lives in `stryker-config.yml` at the repository root; the tool is
invoked from the `src/` directory. Stryker runs are expected before raising a PR rather
than on every commit.

## Consequences

- Tests are verified to actually kill mutations, raising confidence beyond coverage alone.
- Slow step kept out of the fast feedback loop; developers opt in before submitting a PR.
- Surviving mutants become explicit action items, not silent gaps.
