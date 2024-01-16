namespace Objectivity.AutoFixture.XUnit2.Core.SpecimenBuilders
{
    using System;
    using System.Collections;
    using System.Reflection;

    using global::AutoFixture.Kernel;

    using Objectivity.AutoFixture.XUnit2.Core.Common;

    internal sealed class RequestFactoryRelay : ISpecimenBuilder
    {
        private readonly Func<Type, object> requestFactory;

        public RequestFactoryRelay(Func<Type, object> requestFactory)
        {
            this.requestFactory = requestFactory.NotNull(nameof(requestFactory));
        }

        public object Create(object request, ISpecimenContext context)
        {
            if (context is null)
            {
                throw new ArgumentNullException(nameof(context));
            }

            if (request.NotNull(nameof(request)) is ParameterInfo parameterInfo)
            {
                var parameterType = Nullable.GetUnderlyingType(parameterInfo.ParameterType)
                    ?? parameterInfo.ParameterType;

                return parameterType != typeof(string)
                    && parameterType.TryGetEnumerableSingleTypeArgument(out var itemType)
                    ? this.CreateMultiple(parameterType, itemType, context)
                    : this.CreateSingle(parameterType, context);
            }

            return new NoSpecimen();
        }

        private object CreateSingle(Type type, ISpecimenContext context)
        {
            var transformedRequest = this.requestFactory(type);
            return transformedRequest is not null
                ? context.Resolve(transformedRequest)
                : new NoSpecimen();
        }

        private object CreateMultiple(Type collectionType, Type itemType, ISpecimenContext context)
        {
            var transformedRequest = this.requestFactory(itemType);
            if (transformedRequest is not null
                && context.Resolve(new MultipleRequest(transformedRequest)) is IEnumerable elements)
            {
                var items = elements.ToTypedArray(itemType);
                return collectionType.IsArray || collectionType.IsAbstract
                    ? items
                    : Activator.CreateInstance(collectionType, items);
            }

            return new NoSpecimen();
        }
    }
}
