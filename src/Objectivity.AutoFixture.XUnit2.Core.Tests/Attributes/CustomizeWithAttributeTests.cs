namespace Objectivity.AutoFixture.XUnit2.Core.Tests.Attributes
{
    using System;
    using System.Collections.Generic;
    using System.Diagnostics.CodeAnalysis;
    using System.Linq;
    using System.Reflection;

    using FluentAssertions;

    using global::AutoFixture;
    using global::AutoFixture.Kernel;
    using global::AutoFixture.Xunit2;

    using Objectivity.AutoFixture.XUnit2.Core.Attributes;
    using Objectivity.AutoFixture.XUnit2.Core.Customizations;

    using Xunit;

    [Collection("CustomizeWithAttribute")]
    [Trait("Category", "CustomizeAttribute")]
    public class CustomizeWithAttributeTests
    {
        public static TheoryData<bool, object[], int> ArgumentsDiscoveryCustomizationTestData { get; } = new()
        {
            { false, null, 0 },
            { false, Array.Empty<object>(), 0 },
            { false, new object[] { bool.TrueString }, 1 },
            { true, null, 1 },
            { true, Array.Empty<object>(), 1 },
            { true, new object[] { bool.TrueString }, 2 },
        };

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
            Func<object> act = () => customizeAttribute.GetCustomization(null);

            // Assert
            act.Should().Throw<MissingMethodException>();
        }

        [Fact(DisplayName = "GIVEN CustomizeWithAttribute with IncludeParameterType set WHEN GetCustomization without ParameterInfo is invoked THEN exception is thrown")]
        public void GivenAttributeWithIncludeParameterTypeSet_WhenGetCustomizationWithoutParameterInfoIsInvoked_ThenExceptionIsThrown()
        {
            // Arrange
            var customizationType = typeof(DoNotThrowOnRecursionCustomization);
            var customizeAttribute = new CustomizeWithAttribute(customizationType) { IncludeParameterType = true };

            // Act
            Func<object> act = () => customizeAttribute.GetCustomization(null);

            // Assert
            act.Should().Throw<ArgumentNullException>()
                .And.ParamName.Should().Be("parameter");
        }

        [Fact(DisplayName = "GIVEN uninitialized type WHEN constructor is invoked THEN exception is thrown")]
        public void GivenUninitializedType_WhenConstructorIsInvoked_ThenExceptionIsThrown()
        {
            // Arrange
            const Type customizationType = null;

            // Act
            Func<object> act = () => new CustomizeWithAttribute(customizationType);

            // Assert
            act.Should().Throw<ArgumentNullException>()
                .And.ParamName.Should().Be("type");
        }

        [Fact(DisplayName = "GIVEN unsupported type WHEN constructor is invoked THEN exception is thrown")]
        public void GivenUnsupportedType_WhenConstructorIsInvoked_ThenExceptionIsThrown()
        {
            // Arrange
            var customizationType = typeof(string);

            // Act
            Func<object> act = () => new CustomizeWithAttribute(customizationType);

            // Assert
            act.Should().Throw<ArgumentException>()
                .And.Message.Should().NotBeNullOrEmpty()
                .And.Contain(nameof(ICustomization));
        }

        [Fact(DisplayName = "GIVEN CustomizeWith attribute with IncludeParameterType set WHEN GetCustomization is invoked THEN customization with expected type is returned")]
        public void GivenCustomizeWithAttributeWithIncludeParameterTypeSet_WhenGetCustomizationIsInvoked_ThenCustomizationWithExpectedTypeIsReturned()
        {
            // Arrange
            var customizationType = typeof(ArgumentsDiscoveryCustomization);
            var customizeAttribute = new CustomizeWithAttribute(customizationType) { IncludeParameterType = true };
            var parameter = typeof(CustomizeWithAttributeTests)
                .GetMethod(nameof(this.MethodUnderTest), BindingFlags.Instance | BindingFlags.NonPublic)
                .GetParameters()
                .First();

            // Act
            var customization = customizeAttribute.GetCustomization(parameter) as ArgumentsDiscoveryCustomization;

            // Assert
            customization.Should().NotBeNull().And.BeAssignableTo(customizationType);
            customization.Args.Should().HaveCount(1).And.Subject.First().Should().Be(parameter.ParameterType);
        }

        [MemberData(nameof(ArgumentsDiscoveryCustomizationTestData))]
        [Theory(DisplayName = "GIVEN CustomizeWith attribute with arguments WHEN GetCustomization is invoked THEN expected customization is returned with expected number of arguments")]
        public void GivenCustomizeWithAttributeWithArguments_WhenGetCustomizationIsInvoked_ThenExpectedCustomizationIsReturnedWithExpectedNumberOfArguments(bool includeParameterType, object[] args, int expectedNumberOfArguments)
        {
            // Arrange
            var customizationType = typeof(ArgumentsDiscoveryCustomization);
            var customizeAttribute = new CustomizeWithAttribute(customizationType, args) { IncludeParameterType = includeParameterType };
            var parameter = typeof(CustomizeWithAttributeTests)
                .GetMethod(nameof(this.MethodUnderTest), BindingFlags.Instance | BindingFlags.NonPublic)
                .GetParameters()
                .First();

            // Act
            var customization = customizeAttribute.GetCustomization(parameter) as ArgumentsDiscoveryCustomization;

            // Assert
            customization.Should().NotBeNull().And.BeAssignableTo(customizationType);
            customization.Args.Should().HaveCount(expectedNumberOfArguments);
        }

        [AutoData]
        [Theory(DisplayName = "GIVEN CustomizeWith applied to the second argument WHEN data populated THEN only second one has customization")]
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

        [AutoData]
        [Theory(DisplayName = "GIVEN CustomizeWith applied to the first argument WHEN data populated THEN all arguments has customization")]
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

        [AutoData]
        [Theory(DisplayName = "GIVEN CustomizeWith applied to the first argument of a certain type WHEN data populated THEN only the first one has customization")]
        public void GivenCustomizeWithAppliedToTheFirstArgumentOfACertainType_WhenDataPopulated_ThenOnlyTheFirstOneHasCustomization(
            [EmptyCollection] IList<string> instanceWithCustomization,
            IList<int?> instanceOfDifferentTypeWithoutCustomization)
        {
            // Arrange
            // Act
            // Assert
            instanceWithCustomization.Should().BeEmpty();
            instanceOfDifferentTypeWithoutCustomization.Should().NotBeEmpty();
        }

        [SuppressMessage("Roslynator", "RCS1163:Unused parameter.", Justification = "Required for test.")]
        [SuppressMessage("Style", "IDE0060:Remove unused parameter", Justification = "Required for test.")]
        [SuppressMessage("ReSharper", "UnusedParameter.Global", Justification = "Required for test.")]
        protected void MethodUnderTest(bool parameter)
        {
            // Empty method under test
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

        protected class ArgumentsDiscoveryCustomization : ICustomization
        {
            public ArgumentsDiscoveryCustomization(params object[] args)
            {
                this.Args = args;
            }

            public IReadOnlyCollection<object> Args { get; }

            public void Customize(IFixture fixture)
            {
                // Method intentionally left empty.
            }
        }
    }
}
