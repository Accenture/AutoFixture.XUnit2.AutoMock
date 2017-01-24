namespace Objectivity.AutoFixture.XUnit2.AutoMoq.Tests.Customizations
{
    using AutoMoq.Customizations;
    using Ploeh.AutoFixture;
    using Ploeh.AutoFixture.Xunit2;
    using Xunit;

    [Collection("IgnoreVirtualMembersCustomization")]
    [Trait("Category", "Customizations")]
    public class IgnoreVirtualMembersCustomizationTests
    {
        [Theory(DisplayName = "GIVEN existing customization for fixture with ignore virtual members set to true WHEN Customize is invoked THEN fixture should not create virtual members")]
        [AutoData]
        public void GivenExistingCustomizationForFixtureWithIgnoreVirtualMembersSetToTrue_WhenCustomizeIsInvoked_ThenFixtureShouldNotCreateVirtualMembers(Fixture fixture)
        {
            // Arrange
            var customization = new IgnoreVirtualMembersCustomization(ignoreVirtualMembers: true);

            // Act
            fixture.Customize(customization);

            // Assert
            fixture.ShouldIgnoreVirtualMembers();
        }

        [Theory(DisplayName = "GIVEN existing customization for fixture with ignore virtual members set to false WHEN Customize is invoked THEN fixture should create virtual members")]
        [AutoData]
        public void GivenExistingCustomizationForFixtureWithIgnoreVirtualMembersSetToFalse_WhenCustomizeIsInvoked_ThenFixtureShouldCreateVirtualMembers(Fixture fixture)
        {
            // Arrange
            var customization = new IgnoreVirtualMembersCustomization(ignoreVirtualMembers: false);

            // Act
            fixture.Customize(customization);

            // Assert
            fixture.ShouldNotIgnoreVirtualMembers();
        }
    }
}