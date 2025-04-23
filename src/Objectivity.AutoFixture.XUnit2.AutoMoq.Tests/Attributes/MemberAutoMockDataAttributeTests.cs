namespace Objectivity.AutoFixture.XUnit2.AutoMoq.Tests.Attributes
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    using global::AutoFixture;
    using global::AutoFixture.AutoMoq;
    using global::AutoFixture.Xunit2;
    using Moq;

    using Objectivity.AutoFixture.XUnit2.AutoMoq.Attributes;
    using Objectivity.AutoFixture.XUnit2.Core.Common;
    using Objectivity.AutoFixture.XUnit2.Core.Customizations;

    using Xunit;

    [Collection("MemberAutoMockDataAttribute")]
    [Trait("Category", "DataAttribute")]
    public class MemberAutoMockDataAttributeTests
    {
        public static IEnumerable<object[]> TestData { get; } = new[]
        {
            new object[] { 1, 2, 3 },
            new object[] { 4, 5, 6 },
            new object[] { 7, 8, 9 },
        };

        public static IEnumerable<object[]> TestDataShareFixture { get; } = new[]
        {
            new object[] { 0, new CurrentDateTimeCustomization() },
            new object[] { 1, new NumericSequencePerTypeCustomization() },
        };

        public static IEnumerable<object[]> TestDataDoNotShareFixture => TestDataShareFixture.Select(item => new[] { item.Last() });

        public int TestMethod(int first, int second, int third, int fourth, IDisposable disposable)
        {
            disposable.NotNull(nameof(disposable)).Dispose();
            return first + second + third + fourth;
        }

        [AutoData]
        [Theory(DisplayName = "WHEN constructor is invoked THEN has shared fixture")]
        public void WhenConstructorIsInvoked_ThenHasSharedFixture(Fixture fixture)
        {
            // Arrange
            // Act
            var attribute = new MemberAutoMockDataAttribute(fixture.Create<string>());

            // Assert
            Assert.NotNull(attribute.Fixture);
            Assert.False(attribute.IgnoreVirtualMembers);
            Assert.True(attribute.ShareFixture);
        }

        [Fact(DisplayName = "GIVEN existing member name WHEN GetData is invoked THEN appropriate data is returned")]
        public void GivenExistingMemberName_WhenGetDataIsInvoked_ThenAppropriateDataIsReturned()
        {
            // Arrange
            var attribute = new MemberAutoMockDataAttribute(nameof(TestData));
            var methodInfo = typeof(MemberAutoMockDataAttributeTests).GetMethod(nameof(this.TestMethod));
            var numberOfParameters = methodInfo.GetParameters().Length;

            // Act
            var data = attribute.GetData(methodInfo).ToList();

            // Assert
            Assert.Equal(TestData.Count(), data.Count);
            for (var i = data.Count - 1; i >= 0; i--)
            {
                var source = TestData.ElementAt(i);
                var result = data.ElementAt(i);
                var typeName = result[numberOfParameters - 1].GetType().Name;

                Assert.Equal(numberOfParameters, result.Length);
                Assert.Equal(source, result.Take(source.Length));
                Assert.StartsWith("IDisposableProxy", typeName);
            }
        }

        [InlineAutoData(true)]
        [InlineAutoData(false)]
        [Theory(DisplayName = "GIVEN IgnoreVirtualMembers WHEN GetData is invoked THEN fixture is customized correctly")]
        public void GivenIgnoreVirtualMembers_WhenGetDataIsInvoked_ThenFixtureIsCustomizedCorrectly(bool ignoreVirtualMembers)
        {
            // Arrange
            var fixture = new Mock<IFixture>();
            var customizations = new List<ICustomization>();
            fixture.Setup(x => x.Customize(It.IsAny<ICustomization>()))
                .Callback<ICustomization>(customizations.Add)
                .Returns(fixture.Object);

            var attribute = new MemberAutoMockDataAttribute(fixture.Object, nameof(TestData))
            {
                IgnoreVirtualMembers = ignoreVirtualMembers,
            };
            var methodInfo = typeof(MemberAutoMockDataAttributeTests).GetMethod(nameof(this.TestMethod));

            // Act
            attribute.GetData(methodInfo);

            // Assert
            Assert.Equal(2, customizations.Count);
            var customization = Assert.IsType<AutoDataCommonCustomization>(customizations[0]);
            Assert.Equal(ignoreVirtualMembers, customization.IgnoreVirtualMembers);
            Assert.IsType<AutoMoqCustomization>(customizations[1]);
        }

        [MemberAutoMockData(nameof(TestDataShareFixture), ShareFixture = true)]
        [Theory(DisplayName = "GIVEN MemberAutoMockData WHEN ShareFixture is set to true THEN same fixture per data row is used.")]
        public void GivenMemberAutoMockData_WhenShareFixtureIsSetToTrue_ThenSameFixturePerDataRowIsUsed(int index, ICustomization customization, IFixture fixture)
        {
            // Arrange
            const int expectedCustomizationsCount = 19;

            // Act
            fixture.Customize(customization);

            // Assert
            Assert.Equal(expectedCustomizationsCount + index, fixture.Customizations.Count);
        }

        [MemberAutoMockData(nameof(TestDataDoNotShareFixture), ShareFixture = false)]
        [Theory(DisplayName = "GIVEN MemberAutoMockData WHEN ShareFixture is set to false THEN unique fixture per data row is created.")]
        public void GivenMemberAutoMockData_WhenShareFixtureIsSetToFalse_ThenUniqueFixturePerDataRowIsCreated(ICustomization customization, IFixture fixture)
        {
            // Arrange
            const int expectedCustomizationsCount = 19;

            // Act
            fixture.Customize(customization);

            // Assert
            Assert.Equal(expectedCustomizationsCount, fixture.Customizations.Count);
        }

        [Fact(DisplayName = "GIVEN uninitialized fixture WHEN constructor is invoked THEN exception is thrown")]
        public void GivenUninitializedFixture_WhenConstructorIsInvoked_ThenExceptionIsThrown()
        {
            // Arrange
            var fixture = new Fixture();
            const Fixture uninitializedFixture = null;

            // Act
            object Act() => new MemberAutoMockDataAttribute(uninitializedFixture, fixture.Create<string>());

            // Assert
            var exception = Assert.Throws<ArgumentNullException>(Act);
            Assert.Equal("fixture", exception.ParamName);
        }

        [Fact(DisplayName = "GIVEN uninitialized member name WHEN constructor is invoked THEN exception is thrown")]
        public void GivenUninitializedMemberName_WhenConstructorIsInvoked_ThenExceptionIsThrown()
        {
            // Arrange
            const string memberName = null;

            // Act
            static object Act() => new MemberAutoMockDataAttribute(memberName);

            // Assert
            var exception = Assert.Throws<ArgumentNullException>(Act);
            Assert.Equal("memberName", exception.ParamName);
        }

        [MemberAutoMockData(nameof(TestData))]
        [Theory(DisplayName = "GIVEN test method has some member generated parameters WHEN test run THEN parameters are provided")]
        public void GivenTestMethodHasSomeMemberGeneratedParameters_WhenTestRun_ThenParametersAreProvided(
            int first,
            int second,
            int third,
            int fourth,
            IFakeObjectUnderTest objectInstance)
        {
            // Arrange
            var testData = TestData.ToList();

            // Act
            // Assert
            Assert.Contains(first, new[] { (int)testData[0][0], (int)testData[1][0], (int)testData[2][0] });
            Assert.Contains(second, new[] { (int)testData[0][1], (int)testData[1][1], (int)testData[2][1] });
            Assert.Contains(third, new[] { (int)testData[0][2], (int)testData[1][2], (int)testData[2][2] });
            Assert.NotEqual(0, fourth);

            Assert.NotNull(objectInstance);
            Assert.NotNull(objectInstance.StringProperty);
            Assert.NotEmpty(objectInstance.StringProperty);
        }
    }
}
