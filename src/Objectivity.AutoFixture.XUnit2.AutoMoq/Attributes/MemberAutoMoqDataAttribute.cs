namespace Objectivity.AutoFixture.XUnit2.AutoMoq.Attributes
{
    using System;
    using System.Collections.Generic;
    using System.Reflection;
    using Common;
    using Customizations;
    using MemberData;
    using Ploeh.AutoFixture;
    using Providers;
    using Xunit;
    using Xunit.Sdk;

    [AttributeUsage(AttributeTargets.Method, AllowMultiple = true, Inherited = true)]
    [DataDiscoverer("Xunit.Sdk.MemberDataDiscoverer", "xunit.core")]
    public sealed class MemberAutoMoqDataAttribute : MemberDataAttributeBase
    {
        public MemberAutoMoqDataAttribute(string memberName, params object[] parameters)
            : this(new Fixture(), memberName.NotNull(nameof(memberName)), parameters)
        {
        }

        public MemberAutoMoqDataAttribute(IFixture fixture, string memberName, params object[] parameters)
            : base(memberName.NotNull(nameof(memberName)), parameters)
        {
            this.Fixture = fixture.NotNull(nameof(fixture));
        }

        public IFixture Fixture { get; }

        /// <summary>
        /// Gets or sets a value indicating whether virtual members should be ignored during object creation.
        /// </summary>
        public bool IgnoreVirtualMembers { get; set; } = false;

        /// <summary>
        /// Gets or sets a value indicating whether to share a fixture across all data items; true by default.
        /// </summary>
        public bool ShareFixture { get; set; } = true;

        public override IEnumerable<object[]> GetData(MethodInfo testMethod)
        {
            // Customize shared fixture
            this.Fixture.Customize(new AutoMoqDataCustomization(this.IgnoreVirtualMembers));

            return base.GetData(testMethod);
        }

        protected override object[] ConvertDataItem(MethodInfo testMethod, object item)
        {
            var fixture = this.ShareFixture
                ? this.Fixture
                : new Fixture().Customize(new AutoMoqDataCustomization(this.IgnoreVirtualMembers));

            var converter = new MemberAutoMoqDataItemConverter(fixture, new InlineAutoDataAttributeProvider());

            return converter.Convert(testMethod, item, this.MemberName, this.MemberType);
        }
    }
}