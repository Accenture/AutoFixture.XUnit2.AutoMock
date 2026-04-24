---
id: decision-26
title: TreatWarningsAsErrors with the five-package analyzer stack
date: '2026-04-24'
status: accepted
category: coding-style
---
## Context

Analyzer warnings that do not fail builds tend to accumulate over time. Style and quality violations that are treated as advisory become invisible noise: contributors learn to ignore them, and the warning count grows until the list is useless as a quality signal.

As the project matured, the question was:

1. Whether to enforce a strict zero-warning build policy.
2. Which analyzer packages to include.
3. How to handle legitimate suppressions.
4. How to handle informational advisories that are not code quality issues (e.g., NuGet
   vulnerability notifications).

## Decision

`TreatWarningsAsErrors=true` and `CodeAnalysisTreatWarningsAsErrors=true` are set globally in
`Directory.Build.props`. The analyzer stack is fixed at five packages:

| Package | Domain |
| --- | --- |
| StyleCop.Analyzers | Namespace ordering, `using` placement, member ordering, formatting |
| Roslynator.Analyzers | General C# quality, idiomatic patterns |
| Roslynator.Formatting.Analyzers | Whitespace and blank-line formatting |
| SonarAnalyzer.CSharp | Reliability, maintainability, security hotspots |
| Microsoft.CodeAnalysis.NetAnalyzers | .NET API usage correctness |

Test projects additionally include `xunit.analyzers` for xUnit2-specific best practices.

NuGet audit warnings (NU1901–NU1904) are listed in `WarningsNotAsErrors` because they are informational dependency advisories, not code quality issues; they are reviewed separately.

`[SuppressMessage]` is permitted only with a non-empty `Justification` parameter.

## Consequences

- A passing `dotnet build` is a complete quality signal: no style violations, no analyzer issues, no code correctness warnings have been introduced.
- Adding a new analyzer package to the stack is a deliberate decision: it may introduce violations across all existing code and must be resolved before the build passes again.
- The five packages cover distinct domains with minimal overlap, so contradictory rules are unlikely; when they do conflict, the `.editorconfig` severity settings are the tiebreaker.
- Each `[SuppressMessage]` with a `Justification` serves as in-code documentation of an intentional exception, making suppressions discoverable and reviewable.
