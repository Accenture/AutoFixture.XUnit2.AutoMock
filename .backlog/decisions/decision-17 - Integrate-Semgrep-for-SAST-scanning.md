---
id: decision-17
title: Integrate Semgrep for SAST scanning
date: '2023-12-20'
status: accepted
---
## Context

CodeQL provides GitHub-native SAST with strong C# support, but its rule coverage is focused
on common vulnerability classes. Semgrep offers complementary community-maintained rule sets
and custom pattern matching that can catch additional issues CodeQL does not cover.

## Decision

Add Semgrep as a parallel SAST scanning step in CI alongside CodeQL. Use the default
Semgrep ruleset for C# plus any community rulesets relevant to the project. Semgrep runs
on push and pull request to master.

## Consequences

- Broader SAST coverage through two independent tools with different rule philosophies.
- Some overlap with CodeQL is intentional — independent tools reaching the same conclusion increases confidence.
- Semgrep findings are reviewed in CI alongside CodeQL results; both must pass before merge.
