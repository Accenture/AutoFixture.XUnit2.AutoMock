namespace Objectivity.AutoFixture.XUnit2.Core.Customizations
{
    using Common;
    using Ploeh.AutoFixture;
    using SpecimenBuilders;

    public class IgnoreVirtualMembersCustomization : ICustomization
    {
        public IgnoreVirtualMembersCustomization(bool ignoreVirtualMembers)
        {
            this.IgnoreVirtualMembers = ignoreVirtualMembers;
        }

        public bool IgnoreVirtualMembers { get; }

        public void Customize(IFixture fixture)
        {
            if (this.IgnoreVirtualMembers)
            {
                fixture.NotNull(nameof(fixture)).Customizations.Add(new IgnoreVirtualMembersSpecimenBuilder());
            }
        }
    }
}