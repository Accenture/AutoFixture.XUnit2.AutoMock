# Test Class Template

Copy this scaffold, replace all `<placeholder>` values, and delete any sections you do not need.

```csharp
namespace Objectivity.AutoFixture.XUnit2.<Module>.Tests.<Area>
{
    // using directives go INSIDE the namespace block (StyleCop SA1210)
    using global::AutoFixture;
    using global::AutoFixture.Xunit2;
    using Moq;  // or FakeItEasy / NSubstitute depending on the module
    using Xunit;

    // [Collection] uses the SUBJECT class name, not the test class name
    [Collection("<SubjectClass>")]
    [Trait("Category", "<Area>")]  // e.g. "Attributes", "AutoData", "Core"
    public class <SubjectClass>Tests
    {
        // --- Fact: single deterministic case ---
        // DisplayName: "WHEN <action> THEN <outcome>"
        // Method name mirrors DisplayName in PascalCase_WithUnderscores
        [Fact(DisplayName = "WHEN <action> THEN <outcome>")]
        public void When<Action>_Then<Outcome>()
        {
            // Arrange

            // Act

            // Assert
        }

        // --- Theory: multiple data rows, precondition varies ---
        // Data-source attribute ABOVE [Theory], never below
        [AutoMockData]
        [Theory(DisplayName = "GIVEN <precondition> WHEN <action> THEN <outcome>")]
        public void Given<Precondition>_When<Action>_Then<Outcome>(
            IFixture fixture)
        {
            // Arrange

            // Act

            // Assert
        }

        // --- Theory with inline values ---
        [InlineAutoMockData(42)]
        [Theory(DisplayName = "GIVEN <precondition> WHEN <action> THEN <outcome>")]
        public void Given<Precondition>_When<Action>_Then<Outcome>2(
            int value,
            IFixture fixture)
        {
            // Arrange

            // Act

            // Assert
        }
    }
}
```

## Placeholder guide

| Placeholder | Replace with |
| --- | --- |
| `<Module>` | `Core`, `AutoMoq`, `AutoFakeItEasy`, or `AutoNSubstitute` |
| `<Area>` | `Attributes`, `AutoData`, `Common`, etc. — mirrors source folder |
| `<SubjectClass>` | The class under test (without `Tests` suffix) |
| `<action>` | What the code does, e.g. `constructor is invoked` |
| `<outcome>` | What the expected result is, e.g. `properties are set` |
| `<precondition>` | The varying condition for a Theory, e.g. `value is negative` |

## Common attribute wiring test pattern

Tests that verify attribute wiring use `Mock<IFixture>` and `Mock<IAutoFixtureAttributeProvider>`:

```csharp
[AutoMockData]
[Theory(DisplayName = "GIVEN attribute WHEN applied THEN customization is registered")]
public void GivenAttribute_WhenApplied_ThenCustomizationIsRegistered(
    Mock<IFixture> fixtureMock,
    Mock<IAutoFixtureAttributeProvider> providerMock)
{
    // Arrange
    var sut = new MyAttribute();

    // Act
    sut.GetCustomization(/* parameter */);

    // Assert
    fixtureMock.Verify(f => f.Customize(It.IsAny<ICustomization>()), Times.Once);
}
```
