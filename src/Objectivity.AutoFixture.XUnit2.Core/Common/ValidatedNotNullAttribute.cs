namespace Objectivity.AutoFixture.XUnit2.Core.Common
{
    using System;
    using System.Diagnostics.CodeAnalysis;

    [AttributeUsage(AttributeTargets.Parameter, AllowMultiple = false)]
    [SuppressMessage("ReSharper", "RedundantTypeDeclarationBody", Justification = "Indicates to Code Analysis that a method validates a particular parameter.")]
    internal sealed class ValidatedNotNullAttribute : Attribute
    {
    }
}
