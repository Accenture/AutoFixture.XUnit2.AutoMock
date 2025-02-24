namespace Objectivity.AutoFixture.XUnit2.AutoNSubstitute.Tests
{
    using System.Diagnostics.CodeAnalysis;

    public interface IFakeObjectUnderTest
    {
        [SuppressMessage("ReSharper", "UnusedMember.Global", Justification = "Required for test.")]
        string StringProperty { get; set; }
    }
}
