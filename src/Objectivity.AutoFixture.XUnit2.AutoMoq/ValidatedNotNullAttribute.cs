namespace Objectivity.AutoFixture.XUnit2.AutoMoq
{
    using System;

    [AttributeUsage(AttributeTargets.Parameter, AllowMultiple = false, Inherited = false)]
    internal sealed class ValidatedNotNullAttribute : Attribute
    {
    }
}
