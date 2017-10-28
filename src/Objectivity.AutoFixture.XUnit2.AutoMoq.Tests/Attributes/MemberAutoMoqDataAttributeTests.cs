namespace Objectivity.AutoFixture.XUnit2.AutoMoq.Tests.Attributes
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using Objectivity.AutoFixture.XUnit2.Core.Attributes;
    using Objectivity.AutoFixture.XUnit2.Core.Customizations;
    using FluentAssertions;
    using Moq;
    using Ploeh.AutoFixture;
    using Ploeh.AutoFixture.Xunit2;
    using Xunit;
    using Ploeh.AutoFixture.AutoMoq;
    using Objectivity.AutoFixture.XUnit2.AutoMoq.Attributes;

    [Collection("MemberAutoMoqDataAttribute")]
    [Trait("Category", "Attributes")]
    public class MemberAutoMoqDataAttributeTests
    {
        public static IEnumerable<object[]> TestData { get; } = new[]
        {
            new object[] {1, 2, 3},
            new object[] {4, 5, 6},
            new object[] {7, 8, 9}
        };

        public int TestMethod(int first, int second, int third, int fourth, IDisposable disposable)
        {
            if (disposable == null)
            {
                throw new ArgumentNullException(nameof(disposable));
            }

            disposable.Dispose();
            return first + second + third + fourth;
        }

        [Theory(DisplayName = "WHEN constructor is invoked THEN has shared fixture")]
        [AutoData]
        public void WhenConstructorIsInvoked_ThenHasSharedFixture(Fixture fixture)
        {
            // Arrange
            // Act
            var attribute = new MemberAutoMoqDataAttribute(fixture.Create<string>());

            // Assert
            attribute.Fixture.Should().NotBeNull();
            attribute.IgnoreVirtualMembers.Should().BeFalse();
            attribute.ShareFixture.Should().BeTrue();
        }

        [Fact(DisplayName = "GIVEN existing member name WHEN GetData is invoked THEN appropriate data is returned")]
        public void GivenExistingMemberName_WhenGetDataIsInvoked_ThenAppropriateDataIsReturned()
        {
            // Arrange
            var attribute = new MemberAutoMoqDataAttribute("TestData");
            var methodInfo = typeof(MemberAutoMoqDataAttributeTests).GetMethod("TestMethod");
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
                result[numberOfParameters - 1].GetType().Name.Should().StartWith("ObjectProxy", "that way we know it was mocked with MOQ.");
            }
        }

        [Theory(DisplayName = "GIVEN IgnoreVirtualMembers WHEN GetData is invoked THEN fixture is customized correctly")]
        [InlineAutoData(true)]
        [InlineAutoData(false)]
        public void GivenIgnoreVirtualMembers_WhenGetDataIsInvoked_ThenFixtureIsCustomizedCorrectly(bool ignoreVirtualMembers)
        {
            // Arrange
            var fixture = new Mock<IFixture>();
            var customizations = new List<ICustomization>();
            fixture.Setup(x => x.Customize(It.IsAny<ICustomization>())).Callback<ICustomization>(customization => customizations.Add(customization));

            var attribute = new MemberAutoMoqDataAttribute(fixture.Object, "TestData")
            {
                IgnoreVirtualMembers = ignoreVirtualMembers
            };
            var methodInfo = typeof(MemberAutoMoqDataAttributeTests).GetMethod("TestMethod");

            // Act
            attribute.GetData(methodInfo);

            // Assert
            customizations[0].Should().BeOfType<AutoConfiguredMoqCustomization>();
            customizations[1].Should().BeOfType<AutoDataCommonCustomization>();
            customizations[2].Should().BeOfType<IgnoreVirtualMembersCustomization>();
            ((IgnoreVirtualMembersCustomization)customizations[2]).IgnoreVirtualMembers.Should().Be(ignoreVirtualMembers);
        }

        [Fact(DisplayName = "GIVEN uninitialized fixture WHEN constructor is invoked THEN exception is thrown")]
        public void GivenUninitializedFixture_WhenConstructorIsInvoked_ThenExceptionIsThrown()
        {
            // Arrange
            var fixture = new Fixture();
            const Fixture uninitializedFixture = null;

            // Act
            // Assert
            Assert.Throws<ArgumentNullException>(() => new MemberAutoMoqDataAttribute(uninitializedFixture, fixture.Create<string>()));
        }

        [Fact(DisplayName = "GIVEN uninitialized member name WHEN constructor is invoked THEN exception is thrown")]
        public void GivenUninitializedMemberName_WhenConstructorIsInvoked_ThenExceptionIsThrown()
        {
            // Arrange
            const string memberName = null;

            // Act
            // Assert
            Assert.Throws<ArgumentNullException>(() => new MemberAutoMoqDataAttribute(memberName));
        }


        [MemberAutoMoqData("TestData")]
        [Theory(DisplayName = "GIVEN test method has some member generated parameters WHEN test run THEN parameters are provided")]
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
            disposable.GetType().Name.Should().StartWith("ObjectProxy", "that way we know it was mocked with MOQ.");
        }
    }
}