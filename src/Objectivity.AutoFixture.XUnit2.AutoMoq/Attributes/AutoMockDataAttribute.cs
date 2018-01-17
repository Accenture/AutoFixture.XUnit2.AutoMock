namespace Objectivity.AutoFixture.XUnit2.AutoMoq.Attributes
{
    using System;
    using Objectivity.AutoFixture.XUnit2.Core.Attributes;
    using Objectivity.AutoFixture.XUnit2.Core.Common;
    using Objectivity.AutoFixture.XUnit2.Core.Providers;
    using Ploeh.AutoFixture;
    using Ploeh.AutoFixture.AutoMoq;

    [AttributeUsage(AttributeTargets.Method, AllowMultiple = false, Inherited = true)]
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
            return fixture.NotNull(nameof(fixture)).Customize(new AutoConfiguredMoqCustomization());
        }
    }
}