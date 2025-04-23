namespace Objectivity.AutoFixture.XUnit2.Core.Tests.SpecimenBuilders
{
    using System;
    using System.Diagnostics.CodeAnalysis;
    using global::AutoFixture.Kernel;
    using global::AutoFixture.Xunit2;
    using Moq;

    using Objectivity.AutoFixture.XUnit2.Core.SpecimenBuilders;

    using Xunit;

    [Collection("IgnoreVirtualMembersSpecimenBuilder")]
    [Trait("Category", "SpecimenBuilders")]
    public class IgnoreVirtualMembersSpecimenBuilderTests
    {
        private readonly ISpecimenContext context = new Mock<ISpecimenContext>().Object;

        [Fact(DisplayName = "GIVEN default constructor WHEN invoked THEN reflected type should be null")]
        public void GivenDefaultConstructor_WhenInvoked_ThenReflectedTypeShouldBeNull()
        {
            // Arrange
            // Act
            var builder = new IgnoreVirtualMembersSpecimenBuilder();

            // Assert
            Assert.Null(builder.ReflectedType);
        }

        [AutoData]
        [Theory(DisplayName = "GIVEN existing type WHEN constructor with parameter is invoked THEN that type should be reflected")]
        public void GivenExistingType_WhenConstructorWithParameterIsInvoked_ThenThatTypeShouldBeReflected(Type reflectedType)
        {
            // Arrange
            // Act
            var builder = new IgnoreVirtualMembersSpecimenBuilder(reflectedType);

            // Assert
            Assert.Same(reflectedType, builder.ReflectedType);
        }

        [AutoData]
        [Theory(DisplayName = "GIVEN uninitialized request WHEN Create is invoked THEN NoSpecimen is returned")]
        public void GivenUninitializedRequest_WhenCreateInvoked_ThenNoSpecimenInstance(
            [Modest] IgnoreVirtualMembersSpecimenBuilder builder)
        {
            // Arrange
            // Act
            var specimen = builder.Create(null, this.context);

            // Assert
            Assert.IsType<NoSpecimen>(specimen);
        }

        [AutoData]
        [Theory(DisplayName = "GIVEN not PropertyInfo request WHEN Create is invoked THEN NoSpecimen is returned")]
        public void GivenNotPropertyInfoRequest_WhenCreateInvoked_ThenNoSpecimenInstance(
            [Modest] IgnoreVirtualMembersSpecimenBuilder builder)
        {
            // Arrange
            // Act
            var specimen = builder.Create(new object(), this.context);

            // Assert
            Assert.IsType<NoSpecimen>(specimen);
        }

        [AutoData]
        [Theory(DisplayName = "GIVEN not virtual PropertyInfo request WHEN Create is invoked THEN NoSpecimen is returned")]
        public void GivenNotVirtualPropertyInfoRequest_WhenCreateInvoked_ThenNoSpecimenInstance(
            [Modest] IgnoreVirtualMembersSpecimenBuilder builder)
        {
            // Arrange
            var notVirtualPropertyInfo = typeof(FakeObject).GetProperty(nameof(FakeObject.NotVirtualProperty));

            // Act
            var specimen = builder.Create(notVirtualPropertyInfo, this.context);

            // Assert
            Assert.IsType<NoSpecimen>(specimen);
        }

        [AutoData]
        [Theory(DisplayName = "GIVEN virtual PropertyInfo request but hosted in different type WHEN Create is invoked THEN NoSpecimen is returned")]
        public void GivenVirtualPropertyInfoRequestHostedInDifferentType_WhenCreateInvoked_ThenNoSpecimenInstance(
            [Frozen] Type reflectedType,
            [Greedy] IgnoreVirtualMembersSpecimenBuilder builder)
        {
            // Arrange
            var virtualPropertyInfo = typeof(FakeObject).GetProperty(nameof(FakeObject.VirtualProperty));

            // Act
            var specimen = builder.Create(virtualPropertyInfo, this.context);

            // Assert
            Assert.Same(reflectedType, builder.ReflectedType);
            Assert.IsType<NoSpecimen>(specimen);
        }

        [AutoData]
        [Theory(DisplayName = "GIVEN virtual PropertyInfo request WHEN Create is invoked THEN OmitSpecimen is returned")]
        public void GivenVirtualPropertyInfoRequest_WhenCreateInvoked_ThenOmitSpecimenInstance(
            [Modest] IgnoreVirtualMembersSpecimenBuilder builder)
        {
            // Arrange
            var virtualPropertyInfo = typeof(FakeObject).GetProperty(nameof(FakeObject.VirtualProperty));

            // Act
            var specimen = builder.Create(virtualPropertyInfo, this.context);

            // Assert
            Assert.IsType<OmitSpecimen>(specimen);
        }

        [Fact(DisplayName = "GIVEN virtual PropertyInfo request hosted in appropriate type WHEN Create is invoked THEN OmitSpecimen is returned")]
        public void GivenVirtualPropertyInfoRequestHostedInAppropriateType_WhenCreateInvoked_ThenOmitSpecimenInstance()
        {
            // Arrange
            var reflectedType = typeof(FakeObject);
            var builder = new IgnoreVirtualMembersSpecimenBuilder(reflectedType);
            var virtualPropertyInfo = reflectedType.GetProperty(nameof(FakeObject.VirtualProperty));

            // Act
            var specimen = builder.Create(virtualPropertyInfo, this.context);

            // Assert
            Assert.IsType<OmitSpecimen>(specimen);
        }

        [SuppressMessage("Microsoft.Performance", "CA1812:AvoidUninstantiatedInternalClasses", Justification = "Design required by tests.")]
        [SuppressMessage("ReSharper", "All", Justification = "Design required by tests.")]
        protected class FakeObject
        {
            public object NotVirtualProperty { get; set; }

            public virtual object VirtualProperty { get; set; }
        }
    }
}
