namespace Objectivity.AutoFixture.XUnit2.Core.Attributes
{
    using System;
    using System.Reflection;

    using global::AutoFixture;
    using global::AutoFixture.Kernel;
    using global::AutoFixture.Xunit2;
    using Objectivity.AutoFixture.XUnit2.Core.Factories;
    using Objectivity.AutoFixture.XUnit2.Core.SpecimenBuilders;

    [AttributeUsage(AttributeTargets.Parameter, AllowMultiple = false)]
    public sealed class PickNegativeAttribute : CustomizeAttribute
    {
        private readonly NegativeValuesRequestFactory negativeValuesRequestFactory = new();

        public override ICustomization GetCustomization(ParameterInfo parameter)
        {
            if (parameter is null)
            {
                throw new ArgumentNullException(nameof(parameter));
            }

            return new CompositeSpecimenBuilder(
                new FilteringSpecimenBuilder(
                    new RequestFactoryRelay(this.negativeValuesRequestFactory.Create),
                    new EqualRequestSpecification(parameter)),
                new RandomFixedValuesGenerator())
                .ToCustomization();
        }
    }
}
