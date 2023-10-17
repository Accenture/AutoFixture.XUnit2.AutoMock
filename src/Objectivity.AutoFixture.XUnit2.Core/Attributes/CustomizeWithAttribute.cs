namespace Objectivity.AutoFixture.XUnit2.Core.Attributes
{
    using System;
    using System.Diagnostics.CodeAnalysis;
    using System.Reflection;

    using global::AutoFixture;

    [AttributeUsage(AttributeTargets.Parameter, AllowMultiple = true)]
    [SuppressMessage("Performance", "CA1813:Avoid unsealed attributes", Justification = "This attribute should be extendable by inheritance.")]
    public class CustomizeWithAttribute : CustomizeAdapterAttribute<ICustomization>
    {
        public CustomizeWithAttribute(Type type, params object[] args)
            : base(type, args)
        {
        }

        public override ICustomization GetCustomization(ParameterInfo parameter)
        {
            return this.CreateInstance(parameter) as ICustomization;
        }
    }
}
