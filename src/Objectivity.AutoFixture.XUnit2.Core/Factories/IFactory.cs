namespace Objectivity.AutoFixture.XUnit2.Core.Factories
{
    internal interface IFactory<in TInput, out TOutput>
    {
        TOutput Create(TInput input);
    }
}
