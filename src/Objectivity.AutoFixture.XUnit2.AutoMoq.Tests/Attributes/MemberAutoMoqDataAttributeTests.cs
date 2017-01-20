namespace Objectivity.AutoFixture.XUnit2.AutoMoq.Tests.Attributes
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using AutoMoq.Attributes;
    using FluentAssertions;
    using Ploeh.AutoFixture;
    using Xunit;

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

        [Fact(DisplayName = "WHEN constructor is invoked THAN has shared fixture")]
        public void WhenConstructorIsInvoked_ThanHasSharedFixture()
        {
            // Arrange
            var fixture = new Fixture();

            // Act
            var attribute = new MemberAutoMoqDataAttribute(fixture.Create<string>());

            // Assert
            attribute.Fixture.Should().NotBeNull();
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
    }
}