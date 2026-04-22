---
id: decision-19
title: Add CustomizeWith parameter attributes
date: '2019-08-24'
status: accepted
---
## Context

Users occasionally need to apply a full `ICustomization` to a single parameter without
affecting the fixture configuration for other parameters. No mechanism existed for
per-parameter customization injection beyond the built-in `[Frozen]` attribute.

## Decision

Add two parameter-level attributes to Core:

- `[CustomizeWith(typeof(T))]` — activates the specified `ICustomization` for a parameter
- `[CustomizeWith<T>]` — generic variant; equivalent in behavior, reduces casting boilerplate

Both are implemented via `IParameterCustomizationSource` and apply the customization to the
fixture immediately before the parameter is resolved.

## Consequences

- Fine-grained customization without polluting the fixture globally for all parameters.
- The generic variant is the preferred form; the `typeof` variant exists for contexts where generic attributes are unavailable.
- Combinable with `[Frozen]`, and the other data-narrowing parameter attributes in the same parameter list.
