namespace Objectivity.AutoFixture.XUnit2.AutoMoq.Tests.Customizations
{
    using AutoMoq.Customizations;
    using Ploeh.AutoFixture;
    using Xunit;

    [Collection("AutoMoqDataCustomization")]
    [Trait("Category", "Customizations")]
    public class AutoMoqDataCustomizationTests
    {
        [Fact(DisplayName = "GIVEN existing customization for fixture WHEN Customize is invoked THEN fixture is appropriately customized")]
        public void GivenExistingCustomizationForFixture_WhenCustomizeIsInvoked_ThenFixtureIsAppropriatelyCustomized()
        {
            // Arrange
            var fixture = new Fixture();
            var customization = new AutoMoqDataCustomization();

            // Act
            fixture.Customize(customization);

            // Assert
            fixture.ShouldBeAutoMoqCustomized();
            fixture.ShouldNotThrowOnRecursion();
            fixture.ShouldOmitRecursion();
        }
    }
}
