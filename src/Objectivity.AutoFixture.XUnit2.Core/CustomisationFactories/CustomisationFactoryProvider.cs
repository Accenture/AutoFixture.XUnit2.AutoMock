namespace Objectivity.AutoFixture.XUnit2.Core.CustomisationFactories
{
    using System;
    using System.Collections.Generic;
    using System.Collections.ObjectModel;
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

        public ReadOnlyCollection<Type> SupportedTypes => new(this.factories.Keys.ToList());

        public ICustomisationFactory GetFactory(this Type type)
        {
            return this.factories.First(x => x.Key.IsAssignableFrom(type)).Value.Value;
        }
    }
}
