namespace Objectivity.AutoFixture.XUnit2.Core.Tests.Attributes
{
    using System;
    using System.Linq;
    using System.Reflection;

    using FluentAssertions;

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
            Func<object> act = () => new ExceptAttribute(null);

            // Assert
            act.Should().Throw<ArgumentNullException>()
                .And.ParamName.Should().Be("values");
        }

        [Fact(DisplayName = "GIVEN no arguments WHEN constructor is invoked THEN exception is thrown")]
        public void GivenNoArguments_WhenConstructorIsInvoked_ThenExceptionIsThrown()
        {
            // Arrange
            // Act
            Func<object> act = () => new ExceptAttribute();

            // Assert
            act.Should().Throw<ArgumentException>()
                .And.Message.Should().NotBeNullOrEmpty()
                .And.Contain("At least one value");
        }

        [AutoData]
        [Theory(DisplayName = "GIVEN uninitialized argument WHEN GetCustomization is invoked THEN exception is thrown")]
        public void GivenUninitializedArgument_WhenGetCustomizationIsInvoked_ThenExceptionIsThrown(
            int[] values)
        {
            // Arrange
            var attribute = new ExceptAttribute(values);

            // Act
            Action act = () => attribute.GetCustomization(null);

            // Assert
            act.Should().Throw<ArgumentNullException>()
                .And.ParamName.Should().Be("parameter");
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
            attribute.Values.Should().HaveCount(1).And.BeEquivalentTo(new[] { first });
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
            attribute.Values.Should().HaveCount(2).And.BeEquivalentTo(new[] { first, second });
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
            result.Should().NotBeNull()
                .And.BeOfType(expectedType)
                .And.NotBe(item);
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
            targetValues.Should().AllSatisfy(x => x.Should().Be(Numbers.Five));
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
            firstSet.Should().AllSatisfy(x => x.Should().Be(Numbers.Five));
            secondSet.Where(x => x != Numbers.Five).Should().HaveCountGreaterThan(1);
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
            // Act
            // Assert
            firstSet.Should().AllSatisfy(x => x.Should().BeOneOf(Numbers.Three, Numbers.Four, Numbers.Five));
            secondSet.Should().AllSatisfy(x => x.Should().BeOneOf(Numbers.None, Numbers.One, Numbers.Two));
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
            firstValue.Should().Be(Numbers.Five);
            secondValue.Should().Be(firstValue);
        }
    }
}
