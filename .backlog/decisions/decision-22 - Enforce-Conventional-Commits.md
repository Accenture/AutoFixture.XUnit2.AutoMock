---
id: decision-22
title: Enforce Conventional Commits
date: '2026-04-22'
status: accepted
---
## Context

Commit messages were inconsistently formatted across contributors.

## Decision

Enforce Conventional Commits format (`[type]([scope]): [description]`) on every commit.
Enforcement uses a Husky.NET `commit-msg` hook for local validation and CommitLint.Net
in CI to block merges that contain non-conforming commit messages.

## Consequences

- Consistent, machine-readable commit history across all contributors.
- Automated changelog generation becomes feasible as a follow-on step.
- Contributors receive immediate feedback at commit time rather than discovering the issue at CI.
