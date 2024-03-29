﻿namespace Objectivity.AutoFixture.XUnit2.Core.Tests.Customizations
{
    using global::AutoFixture;
    using global::AutoFixture.Xunit2;
    using Objectivity.AutoFixture.XUnit2.Core.Customizations;

    using Xunit;

    [Collection("AutoDataCommonCustomization")]
    [Trait("Category", "Customizations")]
    public class AutoDataCommonCustomizationTests
    {
        [AutoData]
        [Theory(DisplayName = "GIVEN existing customization with ignoring virtual members WHEN Customize is invoked THEN fixture is appropriately customized")]
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

        [AutoData]
        [Theory(DisplayName = "GIVEN existing customization without ignoring virtual members WHEN Customize is invoked THEN fixture is appropriately customized")]
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
