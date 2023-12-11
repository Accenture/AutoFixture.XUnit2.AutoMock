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
            const string expectedParameterName = "value";

            // Act
            Func<object> act = () => value.NotNull(expectedParameterName);

            // Assert
            act.Should().Throw<ArgumentNullException>()
                .And.ParamName.Should().Be(expectedParameterName);
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
