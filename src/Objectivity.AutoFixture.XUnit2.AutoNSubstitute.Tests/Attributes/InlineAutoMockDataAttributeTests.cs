namespace Objectivity.AutoFixture.XUnit2.AutoNSubstitute.Tests.Attributes
{
    using System.Collections.Generic;
    using System.Reflection;

    using global::AutoFixture;
    using global::AutoFixture.AutoNSubstitute;
    using global::AutoFixture.Xunit2;
    using NSubstitute;

    using Objectivity.AutoFixture.XUnit2.AutoNSubstitute.Attributes;
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
            Assert.Equal(initialValues, attribute.Values);
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
            var fixture = Substitute.For<IFixture>();
            var customizations = new List<ICustomization>();
            fixture.Customize(Arg.Do<ICustomization>(customizations.Add))
                .Returns(fixture);
            var dataAttribute = Substitute.For<DataAttribute>();
            dataAttribute.GetData(Arg.Any<MethodInfo>()).Returns(data);
            var provider = Substitute.For<IAutoFixtureInlineAttributeProvider>();
            provider.GetAttribute(Arg.Any<IFixture>()).Returns(dataAttribute);
            var attribute = new InlineAutoMockDataAttribute(fixture, provider)
            {
                IgnoreVirtualMembers = ignoreVirtualMembers,
            };
            var methodInfo = typeof(InlineAutoMockDataAttributeTests).GetMethod(nameof(this.MethodUnderTest), BindingFlags.Instance | BindingFlags.NonPublic);

            // Act
            var result = attribute.GetData(methodInfo);

            // Assert
            Assert.Same(data, result);
            provider.Received(1).GetAttribute(Arg.Any<IFixture>());
            dataAttribute.Received(1).GetData(Arg.Any<MethodInfo>());

            Assert.Equal(2, customizations.Count);
            var customization = Assert.IsType<AutoDataCommonCustomization>(customizations[0]);
            Assert.Equal(ignoreVirtualMembers, customization.IgnoreVirtualMembers);
            Assert.IsType<AutoNSubstituteCustomization>(customizations[1]);
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
            Assert.NotEmpty(objectInstance.StringProperty);
        }

        protected void MethodUnderTest()
        {
            // Empty method under test
        }
    }
}
