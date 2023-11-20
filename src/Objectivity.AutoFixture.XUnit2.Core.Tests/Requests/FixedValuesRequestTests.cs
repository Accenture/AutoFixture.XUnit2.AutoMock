namespace Objectivity.AutoFixture.XUnit2.Core.Tests.Requests
{
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.Diagnostics.CodeAnalysis;
    using System.Globalization;
    using System.Linq;

    using FluentAssertions;

    using Objectivity.AutoFixture.XUnit2.Core.Requests;

    using Xunit;

    public class FixedValuesRequestTests
    {
        public static IEnumerable<object[]> ComparisonTestData { get; } = new[]
        {
            new object[] { typeof(int), new[] { 1 }, typeof(int), new[] { 1 }, true },
            new object[] { typeof(int), new[] { 1, 2, 3 }, typeof(int), new[] { 3, 2, 1 }, true },
            new object[] { typeof(int), new[] { 1 }, typeof(long), new[] { 1 }, false },
            new object[] { typeof(int), new[] { 1 }, typeof(int), new[] { 2 }, false },
            new object[] { typeof(int), new[] { 1, 2 }, typeof(int), new[] { 2 }, false },
        };

        [Fact(DisplayName = "GIVEN uninitialized type argument WHEN constructor is invoked THEN exception is thrown")]
        public void GivenUninitializedTypeArgument_WhenConstructorIsInvoked_ThenExceptionIsThrown()
        {
            // Arrange
            Type type = null;
            object[] values = null;

            // Act
            // Assert
            Assert.Throws<ArgumentNullException>(() => new FixedValuesRequest(type, values));
        }

        [Fact(DisplayName = "GIVEN uninitialized vaues argument WHEN constructor is invoked THEN exception is thrown")]
        public void GivenUninitializedValuesArgument_WhenConstructorIsInvoked_ThenExceptionIsThrown()
        {
            // Arrange
            var type = typeof(int);
            object[] values = null;

            // Act
            // Assert
            Assert.Throws<ArgumentNullException>(() => new FixedValuesRequest(type, values));
        }

        [Fact(DisplayName = "GIVEN empty argument WHEN constructor is invoked THEN exception is thrown")]
        public void GivenEmptyArgument_WhenConstructorIsInvoked_ThenExceptionIsThrown()
        {
            // Arrange
            var type = typeof(int);
            var values = Array.Empty<object>();

            // Act
            // Assert
            Assert.Throws<ArgumentException>(() => new FixedValuesRequest(type, values));
        }

        [InlineData(typeof(int), 2)]
        [InlineData(1, typeof(int))]
        [Theory(DisplayName = "GIVEN uncomparable argument WHEN constructor is invoked THEN exception is thrown")]
        public void GivenUncomparableArgument_WhenConstructorIsInvoked_ThenExceptionIsThrown(object first, object second)
        {
            // Arrange
            var type = typeof(int);

            // Act
            // Assert
            Assert.Throws<ArgumentException>(() => new FixedValuesRequest(type, first, second));
        }

        [Theory(DisplayName = "GIVEN two requests WHEN Equals is invoked THEN expected value is returned")]
        [MemberData(nameof(ComparisonTestData))]
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

        [Theory(DisplayName = "GIVEN two requests WHEN GetHashCode is invoked THEN expected value is returned")]
        [MemberData(nameof(ComparisonTestData))]
        public void GivenTwoRequests_WhenGetHashCodeIsInvoked_ThenExpectedValueIsReturned(
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
        [SuppressMessage("Maintainability", "CA1508:Avoid dead conditional code", Justification = "Test the logic")]
        public void GivenUninitializedRequest_WhenEqualsIsInvoked_ThenFalseIsReturned()
        {
            // Arrange
            var initialized = new FixedValuesRequest(typeof(int), 1);
            FixedValuesRequest uninitialized = null;

            // Act
            var result = initialized.Equals(uninitialized);

            // Assert
            result.Should().Be(false);
        }

        [Fact(DisplayName = "GIVEN different type of object WHEN Equals is invoked THEN False is returned")]
        public void GivenDifferentTypeOfObject_WhenEqualsIsInvoked_ThenFalseIsReturned()
        {
            // Arrange
            var request = new FixedValuesRequest(typeof(int), 1);
            var differentObject = typeof(int);

            // Act
            var result = request.Equals(differentObject);

            // Assert
            result.Should().Be(false);
        }
    }
}
