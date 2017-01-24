namespace Objectivity.AutoFixture.XUnit2.AutoMoq.Examples.Attributes
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
