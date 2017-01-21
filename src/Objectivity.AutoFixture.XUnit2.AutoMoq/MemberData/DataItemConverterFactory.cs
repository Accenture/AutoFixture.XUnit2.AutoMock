namespace Objectivity.AutoFixture.XUnit2.AutoMoq.MemberData
{
    using Attributes;
    using Common;
    using Ploeh.AutoFixture;

    internal static class DataItemConverterFactory
    {
        public static IDataItemConverter Create(bool shareFixture, bool ignoreVirtualMembers, IFixture fixture)
        {
            var autoMoqDataAttribute = shareFixture ? new AutoMoqDataAttribute(fixture.NotNull(nameof(fixture)), ignoreVirtualMembers) : new AutoMoqDataAttribute(ignoreVirtualMembers);
            return new MemberAutoMoqDataItemConverter(new InlineAutoMoqDataAttributeProvider(autoMoqDataAttribute));
        }
    }
}