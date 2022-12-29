namespace Objectivity.AutoFixture.XUnit2.Core.Tests.Customizations
{
    using global::AutoFixture;
    using global::AutoFixture.Xunit2;
    using Objectivity.AutoFixture.XUnit2.Core.Customizations;

    using Xunit;

    [Collection("DoNotThrowOnRecursionCustomization")]
    [Trait("Category", "Customizations")]
    public class DoNotThrowOnRecursionCustomizationTests
    {
        [Theory(DisplayName = "GIVEN existing customization for fixture WHEN Customize is invoked THEN fixture should not throw on recursion")]
        [AutoData]
        public void GivenExistingCustomizationForFixture_WhenCustomizeIsInvoked_ThenFixtureShouldNotThrowOnRecursion(Fixture fixture, DoNotThrowOnRecursionCustomization customization)
        {
            // Arrange
            // Act
            fixture.Customize(customization);

            // Assert
            fixture.ShouldNotThrowOnRecursion();
        }
    }
}
