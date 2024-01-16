namespace Objectivity.AutoFixture.XUnit2.Core.Common
{
    using System;
    using System.Collections;
    using System.Diagnostics.CodeAnalysis;

    [SuppressMessage("Design", "CA1010:Generic interface should also be implemented", Justification = "It is not necessary for internal design.")]
    internal sealed class RoundRobinEnumerable<T> : IEnumerator
    {
        private const int InitialPosition = -1;
        private const int FirstElementPosition = 0;

        private readonly T[] values;
        private readonly int maxPosition;
        private int position = InitialPosition;

        public RoundRobinEnumerable(params T[] values)
        {
            this.values = values.NotNull(nameof(values));
            if (values.Length == 0)
            {
                throw new ArgumentException("At least one value is expected to be specified.", nameof(values));
            }

            this.maxPosition = values.Length - 1;
        }

        public T Current
        {
            get
            {
                try
                {
                    return this.values[this.position];
                }
                catch (IndexOutOfRangeException)
                {
                    throw new InvalidOperationException("Enumerator is at its initial position thus cannot return any value.");
                }
            }
        }

        object IEnumerator.Current => this.Current;

        public bool MoveNext()
        {
            this.position = this.position >= this.maxPosition
                ? FirstElementPosition
                : this.position + 1;

            return true;
        }

        public void Reset()
        {
            this.position = InitialPosition;
        }
    }
}
