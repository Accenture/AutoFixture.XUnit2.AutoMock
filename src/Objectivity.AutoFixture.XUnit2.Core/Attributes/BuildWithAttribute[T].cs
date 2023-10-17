namespace Objectivity.AutoFixture.XUnit2.Core.Attributes
{
    using System.Diagnostics.CodeAnalysis;
    using global::AutoFixture.Kernel;

    [SuppressMessage("Performance", "CA1813:Avoid unsealed attributes", Justification = "This attribute should be extendable by inheritance.")]
    public class BuildWithAttribute<T> : BuildWithAttribute
            where T : ISpecimenBuilder
    {
        public BuildWithAttribute(params object[] args)
            : base(typeof(T), args)
        {
        }
    }
}
