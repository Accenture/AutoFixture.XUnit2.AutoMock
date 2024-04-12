namespace Objectivity.AutoFixture.XUnit2.Core.Tests.MemberData
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
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
        private static readonly Type MemberType = typeof(MemberAutoDataItemConverterTests);
        private static readonly MethodInfo TestMethod = MemberType.GetMethod(nameof(MethodUnderTest), BindingFlags.Instance | BindingFlags.NonPublic);
        private readonly Fixture fixture = new();
        private readonly Mock<IAutoFixtureInlineAttributeProvider> dataAttributeProvider = new();
        private readonly Mock<DataAttribute> dataAttribute = new();
        private readonly IDataItemConverter converter;
        private readonly string memberName;

        public MemberAutoDataItemConverterTests()
        {
            var data = this.fixture.Create<IEnumerable<object[]>>();
            this.dataAttributeProvider.Setup(p => p.GetAttribute(this.fixture, It.IsAny<object[]>())).Returns(this.dataAttribute.Object);
            this.dataAttribute.Setup(a => a.GetData(It.IsAny<MethodInfo>())).Returns(data);
            this.converter = new MemberAutoDataItemConverter(this.fixture, this.dataAttributeProvider.Object);
            this.memberName = this.fixture.Create<string>();
        }

        public static TheoryData<Type, string> MemberTypeTestData { get; } = new()
        {
            { MemberType, MemberType.Name },
            { typeof(string), nameof(String) },
            { null, MemberType.Name },
        };

        [Fact(DisplayName = "GIVEN provider with no data attribute WHEN Convert is invoked THEN Null is returned")]
        public void GivenProviderWithNoDataAttribute_WhenConvertIsInvoked_ThenNullReturned()
        {
            // Arrange
            var noData = Enumerable.Empty<object[]>();
            var noDataAttribute = new Mock<DataAttribute>();
            noDataAttribute.Setup(a => a.GetData(It.IsAny<MethodInfo>())).Returns(noData);
            var noDataProvider = new Mock<IAutoFixtureInlineAttributeProvider>();
            noDataProvider.Setup(x => x.GetAttribute(It.IsAny<IFixture>(), It.IsNotNull<object[]>())).Returns(noDataAttribute.Object);
            var noDataConverter = new MemberAutoDataItemConverter(this.fixture, noDataProvider.Object);
            var item = this.fixture.Create<object[]>();

            // Act
            var data = noDataConverter.Convert(TestMethod, item, this.memberName, MemberType);

            // Assert
            data.Should().BeNull();
            noDataProvider.VerifyAll();
            noDataAttribute.VerifyAll();
        }

        [Fact(DisplayName = "GIVEN valid parameters WHEN Convert is invoked THEN appropriate code is invoked and data is returned")]
        public void GivenValidParameters_WhenConvertIsInvoked_ThenAppropriateCodeIsInvokedAndDataIsReturned()
        {
            // Arrange
            var item = this.fixture.Create<object[]>();

            // Act
            var data = this.converter.Convert(TestMethod, item, this.memberName, MemberType);

            // Assert
            data.Should().NotBeNull();
            this.dataAttributeProvider.VerifyAll();
            this.dataAttribute.VerifyAll();
        }

        [Fact(DisplayName = "GIVEN uninitialized item WHEN Convert is invoked THEN Null is returned")]
        public void GivenUninitializedItem_WhenConvertInvoked_ThenNullReturned()
        {
            // Arrange
            const object item = null;

            // Act
            var data = this.converter.Convert(TestMethod, item, this.memberName, MemberType);

            // Assert
            data.Should().BeNull();
        }

        [MemberData(nameof(MemberTypeTestData))]
        [Theory(DisplayName = "GIVEN item of unexpected type WHEN Convert is invoked THEN exception is thrown")]
        public void GivenItemOfUnexpectedType_WhenConvertIsInvoked_ThenExceptionIsThrown(
            Type memberType,
            string expectedTypeName)
        {
            // Arrange
            var item = this.fixture.Create<object>();

            // Act
            Func<object> act = () => this.converter.Convert(TestMethod, item, this.memberName, memberType);

            // Assert
            act.Should().Throw<ArgumentException>()
                .And.Message.Should().NotBeNullOrEmpty()
                .And.Contain(this.memberName).And.Contain(expectedTypeName);
        }

        [Fact(DisplayName = "GIVEN uninitialized test method WHEN Convert is invoked THEN exception is thrown")]
        public void GivenUninitializedTestMethod_WhenConvertIsInvoked_ThenExceptionIsThrown()
        {
            // Arrange
            const MethodInfo method = null;
            var item = this.fixture.Create<object[]>();

            // Act
            Func<object> act = () => this.converter.Convert(method, item, this.memberName, MemberType);

            // Assert
            act.Should().Throw<ArgumentNullException>()
                .And.ParamName.Should().Be("testMethod");
        }

        protected void MethodUnderTest()
        {
            // Empty method under test
        }
    }
}
