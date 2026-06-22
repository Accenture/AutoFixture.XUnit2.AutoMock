# CustomizeWith\<T> Attribute

A generic version of the `CustomizeWith` attribute has been introduced for ease of use. The same rules apply as for the non-generic version.

## Example

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
