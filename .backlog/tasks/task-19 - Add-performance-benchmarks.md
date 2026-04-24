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

The benchmarks must answer three questions: (a) what overhead does this library add on top of
AutoFixture, (b) which layer of the attribute pipeline is responsible for that overhead, and
(c) which usage patterns are expensive enough to warrant guidance or refactoring.

Tooling selection, benchmark scope, project structure, and xUnit execution model analysis are
recorded in [DECISION-23 - Performance benchmark tooling and approach](../decisions/decision-23%20-%20Performance-benchmark-tooling-and-approach.md).

**Summary of decisions:**

- **Tool:** BenchmarkDotNet with `[MemoryDiagnoser]`
- **Scope:** full attribute pipeline (`GetData(MethodInfo)`) as primary scope; `LibraryOverheadBenchmark` additionally includes a fixture-only method as a `[Baseline]` reference to compute the overhead ratio
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
| `LibraryOverheadBenchmark` | Three layers for a flat POCO: AutoFixture-only baseline → AutoFixture + our customization chain → full attribute pipeline. Uses `[Benchmark(Baseline = true)]` on the AutoFixture-only method so the output table includes a **Ratio** column revealing exactly how much overhead this library adds at each layer. |
| `MemberDataShareFixtureBenchmark` | `[MemberAutoMockData]` with a 5-item data source, comparing `ShareFixture = true` vs `ShareFixture = false`. Exposes the per-item customization chain cost, which is the worst-case usage pattern. |

### Layered measurement design for `LibraryOverheadBenchmark`

The three methods isolate responsibility:

- **Layer A - AutoFixture only** (`Baseline = true`): `new Fixture(); Customize(AutoMoq); fixture.Create<T>()` - pure AutoFixture cost, no attribute machinery
- **Layer B - Customization chain**: Layer A + `AutoDataCommonCustomization` - adds our recursion-handling setup (`DoNotThrowOnRecursion`, `OmitOnRecursion`)
- **Layer C - Full pipeline**: `attribute.GetData(MethodInfo)` called twice - adds attribute wiring, reflection on parameters, per-parameter `SpecimenContext` allocation

Delta (A→B) = cost of `DoNotThrowOnRecursionCustomization` LINQ enumeration + allocations.
Delta (B→C) = cost of attribute machinery: `testMethod.GetParameters()`, `p.GetCustomAttributes()`, LINQ sort per parameter, `new SpecimenContext()` per parameter.

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
- [ ] #3 All six benchmark classes are implemented in each project using that module's own attribute types, each carrying `[MemoryDiagnoser]`: `PocoGenerationBenchmark`, `DeepGraphGenerationBenchmark`, `FrozenVsUnfrozenBenchmark`, `VirtualMembersBenchmark`, `LibraryOverheadBenchmark`, `MemberDataShareFixtureBenchmark`
- [ ] #4 Each benchmark calls `GetData()` twice per iteration using a real `MethodInfo` resolved in `[GlobalSetup]`
- [ ] #5 Running any project with `--configuration Release -- --filter '*'` produces a valid Markdown summary table
- [ ] #6 `BenchmarkDotNet.Artifacts/` is added to `.gitignore`
- [ ] #7 A `## Running benchmarks` section is added to `CONTRIBUTING.md` explaining the invocation command for each project
- [ ] #8 `LibraryOverheadBenchmark` exists in each project with exactly three methods (`FixtureOnly`, `CustomizationChain`, `FullPipeline`); `FixtureOnly` carries `[Benchmark(Baseline = true)]`; the produced Markdown summary table includes a **Ratio** column
- [ ] #9 `MemberDataShareFixtureBenchmark` exists in each project comparing `ShareFixture = true` vs `ShareFixture = false` using a static 5-item data source; both variants are measured with `[MemoryDiagnoser]`
- [ ] #10 After a first run, a `docs/performance-findings.md` file is added recording: (a) the Ratio values from `LibraryOverheadBenchmark` per mock module, (b) which usage patterns are cheap vs expensive, and (c) any usage recommendations or refactoring proposals justified by the numbers
<!-- AC:END -->

## Related

- Future: CI benchmarking with PR delta comments (noted in DECISION-23 Consequences; no task
  created yet - requires a separate design decision on tooling and workflow).
