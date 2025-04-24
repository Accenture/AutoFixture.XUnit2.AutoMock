namespace Objectivity.AutoFixture.XUnit2.Core.Tests.Attributes
{
    using System;
    using System.Reflection;

    using global::AutoFixture;
    using global::AutoFixture.Kernel;
    using global::AutoFixture.Xunit2;

    using Moq;

    using Objectivity.AutoFixture.XUnit2.Core.Attributes;

    using Xunit;

    [Collection("PickFromValuesAttribute")]
    [Trait("Category", "CustomizeAttribute")]
    public class PickFromValuesAttributeTests
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

        public static TheoryData<int, int> MemberAutoDataOverValuesTestData { get; } = new()
        {
            { 10, 10 },
        };

        public static TheoryData<object, object, object> CustomizationUsageTestData { get; } = new()
        {
            { 1, 1, 2 },
            { 1.5f, 1.5f, -0.3f },
            { string.Empty, "a", "b" },
            { false, false, false },
            { DateTime.UtcNow, DateTime.UtcNow, DateTime.MinValue },
            { Numbers.Five, Numbers.Five, Numbers.One },
            { ValueTuple.Create(5), ValueTuple.Create(5), ValueTuple.Create(-3) },
            { Tuple.Create(1, 2), Tuple.Create(1, 2), Tuple.Create(-1, 0) },
            { new(), 1, 1.5f },
        };

        [Fact(DisplayName = "GIVEN uninitialized argument WHEN constructor is invoked THEN exception is thrown")]
        public void GivenUninitializedArgument_WhenConstructorIsInvoked_ThenExceptionIsThrown()
        {
            // Arrange
            // Act
            static object Act() => new PickFromValuesAttribute(null);

            // Assert
            var exception = Assert.Throws<ArgumentNullException>(Act);
            Assert.Equal("values", exception.ParamName);
        }

        [AutoData]
        [Theory(DisplayName = "GIVEN uninitialized argument WHEN GetCustomization is invoked THEN exception is thrown")]
        public void GivenUninitializedArgument_WhenGetCustomizationIsInvoked_ThenExceptionIsThrown(
            int[] values)
        {
            // Arrange
            var attribute = new PickFromValuesAttribute(values);

            // Act
            void Act() => attribute.GetCustomization(null);

            // Assert
            var exception = Assert.Throws<ArgumentNullException>(Act);
            Assert.Equal("parameter", exception.ParamName);
        }

        [Fact(DisplayName = "GIVEN no arguments WHEN constructor is invoked THEN exception is thrown")]
        public void GivenNoArguments_WhenConstructorIsInvoked_ThenExceptionIsThrown()
        {
            // Arrange
            // Act
            static object Act() => new PickFromValuesAttribute();

            // Assert
            var exception = Assert.Throws<ArgumentException>(Act);
            Assert.NotNull(exception.Message);
            Assert.NotEmpty(exception.Message);
            Assert.Contains("At least one value", exception.Message);
        }

        [InlineData(1, 1)]
        [InlineData("a", "a")]
        [Theory(DisplayName = "GIVEN identical arguments WHEN constructor is invoked THEN unique parameters are properly assigned")]
        public void GivenIdenticalArguments_WhenConstructorIsInvoked_ThenUniqueParametersAreProperlyAssigned<T>(
            T first,
            T second)
        {
            // Arrange
            var attribute = new PickFromValuesAttribute(first, second);

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
            var attribute = new PickFromValuesAttribute(first, second);

            // Assert
            Assert.Equal(2, attribute.Values.Count);
            Assert.Equivalent(new[] { first, second }, attribute.Values);
        }

        [MemberData(nameof(CustomizationUsageTestData))]
        [Theory(DisplayName = "GIVEN valid parameters WHEN customization is used THEN expected values are generated")]
        public void GivenValidParameters_WhenCustomizationIsUsed_ThenExpectedValuesAreGenerated<T>(
            T item,
            params object[] values)
        {
            // Arrange
            var attribute = new PickFromValuesAttribute(values);
            var request = new Mock<ParameterInfo>();
            request.SetupGet(x => x.ParameterType)
                .Returns(item.GetType());
            IFixture fixture = new Fixture();
            fixture.Customize(attribute.GetCustomization(request.Object));

            // Act
            item = (T)fixture.Create(request.Object, new SpecimenContext(fixture));

            // Assert
            Assert.NotNull(item);
            Assert.Contains(item, values);
        }

        [AutoData]
        [Theory(DisplayName = "GIVEN values specified WHEN byte populated THEN the value from set is generated")]
        public void GivenValuesSpecified_WhenBytePopulated_ThenTheValueFromSetIsGenerated(
            [PickFromValues(1, 5, 20)] byte targetValue)
        {
            // Arrange
            var expectedValues = new byte[] { 1, 5, 20 };

            // Act
            // Assert
            Assert.Contains(targetValue, expectedValues);
        }

        [AutoData]
        [Theory(DisplayName = "GIVEN values specified WHEN short populated THEN the value from set is generated")]
        public void GivenValuesSpecified_WhenUShortPopulated_ThenTheValueFromSetIsGenerated(
            [PickFromValues(1, 5, 4)] ushort targetValue)
        {
            // Arrange
            var expectedValues = new ushort[] { 1, 5, 4 };

            // Act
            // Assert
            Assert.Contains(targetValue, expectedValues);
        }

        [AutoData]
        [Theory(DisplayName = "GIVEN values specified WHEN unsigned long populated THEN the value from set is generated")]
        public void GivenValuesSpecified_WhenULongPopulated_ThenTheValueFromSetIsGenerated(
            [PickFromValues(1, long.MaxValue, ulong.MaxValue)] ulong targetValue)
        {
            // Arrange
            var expectedValues = new ulong[] { 1, long.MaxValue, ulong.MaxValue };

            // Act
            // Assert
            Assert.Contains(targetValue, expectedValues);
        }

        [AutoData]
        [Theory(DisplayName = "GIVEN values specified WHEN signed byte populated THEN the value from set is generated")]
        public void GivenValuesSpecified_WhenSBytePopulated_ThenTheValueFromSetIsGenerated(
            [PickFromValues(sbyte.MinValue, -50, -1)] sbyte targetValue)
        {
            // Arrange
            var expectedValues = new sbyte[] { sbyte.MinValue, -50, -1 };

            // Act
            // Assert
            Assert.Contains(targetValue, expectedValues);
        }

        [AutoData]
        [Theory(DisplayName = "GIVEN values specified WHEN short populated THEN only decorated parameter has value from specification")]
        public void GivenValuesSpecified_WhenShortPopulated_ThenOnlyDecoratedParameterHasValueFromSpecification(
            [PickFromValues(short.MinValue, sbyte.MinValue, -1)] short targetValue,
            short unrestrictedValue)
        {
            // Arrange
            var expectedValues = new short[] { short.MinValue, sbyte.MinValue, -1 };

            // Act
            // Assert
            Assert.Contains(targetValue, expectedValues);
            Assert.True(unrestrictedValue >= 0);
        }

        [AutoData]
        [Theory(DisplayName = "GIVEN values specified WHEN integer populated THEN only decorated parameter has value from specification")]
        public void GivenValuesSpecified_WhenIntPopulated_ThenOnlyDecoratedParameterHasValueFromSpecification(
            [PickFromValues(int.MinValue, short.MinValue, sbyte.MinValue)] int targetValue,
            int unrestrictedValue)
        {
            // Arrange
            var expectedValues = new[] { int.MinValue, short.MinValue, sbyte.MinValue };

            // Act
            // Assert
            Assert.Contains(targetValue, expectedValues);
            Assert.True(unrestrictedValue >= 0);
        }

        [AutoData]
        [Theory(DisplayName = "GIVEN values specified WHEN long populated THEN only decorated parameter has value from specification")]
        public void GivenValuesSpecified_WhenLongPopulated_ThenOnlyDecoratedParameterHasValueFromSpecification(
            [PickFromValues(long.MinValue, int.MinValue, short.MinValue)] long targetValue,
            long unrestrictedValue)
        {
            // Arrange
            var expectedValues = new[] { long.MinValue, int.MinValue, short.MinValue };

            // Act
            // Assert
            Assert.Contains(targetValue, expectedValues);
            Assert.True(unrestrictedValue >= 0);
        }

        [AutoData]
        [Theory(DisplayName = "GIVEN values specified WHEN float populated THEN only decorated parameter has value from specification")]
        public void GivenValuesSpecified_WhenFloatPopulated_ThenOnlyDecoratedParameterHasValueFromSpecification(
            [PickFromValues(float.MinValue, int.MinValue, short.MinValue)] float targetValue,
            float unrestrictedValue)
        {
            // Arrange
            var expectedValues = new[] { float.MinValue, int.MinValue, short.MinValue };

            // Act
            // Assert
            Assert.Contains(targetValue, expectedValues);
            Assert.True(unrestrictedValue >= 0);
        }

        [AutoData]
        [Theory(DisplayName = "GIVEN values specified WHEN double populated THEN only decorated parameter has value from specification")]
        public void GivenValuesSpecified_WhenDoublePopulated_ThenOnlyDecoratedParameterHasValueFromSpecification(
            [PickFromValues(double.MinValue, float.MinValue, int.MinValue, short.MinValue)] double targetValue,
            double unrestrictedValue)
        {
            // Arrange
            var expectedValues = new[] { double.MinValue, float.MinValue, int.MinValue, short.MinValue };

            // Act
            // Assert
            Assert.Contains(targetValue, expectedValues);
            Assert.True(unrestrictedValue >= 0);
        }

        [AutoData]
        [Theory(DisplayName = "GIVEN values specified WHEN decimal populated THEN only decorated parameter has value from specification")]
        public void GivenValuesSpecified_WhenDecimalPopulated_ThenOnlyDecoratedParameterHasValueFromSpecification(
            [PickFromValues(long.MinValue, int.MinValue, short.MinValue)] decimal targetValue,
            decimal unrestrictedValue)
        {
            // Arrange
            var expectedValues = new decimal[] { long.MinValue, int.MinValue, short.MinValue };

            // Act
            // Assert
            Assert.Contains(targetValue, expectedValues);
            Assert.True(unrestrictedValue >= 0);
        }

        [AutoData]
        [Theory(DisplayName = "GIVEN values specified for argument WHEN enum populated THEN the value from set is generated")]
        public void GivenValuesSpecifiedForArgument_WhenEnumPopulated_ThenTheValueFromSetIsGenerated(
            [PickFromValues(Numbers.One, Numbers.Five)] Numbers targetValue)
        {
            // Arrange
            var expectedValues = new[] { Numbers.One, Numbers.Five };

            // Act
            // Assert
            Assert.Contains(targetValue, expectedValues);
        }

        [AutoData]
        [Theory(DisplayName = "GIVEN values outside enum specified for argument WHEN enum populated THEN the value from set is generated")]
        public void GivenValuesOutsideEnumSpecifiedForArgument_WhenEnumPopulated_ThenTheValueFromSetIsGenerated(
            [PickFromValues(100, 200)] Numbers targetValue)
        {
            // Arrange
            var expectedValues = new[] { (Numbers)100, (Numbers)200 };

            // Act
            // Assert
            Assert.Contains(targetValue, expectedValues);
        }

        [AutoData]
        [Theory(DisplayName = "GIVEN values specified for argument WHEN flag populated THEN the value from set is generated")]
        public void GivenValuesSpecifiedForArgument_WhenFlagPopulated_ThenTheValueFromSetIsGenerated(
            [PickFromValues(Numbers.One | Numbers.Three, Numbers.Five)] Numbers targetValue)
        {
            // Arrange
            var expectedValues = new[] { Numbers.One | Numbers.Three, Numbers.Five };

            // Act
            // Assert
            Assert.Contains(targetValue, expectedValues);
        }

        [InlineAutoData(10, 10)]
        [InlineAutoData(0, 0)]
        [Theory(DisplayName = "GIVEN values specified and inline value outside values WHEN data populated THEN values definition is ignored and inline one is used")]
        public void GivenValuesSpecifiedAndInlineValueOutsideValues_WhenDataPopulated_ThenValuesDefinitionIsIgnoredAndInlineOneIsUsed(
            [PickFromValues(int.MinValue)] int value, int expectedResult)
        {
            // Arrange
            // Act
            // Assert
            Assert.Equal(expectedResult, value);
            Assert.NotEqual(int.MinValue, value);
        }

        [MemberAutoData(nameof(MemberAutoDataOverValuesTestData))]
        [Theory(DisplayName = "GIVEN values specified and member data value outside values WHEN data populated THEN values definition is ignored and member data is used")]
        public void GivenValuesSpecifiedAndMemberDataValueOutsideValues_WhenDataPopulated_ThenValuesDefinitionIsIgnoredAndMemberDataIsUsed(
            [PickFromValues(int.MinValue)] int value,
            int expectedResult,
            [PickFromValues(int.MinValue)] int unrestrictedValue)
        {
            // Arrange
            // Act
            // Assert
            Assert.Equal(expectedResult, value);
            Assert.NotEqual(int.MinValue, value);
            Assert.Equal(int.MinValue, unrestrictedValue);
        }

        [AutoData]
        [Theory(DisplayName = "GIVEN values specified for collection WHEN unsigned short populated THEN the value from set is generated")]
        public void GivenValuesSpecifiedForCollection_WhenEnumPopulated_ThenTheValueFromSetIsGenerated(
            [PickFromValues(int.MinValue, (int)short.MinValue, -1)] int[] targetValues,
            int[] unrestrictedValues)
        {
            // Arrange
            var supported = new[] { int.MinValue, -1, short.MinValue };

            // Act
            // Assert
            Assert.All(targetValues, x => Assert.Contains(x, supported));
            Assert.All(unrestrictedValues, x => Assert.True(x >= 0));
        }
    }
}
