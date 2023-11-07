namespace Objectivity.AutoFixture.XUnit2.Core.Attributes
{
    using System;
    using System.Diagnostics.CodeAnalysis;

    [AttributeUsage(AttributeTargets.Parameter, AllowMultiple = true)]
    [SuppressMessage("Performance", "CA1813:Avoid unsealed attributes", Justification = "This attribute should be extendable by inheritance.")]
    public class CustomizeWithAttribute : CustomizeAdapterAttribute
    {
        public CustomizeWithAttribute(Type type, params object[] args)
            : base(type, args)
        {
        }
    }
}
