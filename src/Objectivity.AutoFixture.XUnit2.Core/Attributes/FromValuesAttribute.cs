namespace Objectivity.AutoFixture.XUnit2.Core.Attributes
{
    using System;
    using System.Collections.Generic;
    using System.Reflection;

    using global::AutoFixture;
    using global::AutoFixture.Kernel;
    using global::AutoFixture.Xunit2;

    using Objectivity.AutoFixture.XUnit2.Core.Common;
    using Objectivity.AutoFixture.XUnit2.Core.Requests;
    using Objectivity.AutoFixture.XUnit2.Core.SpecimenBuilders;

    [AttributeUsage(AttributeTargets.Parameter, AllowMultiple = true)]
    public sealed class FromValuesAttribute : CustomizeAttribute
    {
        private readonly object[] inputValues;
        private readonly Lazy<IReadOnlyCollection<object>> readonlyValues;

        public FromValuesAttribute(params object[] values)
        {
            this.inputValues = values.NotNull(nameof(values));
            if (this.inputValues.Length == 0)
            {
                throw new ArgumentException("At least one value is expected to be specified.", nameof(values));
            }

            if (Array.Exists(this.inputValues, x => x is not IComparable))
            {
                throw new ArgumentException("All values are expected to be comparable.", nameof(values));
            }

            this.readonlyValues = new Lazy<IReadOnlyCollection<object>>(() => Array.AsReadOnly(this.inputValues));
        }

        public IReadOnlyCollection<object> Values => this.readonlyValues.Value;

        public override ICustomization GetCustomization(ParameterInfo parameter)
        {
            return new CompositeSpecimenBuilder(
                new FilteringSpecimenBuilder(
                    new RequestFactoryRelay((type) => new FixedValuesRequest(type, this.inputValues)),
                    new EqualRequestSpecification(parameter)),
                new RandomFixedValuesGenerator())
                .ToCustomization();
        }
    }
}
