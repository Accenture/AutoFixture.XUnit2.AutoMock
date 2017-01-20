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
    using Ploeh.AutoFixture;
    using Ploeh.AutoFixture.AutoMoq;
    using Ploeh.AutoFixture.Xunit2;

    [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1019:DefineAccessorsForAttributeArguments", Justification = "MJI: There is no need to provide public accessor for ignoreVirtualMembers property")]
    [AttributeUsage(AttributeTargets.Method, AllowMultiple = false, Inherited = true)]
    public sealed class AutoMoqDataAttribute : AutoDataAttribute
    {
        public AutoMoqDataAttribute(bool ignoreVirtualMembers = false)
            : this(new Fixture(), ignoreVirtualMembers)
        {
        }

        public AutoMoqDataAttribute(IFixture fixture, bool ignoreVirtualMembers = false)
            : base(fixture.NotNull(nameof(fixture)))
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

            if (ignoreVirtualMembers)
            {
                fixture.Customizations.Add(new IgnoreVirtualMembersSpecimenBuilder());
            }
        }
    }
}
