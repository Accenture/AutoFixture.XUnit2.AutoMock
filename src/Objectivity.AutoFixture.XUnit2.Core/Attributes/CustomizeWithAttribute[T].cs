namespace Objectivity.AutoFixture.XUnit2.Core.Attributes
{
    using System;
    using System.Diagnostics.CodeAnalysis;

    using global::AutoFixture;

    [AttributeUsage(AttributeTargets.Parameter, AllowMultiple = false)]
    [SuppressMessage("Performance", "CA1813:Avoid unsealed attributes", Justification = "This attribute should be extendable by inheritance.")]
    public class CustomizeWithAttribute<T> : CustomizeWithAttribute
        where T : ICustomization
    {
        [SuppressMessage("ReSharper", "MemberCanBeProtected.Global", Justification = "This attribute should be used directly.")]
        public CustomizeWithAttribute(params object[] args)
            : base(typeof(T), args)
        {
        }
    }
}
