namespace Objectivity.AutoFixture.XUnit2.Core.Customizations
{
    using Common;
    using global::AutoFixture;
    using SpecimenBuilders;

    public class IgnoreVirtualMembersCustomization : ICustomization
    {
        public void Customize(IFixture fixture)
        {
            fixture.NotNull(nameof(fixture)).Customizations.Add(new IgnoreVirtualMembersSpecimenBuilder());
        }
    }
}