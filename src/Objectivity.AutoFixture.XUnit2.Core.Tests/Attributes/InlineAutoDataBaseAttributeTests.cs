namespace Objectivity.AutoFixture.XUnit2.Core.Tests.Attributes
{
    using System;

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
            Assert.Equal(fixture, attribute.Fixture);
            Assert.False(attribute.IgnoreVirtualMembers);
            Assert.Equal(provider.Object, attribute.Provider);
            Assert.Empty(attribute.Values);
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
            Assert.Equal(fixture, attribute.Fixture);
            Assert.False(attribute.IgnoreVirtualMembers);
            Assert.Equal(provider.Object, attribute.Provider);
            Assert.Equal(initialValues, attribute.Values);
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
            Assert.Equal(fixture, attribute.Fixture);
            Assert.False(attribute.IgnoreVirtualMembers);
            Assert.Equal(provider.Object, attribute.Provider);
            Assert.Empty(attribute.Values);
        }

        [Fact(DisplayName = "GIVEN uninitialized fixture WHEN constructor is invoked THEN exception is thrown")]
        public void GivenUninitializedFixture_WhenConstructorIsInvoked_ThenExceptionIsThrown()
        {
            // Arrange
            const Fixture fixture = null;
            var provider = new Mock<IAutoFixtureInlineAttributeProvider>();

            // Act
            object Act() => new InlineAutoDataBaseAttributeUnderTest(fixture, provider.Object);

            // Assert
            var exception = Assert.Throws<ArgumentNullException>(Act);
            Assert.Equal("fixture", exception.ParamName);
        }

        [AutoData]
        [Theory(DisplayName = "GIVEN uninitialized attribute provider WHEN constructor is invoked THEN exception is thrown")]
        public void GivenUninitializedAttributeProvider_WhenConstructorIsInvoked_ThenExceptionIsThrown(Fixture fixture)
        {
            // Arrange
            const IAutoFixtureInlineAttributeProvider provider = null;

            // Act
            object Act() => new InlineAutoDataBaseAttributeUnderTest(fixture, provider);

            // Assert
            var exception = Assert.Throws<ArgumentNullException>(Act);
            Assert.Equal("provider", exception.ParamName);
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
