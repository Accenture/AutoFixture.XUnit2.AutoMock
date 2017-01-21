namespace Objectivity.AutoFixture.XUnit2.AutoMoq.MemberData
{
    using Attributes;
    using Xunit.Sdk;

    internal class InlineAutoMoqDataAttributeProvider : IDataAttributeProvider
    {
        public InlineAutoMoqDataAttributeProvider()
            : this(null)
        {
        }

        public InlineAutoMoqDataAttributeProvider(AutoMoqDataAttribute autoMoqDataAttribute)
        {
            this.AutoMoqDataAttribute = autoMoqDataAttribute;
        }

        public AutoMoqDataAttribute AutoMoqDataAttribute { get; }

        public DataAttribute GetAttribute(params object[] values)
        {
            return this.AutoMoqDataAttribute == null
                ? new InlineAutoMoqDataAttribute(values)
                : new InlineAutoMoqDataAttribute(this.AutoMoqDataAttribute, values);
        }
    }
}