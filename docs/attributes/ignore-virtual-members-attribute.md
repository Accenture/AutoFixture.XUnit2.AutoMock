# IgnoreVirtualMembers Attribute

An attribute that can be applied to parameters in an `AutoDataAttribute`-driven `Theory` to indicate that the parameter value should not have `virtual` properties populated when the `IFixture` creates an instance of that type.

This attribute allows disabling the generation of members marked as `virtual` on a decorated type, whereas `IgnoreVirtualMembers` arguments of mocking attributes mentioned above disable such a generation for all types created by `IFixture`.

**Caution:** Order is important! Applying `IgnoreVirtualMembers` attribute to the subsequent parameter makes preceding parameters of the same type to have `virtual` properties populated and the particular parameter with the following ones of the same type to have `virtual` properties unpopulated.

## Example

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
