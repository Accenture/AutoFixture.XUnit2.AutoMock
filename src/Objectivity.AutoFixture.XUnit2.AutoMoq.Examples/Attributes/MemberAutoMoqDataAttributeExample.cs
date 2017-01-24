namespace Objectivity.AutoFixture.XUnit2.AutoMoq.Examples.Attributes
{
    using System.Collections.Generic;
    using AutoMoq.Attributes;
    using FluentAssertions;
    using Helpers;
    using Xunit;
    using Xunit.Abstractions;

    public class MemberAutoMoqDataAttributeExample : BaseAttributeExample
    {
        public MemberAutoMoqDataAttributeExample(ITestOutputHelper output) : base(output)
        {
        }

        public static IEnumerable<object[]> SimpleData { get; } = new List<object[]>
        {
            new object[] {"name1", 1},
            new object[] {"name2"},
            new object[] {"name3"},
            new object[] {"name4"}
        };

        public static IEnumerable<object[]> ComplexData { get; } = new List<object[]>
        {
            new object[]
            {
                "name1", 1, new SampleClass
                {
                    NotVirtualProperty = "va1",
                    VirtualProperty = "val2"

                }
            },
            new object[] {"name2", 2},
            new object[] {"name3"},
            new object[] {"name4"}
        };

        public static IEnumerable<object[]> ComplexDataWithIgnoreVirtualMembers { get; } = new List<object[]>
        {
            new object[]
            {
                "name1", 1, new SampleClass
                {
                    NotVirtualProperty = "val1",
                    VirtualProperty = null

                }
            },
            new object[] {"name2", 2},
            new object[] {"name3"},
            new object[] {"name4"}
        };

        [Theory]
        [MemberAutoMoqData("SimpleData", ShareFixture = true)]

        public void SimpleTypeGeneration(string name, int number, double otherNumber)
        {
            Output.WriteLine($"Name: {name}");
            Output.WriteLine($"Number: {number}");
            Output.WriteLine($"Other number: {otherNumber}");
        }

        [Theory]
        [MemberAutoMoqData("ComplexData", IgnoreVirtualMembers = false, ShareFixture = false)]
        public void ComplexTypeGeneration(string name, int number, SampleClass testObject)
        {
            Output.WriteLine($"Name: {name}");
            Output.WriteLine($"Number: {number}");

            Output.WriteLine($"Not Virtual Property: {testObject.NotVirtualProperty}");
            testObject.VirtualProperty.Should().NotBeNull();
            Output.WriteLine($"Virtual Property: {testObject.VirtualProperty}");
        }

        [Theory]
        [MemberAutoMoqData("ComplexDataWithIgnoreVirtualMembers", IgnoreVirtualMembers = true)]
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