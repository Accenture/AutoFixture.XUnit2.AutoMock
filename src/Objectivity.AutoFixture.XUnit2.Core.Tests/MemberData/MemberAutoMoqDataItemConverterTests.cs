namespace Objectivity.AutoFixture.XUnit2.Core.Tests.MemberData
{
    using System;
    using System.Collections.Generic;
    using System.Reflection;
    using Objectivity.AutoFixture.XUnit2.Core.MemberData;
    using Objectivity.AutoFixture.XUnit2.Core.Providers;
    using FluentAssertions;
    using Moq;
    using Ploeh.AutoFixture;
    using Xunit;
    using Xunit.Sdk;

    [Collection("MemberAutoMoqDataItemConverter")]
    [Trait("Category", "MemberData")]
    public class MemberAutoMoqDataItemConverterTests
    {
        private readonly Fixture fixture = new Fixture();
        private readonly Mock<IAutoFixtureInlineAttributeProvider> dataAttributeProvider = new Mock<IAutoFixtureInlineAttributeProvider>();
        private readonly Mock<DataAttribute> dataAttribute = new Mock<DataAttribute>();
        private readonly MemberAutoMoqDataItemConverter converter;
        private readonly Type memberType = typeof(MemberAutoMoqDataItemConverterTests);
        private readonly MethodInfo testMethod;
        private readonly string memberName;

        public MemberAutoMoqDataItemConverterTests()
        {
            var data = fixture.Create<IEnumerable<object[]>>();
            dataAttributeProvider.Setup(p => p.GetAttribute(fixture, It.IsAny<object[]>())).Returns(dataAttribute.Object);
            dataAttribute.Setup(a => a.GetData(It.IsAny<MethodInfo>())).Returns(data);
            converter = new MemberAutoMoqDataItemConverter(fixture, this.dataAttributeProvider.Object);
            testMethod = memberType.GetMethod("TestMethod");
            memberName = fixture.Create<string>();
        }

        public void TestMethod()
        {
        }

        [Fact(DisplayName = "GIVEN valid parameters WHEN Convert is invoked THEN appropriate code is invoked and data is returned")]
        public void GivenValidParameters_WhenConvertIsInvoked_ThenAppropriateCodeIsInvokedAndDataIsReturned()
        {
            // Arrange
            var item = fixture.Create<object[]>();

            // Act
            var data = converter.Convert(testMethod, item, memberName, memberType);

            // Assert
            data.Should().NotBeNull();
            dataAttributeProvider.VerifyAll();
            dataAttribute.VerifyAll();
        }

        [Fact(DisplayName = "GIVEN uninitialized item WHEN Convert is invoked THEN exception is thrown")]
        public void GivenUninitializedItem_WhenConvertInvoked_ThenNullReturned()
        {
            // Arrange
            const object item = null;

            // Act
            var data = converter.Convert(testMethod, item, memberName, memberType);

            // Assert
            data.Should().BeNull();
        }

        [Fact(DisplayName = "GIVEN item of unexpected type WHEN Convert is invoked THEN exception is thrown")]
        public void GivenItemOfUnexpectedType_WhenConvertIsInvoked_ThenExceptionIsThrown()
        {
            // Arrange
            var item = fixture.Create<object>();

            // Act
            // Assert
            Assert.Throws<ArgumentException>(() => converter.Convert(testMethod, item, memberName, memberType));
        }

        [Fact(DisplayName = "GIVEN uninitialized test method WHEN Convert is invoked THEN exception is thrown")]
        public void GivenUninitializedTestMethod_WhenConvertIsInvoked_ThenExceptionIsThrown()
        {
            // Arrange
            const MethodInfo method = null;
            var item = fixture.Create<object[]>();

            // Act
            // Assert
            Assert.Throws<ArgumentNullException>(() => converter.Convert(method, item, memberName, memberType));
        }
    }
}
