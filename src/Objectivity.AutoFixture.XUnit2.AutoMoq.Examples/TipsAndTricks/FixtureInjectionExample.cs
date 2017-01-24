namespace Objectivity.AutoFixture.XUnit2.AutoMoq.Examples.TipsAndTricks
{
    using AutoMoq.Attributes;
    using FluentAssertions;
    using Ploeh.AutoFixture;
    using Xunit;

    public class FixtureInjectionExample
    {
        [Theory]
        [AutoMoqData]
        public void FixtureInjection(IFixture fixture)
        {
            fixture.Should().NotBeNull();
        }
    }
}
