﻿namespace Objectivity.AutoFixture.XUnit2.Core.Providers
{
    using global::AutoFixture;

    using Xunit.Sdk;

    public interface IAutoFixtureAttributeProvider
    {
        DataAttribute GetAttribute(IFixture fixture);
    }
}
