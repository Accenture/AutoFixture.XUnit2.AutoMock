namespace Objectivity.AutoFixture.XUnit2.Core.Tests.Common
{
    using System;
    using System.Collections;
    using System.Linq;

    using FluentAssertions;

    using global::AutoFixture.Xunit2;

    using Objectivity.AutoFixture.XUnit2.Core.Common;

    using Xunit;

    [Collection("RoundRobinEnumerable")]
    [Trait("Category", "Common")]
    public class RoundRobinEnumerableTests
    {
        [Fact(DisplayName = "GIVEN uninitialized argument WHEN constructor is invoked THEN exception is thrown")]
        public void GivenUninitializedArgument_WhenConstructorIsInvoked_ThenExceptionIsThrown()
        {
            // Arrange
            // Act
            Func<object> act = () => new RoundRobinEnumerable<object>(null);

            // Assert
            act.Should().Throw<ArgumentNullException>()
                .And.ParamName.Should().Be("values");
        }

        [Fact(DisplayName = "GIVEN no arguments WHEN constructor is invoked THEN exception is thrown")]
        public void GivenNoArguments_WhenConstructorIsInvoked_ThenExceptionIsThrown()
        {
            // Arrange
            // Act
            Func<object> act = () => new RoundRobinEnumerable<object>();

            // Assert
            act.Should().Throw<ArgumentException>()
                .And.Message.Should().NotBeNullOrEmpty()
                .And.Contain("At least one value");
        }

        [AutoData]
        [Theory(DisplayName = "GIVEN round robin with values WHEN enumerating twice THEN ordered values enumerated twice")]
        public void GivenRoundRobinWithValues_WhenEnumeratingTwice_ThenOrderedValuesEnumeratedTwice(
            int[] values)
        {
            // Arrange
            var enumerator = ((IEnumerable)new RoundRobinEnumerable<int>(values)).GetEnumerator();
            var duplicatedValues = values.Concat(values.ToArray()).ToArray();

            // Act
            var items = duplicatedValues.Select(x =>
            {
                enumerator.MoveNext();
                return x.Equals(enumerator.Current);
            }).ToArray();

            // Assert
            items.Should().AllSatisfy(x => x.Should().BeTrue()).And.HaveCount(duplicatedValues.Length);
        }
    }
}
