namespace Objectivity.AutoFixture.XUnit2.Core.Tests.SpecimenBuilders
{
    using System;

    using Objectivity.AutoFixture.XUnit2.Core.SpecimenBuilders;

    using Xunit;

    [Collection("RandomValuesParameterBuilder")]
    [Trait("Category", "SpecimenBuilders")]
    public class RandomValuesParameterBuilderTests
    {
        [Fact(DisplayName = "GIVEN uninitialized argument WHEN constructor is invoked THEN exception is thrown")]
        public void GivenUninitializedArgument_WhenConstructorIsInvoked_ThenExceptionIsThrown()
        {
            // Arrange
            // Act
            // Assert
            Assert.Throws<ArgumentNullException>(() => new RandomFixedValuesParameterBuilder(null));
        }

        [Fact(DisplayName = "GIVEN empty argument WHEN constructor is invoked THEN exception is thrown")]
        public void GivenEmptyArgument_WhenConstructorIsInvoked_ThenExceptionIsThrown()
        {
            // Arrange
            // Act
            // Assert
            Assert.Throws<ArgumentException>(() => new RandomFixedValuesParameterBuilder(Array.Empty<object>()));
        }

        [InlineData(typeof(int), 2)]
        [InlineData(1, typeof(int))]
        [Theory(DisplayName = "GIVEN uncomparable argument WHEN constructor is invoked THEN exception is thrown")]
        public void GivenUncomparableArgument_WhenConstructorIsInvoked_ThenExceptionIsThrown(object first, object second)
        {
            // Arrange
            // Act
            // Assert
            Assert.Throws<ArgumentException>(() => new RandomFixedValuesParameterBuilder(first, second));
        }

        [Fact(DisplayName = "GIVEN empty argument WHEN Create is invoked THEN exception is thrown")]
        public void GivenEmptyArgument_WhenCreateIsInvoked_ThenExceptionIsThrown()
        {
            // Arrange
            var builder = new RandomFixedValuesParameterBuilder(int.MinValue, int.MaxValue);

            // Act
            // Assert
            Assert.Throws<ArgumentNullException>(() => builder.Create(new object(), null));
        }
    }
}
