﻿namespace Objectivity.AutoFixture.XUnit2.Core.CustomisationFactories
{
    using System;
    using System.Reflection;

    using global::AutoFixture;

    public interface ICustomisationFactory
    {
        ICustomization Create(ParameterInfo parameter, bool includeParameterType, Type type, params object[] args);
    }
}