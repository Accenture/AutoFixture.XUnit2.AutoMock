namespace Objectivity.AutoFixture.XUnit2.AutoNSubstitute.Providers
{
    using Objectivity.AutoFixture.XUnit2.Core.Providers;
    using Ploeh.AutoFixture;
    using Ploeh.AutoFixture.Xunit2;
    using Xunit.Sdk;

    internal class AutoDataAttributeProvider : IAutoFixtureAttributeProvider
    {
        public DataAttribute GetAttribute(IFixture fixture)
        {
            return new AutoDataAttribute(fixture);
        }
    }
}