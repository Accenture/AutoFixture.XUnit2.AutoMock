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

    [Collection("InlineAutoMockDataAttribute")]
    [Trait("Category", "DataAttribute")]
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
            var provider = new Mock<IAutoFixtureInlineAttributeProvider>();
            provider.Setup(p => p.GetAttribute(It.IsAny<IFixture>())).Returns(dataAttribute.Object);
            var attribute = new InlineAutoMockDataAttribute(fixture.Object, provider.Object)
            {
                IgnoreVirtualMembers = ignoreVirtualMembers,
            };
            var methodInfo = typeof(InlineAutoMockDataAttributeTests).GetMethod(nameof(this.MethodUnderTest), BindingFlags.Instance | BindingFlags.NonPublic);

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

        [InlineAutoMockData(100)]
        [Theory(DisplayName = "GIVEN test method has some inline parameters WHEN test run THEN parameters are generated")]
        public void GivenTestMethodHasSomeInlineParameters_WhenTestRun_ThenParametersAreGenerated(
            int firstValueInstance,
            int secondValueInstance,
            IFakeObjectUnderTest objectInstance)
        {
            // Arrange
            // Act
            // Assert
            firstValueInstance.Should().Be(100);
            secondValueInstance.Should().NotBe(0);

            objectInstance.Should().NotBeNull();
            objectInstance.StringProperty.Should().NotBeNullOrEmpty();
        }

        protected void MethodUnderTest()
        {
            // Empty method under test
        }
    }
}
