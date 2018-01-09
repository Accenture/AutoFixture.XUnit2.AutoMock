namespace Objectivity.AutoFixture.XUnit2.Core.Tests.SpecimenBuilders
{
    using FluentAssertions;
    using Moq;
    using Objectivity.AutoFixture.XUnit2.Core.SpecimenBuilders;
    using Ploeh.AutoFixture.Kernel;
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

        [Fact(DisplayName = "GIVEN virtual PropertyInfo request WHEN Create is invoked THEN null is returned")]
        public void GivenVirtualPropertyInfoRequest_WhenCreateInvoked_ThenNoSpecimenInstance()
        {
            // Arrange
            var builder = new IgnoreVirtualMembersSpecimenBuilder();
            var virtualPropertyInfo = typeof(FakeObject).GetProperty(nameof(FakeObject.VirtualProperty));

            // Act
            var specimen = builder.Create(virtualPropertyInfo, this.context);

            // Assert
            specimen.Should().BeNull();
        }

        private class FakeObject
        {
            public object NotVirtualProperty { get; set; }

            public virtual object VirtualProperty { get; set; }
        }
    }
}
