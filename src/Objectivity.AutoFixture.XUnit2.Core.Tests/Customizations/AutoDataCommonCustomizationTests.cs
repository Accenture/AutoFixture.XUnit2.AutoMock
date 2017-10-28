namespace Objectivity.AutoFixture.XUnit2.Core.Tests.Customizations
{
    using Objectivity.AutoFixture.XUnit2.Core.Customizations;
    using Ploeh.AutoFixture;
    using Ploeh.AutoFixture.Xunit2;
    using Xunit;

    [Collection("AutoMoqDataCustomization")]
    [Trait("Category", "Customizations")]
    public class AutoMoqDataCustomizationTests
    {
        [Theory(DisplayName = "GIVEN existing customization for fixture WHEN Customize is invoked THEN fixture is appropriately customized")]
        [AutoData]
        public void GivenExistingCustomizationForFixture_WhenCustomizeIsInvoked_ThenFixtureIsAppropriatelyCustomized(Fixture fixture, AutoDataCommonCustomization customization)
        {
            // Arrange
            // Act
            fixture.Customize(customization);

            // Assert
            fixture.ShouldNotThrowOnRecursion();
            fixture.ShouldOmitRecursion();
        }
    }
}
