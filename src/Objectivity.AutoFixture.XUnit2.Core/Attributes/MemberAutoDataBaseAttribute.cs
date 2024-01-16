namespace Objectivity.AutoFixture.XUnit2.Core.Attributes
{
    using System;
    using System.Collections.Generic;
    using System.Reflection;

    using global::AutoFixture;

    using Objectivity.AutoFixture.XUnit2.Core.Common;
    using Objectivity.AutoFixture.XUnit2.Core.Customizations;
    using Objectivity.AutoFixture.XUnit2.Core.MemberData;
    using Objectivity.AutoFixture.XUnit2.Core.Providers;

    using Xunit;
    using Xunit.Sdk;

    [AttributeUsage(AttributeTargets.Method, AllowMultiple = true, Inherited = true)]
    [DataDiscoverer("Xunit.Sdk.MemberDataDiscoverer", "xunit.core")]
    public abstract class MemberAutoDataBaseAttribute : MemberDataAttributeBase
    {
        protected MemberAutoDataBaseAttribute(IFixture fixture, string memberName, params object[] parameters)
            : base(memberName.NotNull(nameof(memberName)), parameters)
        {
            this.Fixture = fixture.NotNull(nameof(fixture));
        }

        public IFixture Fixture { get; }

        /// <summary>
        /// Gets or sets a value indicating whether virtual members should be ignored during object creation.
        /// </summary>
        /// <value>Indicates whether virtual members should be ignored during object creation.</value>
        public bool IgnoreVirtualMembers { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether to share a fixture across all data items; true by default.
        /// </summary>
        /// <value>Indicates whether to share a fixture across all data items; true by default.</value>
        public bool ShareFixture { get; set; } = true;

        public override IEnumerable<object[]> GetData(MethodInfo testMethod)
        {
            // Customize shared fixture
            this.CustomizeFixture(this.Fixture);

            return base.GetData(testMethod);
        }

        protected override object[] ConvertDataItem(MethodInfo testMethod, object item)
        {
            var fixture = this.ShareFixture
                ? this.Fixture
                : this.CustomizeFixture(new Fixture());

            var converter = new MemberAutoDataItemConverter(fixture, this.CreateProvider());

            return converter.Convert(testMethod, item, this.MemberName, this.MemberType);
        }

        protected abstract IFixture Customize(IFixture fixture);

        protected abstract IAutoFixtureInlineAttributeProvider CreateProvider();

        private IFixture CustomizeFixture(IFixture fixture)
        {
            fixture.Customize(new AutoDataCommonCustomization(this.IgnoreVirtualMembers));
            return this.Customize(fixture);
        }
    }
}
