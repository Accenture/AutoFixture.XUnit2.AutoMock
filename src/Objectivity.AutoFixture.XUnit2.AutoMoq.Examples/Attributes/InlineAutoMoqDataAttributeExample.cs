namespace Objectivity.AutoFixture.XUnit2.AutoMoq.Examples.Attributes
{
    using System;
    using AutoMoq.Attributes;
    using FluentAssertions;
    using Helpers;
    using Xunit;
    using Xunit.Abstractions;

    [Collection("InlineAutoMoqDataAttribute")]
    [Trait("Category", "Samples")]
    public class InlineAutoMoqDataAttributeExample : BaseAttributeExample
    {

        public InlineAutoMoqDataAttributeExample(ITestOutputHelper output) : base(output)
        {
        }

        [Theory]
        [InlineAutoMoqData("name1")]
        [InlineAutoMoqData("name2", 1)]
        [InlineAutoMoqData("name3", 2, 1.0f)]
        [InlineAutoMoqData()]
        public void SimpleTypeGeneration(string name, int number, double otherNumber)
        {
            Output.WriteLine($"Name: {name}");
            Output.WriteLine($"Number: {number}");
            Output.WriteLine($"Other number: {otherNumber}");
        }

        [Theory]
        [InlineAutoMoqData("name1")]
        [InlineAutoMoqData("name2", 1)]
        [InlineAutoMoqData()]
        public void ComplexTypeGeneration(string name, int number, SampleClass testObject)
        {
            Output.WriteLine($"Name: {name}");
            Output.WriteLine($"Number: {number}");

            Output.WriteLine($"Not Virtual Property: {testObject.NotVirtualProperty}");
            testObject.VirtualProperty.Should().NotBeNull();
            Output.WriteLine($"Virtual Property: {testObject.VirtualProperty}");
        }

        [Theory]
        [InlineAutoMoqData("name1", IgnoreVirtualMembers = true)]
        [InlineAutoMoqData("name2", 1, IgnoreVirtualMembers = true)]
        [InlineAutoMoqData(IgnoreVirtualMembers = true)]
        public void ComplexTypeGenerationWithIgnoreVirtualMembers(string name, int number, SampleClass testObject)
        {
            Output.WriteLine($"Name: {name}");
            Output.WriteLine($"Number: {number}");

            Output.WriteLine($"Not Virtual Property: {testObject.NotVirtualProperty}");
            testObject.VirtualProperty.Should().BeNull();
            Output.WriteLine($"Virtual Property: {testObject.VirtualProperty}");
        }
    }
}
