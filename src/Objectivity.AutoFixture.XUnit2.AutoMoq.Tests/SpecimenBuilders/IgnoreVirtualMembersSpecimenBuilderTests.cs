namespace Objectivity.AutoFixture.XUnit2.AutoMoq.Tests.SpecimenBuilders
{
    using AutoMoq.SpecimenBuilders;
    using FluentAssertions;
    using Moq;
    using Ploeh.AutoFixture.Kernel;
    using Ploeh.AutoFixture.Xunit2;
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

        [Theory(DisplayName = "GIVEN uninitialized request WHEN Create is invoked THEN NoSpecimen is returned")]
        [AutoData]
        public void GivenUninitializedRequest_WhenCreateInvoked_ThenNoSpecimenInstance(IgnoreVirtualMembersSpecimenBuilder builder)
        {
            // Arrange
            // Act
            var specimen = builder.Create(null, this.context);

            // Assert
            specimen.Should().BeOfType<NoSpecimen>();
        }

        [Theory(DisplayName = "GIVEN not PropertyInfo request WHEN Create is invoked THEN NoSpecimen is returned")]
        [AutoData]
        public void GivenNotPropertyInfoRequest_WhenCreateInvoked_ThenNoSpecimenInstance(IgnoreVirtualMembersSpecimenBuilder builder)
        {
            // Arrange
            // Act
            var specimen = builder.Create(new object(), this.context);

            // Assert
            specimen.Should().BeOfType<NoSpecimen>();
        }

        [Theory(DisplayName = "GIVEN not virtual PropertyInfo request WHEN Create is invoked THEN NoSpecimen is returned")]
        [AutoData]
        public void GivenNotVirtualPropertyInfoRequest_WhenCreateInvoked_ThenNoSpecimenInstance(IgnoreVirtualMembersSpecimenBuilder builder)
        {
            // Arrange
            var notVirtualPropertyInfo = typeof(FakeObject).GetProperty(nameof(FakeObject.NotVirtualProperty));

            // Act
            var specimen = builder.Create(notVirtualPropertyInfo, this.context);

            // Assert
            specimen.Should().BeOfType<NoSpecimen>();
        }

        [Theory(DisplayName = "GIVEN virtual PropertyInfo request WHEN Create is invoked THEN null is returned")]
        [AutoData]
        public void GivenVirtualPropertyInfoRequest_WhenCreateInvoked_ThenNoSpecimenInstance(IgnoreVirtualMembersSpecimenBuilder builder)
        {
            // Arrange
            var virtualPropertyInfo = typeof(FakeObject).GetProperty(nameof(FakeObject.VirtualProperty));

            // Act
            var specimen = builder.Create(virtualPropertyInfo, this.context);

            // Assert
            specimen.Should().BeNull();
        }

        private class FakeObject
        {
            public object NotVirtualProperty { get; set; }
            public virtual object VirtualProperty{ get; set; }
        }
    }
}
