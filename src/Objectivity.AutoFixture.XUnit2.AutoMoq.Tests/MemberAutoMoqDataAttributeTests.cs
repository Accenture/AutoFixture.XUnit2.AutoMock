namespace Objectivity.AutoFixture.XUnit2.AutoMoq.Tests
{
    using System;
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
    }
}
