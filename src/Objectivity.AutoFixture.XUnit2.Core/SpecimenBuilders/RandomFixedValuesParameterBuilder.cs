namespace Objectivity.AutoFixture.XUnit2.Core.SpecimenBuilders
{
    using System;
    using System.Collections;
    using System.Collections.Generic;

    using global::AutoFixture;
    using global::AutoFixture.Kernel;

    using Objectivity.AutoFixture.XUnit2.Core.Common;
    using Objectivity.AutoFixture.XUnit2.Core.Extensions;
    using Objectivity.AutoFixture.XUnit2.Core.Requests;

    internal class RandomFixedValuesParameterBuilder : ISpecimenBuilder
    {
        private readonly object[] inputValues;
        private readonly Lazy<IReadOnlyCollection<object>> readonlyValues;
        private readonly IRequestMemberTypeResolver typeResolver = new RequestMemberTypeResolver();

        public RandomFixedValuesParameterBuilder(params object[] values)
        {
            this.inputValues = values.NotNull(nameof(values));
            if (this.inputValues.Length == 0)
            {
                throw new ArgumentException("At least one value is expected to be specified.", nameof(values));
            }

            this.readonlyValues = new Lazy<IReadOnlyCollection<object>>(() => Array.AsReadOnly(this.inputValues));
        }

        public IReadOnlyCollection<object> Values => this.readonlyValues.Value;

        public object Create(object request, ISpecimenContext context)
        {
            if (context is null)
            {
                throw new ArgumentNullException(nameof(context));
            }

            if (!this.typeResolver.TryGetMemberType(request, out var type))
            {
                type = request as Type;
            }

            if (type is not null)
            {
                if (type.TryGetEnumerableSingleTypeArgument(out var itemType))
                {
                    var valuesRequest = new FixedValuesRequest(itemType, this.inputValues);
                    var specimen = context.Resolve(new MultipleRequest(valuesRequest));

                    if (specimen is IEnumerable elements)
                    {
                        var items = elements.ToTypedArray(itemType);
                        return type.IsArray || type.IsInterface || type.IsAbstract
                            ? items
                            : Activator.CreateInstance(type, items);
                    }
                }
                else
                {
                    var valuesRequest = new FixedValuesRequest(type, this.inputValues);
                    return context.Resolve(valuesRequest);
                }
            }

            return new NoSpecimen();
        }
    }
}
