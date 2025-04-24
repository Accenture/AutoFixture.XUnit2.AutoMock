namespace Objectivity.AutoFixture.XUnit2.Core.Tests.SpecimenBuilders
{
    using System;
    using System.Linq;

    using global::AutoFixture;
    using global::AutoFixture.Kernel;
    using global::AutoFixture.Xunit2;
    using Moq;

    using Objectivity.AutoFixture.XUnit2.Core.Requests;
    using Objectivity.AutoFixture.XUnit2.Core.SpecimenBuilders;

    using Xunit;

    [Collection("RandomFixedValuesGenerator")]
    [Trait("Category", "SpecimenBuilders")]
    public class RandomFixedValuesGeneratorTests
    {
        [Fact(DisplayName = "GIVEN uninitialized context WHEN Create is invoked THEN exception is thrown")]
        public void GivenUninitializedContext_WhenCreateIsInvoked_ThenExceptionIsThrown()
        {
            // Arrange
            var builder = new RandomFixedValuesGenerator();

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
            var builder = new RandomFixedValuesGenerator();
            var context = new Mock<ISpecimenContext>();

            // Act
            object Act() => builder.Create(null, context.Object);

            // Assert
            var exception = Assert.Throws<ArgumentNullException>(Act);
            Assert.Equal("request", exception.ParamName);
        }

        [Fact(DisplayName = "GIVEN unsupported request WHEN Create is invoked THEN NoSpecimen is returned")]
        public void GivenUnsupportedRequest_WhenCreateIsInvoked_ThenNoSpecimenIsReturned()
        {
            // Arrange
            var builder = new RandomFixedValuesGenerator();
            var context = new Mock<ISpecimenContext>();
            var request = typeof(string);

            // Act
            var result = builder.Create(request, context.Object);

            // Assert
            Assert.NotNull(result);
            Assert.IsType<NoSpecimen>(result);
        }

        [Fact(DisplayName = "GIVEN request with many values WHEN Create is invoked many times THEN only defined values are returned")]
        public void GivenRequestWithManyValues_WhenCreateIsInvokedManyTimes_ThenOnlyDefinedValuesAreReturned()
        {
            // Arrange
            var builder = new RandomFixedValuesGenerator();
            var context = new Mock<ISpecimenContext>();
            var expectedValues = new object[] { 1, 2 };
            var request = new FixedValuesRequest(typeof(int), expectedValues);

            // Act
            var result = Enumerable.Range(0, 10).Select((_) => (int)builder.Create(request, context.Object)).ToList();

            // Assert
            Assert.NotNull(result);
            Assert.Equal(10, result.Count);
            Assert.All(result, x => Assert.Contains(x, expectedValues));
        }

        [AutoData]
        [Theory(DisplayName = "GIVEN request with single value WHEN Create is invoked many times THEN only repeated defined value is returned")]
        internal void GivenRequestWithSingleValue_WhenCreateIsInvokedManyTimes_ThenOnlyRepeatedDefinedValueIsReturned(
            RandomFixedValuesGenerator builder,
            IFixture fixture)
        {
            // Arrange
            var context = new Mock<ISpecimenContext>();
            var expectedValue = fixture.Create<int>();
            var request = new FixedValuesRequest(typeof(int), expectedValue);

            // Act
            var result = Enumerable.Range(0, 10).Select((_) => (int)builder.Create(request, context.Object)).ToList();

            // Assert
            Assert.NotNull(result);
            Assert.Equal(10, result.Count);
            Assert.All(result, x => Assert.Equal(expectedValue, x));
        }

        [AutoData]
        [Theory(DisplayName = "GIVEN many requests WHEN Create is invoked per each request THEN different defined value are returned")]
        internal void GivenManyRequests_WhenCreateIsInvokedPerEachRequest_ThenDifferentDefinedValueAreReturned(
            RandomFixedValuesGenerator builder)
        {
            // Arrange
            var context = new Mock<ISpecimenContext>();
            var setup = new[] { 1, 2 }.ToDictionary(
                x => x,
                x => new FixedValuesRequest(typeof(int), x));

            // Act
            var result = setup.Select(x => (int)builder.Create(x.Value, context.Object)).ToArray();

            // Assert
            Assert.Equal(setup.First().Key, result.First());
            Assert.Equal(setup.Last().Key, result.Last());
        }
    }
}
