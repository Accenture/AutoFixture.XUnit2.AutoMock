namespace Objectivity.AutoFixture.XUnit2.Core.Attributes
{
    using System;
    using global::AutoFixture;
    using global::AutoFixture.Xunit2;
    using Objectivity.AutoFixture.XUnit2.Core.Common;

    [AttributeUsage(AttributeTargets.Method, AllowMultiple = false, Inherited = true)]
    internal sealed class AutoDataAdapterAttribute : AutoDataAttribute
    {
        public AutoDataAdapterAttribute(IFixture fixture)
            : base(() => fixture)
        {
            this.AdaptedFixture = fixture.NotNull(nameof(fixture));
        }

        public IFixture AdaptedFixture { get; }
    }
}
