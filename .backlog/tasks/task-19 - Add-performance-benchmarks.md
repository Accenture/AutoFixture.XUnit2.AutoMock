---
id: TASK-19
title: Add performance benchmarks
status: To Do
assignee:
  - piotrzajac
  - claude
created_date: '2026-04-22'
updated_date: '2026-04-22'
labels:
  - performance
dependencies: []
priority: low
---

## Context

Tooling selection, benchmark scope, project structure, and xUnit execution model analysis are
recorded in [DECISION-1 — Performance benchmark tooling and approach](../decisions/decision-23%20-%20Performance-benchmark-tooling-and-approach.md).

**Summary of decisions:**

- **Tool:** BenchmarkDotNet with `[MemoryDiagnoser]`
- **Scope:** full attribute pipeline (`GetData(MethodInfo)`) — not fixture-only
- **Structure:** three projects, one per mock module; not added to any `.sln` file
- **Execution model:** call `GetData()` directly twice per iteration (IDE double-discovery);
  use a real `MethodInfo` from a representative method defined in the benchmark project

### Benchmark scenarios (identical across all three projects, different attribute types)

| Benchmark class | Scenario |
| --- | --- |
| `PocoGenerationBenchmark` | Flat POCO with 5 primitive properties |
| `DeepGraphGenerationBenchmark` | Object graph 4 levels deep with collections |
| `FrozenVsUnfrozenBenchmark` | Same type with and without `[Frozen]` |
| `VirtualMembersBenchmark` | Many virtual properties, suppressed vs not |

### Running benchmarks

```bash
dotnet run --project src/Objectivity.AutoFixture.XUnit2.AutoMoq.Benchmarks \
           --configuration Release -- --filter '*'
```

(Repeat for `AutoFakeItEasy.Benchmarks` and `AutoNSubstitute.Benchmarks`.)

---

## Acceptance Criteria

<!-- AC:BEGIN -->
- [ ] #1 Three benchmark projects exist under `src/`, one per mock module, each building with `dotnet build --configuration Release`
- [ ] #2 BenchmarkDotNet is the only benchmarking dependency; no test framework packages are added
- [ ] #3 All four benchmark classes are implemented in each project using that module's own attribute types, each carrying `[MemoryDiagnoser]`
- [ ] #4 Each benchmark calls `GetData()` twice per iteration using a real `MethodInfo` resolved in `[GlobalSetup]`
- [ ] #5 Running any project with `--configuration Release -- --filter '*'` produces a valid Markdown summary table
- [ ] #6 `BenchmarkDotNet.Artifacts/` is added to `.gitignore`
- [ ] #7 A `## Running benchmarks` section is added to `CONTRIBUTING.md` explaining the invocation command for each project
<!-- AC:END -->
