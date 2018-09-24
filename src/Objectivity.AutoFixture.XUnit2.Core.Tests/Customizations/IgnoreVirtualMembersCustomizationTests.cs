namespace Objectivity.AutoFixture.XUnit2.Core.Tests.Customizations
{
    using System;
    using FluentAssertions;
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

        [Fact(DisplayName = "GIVEN default constructor WHEN invoked THEN reflected type should be null")]
        public void GivenDefaultConstructor_WhenInvoked_ThenReflectedTypeShouldBeNull()
        {
            // Arrange
            // Act
            var customization = new IgnoreVirtualMembersCustomization();

            // Assert
            customization.ReflectedType.Should().BeNull();
        }

        [Theory(DisplayName = "GIVEN existing type WHEN constructor with parameter is invoked THEN that type should be reflected")]
        [AutoData]
        public void GivenExistingType_WhenConstructorWithParameterIsInvoked_ThenThatTypeShouldBeReflected(Type reflectedType)
        {
            // Arrange
            // Act
            var customization = new IgnoreVirtualMembersCustomization(reflectedType);

            // Assert
            customization.ReflectedType.Should().BeSameAs(reflectedType);
        }
    }
}