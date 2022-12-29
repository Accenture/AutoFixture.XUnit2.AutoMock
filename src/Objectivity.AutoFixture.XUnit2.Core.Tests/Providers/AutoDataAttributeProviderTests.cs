﻿namespace Objectivity.AutoFixture.XUnit2.Core.Tests.Providers
{
    using System.Diagnostics.CodeAnalysis;

    using FluentAssertions;

    using global::AutoFixture;
    using global::AutoFixture.Xunit2;
    using Objectivity.AutoFixture.XUnit2.Core.Attributes;
    using Objectivity.AutoFixture.XUnit2.Core.Providers;

    using Xunit;

    [Collection("AutoDataAttributeProvider")]
    [Trait("Category", "Providers")]
    public class AutoDataAttributeProviderTests
    {
        [Theory(DisplayName = "GIVEN initialized fixture WHEN GetAttribute is invoked THEN attribute with specified fixture is returned")]
        [AutoData]
        [SuppressMessage("ReSharper", "PossibleNullReferenceException", Justification = "Assertion checks it earlier and throws exception.")]
        public void GivenInitializedFixture_WhenGetAttributeIsInvoked_ThenAttributeWithSpecifiedFixtureIsReturned(Fixture fixture)
        {
            // Arrange
            var provider = new AutoDataAttributeProvider();

            // Act
            var dataAttribute = provider.GetAttribute(fixture) as AutoDataAdapterAttribute;

            // Assert
            dataAttribute.Should().NotBeNull();
            dataAttribute.AdaptedFixture.Should().Be(fixture);
        }
    }
}
