// --------------------------------------------------------------------------------------------------------------------
// <copyright file="MemberAutoMoqDataAttribute.cs" company="Objectivity Bespoke Software Specialists">
//   Copyright (c) Objectivity Bespoke Software Specialists. All rights reserved.
// </copyright>
// <summary>
//   Defines the MemberAutoMoqDataAttribute type.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace Objectivity.AutoFixture.XUnit2.AutoMoq
{
    using System;
    using System.Linq;
    using System.Reflection;
    using Xunit;
    using Xunit.Sdk;

    [AttributeUsage(AttributeTargets.Method, AllowMultiple = true, Inherited = true)]
    [DataDiscoverer("Xunit.Sdk.MemberDataDiscoverer", "xunit.core")]
    public sealed class MemberAutoMoqDataAttribute : MemberDataAttributeBase
    {
        public MemberAutoMoqDataAttribute(string memberName, params object[] parameters)
            : base(memberName.NotNull(nameof(memberName)), parameters)
        {
            this.IgnoreVirtualMembers = false;
        }

        public MemberAutoMoqDataAttribute(string memberName, bool ignoreVirtualMembers, params object[] parameters)
            : base(memberName.NotNull(nameof(memberName)), parameters)
        {
            this.IgnoreVirtualMembers = ignoreVirtualMembers;
        }

        public bool IgnoreVirtualMembers { get; }

        protected override object[] ConvertDataItem(MethodInfo testMethod, object item)
        {
            if (item == null)
            {
                return null;
            }

            var autoMoqData = new InlineAutoMoqDataAttribute(this.IgnoreVirtualMembers, item as object[]);
            return autoMoqData.GetData(testMethod).FirstOrDefault();
        }
    }
}