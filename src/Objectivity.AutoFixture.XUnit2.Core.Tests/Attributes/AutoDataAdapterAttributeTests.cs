namespace Objectivity.AutoFixture.XUnit2.Core.Tests.Attributes
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;
    using FluentAssertions;
    using global::AutoFixture;
    using Objectivity.AutoFixture.XUnit2.Core.Attributes;
    using Xunit;

    [Collection("AutoDataAdapterAttribute")]
    [Trait("Category", "Attributes")]
    public class AutoDataAdapterAttributeTests
    {
        [Fact(DisplayName = "GIVEN fixture WHEN constructor is invoked THEN passed fixture is being adapted")]
        public void GivenFixture_WhenConstructorIsInvoked_ThenPassedFixtureIsBeingAdapted()
        {
            // Arrange
            IFixture fixture = new Fixture();

            // Act
            var attribute = new AutoDataAdapterAttribute(fixture);

            // Assert
            attribute.AdaptedFixture.Should().Be(fixture);
        }

        [Fact(DisplayName = "GIVEN uninitialized fixture WHEN constructor is invoked THEN exception is thrown")]
        public void GivenUninitializedFixture_WhenConstructorIsInvoked_ThenExceptionIsThrown()
        {
            // Arrange
            const IFixture fixture = null;

            // Act
            // Assert
            Assert.Throws<ArgumentNullException>(() => new AutoDataAdapterAttribute(fixture));
        }
    }
}
