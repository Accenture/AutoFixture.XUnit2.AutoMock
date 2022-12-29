namespace Objectivity.AutoFixture.XUnit2.Core.Tests.MemberData
{
    using System;
    using System.Collections.Generic;
    using System.Reflection;

    using FluentAssertions;

    using global::AutoFixture;

    using Moq;

    using Objectivity.AutoFixture.XUnit2.Core.MemberData;
    using Objectivity.AutoFixture.XUnit2.Core.Providers;

    using Xunit;
    using Xunit.Sdk;

    [Collection("MemberAutoDataItemConverter")]
    [Trait("Category", "MemberData")]
    public class MemberAutoDataItemConverterTests
    {
        private readonly Fixture fixture = new();
        private readonly Mock<IAutoFixtureInlineAttributeProvider> dataAttributeProvider = new();
        private readonly Mock<DataAttribute> dataAttribute = new();
        private readonly IDataItemConverter converter;
        private readonly Type memberType = typeof(MemberAutoDataItemConverterTests);
        private readonly MethodInfo testMethod;
        private readonly string memberName;

        public MemberAutoDataItemConverterTests()
        {
            var data = this.fixture.Create<IEnumerable<object[]>>();
            this.dataAttributeProvider.Setup(p => p.GetAttribute(this.fixture, It.IsAny<object[]>())).Returns(this.dataAttribute.Object);
            this.dataAttribute.Setup(a => a.GetData(It.IsAny<MethodInfo>())).Returns(data);
            this.converter = new MemberAutoDataItemConverter(this.fixture, this.dataAttributeProvider.Object);
            this.testMethod = this.memberType.GetMethod(nameof(this.MethodUnderTest), BindingFlags.Instance | BindingFlags.NonPublic);
            this.memberName = this.fixture.Create<string>();
        }

        [Fact(DisplayName = "GIVEN valid parameters WHEN Convert is invoked THEN appropriate code is invoked and data is returned")]
        public void GivenValidParameters_WhenConvertIsInvoked_ThenAppropriateCodeIsInvokedAndDataIsReturned()
        {
            // Arrange
            var item = this.fixture.Create<object[]>();

            // Act
            var data = this.converter.Convert(this.testMethod, item, this.memberName, this.memberType);

            // Assert
            data.Should().NotBeNull();
            this.dataAttributeProvider.VerifyAll();
            this.dataAttribute.VerifyAll();
        }

        [Fact(DisplayName = "GIVEN uninitialized item WHEN Convert is invoked THEN exception is thrown")]
        public void GivenUninitializedItem_WhenConvertInvoked_ThenNullReturned()
        {
            // Arrange
            const object item = null;

            // Act
            var data = this.converter.Convert(this.testMethod, item, this.memberName, this.memberType);

            // Assert
            data.Should().BeNull();
        }

        [Fact(DisplayName = "GIVEN item of unexpected type WHEN Convert is invoked THEN exception is thrown")]
        public void GivenItemOfUnexpectedType_WhenConvertIsInvoked_ThenExceptionIsThrown()
        {
            // Arrange
            var item = this.fixture.Create<object>();

            // Act
            // Assert
            Assert.Throws<ArgumentException>(() => this.converter.Convert(this.testMethod, item, this.memberName, this.memberType));
        }

        [Fact(DisplayName = "GIVEN uninitialized test method WHEN Convert is invoked THEN exception is thrown")]
        public void GivenUninitializedTestMethod_WhenConvertIsInvoked_ThenExceptionIsThrown()
        {
            // Arrange
            const MethodInfo method = null;
            var item = this.fixture.Create<object[]>();

            // Act
            // Assert
            Assert.Throws<ArgumentNullException>(() => this.converter.Convert(method, item, this.memberName, this.memberType));
        }

        protected void MethodUnderTest()
        {
            // Empty method under test
        }
    }
}
