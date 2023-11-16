namespace Objectivity.AutoFixture.XUnit2.Core.Tests.Attributes
{
    using System;
    using System.Collections.Generic;
    using System.Collections.ObjectModel;
    using System.Diagnostics.CodeAnalysis;

    using FluentAssertions;

    using global::AutoFixture.Xunit2;

    using Objectivity.AutoFixture.XUnit2.Core.Attributes;

    using Xunit;

    [Collection("FromRangeAttribute")]
    [Trait("Category", "Attributes")]
    public class FromRangeAttributeTests
    {
        public static IEnumerable<object[]> TestData { get; } = new[]
        {
            new object[] { 10, 10 },
            new object[] { 0, 0 },
        };

        [Fact(DisplayName = "GIVEN minimum greater than maximum WHEN constructor is invoked THEN exception is thrown")]
        public void GivenMinimumGreaterThanMaximum_WhenConstructorIsInvoked_ThenExceptionIsThrown()
        {
            // Arrange
            const int min = 100;
            const int max = 1;

            // Act
            // Assert
            Assert.Throws<ArgumentOutOfRangeException>(() => new FromRangeAttribute(min, max));
        }

        [Fact(DisplayName = "GIVEN valid parameters WHEN constructor is invoked THEN parameters are propelry assigned")]
        public void GivenValidParameters_WhenConstructorIsInvoked_ThenParametersAreProperlyAssigned()
        {
            // Arrange
            const int min = 1;
            const int max = 100;

            // Act
            var range = new FromRangeAttribute(min, max);

            // Assert
            range.Maximum.Should().NotBeNull().And.Be(max);
            range.Minimum.Should().NotBeNull().And.Be(min);
        }

        [AutoData]
        [Theory(DisplayName = "GIVEN renge specified WHEN byte populated THEN the value from range is generated")]
        public void GivenRengeSpecified_WhenBytePopulated_ThenTheValueFromRangeIsGenerated(
            [FromRange(byte.MaxValue - 10, byte.MaxValue)] byte rangeValue)
        {
            // Arrange
            // Act
            // Assert
            rangeValue.Should().BeInRange(byte.MaxValue - 10, byte.MaxValue);
        }

        [AutoData]
        [Theory(DisplayName = "GIVEN renge specified WHEN unsigned short populated THEN the value from range is generated")]
        public void GivenRengeSpecified_WhenUShortPopulated_ThenTheValueFromRangeIsGenerated(
            [FromRange(ushort.MaxValue - 10, ushort.MaxValue)] ushort rangeValue)
        {
            // Arrange
            // Act
            // Assert
            rangeValue.Should().BeInRange(ushort.MaxValue - 10, ushort.MaxValue);
        }

        [AutoData]
        [Theory(DisplayName = "GIVEN renge specified WHEN unsigned integer populated THEN the value from range is generated")]
        public void GivenRengeSpecified_WhenUIntPopulated_ThenTheValueFromRangeIsGenerated(
            [FromRange(uint.MaxValue - 10, uint.MaxValue)] uint rangeValue)
        {
            // Arrange
            // Act
            // Assert
            rangeValue.Should().BeInRange(uint.MaxValue - 10, uint.MaxValue);
        }

        [AutoData]
        [Theory(DisplayName = "GIVEN renge specified WHEN unsigned long populated THEN the value from range is generated")]
        public void GivenRengeSpecified_WhenULongPopulated_ThenTheValueFromRangeIsGenerated(
            [FromRange(ulong.MaxValue - 10, ulong.MaxValue)] ulong rangeValue)
        {
            // Arrange
            // Act
            // Assert
            rangeValue.Should().BeInRange(ulong.MaxValue - 10, ulong.MaxValue);
        }

        [AutoData]
        [Theory(DisplayName = "GIVEN renge specified WHEN signed byte populated THEN the value from range is generated")]
        public void GivenRengeSpecified_WhenSBytePopulated_ThenTheValueFromRangeIsGenerated(
            [FromRange(sbyte.MaxValue - 10, sbyte.MaxValue)] sbyte rangeValue)
        {
            // Arrange
            // Act
            // Assert
            rangeValue.Should().BeInRange(sbyte.MaxValue - 10, sbyte.MaxValue);
        }

        [AutoData]
        [Theory(DisplayName = "GIVEN renge specified WHEN short populated THEN only decorated parameter has value from range")]
        public void GivenRengeSpecified_WhenShortPopulated_ThenOnlyDecoratedParameterHasValueFromRange(
            [FromRange(short.MinValue, short.MinValue + 10)] short rangeValue,
            short unrestrictedValue)
        {
            // Arrange
            // Act
            // Assert
            rangeValue.Should().BeInRange(short.MinValue, short.MinValue + 10);
            unrestrictedValue.Should().BeGreaterThanOrEqualTo(0);
        }

        [AutoData]
        [Theory(DisplayName = "GIVEN renge specified WHEN integer populated THEN only decorated parameter has value from range")]
        public void GivenRengeSpecified_WhenIntPopulated_ThenOnlyDecoratedParameterHasValueFromRange(
            [FromRange(int.MinValue, sbyte.MinValue)] int rangeValue,
            int unrestrictedValue)
        {
            // Arrange
            // Act
            // Assert
            rangeValue.Should().BeInRange(int.MinValue, sbyte.MinValue);
            unrestrictedValue.Should().BeGreaterThanOrEqualTo(0);
        }

        [AutoData]
        [Theory(DisplayName = "GIVEN renge specified WHEN long populated THEN only decorated parameter has value from range")]
        public void GivenRengeSpecified_WhenLongPopulated_ThenOnlyDecoratedParameterHasValueFromRange(
            [FromRange(long.MinValue, long.MinValue + 10)] long rangeValue,
            long unrestrictedValue)
        {
            // Arrange
            // Act
            // Assert
            rangeValue.Should().BeInRange(long.MinValue, long.MinValue + 10);
            unrestrictedValue.Should().BeGreaterThanOrEqualTo(0);
        }

        [AutoData]
        [Theory(DisplayName = "GIVEN renge specified WHEN float populated THEN only decorated parameter has value from range")]
        public void GivenRengeSpecified_WhenFloatPopulated_ThenOnlyDecoratedParameterHasValueFromRange(
            [FromRange(float.MinValue, float.MinValue + 10)] float rangeValue,
            float unrestrictedValue)
        {
            // Arrange
            // Act
            // Assert
            rangeValue.Should().BeInRange(float.MinValue, float.MinValue + 10);
            unrestrictedValue.Should().BeGreaterThanOrEqualTo(0);
        }

        [AutoData]
        [Theory(DisplayName = "GIVEN renge specified WHEN double populated THEN only decorated parameter has value from range")]
        public void GivenRengeSpecified_WhenDoublePopulated_ThenOnlyDecoratedParameterHasValueFromRange(
            [FromRange(double.MinValue, double.MinValue + 10)] double rangeValue,
            double unrestrictedValue)
        {
            // Arrange
            // Act
            // Assert
            rangeValue.Should().BeInRange(double.MinValue, double.MinValue + 10);
            unrestrictedValue.Should().BeGreaterThanOrEqualTo(0);
        }

        [AutoData]
        [Theory(DisplayName = "GIVEN renge specified WHEN decimal populated THEN only decorated parameter has value from range")]
        public void GivenRengeSpecified_WhenDecimalPopulated_ThenOnlyDecoratedParameterHasValueFromRange(
            [FromRange(-12.3, -4.5)] decimal rangeValue,
            decimal unrestrictedValue)
        {
            // Arrange
            // Act
            // Assert
            rangeValue.Should().BeInRange(-12.3m, -4.5m);
            unrestrictedValue.Should().BeGreaterThanOrEqualTo(0);
        }

        [InlineAutoData(10, 10)]
        [InlineAutoData(0, 0)]
        [Theory(DisplayName = "GIVEN renge specified and inline value outside range WHEN data populated THEN values from range are ignored and inline one is used")]
        public void GivenRengeSpecifiedAndInlineValueOutsideRange_WhenDataPopulated_ThenValuesFromRangeAreIgnoredAndInlineOneIsUsed(
            [FromRange(int.MinValue, short.MinValue)] int value,
            int expectedResult,
            [FromRange(short.MinValue, sbyte.MinValue)] int unrestrictedValue)
        {
            // Arrange
            // Act
            // Assert
            value.Should().Be(expectedResult).And.NotBeInRange(int.MinValue, sbyte.MinValue);
            unrestrictedValue.Should().BeInRange(short.MinValue, sbyte.MinValue);
        }

        [MemberAutoData(nameof(TestData))]
        [Theory(DisplayName = "GIVEN renge specified and member data value outside range WHEN data populated THEN values from range are ignored and member data is used")]
        public void GivenRengeSpecifiedAndMemberDataValueOutsideRange_WhenDataPopulated_ThenValuesFromRangeAreIgnoredAndMemberDataIsUsed(
            [FromRange(int.MinValue, sbyte.MinValue)] int value,
            int expectedResult,
            [FromRange(int.MinValue, sbyte.MinValue)] int unrestrictedValue)
        {
            // Arrange
            // Act
            // Assert
            value.Should().Be(expectedResult).And.NotBeInRange(int.MinValue, sbyte.MinValue);
            unrestrictedValue.Should().BeInRange(int.MinValue, sbyte.MinValue);
        }

        [AutoData]
        [Theory(DisplayName = "GIVEN renge specified WHEN arrays populated THEN only decorated parameter has value from range")]
        public void GivenRengeSpecified_WhenArraysPopulated_ThenOnlyDecoratedParameterHasValuesFromRange(
            [FromRange(int.MinValue, sbyte.MinValue)] int[] rangeValues,
            int[] unrestrictedValues)
        {
            // Arrange
            // Act
            // Assert
            rangeValues.Should().AllSatisfy(x => x.Should().BeInRange(int.MinValue, sbyte.MinValue));
            unrestrictedValues.Should().AllSatisfy(x => x.Should().BeGreaterThanOrEqualTo(0));
        }

        [AutoData]
        [Theory(DisplayName = "GIVEN renge specified WHEN enumerables populated THEN only decorated parameter has value from range")]
        public void GivenRengeSpecified_WhenEnumerablesPopulated_ThenOnlyDecoratedParameterHasValuesFromRange(
            [FromRange(int.MinValue, sbyte.MinValue)] IEnumerable<int> rangeValues,
            IEnumerable<int> unrestrictedValues)
        {
            // Arrange
            // Act
            // Assert
            rangeValues.Should().AllSatisfy(x => x.Should().BeInRange(int.MinValue, sbyte.MinValue));
            unrestrictedValues.Should().AllSatisfy(x => x.Should().BeGreaterThanOrEqualTo(0));
        }

        [AutoData]
        [Theory(DisplayName = "GIVEN renge specified WHEN lists populated THEN only decorated parameter has value from range")]
        [SuppressMessage("Design", "CA1002:Do not expose generic lists", Justification = "We are testing generic lists")]
        public void GivenRengeSpecified_WhenListPopulated_ThenOnlyDecoratedParameterHasValuesFromRange(
            [FromRange(int.MinValue, sbyte.MinValue)] List<int> rangeValues,
            List<int> unrestrictedValues)
        {
            // Arrange
            // Act
            // Assert
            rangeValues.Should().AllSatisfy(x => x.Should().BeInRange(int.MinValue, sbyte.MinValue));
            unrestrictedValues.Should().AllSatisfy(x => x.Should().BeGreaterThanOrEqualTo(0));
        }

        [AutoData]
        [Theory(DisplayName = "GIVEN renge specified WHEN sets populated THEN only decorated parameter has value from range")]
        [SuppressMessage("Design", "CA1002:Do not expose generic lists", Justification = "We are testing generic lists")]
        public void GivenRengeSpecified_WhenSetsPopulated_ThenOnlyDecoratedParameterHasValuesFromRange(
            [FromRange(int.MinValue, sbyte.MinValue)] HashSet<int> rangeValues,
            HashSet<int> unrestrictedValues)
        {
            // Arrange
            // Act
            // Assert
            rangeValues.Should().AllSatisfy(x => x.Should().BeInRange(int.MinValue, sbyte.MinValue));
            unrestrictedValues.Should().AllSatisfy(x => x.Should().BeGreaterThanOrEqualTo(0));
        }

        [AutoData]
        [Theory(DisplayName = "GIVEN renge specified WHEN collections populated THEN only decorated parameter has value from range")]
        [SuppressMessage("Design", "CA1002:Do not expose generic lists", Justification = "We are testing generic lists")]
        public void GivenRengeSpecified_WhenCollectionsPopulated_ThenOnlyDecoratedParameterHasValuesFromRange(
            [FromRange(int.MinValue, sbyte.MinValue)] Collection<int> rangeValues,
            Collection<int> unrestrictedValues)
        {
            // Arrange
            // Act
            // Assert
            rangeValues.Should().AllSatisfy(x => x.Should().BeInRange(int.MinValue, sbyte.MinValue));
            unrestrictedValues.Should().AllSatisfy(x => x.Should().BeGreaterThanOrEqualTo(0));
        }

        [AutoData]
        [Theory(DisplayName = "GIVEN renge specified WHEN read-only collections populated THEN only decorated parameter has value from range")]
        [SuppressMessage("Design", "CA1002:Do not expose generic lists", Justification = "We are testing generic lists")]
        public void GivenRengeSpecified_WhenReadOnlyCollectionsPopulated_ThenOnlyDecoratedParameterHasValuesFromRange(
            [FromRange(int.MinValue, sbyte.MinValue)] ReadOnlyCollection<int> rangeValues,
            ReadOnlyCollection<int> unrestrictedValues)
        {
            // Arrange
            // Act
            // Assert
            rangeValues.Should().AllSatisfy(x => x.Should().BeInRange(int.MinValue, sbyte.MinValue));
            unrestrictedValues.Should().AllSatisfy(x => x.Should().BeGreaterThanOrEqualTo(0));
        }

        [AutoData]
        [Theory(DisplayName = "GIVEN renge with single value WHEN array populated THEN all values equal specified one")]
        public void GivenRengeWithSingleValue_WhenArrayPopulated_ThenAllValuesEqualSpecifiedOne(
            [FromRange(int.MinValue, int.MinValue)] int[] rangeValues)
        {
            // Arrange
            // Act
            // Assert
            rangeValues.Should().HaveCountGreaterThan(1).And.AllSatisfy(x => x.Should().Be(int.MinValue));
        }
    }
}
