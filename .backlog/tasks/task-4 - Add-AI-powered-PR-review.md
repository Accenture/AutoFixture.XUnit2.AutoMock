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

## Final Summary

<!-- SECTION:FINAL_SUMMARY:BEGIN -->
Selected and configured **CodeRabbit** — free for public OSS repos, no infrastructure to maintain.

Delivered `.coderabbit.yaml` at the repo root with:

- `auto_review.enabled: true` + `high_level_summary: true` — automatic PR summary comment on every PR (AC #1)
- `profile: "chill"` — non-blocking review style that complements rather than replaces human review
- `auto_apply_labels: true` — automatic PR labelling

AC #3 (OSS licensing) is satisfied — CodeRabbit is free for public repos.
AC #2 (project conventions awareness) was intentionally skipped by the team; the tool autodiscovers `AGENTS.md` but no explicit `reviews.instructions` were added.
<!-- SECTION:FINAL_SUMMARY:END -->
