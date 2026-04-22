---
id: decision-5
title: Target netstandard2.0/2.1 and net472/net48
date: '2018-04-17'
status: accepted
---
## Context

Many enterprise teams still run test suites on .NET Framework 4.7.2 and 4.8. Targeting only modern .NET would exclude those users, while targeting only .NET Framework would prevent adoption on .NET Core and later runtimes.

## Decision

Library projects target `netstandard2.0`, `netstandard2.1`, `net472`, and `net48`.
Test projects target `.NET Core`, `net472`, and `net48` to validate the full compatibility matrix on each CI run.

## Consequences

- Maximum compatibility across the .NET ecosystem.
- CI must run on `windows-latest` to execute the `net472`/`net48` test slices.
- The test matrix is larger but covers the full surface claimed by the library.
- When .NET Framework support is eventually dropped, all four framework targets in library `.csproj` files must be updated simultaneously.
