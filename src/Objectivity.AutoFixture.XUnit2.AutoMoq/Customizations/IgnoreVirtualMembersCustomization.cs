namespace Objectivity.AutoFixture.XUnit2.AutoMoq.Customizations
{
    using Common;
    using Ploeh.AutoFixture;
    using SpecimenBuilder;

    public class IgnoreVirtualMembersCustomization : ICustomization
    {
        private readonly bool ignoreVirtualMembers;

        public IgnoreVirtualMembersCustomization(bool ignoreVirtualMembers)
        {
            this.ignoreVirtualMembers = ignoreVirtualMembers;
        }

        public void Customize(IFixture fixture)
        {
            if (this.ignoreVirtualMembers)
            {
                fixture.NotNull(nameof(fixture)).Customizations.Add(new IgnoreVirtualMembersSpecimenBuilder());
            }
        }
    }
}