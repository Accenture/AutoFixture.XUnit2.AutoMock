namespace Objectivity.AutoFixture.XUnit2.Core.Providers
{
    using Ploeh.AutoFixture;
    using Ploeh.AutoFixture.Xunit2;
    using Xunit;
    using Xunit.Sdk;

    public sealed class InlineAutoDataAttributeProvider : IAutoFixtureInlineAttributeProvider
    {
        public DataAttribute GetAttribute(IFixture fixture, params object[] values)
        {
            return new CompositeDataAttribute(new InlineDataAttribute(values), new AutoDataAttribute(fixture));
        }
    }
}