namespace Objectivity.AutoFixture.XUnit2.Core.Attributes
{
    using System;
    using System.Reflection;

    using global::AutoFixture;
    using global::AutoFixture.Xunit2;
    using Objectivity.AutoFixture.XUnit2.Core.CustomisationFactories;
    using Objectivity.AutoFixture.XUnit2.Core.SpecimenBuilders;

    [AttributeUsage(AttributeTargets.Parameter, AllowMultiple = true)]
    public sealed class FromRangeAttribute : CustomizeAttribute
    {
        private readonly ICustomisationFactoryProvider factoryProvider = new CustomisationFactoryProvider();

        public FromRangeAttribute(sbyte minimum, sbyte maximum)
            : this((object)minimum, maximum)
        {
        }

        public FromRangeAttribute(byte minimum, byte maximum)
            : this((object)minimum, maximum)
        {
        }

        public FromRangeAttribute(short minimum, short maximum)
            : this((object)minimum, maximum)
        {
        }

        public FromRangeAttribute(ushort minimum, ushort maximum)
            : this((object)minimum, maximum)
        {
        }

        public FromRangeAttribute(int minimum, int maximum)
            : this((object)minimum, maximum)
        {
        }

        public FromRangeAttribute(uint minimum, uint maximum)
            : this((object)minimum, maximum)
        {
        }

        public FromRangeAttribute(long minimum, long maximum)
            : this((object)minimum, maximum)
        {
        }

        public FromRangeAttribute(ulong minimum, ulong maximum)
            : this((object)minimum, maximum)
        {
        }

        public FromRangeAttribute(double minimum, double maximum)
            : this((object)minimum, maximum)
        {
        }

        public FromRangeAttribute(float minimum, float maximum)
            : this((object)minimum, maximum)
        {
        }

        private FromRangeAttribute(object minimum, object maximum)
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
            var type = typeof(RandomRangedNumberParameterBuilder);
            var factory = this.factoryProvider.GetFactory(type);
            return factory.Create(parameter, false, type, this.Minimum, this.Maximum);
        }
    }
}
