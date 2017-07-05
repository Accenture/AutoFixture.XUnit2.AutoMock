namespace Objectivity.AutoFixture.XUnit2.AutoMoq.Tests.Fixture
{
    using AutoMoq.Fixture;
    using Ploeh.AutoFixture;
    using Ploeh.AutoFixture.Xunit2;
    using Xunit;

    [Collection("FixtureExtensions")]
    [Trait("Category", "Fixture")]
    public class FixtureExtensionsTests
    {
        [Theory(DisplayName = "GIVEN existing fixture WHEN applying customizations THEN fixture is appropriately customized")]
        [InlineAutoData(true)]
        [InlineAutoData(false)]
        public void GivenExistingFixture_WhenApplyingCustomizations_ThenFixtureIsAppropriatelyCustomized(bool ignoreVirtualMembers, Fixture fixture)
        {
            // Arrange
            // Act
            fixture.ApplyCustomizations(ignoreVirtualMembers);

            // Assert
            fixture.ShouldBeAutoMoqCustomized();
            fixture.ShouldNotThrowOnRecursion();
            fixture.ShouldOmitRecursion();
            if (ignoreVirtualMembers)
            {
                fixture.ShouldIgnoreVirtualMembers();
            }
            else
            {
                fixture.ShouldNotIgnoreVirtualMembers();
            }
        }
    }
}
