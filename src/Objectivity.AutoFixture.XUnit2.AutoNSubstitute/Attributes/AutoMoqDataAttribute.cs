namespace Objectivity.AutoFixture.XUnit2.AutoNSubstitute.Attributes
{
    using System;
    using Objectivity.AutoFixture.XUnit2.AutoNSubstitute.Providers;
    using Objectivity.AutoFixture.XUnit2.Core.Attributes;
    using Objectivity.AutoFixture.XUnit2.Core.Common;
    using Objectivity.AutoFixture.XUnit2.Core.Providers;
    using Ploeh.AutoFixture;
    using Ploeh.AutoFixture.AutoNSubstitute;

    [AttributeUsage(AttributeTargets.Method, AllowMultiple = false, Inherited = true)]
    public sealed class AutoMoqDataAttribute : AutoMoqDataBaseAttribute
    {
        public AutoMoqDataAttribute()
            : this(new Fixture(), new AutoDataAttributeProvider())
        {
        }

        public AutoMoqDataAttribute(IFixture fixture, IAutoFixtureAttributeProvider provider)
            : base(fixture, provider)
        {
        }

        public override IFixture Customize(IFixture fixture)
        {
            return fixture.NotNull(nameof(fixture)).Customize(new AutoConfiguredNSubstituteCustomization());
        }
    }
}
