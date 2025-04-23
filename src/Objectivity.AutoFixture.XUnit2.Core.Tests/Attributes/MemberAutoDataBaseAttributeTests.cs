namespace Objectivity.AutoFixture.XUnit2.Core.Tests.Attributes
{
    using System;
    using System.Reflection;

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
        private static readonly Type MemberType = typeof(MemberAutoDataBaseAttributeTests);
        private static readonly MethodInfo TestMethod = MemberType.GetMethod(nameof(MethodUnderTest), BindingFlags.Instance | BindingFlags.NonPublic);

        public static TheoryData<Type, string> MemberTypeTestData { get; } = new()
        {
            { MemberType, MemberType.Name },
            { typeof(string), nameof(String) },
            { null, MemberType.Name },
        };

        public static TheoryData<string> NullTheoryData => null;

        [AutoData]
        [Theory(DisplayName = "GIVEN uninitialized fixture WHEN constructor is invoked THEN exception is thrown")]
        public void GivenUninitializedFixture_WhenConstructorIsInvoked_ThenExceptionIsThrown(string memberName)
        {
            // Arrange
            const IFixture fixture = null;
            var provider = new Mock<IAutoFixtureAttributeProvider>();

            // Act
            object Act() => new MemberAutoDataBaseAttributeUnderTest(fixture, memberName, provider.Object);

            // Assert
            var exception = Assert.Throws<ArgumentNullException>(Act);
            Assert.Equal("fixture", exception.ParamName);
        }

        [MemberData(nameof(MemberTypeTestData))]
        [Theory(DisplayName = "GIVEN item of unexpected type WHEN ConvertDataItem is invoked THEN exception is thrown")]
        public void GivenItemOfUnexpectedType_WhenConvertDataItemIsInvoked_ThenExceptionIsThrown(
            Type memberType,
            string expectedTypeName)
        {
            // Arrange
            var fixture = new Fixture();
            var memberName = fixture.Create<string>();
            var item = fixture.Create<object>();
            var attribute = new MemberAutoDataBaseAttributeUnderTest(fixture, memberName) { MemberType = memberType };

            // Act
            object[] Act() => attribute.CallConvertDataItem(TestMethod, item);

            // Assert
            var exception = Assert.Throws<ArgumentException>((Func<object[]>)Act);
            Assert.NotNull(exception.Message);
            Assert.NotEmpty(exception.Message);
            Assert.Contains(memberName, exception.Message);
            Assert.Contains(expectedTypeName, exception.Message);
        }

        [Fact(DisplayName = "GIVEN array WHEN ConvertDataItem is invoked THEN the same array is returned")]
        public void GivenArray_WhenConvertDataItemIsInvoked_ThenTheSameArrayIsReturned()
        {
            // Arrange
            var fixture = new Fixture();
            var array = fixture.Create<object[]>();
            var attribute = new MemberAutoDataBaseAttributeUnderTest(fixture, nameof(NullTheoryData));

            // Act
            var data = attribute.CallConvertDataItem(TestMethod, array);

            // Assert
            Assert.Equal(array, data);
        }

        [Fact(DisplayName = "GIVEN null theory data WHEN GetData is invoked THEN null is returned")]
        public void GivenNullItem_WhenGetDataIsInvoked_ThenNullIsReturned()
        {
            // Arrange
            var fixture = new Fixture();
            var attribute = new MemberAutoDataBaseAttributeUnderTest(fixture, nameof(NullTheoryData));

            // Act
            var data = attribute.GetData(TestMethod);

            // Assert
            Assert.Null(data);
        }

        protected void MethodUnderTest()
        {
            // Empty method under test
        }

        [AttributeUsage(AttributeTargets.Method, AllowMultiple = true, Inherited = true)]
        private sealed class MemberAutoDataBaseAttributeUnderTest : MemberAutoDataBaseAttribute
        {
            public MemberAutoDataBaseAttributeUnderTest(IFixture fixture, string memberName, params object[] parameters)
                : base(fixture, memberName, parameters)
            {
            }

            public object[] CallConvertDataItem(MethodInfo testMethod, object item) => this.ConvertDataItem(testMethod, item);

            protected override IAutoFixtureInlineAttributeProvider CreateProvider()
            {
                throw new NotImplementedException();
            }

            protected override IFixture Customize(IFixture fixture)
            {
                return fixture;
            }
        }
    }
}
