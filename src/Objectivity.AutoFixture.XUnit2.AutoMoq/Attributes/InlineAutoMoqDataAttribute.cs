// --------------------------------------------------------------------------------------------------------------------
// <copyright file="InlineAutoMoqDataAttribute.cs" company="Objectivity Bespoke Software Specialists">
//   Copyright (c) Objectivity Bespoke Software Specialists. All rights reserved.
// </copyright>
// <summary>
//   Defines the InlineAutoMoqDataAttribute type.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace Objectivity.AutoFixture.XUnit2.AutoMoq.Attributes
{
    using System;
    using Common;
    using Ploeh.AutoFixture.Xunit2;

    [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1019:DefineAccessorsForAttributeArguments", Justification = "MJI: There is no need to provide public accessor for ignoreVirtualMembers property")]
    [AttributeUsage(AttributeTargets.Method, AllowMultiple = true)]
    public sealed class InlineAutoMoqDataAttribute : InlineAutoDataAttribute
    {
        public InlineAutoMoqDataAttribute(params object[] values)
            : this(new AutoMoqDataAttribute(), values)
        {
        }

        public InlineAutoMoqDataAttribute(bool ignoreVirtualMembers, params object[] values)
            : this(new AutoMoqDataAttribute(ignoreVirtualMembers), values)
        {
        }

        public InlineAutoMoqDataAttribute(AutoMoqDataAttribute autoDataAttribute, params object[] values)
            : base(autoDataAttribute.NotNull(nameof(autoDataAttribute)), values)
        {
        }
    }
}