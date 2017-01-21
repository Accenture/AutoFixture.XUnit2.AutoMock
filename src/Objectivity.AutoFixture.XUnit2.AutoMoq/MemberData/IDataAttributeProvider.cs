namespace Objectivity.AutoFixture.XUnit2.AutoMoq.MemberData
{
    using Xunit.Sdk;

    public interface IDataAttributeProvider
    {
        DataAttribute GetAttribute(params object[] values);
    }
}