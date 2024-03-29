﻿namespace Objectivity.AutoFixture.XUnit2.Core.Attributes
{
    using System;

    using global::AutoFixture;
    using global::AutoFixture.Xunit2;
    using Objectivity.AutoFixture.XUnit2.Core.Customizations;

    /// <summary>
    /// An attribute that can be applied to parameters in an <see cref="AutoDataAttribute"/>-driven
    /// Theory to indicate that the parameter value should not have virtual properties populated
    /// when the <see cref="IFixture"/> creates an instance of that type.
    /// </summary>
    [AttributeUsage(AttributeTargets.Parameter, AllowMultiple = false)]
    public sealed class IgnoreVirtualMembersAttribute : CustomizeWithAttribute<IgnoreVirtualMembersCustomization>
    {
        public IgnoreVirtualMembersAttribute()
        {
            this.IncludeParameterType = true;
        }
    }
}
