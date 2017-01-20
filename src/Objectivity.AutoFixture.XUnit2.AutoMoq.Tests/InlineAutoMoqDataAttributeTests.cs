namespace Objectivity.AutoFixture.XUnit2.AutoMoq.Tests
{
    using System;
    using System.Linq;
    using FluentAssertions;
    using Xunit;

    public class InlineAutoMoqDataAttributeTests
    {
        [Fact]
        public void WhenParameterlessConstructorInvoked_ThenHasNoValuesAndAppropriateFixtureIsCreatedAndConfigured()
        {
            // Arrange
            // Act
            var attribute = new InlineAutoMoqDataAttribute();

            // Assert
            attribute.AutoDataAttribute.Should()
                .BeOfType<AutoMoqDataAttribute>()
                .Which.Fixture.ShouldBeConfigured();
            attribute.AutoDataAttribute.Fixture.ShouldNotIgnoreVirtualMembers();
            attribute.Values.Count().Should().Be(0);
        }

        [Fact]
        public void GivenExistingInlineValues_WhenConstructorInvoked_ThenHasSpecifiedValuesAndAppropriateFixtureIsCreatedAndConfigured()
        {
            // Arrange
            var initialValues = new[] {"test", 1, new object()};

            // Act
            var attribute = new InlineAutoMoqDataAttribute(initialValues[0], initialValues[1], initialValues[2]);

            // Assert
            attribute.AutoDataAttribute.Should()
                .BeOfType<AutoMoqDataAttribute>()
                .Which.Fixture.ShouldBeConfigured();
            attribute.AutoDataAttribute.Fixture.ShouldNotIgnoreVirtualMembers();
            attribute.Values.Should()
                .HaveSameCount(initialValues)
                .And.ContainInOrder(initialValues);
        }

        [Fact]
        public void GivenUninitializedInlineValues_WhenConstructorInvoked_ThenHasUninitializedInlineValuesAndAppropriateFixtureIsCreatedAndConfigured()
        {
            // Arrange
            const object[] initialValues = null;

            // Act
            var attribute = new InlineAutoMoqDataAttribute(initialValues);

            // Assert
            attribute.AutoDataAttribute.Should()
                .BeOfType<AutoMoqDataAttribute>()
                .Which.Fixture.ShouldBeConfigured();
            attribute.AutoDataAttribute.Fixture.ShouldNotIgnoreVirtualMembers();
            attribute.Values.Should().BeNull();
        }

        [Fact]
        public void GivenExistingInlineValuesAndIgnoreVirtualMembers_WhenConstructorInvoked_ThenHasSpecifiedValuesAndAppropriateFixtureIsCreatedAndConfigured()
        {
            // Arrange
            var initialValues = new[] { "test", 1, new object() };

            // Act
            var attribute = new InlineAutoMoqDataAttribute(true, initialValues[0], initialValues[1], initialValues[2]);

            // Assert
            attribute.AutoDataAttribute.Should()
                .BeOfType<AutoMoqDataAttribute>()
                .Which.Fixture.ShouldBeConfigured();
            attribute.AutoDataAttribute.Fixture.ShouldIgnoreVirtualMembers();
            attribute.Values.Should()
                .HaveSameCount(initialValues)
                .And.ContainInOrder(initialValues);
        }

        [Fact]
        public void GivenExistingAutoDataAttribute_WhenConstructorInvoked_ThenFixtureOfSpecifiedAttributeIsConfigured()
        {
            // Arrange
            var autoDataAttribute = new AutoMoqDataAttribute();

            // Act
            var attribute = new InlineAutoMoqDataAttribute(autoDataAttribute);

            // Assert
            attribute.AutoDataAttribute.Should().Be(autoDataAttribute);
            attribute.Values.Count().Should().Be(0);
        }

        [Fact]
        public void GivenExistingAutoDataAttributeAndInlineValues_WhenConstructorInvoked_ThenHasSpecifiedValuesAndFixtureOfSpecifiedAttributeIsConfigured()
        {
            // Arrange
            var autoDataAttribute = new AutoMoqDataAttribute();
            var initialValues = new[] { "test", 1, new object() };

            // Act
            var attribute = new InlineAutoMoqDataAttribute(autoDataAttribute, initialValues[0], initialValues[1], initialValues[2]);

            // Assert
            attribute.AutoDataAttribute.Should().Be(autoDataAttribute);
            attribute.Values.Should()
                .HaveSameCount(initialValues)
                .And.ContainInOrder(initialValues);
        }

        [Fact]
        public void GivenExistingAutoDataAttributeAndUninitializedInlineValues_WhenConstructorInvoked_ThenHasUninitializedInlineValuesAndFixtureOfSpecifiedAttributeIsConfigured()
        {
            // Arrange
            var autoDataAttribute = new AutoMoqDataAttribute();
            const object[] initialValues = null;

            // Act
            var attribute = new InlineAutoMoqDataAttribute(autoDataAttribute, initialValues);

            // Assert
            attribute.AutoDataAttribute.Should().Be(autoDataAttribute);
            attribute.Values.Should().BeNull();
        }

        [Fact]
        public void GivenUninitializedAutoDataAttribute_WhenConstructorInvoked_ThenExceptionShouldBeThrown()
        {
            // Arrange
            const AutoMoqDataAttribute autoDataAttribute = null;

            // Act
            // Assert
            Assert.Throws<ArgumentNullException>(() => new InlineAutoMoqDataAttribute(autoDataAttribute));
        }
    }
}
