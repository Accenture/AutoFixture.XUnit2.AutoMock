namespace Objectivity.AutoFixture.XUnit2.Core.CustomisationFactories
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    using global::AutoFixture;
    using global::AutoFixture.Kernel;

    internal class CustomisationFactoryProvider : ICustomisationFactoryProvider
    {
        private readonly Dictionary<Type, Lazy<ICustomisationFactory>> factories = new()
        {
            {
                typeof(ICustomization),
                new Lazy<ICustomisationFactory>(() => new CustomisationFactory())
            },
            {
                typeof(ISpecimenBuilder),
                new Lazy<ICustomisationFactory>(() => new SpecimenBuilderCustomisationFactory())
            },
        };

        public ICustomisationFactory GetFactory(Type type)
        {
            var factoryPair = this.factories.First(x => x.Key.IsAssignableFrom(type));

            return factoryPair.Value.Value;
        }
    }
}
