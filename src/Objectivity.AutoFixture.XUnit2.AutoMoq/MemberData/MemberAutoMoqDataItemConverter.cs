namespace Objectivity.AutoFixture.XUnit2.AutoMoq.MemberData
{
    using System;
    using System.Globalization;
    using System.Linq;
    using System.Reflection;
    using Common;

    internal class MemberAutoMoqDataItemConverter : IDataItemConverter
    {
        public MemberAutoMoqDataItemConverter(IDataAttributeProvider dataAttributeProvider)
        {
            this.DataAttributeProvider = dataAttributeProvider;
        }

        public IDataAttributeProvider DataAttributeProvider { get; }

        public object[] Convert(MethodInfo testMethod, object item, string memberName, Type memberType)
        {
            if (item == null)
            {
                return null;
            }

            var method = testMethod.NotNull(nameof(testMethod));
            var values = EnsureDataStructure(item, memberName, memberType ?? method.DeclaringType);
            var dataAttribute = this.DataAttributeProvider.GetAttribute(values);

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