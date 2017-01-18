// --------------------------------------------------------------------------------------------------------------------
// <copyright file="InlineAutoMoqDataAttribute.cs" company="Objectivity Bespoke Software Specialists">
//   Copyright (c) Objectivity Bespoke Software Specialists. All rights reserved.
// </copyright>
// <summary>
//   Defines the InlineAutoMoqDataAttribute type.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace Objectivity.AutoFixture.XUnit2.AutoMoq
{
    using System;
    using Conditions;
    using Ploeh.AutoFixture.Xunit2;

    [AttributeUsage(AttributeTargets.Method, AllowMultiple = true)]
    public sealed class InlineAutoMoqDataAttribute : InlineAutoDataAttribute
    {
        public InlineAutoMoqDataAttribute(params object[] values)
            : this(new AutoMoqDataAttribute(), values)
        {
        }

        public InlineAutoMoqDataAttribute(AutoMoqDataAttribute autoDataAttribute, params object[] values)
            : base(autoDataAttribute.Requires().IsNotNull().Value, values)
        {
        }
    }
}