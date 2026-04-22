---
id: decision-3
title: Extract shared logic into a Core assembly
date: '2017-10-28'
status: accepted
---
## Context

As AutoNSubstitute was being added alongside AutoMoq, shared logic (base attributes, customizations, provider interfaces, specimen builders) was being duplicated across both projects. A single source of truth was needed before the duplication compounded further.

## Decision

Create `Objectivity.AutoFixture.XUnit2.Core` as a shared assembly containing all common infrastructure: `AutoDataBaseAttribute`, `InlineAutoDataBaseAttribute`, `MemberAutoDataBaseAttribute`, `AutoDataCommonCustomization`, provider interfaces, specimen builders, and parameter attributes. Core is bundled into each mock module package but is not published as a standalone NuGet package.

## Consequences

- Hub-and-spoke architecture: each mock module overrides only `Customize(IFixture)`; all shared behavior lives in one place.
- Bugs and improvements to Core benefit all three modules simultaneously.
- Core cannot be installed by consumers directly — it is an implementation detail.
