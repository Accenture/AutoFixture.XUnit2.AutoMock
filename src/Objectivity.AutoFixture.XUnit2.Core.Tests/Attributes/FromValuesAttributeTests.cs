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
            targetValue.Should().BeOneOf(1, 5, 4);
        }

        [AutoData]
        [Theory(DisplayName = "GIVEN values specified WHEN unsigned short populated THEN the value from set is generated")]
        public void GivenValuesSpecified_WhenEnumPopulated_ThenTheValueFromSetIsGenerated(
            [FromValues(Test.One, Test.Five, 100)] Test targetValue)
        {
            targetValue.Should().BeOneOf(Test.One, Test.Five);
        }
    }
}
