namespace Objectivity.AutoFixture.XUnit2.Core.Tests.Customizations
{
    using System;

    using global::AutoFixture;
    using global::AutoFixture.Xunit2;
    using Objectivity.AutoFixture.XUnit2.Core.Customizations;

    using Xunit;

    [Collection("IgnoreVirtualMembersCustomization")]
    [Trait("Category", "Customizations")]
    public class IgnoreVirtualMembersCustomizationTests
    {
        [AutoData]
        [Theory(DisplayName = "GIVEN existing customization to ignore virtual members for fixture WHEN Customize is invoked THEN fixture should not create virtual members")]
        public void GivenExistingCustomizationToIgnoreVirtualMembersForFixture_WhenCustomizeIsInvoked_ThenFixtureShouldNotCreateVirtualMembers(
            Fixture fixture,
            [Modest] IgnoreVirtualMembersCustomization customization)
        {
            // Arrange
            // Act
            fixture.Customize(customization);

            // Assert
            Assert.Multiple(fixture.ShouldIgnoreVirtualMembers);
        }

        [AutoData]
        [Theory(DisplayName = "GIVEN existing customization to ignore virtual members for fixture WHEN Customize is invoked THEN fixture should not create virtual members")]
        public void GivenExistingCustomizationToIgnoreVirtualMembersWithTypeForFixture_WhenCustomizeIsInvoked_ThenFixtureShouldNotCreateVirtualMembers(
            Fixture fixture,
            [Frozen] Type reflectedType,
            [Greedy] IgnoreVirtualMembersCustomization customization)
        {
            // Arrange
            // Act
            fixture.Customize(customization);

            // Assert
            Assert.Multiple(() => fixture.ShouldIgnoreVirtualMembers(reflectedType));
        }

        [Fact(DisplayName = "GIVEN default constructor WHEN invoked THEN reflected type should be null")]
        public void GivenDefaultConstructor_WhenInvoked_ThenReflectedTypeShouldBeNull()
        {
            // Arrange
            // Act
            var customization = new IgnoreVirtualMembersCustomization();

            // Assert
            Assert.Null(customization.ReflectedType);
        }

        [AutoData]
        [Theory(DisplayName = "GIVEN existing type WHEN constructor with parameter is invoked THEN that type should be reflected")]
        public void GivenExistingType_WhenConstructorWithParameterIsInvoked_ThenThatTypeShouldBeReflected(Type reflectedType)
        {
            // Arrange
            // Act
            var customization = new IgnoreVirtualMembersCustomization(reflectedType);

            // Assert
            Assert.Same(reflectedType, customization.ReflectedType);
        }
    }
}
