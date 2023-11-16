﻿namespace Objectivity.AutoFixture.XUnit2.Core.Tests.Attributes
{
    using System;
    using System.Collections.Generic;

    using FluentAssertions;

    using global::AutoFixture.Xunit2;

    using Objectivity.AutoFixture.XUnit2.Core.Attributes;

    using Xunit;

    [Collection("FromValuesAttribute")]
    [Trait("Category", "Attributes")]
    public class FromValuesAttributeTests
    {
        [Flags]
        public enum Numbers
        {
            None = 0,
            One = 1,
            Two = 2,
            Three = 4,
            Four = 8,
            Five = 16,
        }

        public static IEnumerable<object[]> TestData { get; } = new[]
        {
            new object[] { 10, 10 },
            new object[] { 0, 0 },
        };

        [Fact(DisplayName = "GIVEN uninitialized argument WHEN constructor is invoked THEN exception is thrown")]
        public void GivenUninitializedArgument_WhenConstructorIsInvoked_ThenExceptionIsThrown()
        {
            // Arrange
            // Act
            // Assert
            Assert.Throws<ArgumentNullException>(() => new FromValuesAttribute(null));
        }

        [Fact(DisplayName = "GIVEN no arguments WHEN constructor is invoked THEN exception is thrown")]
        public void GivenNoArguments_WhenConstructorIsInvoked_ThenExceptionIsThrown()
        {
            // Arrange
            // Act
            // Assert
            Assert.Throws<ArgumentException>(() => new FromValuesAttribute());
        }

        [InlineData(typeof(int), 2)]
        [InlineData(1, typeof(int))]
        [Theory(DisplayName = "GIVEN uncomparable argument WHEN constructor is invoked THEN exception is thrown")]
        public void GivenUncomparableArgument_WhenConstructorIsInvoked_ThenExceptionIsThrown(object first, object second)
        {
            // Arrange
            // Act
            // Assert
            Assert.Throws<ArgumentException>(() => new FromValuesAttribute(first, second));
        }

        [Fact(DisplayName = "GIVEN valid parameters WHEN constructor is invoked THEN parameters are propelry assigned")]
        public void GivenValidParameters_WhenConstructorIsInvoked_ThenParametersAreProperlyAssigned()
        {
            // Arrange
            const int item = 1;

            // Act
            var attribute = new FromValuesAttribute(item);

            // Assert
            attribute.Values.Should().HaveCount(1).And.Contain(item);
        }

        [AutoData]
        [Theory(DisplayName = "GIVEN values specified WHEN byte populated THEN the value from set is generated")]
        public void GivenValuesSpecified_WhenBytePopulated_ThenTheValueFromSetIsGenerated(
            [FromValues(1, 5, 20)] byte targetValue)
        {
            // Arrange
            // Act
            // Assert
            targetValue.Should().BeOneOf(1, 5, 20);
        }

        [AutoData]
        [Theory(DisplayName = "GIVEN values specified WHEN short populated THEN the value from set is generated")]
        public void GivenValuesSpecified_WhenUShortPopulated_ThenTheValueFromSetIsGenerated(
            [FromValues(1, 5, 4)] ushort targetValue)
        {
            // Arrange
            // Act
            // Assert
            targetValue.Should().BeOneOf(1, 5, 4);
        }

        [AutoData]
        [Theory(DisplayName = "GIVEN values specified WHEN unsigned long populated THEN the value from set is generated")]
        public void GivenValuesSpecified_WhenULongPopulated_ThenTheValueFromSetIsGenerated(
            [FromValues(1, long.MaxValue, ulong.MaxValue)] ulong targetValue)
        {
            // Arrange
            // Act
            // Assert
            targetValue.Should().BeOneOf(1, long.MaxValue, ulong.MaxValue);
        }

        [AutoData]
        [Theory(DisplayName = "GIVEN values specified WHEN signed byte populated THEN the value from set is generated")]
        public void GivenValuesSpecified_WhenSBytePopulated_ThenTheValueFromSetIsGenerated(
            [FromValues(sbyte.MinValue, -50, -1)] sbyte targetValue)
        {
            // Arrange
            // Act
            // Assert
            targetValue.Should().BeOneOf(sbyte.MinValue, -50, -1);
        }

        [AutoData]
        [Theory(DisplayName = "GIVEN values specified WHEN short populated THEN only decorated parameter has value from specification")]
        public void GivenValuesSpecified_WhenShortPopulated_ThenOnlyDecoratedParameterHasValueFromSpecification(
            [FromValues(short.MinValue, sbyte.MinValue, -1)] short targetValue,
            short unrestrictedValue)
        {
            // Arrange
            // Act
            // Assert
            targetValue.Should().BeOneOf(short.MinValue, sbyte.MinValue, -1);
            unrestrictedValue.Should().BeGreaterThanOrEqualTo(0);
        }

        [AutoData]
        [Theory(DisplayName = "GIVEN values specified WHEN integer populated THEN only decorated parameter has value from specification")]
        public void GivenValuesSpecified_WhenIntPopulated_ThenOnlyDecoratedParameterHasValueFromSpecification(
            [FromValues(int.MinValue, short.MinValue, sbyte.MinValue)] int targetValue,
            int unrestrictedValue)
        {
            // Arrange
            // Act
            // Assert
            targetValue.Should().BeOneOf(int.MinValue, short.MinValue, sbyte.MinValue);
            unrestrictedValue.Should().BeGreaterThanOrEqualTo(0);
        }

        [AutoData]
        [Theory(DisplayName = "GIVEN values specified WHEN long populated THEN only decorated parameter has value from specification")]
        public void GivenValuesSpecified_WhenLongPopulated_ThenOnlyDecoratedParameterHasValueFromSpecification(
            [FromValues(long.MinValue, int.MinValue, short.MinValue)] long targetValue,
            long unrestrictedValue)
        {
            // Arrange
            // Act
            // Assert
            targetValue.Should().BeOneOf(long.MinValue, int.MinValue, short.MinValue);
            unrestrictedValue.Should().BeGreaterThanOrEqualTo(0);
        }

        [AutoData]
        [Theory(DisplayName = "GIVEN values specified WHEN float populated THEN only decorated parameter has value from specification")]
        public void GivenValuesSpecified_WhenFloatPopulated_ThenOnlyDecoratedParameterHasValueFromSpecification(
            [FromValues(float.MinValue, int.MinValue, short.MinValue)] float targetValue,
            float unrestrictedValue)
        {
            // Arrange
            // Act
            // Assert
            targetValue.Should().BeOneOf(float.MinValue, int.MinValue, short.MinValue);
            unrestrictedValue.Should().BeGreaterThanOrEqualTo(0);
        }

        [AutoData]
        [Theory(DisplayName = "GIVEN values specified WHEN double populated THEN only decorated parameter has value from specification")]
        public void GivenValuesSpecified_WhenDoublePopulated_ThenOnlyDecoratedParameterHasValueFromSpecification(
            [FromValues(double.MinValue, float.MinValue, int.MinValue, short.MinValue)] double targetValue,
            double unrestrictedValue)
        {
            // Arrange
            // Act
            // Assert
            targetValue.Should().BeOneOf(double.MinValue, float.MinValue, int.MinValue, short.MinValue);
            unrestrictedValue.Should().BeGreaterThanOrEqualTo(0);
        }

        [AutoData]
        [Theory(DisplayName = "GIVEN values specified WHEN decimal populated THEN only decorated parameter has value from specification")]
        public void GivenValuesSpecified_WhenDecimalPopulated_ThenOnlyDecoratedParameterHasValueFromSpecification(
            [FromValues(long.MinValue, int.MinValue, short.MinValue)] decimal targetValue,
            decimal unrestrictedValue)
        {
            // Arrange
            // Act
            // Assert
            targetValue.Should().BeOneOf(long.MinValue, int.MinValue, short.MinValue);
            unrestrictedValue.Should().BeGreaterThanOrEqualTo(0);
        }

        [AutoData]
        [Theory(DisplayName = "GIVEN values specified for argument WHEN enum populated THEN the value from set is generated")]
        public void GivenValuesSpecifiedForAgrument_WhenEnumPopulated_ThenTheValueFromSetIsGenerated(
            [FromValues(Numbers.One, Numbers.Five, 100)] Numbers targetValue)
        {
            // Arrange
            // Act
            // Assert
            targetValue.Should().BeOneOf(Numbers.One, Numbers.Five, (Numbers)100);
        }

        [AutoData]
        [Theory(DisplayName = "GIVEN values specified for argument WHEN flag populated THEN the value from set is generated")]
        public void GivenValuesSpecifiedForAgrument_WhenFlagPopulated_ThenTheValueFromSetIsGenerated(
            [FromValues(Numbers.One | Numbers.Three, Numbers.Five)] Numbers targetValue)
        {
            // Arrange
            // Act
            // Assert
            targetValue.Should().BeOneOf(Numbers.One | Numbers.Three, Numbers.Five);
        }

        [InlineAutoData(10, 10)]
        [InlineAutoData(0, 0)]
        [Theory(DisplayName = "GIVEN values specified and inline value outside values WHEN data populated THEN values definition is ignored and inline one is used")]
        public void GivenValuesSpecifiedAndInlineValueOutsideValues_WhenDataPopulated_ThenValuesDefinitionIsIgnoredAndInlineOneIsUsed(
            [FromValues(int.MinValue)] int value, int expectedResult)
        {
            // Arrange
            // Act
            // Assert
            value.Should().Be(expectedResult).And.NotBe(int.MinValue);
        }

        [MemberAutoData(nameof(TestData))]
        [Theory(DisplayName = "GIVEN values specified and member data value outside values WHEN data populated THEN values definition is ignored and member data is used")]
        public void GivenValuesSpecifiedAndMemberDataValueOutsideValues_WhenDataPopulated_ThenValuesDefinitionIsIgnoredAndMemberDataIsUsed(
            [FromValues(int.MinValue)] int value,
            int expectedResult,
            [FromValues(int.MinValue)] int unrestrictedValue)
        {
            // Arrange
            // Act
            // Assert
            value.Should().Be(expectedResult).And.NotBe(int.MinValue);
            unrestrictedValue.Should().Be(int.MinValue);
        }

        [AutoData]
        [Theory(DisplayName = "GIVEN values specified for collection WHEN unsigned short populated THEN the value from set is generated")]
        public void GivenValuesSpecifiedForCollection_WhenEnumPopulated_ThenTheValueFromSetIsGenerated(
            [FromValues(int.MinValue, (int)short.MinValue, -1)] int[] targetValues,
            int[] unrestrictedValues)
        {
            // Arrange
            var supported = new[] { int.MinValue, -1, short.MinValue };

            // Act
            // Assert
            targetValues.Should().AllSatisfy(x => supported.Should().Contain(x));
            unrestrictedValues.Should().AllSatisfy(x => x.Should().BeGreaterThanOrEqualTo(0));
        }
    }
}
