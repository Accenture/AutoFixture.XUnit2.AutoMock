---
id: decision-22
title: Adopt CodeRabbit for AI-powered PR review
date: '2026-04-12'
status: accepted
---
## Context

Human code review is thorough for logic but inconsistent at enforcing coding conventions, catching subtle anti-patterns, and ensuring every changed file receives attention. A first-pass automated reviewer would raise quality before human reviewers engage.

## Decision

Integrate CodeRabbit as an automated AI reviewer on all pull requests. CodeRabbit posts inline review comments and a PR summary alongside human reviewers. It is configured to understand the project's conventions (BDD naming, AAA structure, attribute ordering rules).

## Consequences

- Every PR receives a first-pass review that catches convention violations and straightforward issues before human review.
- Human reviewers focus on higher-level concerns: design, correctness, and intent.
- AI review comments are advisory; maintainers retain final merge authority.
