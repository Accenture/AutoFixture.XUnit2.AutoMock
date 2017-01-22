namespace Objectivity.AutoFixture.XUnit2.AutoMoq.Tests.Customizations
{
    using AutoMoq.Customizations;
    using Ploeh.AutoFixture;
    using Xunit;

    [Collection("DoNotThrowOnRecursionCustomization")]
    [Trait("Category", "Customizations")]
    public class DoNotThrowOnRecursionCustomizationTests
    {
        [Fact(DisplayName = "GIVEN existing customization for fixture WHEN Customize is invoked THEN fixture should not throw on recursion")]
        public void GivenExistingCustomizationForFixture_WhenCustomizeIsInvoked_ThenFixtureShouldNotThrowOnRecursion()
        {
            // Arrange
            var fixture = new Fixture();
            var customization = new DoNotThrowOnRecursionCustomization();

            // Act
            fixture.Customize(customization);

            // Assert
            fixture.ShouldNotThrowOnRecursion();
        }
    }
}