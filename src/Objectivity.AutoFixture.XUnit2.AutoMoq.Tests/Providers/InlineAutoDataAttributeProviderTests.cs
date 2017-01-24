namespace Objectivity.AutoFixture.XUnit2.AutoMoq.Tests.Providers
{
    using System.Diagnostics.CodeAnalysis;
    using System.Linq;
    using AutoMoq.Providers;
    using FluentAssertions;
    using Ploeh.AutoFixture;
    using Ploeh.AutoFixture.Xunit2;
    using Xunit;

    [Collection("InlineAutoDataAttributeProvider")]
    [Trait("Category", "Providers")]
    public class InlineAutoDataAttributeProviderTests
    {
        [Theory(DisplayName = "GIVEN initialized fixture WHEN GetAttribute is invoked THEN attribute with specified fixture is returned")]
        [AutoData]
        [SuppressMessage("ReSharper", "PossibleNullReferenceException", Justification = "Assertion checks it earlier and throws exception.")]
        public void GivenInitializedFixture_WhenGetAttributeIsInvoked_ThenAttributeWithSpecifiedFixtureIsReturned(Fixture fixture)
        {
            // Arrange
            var provider = new InlineAutoDataAttributeProvider();
                 
            // Act
            var dataAttribute = provider.GetAttribute(fixture) as CompositeDataAttribute;

            // Assert
            dataAttribute.Should().NotBeNull();
            dataAttribute.Attributes.FirstOrDefault(a => a is AutoDataAttribute).As<AutoDataAttribute>().Fixture.Should().Be(fixture);
        }
    }
}
