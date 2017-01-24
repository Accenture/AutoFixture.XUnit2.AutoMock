namespace Objectivity.AutoFixture.XUnit2.AutoMoq.Examples.TipsAndTricks
{
    using AutoMoq.Attributes;
    using FluentAssertions;
    using Helpers;
    using Xunit;

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
