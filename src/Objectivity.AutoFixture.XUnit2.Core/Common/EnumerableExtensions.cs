namespace Objectivity.AutoFixture.XUnit2.Core.Common
{
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.Linq;
    using System.Reflection;

    internal static class EnumerableExtensions
    {
        private static readonly MethodInfo BuildTypedArrayMethodInfo =
            typeof(EnumerableExtensions).GetTypeInfo().GetMethod(
                nameof(BuildTypedArray),
                BindingFlags.Static | BindingFlags.NonPublic);

        public static bool TryGetEnumerableSingleTypeArgument(this Type type, out Type argument)
        {
            if (type.NotNull(nameof(type)).IsArray)
            {
                argument = type.GetElementType();
                return true;
            }

            var interfaces = type.GetInterfaces();
            if (type.IsInterface)
            {
                interfaces = interfaces.Append(type).ToArray();
            }

            var genericInterface = Array.Find(
                interfaces,
                x => x.IsGenericType && x.GetGenericTypeDefinition() == typeof(IEnumerable<>));
            if (genericInterface is not null)
            {
                var typeArguments = genericInterface.GenericTypeArguments;
                if (typeArguments.Length == 1)
                {
                    argument = typeArguments[0];
                    return true;
                }
            }

            argument = null;
            return false;
        }

        public static object ToTypedArray(this IEnumerable items, Type itemType)
        {
            var method = BuildTypedArrayMethodInfo.MakeGenericMethod(itemType.NotNull(nameof(itemType)));
            return method.Invoke(null, new object[] { items.NotNull(nameof(items)) });
        }

        private static TElementType[] BuildTypedArray<TElementType>(IEnumerable items)
        {
            return items.Cast<TElementType>()
                .ToArray();
        }
    }
}
