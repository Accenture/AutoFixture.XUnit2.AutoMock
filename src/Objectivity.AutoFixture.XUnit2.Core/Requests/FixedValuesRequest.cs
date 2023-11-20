namespace Objectivity.AutoFixture.XUnit2.Core.Requests
{
    using System;

    internal sealed class FixedValuesRequest : ValuesRequest
    {
        public FixedValuesRequest(Type operandType, params object[] values)
            : base(operandType, values)
        {
        }

        public override bool Equals(object obj)
        {
            if (obj is FixedValuesRequest other)
            {
                return this.Equals(other);
            }

            return false;
        }

        public override int GetHashCode()
        {
            return base.GetHashCode() ^ typeof(FixedValuesRequest).GetHashCode();
        }
    }
}
