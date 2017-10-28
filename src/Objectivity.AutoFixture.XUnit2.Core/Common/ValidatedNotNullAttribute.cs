namespace Objectivity.AutoFixture.XUnit2.Core.Common
{
    using System;

    [AttributeUsage(AttributeTargets.Parameter, AllowMultiple = false)]
    internal sealed class ValidatedNotNullAttribute : Attribute
    {
    }
}