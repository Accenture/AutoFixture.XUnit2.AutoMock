# PickFromRange Attribute

Ensures that only values from a specified range will be generated.

```csharp
[Theory]
[AutoData]
public void RangeAttributeUsage(
    [PickFromRange(11, 19)] int teenagerAge)
{
    Assert.True(teenagerAge is > 11 and < 19);
}
```
