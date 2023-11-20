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

    [Collection("RandomValuesParameterBuilder")]
    [Trait("Category", "SpecimenBuilders")]
    public class RandomFixedValuesParameterBuilderTests
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

        [Fact(DisplayName = "GIVEN uninitialized context WHEN Create is invoked THEN exception is thrown")]
        public void GivenUninitializedContext_WhenCreateIsInvoked_ThenExceptionIsThrown()
        {
            // Arrange
            var builder = new RandomFixedValuesParameterBuilder(int.MinValue, int.MaxValue);

            // Act
            // Assert
            Assert.Throws<ArgumentNullException>(() => builder.Create(new object(), null));
        }

        [Fact(DisplayName = "GIVEN uninitialized request WHEN Create is invoked THEN exception is thrown")]
        public void GivenUninitializedRequest_WhenCreateIsInvoked_ThenExceptionIsThrown()
        {
            // Arrange
            var builder = new RandomFixedValuesParameterBuilder(int.MinValue, int.MaxValue);
            var context = new Mock<ISpecimenContext>();

            // Act
            // Assert
            Assert.Throws<ArgumentNullException>(() => builder.Create(null, context.Object));
        }

        [Fact(DisplayName = "GIVEN unsupported request type WHEN create is invoked THEN NoSpecimen is returned")]
        public void GivenUnsupportedRequestType_WhenCreateIsInvoked_ThenNoSpecimenIsReturned()
        {
            // Arrange
            var builder = new RandomFixedValuesParameterBuilder(1);
            var context = new Mock<ISpecimenContext>();
            var request = new object();

            // Act
            var result = builder.Create(request, context.Object);

            // Assert
            result.Should().BeOfType<NoSpecimen>();
        }

        [InlineAutoData(10)]
        [Theory(DisplayName = "GIVEN valid ParameterInfo request WHEN create is invoked THEN defined value is returned")]
        public void GivenValidParameterInfoRequest_WhenCreateIsInvoked_ThenValueFromRangeIsReturned(
            int value)
        {
            // Arrange
            var builder = new RandomFixedValuesParameterBuilder(value);
            var context = new Mock<ISpecimenContext>();
            context.Setup(x => x.Resolve(It.IsAny<object>())).Returns(value);
            var request = this.GetType()
                .GetMethod(nameof(this.GivenValidParameterInfoRequest_WhenCreateIsInvoked_ThenValueFromRangeIsReturned))
                .GetParameters()
                .First();

            // Act
            var result = builder.Create(request, context.Object);

            // Assert
            result.Should().NotBeNull().And.Subject.As<int>().Should().Be(value);
        }

        [AutoData]
        [Theory(DisplayName = "GIVEN valid type request WHEN create is invoked THEN defined value is returned")]
        public void GivenValidTypeRequest_WhenCreateIsInvoked_ThenValueFromRangeIsReturned(
            int value)
        {
            // Arrange
            var builder = new RandomFixedValuesParameterBuilder(value);
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
            var builder = new RandomFixedValuesParameterBuilder(values.Cast<object>().ToArray());
            var context = new Mock<ISpecimenContext>();
            var request = values.GetType();
            context.Setup(x => x.Resolve(It.IsAny<object>())).Returns(values.First());

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
            var builder = new RandomFixedValuesParameterBuilder(values.Cast<object>().ToArray());
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
