namespace Objectivity.AutoFixture.XUnit2.Core.SpecimenBuilders
{
    using System;
    using System.Collections;
    using System.Collections.Concurrent;
    using System.Diagnostics.CodeAnalysis;
    using System.Linq;

    using global::AutoFixture.Kernel;

    using Objectivity.AutoFixture.XUnit2.Core.Common;
    using Objectivity.AutoFixture.XUnit2.Core.Requests;

    internal sealed class RandomFixedValuesGenerator : ISpecimenBuilder
    {
        private readonly ConcurrentDictionary<FixedValuesRequest, IEnumerator> enumerators = new();

        public object Create(object request, ISpecimenContext context)
        {
            if (context is null)
            {
                throw new ArgumentNullException(nameof(context));
            }

            if (request.NotNull(nameof(request)) is FixedValuesRequest fixedValuesRequest)
            {
                return this.CreateValue(fixedValuesRequest);
            }

            return new NoSpecimen();
        }

        [SuppressMessage("Security", "CA5394:Do not use insecure randomness", Justification = "It is good enough for collection randomization.")]
        private static IEnumerator CreateEnumerable(FixedValuesRequest request)
        {
            var random = new Random();

            // Stryker disable once all : mutating ordering by random still brings random results
            var values = request.Values.OrderBy((_) => random.Next()).ToArray();

            return new RoundRobinEnumerable<object>(values);
        }

        private object CreateValue(FixedValuesRequest request)
        {
            var generator = this.enumerators.GetOrAdd(request, CreateEnumerable);
            generator.MoveNext();

            return generator.Current;
        }
    }
}
