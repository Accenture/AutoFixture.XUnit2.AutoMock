# Tips and Tricks

## Fixture Injection

You can inject the same instance of `IFixture` into a test method by adding the mentioned interface as an argument of the test method.

```csharp
[Theory]
[AutoMockData]
public void FixtureInjection(IFixture fixture)
{
    Assert.NotNull(fixture);
}
```

## IgnoreVirtualMembers Issue

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

Only `Substitute` property is explicitly marked as `virtual`. In such a situation, *the compiler* marks the remaining properties as `virtual` and `sealed`. Consequently [AutoFixture](https://github.com/AutoFixture/AutoFixture) assigns `null` value to those properties when the `IgnoreVirtualMembers` option is set to `true`.

```csharp
[Theory]
[AutoMockData(IgnoreVirtualMembers = true)]
public void IssueWithClassThatImplementsInterface(User user)
{
    Assert.Null(user.Name);
    Assert.Null(user.Substitute);
}
```
