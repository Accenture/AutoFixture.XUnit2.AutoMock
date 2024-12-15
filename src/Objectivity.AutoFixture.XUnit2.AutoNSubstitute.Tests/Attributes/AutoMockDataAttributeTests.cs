namespace Objectivity.AutoFixture.XUnit2.AutoNSubstitute.Tests.Attributes
{
    using System.Collections.Generic;
    using System.Reflection;

    using FluentAssertions;

    using global::AutoFixture;
    using global::AutoFixture.AutoNSubstitute;
    using global::AutoFixture.Xunit2;
    using NSubstitute;

    using Objectivity.AutoFixture.XUnit2.AutoNSubstitute.Attributes;
    using Objectivity.AutoFixture.XUnit2.Core.Customizations;
    using Objectivity.AutoFixture.XUnit2.Core.Providers;

    using Xunit;
    using Xunit.Sdk;

    [Collection("AutoMockDataAttribute")]
    [Trait("Category", "DataAttribute")]
    public class AutoMockDataAttributeTests
    {
        [Fact(DisplayName = "WHEN parameterless constructor is invoked THEN fixture and attribute provider are created")]
        public void WhenParameterlessConstructorIsInvoked_ThenFixtureAndAttributeProviderAreCreated()
        {
            // Arrange
            // Act
            var attribute = new AutoMockDataAttribute();

            // Assert
            attribute.Fixture.Should().NotBeNull();
            attribute.Provider.Should().NotBeNull();
            attribute.IgnoreVirtualMembers.Should().BeFalse();
        }

        [InlineAutoData(true)]
        [InlineAutoData(false)]
        [Theory(DisplayName = "WHEN GetData is invoked THEN fixture is configured and data returned")]
        public void WhenGetDataIsInvoked_ThenFixtureIsConfiguredAndDataReturned(bool ignoreVirtualMembers)
        {
            // Arrange
            var data = new[]
            {
                new object[] { 1, 2, 3 },
                new object[] { 4, 5, 6 },
                new object[] { 7, 8, 9 },
            };
            var fixture = Substitute.For<IFixture>();
            var customizations = new List<ICustomization>();
            fixture.Customize(Arg.Do<ICustomization>(customizations.Add))
                .Returns(fixture);
            var dataAttribute = Substitute.For<DataAttribute>();
            dataAttribute.GetData(Arg.Any<MethodInfo>()).Returns(data);
            var provider = Substitute.For<IAutoFixtureAttributeProvider>();
            provider.GetAttribute(Arg.Any<IFixture>()).Returns(dataAttribute);
            var attribute = new AutoMockDataAttribute(fixture, provider)
            {
                IgnoreVirtualMembers = ignoreVirtualMembers,
            };
            var methodInfo = typeof(AutoMockDataAttributeTests).GetMethod(nameof(this.MethodUnderTest), BindingFlags.Instance | BindingFlags.NonPublic);

            // Act
            var result = attribute.GetData(methodInfo);

            // Assert
            result.Should().BeSameAs(data);
            provider.Received(1).GetAttribute(Arg.Any<IFixture>());
            dataAttribute.Received(1).GetData(Arg.Any<MethodInfo>());

            customizations.Count.Should().Be(2);
            customizations[0]
                .Should()
                .BeOfType<AutoDataCommonCustomization>()
                .Which.IgnoreVirtualMembers.Should()
                .Be(ignoreVirtualMembers);
            customizations[1].Should().BeOfType<AutoNSubstituteCustomization>();
        }

        [AutoMockData]
        [Theory(DisplayName = "GIVEN test method has some value parameters WHEN test run THEN parameters are generated")]
        public void GivenTestMethodHasSomeValueParameters_WhenTestRun_ThenParametersAreGenerated(int value)
        {
            // Arrange
            // Act
            // Assert
            value.Should().NotBe(0);
        }

        [AutoMockData]
        [Theory(DisplayName = "GIVEN test method has some object parameters WHEN test run THEN parameters are generated")]
        public void GivenTestMethodHasSomeObjectParameters_WhenTestRun_ThenParametersAreGenerated(IFakeObjectUnderTest value)
        {
            // Arrange
            // Act
            // Assert
            value.Should().NotBeNull();
            value.StringProperty.Should().NotBeNullOrEmpty();
        }

        protected void MethodUnderTest()
        {
            // Empty method under test
        }
    }
}
