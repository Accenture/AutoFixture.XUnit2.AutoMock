# Objectivity.AutoFixture.XUnit2.AutoMock

[![CI/CD](https://github.com/ObjectivityLtd/AutoFixture.XUnit2.AutoMock/actions/workflows/cicd.yml/badge.svg?branch=master)](https://github.com/ObjectivityLtd/AutoFixture.XUnit2.AutoMock/actions/workflows/cicd.yml) [![codecov](https://codecov.io/gh/ObjectivityLtd/AutoFixture.XUnit2.AutoMock/branch/master/graph/badge.svg)](https://codecov.io/gh/ObjectivityLtd/AutoFixture.XUnit2.AutoMock) [![Mutation testing badge](https://img.shields.io/endpoint?style=flat&url=https%3A%2F%2Fbadge-api.stryker-mutator.io%2Fgithub.com%2FObjectivityLtd%2FAutoFixture.XUnit2.AutoMock%2Fmaster)](https://dashboard.stryker-mutator.io/reports/github.com/ObjectivityLtd/AutoFixture.XUnit2.AutoMock/master) [![License: MIT](https://img.shields.io/github/license/ObjectivityLtd/AutoFixture.XUnit2.AutoMock?label=License&color=brightgreen)](https://opensource.org/licenses/MIT) [![FOSSA Status](https://app.fossa.com/api/projects/git%2Bgithub.com%2FObjectivityLtd%2FAutoFixture.XUnit2.AutoMock.svg?type=shield)](https://app.fossa.com/projects/git%2Bgithub.com%2FObjectivityLtd%2FAutoFixture.XUnit2.AutoMock?ref=badge_shield)

Accelerates preparation of mocked structures for unit tests under  [XUnit2](http://xunit.github.io/) by configuring [AutoFixture](https://github.com/AutoFixture/AutoFixture) data generation to use a mocking library of your choice. Gracefully handles recursive structures by omitting recursions.

It provides the following mocking attributes:

- AutoMockData
- InlineAutoMockData
- MemberAutoMockData

***

## Supported mocking libraries

| Mocking library                                           | Corresponding NuGet package |
| ---------------------------------------------------------:|:--------------------------- |
| [Moq](https://github.com/moq/moq4)                        | [![AutoMoq](https://img.shields.io/nuget/v/Objectivity.AutoFixture.XUnit2.AutoMoq.svg?label=AutoMoq) ![Downloads](https://img.shields.io/nuget/dt/Objectivity.AutoFixture.XUnit2.AutoMoq.svg)](https://www.nuget.org/packages/Objectivity.AutoFixture.XUnit2.AutoMoq/) |
| [NSubstitute](https://github.com/nsubstitute/NSubstitute) | [![AutoNSubstitute](https://img.shields.io/nuget/v/Objectivity.AutoFixture.XUnit2.AutoNSubstitute.svg?label=AutoNSubstitute) ![Downloads](https://img.shields.io/nuget/dt/Objectivity.AutoFixture.XUnit2.AutoNSubstitute.svg)](https://www.nuget.org/packages/Objectivity.AutoFixture.XUnit2.AutoNSubstitute/) |
| [FakeItEasy](https://github.com/FakeItEasy/FakeItEasy)    | [![AutoFakeItEasy](https://img.shields.io/nuget/v/Objectivity.AutoFixture.XUnit2.AutoFakeItEasy.svg?label=AutoFakeItEasy) ![Downloads](https://img.shields.io/nuget/dt/Objectivity.AutoFixture.XUnit2.AutoFakeItEasy.svg)](https://www.nuget.org/packages/Objectivity.AutoFixture.XUnit2.AutoFakeItEasy) |

## Attributes

### AutoMockData

Provides auto-generated data specimens generated by [AutoFixture](https://github.com/AutoFixture/AutoFixture) with a mocking library as an extension to xUnit.net's `Theory` attribute.

#### Arguments

- IgnoreVirtualMembers - disables generation of members marked as `virtual`; by default set to `false`

#### Example

```csharp
[Theory]
[AutoMockData]
public void GivenCurrencyConverter_WhenConvertToPln_ThenMustReturnCorrectConvertedAmount(
    string testCurrencySymbol,
    [Frozen] ICurrencyExchangeProvider currencyProvider,
    CurrencyConverter currencyConverter)
{
    // Arrange
    Mock.Get(currencyProvider)
        .Setup(cp => cp.GetCurrencyExchangeRate(testCurrencySymbol))
        .Returns(100M);

    // Act
    decimal result = currencyConverter.ConvertToPln(testCurrencySymbol, 100M);

    // Assert
    Assert.Equal(10000M, result);
}
```

### InlineAutoMockData

Provides a data source for a `Theory`, with the data coming from inline values combined with auto-generated data specimens generated by [AutoFixture](https://github.com/AutoFixture/AutoFixture) with a mocking library.

#### Arguments

- IgnoreVirtualMembers - disables generation of members marked as `virtual`; by default set to `false`

#### Example

```csharp
[Theory]
[InlineAutoMockData("USD", 3, 10, 30)]
[InlineAutoMockData("EUR", 4, 20, 80)]
public void GivenCurrencyConverter_WhenConvertToPln_ThenMustReturnCorrectConvertedAmount(
    string testCurrencySymbol,
    decimal exchangeRate,
    decimal currencyAmount,
    decimal expectedPlnAmount,
    [Frozen] ICurrencyExchangeProvider currencyProvider,
    CurrencyConverter currencyConverter)
{
    // Arrange
    Mock.Get(currencyProvider)
        .Setup(cp => cp.GetCurrencyExchangeRate(testCurrencySymbol))
        .Returns(exchangeRate);

    // Act
    decimal result = currencyConverter.ConvertToPln(testCurrencySymbol, currencyAmount);

    // Assert
    Assert.Equal(expectedPlnAmount, result);
}
```

### MemberAutoMockData

Provides a data source for a `Theory`, with the data coming from one of the following sources:

- A static property
- A static field
- A static method (with parameters)

combined with auto-generated data specimens generated by [AutoFixture](https://github.com/AutoFixture/AutoFixture) with a mocking library.

The member must return something compatible with `Enumerable<object[]>` with the test data.

**Caution:** The property is completely enumerated by .ToList() before any test is run. Hence it should return independent object sets.

#### Arguments

- IgnoreVirtualMembers - disables generation of members marked as `virtual`; by default set to `false`
- ShareFixture - indicates whether to share a `fixture` across all data items should be used or new one; by default set to `true`

#### Example

```csharp
public class CurrencyConverterFixture
{
    public static IEnumerable<object[]> CurrencyConversionRatesWithResult()
    {
        return new List<object[]>
            {
                new object[] { "USD", 3M, 10M, 30M },
                new object[] { "EUR", 4M, 20M, 80M }
            };
    }
}
```

```csharp
[Theory]
[MemberAutoMockData("CurrencyConversionRatesWithResult", MemberType = typeof(CurrencyConverterFixture))]
public void GivenCurrencyConverter_WhenConvertToPln_ThenMustReturnCorrectConvertedAmount(
    string testCurrencySymbol,
    decimal exchangeRate,
    decimal currencyAmount,
    decimal expectedPlnAmount,
    [Frozen] ICurrencyExchangeProvider currencyProvider,
    CurrencyConverter currencyConverter)
{
    // Arrange
    Mock.Get(currencyProvider)
        .Setup(cp => cp.GetCurrencyExchangeRate(testCurrencySymbol))
        .Returns(exchangeRate);

    // Act
    decimal result = currencyConverter.ConvertToPln(testCurrencySymbol, currencyAmount);

    // Assert
    Assert.Equal(expectedPlnAmount, result);
}
```

### IgnoreVirtualMembers

An attribute that can be applied to parameters in an `AutoDataAttribute`-driven `Theory` to indicate that the parameter value should not have `virtual` properties populated when the `IFixture` creates an instance of that type.

This attribute allows to disable the generation of members marked as `virtual` on a decorated type wheres `IgnoreVirtualMembers` arguments of mocking attributes mentioned above disable such a generation for all types created by `IFixture`.

**Caution:** Order is important! Applying `IgnoreVirtualMembers` attribute to the subsequent paramater makes precedig parameters of the same type to have `virtual` properties populated and the particular parameter with the following ones of the same type to have `virtual` properties unpopulated.

#### Example

```csharp
public class User
{
    public string Name { get; set; }
    public virtual Address Address { get; set; }
}
```

```csharp
[Theory]
[AutoData]
public void IgnoreVirtualMembersUsage(
    User firstUser,
    [IgnoreVirtualMembers] User secondUser,
    User thirdUser)
{
    Assert.NotNull(firstUser.Name);
    Assert.NotNull(firstUser.Address);

    Assert.NotNull(secondUser.Name);
    Assert.Null(secondUser.Address);

    Assert.NotNull(thirdUser.Name);
    Assert.Null(thirdUser.Address);
}
```

### CustomizeWith *(borrowed from [@devdigital](https://github.com/devdigital/practice-guide-dotnet-testing/wiki/Unit-Tests-Implementation))*

An attribute that can be applied to parameters in an `AutoDataAttribute`-driven `Theory` to apply additional customization when the `IFixture` creates an instance of that type.

#### Arguments

- IncludeParameterType - indicates whether attribute target parameter `Type` should included as a first argument when creating customization; by default set to `false`

**Caution:** Order is important! Applying `CustomizeWith` attribute to the subsequent paramater makes precedig parameters of the same type to be created without specified customization and the particular parameter with the specified customization.

#### Example

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

### CustomizeWith\<T>

A generic version of the `CustomizeWith` attribute has been introduced for ease of use. The same rules apply as for the non-generic version.

#### Example

```csharp
public class EmptyCollectionCustomization : ICustomization
{
    public EmptyCollectionCustomization(Type reflectedType)
    {
        this.ReflectedType = reflectedType;
    }

    public Type ReflectedType { get; }

    public void Customize(IFixture fixture)
    {
        var emptyArray = Array.CreateInstance(this.ReflectedType.GenericTypeArguments.Single(), 0);

        fixture.Customizations.Add(
            new FilteringSpecimenBuilder(
                new FixedBuilder(emptyArray),
                new ExactTypeSpecification(this.ReflectedType)));
    }
}
```

```csharp
public sealed class EmptyCollectionAttribute : CustomizeWithAttribute<EmptyCollectionCustomization>
{
    public EmptyCollectionAttribute()
    {
        this.IncludeParameterType = true;
    }
}
```

```csharp
[Theory]
[AutoData]
public void CustomizeWithAttributeUsage(
    IList<string> firstStore,
    [EmptyCollection] IList<string> secondStore,
    IList<string> thirdStore,
    IList<int?> fourthStore)
{
    Assert.NotEmpty(firstStore);
    Assert.Empty(secondStore);
    Assert.Empty(thirdStore);
    Assert.NotEmpty(fourthStore);
}
```

***

## Data filtering attributes

The following attributes helps narrowing down data generation to specific values or omitting certain values.

For these attributes to work, they must be used in conjunction with other data generation attributes.

They can be applied to simple types and collections.

### Except

An attribute ensuring that values from outside the specified list will be generated.

#### Example

```csharp
[Theory]
[AutoData]
public void ExceptAttributeUsage(
    [Except(DayOfWeek.Saturday, DayOfWeek.Sunday)] DayOfWeek workday)
{
    Assert.True(workday is >= DayOfWeek.Monday and <= DayOfWeek.Friday);
}
```

### Range

An attribute ensuring that only values from specified range will be generated.

#### Example

```csharp
[Theory]
[AutoData]
public void RangeAttributeUsage(
    [Range(11, 19)] int teenagerAge)
{
    Assert.True(teenagerAge is > 11 and < 19);
}
```

### Values

An attribute ensuring that only values from the specified list will be generated.

#### Example

```csharp
[Theory]
[AutoData]
public void ValuesAttributeUsage(
    [Values(DayOfWeek.Saturday, DayOfWeek.Sunday)] HashSet<DayOfWeek> weekend)
{
    var weekendDays = new[] { DayOfWeek.Saturday, DayOfWeek.Sunday };
    Assert.Equal(weekendDays.Length, weekend.Count);
    Assert.All(weekendDays, (day) => Assert.Contains(day, weekend));
}
```

***

## Tips and tricks

### Fixture injection

You can inject same instance of `IFixture` to a test method by adding mentioned interface as an argument of test method.

```csharp
[Theory]
[AutoMockData]
public void FixtureInjection(IFixture fixture)
{
    Assert.NotNull(fixture);
}
```

### IgnoreVirtualMembers issue

You should be aware that the *CLR* requires that interface methods be marked as virtual. Please look at the following example:

```csharp
public interface IUser
{
    string Name { get; set; }
    User Substitute { get; set; }
}

public class User : IUser
{
    public string Name { get; set; }
    public virtual User Substitute { get; set; }
}
```

You can see than only `Substitute` property has been explicitly marked as `virtual`. In such situation *the compiler* will mark other properties as `virtual` and `sealed`. And finally [AutoFixture](https://github.com/AutoFixture/AutoFixture) will assign `null` value to those properties when option `IgnoreVirtualMembers` will be set to `true`.

```csharp
[Theory]
[AutoMockData(IgnoreVirtualMembers = true)]
public void IssueWithClassThatImplementsInterface(User user)
{
    Assert.Null(user.Name);
    Assert.Null(user.Substitute);
}
```


## License
[![FOSSA Status](https://app.fossa.com/api/projects/git%2Bgithub.com%2FObjectivityLtd%2FAutoFixture.XUnit2.AutoMock.svg?type=large)](https://app.fossa.com/projects/git%2Bgithub.com%2FObjectivityLtd%2FAutoFixture.XUnit2.AutoMock?ref=badge_large)
