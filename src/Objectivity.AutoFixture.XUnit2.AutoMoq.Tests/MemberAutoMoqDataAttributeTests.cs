namespace Objectivity.AutoFixture.XUnit2.AutoMoq.Tests
{
    using System;
    using FluentAssertions;
    using Xunit;

    public class MemberAutoMoqDataAttributeTests
    {
        [Fact]
        public void GivenUninitializedMemberName_WhenConstructorInvoked_ThenExceptionShouldBeThrown()
        {
            // Arrange
            const string memberName = null;

            // Act
            // Assert
            Assert.Throws<ArgumentNullException>(() => new MemberAutoMoqDataAttribute(memberName));
        }

        [Fact]
        public void GivenMemberNameWithParameters_WhenConstructorInvoked_ThenHasSpecifiedValues()
        {
            // Arrange
            const string memberName = "Name";
            var parameters = new[] {new object(), new object()};

            // Act
            var attribute = new MemberAutoMoqDataAttribute(memberName, parameters);

            // Assert
            attribute.MemberName.Should().Be(memberName);
            attribute.Parameters.Should().BeSameAs(parameters);
            attribute.IgnoreVirtualMembers.Should().BeFalse();
        }

        [Fact]
        public void GivenMemberNameWithParametersAndIgnoreVirtualMembers_WhenConstructorInvoked_ThenHasSpecifiedValues()
        {
            // Arrange
            const string memberName = "Name";
            var parameters = new[] { new object(), new object() };

            // Act
            var attribute = new MemberAutoMoqDataAttribute(memberName, true, parameters);

            // Assert
            attribute.MemberName.Should().Be(memberName);
            attribute.Parameters.Should().BeSameAs(parameters);
            attribute.IgnoreVirtualMembers.Should().BeTrue();
        }
    }
}
