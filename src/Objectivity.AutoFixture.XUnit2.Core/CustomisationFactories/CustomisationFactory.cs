namespace Objectivity.AutoFixture.XUnit2.Core.CustomisationFactories
{
    using System;
    using System.Linq;
    using System.Reflection;

    using global::AutoFixture;

    using Objectivity.AutoFixture.XUnit2.Core.Common;

    internal class CustomisationFactory : ICustomisationFactory
    {
        public ICustomization Create(ParameterInfo parameter, bool includeParameterType, Type type, params object[] args)
        {
            var constructorArgs = includeParameterType
                ? new object[] { parameter.NotNull(nameof(parameter)).ParameterType }
                    .Concat(args ?? Array.Empty<object>())
                    .ToArray()
                : args;

            return Activator.CreateInstance(type, constructorArgs) as ICustomization;
        }
    }
}
