namespace Objectivity.AutoFixture.XUnit2.Core.Attributes
{
    using System;
    using System.Linq;
    using System.Reflection;

    using global::AutoFixture;
    using global::AutoFixture.Kernel;
    using global::AutoFixture.Xunit2;

    using Objectivity.AutoFixture.XUnit2.Core.Requests;
    using Objectivity.AutoFixture.XUnit2.Core.SpecimenBuilders;

    [AttributeUsage(AttributeTargets.Parameter, AllowMultiple = false)]
    public sealed class PickNegativeAttribute : CustomizeAttribute
    {
        private static readonly decimal DecimalEpsilon = new(1, 0, 0, true, 28);

        public override ICustomization GetCustomization(ParameterInfo parameter)
        {
            return new CompositeSpecimenBuilder(
                new FilteringSpecimenBuilder(
                    new RequestFactoryRelay((type) =>
                    {
                        if (type is not null)
                        {
                            return type.IsEnum
                                ? GetRequestForEnum(type)
                                : GetRequestForNumericType(type);
                        }

                        return null;
                    }),
                    new EqualRequestSpecification(parameter)),
                new RandomFixedValuesGenerator())
                .ToCustomization();
        }

        private static FixedValuesRequest GetRequestForEnum(Type type)
        {
            var values = Enum.GetValues(type);
            var result = Type.GetTypeCode(type) switch
            {
                TypeCode.SByte => values.Cast<sbyte>().Where(x => x < 0).Select(x => Enum.ToObject(type, x)).ToArray(),
                TypeCode.Int16 => values.Cast<short>().Where(x => x < 0).Select(x => Enum.ToObject(type, x)).ToArray(),
                TypeCode.Int32 => values.Cast<int>().Where(x => x < 0).Select(x => Enum.ToObject(type, x)).ToArray(),
                TypeCode.Int64 => values.Cast<long>().Where(x => x < 0).Select(x => Enum.ToObject(type, x)).ToArray(),
                _ => Array.Empty<object>(),
            };

            if (result.Length == 0)
            {
                throw GetObjectCreationException();
            }

            return new FixedValuesRequest(type, result);
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
                _ => throw GetObjectCreationException(),
            };

            return new RangedNumberRequest(type, minimum, maximum);
        }

        private static ObjectCreationException GetObjectCreationException()
        {
            return new ObjectCreationException("The value could not be created. Probably attribute is specified for a type that does not accept negative values.");
        }
    }
}
