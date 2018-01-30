namespace Objectivity.AutoFixture.XUnit2.Core.Providers
{
    using global::AutoFixture;
    using Xunit.Sdk;

    public interface IAutoFixtureInlineAttributeProvider
    {
        DataAttribute GetAttribute(IFixture fixture, params object[] values);
    }
}