namespace Objectivity.AutoFixture.XUnit2.Core.SpecimenBuilders
{
    using System.Reflection;
    using Ploeh.AutoFixture.Kernel;

    internal class IgnoreVirtualMembersSpecimenBuilder : ISpecimenBuilder
    {
        public object Create(object request, ISpecimenContext context)
        {
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