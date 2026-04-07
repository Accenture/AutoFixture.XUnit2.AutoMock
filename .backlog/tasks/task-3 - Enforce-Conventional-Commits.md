---
id: TASK-3
title: Enforce Conventional Commits
status: To Do
assignee: []
created_date: '2026-04-07 20:55'
updated_date: '2026-04-07 21:00'
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
- [ ] #1 A git commit-msg hook is in place that validates the message against Conventional Commits format
- [ ] #2 The commit is prevented when the message is non-conforming — not just warned
- [ ] #3 Setup instructions are added to CONTRIBUTING.md so contributors activate the hooks after cloning
<!-- AC:END -->
