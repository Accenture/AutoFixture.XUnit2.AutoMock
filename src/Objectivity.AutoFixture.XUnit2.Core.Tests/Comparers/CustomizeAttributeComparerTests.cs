namespace Objectivity.AutoFixture.XUnit2.Core.Tests.Comparers
{
    using System.Collections.Generic;

    using FluentAssertions;

    using global::AutoFixture;
    using global::AutoFixture.Xunit2;
    using Objectivity.AutoFixture.XUnit2.Core.Attributes;
    using Objectivity.AutoFixture.XUnit2.Core.Comparers;
    using Objectivity.AutoFixture.XUnit2.Core.Customizations;

    using Xunit;

    [Collection("CustomizeAttributeComparer")]
    [Trait("Category", "Comparers")]
    public class CustomizeAttributeComparerTests
    {
        private static readonly CustomizeAttributeComparer Comparer = new();
        private static readonly CustomizeWithAttribute CustomizeAttribute = new(typeof(DoNotThrowOnRecursionCustomization));
        private static readonly FrozenAttribute FrozenAttribute = new();

        public static IEnumerable<object[]> TestData { get; } = new[]
        {
            new object[] { CustomizeAttribute, CustomizeAttribute, 0 },
            new object[] { FrozenAttribute, FrozenAttribute, 0 },
            new object[] { FrozenAttribute, CustomizeAttribute, 1 },
            new object[] { CustomizeAttribute, FrozenAttribute, -1 },
        };

        [MemberData(nameof(TestData))]
        [Theory(DisplayName = "GIVEN both attributes WHEN Compare is invoked THEN expected result returned")]
        public void GivenBothNonFrozenAttributes_WhenCompareIsInvoked_ThenBothEquals(
            IParameterCustomizationSource x,
            IParameterCustomizationSource y,
            int expectedResult)
        {
            // Arrange
            // Act
            var result = Comparer.Compare(x, y);

            // Assert
            result.Should().Be(expectedResult);
        }
    }
}
