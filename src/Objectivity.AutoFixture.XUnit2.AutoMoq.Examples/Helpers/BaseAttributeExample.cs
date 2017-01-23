namespace Objectivity.AutoFixture.XUnit2.AutoMoq.Examples.Helpers
{
    using Xunit.Abstractions;

    public class BaseAttributeExample
    {
        public BaseAttributeExample(ITestOutputHelper output)
        {
            this.Output = output;
        }

        protected ITestOutputHelper Output { get; }
    }
}
