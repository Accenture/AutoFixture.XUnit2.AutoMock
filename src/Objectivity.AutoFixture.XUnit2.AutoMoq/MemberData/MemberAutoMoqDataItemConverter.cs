namespace Objectivity.AutoFixture.XUnit2.AutoMoq.MemberData
{
    using System;
    using System.Globalization;
    using System.Linq;
    using System.Reflection;
    using Common;
    using Ploeh.AutoFixture;
    using Providers;

    internal class MemberAutoMoqDataItemConverter : IDataItemConverter
    {
        public MemberAutoMoqDataItemConverter(IFixture fixture, IAutoFixtureInlineAttributeProvider dataAttributeProvider)
        {
            this.Fixture = fixture.NotNull(nameof(fixture));
            this.DataAttributeProvider = dataAttributeProvider.NotNull(nameof(dataAttributeProvider));
        }

        public IFixture Fixture { get; }

        public IAutoFixtureInlineAttributeProvider DataAttributeProvider { get; }

        public object[] Convert(MethodInfo testMethod, object item, string memberName, Type memberType)
        {
            if (item == null)
            {
                return null;
            }

            var method = testMethod.NotNull(nameof(testMethod));
            var values = EnsureDataStructure(item, memberName, memberType ?? method.DeclaringType);
            var dataAttribute = this.DataAttributeProvider.GetAttribute(this.Fixture, values);

            return dataAttribute.GetData(method).FirstOrDefault();
        }

        private static object[] EnsureDataStructure(object item, string memberName, Type memgerType)
        {
            var objArray = item as object[];
            if (objArray != null)
            {
                return objArray;
            }

            var message = string.Format(
                CultureInfo.InvariantCulture,
                "Property {0} on {1} yielded an item that is not an object[]",
                memberName,
                memgerType);

            throw new ArgumentException(message);
        }
    }
}