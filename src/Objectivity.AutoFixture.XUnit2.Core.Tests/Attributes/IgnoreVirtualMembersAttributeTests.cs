namespace Objectivity.AutoFixture.XUnit2.Core.Tests.Attributes
{
    using System;
    using System.Diagnostics.CodeAnalysis;
    using System.Reflection;

    using FluentAssertions;
    using global::AutoFixture.Xunit2;
    using Objectivity.AutoFixture.XUnit2.Core.Attributes;

    using Xunit;

    [Collection("IgnoreVirtualMembersAttribute")]
    [Trait("Category", "CustomizeAttribute")]
    [SuppressMessage("Design", "CA1034:Nested types should not be visible", Justification = "Test objects")]
    [SuppressMessage("ReSharper", "ClassNeverInstantiated.Global", Justification = "Test classes instantiated by AutoFixture.")]
    [SuppressMessage("ReSharper", "UnusedMember.Global", Justification = "Test members instantiated by AutoFixture and used in tests.")]
    [SuppressMessage("ReSharper", "UnusedMemberInSuper.Global", Justification = "Test members instantiated by AutoFixture and used in tests.")]
    [SuppressMessage("ReSharper", "UnusedAutoPropertyAccessor.Global", Justification = "Test members instantiated by AutoFixture and used in tests.")]
    public class IgnoreVirtualMembersAttributeTests
    {
        private interface IUserWithSubstitute
        {
            string Name { get; set; }

            User Substitute { get; set; }
        }

        [AutoData]
        [Theory(DisplayName = "GIVEN test method has parameters of various types WHEN the one with interface is decorated with IgnoreVirtualMembersAttribute THEN only that parameter has no virtual properties populated")]
        public void GivenTestMethodHasParametersOfVariousTypes_WhenTheOneWithInterfaceIsDecoratedWithIgnoreVirtualMembersAttribute_ThenOnlyThatParameterHasNoVirtualPropertiesPopulated(
            [IgnoreVirtualMembers] UserWithSubstitute userWithSubstitute,
            User user)
        {
            userWithSubstitute.Name.Should().BeNull();
            userWithSubstitute.Substitute.Should().BeNull();

            user.Name.Should().NotBeNull();
            user.Address.Should().NotBeNull();
        }

        [AutoData]
        [Theory(DisplayName = "GIVEN test method has parameters of various types WHEN the one without interface is decorated with IgnoreVirtualMembersAttribute THEN only that parameter has no virtual properties populated")]
        public void GivenTestMethodHasParametersOfVariousTypes_WhenOneIsDecoratedWithIgnoreVirtualMembersAttribute_ThenOnlyThatParameterHasNoVirtualPropertiesPopulated(
            UserWithSubstitute userWithSubstitute,
            [IgnoreVirtualMembers] User user)
        {
            userWithSubstitute.Name.Should().NotBeNull();
            userWithSubstitute.Substitute.Should().NotBeNull();

            user.Name.Should().NotBeNull();
            user.Address.Should().BeNull();
        }

        [AutoData]
        [Theory(DisplayName = "GIVEN test method has parameters of same type WHEN second one is decorated with IgnoreVirtualMembersAttribute THEN that parameter and following ones have no virtual properties populated")]
        public void GivenTestMethodHasParametersOfSameType_WhenSecondOneIsDecoratedWithIgnoreVirtualMembersAttribute_ThenThatParameterAndFollowingOnesHaveNoVirtualPropertiesPopulated(
            UserWithSubstitute user1,
            [IgnoreVirtualMembers] UserWithSubstitute user2,
            UserWithSubstitute user3)
        {
            user1.Name.Should().NotBeNull();
            user1.Substitute.Should().NotBeNull();

            user2.Name.Should().BeNull();
            user2.Substitute.Should().BeNull();

            user3.Name.Should().BeNull();
            user3.Substitute.Should().BeNull();
        }

        [AutoData]
        [Theory(DisplayName = "GIVEN test method has parameters of same type WHEN first one is decorated with IgnoreVirtualMembersAttribute THEN all parameters has no virtual properties populated")]
        public void GivenTestMethodHasParametersOfSameType_WhenFirstOneIsDecoratedWithIgnoreVirtualMembersAttribute_ThenAllParametersHasNoVirtualPropertiesPopulated(
            [IgnoreVirtualMembers] UserWithSubstitute user1,
            UserWithSubstitute user2)
        {
            user1.Name.Should().BeNull();
            user1.Substitute.Should().BeNull();

            user2.Name.Should().BeNull();
            user2.Substitute.Should().BeNull();
        }

        [AutoData]
        [Theory(DisplayName = "GIVEN test method has value parameter WHEN  is decorated with IgnoreVirtualMembersAttribute THEN parameter value is being populated")]
        public void GivenTestMethodHasValueParameter_WhenIsDecoratedWithIgnoreVirtualMembersAttribute_ThenParameterValueIsBeingPopulated([IgnoreVirtualMembers] uint number)
        {
            number.Should().BeGreaterThanOrEqualTo(0);
        }

        [Fact(DisplayName = "GIVEN uninitialized parameter info WHEN GetCustomization is invoked THEN exception is thrown")]
        public void GivenUninitializedParameterInfo_WhenGetCustomizationIsInvoked_ThenExceptionIsThrown()
        {
            // Arrange
            const ParameterInfo parameterInfo = null;
            var attribute = new IgnoreVirtualMembersAttribute();

            // Act
            Func<object> act = () => attribute.GetCustomization(parameterInfo);

            // Assert
            act.Should().Throw<ArgumentNullException>()
                .And.ParamName.Should().Be("parameter");
        }

        public class Address
        {
            public string Street { get; set; }

            public string ZipCode { get; set; }

            public string City { get; set; }

            public string Country { get; set; }
        }

        public class User
        {
            public string Name { get; set; }

            public virtual Address Address { get; set; }
        }

        public class UserWithSubstitute : IUserWithSubstitute
        {
            public string Name { get; set; }

            public virtual User Substitute { get; set; }
        }
    }
}
