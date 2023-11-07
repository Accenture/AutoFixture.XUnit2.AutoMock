﻿namespace Objectivity.AutoFixture.XUnit2.Core.Attributes
{
    using System;
    using System.Linq;
    using System.Reflection;

    using global::AutoFixture;
    using global::AutoFixture.Xunit2;

    using Objectivity.AutoFixture.XUnit2.Core.Common;
    using Objectivity.AutoFixture.XUnit2.Core.CustomisationFactories;

    public abstract class CustomizeAdapterAttribute : CustomizeAttribute
    {
        private readonly ICustomisationFactoryProvider factoryProvider = new CustomisationFactoryProvider();

        protected CustomizeAdapterAttribute(Type type, params object[] args)
        {
            this.Type = type.NotNull(nameof(type));

            if (!this.factoryProvider.SupportedTypes.Any(x => x.IsAssignableFrom(this.Type)))
            {
                var supportedTypes = string.Join(", ", this.factoryProvider.SupportedTypes.Select(x => x.Name));
                var message = $"Specified argument {nameof(type)} must implement one of supported types: {supportedTypes}.";
                throw new ArgumentException(message);
            }

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