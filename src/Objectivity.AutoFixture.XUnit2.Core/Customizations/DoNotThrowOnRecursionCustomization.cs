namespace Objectivity.AutoFixture.XUnit2.Core.Customizations
{
    using System.Linq;

    using global::AutoFixture;

    using Objectivity.AutoFixture.XUnit2.Core.Common;

    public class DoNotThrowOnRecursionCustomization : ICustomization
    {
        public void Customize(IFixture fixture)
        {
            // Do not throw exception on circular references.
            fixture.NotNull(nameof(fixture))
                .Behaviors
                .OfType<ThrowingRecursionBehavior>()
                .ToList()
                .ForEach(b => fixture.Behaviors.Remove(b));
        }
    }
}
