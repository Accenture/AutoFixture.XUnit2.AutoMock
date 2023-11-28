namespace Objectivity.AutoFixture.XUnit2.Core.Requests
{
    using System;
    using System.Collections.Generic;
    using System.Globalization;
    using System.Linq;

    using Objectivity.AutoFixture.XUnit2.Core.Common;

    internal abstract class ValuesRequest<T> : IEquatable<T>
        where T : ValuesRequest<T>
    {
        private readonly HashSet<object> inputValues;
        private readonly Lazy<IReadOnlyCollection<object>> readonlyValues;

        protected ValuesRequest(Type operandType, params object[] values)
        {
            this.OperandType = operandType.NotNull(nameof(operandType));
            this.inputValues = new HashSet<object>(values.NotNull(nameof(values)));
            if (this.inputValues.Count == 0)
            {
                throw new ArgumentException("At least one value is expected to be specified.", nameof(values));
            }

            if (this.inputValues.GroupBy(x => x).Any(g => g.Count() > 1))
            {
                throw new ArgumentException("All values are expected to be unique.", nameof(values));
            }

            this.readonlyValues = new Lazy<IReadOnlyCollection<object>>(() => Array.AsReadOnly(this.inputValues.ToArray()));
        }

        public Type OperandType { get; }

        public IReadOnlyCollection<object> Values => this.readonlyValues.Value;

        public override bool Equals(object obj)
        {
            if (obj is T other)
            {
                return this.Equals(other);
            }

            return false;
        }

        public bool Equals(T other)
        {
            if (other is null)
            {
                return false;
            }

            return this.OperandType == other.OperandType
                && this.inputValues.SetEquals(other.Values);
        }

        public override int GetHashCode()
        {
            var hc = this.OperandType.GetHashCode() ^ typeof(T).GetHashCode();
            foreach (var inputValue in this.inputValues)
            {
                hc ^= inputValue.GetHashCode();
            }

            return hc;
        }

        public override string ToString()
        {
            var values = string.Join(", ", this.inputValues.Select(x => $"[{x.GetType().Name}] {x}"));
            return string.Format(
                CultureInfo.CurrentCulture,
                "{0} (OperandType: {1}, Values: {2}",
                this.GetType().Name,
                this.OperandType.FullName,
                values);
        }
    }
}
