namespace Objectivity.AutoFixture.XUnit2.Core.Tests.Attributes
{
    using System;
    using System.Diagnostics.CodeAnalysis;
    using System.Linq;
    using System.Reflection;

    using FluentAssertions;

    using global::AutoFixture;
    using global::AutoFixture.Kernel;
    using global::AutoFixture.Xunit2;
    using Objectivity.AutoFixture.XUnit2.Core.Attributes;

    using Xunit;

    [Collection("AutoDataAdapterAttribute")]
    [Trait("Category", "DataAttribute")]
    [SuppressMessage("Design", "CA1034:Nested types should not be visible", Justification = "Test objects")]
    public class AutoDataAdapterAttributeTests
    {
        [AutoData]
        [Theory(DisplayName = "GIVEN fixture WHEN constructor is invoked THEN passed fixture is being adapted and inline values collection is empty")]
        public void GivenFixture_WhenConstructorIsInvoked_ThenPassedFixtureIsBeingAdaptedAndInlineValuesCollectionIsEmpty(Fixture fixture)
        {
            // Arrange
            // Act
            var attribute = new AutoDataAdapterAttribute(fixture, null);

            // Assert
            attribute.AdaptedFixture.Should().Be(fixture);
            attribute.InlineValues.Should().BeEmpty();
        }

        [Fact(DisplayName = "GIVEN uninitialized fixture WHEN constructor is invoked THEN exception is thrown")]
        public void GivenUninitializedFixture_WhenConstructorIsInvoked_ThenExceptionIsThrown()
        {
            // Arrange
            // Act
            // Assert
            Assert.Throws<ArgumentNullException>(() => new AutoDataAdapterAttribute(null));
        }

        [AutoData]
        [Theory(DisplayName = "GIVEN uninitialized method info WHEN GetData is invoked THEN exception is thrown")]
        public void GivenUninitializedMethodInfo_WhenConstructorIsInvoked_ThenExceptionIsThrown(Fixture fixture)
        {
            // Arrange
            var attribute = new AutoDataAdapterAttribute(fixture);

            // Act
            // Assert
            Assert.Throws<ArgumentNullException>(() => attribute.GetData(null));
        }

        [Fact(DisplayName = "GIVEN test data with instance WHEN GetData called THEN auto data generation skipped")]
        public void GivenTestDataWithInstance_WhenGetDataCalled_ThenAutoDataGenerationSkipped()
        {
            // Arrange
            IFixture fixture = new Fixture();
            var attribute = new AutoDataAdapterAttribute(fixture, SpecificTestClass.Create());
            var methodInfo = typeof(AutoDataAdapterAttributeTests).GetMethod(nameof(this.TestMethodWithAbstractTestClass), BindingFlags.Instance | BindingFlags.NonPublic);
            var expectedNumberOfParameters = methodInfo.GetParameters().Length;

            // Act
            var data = attribute.GetData(methodInfo).ToArray();

            // Assert
            data.Should().HaveCount(1)
                .And.Subject.First().Should().HaveCount(expectedNumberOfParameters)
                .And.NotContainNulls()
                .And.Subject.Skip(1).Should().AllBeEquivalentTo(data.First().Last());
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

        [Fact(DisplayName = "GIVEN test method with Frozen customization after others WHEN GetData called THEN ensure parameter is frozen on the end")]
        public void GivenTestMethodWithFrozenCustomizationAfterOthers_WhenGetDataCalled_ThenEnsureParameterIsFrozenOnTheEnd()
        {
            // Arrange
            IFixture fixture = new Fixture();
            var attribute = new AutoDataAdapterAttribute(fixture, null);
            var methodInfo = typeof(AutoDataAdapterAttributeTests).GetMethod(nameof(this.TestMethodWithFrozenCustomizationBeforeOthers), BindingFlags.Instance | BindingFlags.NonPublic);
            var expectedNumberOfParameters = methodInfo.GetParameters().Length;

            // Act
            var data = attribute.GetData(methodInfo).ToArray();

            // Assert
            data.Should().HaveCount(1)
                .And.Subject.First().Should().HaveCount(expectedNumberOfParameters)
                .And.NotContainNulls()
                .And.Subject.Skip(1).Should().AllSatisfy((x) =>
                {
                    var parameters = data.First();
                    x.Should().Be(parameters.Last())
                        .And.NotBe(parameters.First())
                        .And.Subject.As<string>().Should().Contain(StopStringCustomization.Value);
                });
        }

        protected string TestMethodWithAbstractTestClass(
            SpecificTestClass instance,
            [Frozen] string text,
            string message)
        {
            return $"{instance}: {text}, {message}";
        }

        protected string TestMethodWithFrozenCustomizationBeforeOthers(
            string first,
            [Frozen][StopString] string second,
            string third)
        {
            return $"{first}: {second}, {third}";
        }

        [SuppressMessage("Minor Code Smell", "S2094:Classes should not be empty", Justification = "Test class")]
        public abstract class AbstractTestClass
        {
        }

        public sealed class SpecificTestClass : AbstractTestClass
        {
            private SpecificTestClass()
            {
            }

            public static AbstractTestClass Create()
            {
                return new SpecificTestClass();
            }
        }

        protected sealed class StopStringAttribute : CustomizeWithAttribute<StopStringCustomization>
        {
        }

        protected class StopStringCustomization : ICustomization
        {
            public const string Value = "STOP";

            public void Customize(IFixture fixture)
            {
                fixture.Customizations.Add(
                    new FilteringSpecimenBuilder(
                        new FixedBuilder(Value),
                        new ExactTypeSpecification(Value.GetType())));
            }
        }
    }
}
