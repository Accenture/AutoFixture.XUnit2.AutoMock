namespace Objectivity.AutoFixture.XUnit2.Core.Tests.Attributes
{
    using System;
    using System.Diagnostics.CodeAnalysis;
    using System.Linq;
    using System.Reflection;
    using FluentAssertions;
    using global::AutoFixture;
    using Objectivity.AutoFixture.XUnit2.Core.Attributes;
    using Xunit;

    [Collection("AutoDataAdapterAttribute")]
    [Trait("Category", "Attributes")]
    [SuppressMessage("Design", "CA1034:Nested types should not be visible", Justification = "Test objects")]
    public class AutoDataAdapterAttributeTests
    {
        [Fact(DisplayName = "GIVEN fixture WHEN constructor is invoked THEN passed fixture is being adapted and inline values collection is empty")]
        public void GivenFixture_WhenConstructorIsInvoked_ThenPassedFixtureIsBeingAdaptedAndInlineValuesCollectionIsEmpty()
        {
            // Arrange
            IFixture fixture = new Fixture();

            // Act
            var attribute = new AutoDataAdapterAttribute(fixture);

            // Assert
            attribute.AdaptedFixture.Should().Be(fixture);
            attribute.InlineValues.Should().BeEmpty();
        }

        [Fact(DisplayName = "GIVEN uninitialized fixture WHEN constructor is invoked THEN exception is thrown")]
        public void GivenUninitializedFixture_WhenConstructorIsInvoked_ThenExceptionIsThrown()
        {
            // Arrange
            const IFixture fixture = null;

            // Act
            // Assert
            Assert.Throws<ArgumentNullException>(() => new AutoDataAdapterAttribute(fixture));
        }

        [Fact(DisplayName = "GIVEN test data with instance WHEN GetData called THEN auto data generation skipped")]
        public void GivenTestDataWithInstance_WhenGetDataCalled_ThenAutoDataGenerationSkipped()
        {
            // Arrange
            IFixture fixture = new Fixture();
            var attribute = new AutoDataAdapterAttribute(fixture, SpecificTestClass.Create());
            var methodInfo = typeof(AutoDataAdapterAttributeTests).GetMethod(nameof(this.TestMethodWithAbstractTestClass), BindingFlags.Instance | BindingFlags.NonPublic);

            // Act
            var data = attribute.GetData(methodInfo);

            // Assert
            data.Should().HaveCount(1)
                .And.Subject.First().Should().HaveCount(methodInfo.GetParameters().Length)
                .And.NotContainNulls();
        }

        [Fact(DisplayName = "GIVEN empty test data WHEN GetData called THEN auto data generation throws exception")]
        public void GivenEmptyTestData_WhenGetDataCalled_ThenAutoDataGenerationSkipped()
        {
            // Arrange
            IFixture fixture = new Fixture();
            var attribute = new AutoDataAdapterAttribute(fixture);
            var methodInfo = typeof(AutoDataAdapterAttributeTests).GetMethod(nameof(this.TestMethodWithAbstractTestClass), BindingFlags.Instance | BindingFlags.NonPublic);

            // Act
            Action data = () => attribute.GetData(methodInfo);

            // Assert
            data.Should().Throw<Exception>();
        }

        protected string TestMethodWithAbstractTestClass(SpecificTestClass instance, string text)
        {
            return $"{instance}: {text}";
        }

        public abstract class AbstractTestClass
        {
        }

        public class SpecificTestClass : AbstractTestClass
        {
            private SpecificTestClass()
            {
            }

            public static AbstractTestClass Create()
            {
                return new SpecificTestClass();
            }
        }
    }
}
