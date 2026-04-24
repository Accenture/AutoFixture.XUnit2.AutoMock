---
id: decision-4
title: Support multiple mocking frameworks as separate modules
date: '2017-11-07'
status: accepted
---
## Context

AutoFixture supports multiple mock backends (Moq, NSubstitute, FakeItEasy). Teams using only one mocking framework should not be forced to take a transitive dependency on the others, and each framework should be independently versionable.

## Decision

Build a separate NuGet package for each mocking framework - `AutoMoq`, `AutoFakeItEasy`, `AutoNSubstitute` - each depending on Core. Every module exposes equivalent three attribute shapes (`[AutoMockData]`, `[InlineAutoMockData]`, `[MemberAutoMockData]`) within its own module namespace, and overrides only `Customize(IFixture)` to apply the framework-specific `ICustomization`.

## Consequences

- Users install only the package matching their mocking framework of choice.
- Each module is independently publishable and versionable.
- Three separate project pairs must be maintained, but the surface area per project is minimal (one `Customize` override each).
