# Extension Model Reference — Parameter Attributes

This document walks through `PickFromRangeAttribute` as a canonical example of how parameter
attributes are implemented in this repository.

## Annotated source

```csharp
namespace Objectivity.AutoFixture.XUnit2.Core.Attributes
{
    // ① using directives go INSIDE the namespace block (StyleCop SA1210)
    using System;
    using System.Reflection;

    // ② global:: prefix on all external packages (decision — C# style)
    using global::AutoFixture;
    using global::AutoFixture.Kernel;
    using global::AutoFixture.Xunit2;

    using Objectivity.AutoFixture.XUnit2.Core.Common;
    using Objectivity.AutoFixture.XUnit2.Core.SpecimenBuilders;

    // ③ AttributeUsage restricts to Parameter; AllowMultiple = false is typical
    [AttributeUsage(AttributeTargets.Parameter, AllowMultiple = false)]
    // ④ sealed class (decision-28) — all attribute-derived types are sealed
    // ⑤ derives from CustomizeAttribute (AutoFixture.Xunit2), not DataAttribute
    public sealed class PickFromRangeAttribute : CustomizeAttribute
    {
        // Public constructor overloads for each supported numeric type
        public PickFromRangeAttribute(int minimum, int maximum)
            : this((object)minimum, maximum)
        {
        }

        // ... additional overloads (uint, long, ulong, double, float) follow the same pattern

        // Private canonical constructor validates the range
        private PickFromRangeAttribute(object minimum, object maximum)
        {
            if (((IComparable)minimum).CompareTo((IComparable)maximum) > 0)
            {
                throw new ArgumentOutOfRangeException(
                    nameof(minimum),
                    $"Parameter {nameof(minimum)} must be lower or equal to {nameof(maximum)}.");
            }

            this.Minimum = minimum;
            this.Maximum = maximum;
        }

        public object Minimum { get; }

        public object Maximum { get; }

        // ⑥ override GetCustomization — the single required method from CustomizeAttribute
        public override ICustomization GetCustomization(ParameterInfo parameter)
        {
            // ⑦ parameter.NotNull(nameof(parameter)) — use NotNull() guard (decision-29)
            //    NotNull() is an extension method from Core/Common/
            //    It throws ArgumentNullException when parameter is null
            return new FilteringSpecimenBuilder(
                new RequestFactoryRelay(
                    (type) => new RangedNumberRequest(type, this.Minimum, this.Maximum)),
                new EqualRequestSpecification(parameter.NotNull(nameof(parameter))))
                .ToCustomization();
        }
    }
}
```

## Key points

| # | Rule | Where enforced |
| --- | --- | --- |
| ① | `using` inside namespace | StyleCop SA1210 (build fails if violated) |
| ② | `global::` prefix on external packages | StyleCop SA1135 / team convention |
| ③ | `[AttributeUsage(AttributeTargets.Parameter)]` | Restricts attribute usage to parameters |
| ④ | `sealed` | decision-28 |
| ⑤ | Derives from `CustomizeAttribute` | AutoFixture.Xunit2 integration contract |
| ⑦ | `NotNull()` guard | decision-29 |

## SpecimenBuilders used in data-narrowing attributes

| Class | Purpose |
| --- | --- |
| `FilteringSpecimenBuilder` | Wraps a builder and applies it only to matching requests |
| `RequestFactoryRelay` | Creates an AutoFixture request from the parameter type |
| `EqualRequestSpecification` | Matches requests equal to the given `ParameterInfo` |
| `RangedNumberRequest` | Built-in AutoFixture request for a number within a range |

## Minimal attribute skeleton

Use this as a starting point for a new attribute:

```csharp
namespace Objectivity.AutoFixture.XUnit2.Core.Attributes
{
    using System;
    using System.Reflection;

    using global::AutoFixture;
    using global::AutoFixture.Xunit2;

    using Objectivity.AutoFixture.XUnit2.Core.Common;

    [AttributeUsage(AttributeTargets.Parameter, AllowMultiple = false)]
    public sealed class MyNewAttribute : CustomizeAttribute
    {
        public MyNewAttribute(/* constructor parameters */)
        {
            // validate and store
        }

        public override ICustomization GetCustomization(ParameterInfo parameter)
        {
            parameter.NotNull(nameof(parameter));

            // return an ICustomization that applies the constraint
            throw new NotImplementedException();
        }
    }
}
```
