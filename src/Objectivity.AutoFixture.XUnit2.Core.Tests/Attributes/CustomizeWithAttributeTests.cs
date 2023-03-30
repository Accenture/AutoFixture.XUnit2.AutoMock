namespace Objectivity.AutoFixture.XUnit2.Core.Tests.Attributes
{
    using System;
    using System.Diagnostics.CodeAnalysis;

    using FluentAssertions;

    using global::AutoFixture;
    using global::AutoFixture.Xunit2;
    using Objectivity.AutoFixture.XUnit2.Core.Attributes;
    using Objectivity.AutoFixture.XUnit2.Core.Customizations;

    using Xunit;

    [Collection("CustomizeWithAttribute")]
    [Trait("Category", "Attributes")]
    public class CustomizeWithAttributeTests
    {
        [Fact(DisplayName = "GIVEN customization type with no arguments WHEN GetCustomization is invoked THEN customization instance is returned")]
        public void GivenCustomizationTypeWithNoArguments_WhenGetCustomizationIsInvoked_ThenCustomizationInstanceIsReturned()
        {
            // Arrange
            var customizationType = typeof(DoNotThrowOnRecursionCustomization);
            var customizeAttribute = new CustomizeWithAttribute(customizationType);

            // Act
            var customization = customizeAttribute.GetCustomization(null);

            // Assert
            customization.Should().NotBeNull().And.BeAssignableTo(customizationType);
        }

        [Fact(DisplayName = "GIVEN customization type with arguments WHEN GetCustomization is invoked THEN customization instance is returned")]
        public void GivenCustomizationTypeWithArguments_WhenGetCustomizationIsInvoked_ThenCustomizationInstanceIsReturned()
        {
            // Arrange
            var customizationType = typeof(AutoDataCommonCustomization);
            var customizeAttribute = new CustomizeWithAttribute(customizationType, true);

            // Act
            var customization = customizeAttribute.GetCustomization(null);

            // Assert
            customization.Should().NotBeNull().And.BeAssignableTo(customizationType);
        }

        [Fact(DisplayName = "GIVEN customization type requiring arguments without any WHEN GetCustomization is invoked THEN exception is thrown")]
        public void GivenCustomizationTypeRequiringArgumentsWithoutAny_WhenGetCustomizationIsInvoked_ThenExceptionIsThrown()
        {
            // Arrange
            var customizationType = typeof(AutoDataCommonCustomization);
            var customizeAttribute = new CustomizeWithAttribute(customizationType);

            // Act
            // Assert
            Assert.Throws<MissingMethodException>(() => customizeAttribute.GetCustomization(null));
        }

        [Fact(DisplayName = "GIVEN CustomizeWithAttribute with IncludeParameterType set WHEN GetCustomization without ParameterInfo is invoked THEN exception is thrown")]
        public void GivenAttributeWithIncludeParameterTypeSet_WhenGetCustomizationWithoutParameterInfoIsInvoked_ThenExceptionIsThrown()
        {
            // Arrange
            var customizationType = typeof(DoNotThrowOnRecursionCustomization);
            var customizeAttribute = new CustomizeWithAttribute(customizationType) { IncludeParameterType = true };

            // Act
            // Assert
            Assert.Throws<ArgumentNullException>(() => customizeAttribute.GetCustomization(null));
        }

        [Fact(DisplayName = "GIVEN uninitialized type WHEN constructor is invoked THEN exception is thrown")]
        public void GivenUninitializedType_WhenConstructorIsInvoked_ThenExceptionIsThrown()
        {
            // Arrange
            const Type customizationType = null;

            // Act
            // Assert
            Assert.Throws<ArgumentNullException>(() => new CustomizeWithAttribute(customizationType));
        }

        [Fact(DisplayName = "GIVEN unsupported type WHEN constructor is invoked THEN exception is thrown")]
        public void GivenUnsupportedType_WhenConstructorIsInvoked_ThenExceptionIsThrown()
        {
            // Arrange
            var customizationType = typeof(string);

            // Act
            // Assert
            Assert.Throws<ArgumentException>(() => new CustomizeWithAttribute(customizationType));
        }

        [Theory(DisplayName = "GIVEN CustomizeWith applied to the second argument WHEN data populated THEN only second one has customization")]
        [AutoData]
        public void GivenCustomizeWithAppliedToTheSecondArgument_WhenDataPopulated_ThenOnlySecondOneHasCustomization(
            PropertyHolder<string> stringWithoutCustomization,
            [NoAutoProperties] PropertyHolder<string> stringWithCustomization,
            PropertyHolder<int?> intWithoutCustomization,
            [NoAutoProperties] PropertyHolder<int?> intWithCustomization)
        {
            // Arrange
            // Act
            // Assert
            stringWithoutCustomization.Property.Should().NotBeNull();
            stringWithCustomization.Property.Should().BeNull();
            intWithoutCustomization.Property.Should().NotBeNull();
            intWithCustomization.Property.Should().BeNull();
        }

        [Theory(DisplayName = "GIVEN CustomizeWith applied to the first argument WHEN data populated THEN all arguments has customization")]
        [AutoData]
        public void GivenCustomizeWithAppliedToTheFirstArgument_WhenDataPopulated_ThenAllArgumentsHasCustomization(
            [CustomizeWith(typeof(NoAutoPropertiesCustomization), typeof(PropertyHolder<string>))] PropertyHolder<string> instanceWithCustomization,
            PropertyHolder<string> instanceWithoutCustomization)
        {
            // Arrange
            // Act
            // Assert
            instanceWithoutCustomization.Property.Should().BeNull();
            instanceWithCustomization.Property.Should().BeNull();
        }

        [SuppressMessage("Design", "CA1034:Nested types should not be visible", Justification = "Test objects")]
        public class PropertyHolder<T>
        {
            public T Property { get; set; }
        }

        protected class NoAutoPropertiesAttribute : CustomizeWithAttribute<NoAutoPropertiesCustomization>
        {
            public NoAutoPropertiesAttribute()
            {
                this.IncludeParameterType = true;
            }
        }
    }
}
