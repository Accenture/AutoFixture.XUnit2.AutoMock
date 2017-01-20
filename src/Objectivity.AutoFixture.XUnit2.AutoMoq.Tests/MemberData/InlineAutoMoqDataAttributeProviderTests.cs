namespace Objectivity.AutoFixture.XUnit2.AutoMoq.Tests.MemberData
{
    using AutoMoq.Attributes;
    using AutoMoq.MemberData;
    using FluentAssertions;
    using Xunit;

    [Collection("DataItemConverterFactory")]
    [Trait("Category", "MemberData")]
    public class InlineAutoMoqDataAttributeProviderTests
    {
        [Fact(DisplayName = "GIVEN initialized AutoMoqDataAttribute WHEN GetAttribute is invoked THEN attribute with specified AutoMoqDataAttribute is returned")]
        public void GivenInitializedAutoMoqDataAttribute_WhenGetAttributeIsInvoked_ThenAttributeWithSpecifiedAutoMoqDataAttributeIsReturned()
        {
            // Arrange
            var autoMoqDataAttribute = new AutoMoqDataAttribute();
            var provider = new InlineAutoMoqDataAttributeProvider(autoMoqDataAttribute);

            // Act
            var dataAttribute = provider.GetAttribute() as InlineAutoMoqDataAttribute;

            // Assert
            dataAttribute.Should().NotBeNull();
            dataAttribute.AutoDataAttribute.Should().Be(autoMoqDataAttribute);
        }

        [Fact(DisplayName = "GIVEN uninitialized AutoMoqDataAttribute WHEN GetAttribute is invoked THEN attribute with new AutoMoqDataAttribute is returned")]
        public void GivenUninitializedAutoMoqDataAttribute_WhenGetAttributeIsInvoked_ThenAttributeWithNewAutoMoqDataAttributeIsReturned()
        {
            // Arrange
            var provider = new InlineAutoMoqDataAttributeProvider();

            // Act
            var dataAttribute = provider.GetAttribute() as InlineAutoMoqDataAttribute;

            // Assert
            dataAttribute.Should().NotBeNull();
            dataAttribute.AutoDataAttribute.Should().NotBeNull();
        }
    }
}
