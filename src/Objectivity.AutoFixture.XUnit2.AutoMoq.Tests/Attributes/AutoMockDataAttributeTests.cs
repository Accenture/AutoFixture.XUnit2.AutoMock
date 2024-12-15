namespace Objectivity.AutoFixture.XUnit2.AutoMoq.Tests.Attributes
{
    using System.Collections.Generic;
    using System.Reflection;

    using FluentAssertions;

    using global::AutoFixture;
    using global::AutoFixture.AutoMoq;
    using global::AutoFixture.Xunit2;
    using Moq;

    using Objectivity.AutoFixture.XUnit2.AutoMoq.Attributes;
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
            var fixture = new Mock<IFixture>();
            var customizations = new List<ICustomization>();
            fixture.Setup(x => x.Customize(It.IsAny<ICustomization>()))
                .Callback<ICustomization>(customizations.Add)
                .Returns(fixture.Object);
            var dataAttribute = new Mock<DataAttribute>();
            dataAttribute.Setup(a => a.GetData(It.IsAny<MethodInfo>())).Returns(data);
            var provider = new Mock<IAutoFixtureAttributeProvider>();
            provider.Setup(p => p.GetAttribute(It.IsAny<IFixture>())).Returns(dataAttribute.Object);
            var attribute = new AutoMockDataAttribute(fixture.Object, provider.Object)
            {
                IgnoreVirtualMembers = ignoreVirtualMembers,
            };
            var methodInfo = typeof(AutoMockDataAttributeTests).GetMethod(nameof(this.MethodUnderTest), BindingFlags.Instance | BindingFlags.NonPublic);

            // Act
            var result = attribute.GetData(methodInfo);

            // Assert
            result.Should().BeSameAs(data);
            provider.VerifyAll();
            dataAttribute.VerifyAll();

            customizations.Count.Should().Be(2);
            customizations[0]
                .Should()
                .BeOfType<AutoDataCommonCustomization>()
                .Which.IgnoreVirtualMembers.Should()
                .Be(ignoreVirtualMembers);
            customizations[1].Should().BeOfType<AutoMoqCustomization>();
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
