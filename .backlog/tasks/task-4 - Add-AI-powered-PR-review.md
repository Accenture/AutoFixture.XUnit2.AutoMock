---
id: TASK-4
title: Add AI-powered PR review
status: Done
assignee:
  - claude
  - piotrzajac
created_date: '2026-04-07 20:56'
updated_date: '2026-04-12 20:20'
labels:
  - ci-cd
  - dx
dependencies: []
priority: low
---

## Description

<!-- SECTION:DESCRIPTION:BEGIN -->
Integrate an AI-powered PR review tool (e.g. CodeRabbit, Reviewpad, or similar) that automatically reviews pull requests and posts inline comments. Should complement, not replace, human review. Contributor should evaluate tools against free-tier / OSS pricing suitability for a public Accenture-maintained repo and configure it to be aware of project conventions (AGENTS.md).
<!-- SECTION:DESCRIPTION:END -->

## Acceptance Criteria
<!-- AC:BEGIN -->
- [x] #1 AI reviewer posts a summary comment on every PR automatically
- [x] #2 Tool is configured to understand project conventions (reference AGENTS.md or equivalent)
- [x] #3 Licensing and cost are acceptable for a public OSS repo
<!-- AC:END -->

## Completion Notes

CodeRabbit was configured via `.coderabbit.yaml` at the repo root. It automatically posts PR summaries (`high_level_summary: true`, `auto_review.enabled: true`), satisfying AC #1. CodeRabbit is free for public OSS repos, satisfying AC #3. The tool natively supports AGENTS.md satisfying the AC #2.
