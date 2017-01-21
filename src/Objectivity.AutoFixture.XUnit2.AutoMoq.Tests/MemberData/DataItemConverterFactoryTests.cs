namespace Objectivity.AutoFixture.XUnit2.AutoMoq.Tests.MemberData
{
    using System;
    using System.Diagnostics.CodeAnalysis;
    using AutoMoq.MemberData;
    using FluentAssertions;
    using Ploeh.AutoFixture;
    using Xunit;

    [Collection("DataItemConverterFactory")]
    [Trait("Category", "MemberData")]
    public class DataItemConverterFactoryTests
    {
        [Fact(DisplayName = "GIVEN shared fixture and not ignore virtual members WHEN Create is invoked THEN data item converter with the same fixture that will generate values for virtual members is returned")]
        [SuppressMessage("ReSharper", "PossibleNullReferenceException", Justification = "Assertion checks it earlier and throws exception.")]
        public void GivenSharedFixtureAndNotIgnoreVirtualMembers_WhenCreateIsInvoked_ThenDataItemConverterWithTheSameFixtureThatWillGenerateValuesForVirtualMembersIsReturned()
        {
            // Arrange
            const bool shareFixture = true;
            const bool ignoreVirtualMembers = false;
            var fixture = new Fixture();

            // Act
            var converter = DataItemConverterFactory.Create(shareFixture, ignoreVirtualMembers, fixture) as MemberAutoMoqDataItemConverter;

            // Assert
            converter.Should().NotBeNull();
            var provider = converter.DataAttributeProvider as InlineAutoMoqDataAttributeProvider;
            provider.Should().NotBeNull();
            provider.AutoMoqDataAttribute.Fixture.Should().Be(fixture);
            provider.AutoMoqDataAttribute.Fixture.ShouldNotIgnoreVirtualMembers();
        }

        [Fact(DisplayName = "GIVEN unshared fixture and not ignore virtual members WHEN Create is invoked THEN data item converter with new fixture that will generate values for virtual members is returned")]
        [SuppressMessage("ReSharper", "PossibleNullReferenceException", Justification = "Assertion checks it earlier and throws exception.")]
        public void GivenUnsharedFixtureAndNotIgnoreVirtualMembers_WhenCreateIsInvoked_ThenDataItemConverterWithNewFixtureThatWillGenerateValuesForVirtualMembersIsReturned()
        {
            // Arrange
            const bool shareFixture = false;
            const bool ignoreVirtualMembers = false;
            var fixture = new Fixture();

            // Act
            var converter = DataItemConverterFactory.Create(shareFixture, ignoreVirtualMembers, fixture) as MemberAutoMoqDataItemConverter;

            // Assert
            converter.Should().NotBeNull();
            var provider = converter.DataAttributeProvider as InlineAutoMoqDataAttributeProvider;
            provider.Should().NotBeNull();
            provider.AutoMoqDataAttribute.Fixture.Should().NotBeNull().And.NotBe(fixture);
            provider.AutoMoqDataAttribute.Fixture.ShouldNotIgnoreVirtualMembers();
        }

        [Fact(DisplayName = "GIVEN shared fixture and ignore virtual members WHEN Create is invoked THEN data item converter with the same fixture that will not generate values for virtual members is returned")]
        [SuppressMessage("ReSharper", "PossibleNullReferenceException", Justification = "Assertion checks it earlier and throws exception.")]
        public void GivenSharedFixtureAndIgnoreVirtualMembers_WhenCreateIsInvoked_ThenDataItemConverterWithTheSameFixtureThatWillNotGenerateValuesForVirtualMembersIsReturned()
        {
            // Arrange
            const bool shareFixture = true;
            const bool ignoreVirtualMembers = true;
            var fixture = new Fixture();

            // Act
            var converter = DataItemConverterFactory.Create(shareFixture, ignoreVirtualMembers, fixture) as MemberAutoMoqDataItemConverter;

            // Assert
            converter.Should().NotBeNull();
            var provider = converter.DataAttributeProvider as InlineAutoMoqDataAttributeProvider;
            provider.Should().NotBeNull();
            provider.AutoMoqDataAttribute.Fixture.Should().Be(fixture);
            provider.AutoMoqDataAttribute.Fixture.ShouldIgnoreVirtualMembers();
        }

        [Fact(DisplayName = "GIVEN unshared fixture and ignore virtual members WHEN Create is invoked THEN data item converter with new fixture that will not generate values for virtual members is returned")]
        [SuppressMessage("ReSharper", "PossibleNullReferenceException", Justification = "Assertion checks it earlier and throws exception.")]
        public void GivenUnsharedFixtureAndIgnoreVirtualMembers_WhenCreateIsInvoked_ThenDataItemConverterWithNewFixtureThatWillNotGenerateValuesForVirtualMembersIsReturned()
        {
            // Arrange
            const bool shareFixture = false;
            const bool ignoreVirtualMembers = true;
            var fixture = new Fixture();

            // Act
            var converter = DataItemConverterFactory.Create(shareFixture, ignoreVirtualMembers, fixture) as MemberAutoMoqDataItemConverter;

            // Assert
            converter.Should().NotBeNull();
            var provider = converter.DataAttributeProvider as InlineAutoMoqDataAttributeProvider;
            provider.Should().NotBeNull();
            provider.AutoMoqDataAttribute.Fixture.Should().NotBeNull().And.NotBe(fixture);
            provider.AutoMoqDataAttribute.Fixture.ShouldIgnoreVirtualMembers();
        }

        [Fact(DisplayName = "GIVEN uninitialized fixture WHEN Create is invoked THEN exception is thrown")]
        public void GivenUninitializedFixtureAndNotIgnoreVirtualMembers_WhenCreateIsInvoked_ThenExceptionIsThrown()
        {
            // Arrange
            const bool shareFixture = true;
            const bool ignoreVirtualMembers = false;
            const Fixture uninitializedFixture = null;

            // Act
            // Assert
            Assert.Throws<ArgumentNullException>(() => DataItemConverterFactory.Create(shareFixture, ignoreVirtualMembers, uninitializedFixture));
        }
    }
}
