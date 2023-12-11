namespace Objectivity.AutoFixture.XUnit2.Core.Tests.Attributes
{
    using System;
    using System.Collections.Generic;
    using System.Globalization;
    using System.Reflection;

    using FluentAssertions;

    using global::AutoFixture;
    using global::AutoFixture.Kernel;
    using global::AutoFixture.Xunit2;

    using Moq;

    using Objectivity.AutoFixture.XUnit2.Core.Attributes;

    using Xunit;

    public class PickNegativeAttributeTests
    {
        public enum Numbers
        {
            MinusFour = -4,
            MinusTwo = -2,
            MinusOne = -1,
            Zero = 0,
            One = 1,
            Two = 2,
            Four = 4,
        }

        public static IEnumerable<object[]> CustomizationUsageTestData { get; } = new[]
        {
            new object[] { 1 },
            [1L],
            [(short)1],
            [(sbyte)1],
            [1F],
            [1D],
            [1M],
        };

        [Fact(DisplayName = "GIVEN uninitialized argument WHEN GetCustomization is invoked THEN exception is thrown")]
        public void GivenUninitializedArgument_WhenGetCustomizationIsInvoked_ThenExceptionIsThrown()
        {
            // Arrange
            var attribute = new PickNegativeAttribute();

            // Act
            Action act = () => attribute.GetCustomization(null);

            // Assert
            act.Should().Throw<ArgumentNullException>().And.ParamName.Should().Be("parameter");
        }

        [MemberData(nameof(CustomizationUsageTestData))]
        [Theory(DisplayName = "GIVEN supported numeric type WHEN GetCustomization is invoked THEN returns negative value")]
        public void GivenSupportedNumericType_WhenGetCustomizationIsInvoked_ThenReturnsNegativeValue<T>(T item)
            where T : IComparable<T>
        {
            // Arrange
            var attribute = new PickNegativeAttribute();
            var request = new Mock<ParameterInfo>();
            var type = typeof(T);
            request.SetupGet(x => x.ParameterType)
                .Returns(type);
            IFixture fixture = new Fixture();
            fixture.Customize(attribute.GetCustomization(request.Object));
            var zero = (T)Convert.ChangeType(0, type, CultureInfo.InvariantCulture);

            // Act
            item = (T)fixture.Create(request.Object, new SpecimenContext(fixture));

            // Assert
            item.Should().BeOfType(type).And.Match(x => x.CompareTo(zero) < 0);
        }

        [MemberData(nameof(CustomizationUsageTestData))]
        [Theory(DisplayName = "GIVEN supported numeric array type WHEN GetCustomization is invoked THEN returns negative value")]
        public void GivenSupportedNumericArrayType_WhenGetCustomizationIsInvoked_ThenReturnsNegativeValues<T>(
            T item)
            where T : IComparable<T>
        {
            // Arrange
            var attribute = new PickNegativeAttribute();
            var request = new Mock<ParameterInfo>();
            request.SetupGet(x => x.ParameterType)
                .Returns(typeof(T[]));
            IFixture fixture = new Fixture();
            fixture.Customize(attribute.GetCustomization(request.Object));
            var zero = (T)Convert.ChangeType(0, item.GetType(), CultureInfo.InvariantCulture);

            // Act
            var result = (T[])fixture.Create(request.Object, new SpecimenContext(fixture));

            // Assert
            result.Should().AllSatisfy(x => x.Should().BeLessThan(zero));
        }

        [Fact(DisplayName = "GIVEN supported enum type WHEN GetCustomization is invoked THEN returns negative value")]
        public void GivenSupportedEnumType_WhenGetCustomizationIsInvoked_ThenReturnsNegativeValue()
        {
            // Arrange
            var type = typeof(Numbers);
            var attribute = new PickNegativeAttribute();
            var request = new Mock<ParameterInfo>();
            request.SetupGet(x => x.ParameterType)
                .Returns(type);
            IFixture fixture = new Fixture();
            fixture.Customize(attribute.GetCustomization(request.Object));

            // Act
            var result = fixture.Create(request.Object, new SpecimenContext(fixture));

            // Assert
            result.Should().BeOfType(type)
                .And.Subject.As<Numbers>().Should().BeOneOf(Numbers.MinusFour, Numbers.MinusTwo, Numbers.MinusOne);
        }

        [AutoData]
        [Theory(DisplayName = "GIVEN PickNegative attribute specified WHEN signed byte populated THEN the negative value is generated")]
        public void GivenPickNegativeAttributeSpecified_WhenSBytePopulated_ThenTheNegativeValueIsGenerated(
            [PickNegative] sbyte targetValue)
        {
            // Arrange
            // Act
            // Assert
            targetValue.Should().BeLessThan(0);
        }

        [AutoData]
        [Theory(DisplayName = "GIVEN PickNegative attribute specified WHEN short populated THEN the negative value is generated")]
        public void GivenPickNegativeAttributeSpecified_WhenShortPopulated_ThenTheNegativeValueIsGenerated(
            [PickNegative] short targetValue)
        {
            // Arrange
            // Act
            // Assert
            targetValue.Should().BeLessThan(0);
        }

        [AutoData]
        [Theory(DisplayName = "GIVEN PickNegative attribute specified WHEN integer populated THEN the negative value is generated")]
        public void GivenPickNegativeAttributeSpecified_WhenIntegerPopulated_ThenTheNegativeValueIsGenerated(
            [PickNegative] int targetValue)
        {
            // Arrange
            // Act
            // Assert
            targetValue.Should().BeLessThan(0);
        }

        [AutoData]
        [Theory(DisplayName = "GIVEN PickNegative attribute specified WHEN long populated THEN the negative value is generated")]
        public void GivenPickNegativeAttributeSpecified_WhenLongPopulated_ThenTheNegativeValueIsGenerated(
            [PickNegative] long targetValue)
        {
            // Arrange
            // Act
            // Assert
            targetValue.Should().BeLessThan(0);
        }

        [AutoData]
        [Theory(DisplayName = "GIVEN PickNegative attribute specified WHEN float populated THEN the negative value is generated")]
        public void GivenPickNegativeAttributeSpecified_WhenFloatPopulated_ThenTheNegativeValueIsGenerated(
            [PickNegative] float targetValue)
        {
            // Arrange
            // Act
            // Assert
            targetValue.Should().BeLessThan(0);
        }

        [AutoData]
        [Theory(DisplayName = "GIVEN PickNegative attribute specified WHEN double populated THEN the negative value is generated")]
        public void GivenPickNegativeAttributeSpecified_WhenDoublePopulated_ThenTheNegativeValueIsGenerated(
            [PickNegative] double targetValue)
        {
            // Arrange
            // Act
            // Assert
            targetValue.Should().BeLessThan(0);
        }

        [AutoData]
        [Theory(DisplayName = "GIVEN PickNegative attribute specified WHEN decimal populated THEN the negative value is generated")]
        public void GivenPickNegativeAttributeSpecified_WhenDecimalPopulated_ThenTheNegativeValueIsGenerated(
            [PickNegative] decimal targetValue)
        {
            // Arrange
            // Act
            // Assert
            targetValue.Should().BeLessThan(0);
        }

        [InlineData(null)]
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
        [InlineData(typeof(ValueTuple))]
        [Theory(DisplayName = "GIVEN unsupported type WHEN GetCustomization is invoked THEN exception is thrown")]
        public void GivenUnsupportedType_WhenGetCustomizationIsInvoked_ThrowsExceptionIsThrown(
            Type type)
        {
            // Arrange
            var attribute = new PickNegativeAttribute();
            var request = new Mock<ParameterInfo>();
            request.SetupGet(x => x.ParameterType)
                .Returns(type);
            IFixture fixture = new Fixture();
            fixture.Customize(attribute.GetCustomization(request.Object));

            // Act
            Action act = () => fixture.Create(request.Object, new SpecimenContext(fixture));

            // Assert
            act.Should().Throw<ObjectCreationException>();
        }
    }
}
