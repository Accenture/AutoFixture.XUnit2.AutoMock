namespace Objectivity.AutoFixture.XUnit2.Core.Attributes
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Reflection;

    using global::AutoFixture;
    using global::AutoFixture.Kernel;
    using global::AutoFixture.Xunit2;

    using Objectivity.AutoFixture.XUnit2.Core.Common;
    using Objectivity.AutoFixture.XUnit2.Core.Requests;
    using Objectivity.AutoFixture.XUnit2.Core.SpecimenBuilders;

    [AttributeUsage(AttributeTargets.Parameter, AllowMultiple = false)]
    public sealed class ValuesAttribute : CustomizeAttribute
    {
        private readonly HashSet<object> inputValues;
        private readonly Lazy<IReadOnlyCollection<object>> readonlyValues;

        public ValuesAttribute(params object[] values)
        {
            this.inputValues = new HashSet<object>(values.NotNull(nameof(values)));
            if (this.inputValues.Count == 0)
            {
                throw new ArgumentException("At least one value is expected to be specified.", nameof(values));
            }

            this.readonlyValues = new Lazy<IReadOnlyCollection<object>>(() => Array.AsReadOnly(this.inputValues.ToArray()));
        }

        public IReadOnlyCollection<object> Values => this.readonlyValues.Value;

        public override ICustomization GetCustomization(ParameterInfo parameter)
        {
            return new CompositeSpecimenBuilder(
                new FilteringSpecimenBuilder(
                    new RequestFactoryRelay((type) => new FixedValuesRequest(type, this.inputValues.ToArray())),
                    new EqualRequestSpecification(parameter)),
                new RandomFixedValuesGenerator())
                .ToCustomization();
        }
    }
}
