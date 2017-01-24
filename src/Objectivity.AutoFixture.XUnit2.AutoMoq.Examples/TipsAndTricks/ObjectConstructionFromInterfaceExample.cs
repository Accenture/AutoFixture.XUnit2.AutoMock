namespace Objectivity.AutoFixture.XUnit2.AutoMoq.Examples.TipsAndTricks
{
    using AutoMoq.Attributes;
    using FluentAssertions;
    using Helpers;
    using Xunit;

    [Collection("TipsAndTricks")]
    [Trait("Category", "Samples")]
    public class ObjectConstructionFromInterfaceExample
    {
        [Theory]
        [AutoMoqData]
        public void GenerateUserFromInterface(IUser user)
        {
            user.Should().NotBeNull();
        }
    }
}
