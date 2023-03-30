namespace Objectivity.AutoFixture.XUnit2.Core.Tests.Attributes
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    using FluentAssertions;

    using global::AutoFixture;
    using global::AutoFixture.Kernel;
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
            IList<string> instanceWithoutCustomization,
            [EmptyCollection] IList<string> instanceWithCustomization)
        {
            // Arrange
            // Act
            // Assert
            instanceWithoutCustomization.Should().NotBeEmpty();
            instanceWithCustomization.Should().BeEmpty();
        }

        [Theory(DisplayName = "GIVEN CustomizeWith applied to the first argument WHEN data populated THEN all arguments has customization")]
        [AutoData]
        public void GivenCustomizeWithAppliedToTheFirstArgument_WhenDataPopulated_ThenAllArgumentsHasCustomization(
            [CustomizeWith(typeof(EmptyCollectionCustomization), typeof(IList<string>))] IList<string> instanceWithCustomization,
            IList<string> instanceWithoutCustomization)
        {
            // Arrange
            // Act
            // Assert
            instanceWithoutCustomization.Should().BeEmpty();
            instanceWithCustomization.Should().BeEmpty();
        }

        [Theory(DisplayName = "GIVEN CustomizeWith applied to the first argument of a cecrtain type WHEN data populated THEN only the first one has customization")]
        [AutoData]
        public void GivenCustomizeWithAppliedToTheFirstArgumentOfACecrtainType_WhenDataPopulated_ThenOnlyTheFirstOneHasCustomization(
            [EmptyCollection] IList<string> instanceWithCustomization,
            IList<int?> instanceOfDifferentTypeWithoutCustomization)
        {
            // Arrange
            // Act
            // Assert
            instanceWithCustomization.Should().BeEmpty();
            instanceOfDifferentTypeWithoutCustomization.Should().NotBeEmpty();
        }

        protected sealed class EmptyCollectionAttribute : CustomizeWithAttribute<EmptyCollectionCustomization>
        {
            public EmptyCollectionAttribute()
            {
                this.IncludeParameterType = true;
            }
        }

        protected class EmptyCollectionCustomization : ICustomization
        {
            public EmptyCollectionCustomization(Type reflectedType)
            {
                this.ReflectedType = reflectedType;
            }

            public Type ReflectedType { get; }

            public void Customize(IFixture fixture)
            {
                var emptyArray = Array.CreateInstance(this.ReflectedType.GenericTypeArguments.Single(), 0);

                fixture.Customizations.Add(
                    new FilteringSpecimenBuilder(
                        new FixedBuilder(emptyArray),
                        new ExactTypeSpecification(this.ReflectedType)));
            }
        }
    }
}
