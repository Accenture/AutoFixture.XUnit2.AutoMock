namespace Objectivity.AutoFixture.XUnit2.AutoMoq.Fixture
{
    using Common;
    using Customizations;
    using Ploeh.AutoFixture;

    public static class FixtureExtensions
    {
        public static IFixture ApplyCustomizations(this IFixture fixture, bool ignoreVirtualMembers)
        {
            return fixture.NotNull(nameof(fixture))
                .Customize(new AutoMoqDataCustomization())
                .Customize(new IgnoreVirtualMembersCustomization(ignoreVirtualMembers));
        }
    }
}
