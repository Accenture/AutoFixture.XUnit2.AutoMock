namespace Objectivity.AutoFixture.XUnit2.Core.Tests.Attributes
{
    using System;
    using Moq;
    using Objectivity.AutoFixture.XUnit2.Core.Attributes;
    using Objectivity.AutoFixture.XUnit2.Core.Providers;
    using Ploeh.AutoFixture;
    using Ploeh.AutoFixture.Xunit2;
    using Xunit;
    using Xunit.Sdk;

    [Collection("MemberAutoDataBaseAttribute")]
    [Trait("Category", "Attributes")]
    public class MemberAutoDataBaseAttributeTests
    {
        [Theory(DisplayName = "GIVEN uninitialized fixture WHEN constructor is invoked THEN exception is thrown")]
        [AutoData]
        public void GivenUninitializedFixture_WhenConstructorIsInvoked_ThenExceptionIsThrown(string memberName)
        {
            // Arrange
            const IFixture fixture = null;
            var provider = new Mock<IAutoFixtureAttributeProvider>();

            // Act
            // Assert
            Assert.Throws<ArgumentNullException>(() => new MemberAutoDataBaseAttributeUnderTest(fixture, memberName, provider.Object));
        }

        private class MemberAutoDataBaseAttributeUnderTest : MemberAutoDataBaseAttribute
        {
            public MemberAutoDataBaseAttributeUnderTest(IFixture fixture, string memberName, params object[] parameters)
                : base(fixture, memberName, parameters)
            {
            }

            protected override IAutoFixtureInlineAttributeProvider CreateProvider()
            {
                throw new NotImplementedException();
            }

            protected override IFixture Customize(IFixture fixture)
            {
                throw new NotImplementedException();
            }
        }
    }
}
