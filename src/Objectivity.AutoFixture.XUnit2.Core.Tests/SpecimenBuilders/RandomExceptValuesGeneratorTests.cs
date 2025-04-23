namespace Objectivity.AutoFixture.XUnit2.Core.Tests.SpecimenBuilders
{
    using System;

    using global::AutoFixture;
    using global::AutoFixture.Kernel;

    using Moq;

    using Objectivity.AutoFixture.XUnit2.Core.Requests;
    using Objectivity.AutoFixture.XUnit2.Core.SpecimenBuilders;

    using Xunit;

    [Collection("RandomExceptValuesGenerator")]
    [Trait("Category", "SpecimenBuilders")]
    public class RandomExceptValuesGeneratorTests
    {
        [Fact(DisplayName = "GIVEN uninitialized context WHEN Create is invoked THEN exception is thrown")]
        public void GivenUninitializedContext_WhenCreateIsInvoked_ThenExceptionIsThrown()
        {
            // Arrange
            var builder = new RandomExceptValuesGenerator();

            // Act
            object Act() => builder.Create(new object(), null);

            // Assert
            var exception = Assert.Throws<ArgumentNullException>(Act);
            Assert.Equal("context", exception.ParamName);
        }

        [Fact(DisplayName = "GIVEN uninitialized request WHEN Create is invoked THEN exception is thrown")]
        public void GivenUninitializedRequest_WhenCreateIsInvoked_ThenExceptionIsThrown()
        {
            // Arrange
            var builder = new RandomExceptValuesGenerator();
            var context = new Mock<ISpecimenContext>();

            // Act
            object Act() => builder.Create(null, context.Object);

            // Assert
            var exception = Assert.Throws<ArgumentNullException>(Act);
            Assert.Equal("request", exception.ParamName);
        }

        [Fact(DisplayName = "GIVEN excluded value and context which resolves to the same value WHEN Create is invoked THEN exception is thrown")]
        public void GivenExcludedValueAndContextWhichResolvesToTheSameValue_WhenCreateIsInvoked_ThenExceptionIsThrown()
        {
            // Arrange
            var builder = new RandomExceptValuesGenerator();
            var context = new Mock<ISpecimenContext>();
            const int duplicateValue = 5;
            context.Setup(x => x.Resolve(It.IsNotNull<object>())).Returns(duplicateValue);
            var request = new ExceptValuesRequest(duplicateValue.GetType(), duplicateValue);

            // Act
            object Act() => builder.Create(request, context.Object);

            // Assert
            var exception = Assert.Throws<ObjectCreationException>(Act);
            Assert.NotNull(exception.Message);
            Assert.NotEmpty(exception.Message);
        }

        [Fact(DisplayName = "GIVEN unsupported request WHEN Create is invoked THEN NoSpecimen is returned")]
        public void GivenUnsupportedRequest_WhenCreateIsInvoked_ThenNoSpecimenIsReturned()
        {
            // Arrange
            var builder = new RandomExceptValuesGenerator();
            var context = new Mock<ISpecimenContext>();
            var request = typeof(string);

            // Act
            var result = builder.Create(request, context.Object);

            // Assert
            Assert.NotNull(result);
            Assert.IsType<NoSpecimen>(result);
        }
    }
}
