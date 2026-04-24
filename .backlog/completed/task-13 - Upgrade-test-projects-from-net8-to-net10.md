---
id: TASK-13
title: 'Upgrade test projects from net8.0 to net10.0'
status: Done
assignee:
  - claude
  - piotrzajac
created_date: '2026-04-12'
labels:
  - feature
dependencies: []
priority: low
---

## Description

<!-- SECTION:DESCRIPTION:BEGIN -->
Replace `net8.0` with `net10.0` in all four test projects. .NET 10 is LTS (supported until November 2028), making it the right long-term target. .NET 9 STS reaches end of support in May 2026 and is not worth targeting. .NET 8 LTS ends November 2026. Library projects already target `netstandard2.0`/`netstandard2.1` which covers all modern .NET versions - no change is needed there.

**Current state:**

- Library projects (Core, AutoMoq, AutoFakeItEasy, AutoNSubstitute): `netstandard2.0;netstandard2.1` + `net472;net48` on Windows
- Test projects (all four `*.Tests`): `net8.0` + `net472;net48` on Windows

**Proposed change:**

- Replace `net8.0` with `net10.0` in all four test projects
- Verify all dependencies support `net10.0` (AutoFixture, xUnit, Moq, FakeItEasy, NSubstitute, Microsoft.NET.Test.Sdk)
- Update CI (`windows-latest` runner already supports .NET 10)
- Update `AGENTS.md` Target Frameworks table
<!-- SECTION:DESCRIPTION:END -->

## Acceptance Criteria
<!-- AC:BEGIN -->
- [x] #1 `net10.0` replaces `net8.0` in the `<TargetFrameworks>` in all four test `.csproj` files
- [x] #2 All tests pass across `net10.0`, `net472`, and `net48`
- [x] #3 CI pipeline passes without errors on all framework slices
- [x] #4 `AGENTS.md` Target Frameworks table is updated to reflect `net10.0` for test projects
<!-- AC:END -->

## Files Affected

- `src/Objectivity.AutoFixture.XUnit2.Core.Tests/Objectivity.AutoFixture.XUnit2.Core.Tests.csproj`
- `src/Objectivity.AutoFixture.XUnit2.AutoMoq.Tests/Objectivity.AutoFixture.XUnit2.AutoMoq.Tests.csproj`
- `src/Objectivity.AutoFixture.XUnit2.AutoFakeItEasy.Tests/Objectivity.AutoFixture.XUnit2.AutoFakeItEasy.Tests.csproj`
- `src/Objectivity.AutoFixture.XUnit2.AutoNSubstitute.Tests/Objectivity.AutoFixture.XUnit2.AutoNSubstitute.Tests.csproj`
- `AGENTS.md` (Target Frameworks table)
