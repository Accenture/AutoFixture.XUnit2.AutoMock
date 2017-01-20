namespace Objectivity.AutoFixture.XUnit2.AutoMoq.Tests
{
    using System;
    using Ploeh.AutoFixture;
    using Xunit;

    public class AutoMoqDataAttributeTests
    {
        [Fact]
        public void WhenParameterlessConstructorInvoked_ThenAppropriateFixtureIsCreatedAndConfigured()
        {
            // Arrange
            // Act
            var attribute = new AutoMoqDataAttribute();

            // Assert
            attribute.Fixture.ShouldBeConfigured();
            attribute.Fixture.ShouldNotIgnoreVirtualMembers();
        }

        [Fact]
        public void WhenConstructorInvokedWithIgnoreVirtualMembersSetToTrue_ThenAppropriateFixtureIsCreatedAndConfigured()
        {
            // Arrange
            // Act
            var attribute = new AutoMoqDataAttribute(true);

            // Assert
            attribute.Fixture.ShouldBeConfigured();
            attribute.Fixture.ShouldIgnoreVirtualMembers();
        }

        [Fact]
        public void GivenExistingFixture_WhenConstructorInvoked_ThenSpecifiedFixtureIsConfigured()
        {
            // Arrange
            var fixture = new Fixture();

            // Act
            var attribute = new AutoMoqDataAttribute(fixture);

            // Assert
            attribute.Fixture.ShouldBeConfigured();
            attribute.Fixture.ShouldNotIgnoreVirtualMembers();
        }

        [Fact]
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

        [Fact]
        public void GivenUninitializedFixture_WhenConstructorInvoked_ThenExceptionShouldBeThrown()
        {
            // Arrange
            const Fixture fixture = null;

            // Act
            // Assert
            Assert.Throws<ArgumentNullException>(() => new AutoMoqDataAttribute(fixture));
        }
    }
}
