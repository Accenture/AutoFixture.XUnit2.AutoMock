namespace Objectivity.AutoFixture.XUnit2.AutoMoq.Customizations
{
    using Common;
    using Ploeh.AutoFixture;
    using Ploeh.AutoFixture.AutoMoq;

    public class AutoMoqDataCustomization : ICustomization
    {
        public void Customize(IFixture fixture)
        {
            fixture.NotNull(nameof(fixture))
                .Customize(new AutoConfiguredMoqCustomization())
                .Customize(new DoNotThrowOnRecursionCustomization())
                .Customize(new OmitOnRecursionCustomization());
        }
    }
}
