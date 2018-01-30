namespace Objectivity.AutoFixture.XUnit2.Core.Attributes
{
    using global::AutoFixture;
    using global::AutoFixture.Xunit2;
    using Objectivity.AutoFixture.XUnit2.Core.Common;

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
