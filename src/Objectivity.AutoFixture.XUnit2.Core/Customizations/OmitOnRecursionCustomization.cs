namespace Objectivity.AutoFixture.XUnit2.Core.Customizations
{
    using Common;
    using global::AutoFixture;

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