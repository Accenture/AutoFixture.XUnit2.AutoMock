namespace Objectivity.AutoFixture.XUnit2.Core.Tests.Common
{
    using System;
    using System.Collections;
    using System.Collections.Generic;

    using FluentAssertions;
    using global::AutoFixture.Xunit2;
    using Objectivity.AutoFixture.XUnit2.Core.Common;

    using Xunit;

    [Collection("EnumerableExtensions")]
    [Trait("Category", "Common")]
    public class EnumerableExtensionsTests
    {
        [Fact(DisplayName = "GIVEN uninitialized argument WHEN TryGetEnumerableSingleTypeArgument is invoked THEN exception is thrown")]
        public void GivenUninitializedArgument_WhenTryGetEnumerableSingleTypeArgumentIsInvoked_ThenExceptionIsThrown()
        {
            // Arrange
            Type enumerableType = null;

            // Act
            // Assert
            Assert.Throws<ArgumentNullException>(() => enumerableType.TryGetEnumerableSingleTypeArgument(out var itemType));
        }

        [Theory(DisplayName = "GIVEN valid collection WHEN TryGetEnumerableSingleTypeArgument is invoked THEN collection single type argument returned")]
        [InlineData(typeof(int[]), typeof(int))]
        [InlineData(typeof(List<int>), typeof(int))]
        [InlineData(typeof(Dictionary<int, int>), typeof(KeyValuePair<int, int>))]
        [InlineData(typeof(IEnumerable<int>), typeof(int))]
        public void GivenValidCollection_WhenTryGetEnumerableSingleTypeArgumentIsInvoked_ThenCollectionSingleTypeArgumentReturned(Type enumerableType, Type expectedType)
        {
            // Arrange
            // Act
            var isSuccessful = enumerableType.TryGetEnumerableSingleTypeArgument(out var itemType);

            // Assert
            isSuccessful.Should().BeTrue();
            itemType.Should().Be(expectedType);
        }

        [Theory(DisplayName = "GIVEN invalid collection WHEN TryGetEnumerableSingleTypeArgument is invoked THEN no argument returned")]
        [InlineData(typeof(Tuple<int, int>))]
        [InlineData(typeof(IEnumerable))]
        public void GivenInvalidCollection_WhenTryGetEnumerableSingleTypeArgumentIsInvoked_ThenNoArgumentReturned(Type enumerableType)
        {
            // Arrange
            // Act
            var isSuccessful = enumerableType.TryGetEnumerableSingleTypeArgument(out _);

            // Assert
            isSuccessful.Should().BeFalse();
        }

        [Fact(DisplayName = "GIVEN generic definition collection WHEN TryGetEnumerableSingleTypeArgument is invoked THEN no argument returned")]
        public void GivenGenericDefinitionCollection_WhenTryGetEnumerableSingleTypeArgumentIsInvoked_ThenNoArgumentReturned()
        {
            // Arrange
            var enumerableType = typeof(IEnumerable<int>).GetGenericTypeDefinition();

            // Act
            var isSuccessful = enumerableType.TryGetEnumerableSingleTypeArgument(out _);

            // Assert
            isSuccessful.Should().BeFalse();
        }

        [Theory(DisplayName = "GIVEN uninitialized argument WHEN ToTypedArray is invoked THEN exception is thrown")]
        [InlineData(null, typeof(int))]
        [InlineData(new[] { 1 }, null)]
        public void GivenUninitializedArgument_WhenToTypedArrayIsInvoked_ThenExceptionIsThrown(IEnumerable items, Type itemType)
        {
            // Arrange
            // Act
            // Assert
            Assert.Throws<ArgumentNullException>(() => items.ToTypedArray(itemType));
        }

        [Theory(DisplayName = "GIVEN typed enumerable WHEN ToTypedArray is invoked THEN array is returned")]
        [AutoData]
        public void GivenTypedEnumerable_WhenToTypedArrayIsInvoked_ThenArrayIsReturned(int[] items)
        {
            // Arrange
            var itemType = typeof(int);

            // Act
            var array = items.ToTypedArray(itemType);

            // Assert
            array.Should().BeEquivalentTo(items).And.Subject.GetType().IsArray.Should().BeTrue();
        }

        [Theory(DisplayName = "GIVEN enumerable WHEN ToTypedArray is invoked THEN array is returned")]
        [AutoData]
        public void GivenEnumerable_WhenToTypedArrayIsInvoked_ThenArrayIsReturned(BitArray items)
        {
            // Arrange
            var itemType = typeof(bool);

            // Act
            var array = items.ToTypedArray(itemType);

            // Assert
            array.Should().BeEquivalentTo(items);
        }
    }
}
