---
id: decision-19
title: Adopt Qodana for code quality scanning
date: '2024-01-16'
status: accepted
---
## Context

The in-build analyzer stack (StyleCop, Roslynator, SonarAnalyzer) catches issues at compile time but cannot provide holistic quality scoring, trend analysis across the codebase, or a consolidated view of maintainability and reliability findings separate from the build log.

## Decision

Integrate JetBrains Qodana into CI as a quality gate. Qodana runs a full Roslyn-based inspection pass independently of the build and reports results in a structured format. It runs on push and pull request to master and complements CodeQL (security focus) and Semgrep (SAST focus) with maintainability and reliability checks.

## Consequences

- Quality metrics are visible in CI as a distinct step, separate from build warnings.
- Qodana's inspection coverage overlaps partially with the in-build analyzers; discrepancies surface issues that build-time suppression may have hidden.
- JetBrains Qodana supports .NET and is compatible with the `windows-latest` CI runner.
