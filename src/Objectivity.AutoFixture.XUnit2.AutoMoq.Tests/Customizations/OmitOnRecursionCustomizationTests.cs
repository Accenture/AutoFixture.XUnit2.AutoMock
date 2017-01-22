namespace Objectivity.AutoFixture.XUnit2.AutoMoq.Tests.Customizations
{
    using AutoMoq.Customizations;
    using Ploeh.AutoFixture;
    using Xunit;

    [Collection("OmitOnRecursionCustomization")]
    [Trait("Category", "Customizations")]
    public class OmitOnRecursionCustomizationTests
    {
        [Fact(DisplayName = "GIVEN existing customization for fixture WHEN Customize is invoked THEN fixture should omit recursion")]
        public void GivenExistingCustomizationForFixture_WhenCustomizeIsInvoked_ThenFixtureShouldOmitRecursion()
        {
            // Arrange
            var fixture = new Fixture();
            var customization = new OmitOnRecursionCustomization();

            // Act
            fixture.Customize(customization);

            // Assert
            fixture.ShouldOmitRecursion();
        }
    }
}