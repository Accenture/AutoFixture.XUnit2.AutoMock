namespace Objectivity.AutoFixture.XUnit2.AutoMoq.Examples.TipsAndTricks
{
    using AutoMoq.Attributes;
    using FluentAssertions;
    using Helpers;
    using Xunit;

    [Collection("TipsAndTricks")]
    [Trait("Category", "Samples")]
    public class IgnoreVirtualMembersIssue
    {
        [Theory]
        [AutoMoqData(IgnoreVirtualMembers = true)]
        public void IssueWithClassThatImplementsInterface(User user)
        {
            user.Name.Should().BeNull();
            user.Surname.Should().BeNull();
            user.Substitute.Should().BeNull();
        }
    }
}
