# PickFromValues Attribute

Ensures that only values from the specified list will be generated.

```csharp
[Theory]
[AutoData]
public void ValuesAttributeUsage(
    [PickFromValues(DayOfWeek.Saturday, DayOfWeek.Sunday)] HashSet<DayOfWeek> weekend)
{
    var weekendDays = new[] { DayOfWeek.Saturday, DayOfWeek.Sunday };
    Assert.Equivalent(weekendDays, weekend);
}
```
