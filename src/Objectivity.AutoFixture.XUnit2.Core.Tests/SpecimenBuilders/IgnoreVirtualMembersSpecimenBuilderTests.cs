namespace Objectivity.AutoFixture.XUnit2.Core.Tests.SpecimenBuilders
{
    using System;
    using System.Diagnostics.CodeAnalysis;
    using FluentAssertions;
    using global::AutoFixture.Kernel;
    using global::AutoFixture.Xunit2;
    using Moq;
    using Objectivity.AutoFixture.XUnit2.Core.SpecimenBuilders;
    using Xunit;

    [Collection("IgnoreVirtualMembersSpecimenBuilder")]
    [Trait("Category", "SpecimenBuilders")]
    public class IgnoreVirtualMembersSpecimenBuilderTests
    {
        private readonly ISpecimenContext context;

        public IgnoreVirtualMembersSpecimenBuilderTests()
        {
            this.context = new Mock<ISpecimenContext>().Object;
        }

        [Fact(DisplayName = "GIVEN uninitialized request WHEN Create is invoked THEN NoSpecimen is returned")]
        public void GivenUninitializedRequest_WhenCreateInvoked_ThenNoSpecimenInstance()
        {
            // Arrange
            var builder = new IgnoreVirtualMembersSpecimenBuilder();

            // Act
            var specimen = builder.Create(null, this.context);

            // Assert
            specimen.Should().BeOfType<NoSpecimen>();
        }

        [Fact(DisplayName = "GIVEN not PropertyInfo request WHEN Create is invoked THEN NoSpecimen is returned")]
        public void GivenNotPropertyInfoRequest_WhenCreateInvoked_ThenNoSpecimenInstance()
        {
            // Arrange
            var builder = new IgnoreVirtualMembersSpecimenBuilder();

            // Act
            var specimen = builder.Create(new object(), this.context);

            // Assert
            specimen.Should().BeOfType<NoSpecimen>();
        }

        [Fact(DisplayName = "GIVEN not virtual PropertyInfo request WHEN Create is invoked THEN NoSpecimen is returned")]
        public void GivenNotVirtualPropertyInfoRequest_WhenCreateInvoked_ThenNoSpecimenInstance()
        {
            // Arrange
            var builder = new IgnoreVirtualMembersSpecimenBuilder();
            var notVirtualPropertyInfo = typeof(FakeObject).GetProperty(nameof(FakeObject.NotVirtualProperty));

            // Act
            var specimen = builder.Create(notVirtualPropertyInfo, this.context);

            // Assert
            specimen.Should().BeOfType<NoSpecimen>();
        }

        [Fact(DisplayName = "GIVEN virtual PropertyInfo request WHEN Create is invoked THEN OmitSpecimen is returned")]
        public void GivenVirtualPropertyInfoRequest_WhenCreateInvoked_ThenOmitSpecimenInstance()
        {
            // Arrange
            var builder = new IgnoreVirtualMembersSpecimenBuilder();
            var virtualPropertyInfo = typeof(FakeObject).GetProperty(nameof(FakeObject.VirtualProperty));

            // Act
            var specimen = builder.Create(virtualPropertyInfo, this.context);

            // Assert
            specimen.Should().BeOfType<OmitSpecimen>();
        }

        [Fact(DisplayName = "GIVEN default constructor WHEN invoked THEN reflected type should be null")]
        public void GivenDefaultConstructor_WhenInvoked_ThenReflectedTypeShouldBeNull()
        {
            // Arrange
            // Act
            var customization = new IgnoreVirtualMembersSpecimenBuilder();

            // Assert
            customization.ReflectedType.Should().BeNull();
        }

        [Theory(DisplayName = "GIVEN existing type WHEN constructor with parameter is invoked THEN that type should be reflected")]
        [AutoData]
        public void GivenExistingType_WhenConstructorWithParameterIsInvoked_ThenThatTypeShouldBeReflected(Type reflectedType)
        {
            // Arrange
            // Act
            var customization = new IgnoreVirtualMembersSpecimenBuilder(reflectedType);

            // Assert
            customization.ReflectedType.Should().BeSameAs(reflectedType);
        }

        [SuppressMessage("Microsoft.Performance", "CA1812:AvoidUninstantiatedInternalClasses", Justification = "Design required by tests.")]
        [SuppressMessage("ReSharper", "All", Justification = "Design required by tests.")]
        private class FakeObject
        {
            public object NotVirtualProperty { get; set; }

            public virtual object VirtualProperty { get; set; }
        }
    }
}
