namespace Objectivity.AutoFixture.XUnit2.Core.Attributes
{
    using System;
    using System.Diagnostics.CodeAnalysis;
    using System.Reflection;

    using global::AutoFixture;
    using global::AutoFixture.Xunit2;

    using Objectivity.AutoFixture.XUnit2.Core.Common;
    using Objectivity.AutoFixture.XUnit2.Core.CustomisationFactories;

    [AttributeUsage(AttributeTargets.Parameter, AllowMultiple = true)]
    [SuppressMessage("Performance", "CA1813:Avoid unsealed attributes", Justification = "This attribute should be extendable by inheritance.")]
    public class CustomizeWithAttribute : CustomizeAttribute
    {
        private readonly ICustomisationFactoryProvider factoryProvider = new CustomisationFactoryProvider();

        public CustomizeWithAttribute(Type type, params object[] args)
        {
            this.Type = type.NotNull(nameof(type));
            this.Args = args;
        }

        public Type Type { get; }

        public object[] Args { get; }

        /// <summary>
        /// Gets or sets a value indicating whether attribute target parameter type should included as a first argument when creating customization.
        /// </summary>
        /// <value>Indicates whether attribute target parameter type should included as a first argument when creating customization.</value>
        public bool IncludeParameterType { get; set; }

        public override ICustomization GetCustomization(ParameterInfo parameter)
        {
            var factory = this.factoryProvider.GetFactory(this.Type);

            return factory.Create(parameter, this.IncludeParameterType, this.Type, this.Args);
        }
    }
}
