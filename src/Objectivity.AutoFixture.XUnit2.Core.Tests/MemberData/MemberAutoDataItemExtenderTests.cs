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
    public class MemberAutoDataItemExtenderTests
    {
        private static readonly MethodInfo TestMethod = typeof(MemberAutoDataItemExtenderTests).GetMethod(nameof(MethodUnderTest), BindingFlags.Instance | BindingFlags.NonPublic);
        private readonly Fixture fixture = new();
        private readonly Mock<IAutoFixtureInlineAttributeProvider> dataAttributeProvider = new();
        private readonly Mock<DataAttribute> dataAttribute = new();
        private readonly IDataItemExtender converter;

        public MemberAutoDataItemExtenderTests()
        {
            var data = this.fixture.Create<IEnumerable<object[]>>();
            this.dataAttributeProvider.Setup(p => p.GetAttribute(this.fixture, It.IsAny<object[]>())).Returns(this.dataAttribute.Object);
            this.dataAttribute.Setup(a => a.GetData(It.IsAny<MethodInfo>())).Returns(data);
            this.converter = new MemberAutoDataItemExtender(this.fixture, this.dataAttributeProvider.Object);
        }

        [Fact(DisplayName = "GIVEN provider with no data attribute WHEN Convert is invoked THEN Null is returned")]
        public void GivenProviderWithNoDataAttribute_WhenConvertIsInvoked_ThenNullReturned()
        {
            // Arrange
            var noData = Enumerable.Empty<object[]>();
            var noDataAttribute = new Mock<DataAttribute>();
            noDataAttribute.Setup(a => a.GetData(It.IsAny<MethodInfo>())).Returns(noData);
            var noDataProvider = new Mock<IAutoFixtureInlineAttributeProvider>();
            noDataProvider.Setup(x => x.GetAttribute(It.IsAny<IFixture>(), It.IsNotNull<object[]>())).Returns(noDataAttribute.Object);
            var noDataConverter = new MemberAutoDataItemExtender(this.fixture, noDataProvider.Object);
            var item = this.fixture.Create<object[]>();

            // Act
            var data = noDataConverter.Extend(TestMethod, item);

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
            var data = this.converter.Extend(TestMethod, item);

            // Assert
            data.Should().NotBeNull();
            this.dataAttributeProvider.VerifyAll();
            this.dataAttribute.VerifyAll();
        }

        [Fact(DisplayName = "GIVEN uninitialized item WHEN Convert is invoked THEN Null is returned")]
        public void GivenUninitializedItem_WhenConvertInvoked_ThenNullReturned()
        {
            // Arrange
            const object[] item = null;

            // Act
            var data = this.converter.Extend(TestMethod, item);

            // Assert
            data.Should().BeNull();
        }

        [Fact(DisplayName = "GIVEN uninitialized test method WHEN Convert is invoked THEN exception is thrown")]
        public void GivenUninitializedTestMethod_WhenConvertIsInvoked_ThenExceptionIsThrown()
        {
            // Arrange
            const MethodInfo method = null;
            var item = this.fixture.Create<object[]>();

            // Act
            Func<object> act = () => this.converter.Extend(method, item);

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
