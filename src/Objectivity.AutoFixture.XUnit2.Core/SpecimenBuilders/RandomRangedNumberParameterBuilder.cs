namespace Objectivity.AutoFixture.XUnit2.Core.SpecimenBuilders
{
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.Linq;
    using System.Reflection;

    using global::AutoFixture;
    using global::AutoFixture.Kernel;

    using Objectivity.AutoFixture.XUnit2.Core.Common;

    internal class RandomRangedNumberParameterBuilder : ISpecimenBuilder
    {
        // TODO: Consider encapsulating in separate structure
        private static readonly MethodInfo BuildTypedArrayMethodInfo =
            typeof(RandomRangedNumberParameterBuilder).GetTypeInfo().GetMethod(
                nameof(BuildTypedArray),
                BindingFlags.Static | BindingFlags.NonPublic);

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
                if (type.IsArray)
                {
                    return this.CreateMultiple(type.GetElementType(), context, true);
                }
                else if (TryGetSingleEnumerableTypeArgument(type, out var enumerableType))
                {
                    var items = this.CreateMultiple(enumerableType, context, false);
                    return type.IsInterface || type.IsAbstract
                        ? items
                        : Activator.CreateInstance(type, items);
                }
                else
                {
                    var rangeRequest = new RangedNumberRequest(type, this.Minimum, this.Maximum);
                    return context.Resolve(rangeRequest);
                }
            }

            return new NoSpecimen();
        }

        private static bool TryGetSingleEnumerableTypeArgument(Type currentType, out Type argument)
        {
            var interfaces = currentType.GetInterfaces().Append(currentType).ToArray();
            var typeInfo = Array.Find(
                interfaces,
                x => x.IsGenericType && x.GetGenericTypeDefinition() == typeof(IEnumerable<>));
            if (typeInfo is not null)
            {
                var typeArguments = typeInfo.GenericTypeArguments;
                if (typeArguments.Length == 1)
                {
                    argument = typeArguments[0];
                    return true;
                }
            }

            argument = null;
            return false;
        }

        private static object ToTypedArray(IEnumerable items, Type elementType, bool isArrayExpected)
        {
            return BuildTypedArrayMethodInfo.MakeGenericMethod(elementType).Invoke(null, new object[] { items, isArrayExpected });
        }

        private static IEnumerable<TElementType> BuildTypedArray<TElementType>(IEnumerable items, bool isArrayExpected)
        {
            var casted = items is IEnumerable<TElementType> castedItems
                ? castedItems
                : items.Cast<TElementType>();

            return isArrayExpected ? casted.ToArray() : casted.ToList();
        }

        private object CreateMultiple(Type type, ISpecimenContext context, bool isArrayExpected)
        {
            var rangeRequest = new RangedNumberRequest(type, this.Minimum, this.Maximum);
            var specimen = context.Resolve(new MultipleRequest(rangeRequest));

            if (specimen is not IEnumerable elements)
            {
                return new NoSpecimen();
            }

            return ToTypedArray(elements, type, isArrayExpected);
        }
    }
}
