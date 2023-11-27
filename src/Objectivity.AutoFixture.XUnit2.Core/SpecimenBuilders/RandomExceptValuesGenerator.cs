namespace Objectivity.AutoFixture.XUnit2.Core.SpecimenBuilders
{
    using System;
    using System.Collections.Concurrent;
    using System.Diagnostics.CodeAnalysis;
    using System.Linq;

    using global::AutoFixture;
    using global::AutoFixture.Kernel;

    using Objectivity.AutoFixture.XUnit2.Core.Requests;

    internal class RandomExceptValuesGenerator : ISpecimenBuilder
    {
        [SuppressMessage("Major Bug", "S2583:Conditionally executed code should be reachable", Justification = "Analyser issue as the code is reachable")]
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

            if (request is ExceptValuesRequest exceptValuesRequest)
            {
                var duplicateLimiter = new ConcurrentDictionary<object, bool>();
                object result;

                do
                {
                    result = context.Resolve(exceptValuesRequest.OperandType);
                    var hasDuplicate = duplicateLimiter.AddOrUpdate(
                        result,
                        (_) => false,
                        (_, __) => true);
                    if (hasDuplicate)
                    {
                        throw new ObjectCreationException("The other value could not be created.");
                    }
                }
                while (exceptValuesRequest.Values.Contains(result));

                return result;
            }

            return new NoSpecimen();
        }
    }
}
