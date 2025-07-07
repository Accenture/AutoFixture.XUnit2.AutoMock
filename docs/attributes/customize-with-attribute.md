# CustomizeWith Attribute

An attribute that can be applied to parameters in an `AutoDataAttribute`-driven `Theory` to apply additional customization when the `IFixture` creates an instance of that type.

## Arguments

- `IncludeParameterType` - indicates whether attribute target parameter `Type` should be included as a first argument when creating customization; by default set to `false`

**Caution:** Order is important! Applying `CustomizeWith` attribute to the subsequent parameter makes preceding parameters of the same type to be created without specified customization and the particular parameter with the specified customization.

## Example

```csharp
public class LocalDatesCustomization : ICustomization
{
    public void Customize(IFixture fixture)
    {
        fixture.Register(() => LocalDate.FromDateTime(fixture.Create<DateTime>()));
    }
}
```

```csharp
[Theory]
[InlineAutoMockData("USD")]
[InlineAutoMockData("EUR")]
public void GivenCurrencyConverter_WhenConvertToPlnAtParticularDay_ThenMustReturnCorrectConvertedAmount(
    string testCurrencySymbol,
    [CustomizeWith(typeof(LocalDatesCustomization))] LocalDate day,
    [Frozen] ICurrencyExchangeProvider currencyProvider,
    CurrencyConverter currencyConverter)
{
    // Arrange
    Mock.Get(currencyProvider)
        .Setup(cp => cp.GetCurrencyExchangeRate(testCurrencySymbol, day))
        .Returns(100M);

    // Act
    decimal result = currencyConverter.ConvertToPln(testCurrencySymbol, 100M, day);

    // Assert
    Assert.Equal(10000M, result);
}
```
