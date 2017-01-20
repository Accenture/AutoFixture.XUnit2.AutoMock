namespace Objectivity.AutoFixture.XUnit2.AutoMoq.MemberData
{
    using System;
    using System.Reflection;

    public interface IDataItemConverter
    {
        object[] Convert(MethodInfo testMethod, object item, string memberName, Type memberType);
    }
}