﻿namespace Objectivity.AutoFixture.XUnit2.Core.Tests.Attributes
{
    using System;

    using FluentAssertions;

    using global::AutoFixture;
    using global::AutoFixture.Xunit2;
    using Moq;

    using Objectivity.AutoFixture.XUnit2.Core.Attributes;
    using Objectivity.AutoFixture.XUnit2.Core.Providers;

    using Xunit;

    [Collection("MemberAutoDataBaseAttribute")]
    [Trait("Category", "DataAttribute")]
    public class MemberAutoDataBaseAttributeTests
    {
        [AutoData]
        [Theory(DisplayName = "GIVEN uninitialized fixture WHEN constructor is invoked THEN exception is thrown")]
        public void GivenUninitializedFixture_WhenConstructorIsInvoked_ThenExceptionIsThrown(string memberName)
        {
            // Arrange
            const IFixture fixture = null;
            var provider = new Mock<IAutoFixtureAttributeProvider>();

            // Act
            Func<object> act = () => new MemberAutoDataBaseAttributeUnderTest(fixture, memberName, provider.Object);

            // Assert
            act.Should().Throw<ArgumentNullException>()
                .And.ParamName.Should().Be("fixture");
        }

        [AttributeUsage(AttributeTargets.Method, AllowMultiple = true, Inherited = true)]
        private sealed class MemberAutoDataBaseAttributeUnderTest : MemberAutoDataBaseAttribute
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
