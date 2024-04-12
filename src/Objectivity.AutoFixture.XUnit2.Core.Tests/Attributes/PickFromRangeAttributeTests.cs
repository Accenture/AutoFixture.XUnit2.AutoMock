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

    [Collection("PickFromRangeAttribute")]
    [Trait("Category", "CustomizeAttribute")]
    public class PickFromRangeAttributeTests
    {
        public static TheoryData<int, int> MemberAutoDataOverValuesTestData { get; } = new()
        {
            { 10, 10 },
        };

        [Fact(DisplayName = "GIVEN minimum greater than maximum WHEN constructor is invoked THEN exception is thrown")]
        public void GivenMinimumGreaterThanMaximum_WhenConstructorIsInvoked_ThenExceptionIsThrown()
        {
            // Arrange
            const int min = 100;
            const int max = 1;

            // Act
            Func<object> act = () => new PickFromRangeAttribute(min, max);

            // Assert
            act.Should().Throw<ArgumentOutOfRangeException>()
                .And.Message.Should().NotBeNullOrEmpty()
                .And.Contain("must be lower or equal");
        }

        [Fact(DisplayName = "GIVEN uninitialized argument WHEN GetCustomization is invoked THEN exception is thrown")]
        public void GivenUninitializedArgument_WhenGetCustomizationIsInvoked_ThenExceptionIsThrown()
        {
            // Arrange
            const int min = 1;
            const int max = 100;
            var attribute = new PickFromRangeAttribute(min, max);

            // Act
            Action act = () => attribute.GetCustomization(null);

            // Assert
            act.Should().Throw<ArgumentNullException>()
                .And.ParamName.Should().Be("parameter");
        }

        [Fact(DisplayName = "GIVEN valid parameters WHEN constructor is invoked THEN parameters are properly assigned")]
        public void GivenValidParameters_WhenConstructorIsInvoked_ThenParametersAreProperlyAssigned()
        {
            // Arrange
            const int min = 1;
            const int max = 100;

            // Act
            var range = new PickFromRangeAttribute(min, max);

            // Assert
            range.Maximum.Should().NotBeNull().And.Be(max);
            range.Minimum.Should().NotBeNull().And.Be(min);
        }

        [AutoData]
        [Theory(DisplayName = "GIVEN range specified WHEN byte populated THEN the value from range is generated")]
        public void GivenRangeSpecified_WhenBytePopulated_ThenTheValueFromRangeIsGenerated(
            [PickFromRange(byte.MaxValue - 10, byte.MaxValue)] byte rangeValue)
        {
            // Arrange
            // Act
            // Assert
            rangeValue.Should().BeInRange(byte.MaxValue - 10, byte.MaxValue);
        }

        [AutoData]
        [Theory(DisplayName = "GIVEN range specified WHEN unsigned short populated THEN the value from range is generated")]
        public void GivenRangeSpecified_WhenUShortPopulated_ThenTheValueFromRangeIsGenerated(
            [PickFromRange(ushort.MaxValue - 10, ushort.MaxValue)] ushort rangeValue)
        {
            // Arrange
            // Act
            // Assert
            rangeValue.Should().BeInRange(ushort.MaxValue - 10, ushort.MaxValue);
        }

        [AutoData]
        [Theory(DisplayName = "GIVEN range specified WHEN unsigned integer populated THEN the value from range is generated")]
        public void GivenRangeSpecified_WhenUIntPopulated_ThenTheValueFromRangeIsGenerated(
            [PickFromRange(uint.MaxValue - 10, uint.MaxValue)] uint rangeValue)
        {
            // Arrange
            // Act
            // Assert
            rangeValue.Should().BeInRange(uint.MaxValue - 10, uint.MaxValue);
        }

        [AutoData]
        [Theory(DisplayName = "GIVEN range specified WHEN unsigned long populated THEN the value from range is generated")]
        public void GivenRangeSpecified_WhenULongPopulated_ThenTheValueFromRangeIsGenerated(
            [PickFromRange(ulong.MaxValue - 10, ulong.MaxValue)] ulong rangeValue)
        {
            // Arrange
            // Act
            // Assert
            rangeValue.Should().BeInRange(ulong.MaxValue - 10, ulong.MaxValue);
        }

        [AutoData]
        [Theory(DisplayName = "GIVEN range specified WHEN signed byte populated THEN the value from range is generated")]
        public void GivenRangeSpecified_WhenSBytePopulated_ThenTheValueFromRangeIsGenerated(
            [PickFromRange(sbyte.MaxValue - 10, sbyte.MaxValue)] sbyte rangeValue)
        {
            // Arrange
            // Act
            // Assert
            rangeValue.Should().BeInRange(sbyte.MaxValue - 10, sbyte.MaxValue);
        }

        [AutoData]
        [Theory(DisplayName = "GIVEN range specified WHEN short populated THEN only decorated parameter has value from range")]
        public void GivenRangeSpecified_WhenShortPopulated_ThenOnlyDecoratedParameterHasValueFromRange(
            [PickFromRange(short.MinValue, short.MinValue + 10)] short rangeValue,
            short unrestrictedValue)
        {
            // Arrange
            // Act
            // Assert
            rangeValue.Should().BeInRange(short.MinValue, short.MinValue + 10);
            unrestrictedValue.Should().BeGreaterThanOrEqualTo(0);
        }

        [AutoData]
        [Theory(DisplayName = "GIVEN range specified WHEN integer populated THEN only decorated parameter has value from range")]
        public void GivenRangeSpecified_WhenIntPopulated_ThenOnlyDecoratedParameterHasValueFromRange(
            [PickFromRange(int.MinValue, sbyte.MinValue)] int rangeValue,
            int unrestrictedValue)
        {
            // Arrange
            // Act
            // Assert
            rangeValue.Should().BeInRange(int.MinValue, sbyte.MinValue);
            unrestrictedValue.Should().BeGreaterThanOrEqualTo(0);
        }

        [AutoData]
        [Theory(DisplayName = "GIVEN range specified WHEN long populated THEN only decorated parameter has value from range")]
        public void GivenRangeSpecified_WhenLongPopulated_ThenOnlyDecoratedParameterHasValueFromRange(
            [PickFromRange(long.MinValue, long.MinValue + 10)] long rangeValue,
            long unrestrictedValue)
        {
            // Arrange
            // Act
            // Assert
            rangeValue.Should().BeInRange(long.MinValue, long.MinValue + 10);
            unrestrictedValue.Should().BeGreaterThanOrEqualTo(0);
        }

        [AutoData]
        [Theory(DisplayName = "GIVEN range specified WHEN float populated THEN only decorated parameter has value from range")]
        public void GivenRangeSpecified_WhenFloatPopulated_ThenOnlyDecoratedParameterHasValueFromRange(
            [PickFromRange(float.MinValue, float.MinValue + 10)] float rangeValue,
            float unrestrictedValue)
        {
            // Arrange
            // Act
            // Assert
            rangeValue.Should().BeInRange(float.MinValue, float.MinValue + 10);
            unrestrictedValue.Should().BeGreaterThanOrEqualTo(0);
        }

        [AutoData]
        [Theory(DisplayName = "GIVEN range specified WHEN double populated THEN only decorated parameter has value from range")]
        public void GivenRangeSpecified_WhenDoublePopulated_ThenOnlyDecoratedParameterHasValueFromRange(
            [PickFromRange(double.MinValue, double.MinValue + 10)] double rangeValue,
            double unrestrictedValue)
        {
            // Arrange
            // Act
            // Assert
            rangeValue.Should().BeInRange(double.MinValue, double.MinValue + 10);
            unrestrictedValue.Should().BeGreaterThanOrEqualTo(0);
        }

        [AutoData]
        [Theory(DisplayName = "GIVEN range specified WHEN decimal populated THEN only decorated parameter has value from range")]
        public void GivenRangeSpecified_WhenDecimalPopulated_ThenOnlyDecoratedParameterHasValueFromRange(
            [PickFromRange(-12.3, -4.5)] decimal rangeValue,
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
        [Theory(DisplayName = "GIVEN range specified and inline value outside range WHEN data populated THEN values from range are ignored and inline one is used")]
        public void GivenRangeSpecifiedAndInlineValueOutsideRange_WhenDataPopulated_ThenValuesFromRangeAreIgnoredAndInlineOneIsUsed(
            [PickFromRange(int.MinValue, short.MinValue)] int value,
            int expectedResult,
            [PickFromRange(short.MinValue, sbyte.MinValue)] int unrestrictedValue)
        {
            // Arrange
            // Act
            // Assert
            value.Should().Be(expectedResult).And.NotBeInRange(int.MinValue, sbyte.MinValue);
            unrestrictedValue.Should().BeInRange(short.MinValue, sbyte.MinValue);
        }

        [MemberAutoData(nameof(MemberAutoDataOverValuesTestData))]
        [Theory(DisplayName = "GIVEN range specified and member data value outside range WHEN data populated THEN values from range are ignored and member data is used")]
        public void GivenRangeSpecifiedAndMemberDataValueOutsideRange_WhenDataPopulated_ThenValuesFromRangeAreIgnoredAndMemberDataIsUsed(
            [PickFromRange(int.MinValue, sbyte.MinValue)] int value,
            int expectedResult,
            [PickFromRange(int.MinValue, sbyte.MinValue)] int unrestrictedValue)
        {
            // Arrange
            // Act
            // Assert
            value.Should().Be(expectedResult).And.NotBeInRange(int.MinValue, sbyte.MinValue);
            unrestrictedValue.Should().BeInRange(int.MinValue, sbyte.MinValue);
        }

        [AutoData]
        [Theory(DisplayName = "GIVEN range specified WHEN arrays populated THEN only decorated parameter has value from range")]
        public void GivenRangeSpecified_WhenArraysPopulated_ThenOnlyDecoratedParameterHasValuesFromRange(
            [PickFromRange(int.MinValue, sbyte.MinValue)] int[] rangeValues,
            int[] unrestrictedValues)
        {
            // Arrange
            // Act
            // Assert
            rangeValues.Should().AllSatisfy(x => x.Should().BeInRange(int.MinValue, sbyte.MinValue));
            unrestrictedValues.Should().AllSatisfy(x => x.Should().BeGreaterThanOrEqualTo(0));
        }

        [AutoData]
        [Theory(DisplayName = "GIVEN range specified WHEN enumerable collections populated THEN only decorated parameter has value from range")]
        public void GivenRangeSpecified_WhenEnumerableCollectionsPopulated_ThenOnlyDecoratedParameterHasValuesFromRange(
            [PickFromRange(int.MinValue, sbyte.MinValue)] IEnumerable<int> rangeValues,
            IEnumerable<int> unrestrictedValues)
        {
            // Arrange
            // Act
            // Assert
            rangeValues.Should().AllSatisfy(x => x.Should().BeInRange(int.MinValue, sbyte.MinValue));
            unrestrictedValues.Should().AllSatisfy(x => x.Should().BeGreaterThanOrEqualTo(0));
        }

        [AutoData]
        [Theory(DisplayName = "GIVEN range specified WHEN lists populated THEN only decorated parameter has value from range")]
        [SuppressMessage("Design", "CA1002:Do not expose generic lists", Justification = "We are testing generic lists")]
        public void GivenRangeSpecified_WhenListPopulated_ThenOnlyDecoratedParameterHasValuesFromRange(
            [PickFromRange(int.MinValue, sbyte.MinValue)] List<int> rangeValues,
            List<int> unrestrictedValues)
        {
            // Arrange
            // Act
            // Assert
            rangeValues.Should().AllSatisfy(x => x.Should().BeInRange(int.MinValue, sbyte.MinValue));
            unrestrictedValues.Should().AllSatisfy(x => x.Should().BeGreaterThanOrEqualTo(0));
        }

        [AutoData]
        [Theory(DisplayName = "GIVEN range specified WHEN sets populated THEN only decorated parameter has value from range")]
        public void GivenRangeSpecified_WhenSetsPopulated_ThenOnlyDecoratedParameterHasValuesFromRange(
            [PickFromRange(int.MinValue, sbyte.MinValue)] HashSet<int> rangeValues,
            HashSet<int> unrestrictedValues)
        {
            // Arrange
            // Act
            // Assert
            rangeValues.Should().AllSatisfy(x => x.Should().BeInRange(int.MinValue, sbyte.MinValue));
            unrestrictedValues.Should().AllSatisfy(x => x.Should().BeGreaterThanOrEqualTo(0));
        }

        [AutoData]
        [Theory(DisplayName = "GIVEN range specified WHEN collections populated THEN only decorated parameter has value from range")]
        [SuppressMessage("Design", "CA1002:Do not expose generic lists", Justification = "We are testing generic lists")]
        public void GivenRangeSpecified_WhenCollectionsPopulated_ThenOnlyDecoratedParameterHasValuesFromRange(
            [PickFromRange(int.MinValue, sbyte.MinValue)] Collection<int> rangeValues,
            Collection<int> unrestrictedValues)
        {
            // Arrange
            // Act
            // Assert
            rangeValues.Should().AllSatisfy(x => x.Should().BeInRange(int.MinValue, sbyte.MinValue));
            unrestrictedValues.Should().AllSatisfy(x => x.Should().BeGreaterThanOrEqualTo(0));
        }

        [AutoData]
        [Theory(DisplayName = "GIVEN range specified WHEN read-only collections populated THEN only decorated parameter has value from range")]
        [SuppressMessage("Design", "CA1002:Do not expose generic lists", Justification = "We are testing generic lists")]
        public void GivenRangeSpecified_WhenReadOnlyCollectionsPopulated_ThenOnlyDecoratedParameterHasValuesFromRange(
            [PickFromRange(int.MinValue, sbyte.MinValue)] ReadOnlyCollection<int> rangeValues,
            ReadOnlyCollection<int> unrestrictedValues)
        {
            // Arrange
            // Act
            // Assert
            rangeValues.Should().AllSatisfy(x => x.Should().BeInRange(int.MinValue, sbyte.MinValue));
            unrestrictedValues.Should().AllSatisfy(x => x.Should().BeGreaterThanOrEqualTo(0));
        }

        [AutoData]
        [Theory(DisplayName = "GIVEN range with single value WHEN array populated THEN all values equal specified one")]
        public void GivenRangeWithSingleValue_WhenArrayPopulated_ThenAllValuesEqualSpecifiedOne(
            [PickFromRange(int.MinValue, int.MinValue)] int[] rangeValues)
        {
            // Arrange
            // Act
            // Assert
            rangeValues.Should().HaveCountGreaterThan(1).And.AllSatisfy(x => x.Should().Be(int.MinValue));
        }
    }
}
