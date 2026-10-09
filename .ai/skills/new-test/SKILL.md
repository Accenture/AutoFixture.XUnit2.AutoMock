---
name: new-test
description: Write a new test class for a given subject type following repository conventions. Use when adding tests for a new or existing class. Encodes BDD naming (decision-24) and AAA structure (decision-25). Invoke as: new-test SubjectClass.
---

# new-test

Scaffold a properly structured test class for a given subject.

See `assets/test-template.md` for a complete scaffolded class ready to copy and fill in.

## Steps

**1. Name the test class and file**

- File: `<SubjectClass>Tests.cs`
- Class: `<SubjectClass>Tests`
- Namespace: mirrors the source project namespace under `.Tests`
  - Source: `Objectivity.AutoFixture.XUnit2.Core.Attributes`
  - Test: `Objectivity.AutoFixture.XUnit2.Core.Tests.Attributes`
- Location: `src/<Module>.Tests/<same-folder-path-as-source>/`

**2. Apply required class attributes**

```csharp
[Collection("<SubjectClass>")]   // subject class name — NOT the test class name
[Trait("Category", "<Area>")]    // e.g. "Attributes", "AutoData", "Core"
public class <SubjectClass>Tests
```

**3. Name test methods using BDD convention** (decision-24)

For `[Theory]` (multiple data rows, precondition varies):

- DisplayName: `GIVEN <precondition> WHEN <action> THEN <outcome>`
- Method name: `Given<Precondition>_When<Action>_Then<Outcome>`

For `[Fact]` (single deterministic case):

- DisplayName: `WHEN <action> THEN <outcome>`
- Method name: `When<Action>_Then<Outcome>`

Rules:

- `DisplayName` is **always** set; never omit it
- GIVEN, WHEN, THEN are written in UPPER CASE; the rest is sentence case
- Method name uses PascalCase with underscores between the three BDD sections
- Never use `Test` as a suffix or prefix in method names

**4. Structure every test with AAA** (decision-25)

```csharp
[Fact(DisplayName = "WHEN X THEN Y")]
public void WhenX_ThenY()
{
    // Arrange

    // Act

    // Assert
}
```

All three comment blocks are mandatory even when a section is empty.

**5. Place data-source attributes above `[Theory]`**

```csharp
[AutoMockData]
[Theory(DisplayName = "GIVEN X WHEN Y THEN Z")]
public void GivenX_WhenY_ThenZ(IFixture fixture) { ... }
```

The data-source attribute must appear **above** `[Theory]`, never below.

**6. Inject test data via parameters**

- Never use `new Fixture()` in test methods
- Inject `IFixture` via the test method signature
- For attribute wiring tests: use `Mock<IFixture>` and `Mock<IAutoFixtureAttributeProvider>`

## Rules

- Every `[Fact]` and `[Theory]` must have `DisplayName` set
- `[Collection]` must use the **subject** class name, not the test class name
- Data-source attributes go **above** `[Theory]`
