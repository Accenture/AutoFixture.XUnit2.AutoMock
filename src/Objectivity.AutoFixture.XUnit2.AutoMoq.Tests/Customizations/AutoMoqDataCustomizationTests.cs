namespace Objectivity.AutoFixture.XUnit2.AutoMoq.Tests.Customizations
{
    using AutoMoq.Customizations;
    using Ploeh.AutoFixture;
    using Ploeh.AutoFixture.Xunit2;
    using Xunit;

    [Collection("AutoMoqDataCustomization")]
    [Trait("Category", "Customizations")]
    public class AutoMoqDataCustomizationTests
    {
        [Theory(DisplayName = "GIVEN existing customization with ignoring virtual members WHEN Customize is invoked THEN fixture is appropriately customized")]
        [AutoData]
        public void GivenExistingCustomizationWithIgnoringVirtualMembers_WhenCustomizeIsInvoked_ThenFixtureIsAppropriatelyCustomized(Fixture fixture)
        {
            // Arrange
            const bool ignoreVirtualMembers = true;
            var customization = new AutoMoqDataCustomization(ignoreVirtualMembers);

            // Act
            fixture.Customize(customization);

            // Assert
            fixture.ShouldBeAutoMoqCustomized();
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
            var customization = new AutoMoqDataCustomization(ignoreVirtualMembers);

            // Act
            fixture.Customize(customization);

            // Assert
            fixture.ShouldBeAutoMoqCustomized();
            fixture.ShouldNotThrowOnRecursion();
            fixture.ShouldOmitRecursion();
            fixture.ShouldNotIgnoreVirtualMembers();
        }
    }
}
