---
id: decision-27
title: Minimize XML documentation; prefer self-documenting names
date: '2026-04-24'
status: accepted
category: design
---
## Context

StyleCop enforces XML documentation comments on all public members by default (CS1591).
Comprehensive XML docs require maintenance alongside code changes; they tend to drift from the implementation and produce noise on members whose names are already self-explanatory.

This library's public API is an xUnit attribute layer - most public properties and methods are named directly after their behavior. Blindly satisfying CS1591 would produce meaningless one-liners such as `/// <summary>Gets the fixture.</summary>` on `public IFixture Fixture { get; }`, which adds no information beyond what the declaration
already communicates.

However, a blanket "no XML docs" rule does not match the codebase's actual needs: some members have non-obvious behavioral consequences, and some internal types were copied from external sources whose origin deserves attribution.

## Decision

Documentation warning codes CS1591, CS1573, and CS1712 are suppressed globally in the `.editorconfig`. `GenerateDocumentationFile=true` is retained so that any XML comments that are written are validated at build time - a malformed comment or a missing `<param>` tag will still produce an error.

XML documentation comments are written only where the member name alone does not communicate its contract, side-effect, or constraint. Current examples of members that do carry comments:

- Boolean behavioral properties whose effect is non-obvious from the name alone (e.g., `IgnoreVirtualMembers`, `ShareFixture`).
- Types or members copied from external sources, where an origin-attribution comment explains the provenance (e.g., `Check.cs`).

## Consequences

- No pressure to write meaningless one-liner docs that add noise without adding information.
- The small number of comments that do exist carry signal: their presence on a member indicates that the name alone was judged insufficient.
- NuGet consumers browsing IntelliSense see tooltips only on the members that genuinely need them; the majority of well-named attributes and properties communicate through their names.
- When adding a new public member, the authoring question is: "Is the name sufficient?" If yes, no comment is needed. If the answer requires more than one short sentence to explain, the name should be reconsidered first.
