namespace Objectivity.AutoFixture.XUnit2.Core.Tests.Factories
{
    using System;
    using System.Diagnostics.CodeAnalysis;

    using FluentAssertions;

    using global::AutoFixture;
    using global::AutoFixture.Kernel;

    using Objectivity.AutoFixture.XUnit2.Core.Factories;
    using Objectivity.AutoFixture.XUnit2.Core.Requests;

    using Xunit;

    [Collection("NegativeValuesRequestFactory")]
    [Trait("Category", "Factories")]
    public class NegativeValuesRequestFactoryTests
    {
        private readonly NegativeValuesRequestFactory factory = new();

        [SuppressMessage("Design", "CA1028:Enum Storage should be Int32", Justification = "Required for test")]
        public enum ByteNumbers : byte
        {
            Zero = 0,
            One = 1,
            Two = 2,
        }

        [SuppressMessage("Design", "CA1028:Enum Storage should be Int32", Justification = "Required for test")]
        public enum SignedByteNumbers : sbyte
        {
            MinusTwo = -2,
            MinusOne = -1,
            Zero = 0,
            One = 1,
            Two = 2,
        }

        [SuppressMessage("Design", "CA1028:Enum Storage should be Int32", Justification = "Required for test")]
        public enum ShortNumbers : short
        {
            MinusTwo = -2,
            MinusOne = -1,
            Zero = 0,
            One = 1,
            Two = 2,
        }

        public enum IntNumbers
        {
            MinusTwo = -2,
            MinusOne = -1,
            Zero = 0,
            One = 1,
            Two = 2,
        }

        [SuppressMessage("Design", "CA1028:Enum Storage should be Int32", Justification = "Required for test")]
        public enum LongNumbers : long
        {
            MinusTwo = -2,
            MinusOne = -1,
            Zero = 0,
            One = 1,
            Two = 2,
        }

        public static TheoryData<Type, object, object> NumericTypeUsageTestData { get; } = new()
        {
            { typeof(decimal), decimal.MinValue, new decimal(1, 0, 0, true, 28) },
            { typeof(double), double.MinValue, -double.Epsilon },
            { typeof(short), short.MinValue, (short)-1 },
            { typeof(int), int.MinValue, -1 },
            { typeof(long), long.MinValue, -1L },
            { typeof(sbyte), sbyte.MinValue, (sbyte)-1 },
            { typeof(float), float.MinValue, -float.Epsilon },
        };

        [Fact(DisplayName = "GIVEN uninitialized argument WHEN Create is invoked THEN exception is thrown")]
        public void GivenUninitializedArgument_WhenCreateIsInvoked_ThenExceptionIsThrown()
        {
            // Arrange
            Type type = null;

            // Act
            Action act = () => this.factory.Create(type);

            // Assert
            act.Should().Throw<ArgumentNullException>()
                .And.ParamName.Should().Be("input");
        }

        [InlineData(typeof(SignedByteNumbers), SignedByteNumbers.MinusTwo, SignedByteNumbers.MinusOne)]
        [InlineData(typeof(ShortNumbers), ShortNumbers.MinusTwo, ShortNumbers.MinusOne)]
        [InlineData(typeof(IntNumbers), IntNumbers.MinusTwo, IntNumbers.MinusOne)]
        [InlineData(typeof(LongNumbers), LongNumbers.MinusTwo, LongNumbers.MinusOne)]
        [Theory(DisplayName = "GIVEN supported enum type WHEN Create is invoked THEN returns request with negative values only")]
        public void GivenSupportedEnumType_WhenCreateIsInvoked_ThenReturnsRequestWithNegativeValuesOnly(
            Type type,
            params object[] expectedValues)
        {
            // Arrange
            // Act
            var result = this.factory.Create(type);

            // Assert
            result.Should().BeOfType<FixedValuesRequest>().Subject
                .Values.Should().BeEquivalentTo(expectedValues);
        }

        [MemberData(nameof(NumericTypeUsageTestData))]
        [Theory(DisplayName = "GIVEN supported numeric type WHEN Create is invoked THEN returns request with expected range")]
        public void GivenSupportedNumericType_WhenCreateIsInvoked_ThenReturnsRequestWithExpectedRange<T>(
            Type type,
            T min,
            T max)
        {
            // Arrange
            // Act
            var result = this.factory.Create(type);

            // Assert
            var request = result.Should().BeOfType<RangedNumberRequest>().Subject;
            request.Minimum.Should().Be(min);
            request.Maximum.Should().Be(max);
        }

        [InlineData(typeof(DBNull))]
        [InlineData(typeof(object))]
        [InlineData(typeof(byte))]
        [InlineData(typeof(ushort))]
        [InlineData(typeof(uint))]
        [InlineData(typeof(ulong))]
        [InlineData(typeof(bool))]
        [InlineData(typeof(char))]
        [InlineData(typeof(string))]
        [InlineData(typeof(DateTime))]
        [InlineData(typeof(DayOfWeek))]
        [InlineData(typeof(ByteNumbers))]
        [InlineData(typeof(ValueTuple))]
        [Theory(DisplayName = "GIVEN unsupported type WHEN Create is invoked THEN throws exception")]
        public void GivenUnsupportedType_WhenCreateIsInvoked_ThenThrowsException(
            Type type)
        {
            // Arrange
            // Act
            Action act = () => this.factory.Create(type);

            // Assert
            act.Should().Throw<ObjectCreationException>()
                .And.Message.Should().Contain("does not accept negative values");
        }
    }
}
