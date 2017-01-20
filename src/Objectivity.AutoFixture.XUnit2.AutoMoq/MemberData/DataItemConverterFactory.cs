namespace Objectivity.AutoFixture.XUnit2.AutoMoq.MemberData
{
    using Attributes;
    using Common;
    using Ploeh.AutoFixture;

    internal static class DataItemConverterFactory
    {
        public static IDataItemConverter Create(bool shareFixture, IFixture fixture)
        {
            var autoMoqDataAttribute = shareFixture ? new AutoMoqDataAttribute(fixture.NotNull(nameof(fixture))) : new AutoMoqDataAttribute();
            return new MemberAutoMoqDataItemConverter(new InlineAutoMoqDataAttributeProvider(autoMoqDataAttribute));
        }
    }
}