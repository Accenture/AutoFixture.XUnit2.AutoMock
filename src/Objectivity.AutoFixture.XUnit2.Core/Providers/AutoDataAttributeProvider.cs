namespace Objectivity.AutoFixture.XUnit2.Core.Providers
{
    using global::AutoFixture;
    using Objectivity.AutoFixture.XUnit2.Core.Attributes;
    using Xunit.Sdk;

    public sealed class AutoDataAttributeProvider : IAutoFixtureAttributeProvider
    {
        public DataAttribute GetAttribute(IFixture fixture)
        {
            return new AutoDataAdapterAttribute(fixture);
        }
    }
}