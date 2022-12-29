namespace Objectivity.AutoFixture.XUnit2.Core.Common
{
    using System;
    using System.Diagnostics;

    using JetBrains.Annotations;

    /// <summary>
    /// Copied from <a href="https://github.com/aspnet/EntityFramework/blob/dev/src/Shared/Check.cs">Check.cs</a>.
    /// </summary>
    [DebuggerStepThrough]
    public static class Check
    {
        [ContractAnnotation("value:null => halt")]
        public static T NotNull<T>(
            [NoEnumeration][ValidatedNotNull] this T value,
            [InvokerParameterName][NotNull] string parameterName)
        {
            if (value is null)
            {
                throw new ArgumentNullException(parameterName);
            }

            return value;
        }
    }
}
