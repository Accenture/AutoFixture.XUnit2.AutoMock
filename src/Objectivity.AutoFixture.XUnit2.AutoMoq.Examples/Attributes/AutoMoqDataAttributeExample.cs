namespace Objectivity.AutoFixture.XUnit2.AutoMoq.Examples.Attributes
{
    using System;
    using AutoMoq.Attributes;
    using ExampleClasses;
    using FluentAssertions;
    using Xunit;
    using Xunit.Abstractions;

    public class AutoMoqDataAttributeExample
    {
        private readonly ITestOutputHelper output;

        public AutoMoqDataAttributeExample(ITestOutputHelper output)
        {
            this.output = output;
        }

        [Theory]
        [AutoMoqData]
        public void SimpleTypeGeneration(string name, int number)
        {
            output.WriteLine($"Name: {name}");
            output.WriteLine($"Number: {number}");
        }

        [Theory]
        [AutoMoqData]
        public void ComplexTypeGeneration(Sample testObject)
        {
            output.WriteLine($"Not Virtual Property: {testObject.NotVirtualProperty}");

            testObject.VirtualProperty.Should().NotBeNull();
            output.WriteLine($"Virtual Property: {testObject.VirtualProperty}");
        }

        [Theory]
        [AutoMoqData(IgnoreVirtualMembers = true)]
        public void ComplexTypeGenerationWithIgnoreVirtualMembers(Sample testObject)
        {
            output.WriteLine($"Not Virtual Property: {testObject.NotVirtualProperty}");

            testObject.VirtualProperty.Should().BeNull();
            output.WriteLine($"Virtual Property: {testObject.VirtualProperty}");
        }
    }
}
