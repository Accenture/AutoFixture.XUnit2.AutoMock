﻿namespace Objectivity.AutoFixture.XUnit2.AutoFakeItEasy.Attributes
{
    using System;

    using global::AutoFixture;
    using global::AutoFixture.AutoFakeItEasy;
    using Objectivity.AutoFixture.XUnit2.Core.Attributes;
    using Objectivity.AutoFixture.XUnit2.Core.Common;
    using Objectivity.AutoFixture.XUnit2.Core.Providers;

    using Xunit.Sdk;

    [AttributeUsage(AttributeTargets.Method, AllowMultiple = true, Inherited = true)]
    [DataDiscoverer("AutoFixture.Xunit2.NoPreDiscoveryDataDiscoverer", "AutoFixture.Xunit2")]
    public sealed class MemberAutoMockDataAttribute : MemberAutoDataBaseAttribute
    {
        public MemberAutoMockDataAttribute(string memberName, params object[] parameters)
            : this(new Fixture(), memberName, parameters)
        {
        }

        public MemberAutoMockDataAttribute(IFixture fixture, string memberName, params object[] parameters)
            : base(fixture, memberName, parameters)
        {
        }

        protected override IAutoFixtureInlineAttributeProvider CreateProvider()
        {
            return new InlineAutoDataAttributeProvider();
        }

        protected override IFixture Customize(IFixture fixture)
        {
            return fixture.NotNull(nameof(fixture)).Customize(new AutoFakeItEasyCustomization { ConfigureMembers = true });
        }
    }
}
