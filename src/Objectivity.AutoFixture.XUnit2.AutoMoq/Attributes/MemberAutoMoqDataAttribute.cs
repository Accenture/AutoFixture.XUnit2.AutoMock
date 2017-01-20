// --------------------------------------------------------------------------------------------------------------------
// <copyright file="MemberAutoMoqDataAttribute.cs" company="Objectivity Bespoke Software Specialists">
//   Copyright (c) Objectivity Bespoke Software Specialists. All rights reserved.
// </copyright>
// <summary>
//   Defines the MemberAutoMoqDataAttribute type.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace Objectivity.AutoFixture.XUnit2.AutoMoq.Attributes
{
    using System;
    using System.Reflection;
    using Common;
    using MemberData;
    using Ploeh.AutoFixture;
    using Xunit;
    using Xunit.Sdk;

    [AttributeUsage(AttributeTargets.Method, AllowMultiple = true, Inherited = true)]
    [DataDiscoverer("Xunit.Sdk.MemberDataDiscoverer", "xunit.core")]
    public sealed class MemberAutoMoqDataAttribute : MemberDataAttributeBase
    {
        public MemberAutoMoqDataAttribute(string memberName, params object[] parameters)
            : this(new Fixture(), memberName.NotNull(nameof(memberName)), parameters)
        {
        }

        public MemberAutoMoqDataAttribute(IFixture fixture, string memberName, params object[] parameters)
            : base(memberName.NotNull(nameof(memberName)), parameters)
        {
            this.Fixture = fixture.NotNull(nameof(fixture));
        }

        public IFixture Fixture { get; }

        /// <summary>
        /// Gets or sets a value indicating whether to share a fixture across all data items; true by default.
        /// </summary>
        public bool ShareFixture { get; set; } = true;

        protected override object[] ConvertDataItem(MethodInfo testMethod, object item)
        {
            return DataItemConverterFactory
                .Create(this.ShareFixture, this.Fixture)
                .Convert(testMethod, item, this.MemberName, this.MemberType);
        }
    }
}