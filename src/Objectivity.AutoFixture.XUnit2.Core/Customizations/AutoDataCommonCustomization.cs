namespace Objectivity.AutoFixture.XUnit2.Core.Customizations
{
    using global::AutoFixture;

    using Objectivity.AutoFixture.XUnit2.Core.Common;

    public class AutoDataCommonCustomization : ICustomization
    {
        public AutoDataCommonCustomization(bool ignoreVirtualMembers)
        {
            this.IgnoreVirtualMembers = ignoreVirtualMembers;
        }

        public bool IgnoreVirtualMembers { get; }

        public void Customize(IFixture fixture)
        {
            var adaptedFixture = fixture.NotNull(nameof(fixture))
                .Customize(new DoNotThrowOnRecursionCustomization())
                .Customize(new OmitOnRecursionCustomization());

            if (this.IgnoreVirtualMembers)
            {
                adaptedFixture.Customize(new IgnoreVirtualMembersCustomization());
            }
        }
    }
}
