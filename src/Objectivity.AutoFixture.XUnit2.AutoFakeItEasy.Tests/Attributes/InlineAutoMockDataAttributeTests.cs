﻿namespace Objectivity.AutoFixture.XUnit2.AutoFakeItEasy.Tests.Attributes
{
    using System;
    using System.Collections.Generic;
    using System.Reflection;

    using FakeItEasy;

    using FluentAssertions;

    using global::AutoFixture;
    using global::AutoFixture.AutoFakeItEasy;
    using global::AutoFixture.Xunit2;
    using Objectivity.AutoFixture.XUnit2.AutoFakeItEasy.Attributes;
    using Objectivity.AutoFixture.XUnit2.Core.Customizations;
    using Objectivity.AutoFixture.XUnit2.Core.Providers;

    using Xunit;
    using Xunit.Sdk;

    [Collection("InlineAutoMockDataAttribute")]
    [Trait("Category", "Attributes")]
    public class InlineAutoMockDataAttributeTests
    {
        [Fact(DisplayName = "WHEN parameterless constructor is invoked THEN has no values but fixture and attribute provider are created")]
        public void WhenParameterlessConstructorIsInvoked_ThenHasNoValuesButFixtureAndAttributeProviderAreCreated()
        {
            // Arrange
            // Act
            var attribute = new InlineAutoMockDataAttribute();

            // Assert
            attribute.Fixture.Should().NotBeNull();
            attribute.IgnoreVirtualMembers.Should().BeFalse();
            attribute.Provider.Should().NotBeNull();
            attribute.Values.Should().HaveCount(0);
        }

        [Fact(DisplayName = "GIVEN existing inline values WHEN constructor is invoked THEN has specified values and fixture and attribute provider are created")]
        public void GivenExistingInlineValues_WhenConstructorIsInvoked_ThenHasSpecifiedValuesAndFixtureAndAttributeProviderAreCreated()
        {
            // Arrange
            var initialValues = new[] { "test", 1, new object() };

            // Act
            var attribute = new InlineAutoMockDataAttribute(initialValues[0], initialValues[1], initialValues[2]);

            // Assert
            attribute.Fixture.Should().NotBeNull();
            attribute.IgnoreVirtualMembers.Should().BeFalse();
            attribute.Provider.Should().NotBeNull();
            attribute.Values.Should().BeEquivalentTo(initialValues);
        }

        [Fact(DisplayName = "GIVEN uninitialized values WHEN constructor is invoked THEN has no values and fixture and attribute provider are created")]
        public void GivenUninitializedValues_WhenConstructorIsInvoked_ThenHasNoValuesAndFixtureAndAttributeProviderAreCreated()
        {
            // Arrange
            const object[] initialValues = null;

            // Act
            var attribute = new InlineAutoMockDataAttribute(initialValues);

            // Assert
            attribute.Fixture.Should().NotBeNull();
            attribute.IgnoreVirtualMembers.Should().BeFalse();
            attribute.Provider.Should().NotBeNull();
            attribute.Values.Should().HaveCount(0);
        }

        [Theory(DisplayName = "WHEN GetData is invoked THEN fixture is configured and data returned")]
        [InlineAutoData(true)]
        [InlineAutoData(false)]
        public void WhenGetDataIsInvoked_ThenFixtureIsConfiguredAndDataReturned(bool ignoreVirtualMembers)
        {
            // Arrange
            var data = new[]
            {
                new object[] { 1, 2, 3 },
                new object[] { 4, 5, 6 },
                new object[] { 7, 8, 9 },
            };
            var fixture = A.Fake<IFixture>();
            var customizations = new List<ICustomization>();
            A.CallTo(() => fixture.Customize(A<ICustomization>._))
                .Invokes((ICustomization customization) => customizations.Add(customization))
                .Returns(fixture);
            var dataAttribute = A.Fake<DataAttribute>();
            A.CallTo(() => dataAttribute.GetData(A<MethodInfo>._)).Returns(data);
            var provider = A.Fake<IAutoFixtureInlineAttributeProvider>();
            A.CallTo(() => provider.GetAttribute(A<IFixture>._)).Returns(dataAttribute);
            var attribute = new InlineAutoMockDataAttribute(fixture, provider)
            {
                IgnoreVirtualMembers = ignoreVirtualMembers,
            };
            var methodInfo = typeof(InlineAutoMockDataAttributeTests).GetMethod(nameof(this.TestMethod), BindingFlags.Instance | BindingFlags.NonPublic);

            // Act
            var result = attribute.GetData(methodInfo);

            // Assert
            result.Should().BeSameAs(data);
            A.CallTo(() => provider.GetAttribute(A<IFixture>._)).MustHaveHappenedOnceExactly();
            A.CallTo(() => dataAttribute.GetData(A<MethodInfo>._)).MustHaveHappenedOnceExactly();

            customizations.Count.Should().Be(2);
            customizations[0]
                .Should()
                .BeOfType<AutoDataCommonCustomization>()
                .Which.IgnoreVirtualMembers.Should()
                .Be(ignoreVirtualMembers);
            customizations[1].Should().BeOfType<AutoFakeItEasyCustomization>();
        }

        [InlineAutoMockData(100)]
        [Theory(DisplayName = "GIVEN test method has some inline parameters WHEN test run THEN parameters are generated")]
        public void GivenTestMethodHasSomeInlineParameters_WhenTestRun_ThenParametersAreGenerated(int value, IDisposable disposable)
        {
            // Arrange
            // Act
            // Assert
            value.Should().Be(100);

            disposable.Should().NotBeNull();
            disposable.GetType().Name.Should().Contain("Proxy", "that way we know it was mocked.");
        }

        protected void TestMethod()
        {
        }
    }
}
