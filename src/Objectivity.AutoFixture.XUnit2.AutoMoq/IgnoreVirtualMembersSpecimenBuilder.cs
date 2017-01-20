namespace Objectivity.AutoFixture.XUnit2.AutoMoq
{
    using System;
    using System.Reflection;
    using Ploeh.AutoFixture.Kernel;

    public class IgnoreVirtualMembersSpecimenBuilder : ISpecimenBuilder
    {
        public object Create(object request, ISpecimenContext context)
        {
            if (context == null)
            {
                throw new ArgumentNullException(nameof(context));
            }

            var pi = request as PropertyInfo;
            if (pi == null)
            {
                return new NoSpecimen();
            }

            if (pi.GetGetMethod().IsVirtual)
            {
                return null;
            }

            return new NoSpecimen();
        }
    }
}