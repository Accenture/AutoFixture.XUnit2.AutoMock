namespace Objectivity.AutoFixture.XUnit2.Core.Tests.Attributes
{
    using System;

    using FluentAssertions;

    using global::AutoFixture;
    using global::AutoFixture.Xunit2;
    using Moq;

    using Objectivity.AutoFixture.XUnit2.Core.Attributes;
    using Objectivity.AutoFixture.XUnit2.Core.Providers;

    using Xunit;

    [Collection("InlineAutoDataBaseAttribute")]
    [Trait("Category", "DataAttribute")]
    public class InlineMockDataBaseAttributeTests
    {
        [AutoData]
        [Theory(DisplayName = "GIVEN existing fixture and attribute provider WHEN constructor is invoked THEN has fixture attribute provider and no values")]
        public void GivenExistingFixtureAndAttributeProvider_WhenConstructorIsInvoked_ThenHasFixtureAttributeProviderAndNoValues(Fixture fixture)
        {
            // Arrange
            var provider = new Mock<IAutoFixtureInlineAttributeProvider>();

            // Act
            var attribute = new InlineAutoDataBaseAttributeUnderTest(fixture, provider.Object);

            // Assert
            attribute.Fixture.Should().Be(fixture);
            attribute.IgnoreVirtualMembers.Should().BeFalse();
            attribute.Provider.Should().Be(provider.Object);
            attribute.Values.Should().HaveCount(0);
        }

        [AutoData]
        [Theory(DisplayName = "GIVEN existing fixture, attribute provider and values WHEN constructor is invoked THEN has specified fixture, attribute provider and values")]
        public void GivenExistingFixtureAttributeProviderAndValues_WhenConstructorIsInvoked_ThenHasSpecifiedFixtureAttributeProviderAndValues(Fixture fixture)
        {
            // Arrange
            var provider = new Mock<IAutoFixtureInlineAttributeProvider>();
            var initialValues = new[] { "test", 1, new object() };

            // Act
            var attribute = new InlineAutoDataBaseAttributeUnderTest(fixture, provider.Object, initialValues[0], initialValues[1], initialValues[2]);

            // Assert
            attribute.Fixture.Should().Be(fixture);
            attribute.IgnoreVirtualMembers.Should().BeFalse();
            attribute.Provider.Should().Be(provider.Object);
            attribute.Values.Should().BeEquivalentTo(initialValues);
        }

        [AutoData]
        [Theory(DisplayName = "GIVEN existing fixture, attribute provider and uninitialized values WHEN constructor is invoked THEN has specified fixture, attribute provider and no values")]
        public void GivenExistingFixtureAttributeProviderAndUninitializedValues_WhenConstructorIsInvoked_ThenHasSpecifiedFixtureAttributeProviderAndNoValues(Fixture fixture)
        {
            // Arrange
            var provider = new Mock<IAutoFixtureInlineAttributeProvider>();
            const object[] initialValues = null;

            // Act
            var attribute = new InlineAutoDataBaseAttributeUnderTest(fixture, provider.Object, initialValues);

            // Assert
            attribute.Fixture.Should().Be(fixture);
            attribute.IgnoreVirtualMembers.Should().BeFalse();
            attribute.Provider.Should().Be(provider.Object);
            attribute.Values.Should().HaveCount(0);
        }

        [Fact(DisplayName = "GIVEN uninitialized fixture WHEN constructor is invoked THEN exception is thrown")]
        public void GivenUninitializedFixture_WhenConstructorIsInvoked_ThenExceptionIsThrown()
        {
            // Arrange
            const Fixture fixture = null;
            var provider = new Mock<IAutoFixtureInlineAttributeProvider>();

            // Act
            // Assert
            Assert.Throws<ArgumentNullException>(() => new InlineAutoDataBaseAttributeUnderTest(fixture, provider.Object));
        }

        [AutoData]
        [Theory(DisplayName = "GIVEN uninitialized attribute provider WHEN constructor is invoked THEN exception is thrown")]
        public void GivenUninitializedAttributeProvider_WhenConstructorIsInvoked_ThenExceptionIsThrown(Fixture fixture)
        {
            // Arrange
            const IAutoFixtureInlineAttributeProvider provider = null;

            // Act
            // Assert
            Assert.Throws<ArgumentNullException>(() => new InlineAutoDataBaseAttributeUnderTest(fixture, provider));
        }

        [AttributeUsage(AttributeTargets.Method, AllowMultiple = true)]
        private sealed class InlineAutoDataBaseAttributeUnderTest : InlineAutoDataBaseAttribute
        {
            public InlineAutoDataBaseAttributeUnderTest(IFixture fixture, IAutoFixtureInlineAttributeProvider provider, params object[] values)
                : base(fixture, provider, values)
            {
            }

            protected override IFixture Customize(IFixture fixture)
            {
                throw new NotImplementedException();
            }
        }
    }
}
