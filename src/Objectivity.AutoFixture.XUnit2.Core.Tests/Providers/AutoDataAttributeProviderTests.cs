namespace Objectivity.AutoFixture.XUnit2.Core.Tests.Providers
{
    using global::AutoFixture;
    using global::AutoFixture.Xunit2;
    using Objectivity.AutoFixture.XUnit2.Core.Attributes;
    using Objectivity.AutoFixture.XUnit2.Core.Providers;

    using Xunit;

    [Collection("AutoDataAttributeProvider")]
    [Trait("Category", "Providers")]
    public class AutoDataAttributeProviderTests
    {
        [AutoData]
        [Theory(DisplayName = "GIVEN initialized fixture WHEN GetAttribute is invoked THEN attribute with specified fixture is returned")]
        public void GivenInitializedFixture_WhenGetAttributeIsInvoked_ThenAttributeWithSpecifiedFixtureIsReturned(Fixture fixture)
        {
            // Arrange
            var provider = new AutoDataAttributeProvider();

            // Act
            var dataAttribute = provider.GetAttribute(fixture);

            // Assert
            var typed = Assert.IsType<AutoDataAdapterAttribute>(dataAttribute);
            Assert.Equal(fixture, typed.AdaptedFixture);
        }
    }
}
