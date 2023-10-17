namespace Objectivity.AutoFixture.XUnit2.Core.Tests.Attributes
{
    using FluentAssertions;
    using global::AutoFixture.Xunit2;

    using Objectivity.AutoFixture.XUnit2.Core.Attributes;

    using Xunit;

    [Collection("FromRangeAttribute")]
    [Trait("Category", "Attributes")]
    public class FromRangeAttributeTests
    {
        [AutoData]
        [Theory(DisplayName = "GIVEN renges specified WHEN data populated THEN only decorated parameters has values from ranges")]
        public void GivenRengesSpecified_WhenDataPopulated_ThenOnlyDecoratedParametersHasValuesFromRanges(
            [FromRange(-10, -1)] int value1,
            [FromRange(-39.9, -30.1)] double value2,
            [FromRange(-2.9, -2.1)] decimal value3,
            int value4,
            double value5,
            decimal value6)
        {
            value1.Should().BeInRange(-10, -1);
            value2.Should().BeInRange(-39.9, -30.1);
            value3.Should().BeInRange(-2.9m, -2.1m);
            value4.Should().BeGreaterThanOrEqualTo(0);
            value5.Should().BeGreaterThanOrEqualTo(0);
            value6.Should().BeGreaterThanOrEqualTo(0);
        }
    }
}
