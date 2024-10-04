namespace Objectivity.AutoFixture.XUnit2.Core.MemberData
{
    using System;
    using System.Reflection;

    public interface IDataItemExtender
    {
        object[] Extend(MethodInfo testMethod, object[] values, string memberName, Type memberType);
    }
}
