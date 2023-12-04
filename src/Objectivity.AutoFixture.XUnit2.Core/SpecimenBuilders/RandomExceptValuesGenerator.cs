namespace Objectivity.AutoFixture.XUnit2.Core.SpecimenBuilders
{
    using System;
    using System.Collections.Concurrent;
    using System.Diagnostics.CodeAnalysis;
    using System.Linq;

    using global::AutoFixture;
    using global::AutoFixture.Kernel;
    using Objectivity.AutoFixture.XUnit2.Core.Common;
    using Objectivity.AutoFixture.XUnit2.Core.Requests;

    internal class RandomExceptValuesGenerator : ISpecimenBuilder
    {
        [SuppressMessage("Major Bug", "S2583:Conditionally executed code should be reachable", Justification = "Analyzer issue as the code is reachable")]
        public object Create(object request, ISpecimenContext context)
        {
            if (context is null)
            {
                throw new ArgumentNullException(nameof(context));
            }

            if (request.NotNull(nameof(request)) is ExceptValuesRequest exceptValuesRequest)
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
                        throw new ObjectCreationException("The value could not be created. Probably all possible values were excluded.");
                    }
                }
                while (exceptValuesRequest.Values.Contains(result));

                return result;
            }

            return new NoSpecimen();
        }
    }
}
