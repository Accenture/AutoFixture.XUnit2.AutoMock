﻿namespace Objectivity.AutoFixture.XUnit2.Core.Tests.Attributes
{
    using System;
    using System.Linq;

    using FluentAssertions;

    using global::AutoFixture.Xunit2;

    using Objectivity.AutoFixture.XUnit2.Core.Attributes;

    using Xunit;

    [Collection("ExceptAttribute")]
    [Trait("Category", "Attributes")]
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

        [Fact(DisplayName = "GIVEN uninitialized argument WHEN constructor is invoked THEN exception is thrown")]
        public void GivenUninitializedArgument_WhenConstructorIsInvoked_ThenExceptionIsThrown()
        {
            // Arrange
            // Act
            // Assert
            Assert.Throws<ArgumentNullException>(() => new ExceptAttribute(null));
        }

        [Fact(DisplayName = "GIVEN no arguments WHEN constructor is invoked THEN exception is thrown")]
        public void GivenNoArguments_WhenConstructorIsInvoked_ThenExceptionIsThrown()
        {
            // Arrange
            // Act
            // Assert
            Assert.Throws<ArgumentException>(() => new ExceptAttribute());
        }

        [InlineData(typeof(int), 2)]
        [InlineData(1, typeof(int))]
        [Theory(DisplayName = "GIVEN uncomparable argument WHEN constructor is invoked THEN exception is thrown")]
        public void GivenUncomparableArgument_WhenConstructorIsInvoked_ThenExceptionIsThrown(object first, object second)
        {
            // Arrange
            // Act
            // Assert
            Assert.Throws<ArgumentException>(() => new ExceptAttribute(first, second));
        }

        [Fact(DisplayName = "GIVEN valid parameters WHEN constructor is invoked THEN parameters are propelry assigned")]
        public void GivenValidParameters_WhenConstructorIsInvoked_ThenParametersAreProperlyAssigned()
        {
            // Arrange
            const int item = 1;

            // Act
            var attribute = new ExceptAttribute(item);

            // Assert
            attribute.Values.Should().HaveCount(1).And.Contain(item);
        }

        [AutoData]
        [Theory(DisplayName = "GIVEN multiple values specified but one WHEN values populated THEN the one value is used")]
        public void GivenMultipleValuesSpecifiedButOne_WhenValuesPopulated_ThenTheOneValueIsUsed(
            [Except(Numbers.None, Numbers.One, Numbers.Two, Numbers.Three, Numbers.Four)] Numbers[] targetValues)
        {
            // Arrange
            // Act
            // Assert
            targetValues.Should().AllSatisfy(x => x.Should().Be(Numbers.Five));
        }

        [AutoData]
        [Theory(DisplayName = "GIVEN values specified only for one set WHEN values populated THEN the other set is not impacted")]
        public void GivenValuesSpecifiedOnlyForOneSet_WhenValuesPopulated_ThenTheOtherSetIsNotImpacted(
            [Except(Numbers.None, Numbers.One, Numbers.Two, Numbers.Three, Numbers.Four)] Numbers[] firstSet,
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
            [Except(Numbers.None, Numbers.One, Numbers.Two)] Numbers[] firstSet,
            [Except(Numbers.Three, Numbers.Four, Numbers.Five)] Numbers[] secondSet)
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
            [Except(Numbers.None, Numbers.One, Numbers.Two, Numbers.Three, Numbers.Four)][Frozen] Numbers firstValue,
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
