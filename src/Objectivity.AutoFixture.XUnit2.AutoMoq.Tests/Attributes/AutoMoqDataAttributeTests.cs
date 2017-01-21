namespace Objectivity.AutoFixture.XUnit2.AutoMoq.Tests.Attributes
{
    using System;
    using AutoMoq.Attributes;
    using Ploeh.AutoFixture;
    using Xunit;

    [Collection("AutoMoqDataAttribute")]
    [Trait("Category", "Attributes")]
    public class AutoMoqDataAttributeTests
    {
        [Fact(DisplayName = "WHEN parameterless constructor is invoked THEN appropriate fixture is created and configured")]
        public void WhenParameterlessConstructorIsInvoked_ThenAppropriateFixtureIsCreatedAndConfigured()
        {
            // Arrange
            // Act
            var attribute = new AutoMoqDataAttribute();

            // Assert
            attribute.Fixture.ShouldBeConfigured();
            attribute.Fixture.ShouldNotIgnoreVirtualMembers();
        }

        [Fact(DisplayName = "GIVEN existing fixture WHEN constructor is invoked THEN specified fixture is configured")]
        public void GivenExistingFixture_WhenConstructorIsInvoked_ThenSpecifiedFixtureIsConfigured()
        {
            // Arrange
            var fixture = new Fixture();

            // Act
            var attribute = new AutoMoqDataAttribute(fixture);

            // Assert
            attribute.Fixture.ShouldBeConfigured();
            attribute.Fixture.ShouldNotIgnoreVirtualMembers();
        }

        [Fact(DisplayName = "GIVEN existing fixture and ignore virtual members set to true WHEN constructor is invoked THEN specified fixture is configured")]
        public void GivenExistingFixtureAndIgnoreVirtualMembersSetToTrue_WhenConstructorInvoked_ThenSpecifiedFixtureIsConfigured()
        {
            // Arrange
            var fixture = new Fixture();

            // Act
            var attribute = new AutoMoqDataAttribute(fixture, true);

            // Assert
            attribute.Fixture.ShouldBeConfigured();
            attribute.Fixture.ShouldIgnoreVirtualMembers();
        }

        [Fact(DisplayName = "GIVEN uninitialized fixture WHEN constructor is invoked THEN exception is thrown")]
        public void GivenUninitializedFixture_WhenConstructorIsInvoked_ThenExceptionIsThrown()
        {
            // Arrange
            const Fixture fixture = null;

            // Act
            // Assert
            Assert.Throws<ArgumentNullException>(() => new AutoMoqDataAttribute(fixture));
        }
    }
}
