namespace Objectivity.AutoFixture.XUnit2.Core.Attributes
{
    using System;
    using System.Diagnostics.CodeAnalysis;
    using System.Linq;
    using System.Reflection;

    using global::AutoFixture;
    using global::AutoFixture.Xunit2;

    [AttributeUsage(AttributeTargets.Parameter, AllowMultiple = true)]
    [SuppressMessage("Performance", "CA1813:Avoid unsealed attributes", Justification = "This attribute should be extendable by inheritance.")]
    public class CustomizeWithAttribute : CustomizeAttribute
    {
        public CustomizeWithAttribute(Type type, params object[] args)
        {
            if (type is null)
            {
                throw new ArgumentNullException(nameof(type));
            }

            var customizationType = typeof(ICustomization);
            if (!customizationType.IsAssignableFrom(type))
            {
                throw new ArgumentException($"Specified argument {nameof(type)} must implement {customizationType.Name}");
            }

            this.Type = type;
            this.Args = args;
        }

        public Type Type { get; }

        public object[] Args { get; }

        public bool IncludeParameterType { get; set; }

        public override ICustomization GetCustomization(ParameterInfo parameter)
        {
            var args = this.Args;
            if (this.IncludeParameterType)
            {
                if (parameter is null)
                {
                    throw new ArgumentNullException(nameof(parameter));
                }

                args = new object[] { parameter.ParameterType }
                    .Concat(this.Args ?? Array.Empty<object>())
                    .ToArray();
            }

            return Activator.CreateInstance(this.Type, args) as ICustomization;
        }
    }
}
