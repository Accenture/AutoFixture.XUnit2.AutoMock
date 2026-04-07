---
id: TASK-1
title: Add PR template and issue templates
status: Done
assignee:
  - Claude
  - piotrzajac
created_date: '2026-04-07 20:49'
updated_date: '2026-04-07 22:06'
labels:
  - doc
  - dx
dependencies: []
priority: medium
---

## Description

<!-- SECTION:DESCRIPTION:BEGIN -->
Create GitHub PR and issue templates to guide contributors:

- .github/pull_request_template.md — checklist covering tests, build, conventional commit format, and docs
- .github/ISSUE_TEMPLATE/1-bug-report.yml — structured bug report form (GitHub issue form, not markdown)
- .github/ISSUE_TEMPLATE/2-feature-request.yml — structured feature request form (GitHub issue form, not markdown)
- .github/ISSUE_TEMPLATE/config.yml — disables blank issues (`blank_issues_enabled: false`)
<!-- SECTION:DESCRIPTION:END -->

## Acceptance Criteria
<!-- AC:BEGIN -->
- [x] #1 .github/pull_request_template.md exists with a contributor checklist
- [x] #2 .github/ISSUE_TEMPLATE/1-bug-report.yml exists as a GitHub issue form
- [x] #3 .github/ISSUE_TEMPLATE/2-feature-request.yml exists as a GitHub issue form
- [x] #4 .github/ISSUE_TEMPLATE/config.yml exists with blank_issues_enabled: false
<!-- AC:END -->
