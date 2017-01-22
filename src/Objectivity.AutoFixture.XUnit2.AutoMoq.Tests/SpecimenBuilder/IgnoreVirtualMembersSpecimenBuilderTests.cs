namespace Objectivity.AutoFixture.XUnit2.AutoMoq.Tests.SpecimenBuilder
{
    using AutoMoq.SpecimenBuilder;
    using FluentAssertions;
    using Moq;
    using Ploeh.AutoFixture.Kernel;
    using Xunit;

    public class IgnoreVirtualMembersSpecimenBuilderTests
    {
        private readonly ISpecimenContext context;

        public IgnoreVirtualMembersSpecimenBuilderTests()
        {
            this.context = new Mock<ISpecimenContext>().Object;
        }

        [Fact]
        public void GivenUninitializedRequest_WhenCreateInvoked_ThenNoSpecimenInstance()
        {
            // Arrange
            var builder = new IgnoreVirtualMembersSpecimenBuilder();

            // Act
            var specimen = builder.Create(null, this.context);

            // Assert
            specimen.Should().BeOfType<NoSpecimen>();
        }

        [Fact]
        public void GivenNotPropertyInfoRequest_WhenCreateInvoked_ThenNoSpecimenInstance()
        {
            // Arrange
            var builder = new IgnoreVirtualMembersSpecimenBuilder();

            // Act
            var specimen = builder.Create(new object(), this.context);

            // Assert
            specimen.Should().BeOfType<NoSpecimen>();
        }

        [Fact]
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

        [Fact]
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
            public virtual object VirtualProperty{ get; set; }
        }
    }
}
