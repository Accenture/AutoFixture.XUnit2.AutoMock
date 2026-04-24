---
id: decision-28
title: Seal attribute-derived types by default
date: '2026-04-24'
status: accepted
category: design
---
## Context

The CA1813 analyzer rule ("Avoid unsealed attributes") specifically targets `Attribute`-derived types (see decision-26). Unsealed attribute classes carry a measurable performance cost at runtime because the CLR must check for derived types on every attribute lookup, and they widen the public inheritance surface unnecessarily.

Non-attribute public types such as `ICustomization` and `ISpecimenBuilder` implementations are intentionally open. Consumers of this NuGet library may legitimately subclass or compose those types to extend AutoFixture's object-creation pipeline. Applying a blanket "seal everything" rule to those types would break a supported and documented extension scenario.

## Decision

All `Attribute`-derived types are `sealed` unless they are explicitly `abstract` (e.g., `AutoDataBaseAttribute`, `InlineAutoDataBaseAttribute`). This is exactly what the CA1813 rule enforces and is the narrowest scope that eliminates the runtime cost and inheritance-surface concerns for attributes.

Non-attribute public types (customizations, specimen builders, and other framework extension points) follow normal object-oriented design principles: they are open when consumer subclassing is a supported scenario and sealed only when there is a specific reason to prevent inheritance.

## Consequences

- The CA1813 rule is enforced by the analyzer on every build, so attribute sealing is never accidentally omitted.
- Non-attribute public types remain open for consumer extension without requiring suppression or justification.
- Any `[SuppressMessage("Performance", "CA1813", Justification = "...")]` in the codebase documents an intentional exception for an attribute type that must remain unsealed (e.g., an abstract base attribute), making such exceptions discoverable by search.
