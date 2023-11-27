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

    [Collection("RequestFactoryRelay")]
    [Trait("Category", "SpecimenBuilders")]
    public class RequestFactoryRelayTests
    {
        [Fact(DisplayName = "GIVEN uninitialized argument WHEN constructor is invoked THEN exception is thrown")]
        public void GivenUninitializedArgument_WhenConstructorIsInvoked_ThenExceptionIsThrown()
        {
            // Arrange
            // Act
            // Assert
            Assert.Throws<ArgumentNullException>(() => new RequestFactoryRelay(null));
        }

        [Fact(DisplayName = "GIVEN empty argument WHEN Create is invoked THEN exception is thrown")]
        public void GivenEmptyArgument_WhenCreateIsInvoked_ThenExceptionIsThrown()
        {
            // Arrange
            var factory = new Mock<Func<Type, object>>();
            var builder = new RequestFactoryRelay(factory.Object);

            // Act
            // Assert
            Assert.Throws<ArgumentNullException>(() => builder.Create(new object(), null));
            factory.VerifyNoOtherCalls();
        }

        [Fact(DisplayName = "GIVEN unsupported request type WHEN create is invoked THEN NoSpecimen is returned")]
        public void GivenUnsupportedRequestType_WhenCreateIsInvoked_ThenNoSpecimenIsReturned()
        {
            // Arrange
            var factory = new Mock<Func<Type, object>>();
            var builder = new RequestFactoryRelay(factory.Object);
            var context = new Mock<ISpecimenContext>();
            var request = new object();

            // Act
            var result = builder.Create(request, context.Object);

            // Assert
            result.Should().BeOfType<NoSpecimen>();
            factory.VerifyNoOtherCalls();
        }

        [AutoData]
        [Theory(DisplayName = "GIVEN valid ParameterInfo request WHEN create is invoked THEN proper value is returned")]
        public void GivenValidParameterInfoRequest_WhenCreateIsInvoked_ThenProperValueIsReturned(
            int value)
        {
            // Arrange
            var factory = new Mock<Func<Type, object>>();
            var builder = new RequestFactoryRelay(factory.Object);
            var context = new Mock<ISpecimenContext>();
            context.Setup(x => x.Resolve(It.IsAny<object>())).Returns(value);
            var request = this.GetType()
                .GetMethod(nameof(this.GivenValidParameterInfoRequest_WhenCreateIsInvoked_ThenProperValueIsReturned))
                .GetParameters()
                .First();

            // Act
            var result = builder.Create(request, context.Object);

            // Assert
            result.Should().NotBeNull().And.Subject.As<int>().Should().Be(value);
            factory.Verify(x => x(It.IsAny<Type>()), Times.Once);
            factory.VerifyNoOtherCalls();
            context.Verify(x => x.Resolve(It.IsAny<object>()), Times.Once);
            context.VerifyNoOtherCalls();
        }

        [AutoData]
        [Theory(DisplayName = "GIVEN valid type request WHEN create is invoked THEN proper value is returned")]
        public void GivenValidTypeRequest_WhenCreateIsInvoked_ThenProperValueIsReturned(
            int value)
        {
            // Arrange
            var factory = new Mock<Func<Type, object>>();
            var builder = new RequestFactoryRelay(factory.Object);
            var context = new Mock<ISpecimenContext>();
            context.Setup(x => x.Resolve(It.IsAny<object>())).Returns(value);
            var request = value.GetType();

            // Act
            var result = builder.Create(request, context.Object);

            // Assert
            result.Should().NotBeNull().And.Subject.As<int>().Should().Be(value);
            factory.Verify(x => x(It.IsAny<Type>()), Times.Once);
            factory.VerifyNoOtherCalls();
            context.Verify(x => x.Resolve(It.IsAny<object>()), Times.Once);
            context.VerifyNoOtherCalls();
        }

        [AutoData]
        [Theory(DisplayName = "GIVEN valid enumerable type request WHEN create is invoked resulting in unsupported type THEN NoSpecimen is returned")]
        public void GivenValidEnumerableTypeRequest_WhenCreateIsInvokedResultingInUnsupportedType_ThenNoSpecimenIsReturned(
            IFixture fixture)
        {
            // Arrange
            var values = fixture.CreateMany<int>(1).ToArray();
            var value = values.First();
            var factory = new Mock<Func<Type, object>>();
            factory.Setup(x => x(It.IsAny<Type>())).Returns(value);
            var builder = new RequestFactoryRelay(factory.Object);
            var context = new Mock<ISpecimenContext>();
            context.Setup(x => x.Resolve(It.IsAny<object>())).Returns(value);
            var request = values.GetType();

            // Act
            var result = builder.Create(request, context.Object);

            // Assert
            result.Should().BeOfType<NoSpecimen>();
            factory.Verify(x => x(It.IsAny<Type>()), Times.Once);
            factory.VerifyNoOtherCalls();
            context.Verify(x => x.Resolve(It.IsAny<object>()), Times.Once);
            context.VerifyNoOtherCalls();
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
            var factory = new Mock<Func<Type, object>>();
            factory.Setup(x => x(It.IsAny<Type>())).Returns(values);
            var builder = new RequestFactoryRelay(factory.Object);
            var context = new Mock<ISpecimenContext>();
            context.Setup(x => x.Resolve(It.IsAny<object>())).Returns(values);

            // Act
            var result = builder.Create(requestType, context.Object);

            // Assert
            result.Should().NotBeNull()
                .And.BeOfType(expectedType)
                .And.Subject.As<IEnumerable>().Cast<int>().Should().HaveCount(values.Length);
            factory.Verify(x => x(It.IsAny<Type>()), Times.Once);
            factory.VerifyNoOtherCalls();
            context.Verify(x => x.Resolve(It.IsAny<object>()), Times.Once);
            context.VerifyNoOtherCalls();
        }
    }
}
