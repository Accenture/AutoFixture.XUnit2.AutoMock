namespace Objectivity.AutoFixture.XUnit2.AutoMoq.Attributes
{
    using System;
    using System.Collections.Generic;
    using System.Collections.ObjectModel;
    using System.Reflection;
    using Common;
    using Customizations;
    using Ploeh.AutoFixture;
    using Providers;
    using Xunit.Sdk;

    [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1019:DefineAccessorsForAttributeArguments", Justification = "Parameter 'values' is exposed with ReadOnlyCollection.")]
    [AttributeUsage(AttributeTargets.Method, AllowMultiple = true)]
    public sealed class InlineAutoMoqDataAttribute : DataAttribute
    {
        private readonly object[] values;

        public InlineAutoMoqDataAttribute(params object[] values)
            : this(new Fixture(), new InlineAutoDataAttributeProvider(), values)
        {
        }

        public InlineAutoMoqDataAttribute(IFixture fixture, IAutoFixtureInlineAttributeProvider provider, params object[] values)
        {
            this.Provider = provider.NotNull(nameof(provider));
            this.Fixture = fixture.NotNull(nameof(fixture));
            this.values = values ?? new object[0];
        }

        public IFixture Fixture { get; }

        public IAutoFixtureInlineAttributeProvider Provider { get; }

        public IReadOnlyCollection<object> Values => new ReadOnlyCollection<object>(this.values);

        public override IEnumerable<object[]> GetData(MethodInfo testMethod)
        {
            this.Fixture.Customize(new AutoMoqDataCustomization());

            return this.Provider.GetAttribute(this.Fixture, this.values).GetData(testMethod);
        }
    }
}