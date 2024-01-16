namespace Objectivity.AutoFixture.XUnit2.AutoMoq.Tests
{
    using System.Diagnostics.CodeAnalysis;

    public interface IFakeObjectUnderTest
    {
        [SuppressMessage("ReSharper", "UnusedMember.Global", Justification = "Required for test.")]
        public string StringProperty { get; set; }
    }
}
