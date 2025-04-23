namespace Objectivity.AutoFixture.XUnit2.Core.Tests.Attributes
{
    using System;
    using System.Diagnostics.CodeAnalysis;
    using System.Reflection;
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
            Assert.Null(userWithSubstitute.Name);
            Assert.Null(userWithSubstitute.Substitute);

            Assert.NotNull(user.Name);
            Assert.NotNull(user.Address);
        }

        [AutoData]
        [Theory(DisplayName = "GIVEN test method has parameters of various types WHEN the one without interface is decorated with IgnoreVirtualMembersAttribute THEN only that parameter has no virtual properties populated")]
        public void GivenTestMethodHasParametersOfVariousTypes_WhenOneIsDecoratedWithIgnoreVirtualMembersAttribute_ThenOnlyThatParameterHasNoVirtualPropertiesPopulated(
            UserWithSubstitute userWithSubstitute,
            [IgnoreVirtualMembers] User user)
        {
            Assert.NotNull(userWithSubstitute.Name);
            Assert.NotNull(userWithSubstitute.Substitute);

            Assert.NotNull(user.Name);
            Assert.Null(user.Address);
        }

        [AutoData]
        [Theory(DisplayName = "GIVEN test method has parameters of same type WHEN second one is decorated with IgnoreVirtualMembersAttribute THEN that parameter and following ones have no virtual properties populated")]
        public void GivenTestMethodHasParametersOfSameType_WhenSecondOneIsDecoratedWithIgnoreVirtualMembersAttribute_ThenThatParameterAndFollowingOnesHaveNoVirtualPropertiesPopulated(
            UserWithSubstitute user1,
            [IgnoreVirtualMembers] UserWithSubstitute user2,
            UserWithSubstitute user3)
        {
            Assert.NotNull(user1.Name);
            Assert.NotNull(user1.Substitute);

            Assert.Null(user2.Name);
            Assert.Null(user2.Substitute);

            Assert.Null(user3.Name);
            Assert.Null(user3.Substitute);
        }

        [AutoData]
        [Theory(DisplayName = "GIVEN test method has parameters of same type WHEN first one is decorated with IgnoreVirtualMembersAttribute THEN all parameters has no virtual properties populated")]
        public void GivenTestMethodHasParametersOfSameType_WhenFirstOneIsDecoratedWithIgnoreVirtualMembersAttribute_ThenAllParametersHasNoVirtualPropertiesPopulated(
            [IgnoreVirtualMembers] UserWithSubstitute user1,
            UserWithSubstitute user2)
        {
            Assert.Null(user1.Name);
            Assert.Null(user1.Substitute);

            Assert.Null(user2.Name);
            Assert.Null(user2.Substitute);
        }

        [AutoData]
        [Theory(DisplayName = "GIVEN test method has value parameter WHEN is decorated with IgnoreVirtualMembersAttribute THEN parameter value is being populated")]
        public void GivenTestMethodHasValueParameter_WhenIsDecoratedWithIgnoreVirtualMembersAttribute_ThenParameterValueIsBeingPopulated([IgnoreVirtualMembers] int number)
        {
            Assert.NotEqual(default, number);
        }

        [Fact(DisplayName = "GIVEN uninitialized parameter info WHEN GetCustomization is invoked THEN exception is thrown")]
        public void GivenUninitializedParameterInfo_WhenGetCustomizationIsInvoked_ThenExceptionIsThrown()
        {
            // Arrange
            const ParameterInfo parameterInfo = null;
            var attribute = new IgnoreVirtualMembersAttribute();

            // Act
            object Act() => attribute.GetCustomization(parameterInfo);

            // Assert
            var exception = Assert.Throws<ArgumentNullException>(Act);
            Assert.Equal("parameter", exception.ParamName);
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
