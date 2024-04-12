namespace Objectivity.AutoFixture.XUnit2.Core.Tests.Attributes
{
    using System;
    using System.Diagnostics.CodeAnalysis;
    using System.Globalization;
    using System.Linq;
    using System.Reflection;

    using FluentAssertions;

    using global::AutoFixture;
    using global::AutoFixture.Kernel;
    using global::AutoFixture.Xunit2;

    using Moq;

    using Objectivity.AutoFixture.XUnit2.Core.Attributes;

    using Xunit;

    [Collection("PickNegativeAttribute")]
    [Trait("Category", "CustomizeAttribute")]
    public class PickNegativeAttributeTests
    {
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

        public static TheoryData<object> CustomizationUsageTestData { get; } = new()
        {
            { 1 },
            { 1L },
            { (short)1 },
            { (sbyte)1 },
            { 1F },
            { 1D },
            { 1M },
        };

        [Fact(DisplayName = "GIVEN uninitialized argument WHEN GetCustomization is invoked THEN exception is thrown")]
        public void GivenUninitializedArgument_WhenGetCustomizationIsInvoked_ThenExceptionIsThrown()
        {
            // Arrange
            var attribute = new PickNegativeAttribute();

            // Act
            Action act = () => attribute.GetCustomization(null);

            // Assert
            act.Should().Throw<ArgumentNullException>()
                .And.ParamName.Should().Be("parameter");
        }

        [MemberData(nameof(CustomizationUsageTestData))]
        [Theory(DisplayName = "GIVEN supported numeric type WHEN GetCustomization is invoked THEN returns negative value")]
        public void GivenSupportedNumericType_WhenGetCustomizationIsInvoked_ThenReturnsNegativeValue<T>(
            T item)
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
            item.Should().BeOfType(type)
                .And.Match(x => x.CompareTo(zero) < 0);
        }

        [MemberData(nameof(CustomizationUsageTestData))]
        [Theory(DisplayName = "GIVEN supported numeric array type WHEN GetCustomization is invoked THEN returns negative values")]
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

        [InlineData(SignedByteNumbers.MinusTwo, SignedByteNumbers.MinusOne)]
        [InlineData(ShortNumbers.MinusTwo, ShortNumbers.MinusOne)]
        [InlineData(IntNumbers.MinusTwo, IntNumbers.MinusOne)]
        [InlineData(LongNumbers.MinusTwo, LongNumbers.MinusOne)]
        [Theory(DisplayName = "GIVEN supported enum type WHEN GetCustomization is invoked THEN returns negative value")]
        [SuppressMessage("ReSharper", "CoVariantArrayConversion", Justification = "This is good enougth for object comparison.")]
        public void GivenSupportedEnumType_WhenGetCustomizationIsInvoked_ThenReturnsNegativeValue(
            params Enum[] expectedValues)
        {
            // Arrange
            var attribute = new PickNegativeAttribute();
            var request = new Mock<ParameterInfo>();
            var type = expectedValues[0].GetType();
            request.SetupGet(x => x.ParameterType)
                .Returns(type);
            IFixture fixture = new Fixture();
            fixture.Customize(attribute.GetCustomization(request.Object));

            // Act
            var result = (Enum)fixture.Create(request.Object, new SpecimenContext(fixture));

            // Assert
            result.Should().BeOfType(type)
                .And.BeOneOf(expectedValues);
        }

        [InlineData(SignedByteNumbers.MinusTwo, SignedByteNumbers.MinusOne)]
        [InlineData(ShortNumbers.MinusTwo, ShortNumbers.MinusOne)]
        [InlineData(IntNumbers.MinusTwo, IntNumbers.MinusOne)]
        [InlineData(LongNumbers.MinusTwo, LongNumbers.MinusOne)]
        [Theory(DisplayName = "GIVEN supported enum array type WHEN GetCustomization is invoked THEN returns negative values")]
        [SuppressMessage("ReSharper", "CoVariantArrayConversion", Justification = "This is good enougth for object comparison.")]
        public void GivenSupportedEnumArrayType_WhenGetCustomizationIsInvoked_ThenReturnsNegativeValues(
            params Enum[] expectedValues)
        {
            // Arrange
            var attribute = new PickNegativeAttribute();
            var request = new Mock<ParameterInfo>();
            var type = Array.CreateInstance(expectedValues[0].GetType(), 0).GetType();
            request.SetupGet(x => x.ParameterType)
                .Returns(type);
            IFixture fixture = new Fixture();
            fixture.Customize(attribute.GetCustomization(request.Object));

            // Act
            var result = ((Array)fixture.Create(request.Object, new SpecimenContext(fixture))).Cast<Enum>();

            // Assert
            result.Should().AllSatisfy(x => x.Should().BeOneOf(expectedValues));
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

        [AutoData]
        [Theory(DisplayName = "GIVEN PickNegative attribute specified WHEN integer enum populated THEN the negative value is generated")]
        public void GivenPickNegativeAttributeSpecified_WhenIntegerEnumPopulated_ThenTheNegativeValueIsGenerated(
            [PickNegative] IntNumbers targetValue)
        {
            // Arrange
            // Act
            // Assert
            targetValue.Should().BeOneOf(IntNumbers.MinusOne, IntNumbers.MinusTwo);
        }
    }
}
