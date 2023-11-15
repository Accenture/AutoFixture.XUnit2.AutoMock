namespace Objectivity.AutoFixture.XUnit2.Core.SpecimenBuilders
{
    using System;
    using System.Collections;

    using global::AutoFixture;
    using global::AutoFixture.Kernel;

    using Objectivity.AutoFixture.XUnit2.Core.Common;
    using Objectivity.AutoFixture.XUnit2.Core.Extensions;

    internal class RandomRangedNumberParameterBuilder : ISpecimenBuilder
    {
        private readonly IRequestMemberTypeResolver typeResolver = new RequestMemberTypeResolver();

        public RandomRangedNumberParameterBuilder(object minimum, object maximum)
        {
            this.Minimum = minimum.NotNull(nameof(minimum));
            this.Maximum = maximum.NotNull(nameof(maximum));

            if (minimum is not IComparable comparableMin)
            {
                throw new ArgumentException($"Parameter {nameof(minimum)} is expected to be comparable to parameter {nameof(maximum)}.", nameof(minimum));
            }

            if (maximum is not IComparable comparableMax)
            {
                throw new ArgumentException($"Parameter {nameof(maximum)} is expected to be comparable to parameter {nameof(maximum)}.", nameof(maximum));
            }

            if (comparableMin.CompareTo(comparableMax) > 0)
            {
                throw new ArgumentOutOfRangeException(nameof(minimum), $"Parameter {nameof(minimum)} must be lower or equal to parameter {nameof(maximum)}.");
            }
        }

        public object Minimum { get; }

        public object Maximum { get; }

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
                    var rangeRequest = new RangedNumberRequest(itemType, this.Minimum, this.Maximum);
                    var specimen = context.Resolve(new MultipleRequest(rangeRequest));

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
                    var rangeRequest = new RangedNumberRequest(type, this.Minimum, this.Maximum);
                    return context.Resolve(rangeRequest);
                }
            }

            return new NoSpecimen();
        }
    }
}
