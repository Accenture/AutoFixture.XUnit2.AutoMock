namespace Objectivity.AutoFixture.XUnit2.Core.Tests
{
    using System;
    using System.Linq;

    using global::AutoFixture;
    using global::AutoFixture.Kernel;

    using Objectivity.AutoFixture.XUnit2.Core.SpecimenBuilders;

    using Xunit;

    internal static class FixtureAssertionExtensions
    {
        internal static void ShouldNotThrowOnRecursion(this IFixture fixture)
        {
            // Ensure there is no behavior for throwing exception on recursive structures.
            Assert.DoesNotContain(fixture.Behaviors, IsSpecimenBuilderTransformation<ThrowingRecursionBehavior>);
        }

        internal static void ShouldOmitRecursion(this IFixture fixture)
        {
            // Ensure there is a behavior added for omitting recursive types
            // on default recursion depth.
            Assert.Single(fixture.Behaviors, IsSpecimenBuilderTransformation<OmitOnRecursionBehavior>);
        }

        internal static void ShouldNotIgnoreVirtualMembers(this IFixture fixture)
        {
            Assert.DoesNotContain(fixture.Customizations, IsSpecimenBuilder<IgnoreVirtualMembersSpecimenBuilder>);
        }

        internal static void ShouldIgnoreVirtualMembers(this IFixture fixture)
        {
            Assert.Single(fixture.Customizations, IsSpecimenBuilder<IgnoreVirtualMembersSpecimenBuilder>);
        }

        internal static void ShouldIgnoreVirtualMembers(this IFixture fixture, Type reflectedType)
        {
            Assert.Single(
                fixture.Customizations.OfType<IgnoreVirtualMembersSpecimenBuilder>(),
                c => c.ReflectedType == reflectedType);
        }

        private static bool IsSpecimenBuilderTransformation<T>(ISpecimenBuilderTransformation b)
            where T : ISpecimenBuilderTransformation => b is T;

        private static bool IsSpecimenBuilder<T>(ISpecimenBuilder s)
            where T : ISpecimenBuilder => s is T;
    }
}
