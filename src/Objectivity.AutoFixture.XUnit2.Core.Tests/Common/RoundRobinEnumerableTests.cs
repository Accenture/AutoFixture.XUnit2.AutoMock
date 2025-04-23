namespace Objectivity.AutoFixture.XUnit2.Core.Tests.Common
{
    using System;
    using System.Collections;
    using System.Linq;

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
            static object Act() => new RoundRobinEnumerable<object>(null);

            // Assert
            var exception = Assert.Throws<ArgumentNullException>(Act);
            Assert.Equal("values", exception.ParamName);
        }

        [Fact(DisplayName = "GIVEN no arguments WHEN constructor is invoked THEN exception is thrown")]
        public void GivenNoArguments_WhenConstructorIsInvoked_ThenExceptionIsThrown()
        {
            // Arrange
            // Act
            static object Act() => new RoundRobinEnumerable<object>();

            // Assert
            var exception = Assert.Throws<ArgumentException>(Act);
            Assert.NotNull(exception.Message);
            Assert.NotEmpty(exception.Message);
            Assert.Contains("At least one value", exception.Message);
        }

        [AutoData]
        [Theory(DisplayName = "GIVEN round robin with values WHEN Current is invoked after construction THEN exception is thrown")]
        public void GivenRoundRobinWithValues_WhenCurrentIsInvokedAfterConstruction_ThenExceptionIsThrown(
            int[] values)
        {
            // Arrange
            IEnumerator enumerator = new RoundRobinEnumerable<int>(values);

            // Act
            object Act() => enumerator.Current;

            // Assert
            var exception = Assert.Throws<InvalidOperationException>(Act);
            Assert.NotNull(exception.Message);
            Assert.NotEmpty(exception.Message);
            Assert.Contains("initial position", exception.Message);
        }

        [AutoData]
        [Theory(DisplayName = "GIVEN round robin with values WHEN Current is invoked after reset THEN exception is thrown")]
        public void GivenRoundRobinWithValues_WhenCurrentIsInvokedAfterReset_ThenExceptionIsThrown(
            int[] values)
        {
            // Arrange
            IEnumerator enumerator = new RoundRobinEnumerable<int>(values);

            // Act
            enumerator.MoveNext();
            enumerator.Reset();
            object Act() => enumerator.Current;

            // Assert
            var exception = Assert.Throws<InvalidOperationException>(Act);
            Assert.NotNull(exception.Message);
            Assert.NotEmpty(exception.Message);
            Assert.Contains("initial position", exception.Message);
        }

        [AutoData]
        [Theory(DisplayName = "GIVEN round robin with values WHEN enumerating twice THEN ordered values enumerated twice")]
        public void GivenRoundRobinWithValues_WhenEnumeratingTwice_ThenOrderedValuesEnumeratedTwice(
            int[] values)
        {
            // Arrange
            var enumerator = new RoundRobinEnumerable<int>(values);
            var duplicatedValues = values.Concat(values.ToArray()).ToArray();

            // Act
            var items = duplicatedValues.Select(x =>
            {
                var canMove = enumerator.MoveNext();
                return canMove && enumerator.Current == x;
            }).ToArray();

            // Assert
            Assert.All(items, Assert.True);
            Assert.Equal(duplicatedValues.Length, items.Length);
        }
    }
}
