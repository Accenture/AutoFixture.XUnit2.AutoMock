namespace Objectivity.AutoFixture.XUnit2.Core.Requests
{
    using System;

    internal sealed class ExceptValuesRequest : ValuesRequest<ExceptValuesRequest>
    {
        public ExceptValuesRequest(Type operandType, params object[] values)
            : base(operandType, values)
        {
        }
    }
}
