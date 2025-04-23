namespace Objectivity.AutoFixture.XUnit2.Core.Tests.Attributes
{
    using System;
    using System.Diagnostics.CodeAnalysis;
    using System.Linq;
    using System.Reflection;

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
            Assert.Equal(fixture, attribute.AdaptedFixture);
            Assert.Empty(attribute.InlineValues);
        }

        [Fact(DisplayName = "GIVEN uninitialized fixture WHEN constructor is invoked THEN exception is thrown")]
        public void GivenUninitializedFixture_WhenConstructorIsInvoked_ThenExceptionIsThrown()
        {
            // Arrange
            // Act
            static object Act() => new AutoDataAdapterAttribute(null);

            // Assert
            var exception = Assert.Throws<ArgumentNullException>(Act);
            Assert.Equal("fixture", exception.ParamName);
        }

        [AutoData]
        [Theory(DisplayName = "GIVEN uninitialized method info WHEN GetData is invoked THEN exception is thrown")]
        public void GivenUninitializedMethodInfo_WhenConstructorIsInvoked_ThenExceptionIsThrown(Fixture fixture)
        {
            // Arrange
            var attribute = new AutoDataAdapterAttribute(fixture);

            // Act
            object Act() => attribute.GetData(null);

            // Assert
            var exception = Assert.Throws<ArgumentNullException>(Act);
            Assert.Equal("testMethod", exception.ParamName);
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
            Assert.Single(data);
            Assert.Equal(expectedNumberOfParameters, data.First().Length);
            Assert.All(data.First(), Assert.NotNull);
            Assert.All(data.First().Skip(1), item => Assert.Equal(data.First().Last(), item));
        }

        [Fact(DisplayName = "GIVEN empty test data WHEN GetData called THEN auto data generation throws exception")]
        public void GivenEmptyTestData_WhenGetDataCalled_ThenAutoDataGenerationSkipped()
        {
            // Arrange
            IFixture fixture = new Fixture();
            var attribute = new AutoDataAdapterAttribute(fixture);
            var methodInfo = typeof(AutoDataAdapterAttributeTests).GetMethod(nameof(this.TestMethodWithAbstractTestClass), BindingFlags.Instance | BindingFlags.NonPublic);

            // Act
            void Act() => attribute.GetData(methodInfo);

            // Assert
            Assert.ThrowsAny<Exception>(Act);
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
            Assert.Single(data);
            Assert.Equal(expectedNumberOfParameters, data.First().Length);
            Assert.All(data.First(), Assert.NotNull);
            Assert.All(
                data.First().Skip(1),
                x =>
                {
                    var parameters = data.First();
                    Assert.Equal(parameters.Last(), x);
                    Assert.NotEqual(parameters.First(), x);
                    Assert.Contains(StopStringCustomization.Value, x as string);
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
        protected abstract class AbstractTestClass
        {
        }

        protected sealed class SpecificTestClass : AbstractTestClass
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

        [SuppressMessage("ReSharper", "ClassNeverInstantiated.Global", Justification = "This class is instantiated indirectly.")]
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
