namespace Objectivity.AutoFixture.XUnit2.Core.Attributes
{
    using System;
    using System.Collections.Generic;
    using System.Collections.ObjectModel;
    using System.Diagnostics.CodeAnalysis;
    using System.Reflection;
    using global::AutoFixture;
    using Objectivity.AutoFixture.XUnit2.Core.Common;
    using Objectivity.AutoFixture.XUnit2.Core.Customizations;
    using Objectivity.AutoFixture.XUnit2.Core.Providers;
    using Xunit.Sdk;

    [SuppressMessage("Microsoft.Design", "CA1019:DefineAccessorsForAttributeArguments", Justification = "Parameter 'values' is exposed with ReadOnlyCollection.")]
    [AttributeUsage(AttributeTargets.Method, AllowMultiple = true)]
    public abstract class InlineAutoDataBaseAttribute : DataAttribute
    {
        private readonly object[] values;
        private readonly Lazy<IReadOnlyCollection<object>> readonlyValues;

        protected InlineAutoDataBaseAttribute(IFixture fixture, IAutoFixtureInlineAttributeProvider provider, params object[] values)
        {
            this.Provider = provider.NotNull(nameof(provider));
            this.Fixture = fixture.NotNull(nameof(fixture));
            this.values = values ?? Array.Empty<object>();
            this.readonlyValues = new Lazy<IReadOnlyCollection<object>>(() => Array.AsReadOnly(this.values));
        }

        public IFixture Fixture { get; }

        public IAutoFixtureInlineAttributeProvider Provider { get; }

        /// <summary>
        /// Gets or sets a value indicating whether virtual members should be ignored during object creation.
        /// </summary>
        public bool IgnoreVirtualMembers { get; set; }

        public IReadOnlyCollection<object> Values => this.readonlyValues.Value;

        public override IEnumerable<object[]> GetData(MethodInfo testMethod)
        {
            this.Fixture.Customize(new AutoDataCommonCustomization(this.IgnoreVirtualMembers));
            this.Customize(this.Fixture);

            return this.Provider.GetAttribute(this.Fixture, this.values).GetData(testMethod);
        }

        protected abstract IFixture Customize(IFixture fixture);
    }
}
