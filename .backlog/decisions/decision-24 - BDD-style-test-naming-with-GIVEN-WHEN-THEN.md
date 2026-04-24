---
id: decision-24
title: BDD-style test naming with GIVEN/WHEN/THEN
date: '2026-04-24'
status: accepted
category: testing
---
## Context

Test method names in most .NET projects are either free-form prose or follow the `MethodName_StateUnderTest_ExpectedBehavior` convention. Both styles embed the subject in the name, which means the name reads like an implementation detail rather than a behavioral specification. As the test suite in this repository grew, reviewers needed to open test bodies to understand what a failing test meant - the name alone was insufficient.

xUnit's `DisplayName` property existed and was rendered in CI output and IDE test runners but was left blank or simply duplicated the method name verbatim, adding no value.

## Decision

Every test method must carry a `DisplayName` written in `UPPER CASE GIVEN/WHEN/THEN` form:

- `GIVEN` introduces the precondition (state before the action).
- `WHEN` introduces the action or event under test.
- `THEN` introduces the expected outcome.

For `[Fact]` tests where there is no meaningful precondition, `WHEN...THEN` alone is acceptable.

The method name mirrors `DisplayName` in `PascalCase_WithUnderscores` form - every word from the display name is preserved, capitalised, and joined by underscores. The word `Test` is never used as a prefix or suffix in either the display name or the method name.

```csharp
// Fact: single action → single outcome
[Fact(DisplayName = "WHEN parameterless constructor is invoked THEN fixture and provider are created")]
public void WhenParameterlessConstructorIsInvoked_ThenFixtureAndProviderAreCreated()

// Theory: precondition + action → outcome
[Theory(DisplayName = "GIVEN test method has object parameters WHEN test run THEN parameters are generated")]
public void GivenTestMethodHasObjectParameters_WhenTestRun_ThenParametersAreGenerated(...)
```

## Consequences

- Test output in CI and IDEs reads as a specification document. A failing test announces its behavioral contract without requiring the reader to open the source file.
- The `PascalCase_WithUnderscores` method name is intentionally redundant with `DisplayName` so that stack traces in non-IDE environments (e.g., raw `dotnet test` output) still carry the full semantic label.
- Test names are longer than those produced by the `MethodName_State_Expected` convention. This is an accepted trade-off: clarity of intent outweighs brevity.
- The convention is enforced by code review (CodeRabbit, see decision-22) rather than by a compiler or analyzer rule.
