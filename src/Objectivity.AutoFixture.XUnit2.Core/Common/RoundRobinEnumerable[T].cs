namespace Objectivity.AutoFixture.XUnit2.Core.Common
{
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.Diagnostics.CodeAnalysis;

    internal sealed class RoundRobinEnumerable<T> : IEnumerable<T>
    {
        private readonly IEnumerable<T> values;

        public RoundRobinEnumerable(params T[] values)
        {
            this.values = values.NotNull(nameof(values));
            if (values.Length == 0)
            {
                throw new ArgumentException("At least one value is expected to be specified.", nameof(values));
            }
        }

        [SuppressMessage("Blocker Bug", "S2190:Loops and recursions should not be infinite", Justification = "This is a round robin implementation.")]
        [SuppressMessage("ReSharper", "IteratorNeverReturns", Justification = "This is a round robin implementation.")]
        public IEnumerator<T> GetEnumerator()
        {
            while (true)
            {
                foreach (var @value in this.values)
                {
                    yield return @value;
                }
            }
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return this.GetEnumerator();
        }
    }
}
