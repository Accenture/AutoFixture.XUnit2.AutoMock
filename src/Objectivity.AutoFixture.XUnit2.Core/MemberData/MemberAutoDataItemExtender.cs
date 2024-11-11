namespace Objectivity.AutoFixture.XUnit2.Core.MemberData
{
    using System.Diagnostics.CodeAnalysis;
    using System.Linq;
    using System.Reflection;

    using global::AutoFixture;

    using Objectivity.AutoFixture.XUnit2.Core.Common;
    using Objectivity.AutoFixture.XUnit2.Core.Providers;

    internal class MemberAutoDataItemExtender : IDataItemExtender
    {
        public MemberAutoDataItemExtender(IFixture fixture, IAutoFixtureInlineAttributeProvider dataAttributeProvider)
        {
            this.Fixture = fixture.NotNull(nameof(fixture));
            this.DataAttributeProvider = dataAttributeProvider.NotNull(nameof(dataAttributeProvider));
        }

        [SuppressMessage("ReSharper", "MemberCanBePrivate.Global", Justification = "Testable design.")]
        public IFixture Fixture { get; }

        [SuppressMessage("ReSharper", "MemberCanBePrivate.Global", Justification = "Testable design.")]
        public IAutoFixtureInlineAttributeProvider DataAttributeProvider { get; }

        public object[] Extend(MethodInfo testMethod, object[] values)
        {
            if (values is null)
            {
                return null;
            }

            var method = testMethod.NotNull(nameof(testMethod));
            var dataAttribute = this.DataAttributeProvider.GetAttribute(this.Fixture, values);

            return dataAttribute.GetData(method).FirstOrDefault();
        }
    }
}
