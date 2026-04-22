---
id: decision-1
title: Decouple xUnit from AutoFixture via provider pattern
date: '2017-01-22'
status: accepted
---
## Context

The library originally derived directly from AutoFixture's `AutoDataAttribute`. This created
tight coupling between the xUnit integration and AutoFixture internals, making it difficult
to inject custom behavior, intercept the data-generation pipeline, or add parameter-level
customization without forking AutoFixture itself.

## Decision

Stop inheriting from AutoFixture attributes. Derive from xUnit's `DataAttribute` instead and
drive fixture customization through a dedicated `IAutoFixtureAttributeProvider` abstraction.
The provider receives the configured `IFixture` and returns a `DataAttribute` whose
`GetData()` resolves test parameters. This separates xUnit's data-supply contract from
AutoFixture's specimen-creation pipeline.

## Consequences

- Clear boundary between the xUnit layer and AutoFixture: each can evolve independently.
- The provider pattern enables per-parameter customization via `IParameterCustomizationSource` attributes applied later in the pipeline.
- All three mock modules (AutoMoq, AutoFakeItEasy, AutoNSubstitute) override only `Customize(IFixture)` — the rest of the pipeline is inherited from Core.
