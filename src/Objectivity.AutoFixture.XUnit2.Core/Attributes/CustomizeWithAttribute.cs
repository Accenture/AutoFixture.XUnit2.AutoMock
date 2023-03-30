namespace Objectivity.AutoFixture.XUnit2.Core.Attributes
{
    using System;
    using System.Diagnostics.CodeAnalysis;
    using System.Linq;
    using System.Reflection;

    using global::AutoFixture;
    using global::AutoFixture.Xunit2;
    using Objectivity.AutoFixture.XUnit2.Core.Common;

    [AttributeUsage(AttributeTargets.Parameter, AllowMultiple = true)]
    [SuppressMessage("Performance", "CA1813:Avoid unsealed attributes", Justification = "This attribute should be extendable by inheritance.")]
    public class CustomizeWithAttribute : CustomizeAttribute
    {
        public CustomizeWithAttribute(Type type, params object[] args)
        {
            this.Type = type.NotNull(nameof(type));

            var customizationType = typeof(ICustomization);
            if (!customizationType.IsAssignableFrom(type))
            {
                throw new ArgumentException($"Specified argument {nameof(type)} must implement {customizationType.Name}");
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
            var args = this.IncludeParameterType
                ? new object[] { parameter.NotNull(nameof(parameter)).ParameterType }
                    .Concat(this.Args ?? Array.Empty<object>())
                    .ToArray()
                : this.Args;

            return Activator.CreateInstance(this.Type, args) as ICustomization;
        }
    }
}
