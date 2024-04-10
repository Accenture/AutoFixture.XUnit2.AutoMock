namespace Objectivity.AutoFixture.XUnit2.Core.SpecimenBuilders
{
    using System;
    using System.Reflection;
    using global::AutoFixture.Kernel;

    public class IgnoreVirtualMembersSpecimenBuilder : ISpecimenBuilder
    {
        public IgnoreVirtualMembersSpecimenBuilder()
            : this(null)
        {
        }

        public IgnoreVirtualMembersSpecimenBuilder(Type reflectedType)
        {
            this.ReflectedType = reflectedType;
        }

        public Type ReflectedType { get; }

        public object Create(object request, ISpecimenContext context)
        {
            if (request is PropertyInfo pi) //// is a property
            {
                if (this.ReflectedType is null //// is hosted anywhere
                    ||
                    this.ReflectedType == pi.ReflectedType) //// is hosted in defined type
                {
                    if (pi.GetGetMethod().IsVirtual)
                    {
                        return new OmitSpecimen();
                    }
                }
            }

            return new NoSpecimen();
        }
    }
}
