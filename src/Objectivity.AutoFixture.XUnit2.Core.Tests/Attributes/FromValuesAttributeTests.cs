namespace Objectivity.AutoFixture.XUnit2.Core.Tests.Attributes
{
    using FluentAssertions;

    using global::AutoFixture.Xunit2;

    using Objectivity.AutoFixture.XUnit2.Core.Attributes;

    using Xunit;

    [Collection("FromValuesAttribute")]
    [Trait("Category", "Attributes")]
    public class FromValuesAttributeTests
    {
        public enum Test
        {
            None = 0,
            One = 1,
            Two = 2,
            Three = 3,
            Four = 4,
            Five = 5,
        }

        [AutoData]
        [Theory(DisplayName = "GIVEN values specified WHEN unsigned short populated THEN the value from set is generated")]
        public void GivenValuesSpecified_WhenUShortPopulated_ThenTheValueFromSetIsGenerated(
            [FromValues(1, 5, 4)] ushort targetValue)
        {
            // Arrange
            // Act
            // Assert
            targetValue.Should().BeOneOf(1, 5, 4);
        }

        [AutoData]
        [Theory(DisplayName = "GIVEN values specified for argument WHEN unsigned short populated THEN the value from set is generated")]
        public void GivenValuesSpecifiedForAgrument_WhenEnumPopulated_ThenTheValueFromSetIsGenerated(
            [FromValues(Test.One, Test.Five, 100)] Test targetValue)
        {
            // Arrange
            // Act
            // Assert
            targetValue.Should().BeOneOf(Test.One, Test.Five);
        }

        ////[AutoData]
        ////[Theory(DisplayName = "GIVEN values specified for collection WHEN unsigned short populated THEN the value from set is generated")]
        ////public void GivenValuesSpecifiedForCollection_WhenEnumPopulated_ThenTheValueFromSetIsGenerated(
        ////    [FromValues(Test.One, Test.Five)] Test[] targetValues)
        ////{
        ////    // Arrange
        ////    var supported = new[] { Test.One, Test.Five };

        ////    // Act
        ////    // Assert
        ////    targetValues.Should().AllSatisfy(x => supported.Should().Contain(x));
        ////}
    }
}
