namespace Objectivity.AutoFixture.XUnit2.AutoMoq.Tests.Attributes
{
    using System;
    using System.Linq;
    using AutoMoq.Attributes;
    using FluentAssertions;
    using Xunit;

    [Collection("InlineAutoMoqDataAttribute")]
    [Trait("Category", "Attributes")]
    public class InlineAutoMoqDataAttributeTests
    {
        [Fact(DisplayName = "WHEN parameterless constructor is invoked THEN has no values and appropriate fixture is created and configured")]
        public void WhenParameterlessConstructorIsInvoked_ThenHasNoValuesAndAppropriateFixtureIsCreatedAndConfigured()
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

        [Fact(DisplayName = "GIVEN existing inline values WHEN constructor is invoked THEN has specified values and appropriate fixture is created and configured")]
        public void GivenExistingInlineValues_WhenConstructorIsInvoked_ThenHasSpecifiedValuesAndAppropriateFixtureIsCreatedAndConfigured()
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

        [Fact(DisplayName = "GIVEN existing inline values and ignore virtual members set to true WHEN constructor is invoked THEN has specified values and appropriate fixture is created and configured")]
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

        [Fact(DisplayName = "GIVEN uninitialized values WHEN constructor is invoked THEN has uninitialized values and appropriate fixture is created and configured")]
        public void GivenUninitializedValues_WhenConstructorIsInvoked_ThenHasUninitializedValuesAndAppropriateFixtureIsCreatedAndConfigured()
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

        [Fact(DisplayName = "GIVEN existing auto-data attribute WHEN constructor is invoked THEN fixture of specified attribute is configured")]
        public void GivenExistingAutoDataAttribute_WhenConstructorIsInvoked_ThenFixtureOfSpecifiedAttributeIsConfigured()
        {
            // Arrange
            var autoDataAttribute = new AutoMoqDataAttribute();

            // Act
            var attribute = new InlineAutoMoqDataAttribute(autoDataAttribute);

            // Assert
            attribute.AutoDataAttribute.Should().Be(autoDataAttribute);
            attribute.Values.Count().Should().Be(0);
        }

        [Fact(DisplayName = "GIVEN existing auto-data attribute and values WHEN constructor is invoked THEN has specified values and fixture of specified attribute is configured")]
        public void GivenExistingAutoDataAttributeAndValues_WhenConstructorIsInvoked_ThenHasSpecifiedValuesAndFixtureOfSpecifiedAttributeIsConfigured()
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

        [Fact(DisplayName = "GIVEN existing auto-data attribute and uninitialized values WHEN constructor is invoked THEN has uninitialized values and fixture of specified attribute is configured")]
        public void GivenExistingAutoDataAttributeAndUninitializedValues_WhenConstructorIsInvoked_ThenHasUninitializedValuesAndFixtureOfSpecifiedAttributeIsConfigured()
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

        [Fact(DisplayName = "GIVEN uninitialized auto-data attribute WHEN constructor is invoked THEN exception is thrown")]
        public void GivenUninitializedAutoDataAttribute_WhenConstructorIsInvoked_ThenExceptionIsThrown()
        {
            // Arrange
            const AutoMoqDataAttribute autoDataAttribute = null;

            // Act
            // Assert
            Assert.Throws<ArgumentNullException>(() => new InlineAutoMoqDataAttribute(autoDataAttribute));
        }
    }
}
