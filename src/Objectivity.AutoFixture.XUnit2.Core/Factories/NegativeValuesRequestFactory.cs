namespace Objectivity.AutoFixture.XUnit2.Core.Factories
{
    using System;
    using System.Globalization;
    using System.Linq;

    using global::AutoFixture;
    using global::AutoFixture.Kernel;

    using Objectivity.AutoFixture.XUnit2.Core.Common;
    using Objectivity.AutoFixture.XUnit2.Core.Requests;

    internal class NegativeValuesRequestFactory : IFactory<Type, object>
    {
        private static readonly decimal DecimalEpsilon = new(1, 0, 0, true, 28);

        private static ObjectCreationException ObjectCreationException => new("The value could not be created. Probably attribute is specified for a type that does not accept negative values.");

        public object Create(Type input)
        {
            return input.NotNull(nameof(input)).IsEnum
                ? GetRequestForEnum(input)
                : GetRequestForNumericType(input);
        }

        private static FixedValuesRequest GetRequestForEnum(Type type)
        {
            var values = Type.GetTypeCode(type) switch
            {
                TypeCode.SByte => GetEnumNegativeValues<sbyte>(type),
                TypeCode.Int16 => GetEnumNegativeValues<short>(type),
                TypeCode.Int32 => GetEnumNegativeValues<int>(type),
                TypeCode.Int64 => GetEnumNegativeValues<long>(type),
                _ => Array.Empty<object>(),
            };

            if (values.Length == 0)
            {
                throw ObjectCreationException;
            }

            return new FixedValuesRequest(type, values);
        }

        private static object[] GetEnumNegativeValues<T>(Type type)
            where T : struct, IComparable
        {
            var zero = Convert.ChangeType(0, typeof(T), CultureInfo.InvariantCulture);
            return Enum.GetValues(type)
                .Cast<T>()
                .Where(x => x.CompareTo(zero) < 0)
                .Select(x => Enum.ToObject(type, x))
                .ToArray();
        }

        private static RangedNumberRequest GetRequestForNumericType(Type type)
        {
            var (minimum, maximum) = Type.GetTypeCode(type) switch
            {
                TypeCode.Decimal => ((object, object))(decimal.MinValue, DecimalEpsilon),
                TypeCode.Double => (double.MinValue, -double.Epsilon),
                TypeCode.Int16 => (short.MinValue, (short)-1),
                TypeCode.Int32 => (int.MinValue, -1),
                TypeCode.Int64 => (long.MinValue, -1L),
                TypeCode.SByte => (sbyte.MinValue, (sbyte)-1),
                TypeCode.Single => (float.MinValue, -float.Epsilon),
                _ => throw ObjectCreationException,
            };

            return new RangedNumberRequest(type, minimum, maximum);
        }
    }
}
