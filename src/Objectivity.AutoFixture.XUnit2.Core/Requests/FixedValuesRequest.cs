namespace Objectivity.AutoFixture.XUnit2.Core.Requests
{
    using System;
    using System.Collections.Generic;
    using System.Globalization;
    using System.Linq;

    using Objectivity.AutoFixture.XUnit2.Core.Common;

    internal class FixedValuesRequest : IEquatable<FixedValuesRequest>
    {
        private readonly object[] inputValues;
        private readonly Lazy<IReadOnlyCollection<object>> readonlyValues;

        public FixedValuesRequest(Type operandType, params object[] values)
        {
            this.OperandType = operandType.NotNull(nameof(operandType));
            this.inputValues = values.NotNull(nameof(values));
            if (this.inputValues.Length == 0)
            {
                throw new ArgumentException("At least one value is expected to be specified.", nameof(values));
            }

            if (Array.Exists(this.inputValues, x => x is not IComparable))
            {
                throw new ArgumentException("All values are expected to be comparable.", nameof(values));
            }

            this.readonlyValues = new Lazy<IReadOnlyCollection<object>>(() => Array.AsReadOnly(this.inputValues));
        }

        public Type OperandType { get; }

        public IReadOnlyCollection<object> Values => this.readonlyValues.Value;

        public override bool Equals(object obj)
        {
            if (obj is FixedValuesRequest other)
            {
                return this.Equals(other);
            }

            return false;
        }

        public bool Equals(FixedValuesRequest other)
        {
            if (other is null)
            {
                return false;
            }

            return this.OperandType == other.OperandType
                && this.inputValues.SequenceEqual(other.Values);
        }

        public override int GetHashCode()
        {
            var hc = this.OperandType.GetHashCode();
            foreach (var value in this.inputValues)
            {
                hc ^= value.GetHashCode();
            }

            return hc;
        }

        public override string ToString()
        {
            var values = string.Join(", ", this.inputValues.Select(x => $"[{x.GetType().Name}] {x}"));
            return string.Format(
                CultureInfo.CurrentCulture,
                "FixedValuesRequest (OperandType: {0}, Values: {1}",
                this.OperandType.FullName,
                values);
        }
    }
}
