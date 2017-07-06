namespace Objectivity.AutoFixture.XUnit2.AutoMoq.Attributes
{
    using System;
    using System.Collections.Generic;
    using System.Reflection;
    using Common;
    using Customizations;
    using Ploeh.AutoFixture;
    using Providers;
    using Xunit.Sdk;

    [AttributeUsage(AttributeTargets.Method, AllowMultiple = false, Inherited = true)]
    public sealed class AutoMoqDataAttribute : DataAttribute
    {
        public AutoMoqDataAttribute()
            : this(new Fixture(), new AutoDataAttributeProvider())
        {
        }

        public AutoMoqDataAttribute(IFixture fixture, IAutoFixtureAttributeProvider provider)
        {
            this.Fixture = fixture.NotNull(nameof(fixture));
            this.Provider = provider.NotNull(nameof(provider));
        }

        public IFixture Fixture { get; }

        /// <summary>
        /// Gets or sets a value indicating whether virtual members should be ignored during object creation.
        /// </summary>
        public bool IgnoreVirtualMembers { get; set; } = false;

        public IAutoFixtureAttributeProvider Provider { get; }

        public override IEnumerable<object[]> GetData(MethodInfo testMethod)
        {
            this.Fixture.Customize(new AutoMoqDataCustomization(this.IgnoreVirtualMembers));
            return this.Provider.GetAttribute(this.Fixture).GetData(testMethod);
        }
    }
}