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
            Assert.NotNull(attribute.Fixture);
            Assert.False(attribute.IgnoreVirtualMembers);
            Assert.NotNull(attribute.Provider);
            Assert.Empty(attribute.Values);
        }

        [Fact(DisplayName = "GIVEN existing inline values WHEN constructor is invoked THEN has specified values and fixture and attribute provider are created")]
        public void GivenExistingInlineValues_WhenConstructorIsInvoked_ThenHasSpecifiedValuesAndFixtureAndAttributeProviderAreCreated()
        {
            // Arrange
            var initialValues = new[] { "test", 1, new object() };

            // Act
            var attribute = new InlineAutoMockDataAttribute(initialValues[0], initialValues[1], initialValues[2]);

            // Assert
            Assert.NotNull(attribute.Fixture);
            Assert.False(attribute.IgnoreVirtualMembers);
            Assert.NotNull(attribute.Provider);
            Assert.Equivalent(initialValues, attribute.Values);
        }

        [Fact(DisplayName = "GIVEN uninitialized values WHEN constructor is invoked THEN has no values and fixture and attribute provider are created")]
        public void GivenUninitializedValues_WhenConstructorIsInvoked_ThenHasNoValuesAndFixtureAndAttributeProviderAreCreated()
        {
            // Arrange
            const object[] initialValues = null;

            // Act
            var attribute = new InlineAutoMockDataAttribute(initialValues);

            // Assert
            Assert.NotNull(attribute.Fixture);
            Assert.False(attribute.IgnoreVirtualMembers);
            Assert.NotNull(attribute.Provider);
            Assert.Empty(attribute.Values);
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
            var provider = A.Fake<IAutoFixtureInlineAttributeProvider>();
            A.CallTo(() => provider.GetAttribute(A<IFixture>._)).Returns(dataAttribute);
            var attribute = new InlineAutoMockDataAttribute(fixture, provider)
            {
                IgnoreVirtualMembers = ignoreVirtualMembers,
            };
            var methodInfo = typeof(InlineAutoMockDataAttributeTests).GetMethod(nameof(this.MethodUnderTest), BindingFlags.Instance | BindingFlags.NonPublic);

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
            Assert.Equal(100, firstValueInstance);
            Assert.NotEqual(0, secondValueInstance);

            Assert.NotNull(objectInstance);
            Assert.NotNull(objectInstance.StringProperty);
        }

        protected void MethodUnderTest()
        {
            // Empty method under test
        }
    }
}
