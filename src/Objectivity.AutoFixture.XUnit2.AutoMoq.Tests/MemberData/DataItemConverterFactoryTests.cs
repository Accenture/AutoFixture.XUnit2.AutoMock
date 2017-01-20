namespace Objectivity.AutoFixture.XUnit2.AutoMoq.Tests.MemberData
{
    using System;
    using AutoMoq.MemberData;
    using FluentAssertions;
    using Ploeh.AutoFixture;
    using Xunit;

    [Collection("DataItemConverterFactory")]
    [Trait("Category", "MemberData")]
    public class DataItemConverterFactoryTests
    {
        [Fact(DisplayName = "GIVEN shared fixture WHEN Create is invoked THEN data item converter with the same fixture is returned")]
        public void GivenSharedFixture_WhenCreateIsInvoked_ThenDataItemConverterWithTheSameFixtureIsReturned()
        {
            // Arrange
            const bool shareFixture = true;
            var fixture = new Fixture();

            // Act
            var converter = DataItemConverterFactory.Create(shareFixture, fixture) as MemberAutoMoqDataItemConverter;

            // Assert
            converter.Should().NotBeNull();
            var provider = converter.DataAttributeProvider as InlineAutoMoqDataAttributeProvider;
            provider.Should().NotBeNull();
            provider.AutoMoqDataAttribute.Fixture.Should().Be(fixture);
        }

        [Fact(DisplayName = "GIVEN unshared fixture WHEN Create is invoked THEN data item converter with new fixture is returned")]
        public void GivenUnsharedFixture_WhenCreateIsInvoked_ThenDataItemConverterWithNewFixtureIsReturned()
        {
            // Arrange
            const bool shareFixture = false;
            var fixture = new Fixture();

            // Act
            var converter = DataItemConverterFactory.Create(shareFixture, fixture) as MemberAutoMoqDataItemConverter;

            // Assert
            converter.Should().NotBeNull();
            var provider = converter.DataAttributeProvider as InlineAutoMoqDataAttributeProvider;
            provider.Should().NotBeNull();
            provider.AutoMoqDataAttribute.Fixture.Should().NotBeNull().And.NotBe(fixture);
        }

        [Fact(DisplayName = "GIVEN uninitialized fixture WHEN Create is invoked THEN exception is thrown")]
        public void GivenUninitializedFixture_WhenCreateIsInvoked_ThenExceptionIsThrown()
        {
            // Arrange
            const bool shareFixture = true;
            const Fixture uninitializedFixture = null;

            // Act
            // Assert
            Assert.Throws<ArgumentNullException>(() => DataItemConverterFactory.Create(shareFixture, uninitializedFixture));
        }
    }
}
