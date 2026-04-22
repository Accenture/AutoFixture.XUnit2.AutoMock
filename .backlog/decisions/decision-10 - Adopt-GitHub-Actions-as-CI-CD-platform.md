---
id: decision-10
title: Adopt GitHub Actions as CI/CD platform
date: '2023-03-24'
status: accepted
---
## Context

The project previously used Travis CI. Travis CI's free tier for open-source projects was reduced, and it lacked deep integration with GitHub security features (CodeQL, Dependabot, secret scanning). GitHub Actions offered native integration, a richer ecosystem of community actions, and free minutes for public repositories.

## Decision

Migrate all CI/CD pipelines from Travis CI to GitHub Actions. Organize workflows into reusable modules: `init.yml` (GitVersion, module detection), `build-test-pack.yml`, `publish.yml`, `tag.yml`. Run all jobs on `windows-latest` to support `net472`/`net48` test slices. Security and quality tools could run as parallel jobs in the same pipeline.

## Consequences

- No external CI service dependency; pipeline definition lives in the same repository.
- Native access to GitHub security tab, Dependabot alerts, and environment secrets.
- `windows-latest` runner is required for every job due to `net472`/`net48` constraints, which limits Linux-only optimizations.
