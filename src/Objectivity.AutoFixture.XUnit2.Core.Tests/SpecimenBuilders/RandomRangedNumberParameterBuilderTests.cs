namespace Objectivity.AutoFixture.XUnit2.Core.Tests.SpecimenBuilders
{
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.Collections.ObjectModel;
    using System.Linq;

    using FluentAssertions;

    using global::AutoFixture;
    using global::AutoFixture.Kernel;
    using global::AutoFixture.Xunit2;
    using Moq;

    using Objectivity.AutoFixture.XUnit2.Core.SpecimenBuilders;

    using Xunit;

    [Collection("RandomRangedNumberBuilder")]
    [Trait("Category", "SpecimenBuilders")]
    public class RandomRangedNumberParameterBuilderTests
    {
        [InlineData(null, 2)]
        [InlineData(1, null)]
        [Theory(DisplayName = "GIVEN uninitialized argument WHEN constructor is invoked THEN exception is thrown")]
        public void GivenUninitializedArgument_WhenConstructorIsInvoked_ThenExceptionIsThrown(object minimum, object maximum)
        {
            // Arrange
            // Act
            // Assert
            Assert.Throws<ArgumentNullException>(() => new RandomRangedNumberParameterBuilder(minimum, maximum));
        }

        [InlineData(typeof(int), 2)]
        [InlineData(1, typeof(int))]
        [Theory(DisplayName = "GIVEN uncomparable argument WHEN constructor is invoked THEN exception is thrown")]
        public void GivenUncomparableArgument_WhenConstructorIsInvoked_ThenExceptionIsThrown(object minimum, object maximum)
        {
            // Arrange
            // Act
            // Assert
            Assert.Throws<ArgumentException>(() => new RandomRangedNumberParameterBuilder(minimum, maximum));
        }

        [Fact(DisplayName = "GIVEN minimum greater than maximum WHEN constructor is invoked THEN exception is thrown")]
        public void GivenMinimumGreaterThanMaximum_WhenConstructorIsInvoked_ThenExceptionIsThrown()
        {
            // Arrange
            const int min = 100;
            const int max = 1;

            // Act
            // Assert
            Assert.Throws<ArgumentOutOfRangeException>(() => new RandomRangedNumberParameterBuilder(min, max));
        }

        [Fact(DisplayName = "GIVEN empty argument WHEN Create is invoked THEN exception is thrown")]
        public void GivenEmptyArgument_WhenCreateIsInvoked_ThenExceptionIsThrown()
        {
            // Arrange
            var builder = new RandomRangedNumberParameterBuilder(int.MinValue, int.MaxValue);

            // Act
            // Assert
            Assert.Throws<ArgumentNullException>(() => builder.Create(new object(), null));
        }

        [Fact(DisplayName = "GIVEN unsupported request type WHEN create is invoked THEN NoSpecimen is returned")]
        public void GivenUnsupportedRequestType_WhenCreateIsInvoked_ThenNoSpecimenIsReturned()
        {
            // Arrange
            const int min = 1;
            const int max = 100;
            var builder = new RandomRangedNumberParameterBuilder(min, max);
            var context = new Mock<ISpecimenContext>();
            var request = new object();

            // Act
            var result = builder.Create(request, context.Object);

            // Assert
            result.Should().BeOfType<NoSpecimen>();
        }

        [InlineAutoData(10, 10)]
        [InlineAutoData(1, 100)]
        [Theory(DisplayName = "GIVEN valid ParameterInfo request WHEN create is invoked THEN value from range is returned")]
        public void GivenValidParameterInfoRequest_WhenCreateIsInvoked_ThenValueFromRangeIsReturned(
            int min,
            int max,
            IFixture fixture)
        {
            // Arrange
            var builder = new RandomRangedNumberParameterBuilder(min, max);
            var context = new SpecimenContext(fixture);
            var request = this.GetType()
                .GetMethod(nameof(this.GivenValidParameterInfoRequest_WhenCreateIsInvoked_ThenValueFromRangeIsReturned))
                .GetParameters()
                .First();

            // Act
            var result = builder.Create(request, context);

            // Assert
            result.Should().NotBeNull().And.Subject.As<int>().Should().BeInRange(min, max);
        }

        [AutoData]
        [Theory(DisplayName = "GIVEN valid type request WHEN create is invoked THEN defined value is returned")]
        public void GivenValidTypeRequest_WhenCreateIsInvoked_ThenValueFromRangeIsReturned(
            int value)
        {
            // Arrange
            var builder = new RandomRangedNumberParameterBuilder(value, value);
            var context = new Mock<ISpecimenContext>();
            context.Setup(x => x.Resolve(It.IsAny<object>())).Returns(value);
            var request = value.GetType();

            // Act
            var result = builder.Create(request, context.Object);

            // Assert
            result.Should().NotBeNull().And.Subject.As<int>().Should().Be(value);
        }

        [AutoData]
        [Theory(DisplayName = "GIVEN valid enumerable type request WHEN create is invoked resulting in unsupported type THEN NoSpecimen is returned")]
        public void GivenValidEnumerableTypeRequest_WhenCreateIsInvokedResultingInUnsupportedType_ThenNoSpecimenIsReturned(
            IFixture fixture)
        {
            // Arrange
            var values = fixture.CreateMany<int>(1).ToArray();
            var value = values.First();
            var builder = new RandomRangedNumberParameterBuilder(value, value);
            var context = new Mock<ISpecimenContext>();
            var request = values.GetType();
            context.Setup(x => x.Resolve(It.IsAny<object>())).Returns(value);

            // Act
            var result = builder.Create(request, context.Object);

            // Assert
            result.Should().BeOfType<NoSpecimen>();
        }

        [InlineAutoData(typeof(int[]), typeof(int[]))]
        [InlineAutoData(typeof(IEnumerable<int>), typeof(int[]))]
        [InlineAutoData(typeof(Collection<int>), typeof(Collection<int>))]
        [Theory(DisplayName = "GIVEN valid enumerable type request WHEN create is invoked THEN expected collection is returned")]
        public void GivenValidEnumerableTypeRequest_WhenCreateIsInvoked_ThenExpectedCollectionIsReturned(
            Type requestType,
            Type expectedType,
            IFixture fixture)
        {
            // Arrange
            var values = fixture.CreateMany<int>(1).ToArray();
            var value = values.First();
            var builder = new RandomRangedNumberParameterBuilder(value, value);
            var context = new Mock<ISpecimenContext>();
            context.Setup(x => x.Resolve(It.IsAny<object>())).Returns(values);

            // Act
            var result = builder.Create(requestType, context.Object);

            // Assert
            result.Should().NotBeNull()
                .And.BeOfType(expectedType)
                .And.Subject.As<IEnumerable>().Cast<int>().Should().HaveCount(values.Length);
        }
    }
}
