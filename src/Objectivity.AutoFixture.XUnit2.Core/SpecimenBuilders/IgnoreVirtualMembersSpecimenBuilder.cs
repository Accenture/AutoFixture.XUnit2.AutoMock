namespace Objectivity.AutoFixture.XUnit2.Core.SpecimenBuilders
{
    using System;
    using System.Reflection;
    using global::AutoFixture.Kernel;

    internal class IgnoreVirtualMembersSpecimenBuilder : ISpecimenBuilder
    {
        private readonly Type reflectedType;

        public IgnoreVirtualMembersSpecimenBuilder()
            : this(null)
        {
        }

        public IgnoreVirtualMembersSpecimenBuilder(Type reflectedType)
        {
            this.reflectedType = reflectedType;
        }

        public object Create(object request, ISpecimenContext context)
        {
            var pi = request as PropertyInfo;
            if (pi != null) //// is a property
            {
                if (this.reflectedType == null || //// is hosted anywhere
                    this.reflectedType == pi.ReflectedType) //// is hosted in defined type
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