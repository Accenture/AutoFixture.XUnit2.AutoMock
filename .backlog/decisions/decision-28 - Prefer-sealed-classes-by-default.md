---
id: decision-28
title: Prefer sealed classes by default
date: '2026-04-24'
status: accepted
category: design
---
## Context

C# classes are open for inheritance by default. In a NuGet library with a public API, any unsealed public class implicitly becomes part of the public surface: consumers can subclass it, and any future change to those classes - adding a member, changing a method's behavior, renaming a virtual member - becomes a breaking change.

The CA1813 analyzer rule ("Avoid unsealed attributes") was already enforced (see decision-26), flagging unsealed `Attribute`-derived types. However, there was no explicit documented policy covering non-attribute classes.

## Decision

All classes are `sealed` unless they are explicitly designed to be extended. A class is considered designed for extension when it meets at least one of these criteria:

- It is `abstract` (e.g., `AutoDataBaseAttribute`, `InlineAutoDataBaseAttribute`).
- It has `protected` members whose contracts are intentional extension points.

When a class legitimately needs to be unsealed (e.g., `CustomizeWithAttribute` and `CustomizeWithAttribute<T>` to allow consumers to derive custom parameter-level customizations), the CA1813 suppression must include a non-empty `Justification` that names the intended extension scenario.

## Consequences

- The inheritance surface of the library is explicit and narrow. Consumers cannot accidentally couple their code to an implementation class they were not intended to extend.
- New types default to `sealed` without discussion. Removing `sealed` from a class is the decision that requires justification, not the other way around.
- Any `[SuppressMessage("Performance", "CA1813", Justification = "...")]` in the codebase documents an intentional extension point, making such exceptions discoverable by search.
