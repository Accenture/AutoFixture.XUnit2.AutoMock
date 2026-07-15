---
name: validate
description: Validate code changes before committing. Runs dotnet build (zero warnings required) and dotnet test, then scans changed test files for naming and structural convention violations. Use before every commit in this repository.
compatibility: Requires .NET SDK. Run from the repository root directory.
---

# validate

Run this skill before any commit to catch build warnings, test failures, and convention
violations early.

## Steps

**1. Build** — zero warnings required

```bash
dotnet build src/Objectivity.AutoFixture.XUnit2.AutoMock.sln
```

The build runs StyleCop, Roslynator, SonarAnalyzer, and xunit.analyzers with
`TreatWarningsAsErrors=true`. Any warning is a build failure. A clean build means all
style and correctness rules pass.

**2. Test** — fast slice

```bash
dotnet test src/Objectivity.AutoFixture.XUnit2.AutoMock.sln --framework net10.0
```

For full coverage across all framework slices (net10.0, net472, net48 on Windows):

```bash
dotnet test src/Objectivity.AutoFixture.XUnit2.AutoMock.sln
```

**3. Scan changed test files for convention violations**

Run `git diff --name-only HEAD` and check each modified `*Tests.cs` file:

| Check | Rule |
| --- | --- |
| Missing DisplayName | Every `[Fact]` and `[Theory]` must have `DisplayName = "..."` |
| Wrong DisplayName format | Theory: `GIVEN ... WHEN ... THEN ...`; Fact: `WHEN ... THEN ...` — GIVEN/WHEN/THEN in UPPER CASE |
| Test in method name | Method name must not start or end with `Test` |
| Wrong attribute order | `[AutoMockData]`/`[InlineAutoMockData]`/`[MemberAutoMockData]` must appear **above** `[Theory]` |
| Direct Fixture instantiation | No `new Fixture()` — inject `IFixture` via test method parameters |
| Missing AAA comments | Every test body must have `// Arrange`, `// Act`, `// Assert` section comments |

**4. Report results**

List each issue with file name and violated rule. A clean output with no issues means the
change is ready to commit.

## Pass criteria

- Build exits with code 0, zero warnings
- All tests pass
- No convention violations in changed test files
