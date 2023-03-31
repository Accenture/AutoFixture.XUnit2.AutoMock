namespace Objectivity.AutoFixture.XUnit2.Core.Attributes
{
    using System;
    using System.Diagnostics.CodeAnalysis;

    using global::AutoFixture;

    [AttributeUsage(AttributeTargets.Parameter, AllowMultiple = true)]
    [SuppressMessage("Performance", "CA1813:Avoid unsealed attributes", Justification = "This attribute should be extendable by inheritance.")]
    public class CustomizeWithAttribute<T> : CustomizeWithAttribute
        where T : ICustomization
    {
        public CustomizeWithAttribute(params object[] args)
            : base(typeof(T), args)
        {
        }
    }
}
