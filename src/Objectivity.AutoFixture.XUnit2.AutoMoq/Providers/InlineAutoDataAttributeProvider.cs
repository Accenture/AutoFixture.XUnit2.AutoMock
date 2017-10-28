namespace Objectivity.AutoFixture.XUnit2.AutoMoq.Providers
{
    using Objectivity.AutoFixture.XUnit2.Core.Providers;
    using Ploeh.AutoFixture;
    using Ploeh.AutoFixture.Xunit2;
    using Xunit;
    using Xunit.Sdk;

    internal class InlineAutoDataAttributeProvider : IAutoFixtureInlineAttributeProvider
    {
        public DataAttribute GetAttribute(IFixture fixture, params object[] values)
        {
            return new CompositeDataAttribute(new InlineDataAttribute(values), new AutoDataAttribute(fixture));
        }
    }
}