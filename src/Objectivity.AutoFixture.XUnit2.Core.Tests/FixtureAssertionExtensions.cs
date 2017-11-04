namespace Objectivity.AutoFixture.XUnit2.Core.Tests
{
    using System;
    using System.Linq.Expressions;
    using FluentAssertions;
    using Objectivity.AutoFixture.XUnit2.Core.SpecimenBuilders;
    using Ploeh.AutoFixture;
    using Ploeh.AutoFixture.Kernel;

    internal static class FixtureAssertionExtensions
    {
        internal static void ShouldNotThrowOnRecursion(this IFixture fixture)
        {
            // Ensure there is no behaviour for throwing exception on recursive structures.
            fixture.Behaviors.Should().NotContain(b => b is ThrowingRecursionBehavior);
        }

        internal static void ShouldOmitRecursion(this IFixture fixture)
        {
            // Ensure there is a beaviour added for omitting recursive types
            // on default recursion depth.
            fixture.Behaviors.Should().ContainSingle(b => b is OmitOnRecursionBehavior);
        }

        internal static void ShouldNotIgnoreVirtualMembers(this IFixture fixture)
        {
            fixture.Customizations.Should().NotContain(s => s is IgnoreVirtualMembersSpecimenBuilder);
        }

        internal static void ShouldIgnoreVirtualMembers(this IFixture fixture)
        {
            fixture.Customizations.Should().ContainSingle(s => s is IgnoreVirtualMembersSpecimenBuilder);
        }
    }
}