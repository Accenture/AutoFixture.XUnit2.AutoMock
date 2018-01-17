namespace Objectivity.AutoFixture.XUnit2.Core.Customizations
{
    using Common;
    using Ploeh.AutoFixture;

    public class AutoDataCommonCustomization : ICustomization
    {
        public AutoDataCommonCustomization(bool ignoreVirtualMembers)
        {
            this.IgnoreVirtualMembers = ignoreVirtualMembers;
        }

        public bool IgnoreVirtualMembers { get; }

        public void Customize(IFixture fixture)
        {
            fixture.NotNull(nameof(fixture))
                .Customize(new DoNotThrowOnRecursionCustomization())
                .Customize(new OmitOnRecursionCustomization());

            if (this.IgnoreVirtualMembers)
            {
                fixture.Customize(new IgnoreVirtualMembersCustomization());
            }
        }
    }
}