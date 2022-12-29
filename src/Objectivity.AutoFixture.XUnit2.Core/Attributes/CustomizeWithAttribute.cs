namespace Objectivity.AutoFixture.XUnit2.Core.Attributes
{
    using System;
    using System.Reflection;

    using global::AutoFixture;
    using global::AutoFixture.Xunit2;

    [AttributeUsage(AttributeTargets.Parameter, AllowMultiple = true)]
    public sealed class CustomizeWithAttribute : CustomizeAttribute
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

        public override ICustomization GetCustomization(ParameterInfo parameter)
        {
            return Activator.CreateInstance(this.Type, this.Args) as ICustomization;
        }
    }
}
