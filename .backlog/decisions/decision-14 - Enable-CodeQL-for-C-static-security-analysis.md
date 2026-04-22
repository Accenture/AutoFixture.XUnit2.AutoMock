---
id: decision-14
title: Enable CodeQL for C# static security analysis
date: '2023-09-21'
status: accepted
---
## Context

Manual code review is insufficient for systematically catching security vulnerabilities. As a NuGet library consumed by other projects, a vulnerability in this library propagates to all consumers. An automated static analysis tool integrated with GitHub's security infrastructure would provide continuous protection at no operational cost for public repos.

## Decision

Enable GitHub CodeQL for C# as a required CI check. CodeQL runs on push and pull request to master, analyzing the full C# codebase for vulnerabilities. Results surface in the GitHub Security tab and can block merges when configured as a required status check.

## Consequences

- Continuous vulnerability scanning without additional tooling setup for contributors.
- Security findings are visible to maintainers in the GitHub Security tab.
