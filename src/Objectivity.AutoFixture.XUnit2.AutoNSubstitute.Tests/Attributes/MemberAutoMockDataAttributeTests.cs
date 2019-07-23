namespace Objectivity.AutoFixture.XUnit2.AutoNSubstitute.Tests.Attributes
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using FluentAssertions;
    using global::AutoFixture;
    using global::AutoFixture.AutoNSubstitute;
    using global::AutoFixture.Xunit2;
    using NSubstitute;
    using Objectivity.AutoFixture.XUnit2.AutoNSubstitute.Attributes;
    using Objectivity.AutoFixture.XUnit2.Core.Common;
    using Objectivity.AutoFixture.XUnit2.Core.Customizations;
    using Xunit;

    [Collection("MemberAutoMockDataAttribute")]
    [Trait("Category", "Attributes")]
    public class MemberAutoMockDataAttributeTests
    {
        public static IEnumerable<object[]> TestData { get; } = new[]
        {
            new object[] { 1, 2, 3 },
            new object[] { 4, 5, 6 },
            new object[] { 7, 8, 9 }
        };

        public static IEnumerable<object[]> TestDataShareFixture { get; } = new[]
        {
            new object[] { 0, new CurrentDateTimeCustomization() },
            new object[] { 1, new NumericSequencePerTypeCustomization() },
        };

        public static IEnumerable<object[]> TestDataDoNotShareFixture
        {
            get { return TestDataShareFixture.Select(item => new object[] { item.Last() }); }
        }

        public int TestMethod(int first, int second, int third, int fourth, IDisposable disposable)
        {
            disposable.NotNull(nameof(disposable)).Dispose();
            return first + second + third + fourth;
        }

        [Theory(DisplayName = "WHEN constructor is invoked THEN has shared fixture")]
        [AutoData]
        public void WhenConstructorIsInvoked_ThenHasSharedFixture(Fixture fixture)
        {
            // Arrange
            // Act
            var attribute = new MemberAutoMockDataAttribute(fixture.Create<string>());

            // Assert
            attribute.Fixture.Should().NotBeNull();
            attribute.IgnoreVirtualMembers.Should().BeFalse();
            attribute.ShareFixture.Should().BeTrue();
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
            data.Should().HaveSameCount(TestData);
            for (var i = data.Count - 1; i >= 0; i--)
            {
                var source = TestData.ElementAt(i);
                var result = data.ElementAt(i);

                result.Should().HaveCount(numberOfParameters);
                result.Should().ContainInOrder(source);
                result[numberOfParameters - 1].GetType().Name.Should().StartWith("ObjectProxy", "that way we know it was mocked.");
            }
        }

        [Theory(DisplayName = "GIVEN IgnoreVirtualMembers WHEN GetData is invoked THEN fixture is customized correctly")]
        [InlineAutoData(true)]
        [InlineAutoData(false)]
        public void GivenIgnoreVirtualMembers_WhenGetDataIsInvoked_ThenFixtureIsCustomizedCorrectly(bool ignoreVirtualMembers)
        {
            // Arrange
            var fixture = Substitute.For<IFixture>();
            var customizations = new List<ICustomization>();
            fixture.Customize(Arg.Do<ICustomization>(customization => customizations.Add(customization)))
                .Returns(fixture);

            var attribute = new MemberAutoMockDataAttribute(fixture, nameof(TestData))
            {
                IgnoreVirtualMembers = ignoreVirtualMembers
            };
            var methodInfo = typeof(MemberAutoMockDataAttributeTests).GetMethod(nameof(this.TestMethod));

            // Act
            attribute.GetData(methodInfo);

            // Assert
            customizations.Count.Should().Be(2);
            customizations[0]
                .Should()
                .BeOfType<AutoDataCommonCustomization>()
                .Which.IgnoreVirtualMembers.Should()
                .Be(ignoreVirtualMembers);
            customizations[1].Should().BeOfType<AutoNSubstituteCustomization>();
        }

        [Theory(DisplayName = "GIVEN MemberAutoMockData WHEN ShareFixture is set to true THEN same fixture per data row is used.")]
        [MemberAutoMockData(nameof(TestDataShareFixture), ShareFixture = true)]
        public void GivenMemberAutoMockData_WhenShareFixtureIsSetToTrue_ThenSameFixturePerDataRowIsUsed(int index, ICustomization customization, IFixture fixture)
        {
            // Arrange
            var expectedCostomizationsCount = 20;

            // Act
            var customizations = fixture.Customize(customization);

            // Assert
            fixture.Customizations.Should().HaveCount(expectedCostomizationsCount + index);
        }

        [Theory(DisplayName = "GIVEN MemberAutoMockData WHEN ShareFixture is set to false THEN unique fixture per data row is created.")]
        [MemberAutoMockData(nameof(TestDataDoNotShareFixture), ShareFixture = false)]
        public void GivenMemberAutoMockData_WhenShareFixtureIsSetToFalse_ThenUniqueFixturePerDataRowIsCreated(ICustomization customization, IFixture fixture)
        {
            // Arrange
            var expectedCostomizationsCount = 20;

            // Act
            var customizations = fixture.Customize(customization);

            // Assert
            fixture.Customizations.Should().HaveCount(expectedCostomizationsCount);
        }

        [Fact(DisplayName = "GIVEN uninitialized fixture WHEN constructor is invoked THEN exception is thrown")]
        public void GivenUninitializedFixture_WhenConstructorIsInvoked_ThenExceptionIsThrown()
        {
            // Arrange
            var fixture = new Fixture();
            const Fixture uninitializedFixture = null;

            // Act
            // Assert
            Assert.Throws<ArgumentNullException>(() => new MemberAutoMockDataAttribute(uninitializedFixture, fixture.Create<string>()));
        }

        [Fact(DisplayName = "GIVEN uninitialized member name WHEN constructor is invoked THEN exception is thrown")]
        public void GivenUninitializedMemberName_WhenConstructorIsInvoked_ThenExceptionIsThrown()
        {
            // Arrange
            const string memberName = null;

            // Act
            // Assert
            Assert.Throws<ArgumentNullException>(() => new MemberAutoMockDataAttribute(memberName));
        }

        [Theory(DisplayName = "GIVEN test method has some member generated parameters WHEN test run THEN parameters are provided")]
        [MemberAutoMockData(nameof(TestData))]
        public void GivenTestMethodHasSomeMemberGeneratedParameters_WhenTestRun_ThenParametersAreProvided(int first, int second, int third, int fourth, IDisposable disposable)
        {
            // Arrange
            var testData = TestData.ToList();

            // Act
            // Assert
            first.Should().BeOneOf((int)testData[0][0], (int)testData[1][0], (int)testData[2][0]);
            second.Should().BeOneOf((int)testData[0][1], (int)testData[1][1], (int)testData[2][1]);
            third.Should().BeOneOf((int)testData[0][2], (int)testData[1][2], (int)testData[2][2]);
            fourth.Should().NotBe(default(int));

            disposable.Should().NotBeNull();
            disposable.GetType().Name.Should().StartWith("ObjectProxy", "that way we know it was mocked.");
        }
    }
}