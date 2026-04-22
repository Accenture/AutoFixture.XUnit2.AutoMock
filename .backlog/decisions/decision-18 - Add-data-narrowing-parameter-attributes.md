---
id: decision-18
title: Add data-narrowing parameter attributes
date: '2023-12-05'
status: accepted
---
## Context

AutoFixture generates arbitrary values for parameters. Tests for boundary conditions, enum
subsets, or constrained numeric domains needed values from a restricted range, requiring
manual fixture setup that negated the boilerplate reduction the library provides.

## Decision

Add four parameter-level attributes to Core, all implemented via `IParameterCustomizationSource`:

- `[Except(v1, v2)]` — generate values excluding the specified set
- `[PickFromValues(v1, v2)]` — pick randomly from a fixed set
- `[PickFromRange(min, max)]` — generate within a numeric range
- `[PickNegative]` — generate only negative numeric values

All four are backed by new specimen builders (`RandomExceptValuesGenerator`,
`RandomFixedValuesGenerator`) and request types (`ExceptValuesRequest`, `FixedValuesRequest`).

## Consequences

- Constrained test data without any manual fixture configuration.
- All three mock modules benefit automatically via Core.
- Attributes are combinable with `[Frozen]` in the same parameter list.
