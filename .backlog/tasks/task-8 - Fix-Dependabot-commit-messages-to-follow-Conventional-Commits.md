---
id: TASK-8
title: Fix Dependabot commit messages to follow Conventional Commits
status: Done
assignee:
  - piotrzajac
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
Added `commit-message` configuration with `prefix: "chore"` and `include: "scope"` to both the `nuget` and `github-actions` entries in `.github/dependabot.yml`. Dependabot appends the package-manager name as the scope automatically, producing messages like `chore(nuget): bump ...` and `chore(github-actions): bump ...`.
<!-- SECTION:PLAN:END -->

## Implementation Notes

<!-- SECTION:NOTES:BEGIN -->
Dependabot `commit-message.include: "scope"` uses the package-ecosystem name as the scope (e.g. `nuget`, `github-actions`), which satisfies the Conventional Commits scope requirement without any custom scripting.
<!-- SECTION:NOTES:END -->
