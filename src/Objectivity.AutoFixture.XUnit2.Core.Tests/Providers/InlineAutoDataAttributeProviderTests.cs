namespace Objectivity.AutoFixture.XUnit2.Core.Tests.Providers
{
    using System.Diagnostics.CodeAnalysis;

    using FluentAssertions;

    using global::AutoFixture;
    using global::AutoFixture.Xunit2;
    using Objectivity.AutoFixture.XUnit2.Core.Attributes;
    using Objectivity.AutoFixture.XUnit2.Core.Providers;

    using Xunit;

    [Collection("InlineAutoDataAttributeProvider")]
    [Trait("Category", "Providers")]
    public class InlineAutoDataAttributeProviderTests
    {
        [SuppressMessage("ReSharper", "PossibleNullReferenceException", Justification = "Assertion checks it earlier and throws exception.")]
        [AutoData]
        [Theory(DisplayName = "GIVEN initialized fixture WHEN GetAttribute is invoked THEN attribute with specified fixture is returned")]
        public void GivenInitializedFixture_WhenGetAttributeIsInvoked_ThenAttributeWithSpecifiedFixtureIsReturned(
            Fixture fixture,
            object[] inlineValues)
        {
            // Arrange
            var provider = new InlineAutoDataAttributeProvider();

            // Act
            var attribute = provider.GetAttribute(fixture, inlineValues);

            // Assert
            var autoDataAdapterAttribute = attribute.Should().BeOfType<AutoDataAdapterAttribute>().Which;
            autoDataAdapterAttribute.AdaptedFixture.Should().Be(fixture);
            autoDataAdapterAttribute.InlineValues.Should().BeEquivalentTo(inlineValues);
        }
    }
}
