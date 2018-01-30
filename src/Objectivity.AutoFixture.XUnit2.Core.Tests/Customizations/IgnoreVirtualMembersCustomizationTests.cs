namespace Objectivity.AutoFixture.XUnit2.Core.Tests.Customizations
{
    using global::AutoFixture;
    using global::AutoFixture.Xunit2;
    using Objectivity.AutoFixture.XUnit2.Core.Customizations;
    using Xunit;

    [Collection("IgnoreVirtualMembersCustomization")]
    [Trait("Category", "Customizations")]
    public class IgnoreVirtualMembersCustomizationTests
    {
        [Theory(DisplayName = "GIVEN existing customization to ignore virtual members for fixture WHEN Customize is invoked THEN fixture should not create virtual members")]
        [AutoData]
        public void GivenExistingCustomizationToIgnoreVirtualMembersForFixture_WhenCustomizeIsInvoked_ThenFixtureShouldNotCreateVirtualMembers(Fixture fixture, IgnoreVirtualMembersCustomization customization)
        {
            // Arrange
            // Act
            fixture.Customize(customization);

            // Assert
            fixture.ShouldIgnoreVirtualMembers();
        }
    }
}