namespace Objectivity.AutoFixture.XUnit2.AutoNSubstitute.Attributes
{
    using System;
    using global::AutoFixture;
    using global::AutoFixture.AutoNSubstitute;
    using Objectivity.AutoFixture.XUnit2.Core.Attributes;
    using Objectivity.AutoFixture.XUnit2.Core.Common;
    using Objectivity.AutoFixture.XUnit2.Core.Providers;
    using Xunit.Sdk;

    [AttributeUsage(AttributeTargets.Method, AllowMultiple = false, Inherited = true)]
    [DataDiscoverer("AutoFixture.Xunit2.NoPreDiscoveryDataDiscoverer", "AutoFixture.Xunit2")]
    public sealed class AutoMockDataAttribute : AutoDataBaseAttribute
    {
        public AutoMockDataAttribute()
            : this(new Fixture(), new AutoDataAttributeProvider())
        {
        }

        public AutoMockDataAttribute(IFixture fixture, IAutoFixtureAttributeProvider provider)
            : base(fixture, provider)
        {
        }

        protected override IFixture Customize(IFixture fixture)
        {
            return fixture.NotNull(nameof(fixture)).Customize(new AutoNSubstituteCustomization { ConfigureMembers = true });
        }
    }
}
