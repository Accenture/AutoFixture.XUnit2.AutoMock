---
id: decision-29
title: NotNull() fluent guard over throw new ArgumentNullException
date: '2026-04-24'
status: accepted
category: coding-style
---
## Context

Null-guard patterns in .NET have evolved over time:

1. `if (x == null) throw new ArgumentNullException(nameof(x));` - verbose, requires a temporary variable when the validated value must also be assigned.
2. `ArgumentNullException.ThrowIfNull(x);` - concise but available only from .NET 6+.
3. C# nullable reference types with compiler enforcement - requires opt-in per project and does not eliminate runtime checks in public API.

This library targets netstandard2.0, netstandard2.1, net472, and net48. `ArgumentNullException.ThrowIfNull` is unavailable on the .NET Framework and netstandard targets. A consistent, cross-framework guard mechanism that also supports fluent chaining at constructor callsites was needed.

## Decision

Use `Check.NotNull<T>(this T value, string parameterName)` from `src/Objectivity.AutoFixture.XUnit2.Core/Common/Check.cs` for all null guards. `NotNull()` is a fluent extension method that returns the validated value, enabling inline assignment:

```csharp
// Instead of:
if (fixture == null) throw new ArgumentNullException(nameof(fixture));
this.Fixture = fixture;

// Write:
this.Fixture = fixture.NotNull(nameof(fixture));

// Or chain directly:
fixture.NotNull(nameof(fixture)).Customize(new AutoMoqCustomization());
```

The implementation throws `ArgumentNullException` with the correct parameter name internally.
JetBrains Annotations attributes (`[ContractAnnotation("value:null => halt")]` and `[ValidatedNotNull]`) are applied to `NotNull()` so that Rider and ReSharper understand the value is guaranteed non-null after the call.

## Consequences

- Constructor bodies read as straightforward assignments rather than guard-block-then-assign pairs, reducing visual noise in constructors with multiple parameters.
- Fluent chaining enables single-expression patterns (`x.NotNull(nameof(x)).Method()`) without introducing a temporary variable.
- The implementation works across all target frameworks (netstandard2.0, net472, net48) with no API availability concern.
- IDE null-safety analysis in Rider and ReSharper remains accurate: the annotations prevent false "possible null dereference" warnings after a `NotNull()` call.
- `Check.cs` carries an origin-attribution comment explaining its provenance; this is one of the explicit exceptions to the minimize-XML-documentation rule (see decision-27).
