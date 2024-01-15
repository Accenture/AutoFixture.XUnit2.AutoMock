namespace Objectivity.AutoFixture.XUnit2.AutoFakeItEasy.Tests
{
    using System.Diagnostics.CodeAnalysis;

    public interface IFakeObjectUnderTest
    {
        [SuppressMessage("ReSharper", "UnusedMember.Global", Justification = "Required for test.")]
        public string StringProperty { get; set; }
    }
}
