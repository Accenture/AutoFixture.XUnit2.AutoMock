---
id: decision-23
title: Performance benchmark tooling and approach
date: '2026-04-22'
status: accepted
---
## Context

The library has no performance baseline. Future changes to specimen builders, customizations,
or dependency upgrades could regress generation speed or allocation behavior with no means of
detection. A benchmark suite was proposed to establish that baseline.

Four decisions needed to be made before implementation:

1. Which benchmarking tool to use
2. Whether to benchmark only fixture creation or the full attribute pipeline
3. Whether to use one benchmark project or one per mock module
4. Whether to invoke the xUnit runner or call `GetData()` directly

## Decision

### 1. Tooling: BenchmarkDotNet

| Tool | Category | Verdict |
| --- | --- | --- |
| BenchmarkDotNet | Micro-benchmark | **Accepted** — handles JIT warm-up, GC pressure, allocation tracking (`[MemoryDiagnoser]`), and Markdown/JSON export; de-facto standard for .NET |
| dotnet-counters | Runtime diagnostic | Supplementary only — useful for investigating GC pressure after a benchmark reveals a problem |
| PerfView | ETW profiler | Supplementary only — right tool to pinpoint which specimen builder caused a regression; Windows-only (consistent with CI) |
| MiniProfiler | ASP.NET app profiler | Rejected — requires a running web host; cannot measure in-process library calls |
| k6 | HTTP load testing | Rejected — no HTTP surface in this library |
| Gatling | HTTP load testing | Rejected — same reason as k6 |

Benchmarks are an opt-in `dotnet run --configuration Release` step, never part of `dotnet test`.

### 2. Benchmark scope: full attribute pipeline, not fixture-only

**Fixture-only** (`fixture.Create<T>()` directly): simple, one project, no attribute
involvement. Measures AutoFixture's overhead rather than this library's unique contribution.

**Full attribute pipeline** (`attribute.GetData(MethodInfo)`): exercises attribute wiring,
customization composition, and per-parameter specimen resolution — the cost uniquely
attributable to this library on top of AutoFixture.

**Decision: full attribute pipeline.**

### 3. Project structure: three projects, one per mock module

Benchmarking the full attribute pipeline requires referencing each module's attribute types.
Placing all three in a single project creates namespace ambiguity and mixed `using` directives
that obscure which attribute is under test.

Three separate projects mirror the existing test project structure and keep each benchmark
file unambiguous. The projects are **not** added to any existing `.sln` file.

```text
src/
  Objectivity.AutoFixture.XUnit2.AutoMoq.Benchmarks/
  Objectivity.AutoFixture.XUnit2.AutoFakeItEasy.Benchmarks/
  Objectivity.AutoFixture.XUnit2.AutoNSubstitute.Benchmarks/
```

### 4. Execution model: direct `GetData()` calls, not the xUnit runner

xUnit v2's `TheoryDiscoverer` calls `GetData(MethodInfo)` **once** during the discovery
phase. For non-serializable data (AutoFixture objects containing Castle.Core mock proxies),
xUnit holds the resolved objects in memory and does not call `GetData()` a second time during
execution.

The double-call concern is real at the **IDE layer**: Visual Studio Test Explorer and Rider
each trigger an independent discovery pass on project load and again on "Run". Each benchmark
iteration therefore calls `GetData()` **twice** to reflect this reality.

`xunit.runner.utility` was considered but rejected: xUnit's invocation machinery dominates
the numbers and is not attributable to this library, and it requires a circular build
dependency on a separately compiled test assembly.

Benchmarks use a real `MethodInfo` obtained via reflection from a representative test method
defined in the benchmark project itself — not a synthetic stub — so that parameter type
resolution and per-parameter customization attribute scanning are exercised realistically.

## Consequences

- Three new projects to create and maintain.
- `BenchmarkDotNet.Artifacts/` must be added to `.gitignore`.
- `CONTRIBUTING.md` needs a `## Running benchmarks` section.
- Future CI benchmarking (PR delta comments) is a separate follow-on task.
