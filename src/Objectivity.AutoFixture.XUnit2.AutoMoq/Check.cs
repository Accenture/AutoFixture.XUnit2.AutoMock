namespace Objectivity.AutoFixture.XUnit2.AutoMoq
{
    using System;
    using System.Diagnostics;
    using JetBrains.Annotations;

    /// <summary>
    /// Copied from https://github.com/aspnet/EntityFramework/blob/dev/src/Shared/Check.cs
    /// </summary>
    [DebuggerStepThrough]
    internal static class Check
    {
        [ContractAnnotation("value:null => halt")]
        public static T NotNull<T>(
            [NoEnumeration] [ValidatedNotNull] this T value,
            [InvokerParameterName] [NotNull] string parameterName)
        {
            if (ReferenceEquals(value, null))
            {
                throw new ArgumentNullException(parameterName);
            }

            return value;
        }
    }
}
