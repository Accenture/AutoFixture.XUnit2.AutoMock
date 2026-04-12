---
id: TASK-8
title: Fix Dependabot commit messages to follow Conventional Commits
status: Done
assignee:
  - piotrzajac
  - claude
created_date: '2026-04-12'
updated_date: '2026-04-12'
labels:
  - ci-cd
  - fix
dependencies:
  - TASK-3
priority: low
---

## Description

<!-- SECTION:DESCRIPTION:BEGIN -->
After TASK-3 enforced Conventional Commits for all contributors, the Dependabot configuration in `.github/dependabot.yml` was not updated to produce commit messages in the same format. Dependabot's auto-generated commit messages (e.g. `Bump Moq from 4.20.0 to 4.20.1`) would fail the commit-message CI validation workflow introduced in TASK-3.
<!-- SECTION:DESCRIPTION:END -->

## Acceptance Criteria
<!-- AC:BEGIN -->
- [x] #1 Dependabot NuGet update commits follow the format `chore(nuget): bump <package> from <old> to <new>`
- [x] #2 Dependabot GitHub Actions update commits follow the format `chore(github-actions): bump <action> from <old> to <new>`
<!-- AC:END -->

## Implementation Plan

<!-- SECTION:PLAN:BEGIN -->
Added `commit-message` configuration to both entries in `.github/dependabot.yml`:

- NuGet entry: `prefix: "chore(nuget)"`
- GitHub Actions entry: `prefix: "chore(github-actions)"`

`include: "scope"` was intentionally omitted — that option produces `deps`/`deps-dev` as the scope (dependency-type based), not the ecosystem name. Embedding the scope directly in `prefix` is the only way to produce ecosystem-specific scopes.
<!-- SECTION:PLAN:END -->

## Implementation Notes

<!-- SECTION:NOTES:BEGIN -->
`commit-message.include: "scope"` in Dependabot produces `chore(deps):` or `chore(deps-dev):` based on the dependency type — it does NOT use the ecosystem name as the scope. To get `chore(nuget):` and `chore(github-actions):`, the scope must be baked directly into the `prefix` value and `include: "scope"` must be omitted.
<!-- SECTION:NOTES:END -->

## Final Summary

<!-- SECTION:FINAL_SUMMARY:BEGIN -->
Updated `.github/dependabot.yml` to add `commit-message` configuration to both ecosystem entries:

- NuGet entry: `prefix: "chore(nuget)"` → produces `chore(nuget): bump <package> from X to Y`
- GitHub Actions entry: `prefix: "chore(github-actions)"` → produces `chore(github-actions): bump <action> from X to Y`

`include: "scope"` was intentionally omitted — that option produces `deps`/`deps-dev` as scope (dependency-type based), not the ecosystem name. Embedding the scope in `prefix` is the only way to get ecosystem-specific scopes that satisfy the commit-message CI workflow introduced in TASK-3.
<!-- SECTION:FINAL_SUMMARY:END -->
