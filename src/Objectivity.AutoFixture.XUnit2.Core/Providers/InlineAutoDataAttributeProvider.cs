namespace Objectivity.AutoFixture.XUnit2.Core.Providers
{
    using global::AutoFixture;

    using Objectivity.AutoFixture.XUnit2.Core.Attributes;

    using Xunit.Sdk;

    public sealed class InlineAutoDataAttributeProvider : IAutoFixtureInlineAttributeProvider
    {
        public DataAttribute GetAttribute(IFixture fixture, params object[] values)
        {
            return new AutoDataAdapterAttribute(fixture, values);
        }
    }
}
