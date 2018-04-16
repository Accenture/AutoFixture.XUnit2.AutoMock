namespace Objectivity.AutoFixture.XUnit2.AutoNSubstitute.Attributes
{
    using System;
    using global::AutoFixture;
    using global::AutoFixture.AutoNSubstitute;
    using Objectivity.AutoFixture.XUnit2.Core.Attributes;
    using Objectivity.AutoFixture.XUnit2.Core.Common;
    using Objectivity.AutoFixture.XUnit2.Core.Providers;

    [AttributeUsage(AttributeTargets.Method, AllowMultiple = true)]
    public sealed class InlineAutoMockDataAttribute : InlineAutoDataBaseAttribute
    {
        public InlineAutoMockDataAttribute(params object[] values)
            : this(new Fixture(), new InlineAutoDataAttributeProvider(), values)
        {
        }

        public InlineAutoMockDataAttribute(IFixture fixture, IAutoFixtureInlineAttributeProvider provider, params object[] values)
            : base(fixture, provider, values)
        {
        }

        protected override IFixture Customize(IFixture fixture)
        {
            return fixture.NotNull(nameof(fixture)).Customize(new AutoNSubstituteCustomization { ConfigureMembers = true });
        }
    }
}