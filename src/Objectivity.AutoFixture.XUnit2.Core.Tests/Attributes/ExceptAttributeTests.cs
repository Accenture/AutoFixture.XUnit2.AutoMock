namespace Objectivity.AutoFixture.XUnit2.Core.Tests.Attributes
{
    using System;
    using System.Linq;
    using System.Reflection;

    using global::AutoFixture;
    using global::AutoFixture.Kernel;
    using global::AutoFixture.Xunit2;

    using Moq;

    using Objectivity.AutoFixture.XUnit2.Core.Attributes;

    using Xunit;

    [Collection("ExceptAttribute")]
    [Trait("Category", "CustomizeAttribute")]
    public class ExceptAttributeTests
    {
        public enum Numbers
        {
            None = 0,
            One = 1,
            Two = 2,
            Three = 4,
            Four = 8,
            Five = 16,
        }

        public static TheoryData<object> CustomizationUsageTestData { get; } = new()
        {
            { 1 },
            { 1.5f },
            { "test" },
            { false },
            { Numbers.Five },
            { DateTime.UtcNow },
            { ValueTuple.Create(5) },
            { Tuple.Create(1, 2) },
        };

        [Fact(DisplayName = "GIVEN uninitialized argument WHEN constructor is invoked THEN exception is thrown")]
        public void GivenUninitializedArgument_WhenConstructorIsInvoked_ThenExceptionIsThrown()
        {
            // Arrange
            // Act
            static object Act() => new ExceptAttribute(null);

            // Assert
            var exception = Assert.Throws<ArgumentNullException>(Act);
            Assert.Equal("values", exception.ParamName);
        }

        [Fact(DisplayName = "GIVEN no arguments WHEN constructor is invoked THEN exception is thrown")]
        public void GivenNoArguments_WhenConstructorIsInvoked_ThenExceptionIsThrown()
        {
            // Arrange
            // Act
            static object Act() => new ExceptAttribute();

            // Assert
            var exception = Assert.Throws<ArgumentException>(Act);
            Assert.NotNull(exception.Message);
            Assert.NotEmpty(exception.Message);
            Assert.Contains("At least one value", exception.Message);
        }

        [AutoData]
        [Theory(DisplayName = "GIVEN uninitialized argument WHEN GetCustomization is invoked THEN exception is thrown")]
        public void GivenUninitializedArgument_WhenGetCustomizationIsInvoked_ThenExceptionIsThrown(
            int[] values)
        {
            // Arrange
            var attribute = new ExceptAttribute(values);

            // Act
            void Act() => attribute.GetCustomization(null);

            // Assert
            var exception = Assert.Throws<ArgumentNullException>(Act);
            Assert.Equal("parameter", exception.ParamName);
        }

        [InlineData(1, 1)]
        [InlineData("a", "a")]
        [Theory(DisplayName = "GIVEN identical arguments WHEN constructor is invoked THEN unique parameters are properly assigned")]
        public void GivenIdenticalArguments_WhenConstructorIsInvoked_ThenUniqueParametersAreProperlyAssigned<T>(
            T first,
            T second)
        {
            // Arrange
            var attribute = new ExceptAttribute(first, second);

            // Assert
            Assert.Single(attribute.Values);
            Assert.Equivalent(new[] { first }, attribute.Values);
        }

        [InlineData(typeof(int), 2)]
        [InlineData(1, typeof(int))]
        [Theory(DisplayName = "GIVEN incomparable argument WHEN constructor is invoked THEN parameters are properly assigned")]
        public void GivenIncomparableArgument_WhenConstructorIsInvoked_ThenParametersAreProperlyAssigned(
            object first,
            object second)
        {
            // Arrange
            // Act
            var attribute = new ExceptAttribute(first, second);

            // Assert
            Assert.Equal(2, attribute.Values.Count);
            Assert.Equivalent(new[] { first, second }, attribute.Values);
        }

        [MemberData(nameof(CustomizationUsageTestData))]
        [Theory(DisplayName = "GIVEN valid parameters WHEN customization is used THEN expected values are generated")]
        public void GivenValidParameters_WhenCustomizationIsUsed_ThenExpectedValuesAreGenerated<T>(
            T item)
        {
            // Arrange
            var attribute = new ExceptAttribute(item);
            var request = new Mock<ParameterInfo>();
            var expectedType = item.GetType();
            request.SetupGet(x => x.ParameterType)
                .Returns(expectedType);
            IFixture fixture = new Fixture();
            fixture.Customize(attribute.GetCustomization(request.Object));

            // Act
            var result = fixture.Create(request.Object, new SpecimenContext(fixture));

            // Assert
            Assert.NotNull(result);
            Assert.IsType(expectedType, result);
            Assert.NotEqual(item, result);
        }

        [AutoData]
        [Theory(DisplayName = "GIVEN multiple values specified but one WHEN values populated THEN the one value is used")]
        public void GivenMultipleValuesSpecifiedButOne_WhenValuesPopulated_ThenTheOneValueIsUsed(
            [Except(
                Numbers.None,
                Numbers.One,
                Numbers.Two,
                Numbers.Three,
                Numbers.Four)] Numbers[] targetValues)
        {
            // Arrange
            // Act
            // Assert
            Assert.All(targetValues, x => Assert.Equal(Numbers.Five, x));
        }

        [AutoData]
        [Theory(DisplayName = "GIVEN values specified only for one set WHEN values populated THEN the other set is not impacted")]
        public void GivenValuesSpecifiedOnlyForOneSet_WhenValuesPopulated_ThenTheOtherSetIsNotImpacted(
            [Except(
                Numbers.None,
                Numbers.One,
                Numbers.Two,
                Numbers.Three,
                Numbers.Four)] Numbers[] firstSet,
            Numbers[] secondSet)
        {
            // Arrange
            // Act
            // Assert
            Assert.All(firstSet, x => Assert.Equal(Numbers.Five, x));
            Assert.True(secondSet.Count(x => x != Numbers.Five) > 1);
        }

        [AutoData]
        [Theory(DisplayName = "GIVEN multiple values specified but one WHEN values populated THEN generated sets have no common part")]
        public void GivenDifferentValuesSpecifiedForDifferentSets_WhenValuesPopulated_ThenGeneratedSetsHaveNoCommonPart(
            [Except(
                Numbers.None,
                Numbers.One,
                Numbers.Two)] Numbers[] firstSet,
            [Except(
                Numbers.Three,
                Numbers.Four,
                Numbers.Five)] Numbers[] secondSet)
        {
            // Arrange
            var expectedFirstSet = new[] { Numbers.Three, Numbers.Four, Numbers.Five };
            var expectedSecondSet = new[] { Numbers.None, Numbers.One, Numbers.Two };

            // Act
            // Assert
            Assert.All(firstSet, x => Assert.Contains(x, expectedFirstSet));
            Assert.All(secondSet, x => Assert.Contains(x, expectedSecondSet));
        }

        [AutoData]
        [Theory(DisplayName = "GIVEN values specified only for one set WHEN values populated THEN the other set is not impacted")]
        public void GivenValuesSpecifiedForFirstValueAndFrozen_WhenValuesPopulated_ThenBothValuesAreImpacted(
            [Frozen] [Except(
                Numbers.None,
                Numbers.One,
                Numbers.Two,
                Numbers.Three,
                Numbers.Four)] Numbers firstValue,
            Numbers secondValue)
        {
            // Arrange
            // Act
            // Assert
            Assert.Equal(Numbers.Five, firstValue);
            Assert.Equal(firstValue, secondValue);
        }
    }
}
