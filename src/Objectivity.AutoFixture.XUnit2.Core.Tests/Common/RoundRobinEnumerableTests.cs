namespace Objectivity.AutoFixture.XUnit2.Core.Tests.Common
{
    using System;
    using System.Collections;
    using System.Linq;

    using FluentAssertions;

    using global::AutoFixture.Xunit2;

    using Objectivity.AutoFixture.XUnit2.Core.Common;

    using Xunit;

    public class RoundRobinEnumerableTests
    {
        [Fact(DisplayName = "GIVEN uninitialized argument WHEN constructor is invoked THEN exception is thrown")]
        public void GivenUninitializedArgument_WhenConstructorIsInvoked_ThenExceptionIsThrown()
        {
            // Arrange
            // Act
            // Assert
            Assert.Throws<ArgumentNullException>(() => new RoundRobinEnumerable<object>(null));
        }

        [Fact(DisplayName = "GIVEN no arguments WHEN constructor is invoked THEN exception is thrown")]
        public void GivenNoArguments_WhenConstructorIsInvoked_ThenExceptionIsThrown()
        {
            // Arrange
            // Act
            // Assert
            Assert.Throws<ArgumentException>(() => new RoundRobinEnumerable<object>());
        }

        [AutoData]
        [Theory(DisplayName = "GIVEN round robin with values WHEN enumerating twice THEN oredered values enumerated twice")]
        public void GivenRoundRobinWithValues_WhenEnumeratingTwice_ThenOrederedValuesEnumeratedTwice(
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
