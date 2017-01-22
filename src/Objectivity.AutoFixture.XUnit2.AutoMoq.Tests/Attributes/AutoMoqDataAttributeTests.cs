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

    [Collection("AutoMoqDataAttribute")]
    [Trait("Category", "Attributes")]
    public class AutoMoqDataAttributeTests
    {
        public void TestMethod()
        {
        }

        [Fact(DisplayName = "WHEN parameterless constructor is invoked THEN fixture and attribute provider are created")]
        public void WhenParameterlessConstructorIsInvoked_ThenFixtureAndAttributeProviderAreCreated()
        {
            // Arrange
            // Act
            var attribute = new AutoMoqDataAttribute();

            // Assert
            attribute.Fixture.Should().NotBeNull();
            attribute.Provider.Should().NotBeNull();
            attribute.IgnoreVirtualMembers.Should().BeFalse();
        }

        [Fact(DisplayName = "GIVEN existing fixture and attribute provider WHEN constructor is invoked THEN has specified fixture and attribute provider")]
        public void GivenExistingFixtureAndAttributeProvider_WhenConstructorIsInvoked_ThenHasSpecifiedFixtureAndAttributeProvider()
        {
            // Arrange
            var fixture = new Fixture();
            var provider = new AutoDataAttributeProvider();

            // Act
            var attribute = new AutoMoqDataAttribute(fixture, provider);

            // Assert
            attribute.Fixture.Should().Be(fixture);
            attribute.Provider.Should().Be(provider);
            attribute.IgnoreVirtualMembers.Should().BeFalse();
        }

        [Fact(DisplayName = "GIVEN uninitialized fixture WHEN constructor is invoked THEN exception is thrown")]
        public void GivenUninitializedFixture_WhenConstructorIsInvoked_ThenExceptionIsThrown()
        {
            // Arrange
            const Fixture fixture = null;
            var provider = new AutoDataAttributeProvider();

            // Act
            // Assert
            Assert.Throws<ArgumentNullException>(() => new AutoMoqDataAttribute(fixture, provider));
        }

        [Fact(DisplayName = "GIVEN uninitialized attribute provider WHEN constructor is invoked THEN exception is thrown")]
        public void GivenUninitializedAttributeProvider_WhenConstructorIsInvoked_ThenExceptionIsThrown()
        {
            // Arrange
            var fixture = new Fixture();
            const AutoDataAttributeProvider provider = null;

            // Act
            // Assert
            Assert.Throws<ArgumentNullException>(() => new AutoMoqDataAttribute(fixture, provider));
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
            var provider = new Mock<IAutoFixtureAttributeProvider>();
            provider.Setup(p => p.GetAttribute(It.IsAny<IFixture>())).Returns(dataAttribute.Object);
            var attribute = new AutoMoqDataAttribute(fixture.Object, provider.Object);
            var methodInfo = typeof(AutoMoqDataAttributeTests).GetMethod("TestMethod");

            // Act
            var result = attribute.GetData(methodInfo);

            // Assert
            result.Should().BeSameAs(data);
            fixture.Verify(customizeExpression, Times.Exactly(2));
            provider.VerifyAll();
            dataAttribute.VerifyAll();
        }

        [AutoMoqData]
        [Theory(DisplayName = "GIVEN test method has some parameters WHEN test run THEN parameters are generated")]
        public void GivenTestMethodHasSomeParameters_WhenTestRun_ThenParametersAreGenerated(int value, IDisposable disposable)
        {
            // Arrange
            // Act
            // Assert
            value.Should().NotBe(default(int));

            disposable.Should().NotBeNull();
            disposable.GetType().Name.Should().StartWith("ObjectProxy", "that way we know it was mocked with MOQ.");
        }
    }
}
