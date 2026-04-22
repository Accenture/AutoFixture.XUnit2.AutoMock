---
id: decision-12
title: Use GitVersion in ContinuousDelivery mode
date: '2023-08-30'
status: accepted
---
## Context

Manual version management in `.csproj` files is error-prone: versions can be forgotten, duplicated across projects, or incremented inconsistently. The project needed an automated versioning strategy aligned with semantic versioning and the git workflow.

## Decision

Adopt GitVersion in `ContinuousDelivery` mode. Versions are computed entirely from git history: commits on `master` produce stable semantic versions; feature branches produce pre-release suffixes. No `<Version>` element is set manually in any `.csproj`. The `Directory.Build.props` default of `1.0.0.0` is a local fallback only, never used in CI.

## Consequences

- Version is always derivable from git history and requires no manual intervention.
- CI sets the version automatically before build and pack steps.
- Developers must follow branching conventions for GitVersion to compute the correct version; ad-hoc commits directly to master advance the stable version.
