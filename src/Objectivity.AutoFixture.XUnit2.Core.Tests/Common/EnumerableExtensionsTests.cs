namespace Objectivity.AutoFixture.XUnit2.Core.Tests.Common
{
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.Diagnostics.CodeAnalysis;
    using global::AutoFixture.Xunit2;
    using JetBrains.Annotations;
    using Objectivity.AutoFixture.XUnit2.Core.Common;

    using Xunit;

    [Collection("EnumerableExtensions")]
    [Trait("Category", "Common")]
    public class EnumerableExtensionsTests
    {
        [Fact(DisplayName = "GIVEN uninitialized argument WHEN TryGetEnumerableSingleTypeArgument is invoked THEN exception is thrown")]
        [SuppressMessage("ReSharper", "UnusedVariable", Justification = "This is good enougth to test the logic.")]
        public void GivenUninitializedArgument_WhenTryGetEnumerableSingleTypeArgumentIsInvoked_ThenExceptionIsThrown()
        {
            // Arrange
            Type enumerableType = null;

            // Act
            object Act() => enumerableType.TryGetEnumerableSingleTypeArgument(out _);

            // Assert
            var exception = Assert.Throws<ArgumentNullException>(Act);
            Assert.Equal("type", exception.ParamName);
        }

        [InlineData(typeof(int[]), typeof(int))]
        [InlineData(typeof(List<int>), typeof(int))]
        [InlineData(typeof(Dictionary<int, int>), typeof(KeyValuePair<int, int>))]
        [InlineData(typeof(IEnumerable<int>), typeof(int))]
        [Theory(DisplayName = "GIVEN valid collection WHEN TryGetEnumerableSingleTypeArgument is invoked THEN collection single type argument returned")]
        public void GivenValidCollection_WhenTryGetEnumerableSingleTypeArgumentIsInvoked_ThenCollectionSingleTypeArgumentReturned(Type enumerableType, Type expectedType)
        {
            // Arrange
            // Act
            var isSuccessful = enumerableType.TryGetEnumerableSingleTypeArgument(out var itemType);

            // Assert
            Assert.True(isSuccessful);
            Assert.Equal(expectedType, itemType);
        }

        [InlineData(typeof(Tuple<int, int>))]
        [InlineData(typeof(IEnumerable))]
        [Theory(DisplayName = "GIVEN invalid collection WHEN TryGetEnumerableSingleTypeArgument is invoked THEN no argument returned")]
        public void GivenInvalidCollection_WhenTryGetEnumerableSingleTypeArgumentIsInvoked_ThenNoArgumentReturned(Type enumerableType)
        {
            // Arrange
            // Act
            var isSuccessful = enumerableType.TryGetEnumerableSingleTypeArgument(out _);

            // Assert
            Assert.False(isSuccessful);
        }

        [Fact(DisplayName = "GIVEN generic definition collection WHEN TryGetEnumerableSingleTypeArgument is invoked THEN no argument returned")]
        public void GivenGenericDefinitionCollection_WhenTryGetEnumerableSingleTypeArgumentIsInvoked_ThenNoArgumentReturned()
        {
            // Arrange
            var enumerableType = typeof(IEnumerable<int>).GetGenericTypeDefinition();

            // Act
            var isSuccessful = enumerableType.TryGetEnumerableSingleTypeArgument(out _);

            // Assert
            Assert.False(isSuccessful);
        }

        [InlineData(null, typeof(int), "items")]
        [InlineData(new[] { 1 }, null, "itemType")]
        [Theory(DisplayName = "GIVEN uninitialized argument WHEN ToTypedArray is invoked THEN exception is thrown")]
        public void GivenUninitializedArgument_WhenToTypedArrayIsInvoked_ThenExceptionIsThrown(
            [NoEnumeration] IEnumerable items,
            Type itemType,
            string exceptionParamName)
        {
            // Arrange
            // Act
            object Act() => items.ToTypedArray(itemType);

            // Assert
            var exception = Assert.Throws<ArgumentNullException>(Act);
            Assert.Equal(exceptionParamName, exception.ParamName);
        }

        [AutoData]
        [Theory(DisplayName = "GIVEN typed enumerable WHEN ToTypedArray is invoked THEN array is returned")]
        public void GivenTypedEnumerable_WhenToTypedArrayIsInvoked_ThenArrayIsReturned(int[] items)
        {
            // Arrange
            var itemType = typeof(int);

            // Act
            var array = items.ToTypedArray(itemType);

            // Assert
            Assert.Equal(items, array);
            Assert.True(array.GetType().IsArray);
        }

        [AutoData]
        [Theory(DisplayName = "GIVEN enumerable WHEN ToTypedArray is invoked THEN array is returned")]
        public void GivenEnumerable_WhenToTypedArrayIsInvoked_ThenArrayIsReturned(BitArray items)
        {
            // Arrange
            var itemType = typeof(bool);

            // Act
            var array = items.ToTypedArray(itemType);

            // Assert
            Assert.Equal(items, array);
        }
    }
}
