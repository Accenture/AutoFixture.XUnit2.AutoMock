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
            [FromRange(Ranges.ByteRange.Min, Ranges.ByteRange.Max)] byte rangeValue)
        {
            rangeValue.Should().BeInRange(Ranges.ByteRange.Min, Ranges.ByteRange.Max);
        }

        [AutoData]
        [Theory(DisplayName = "GIVEN renge specified WHEN unsigned short populated THEN the value from range is generated")]
        public void GivenRengeSpecified_WhenUShortPopulated_ThenTheValueFromRangeIsGenerated(
            [FromRange(Ranges.UShortRange.Min, Ranges.UShortRange.Max)] ushort rangeValue)
        {
            rangeValue.Should().BeInRange(Ranges.UShortRange.Min, Ranges.UShortRange.Max);
        }

        [AutoData]
        [Theory(DisplayName = "GIVEN renge specified WHEN unsigned integer populated THEN the value from range is generated")]
        public void GivenRengeSpecified_WhenUIntPopulated_ThenTheValueFromRangeIsGenerated(
            [FromRange(Ranges.UIntRange.Min, Ranges.UIntRange.Max)] uint rangeValue)
        {
            rangeValue.Should().BeInRange(Ranges.UIntRange.Min, Ranges.UIntRange.Max);
        }

        [AutoData]
        [Theory(DisplayName = "GIVEN renge specified WHEN unsigned long populated THEN the value from range is generated")]
        public void GivenRengeSpecified_WhenULongPopulated_ThenTheValueFromRangeIsGenerated(
            [FromRange(Ranges.ULongRange.Min, Ranges.ULongRange.Max)] ulong rangeValue)
        {
            rangeValue.Should().BeInRange(Ranges.ULongRange.Min, Ranges.ULongRange.Max);
        }

        [AutoData]
        [Theory(DisplayName = "GIVEN renge specified WHEN signed byte populated THEN the value from range is generated")]
        public void GivenRengeSpecified_WhenSBytePopulated_ThenTheValueFromRangeIsGenerated(
            [FromRange(Ranges.SByteRange.Min, Ranges.SByteRange.Max)] sbyte rangeValue)
        {
            rangeValue.Should().BeInRange(Ranges.SByteRange.Min, Ranges.SByteRange.Max);
        }

        [AutoData]
        [Theory(DisplayName = "GIVEN renge specified WHEN short populated THEN only decorated parameter has value from range")]
        public void GivenRengeSpecified_WhenShortPopulated_ThenOnlyDecoratedParameterHasValueFromRange(
            [FromRange(Ranges.ShortRange.Min, Ranges.ShortRange.Max)] short rangeValue,
            short unrestrictedValue)
        {
            rangeValue.Should().BeInRange(Ranges.ShortRange.Min, Ranges.ShortRange.Max);
            unrestrictedValue.Should().BeGreaterThanOrEqualTo(0);
        }

        [AutoData]
        [Theory(DisplayName = "GIVEN renge specified WHEN integer populated THEN only decorated parameter has value from range")]
        public void GivenRengeSpecified_WhenIntPopulated_ThenOnlyDecoratedParameterHasValueFromRange(
            [FromRange(Ranges.IntRange.Min, Ranges.IntRange.Max)] int rangeValue,
            int unrestrictedValue)
        {
            rangeValue.Should().BeInRange(Ranges.IntRange.Min, Ranges.IntRange.Max);
            unrestrictedValue.Should().BeGreaterThanOrEqualTo(0);
        }

        [AutoData]
        [Theory(DisplayName = "GIVEN renge specified WHEN long populated THEN only decorated parameter has value from range")]
        public void GivenRengeSpecified_WhenLongPopulated_ThenOnlyDecoratedParameterHasValueFromRange(
            [FromRange(Ranges.LongRange.Min, Ranges.LongRange.Max)] long rangeValue,
            long unrestrictedValue)
        {
            rangeValue.Should().BeInRange(Ranges.LongRange.Min, Ranges.LongRange.Max);
            unrestrictedValue.Should().BeGreaterThanOrEqualTo(0);
        }

        [AutoData]
        [Theory(DisplayName = "GIVEN renge specified WHEN float populated THEN only decorated parameter has value from range")]
        public void GivenRengeSpecified_WhenFloatPopulated_ThenOnlyDecoratedParameterHasValueFromRange(
            [FromRange(Ranges.FloatRange.Min, Ranges.FloatRange.Max)] float rangeValue,
            float unrestrictedValue)
        {
            rangeValue.Should().BeInRange(Ranges.FloatRange.Min, Ranges.FloatRange.Max);
            unrestrictedValue.Should().BeGreaterThanOrEqualTo(0);
        }

        [AutoData]
        [Theory(DisplayName = "GIVEN renge specified WHEN double populated THEN only decorated parameter has value from range")]
        public void GivenRengeSpecified_WhenDoublePopulated_ThenOnlyDecoratedParameterHasValueFromRange(
            [FromRange(Ranges.DoubleRange.Min, Ranges.DoubleRange.Max)] double rangeValue,
            double unrestrictedValue)
        {
            rangeValue.Should().BeInRange(Ranges.DoubleRange.Min, Ranges.DoubleRange.Max);
            unrestrictedValue.Should().BeGreaterThanOrEqualTo(0);
        }

        [AutoData]
        [Theory(DisplayName = "GIVEN renge specified WHEN decimal populated THEN only decorated parameter has value from range")]
        public void GivenRengeSpecified_WhenDecimalPopulated_ThenOnlyDecoratedParameterHasValueFromRange(
            [FromRange((double)Ranges.DecimalRange.Min, (double)Ranges.DecimalRange.Max)] decimal rangeValue,
            decimal unrestrictedValue)
        {
            rangeValue.Should().BeInRange(Ranges.DecimalRange.Min, Ranges.DecimalRange.Max);
            unrestrictedValue.Should().BeGreaterThanOrEqualTo(0);
        }

        [InlineAutoData(10, 10)]
        [InlineAutoData(0, 0)]
        [Theory(DisplayName = "GIVEN renge specified and inline value outside range WHEN data populated THEN values from range are ignored and inline one is used")]
        public void GivenRengeSpecifiedAndInlineValueOutsideRange_WhenDataPopulated_ThenValuesFromRangeAreIgnoredAndInlineOneIsUsed(
            [FromRange(Ranges.IntRange.Min, Ranges.IntRange.Max)] int value, int expectedResult)
        {
            value.Should().Be(expectedResult).And.NotBeInRange(Ranges.IntRange.Min, Ranges.IntRange.Max);
        }

        [MemberAutoData(nameof(TestData))]
        [Theory(DisplayName = "GIVEN renge specified and member data value outside range WHEN data populated THEN values from range are ignored and member data is used")]
        public void GivenRengeSpecifiedAndMemberDataValueOutsideRange_WhenDataPopulated_ThenValuesFromRangeAreIgnoredAndMemberDataIsUsed(
            [FromRange(Ranges.IntRange.Min, Ranges.IntRange.Max)] int value, int expectedResult)
        {
            value.Should().Be(expectedResult).And.NotBeInRange(Ranges.IntRange.Min, Ranges.IntRange.Max);
        }

        [AutoData]
        [Theory(DisplayName = "GIVEN renge specified WHEN arrays populated THEN only decorated parameter has value from range")]
        public void GivenRengeSpecified_WhenArraysPopulated_ThenOnlyDecoratedParameterHasValuesFromRange(
            [FromRange(Ranges.IntRange.Min, Ranges.IntRange.Max)] int[] rangeValues,
            int[] unrestrictedValues)
        {
            rangeValues.Should().AllSatisfy(x => x.Should().BeInRange(Ranges.IntRange.Min, Ranges.IntRange.Max));
            unrestrictedValues.Should().AllSatisfy(x => x.Should().BeGreaterThanOrEqualTo(0));
        }

        [AutoData]
        [Theory(DisplayName = "GIVEN renge specified WHEN enumerables populated THEN only decorated parameter has value from range")]
        public void GivenRengeSpecified_WhenEnumerablesPopulated_ThenOnlyDecoratedParameterHasValuesFromRange(
            [FromRange(Ranges.IntRange.Min, Ranges.IntRange.Max)] IEnumerable<int> rangeValues,
            IEnumerable<int> unrestrictedValues)
        {
            rangeValues.Should().AllSatisfy(x => x.Should().BeInRange(Ranges.IntRange.Min, Ranges.IntRange.Max));
            unrestrictedValues.Should().AllSatisfy(x => x.Should().BeGreaterThanOrEqualTo(0));
        }

        [AutoData]
        [Theory(DisplayName = "GIVEN renge specified WHEN lists populated THEN only decorated parameter has value from range")]
        [SuppressMessage("Design", "CA1002:Do not expose generic lists", Justification = "We are testing generic lists")]
        public void GivenRengeSpecified_WhenListPopulated_ThenOnlyDecoratedParameterHasValuesFromRange(
            [FromRange(Ranges.IntRange.Min, Ranges.IntRange.Max)] List<int> rangeValues,
            List<int> unrestrictedValues)
        {
            rangeValues.Should().AllSatisfy(x => x.Should().BeInRange(Ranges.IntRange.Min, Ranges.IntRange.Max));
            unrestrictedValues.Should().AllSatisfy(x => x.Should().BeGreaterThanOrEqualTo(0));
        }

        [AutoData]
        [Theory(DisplayName = "GIVEN renge specified WHEN sets populated THEN only decorated parameter has value from range")]
        [SuppressMessage("Design", "CA1002:Do not expose generic lists", Justification = "We are testing generic lists")]
        public void GivenRengeSpecified_WhenSetsPopulated_ThenOnlyDecoratedParameterHasValuesFromRange(
            [FromRange(Ranges.IntRange.Min, Ranges.IntRange.Max)] HashSet<int> rangeValues,
            HashSet<int> unrestrictedValues)
        {
            rangeValues.Should().AllSatisfy(x => x.Should().BeInRange(Ranges.IntRange.Min, Ranges.IntRange.Max));
            unrestrictedValues.Should().AllSatisfy(x => x.Should().BeGreaterThanOrEqualTo(0));
        }

        [AutoData]
        [Theory(DisplayName = "GIVEN renge specified WHEN collections populated THEN only decorated parameter has value from range")]
        [SuppressMessage("Design", "CA1002:Do not expose generic lists", Justification = "We are testing generic lists")]
        public void GivenRengeSpecified_WhenCollectionsPopulated_ThenOnlyDecoratedParameterHasValuesFromRange(
            [FromRange(Ranges.IntRange.Min, Ranges.IntRange.Max)] Collection<int> rangeValues,
            Collection<int> unrestrictedValues)
        {
            rangeValues.Should().AllSatisfy(x => x.Should().BeInRange(Ranges.IntRange.Min, Ranges.IntRange.Max));
            unrestrictedValues.Should().AllSatisfy(x => x.Should().BeGreaterThanOrEqualTo(0));
        }

        [AutoData]
        [Theory(DisplayName = "GIVEN renge specified WHEN read-only collections populated THEN only decorated parameter has value from range")]
        [SuppressMessage("Design", "CA1002:Do not expose generic lists", Justification = "We are testing generic lists")]
        public void GivenRengeSpecified_WhenReadOnlyCollectionsPopulated_ThenOnlyDecoratedParameterHasValuesFromRange(
            [FromRange(Ranges.IntRange.Min, Ranges.IntRange.Max)] ReadOnlyCollection<int> rangeValues,
            ReadOnlyCollection<int> unrestrictedValues)
        {
            rangeValues.Should().AllSatisfy(x => x.Should().BeInRange(Ranges.IntRange.Min, Ranges.IntRange.Max));
            unrestrictedValues.Should().AllSatisfy(x => x.Should().BeGreaterThanOrEqualTo(0));
        }

        [AutoData]
        [Theory(DisplayName = "GIVEN renge with single value WHEN array populated THEN all values equal specified one")]
        public void GivenRengeWithSingleValue_WhenArrayPopulated_ThenAllValuesEqualSpecifiedOne(
            [FromRange(Ranges.IntRange.Min, Ranges.IntRange.Min)] int[] rangeValues)
        {
            rangeValues.Should().HaveCountGreaterThan(1).And.AllSatisfy(x => x.Should().Be(Ranges.IntRange.Min));
        }

        private static class Ranges
        {
            public static class SByteRange
            {
                public const sbyte Min = -10;
                public const sbyte Max = -1;
            }

            public static class ByteRange
            {
                public const byte Min = byte.MaxValue - 10;
                public const byte Max = byte.MaxValue;
            }

            public static class ShortRange
            {
                public const short Min = -10;
                public const short Max = -1;
            }

            public static class UShortRange
            {
                public const ushort Min = ushort.MaxValue - 10;
                public const ushort Max = ushort.MaxValue;
            }

            public static class IntRange
            {
                public const int Min = -10;
                public const int Max = -1;
            }

            public static class UIntRange
            {
                public const uint Min = uint.MaxValue - 10;
                public const uint Max = uint.MaxValue;
            }

            public static class LongRange
            {
                public const long Min = -20;
                public const long Max = -11;
            }

            public static class ULongRange
            {
                public const ulong Min = ulong.MaxValue - 10;
                public const ulong Max = ulong.MaxValue;
            }

            public static class FloatRange
            {
                public const float Min = -29.9f;
                public const float Max = -20.1f;
            }

            public static class DoubleRange
            {
                public const double Min = -39.9;
                public const double Max = -30.1;
            }

            public static class DecimalRange
            {
                public const decimal Min = -2.9m;
                public const decimal Max = -2.1m;
            }
        }
    }
}
