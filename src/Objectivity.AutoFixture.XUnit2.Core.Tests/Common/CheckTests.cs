namespace Objectivity.AutoFixture.XUnit2.Core.Tests.Common
{
    using System;
    using System.Diagnostics.CodeAnalysis;

    using FluentAssertions;

    using Objectivity.AutoFixture.XUnit2.Core.Common;

    using Xunit;

    [Collection("Check")]
    [Trait("Category", "Common")]
    [SuppressMessage("ReSharper", "NotResolvedInText", Justification = "Used for testing only.")]
    public class CheckTests
    {
        [Fact(DisplayName = "GIVEN uninitialized object WHEN NotNull is invoked THEN exception is thrown")]
        public void GivenUninitializedObject_WhenNotNullIsInvoked_ThenExceptionIsThrown()
        {
            // Arrange
            const object value = null;

            // Act
            // Assert
            Assert.Throws<ArgumentNullException>(() => value.NotNull("value"));
        }

        [Fact(DisplayName = "GIVEN initialized object WHEN NotNull is invoked THEN the same object is returned")]
        public void GivenInitializedObject_WhenNotNullIsInvoked_ThenTheSameObjectIsReturned()
        {
            // Arrange
            var value = new object();

            // Act
            var result = value.NotNull("value");

            // Assert
            result.Should().Be(value);
        }
    }
}
