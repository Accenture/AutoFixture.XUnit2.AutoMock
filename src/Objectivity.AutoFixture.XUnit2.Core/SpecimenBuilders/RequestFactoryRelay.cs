namespace Objectivity.AutoFixture.XUnit2.Core.SpecimenBuilders
{
    using System;
    using System.Collections;
    using System.Reflection;

    using global::AutoFixture;
    using global::AutoFixture.Kernel;

    using Objectivity.AutoFixture.XUnit2.Core.Common;

    internal sealed class RequestFactoryRelay : ISpecimenBuilder
    {
        public RequestFactoryRelay(Func<Type, object> requestFactory)
        {
            this.RequestFactory = requestFactory.NotNull(nameof(requestFactory));
        }

        public Func<Type, object> RequestFactory { get; }

        public object Create(object request, ISpecimenContext context)
        {
            if (context is null)
            {
                throw new ArgumentNullException(nameof(context));
            }

            if (request.NotNull(nameof(request)) is ParameterInfo parameterInfo)
            {
                var type = Nullable.GetUnderlyingType(parameterInfo.ParameterType)
                    ?? parameterInfo.ParameterType;

                if (type.TryGetEnumerableSingleTypeArgument(out var itemType))
                {
                    var transformedRequest = this.RequestFactory(itemType);
                    var specimen = context.Resolve(new MultipleRequest(transformedRequest));

                    if (specimen is IEnumerable elements)
                    {
                        var items = elements.ToTypedArray(itemType);
                        return type.IsArray || type.IsAbstract
                            ? items
                            : Activator.CreateInstance(type, items);
                    }
                }
                else
                {
                    var transformedRequest = this.RequestFactory(type);
                    return context.Resolve(transformedRequest);
                }
            }

            return new NoSpecimen();
        }
    }
}
