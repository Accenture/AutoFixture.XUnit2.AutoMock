namespace Objectivity.AutoFixture.XUnit2.Core.CustomisationFactories
{
    using System;

    internal interface ICustomisationFactoryProvider
    {
        ICustomisationFactory GetFactory(Type type);
    }
}
