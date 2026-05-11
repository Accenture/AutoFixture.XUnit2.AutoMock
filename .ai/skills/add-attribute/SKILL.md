---
name: add-attribute
description: Scaffold a new xUnit2 parameter attribute in the Core project. Use this skill when adding a new [AttributeName] that applies to all mock modules (AutoMoq, AutoFakeItEasy, AutoNSubstitute). Encodes the extension model, coding conventions, and test conventions for this repository. Invoke as: add-attribute AttributeName.
---

# add-attribute

Scaffold a new parameter attribute in `Core` following all repository conventions.

Parameter attributes apply to individual test method parameters and affect how AutoFixture
generates values for them. They derive from `CustomizeAttribute` (AutoFixture.Xunit2) and
override `GetCustomization`.

See `references/extension-model.md` for an annotated full example.

## Steps

**1. Understand the attribute's purpose**

- What constraint does it place on the generated value?
- Does it accept constructor parameters? What are their types and valid ranges?
- Review existing similar attributes for patterns:
  - `[Except]` — excludes specific values
  - `[PickFromRange(min, max)]` — constrains to a numeric range
  - `[PickFromValues(v1, v2)]` — picks from an explicit list
  - `[PickNegative]` — generates negative numbers only

**2. Create the attribute class**

Path: `src/Objectivity.AutoFixture.XUnit2.Core/Attributes/<Name>Attribute.cs`

Required conventions:

- `sealed class` deriving from `CustomizeAttribute` (decision-28)
- `using` directives go **inside** the namespace block (StyleCop SA1210)
- `global::` prefix on all external packages (AutoFixture, etc.)
- `[AttributeUsage(AttributeTargets.Parameter, AllowMultiple = false)]`
- `parameter.NotNull(nameof(parameter))` guard in `GetCustomization` (decision-29)
- No XML doc comments on public members (decision-27); use self-documenting names

**3. Create the test class**

Path: `src/Objectivity.AutoFixture.XUnit2.Core.Tests/Attributes/<Name>AttributeTests.cs`

Required structure:

- `[Collection("<Name>Attribute")]` — use the **subject** class name
- `[Trait("Category", "Attributes")]`
- BDD DisplayName on every `[Fact]` and `[Theory]` (decision-24)
- AAA structure with mandatory `// Arrange`, `// Act`, `// Assert` comments (decision-25)
- `[AutoMockData]` placed **above** `[Theory]`
- No `new Fixture()` — inject `IFixture` via test method parameters

Minimum test coverage:

- Constructor validation (invalid arguments throw expected exceptions)
- Property values are stored correctly
- `GetCustomization` returns a customization that causes AutoFixture to generate
  values satisfying the constraint

**4. Update AGENTS.md**

Add a row to the "Parameter Attributes (Core, apply to all modules)" table:

```text
| `[<Name>]` | <short description of what it does> |
```

**5. Run validate**

Follow `.ai/skills/validate/SKILL.md` to confirm build and tests pass.

## Rules

- Add to `Core` only if the attribute applies to all three mock modules
- Never add Moq/FakeItEasy/NSubstitute-specific logic to `Core`
- Build must pass with 0 warnings before marking the task complete
