namespace Objectivity.AutoFixture.XUnit2.AutoMoq.Tests
{
    using System;
    using System.Linq.Expressions;
    using AutoMoq.SpecimenBuilder;
    using FluentAssertions;
    using Ploeh.AutoFixture;
    using Ploeh.AutoFixture.AutoMoq;
    using Ploeh.AutoFixture.Kernel;

    internal static class FixtureAssertionExtensions
    {
        internal static void ShouldBeAutoMoqCustomized(this IFixture fixture)
        {
            Expression<Func<ISpecimenBuilder, bool>> mockProcessorPredicate =
                specimenBuilder =>
                    specimenBuilder is Postprocessor &&
                    ((Postprocessor) specimenBuilder).Builder is MockPostprocessor;

            // Ensure mock processor is added to customizations
            fixture.Customizations.Should().ContainSingle(mockProcessorPredicate);
        }

        internal static void ShouldNotThrowOnRecursion(this IFixture fixture)
        {
            // Ensure there is no behaviour for throwing exceprion on recursive structures.
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