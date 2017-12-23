namespace Objectivity.AutoFixture.XUnit2.Core.Tests.Attributes
{
    using System;
    using FluentAssertions;
    using Moq;
    using Objectivity.AutoFixture.XUnit2.Core.Attributes;
    using Objectivity.AutoFixture.XUnit2.Core.Providers;
    using Ploeh.AutoFixture;
    using Ploeh.AutoFixture.Xunit2;
    using Xunit;

    [Collection("AutoMockDataBaseAttribute")]
    [Trait("Category", "Attributes")]
    public class AutoMockDataBaseAttributeTests
    {
        [Theory(DisplayName = "GIVEN existing fixture and attribute provider WHEN constructor is invoked THEN has specified fixture and attribute provider")]
        [AutoData]
        public void GivenExistingFixtureAndAttributeProvider_WhenConstructorIsInvoked_ThenHasSpecifiedFixtureAndAttributeProvider(Fixture fixture)
        {
            // Arrange
            var provider = new Mock<IAutoFixtureAttributeProvider>();

            // Act
            var attribute = new AutoMockDataBaseAttributeUnderTest(fixture, provider.Object);

            // Assert
            attribute.Fixture.Should().Be(fixture);
            attribute.Provider.Should().Be(provider.Object);
            attribute.IgnoreVirtualMembers.Should().BeFalse();
        }

        [Fact(DisplayName = "GIVEN uninitialized fixture WHEN constructor is invoked THEN exception is thrown")]
        public void GivenUninitializedFixture_WhenConstructorIsInvoked_ThenExceptionIsThrown()
        {
            // Arrange
            const Fixture fixture = null;
            var provider = new Mock<IAutoFixtureAttributeProvider>();

            // Act
            // Assert
            Assert.Throws<ArgumentNullException>(() => new AutoMockDataBaseAttributeUnderTest(fixture, provider.Object));
        }

        [Theory(DisplayName = "GIVEN uninitialized attribute provider WHEN constructor is invoked THEN exception is thrown")]
        [AutoData]
        public void GivenUninitializedAttributeProvider_WhenConstructorIsInvoked_ThenExceptionIsThrown(Fixture fixture)
        {
            // Arrange
            const IAutoFixtureAttributeProvider provider = null;

            // Act
            // Assert
            Assert.Throws<ArgumentNullException>(() => new AutoMockDataBaseAttributeUnderTest(fixture, provider));
        }

        private class AutoMockDataBaseAttributeUnderTest : AutoMockDataBaseAttribute
        {
            public AutoMockDataBaseAttributeUnderTest(IFixture fixture, IAutoFixtureAttributeProvider provider)
                : base(fixture, provider)
            {
            }

            public override IFixture Customize(IFixture fixture)
            {
                throw new NotImplementedException();
            }
        }
    }
}
