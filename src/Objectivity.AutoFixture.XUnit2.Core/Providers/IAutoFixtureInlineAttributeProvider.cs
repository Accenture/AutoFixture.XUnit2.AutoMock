namespace Objectivity.AutoFixture.XUnit2.Core.Providers
{
    using Ploeh.AutoFixture;
    using Xunit.Sdk;

    public interface IAutoFixtureInlineAttributeProvider
    {
        DataAttribute GetAttribute(IFixture fixture, params object[] values);
    }
}