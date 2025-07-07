# PickNegative Attribute

Ensures that only negative values will be generated.

**Caution:** It will throw an exception when being used on an unsupported type or on one which does not accept negative values.

```csharp
[Theory]
[AutoData]
public void NegativeAttributeUsage(
    [PickNegative] int negativeNumber)
{
    Assert.True(negativeNumber < 0);
}
```
