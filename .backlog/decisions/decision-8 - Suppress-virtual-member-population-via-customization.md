---
id: decision-8
title: Suppress virtual member population via customization
date: '2018-09-20'
status: accepted
---
## Context

When AutoFixture creates objects for classes that implement interfaces, the CLR marks the interface method implementations as `virtual sealed`. AutoFixture attempts to populate those virtual properties, which conflicts with mock proxy behavior: the proxy already owns those members, and AutoFixture's attempt to set them can throw or produce unexpected state.

## Decision

Introduce `IgnoreVirtualMembersSpecimenBuilder` (returning `OmitSpecimen` for virtual `PropertyInfo` requests) and expose it through `IgnoreVirtualMembersCustomization`. All three base attributes accept `IgnoreVirtualMembers = true` to activate this behavior per attribute instance. A `[IgnoreVirtualMembers]` parameter-level attribute applies it to a specific parameter.

## Consequences

- Mock proxies are not interfered with by AutoFixture's property-population pass.
- Opt-in per attribute - tests that genuinely need virtual properties populated are unaffected.
- Reduces the most common setup error for users working with interface-implementing types.
