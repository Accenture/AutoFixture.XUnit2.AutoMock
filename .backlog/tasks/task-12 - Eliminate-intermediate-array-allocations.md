---
id: TASK-12
title: "Eliminate intermediate array allocations"
status: Done
assignee:
  - claude
  - piotrzajac
created_date: '2026-04-12'
updated_date: '2026-04-12'
labels:
  - fix
dependencies: []
priority: low
---

## Description

<!-- SECTION:DESCRIPTION:BEGIN -->
In `src/Objectivity.AutoFixture.XUnit2.Core/Common/EnumerableExtensions.cs`, the method
`TryGetEnumerableSingleTypeArgument` contained a conditional branch that built an
interfaces array:

```csharp
var interfaces = type.IsInterface
    ? type.GetInterfaces().Concat(new[] { type }).ToArray()
    : type.GetInterfaces();

var genericInterface = Array.Find(
    interfaces,
    x => x.IsGenericType && x.GetGenericTypeDefinition() == typeof(IEnumerable<>));
```

This had two problems:

1. **Equivalent Stryker mutation (survived):** including or excluding the concrete type in
   the array made no observable difference to the `IEnumerable<>` discovery logic, so
   Stryker correctly flagged the conditional as an equivalent mutation that can never be
   killed by a test.
2. **Unnecessary allocations:** for interface types, `.Concat(new[] { type }).ToArray()`
   allocated three objects (the single-element array, the concatenation enumerator, and
   the resulting array) that served no real purpose.
<!-- SECTION:DESCRIPTION:END -->

## Acceptance Criteria

<!-- AC:BEGIN -->
- [x] #1 `TryGetEnumerableSingleTypeArgument` no longer uses a conditional branch to include the type itself in the interfaces array
- [x] #2 No intermediate arrays are allocated during the `IEnumerable<>` discovery
- [x] #3 The previously surviving Stryker equivalent mutation is eliminated
- [x] #4 All existing tests continue to pass with zero build warnings
<!-- AC:END -->

## Implementation Plan

<!-- SECTION:PLAN:BEGIN -->
Replace the conditional array-building approach with a two-step check that avoids any
intermediate collection to first search the declared interfaces (where `IEnumerable<T>` will be found for
concrete collection types), then falls back to checking the type itself (handles the case
where `type` is directly `IEnumerable<T>`).
<!-- SECTION:PLAN:END -->

## Final Summary

<!-- SECTION:FINAL_SUMMARY:BEGIN -->
Applied to `src/Objectivity.AutoFixture.XUnit2.Core/Common/EnumerableExtensions.cs`.

The conditional branch that built an intermediate interfaces array was replaced with a
two-step null-coalescing expression:

```csharp
var genericInterface = Array.Find(type.GetInterfaces(), IsGenericEnumerable)
    ?? (IsGenericEnumerable(type) ? type : null);
```

A private static helper `IsGenericEnumerable(Type)` was extracted to name the predicate
and allow reuse in both branches. The change:

- Eliminates all three intermediate object allocations present in the original interface-type branch
- Kills the equivalent Stryker mutation by removing the conditional that had no observable effect
- Keeps the same logical behaviour: concrete types are found via their interfaces; types
  that are themselves `IEnumerable<T>` are found via the fallback check
- Build passes with zero warnings; all tests pass across `net8.0`, `net472`, and `net48`
<!-- SECTION:FINAL_SUMMARY:END -->
