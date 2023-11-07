namespace Objectivity.AutoFixture.XUnit2.Core.Attributes
{
    using System;
    using System.Reflection;

    using global::AutoFixture;
    using global::AutoFixture.Xunit2;
    using Objectivity.AutoFixture.XUnit2.Core.CustomisationFactories;
    using Objectivity.AutoFixture.XUnit2.Core.SpecimenBuilders;

    [AttributeUsage(AttributeTargets.Parameter, AllowMultiple = true)]
    public sealed class FromValuesAttribute : CustomizeAttribute
    {
        private readonly ICustomisationFactoryProvider factoryProvider = new CustomisationFactoryProvider();

        public FromValuesAttribute(params object[] args)
        {
            this.Args = args ?? Array.Empty<object>();
        }

        public object[] Args { get; }

        public override ICustomization GetCustomization(ParameterInfo parameter)
        {
            var type = typeof(RandomValuesParameterBuilder);
            var factory = this.factoryProvider.GetFactory(type);

            return factory.Create(parameter, false, type, this.Args);
        }
    }
}
