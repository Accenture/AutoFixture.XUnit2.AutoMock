---
id: TASK-3
title: Enforce Conventional Commits
status: Done
assignee:
  - cursor
  - piotrzajac
created_date: '2026-04-07 20:55'
updated_date: '2026-04-09 09:57'
labels:
  - ci-cd
  - dx
dependencies:
  - TASK-2
priority: medium
---

## Description

<!-- SECTION:DESCRIPTION:BEGIN -->
Add tooling to enforce the Conventional Commits specification at commit time. The contributor should evaluate available tools (e.g. husky + commitlint, a pre-commit framework hook, or similar) and choose the best fit. The enforcement must run as a local git hook so that invalid commit messages are rejected before the commit is recorded.
<!-- SECTION:DESCRIPTION:END -->

## Acceptance Criteria
<!-- AC:BEGIN -->
- [x] #1 A git commit-msg hook is in place that validates the message against Conventional Commits format
- [x] #2 The commit is prevented when the message is non-conforming — not just warned
- [x] #3 Setup instructions are added to CONTRIBUTING.md so contributors activate the hooks after cloning
<!-- AC:END -->

## Implementation Plan

<!-- SECTION:PLAN:BEGIN -->
Implemented Conventional Commit enforcement using Husky.NET + CommitLint.Net with repository-local .NET tools. Final scope delivered: (1) created/updated local tool manifest (`dotnet-tools.json`) with Husky, CommitLint.Net, and dotnet-stryker for aligned tool bootstrap via `dotnet tool restore`; (2) installed Husky hooks and added `.husky/commit-msg` + `.husky/task-runner.json` task that runs `dotnet commit-lint --commit-file ${args} --commit-message-config-file commit-message-config.json`; (3) added `commit-message-config.json` rules for Conventional Commits and allowed types (including `ci`); (4) added CI mirror validation workflow `.github/workflows/commit-message.yml` that validates latest commit message using the same config; (5) updated `CONTRIBUTING.md` Getting Started to restore all local tools and install Husky hooks, added references in commit/mutation sections, and switched mutation command to local-tool invocation (`dotnet dotnet-stryker ...`); (6) enforcement remains strict in local hook and CI workflow with no environment-variable bypass path.
<!-- SECTION:PLAN:END -->

## Implementation Notes

<!-- SECTION:NOTES:BEGIN -->
Implemented Husky.NET + CommitLint.Net enforcement with repository-local tooling in `dotnet-tools.json` (Husky, CommitLint.Net, dotnet-stryker) and Conventional Commits rules in `commit-message-config.json`.

Added `.husky/commit-msg` and `.husky/task-runner.json` commit-msg task to run `dotnet commit-lint --commit-file ${args} --commit-message-config-file commit-message-config.json`, blocking invalid commit messages.

Added CI mirror validation in `.github/workflows/commit-message.yml` to lint the latest commit message using the same config.

Updated `CONTRIBUTING.md` to use a single Getting Started bootstrap (`dotnet tool restore` + `dotnet husky install`), added section references for tool usage, and aligned mutation testing to local tool invocation (`dotnet dotnet-stryker -f ../stryker-config.yml`).

Validated behavior locally: valid messages pass and invalid messages fail with non-zero exit.
<!-- SECTION:NOTES:END -->
