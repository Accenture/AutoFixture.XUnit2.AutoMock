namespace Objectivity.AutoFixture.XUnit2.Core.Requests
{
    using System;

    internal sealed class FixedValuesRequest : ValuesRequest<FixedValuesRequest>
    {
        public FixedValuesRequest(Type operandType, params object[] values)
            : base(operandType, values)
        {
        }
    }
}
