namespace Objectivity.AutoFixture.XUnit2.Core.CustomisationFactories
{
    using System;
    using System.Collections.ObjectModel;

    internal interface ICustomisationFactoryProvider
    {
        ReadOnlyCollection<Type> SupportedTypes { get; }

        ICustomisationFactory GetFactory(this Type type);
    }
}
