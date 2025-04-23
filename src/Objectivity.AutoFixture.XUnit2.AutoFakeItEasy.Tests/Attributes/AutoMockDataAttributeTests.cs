namespace Objectivity.AutoFixture.XUnit2.AutoFakeItEasy.Tests.Attributes
{
    using System.Collections.Generic;
    using System.Reflection;

    using FakeItEasy;

    using global::AutoFixture;
    using global::AutoFixture.AutoFakeItEasy;
    using global::AutoFixture.Xunit2;
    using Objectivity.AutoFixture.XUnit2.AutoFakeItEasy.Attributes;
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
            Assert.NotNull(attribute.Fixture);
            Assert.NotNull(attribute.Provider);
            Assert.False(attribute.IgnoreVirtualMembers);
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
            var fixture = A.Fake<IFixture>();
            var customizations = new List<ICustomization>();
            A.CallTo(() => fixture.Customize(A<ICustomization>._))
                .Invokes((ICustomization customization) => customizations.Add(customization))
                .Returns(fixture);
            var dataAttribute = A.Fake<DataAttribute>();
            A.CallTo(() => dataAttribute.GetData(A<MethodInfo>._)).Returns(data);
            var provider = A.Fake<IAutoFixtureAttributeProvider>();
            A.CallTo(() => provider.GetAttribute(A<IFixture>._)).Returns(dataAttribute);
            var attribute = new AutoMockDataAttribute(fixture, provider)
            {
                IgnoreVirtualMembers = ignoreVirtualMembers,
            };
            var methodInfo = typeof(AutoMockDataAttributeTests).GetMethod(nameof(this.MethodUnderTest), BindingFlags.Instance | BindingFlags.NonPublic);

            // Act
            var result = attribute.GetData(methodInfo);

            // Assert
            Assert.Same(data, result);
            A.CallTo(() => provider.GetAttribute(A<IFixture>._)).MustHaveHappenedOnceExactly();
            A.CallTo(() => dataAttribute.GetData(A<MethodInfo>._)).MustHaveHappenedOnceExactly();

            Assert.Equal(2, customizations.Count);
            var customization = Assert.IsType<AutoDataCommonCustomization>(customizations[0]);
            Assert.Equal(ignoreVirtualMembers, customization.IgnoreVirtualMembers);
            Assert.IsType<AutoFakeItEasyCustomization>(customizations[1]);
        }

        [AutoMockData]
        [Theory(DisplayName = "GIVEN test method has some value parameters WHEN test run THEN parameters are generated")]
        public void GivenTestMethodHasSomeValueParameters_WhenTestRun_ThenParametersAreGenerated(int value)
        {
            // Arrange
            // Act
            // Assert
            Assert.NotEqual(0, value);
        }

        [AutoMockData]
        [Theory(DisplayName = "GIVEN test method has some object parameters WHEN test run THEN parameters are generated")]
        public void GivenTestMethodHasSomeObjectParameters_WhenTestRun_ThenParametersAreGenerated(IFakeObjectUnderTest value)
        {
            // Arrange
            // Act
            // Assert
            Assert.NotNull(value);
            Assert.NotNull(value.StringProperty);
            Assert.NotEmpty(value.StringProperty);
        }

        protected void MethodUnderTest()
        {
            // Empty method under test
        }
    }
}
