namespace Objectivity.AutoFixture.XUnit2.Core.SpecimenBuilders
{
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.Diagnostics.CodeAnalysis;
    using System.Linq;

    using global::AutoFixture.Kernel;

    using Objectivity.AutoFixture.XUnit2.Core.Common;
    using Objectivity.AutoFixture.XUnit2.Core.Requests;

    internal class RandomFixedValuesGenerator : ISpecimenBuilder
    {
        private readonly Dictionary<Type, IEnumerator> enumerators = new();
        private readonly object syncRoot = new();

        public object Create(object request, ISpecimenContext context)
        {
            if (request is null)
            {
                throw new ArgumentNullException(nameof(request));
            }

            if (context is null)
            {
                throw new ArgumentNullException(nameof(context));
            }

            if (request is FixedValuesRequest fixedValuesRequest)
            {
                lock (this.syncRoot)
                {
                    return this.CreateValue(fixedValuesRequest);
                }
            }

            return new NoSpecimen();
        }

        private object CreateValue(FixedValuesRequest request)
        {
            var generator = this.EnsureGenerator(request);
            generator.MoveNext();
            return generator.Current;
        }

        [SuppressMessage("Security", "CA5394:Do not use insecure randomness", Justification = "It is good enought for collection randomisation.")]
        private IEnumerator EnsureGenerator(FixedValuesRequest request)
        {
            if (!this.enumerators.TryGetValue(request.OperandType, out var enumerator))
            {
                var random = new Random();
                var values = request.Values.OrderBy((_) => random.Next()).ToArray();
                enumerator = new RoundRobinEnumerable<object>(values).GetEnumerator();
                this.enumerators.Add(request.OperandType, enumerator);
            }

            return enumerator;
        }
    }
}
