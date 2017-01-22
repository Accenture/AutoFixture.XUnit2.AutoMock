namespace Objectivity.AutoFixture.XUnit2.AutoMoq.Tests.Attributes
{
    using System;
    using System.Linq.Expressions;
    using System.Reflection;
    using AutoMoq.Attributes;
    using AutoMoq.Providers;
    using FluentAssertions;
    using Moq;
    using Ploeh.AutoFixture;
    using Xunit;
    using Xunit.Sdk;

    [Collection("InlineAutoMoqDataAttribute")]
    [Trait("Category", "Attributes")]
    public class InlineAutoMoqDataAttributeTests
    {
        [Fact(DisplayName = "WHEN parameterless constructor is invoked THEN has no values but fixture and attribute provider are created")]
        public void WhenParameterlessConstructorIsInvoked_ThenHasNoValuesButFixtureAndAttributeProviderAreCreated()
        {
            // Arrange
            // Act
            var attribute = new InlineAutoMoqDataAttribute();

            // Assert
            attribute.Fixture.Should().NotBeNull();
            attribute.Provider.Should().NotBeNull();
            attribute.Values.Should().HaveCount(0);
        }

        [Fact(DisplayName = "GIVEN existing inline values WHEN constructor is invoked THEN has specified values and fixture and attribute provider are created")]
        public void GivenExistingInlineValues_WhenConstructorIsInvoked_ThenHasSpecifiedValuesAndFixtureAndAttributeProviderAreCreated()
        {
            // Arrange
            var initialValues = new[] {"test", 1, new object()};

            // Act
            var attribute = new InlineAutoMoqDataAttribute(initialValues[0], initialValues[1], initialValues[2]);

            // Assert
            attribute.Fixture.Should().NotBeNull();
            attribute.Provider.Should().NotBeNull();
            attribute.Values.Should().BeEquivalentTo(initialValues);
        }

        [Fact(DisplayName = "GIVEN uninitialized values WHEN constructor is invoked THEN has no values and fixture and attribute provider are created")]
        public void GivenUninitializedValues_WhenConstructorIsInvoked_ThenHasNoValuesAndFixtureAndAttributeProviderAreCreated()
        {
            // Arrange
            const object[] initialValues = null;

            // Act
            var attribute = new InlineAutoMoqDataAttribute(initialValues);

            // Assert
            attribute.Fixture.Should().NotBeNull();
            attribute.Provider.Should().NotBeNull();
            attribute.Values.Should().HaveCount(0);
        }

        [Fact(DisplayName = "GIVEN existing fixture and attribute provider WHEN constructor is invoked THEN has fixture attribute provider and no values")]
        public void GivenExistingFixtureAndAttributeProvider_WhenConstructorIsInvoked_ThenHasFixtureAttributeProviderAndNoValues()
        {
            // Arrange
            var fixture = new Fixture();
            var provider = new InlineAutoDataAttributeProvider();


            // Act
            var attribute = new InlineAutoMoqDataAttribute(fixture, provider);

            // Assert
            attribute.Fixture.Should().Be(fixture);
            attribute.Provider.Should().Be(provider);
            attribute.Values.Should().HaveCount(0);
        }

        [Fact(DisplayName = "GIVEN existing fixture, attribute provider and values WHEN constructor is invoked THEN has specified fixture, attribute provider and values")]
        public void GivenExistingFixtureAttributeProviderAndValues_WhenConstructorIsInvoked_ThenHasSpecifiedFixtureAttributeProviderAndValues()
        {
            // Arrange
            var fixture = new Fixture();
            var provider = new InlineAutoDataAttributeProvider();
            var initialValues = new[] { "test", 1, new object() };

            // Act
            var attribute = new InlineAutoMoqDataAttribute(fixture, provider, initialValues[0], initialValues[1], initialValues[2]);

            // Assert
            attribute.Fixture.Should().Be(fixture);
            attribute.Provider.Should().Be(provider);
            attribute.Values.Should().BeEquivalentTo(initialValues);
        }

        [Fact(DisplayName = "GIVEN existing fixture, attribute provider and uninitialized values WHEN constructor is invoked THEN has specified fixture, attribute provider and no values")]
        public void GivenExistingFixtureAttributeProviderAndUninitializedValues_WhenConstructorIsInvoked_ThenHasSpecifiedFixtureAttributeProviderAndNoValues()
        {
            // Arrange
            var fixture = new Fixture();
            var provider = new InlineAutoDataAttributeProvider();
            const object[] initialValues = null;

            // Act
            var attribute = new InlineAutoMoqDataAttribute(fixture, provider, initialValues);

            // Assert
            attribute.Fixture.Should().Be(fixture);
            attribute.Provider.Should().Be(provider);
            attribute.Values.Should().HaveCount(0);
        }

        [Fact(DisplayName = "GIVEN uninitialized fixture WHEN constructor is invoked THEN exception is thrown")]
        public void GivenUninitializedFixture_WhenConstructorIsInvoked_ThenExceptionIsThrown()
        {
            // Arrange
            const Fixture fixture = null;
            var provider = new InlineAutoDataAttributeProvider();

            // Act
            // Assert
            Assert.Throws<ArgumentNullException>(() => new InlineAutoMoqDataAttribute(fixture, provider));
        }

        [Fact(DisplayName = "GIVEN uninitialized attribute provider WHEN constructor is invoked THEN exception is thrown")]
        public void GivenUninitializedAttributeProvider_WhenConstructorIsInvoked_ThenExceptionIsThrown()
        {
            // Arrange
            var fixture = new Fixture();
            const InlineAutoDataAttributeProvider provider = null;

            // Act
            // Assert
            Assert.Throws<ArgumentNullException>(() => new InlineAutoMoqDataAttribute(fixture, provider));
        }

        [Fact(DisplayName = "WHEN GetData is invoked THEN fixture is configured and data returned")]
        public void WhenGetDataIsInvoked_ThenFixtureIsConfiguredAndDataReturned()
        {
            // Arrange
            var data = new[]
            {
                new object[] {1, 2, 3},
                new object[] {4, 5, 6},
                new object[] {7, 8, 9}
            };
            var fixture = new Mock<IFixture>();
            Expression<Action<IFixture>> customizeExpression = f => f.Customize(It.IsAny<ICustomization>());
            fixture.Setup(customizeExpression);
            var dataAttribute = new Mock<DataAttribute>();
            dataAttribute.Setup(a => a.GetData(It.IsAny<MethodInfo>())).Returns(data);
            var provider = new Mock<IAutoFixtureInlineAttributeProvider>();
            provider.Setup(p => p.GetAttribute(It.IsAny<IFixture>())).Returns(dataAttribute.Object);
            var attribute = new InlineAutoMoqDataAttribute(fixture.Object, provider.Object);
            var methodInfo = typeof(AutoMoqDataAttributeTests).GetMethod("TestMethod");

            // Act
            var result = attribute.GetData(methodInfo);

            // Assert
            result.Should().BeSameAs(data);
            fixture.Verify(customizeExpression, Times.Once);
            provider.VerifyAll();
            dataAttribute.VerifyAll();
        }

        [InlineAutoMoqData(100)]
        [Theory(DisplayName = "GIVEN test method has some inline parameters WHEN test run THEN parameters are generated")]
        public void GivenTestMethodHasSomeInlineParameters_WhenTestRun_ThenParametersAreGenerated(int value, IDisposable disposable)
        {
            // Arrange
            // Act
            // Assert
            value.Should().Be(100);

            disposable.Should().NotBeNull();
            disposable.GetType().Name.Should().StartWith("ObjectProxy", "that way we know it was mocked with MOQ.");
        }
    }
}
