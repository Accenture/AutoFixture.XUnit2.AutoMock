namespace Objectivity.AutoFixture.XUnit2.Core.Tests.SpecimenBuilders
{
    using System;

    using FluentAssertions;

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
            Func<object> act = () => builder.Create(new object(), null);

            // Assert
            act.Should().Throw<ArgumentNullException>()
                .And.ParamName.Should().Be("context");
        }

        [Fact(DisplayName = "GIVEN uninitialized request WHEN Create is invoked THEN exception is thrown")]
        public void GivenUninitializedRequest_WhenCreateIsInvoked_ThenExceptionIsThrown()
        {
            // Arrange
            var builder = new RandomExceptValuesGenerator();
            var context = new Mock<ISpecimenContext>();

            // Act
            Func<object> act = () => builder.Create(null, context.Object);

            // Assert
            act.Should().Throw<ArgumentNullException>()
                .And.ParamName.Should().Be("request");
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
            Func<object> act = () => builder.Create(request, context.Object);

            // Assert
            act.Should().Throw<ObjectCreationException>()
                .And.Message.Should().NotBeNullOrEmpty();
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
            result.Should().NotBeNull().And.BeOfType<NoSpecimen>();
        }
    }
}
