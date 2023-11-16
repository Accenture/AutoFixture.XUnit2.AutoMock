namespace Objectivity.AutoFixture.XUnit2.Core.Tests.SpecimenBuilders
{
    using System;

    using global::AutoFixture.Xunit2;

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

        [AutoData]
        [Theory(DisplayName = "GIVEN empty argument WHEN Create is invoked THEN exception is thrown")]
        internal void GivenEmptyArgument_WhenCreateIsInvoked_ThenExceptionIsThrown(RandomFixedValuesParameterBuilder builder)
        {
            // Arrange
            // Act
            // Assert
            Assert.Throws<ArgumentNullException>(() => builder.Create(new object(), null));
        }
    }
}
