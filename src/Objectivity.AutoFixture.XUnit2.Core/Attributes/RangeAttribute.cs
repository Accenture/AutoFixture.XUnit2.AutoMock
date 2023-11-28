namespace Objectivity.AutoFixture.XUnit2.Core.Attributes
{
    using System;
    using System.Reflection;

    using global::AutoFixture;
    using global::AutoFixture.Kernel;
    using global::AutoFixture.Xunit2;

    using Objectivity.AutoFixture.XUnit2.Core.SpecimenBuilders;

    [AttributeUsage(AttributeTargets.Parameter, AllowMultiple = false)]
    public sealed class RangeAttribute : CustomizeAttribute
    {
        public RangeAttribute(int minimum, int maximum)
            : this((object)minimum, maximum)
        {
        }

        public RangeAttribute(uint minimum, uint maximum)
            : this((object)minimum, maximum)
        {
        }

        public RangeAttribute(long minimum, long maximum)
            : this((object)minimum, maximum)
        {
        }

        public RangeAttribute(ulong minimum, ulong maximum)
            : this((object)minimum, maximum)
        {
        }

        public RangeAttribute(double minimum, double maximum)
            : this((object)minimum, maximum)
        {
        }

        public RangeAttribute(float minimum, float maximum)
            : this((object)minimum, maximum)
        {
        }

        private RangeAttribute(object minimum, object maximum)
        {
            if (((IComparable)minimum).CompareTo((IComparable)maximum) > 0)
            {
                throw new ArgumentOutOfRangeException(nameof(minimum), $"Parameter {nameof(minimum)} must be lower or equal to parameter {nameof(maximum)}.");
            }

            this.Minimum = minimum;
            this.Maximum = maximum;
        }

        public object Minimum { get; }

        public object Maximum { get; }

        public override ICustomization GetCustomization(ParameterInfo parameter)
        {
            return new FilteringSpecimenBuilder(
                new RequestFactoryRelay((type) => new RangedNumberRequest(type, this.Minimum, this.Maximum)),
                new EqualRequestSpecification(parameter))
                .ToCustomization();
        }
    }
}
