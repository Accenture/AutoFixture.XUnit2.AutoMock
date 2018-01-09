namespace Objectivity.AutoFixture.XUnit2.Core.Tests.Customizations
{
    using Objectivity.AutoFixture.XUnit2.Core.Customizations;
    using Ploeh.AutoFixture;
    using Ploeh.AutoFixture.Xunit2;
    using Xunit;

    [Collection("AutoDataCommonCustomization")]
    [Trait("Category", "Customizations")]
    public class AutoDataCommonCustomizationTests
    {
        [Theory(DisplayName = "GIVEN existing customization with ignoring virtual members WHEN Customize is invoked THEN fixture is appropriately customized")]
        [AutoData]
        public void GivenExistingCustomizationForFixture_WhenCustomizeIsInvoked_ThenFixtureIsAppropriatelyCustomized(Fixture fixture)
        {
            // Arrange
            const bool ignoreVirtualMembers = true;
            var customization = new AutoDataCommonCustomization(ignoreVirtualMembers);

            // Act
            fixture.Customize(customization);

            // Assert
            fixture.ShouldNotThrowOnRecursion();
            fixture.ShouldOmitRecursion();
            fixture.ShouldIgnoreVirtualMembers();
        }

        [Theory(DisplayName = "GIVEN existing customization without ignoring virtual members WHEN Customize is invoked THEN fixture is appropriately customized")]
        [AutoData]
        public void GivenExistingCustomizationWithoutIgnoringVirtualMembers_WhenCustomizeIsInvoked_ThenFixtureIsAppropriatelyCustomized(Fixture fixture)
        {
            // Arrange
            const bool ignoreVirtualMembers = false;
            var customization = new AutoDataCommonCustomization(ignoreVirtualMembers);

            // Act
            fixture.Customize(customization);

            // Assert
            fixture.ShouldNotThrowOnRecursion();
            fixture.ShouldOmitRecursion();
            fixture.ShouldNotIgnoreVirtualMembers();
        }
    }
}
