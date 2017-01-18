// --------------------------------------------------------------------------------------------------------------------
// <copyright file="AutoMoqDataAttribute.cs" company="Objectivity Bespoke Software Specialists">
//   Copyright (c) Objectivity Bespoke Software Specialists. All rights reserved.
// </copyright>
// <summary>
//   Defines the AutoMoqDataAttribute type.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace Objectivity.AutoFixture.XUnit2.AutoMoq
{
    using System;
    using System.Linq;

    using Conditions;

    using Ploeh.AutoFixture;
    using Ploeh.AutoFixture.AutoMoq;
    using Ploeh.AutoFixture.Xunit2;

    [AttributeUsage(AttributeTargets.Method, AllowMultiple = false, Inherited = true)]
    public sealed class AutoMoqDataAttribute : AutoDataAttribute
    {
        public AutoMoqDataAttribute()
            : this(new Fixture())
        {
        }

        public AutoMoqDataAttribute(IFixture fixture)
            : base(fixture.Requires().IsNotNull().Value)
        {
            // Configure auto-MOQ.
            fixture.Customize(new AutoConfiguredMoqCustomization());

            // Do not throw exception on circular references.
            fixture.Behaviors
                .OfType<ThrowingRecursionBehavior>()
                .ToList()
                .ForEach(b => fixture.Behaviors.Remove(b));

            // Ommit recursion on first level.
            fixture.Behaviors.Add(new OmitOnRecursionBehavior());
        }
    }
}
