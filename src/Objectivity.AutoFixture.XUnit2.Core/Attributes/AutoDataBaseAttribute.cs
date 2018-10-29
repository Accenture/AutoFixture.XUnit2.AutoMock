namespace Objectivity.AutoFixture.XUnit2.Core.Attributes
{
    using System;
    using System.Collections.Generic;
    using System.Reflection;
    using global::AutoFixture;
    using Objectivity.AutoFixture.XUnit2.Core.Common;
    using Objectivity.AutoFixture.XUnit2.Core.Customizations;
    using Objectivity.AutoFixture.XUnit2.Core.Providers;
    using Xunit.Sdk;

    [AttributeUsage(AttributeTargets.Method, AllowMultiple = false, Inherited = true)]
    public abstract class AutoDataBaseAttribute : DataAttribute
    {
        protected AutoDataBaseAttribute(IFixture fixture, IAutoFixtureAttributeProvider provider)
        {
            this.Fixture = fixture.NotNull(nameof(fixture));
            this.Provider = provider.NotNull(nameof(provider));
        }

        public IFixture Fixture { get; }

        /// <summary>
        /// Gets or sets a value indicating whether virtual members should be ignored during object creation.
        /// </summary>
        public bool IgnoreVirtualMembers { get; set; }

        public IAutoFixtureAttributeProvider Provider { get; }

        public override IEnumerable<object[]> GetData(MethodInfo testMethod)
        {
            this.Fixture.Customize(new AutoDataCommonCustomization(this.IgnoreVirtualMembers));
            this.Customize(this.Fixture);

            return this.Provider.GetAttribute(this.Fixture).GetData(testMethod);
        }

        protected abstract IFixture Customize(IFixture fixture);
    }
}
