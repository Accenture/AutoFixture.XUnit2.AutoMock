namespace Objectivity.AutoFixture.XUnit2.Core.Tests
{
    using System;
    using System.Linq;

    using global::AutoFixture;

    using JetBrains.Annotations;

    using Objectivity.AutoFixture.XUnit2.Core.SpecimenBuilders;

    using Xunit;

    internal static class FixtureAssertionExtensions
    {
        [AssertionMethod]
        internal static void ShouldNotThrowOnRecursion(this IFixture fixture)
        {
            // Ensure there is no behavior for throwing exception on recursive structures.
            Assert.DoesNotContain(fixture.Behaviors, b => b is ThrowingRecursionBehavior);
        }

        [AssertionMethod]
        internal static void ShouldOmitRecursion(this IFixture fixture)
        {
            // Ensure there is a behavior added for omitting recursive types
            // on default recursion depth.
            Assert.Single(fixture.Behaviors, b => b is OmitOnRecursionBehavior);
        }

        [AssertionMethod]
        internal static void ShouldNotIgnoreVirtualMembers(this IFixture fixture)
        {
            Assert.DoesNotContain(fixture.Customizations, s => s is IgnoreVirtualMembersSpecimenBuilder);
        }

        [AssertionMethod]
        internal static void ShouldIgnoreVirtualMembers(this IFixture fixture)
        {
            Assert.Single(fixture.Customizations, s => s is IgnoreVirtualMembersSpecimenBuilder);
        }

        [AssertionMethod]
        internal static void ShouldIgnoreVirtualMembers(this IFixture fixture, Type reflectedType)
        {
            Assert.Single(
                fixture.Customizations.OfType<IgnoreVirtualMembersSpecimenBuilder>(),
                c => c.ReflectedType == reflectedType);
        }
    }
}
