namespace Objectivity.AutoFixture.XUnit2.Core.MemberData
{
    using System;
    using System.Globalization;
    using System.Linq;
    using System.Reflection;
    using global::AutoFixture;
    using Objectivity.AutoFixture.XUnit2.Core.Common;
    using Objectivity.AutoFixture.XUnit2.Core.Providers;

    internal class MemberAutoDataItemConverter : IDataItemConverter
    {
        public MemberAutoDataItemConverter(IFixture fixture, IAutoFixtureInlineAttributeProvider dataAttributeProvider)
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

        private static object[] EnsureDataStructure(object item, string memberName, Type memberType)
        {
            if (item is object[] objArray)
            {
                return objArray;
            }

            var message = string.Format(
                CultureInfo.InvariantCulture,
                "Property {0} on {1} yielded an item that is not an object[]",
                memberName,
                memberType);

            throw new ArgumentException(message);
        }
    }
}