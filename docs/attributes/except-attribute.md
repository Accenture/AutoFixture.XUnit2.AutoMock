# Except Attribute

Ensures that values from outside the specified list will be generated.

```csharp
[Theory]
[AutoData]
public void ExceptAttributeUsage(
    [Except(DayOfWeek.Saturday, DayOfWeek.Sunday)] DayOfWeek workday)
{
    Assert.True(workday is >= DayOfWeek.Monday and <= DayOfWeek.Friday);
}
```
