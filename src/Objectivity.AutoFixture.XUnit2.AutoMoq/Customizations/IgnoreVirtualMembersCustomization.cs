namespace Objectivity.AutoFixture.XUnit2.AutoMoq.Customizations
{
    using Common;
    using Ploeh.AutoFixture;
    using SpecimenBuilders;

    public class IgnoreVirtualMembersCustomization : ICustomization
    {
        public void Customize(IFixture fixture)
        {
            fixture.NotNull(nameof(fixture)).Customizations.Add(new IgnoreVirtualMembersSpecimenBuilder());
        }
    }
}