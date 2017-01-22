namespace Objectivity.AutoFixture.XUnit2.AutoMoq.Providers
{
    using Ploeh.AutoFixture;
    using Xunit.Sdk;

    public interface IAutoFixtureAttributeProvider
    {
        DataAttribute GetAttribute(IFixture fixture);
    }
}