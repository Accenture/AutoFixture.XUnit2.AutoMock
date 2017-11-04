namespace Objectivity.AutoFixture.XUnit2.Core.Attributes
{
    using System;
    using System.Collections.Generic;
    using System.Collections.ObjectModel;
    using System.Reflection;
    using Objectivity.AutoFixture.XUnit2.Core.Common;
    using Objectivity.AutoFixture.XUnit2.Core.Customizations;
    using Objectivity.AutoFixture.XUnit2.Core.Providers;
    using Ploeh.AutoFixture;
    using Xunit.Sdk;

    [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1019:DefineAccessorsForAttributeArguments", Justification = "Parameter 'values' is exposed with ReadOnlyCollection.")]
    [AttributeUsage(AttributeTargets.Method, AllowMultiple = true)]
    public abstract class InlineAutoMoqDataBaseAttribute : DataAttribute
    {
        private readonly object[] values;

        protected InlineAutoMoqDataBaseAttribute(IFixture fixture, IAutoFixtureInlineAttributeProvider provider, params object[] values)
        {
            this.Provider = provider.NotNull(nameof(provider));
            this.Fixture = fixture.NotNull(nameof(fixture));
            this.values = values ?? new object[0];
        }

        public IFixture Fixture { get; }

        public IAutoFixtureInlineAttributeProvider Provider { get; }

        /// <summary>
        /// Gets or sets a value indicating whether virtual members should be ignored during object creation.
        /// </summary>
        public bool IgnoreVirtualMembers { get; set; } = false;

        public IReadOnlyCollection<object> Values => new ReadOnlyCollection<object>(this.values);

        public override IEnumerable<object[]> GetData(MethodInfo testMethod)
        {
            this.Fixture.Customize(new AutoDataCommonCustomization(this.IgnoreVirtualMembers));
            this.Customize(this.Fixture);

            return this.Provider.GetAttribute(this.Fixture, this.values).GetData(testMethod);
        }

        protected abstract IFixture Customize(IFixture fixture);
    }
}
