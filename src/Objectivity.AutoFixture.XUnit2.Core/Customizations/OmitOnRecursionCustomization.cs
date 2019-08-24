namespace Objectivity.AutoFixture.XUnit2.Core.Customizations
{
    using global::AutoFixture;
    using Objectivity.AutoFixture.XUnit2.Core.Common;

    public class OmitOnRecursionCustomization : ICustomization
    {
        public void Customize(IFixture fixture)
        {
            // Omit recursion on first level.
            fixture.NotNull(nameof(fixture))
                .Behaviors
                .Add(new OmitOnRecursionBehavior());
        }
    }
}