namespace Objectivity.AutoFixture.XUnit2.AutoMoq.Tests.Customizations
{
    using AutoMoq.Customizations;
    using Ploeh.AutoFixture;
    using Xunit;

    [Collection("IgnoreVirtualMembersCustomization")]
    [Trait("Category", "Customizations")]
    public class IgnoreVirtualMembersCustomizationTests
    {
        [Fact(DisplayName = "GIVEN existing customization for fixture with ignore virtual members set to true WHEN Customize is invoked THEN fixture should not create virtual members")]
        public void GivenExistingCustomizationForFixtureWithIgnoreVirtualMembersSetToTrue_WhenCustomizeIsInvoked_ThenFixtureShouldNotCreateVirtualMembers()
        {
            // Arrange
            var fixture = new Fixture();
            var customization = new IgnoreVirtualMembersCustomization(ignoreVirtualMembers: true);

            // Act
            fixture.Customize(customization);

            // Assert
            fixture.ShouldIgnoreVirtualMembers();
        }

        [Fact(DisplayName = "GIVEN existing customization for fixture with ignore virtual members set to false WHEN Customize is invoked THEN fixture should create virtual members")]
        public void GivenExistingCustomizationForFixtureWithIgnoreVirtualMembersSetToFalse_WhenCustomizeIsInvoked_ThenFixtureShouldCreateVirtualMembers()
        {
            // Arrange
            var fixture = new Fixture();
            var customization = new IgnoreVirtualMembersCustomization(ignoreVirtualMembers: false);

            // Act
            fixture.Customize(customization);

            // Assert
            fixture.ShouldNotIgnoreVirtualMembers();
        }
    }
}