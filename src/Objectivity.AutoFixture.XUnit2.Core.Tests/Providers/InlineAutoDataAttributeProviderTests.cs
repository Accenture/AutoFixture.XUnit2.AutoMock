namespace Objectivity.AutoFixture.XUnit2.Core.Tests.Providers
{
    using global::AutoFixture;
    using global::AutoFixture.Xunit2;
    using Objectivity.AutoFixture.XUnit2.Core.Attributes;
    using Objectivity.AutoFixture.XUnit2.Core.Providers;

    using Xunit;

    [Collection("InlineAutoDataAttributeProvider")]
    [Trait("Category", "Providers")]
    public class InlineAutoDataAttributeProviderTests
    {
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
            var autoDataAdapterAttribute = Assert.IsType<AutoDataAdapterAttribute>(attribute);
            Assert.Equal(fixture, autoDataAdapterAttribute.AdaptedFixture);
            Assert.Equal(inlineValues, autoDataAdapterAttribute.InlineValues);
        }
    }
}
