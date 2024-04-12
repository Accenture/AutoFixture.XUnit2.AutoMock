namespace Objectivity.AutoFixture.XUnit2.Core.Tests.Requests
{
    using System;
    using System.Collections;
    using System.Diagnostics.CodeAnalysis;
    using System.Globalization;
    using System.Linq;

    using FluentAssertions;
    using global::AutoFixture.Xunit2;
    using Objectivity.AutoFixture.XUnit2.Core.Requests;

    using Xunit;

    [Collection("ValuesRequest")]
    [Trait("Category", "Requests")]
    public class ValuesRequestTests
    {
        public static TheoryData<Type, IEnumerable, Type, IEnumerable, bool> ComparisonTestData { get; } = new()
        {
            { typeof(int), new[] { 1 }, typeof(int), new[] { 1 }, true },
            { typeof(int), new[] { 1, 2, 3 }, typeof(int), new[] { 3, 2, 1 }, true },
            { typeof(int?), new[] { 1, (int?)null }, typeof(int?), new[] { 1, null, (int?)null }, true },
            { typeof(int), new[] { 1 }, typeof(long), new[] { 1 }, false },
            { typeof(int), new[] { 1 }, typeof(int), new[] { 2 }, false },
            { typeof(int), new[] { 1, 2 }, typeof(int), new[] { 2 }, false },
        };

        [Fact(DisplayName = "GIVEN uninitialized type argument WHEN constructor is invoked THEN exception is thrown")]
        public void GivenUninitializedTypeArgument_WhenConstructorIsInvoked_ThenExceptionIsThrown()
        {
            // Arrange
            Type type = null;
            object[] values = null;

            // Act
            Func<object> act = () => new FixedValuesRequest(type, values);

            // Assert
            act.Should().Throw<ArgumentNullException>()
                .And.ParamName.Should().Be("operandType");
        }

        [Fact(DisplayName = "GIVEN uninitialized values argument WHEN constructor is invoked THEN exception is thrown")]
        public void GivenUninitializedValuesArgument_WhenConstructorIsInvoked_ThenExceptionIsThrown()
        {
            // Arrange
            var type = typeof(int);
            object[] values = null;

            // Act
            Func<object> act = () => new FixedValuesRequest(type, values);

            // Assert
            act.Should().Throw<ArgumentNullException>()
                .And.ParamName.Should().Be("values");
        }

        [Fact(DisplayName = "GIVEN empty values argument WHEN constructor is invoked THEN exception is thrown")]
        public void GivenEmptyValuesArgument_WhenConstructorIsInvoked_ThenExceptionIsThrown()
        {
            // Arrange
            var type = typeof(int);
            var values = Array.Empty<object>();

            // Act
            Func<object> act = () => new FixedValuesRequest(type, values);

            // Assert
            act.Should().Throw<ArgumentException>()
                .And.Message.Should().NotBeNullOrEmpty()
                .And.Contain("At least one value");
        }

        [InlineData(typeof(int), 2)]
        [InlineData(1, typeof(int))]
        [Theory(DisplayName = "GIVEN different type arguments WHEN constructor is invoked THEN parameters are properly assigned")]
        public void GivenDifferentTypeArguments_WhenConstructorIsInvoked_ThenParametersAreProperlyAssigned(
            params object[] values)
        {
            // Arrange
            var type = typeof(int);

            // Act
            var attribute = new FixedValuesRequest(type, values);

            // Assert
            attribute.Values.Should().HaveCount(2).And.BeEquivalentTo(values);
        }

        [Fact(DisplayName = "GIVEN valid arguments WHEN ToString is invoked THEN text conteins necessary information")]
        public void GivenValidArguments_WhenToStringIsInvoked_ThenTextConteinsNecessaryInformation()
        {
            // Arrange
            var type = typeof(float);
            const int first = int.MaxValue;
            long? second = long.MinValue;
            byte? third = null;
            var attribute = new FixedValuesRequest(type, first, second, third);

            // Act
            var text = attribute.ToString();

            // Assert
            text.Should().Contain(nameof(FixedValuesRequest))
                .And.Contain(type.Name)
                .And.Contain("[Int32]")
                .And.Contain($"{first.ToString(CultureInfo.InvariantCulture)},")
                .And.Contain("[Int64]")
                .And.Contain($"{second.Value.ToString(CultureInfo.InvariantCulture)},")
                .And.Contain("[Object]")
                .And.Contain("null");
        }

        [MemberData(nameof(ComparisonTestData))]
        [Theory(DisplayName = "GIVEN two requests WHEN Equals is invoked THEN expected value is returned")]
        public void GivenTwoRequests_WhenEqualsIsInvoked_ThenExpectedValueIsReturned(
            Type typeA,
            IEnumerable valuesA,
            Type typeB,
            IEnumerable valuesB,
            bool expectedResult)
        {
            // Arrange
            var a = new FixedValuesRequest(typeA, valuesA.Cast<object>().ToArray());
            var b = new FixedValuesRequest(typeB, valuesB.Cast<object>().ToArray());

            // Act
            var result = Equals(a, b);

            // Assert
            result.Should().Be(expectedResult);
        }

        [MemberData(nameof(ComparisonTestData))]
        [Theory(DisplayName = "GIVEN two requests WHEN hashcodes are compared THEN expected value is returned")]
        public void GivenTwoRequests_WhenHashCodesAreCompared_ThenExpectedValueIsReturned(
            Type typeA,
            IEnumerable valuesA,
            Type typeB,
            IEnumerable valuesB,
            bool expectedResult)
        {
            // Arrange
            var a = new FixedValuesRequest(typeA, valuesA.Cast<object>().ToArray());
            var b = new FixedValuesRequest(typeB, valuesB.Cast<object>().ToArray());

            // Act
            var hashA = a.GetHashCode();
            var hashB = b.GetHashCode();
            var result = hashA == hashB;

            // Assert
            result.Should().Be(expectedResult, $"Hash codes are different. First: {a}, HashCode: {hashA.ToString(CultureInfo.InvariantCulture)}, Second:{b}, HashCode: {hashB.ToString(CultureInfo.InvariantCulture)}");
        }

        [Fact(DisplayName = "GIVEN uninitialized request WHEN Equals is invoked THEN False is returned")]
        [SuppressMessage("Maintainability", "CA1508:Avoid dead conditional code", Justification = "Required to test the logic")]
        public void GivenUninitializedRequest_WhenEqualsIsInvoked_ThenFalseIsReturned()
        {
            // Arrange
            var initialized = new FixedValuesRequest(typeof(int), 1);
            FixedValuesRequest uninitialized = null;

            // Act
            var result = initialized.Equals(uninitialized);

            // Assert
            result.Should().BeFalse();
        }

        [Fact(DisplayName = "GIVEN different type of object WHEN Equals is invoked THEN False is returned")]
        [SuppressMessage("ReSharper", "SuspiciousTypeConversion.Global", Justification = "This is expected comparioson to test logic.")]
        public void GivenDifferentTypeOfObject_WhenEqualsIsInvoked_ThenFalseIsReturned()
        {
            // Arrange
            var request = new FixedValuesRequest(typeof(int), 1);
            var differentObject = typeof(int);

            // Act
            var result = request.Equals(differentObject);

            // Assert
            result.Should().BeFalse();
        }

        [AutoData]
        [Theory(DisplayName = "GIVEN different type of ValuesRequest WHEN Equals is invoked THEN False is returned")]
        [SuppressMessage("ReSharper", "SuspiciousTypeConversion.Global", Justification = "This is expected comparioson to test logic.")]
        public void GivenDifferentTypeOfValuesRequest_WhenEqualsIsInvoked_ThenFalseIsReturned(
            int value)
        {
            // Arrange
            var type = value.GetType();
            var fixedRequest = new FixedValuesRequest(type, value);
            var exceptRequest = new ExceptValuesRequest(type, value);

            // Act
            var result = fixedRequest.Equals(exceptRequest);
            var text = fixedRequest.ToString();

            // Assert
            result.Should().BeFalse();
            text.Should().NotBeNull();
        }

        [AutoData]
        [Theory(DisplayName = "GIVEN ValuesRequests of different type WHEN hashcodes are compared THEN hashcodes are not equal")]
        public void GivenValuesRequestsOfDifferentType_WhenHashCodesAreCompared_ThenHashCodesAreNotEqual(
            int value)
        {
            // Arrange
            var type = value.GetType();
            var fixedRequest = new FixedValuesRequest(type, value);
            var exceptRequest = new ExceptValuesRequest(type, value);

            // Act
            var hashA = fixedRequest.GetHashCode();
            var hashB = exceptRequest.GetHashCode();
            var result = hashA == hashB;

            // Assert
            result.Should().BeFalse();
        }

        [InlineAutoData(typeof(ExceptValuesRequest))]
        [InlineAutoData(typeof(FixedValuesRequest))]
        [Theory(DisplayName = "GIVEN ValuesRequest with single value WHEN GetHashCode is invoked THEN expected value is returned")]
        public void GivenValuesRequestWithSingleValue_WhenGetHashCodeIsInvoked_ThenExpectedValueIsReturned(
            Type requestType,
            int value)
        {
            // Arrange
            var valueType = value.GetType();
            var request = Activator.CreateInstance(requestType, valueType, value);
            var expectedHashCode = valueType.GetHashCode() ^ requestType.GetHashCode() ^ value.GetHashCode();

            // Act
            var actualHashCode = request.GetHashCode();

            // Assert
            actualHashCode.Should().Be(expectedHashCode);
        }
    }
}
