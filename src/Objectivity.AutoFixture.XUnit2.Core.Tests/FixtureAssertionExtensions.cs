namespace Objectivity.AutoFixture.XUnit2.Core.Tests
{
    using System;
    using System.Linq.Expressions;
    using FluentAssertions;
    using global::AutoFixture;
    using global::AutoFixture.Kernel;
    using Objectivity.AutoFixture.XUnit2.Core.SpecimenBuilders;

    internal static class FixtureAssertionExtensions
    {
        internal static void ShouldNotThrowOnRecursion(this IFixture fixture)
        {
            // Ensure there is no behaviour for throwing exception on recursive structures.
            fixture.Behaviors.Should().NotContain(b => b is ThrowingRecursionBehavior);
        }

        internal static void ShouldOmitRecursion(this IFixture fixture)
        {
            // Ensure there is a behaviour added for omitting recursive types
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

        internal static void ShouldIgnoreVirtualMembers(this IFixture fixture, Type reflectedType)
        {
            Expression<Func<ISpecimenBuilder, bool>> predicate = customization =>
                customization is IgnoreVirtualMembersSpecimenBuilder &&
                ((IgnoreVirtualMembersSpecimenBuilder)customization).ReflectedType == reflectedType;
            fixture.Customizations.Should().ContainSingle(predicate);
        }
    }
}