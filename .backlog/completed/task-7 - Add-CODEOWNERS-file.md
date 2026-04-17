---
id: TASK-7
title: Add CODEOWNERS file
status: Done
assignee:
  - claude
  - piotrzajac
created_date: '2026-04-07 20:58'
labels:
  - dx
dependencies: []
priority: low
---

## Description

<!-- SECTION:DESCRIPTION:BEGIN -->
Create a .github/CODEOWNERS file that maps paths to GitHub teams or users who are automatically requested as reviewers when those paths are changed in a PR. At minimum, the default owner should be set. This reduces review latency and ensures the right people are always in the loop.
<!-- SECTION:DESCRIPTION:END -->

## Acceptance Criteria
<!-- AC:BEGIN -->
- [ ] #1 .github/CODEOWNERS exists and is syntactically valid
- [ ] #2 A default owner (* pattern) is defined
- [ ] #3 Reviewers are automatically requested on PRs touching covered paths
<!-- AC:END -->
