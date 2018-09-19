namespace Objectivity.AutoFixture.XUnit2.Core.Customizations
{
    using System;
    using Common;
    using global::AutoFixture;
    using global::AutoFixture.Kernel;
    using SpecimenBuilders;

    public class IgnoreVirtualMembersCustomization : ICustomization
    {
        private readonly Type reflectedType;

        public IgnoreVirtualMembersCustomization()
            : this(null)
        {
        }

        public IgnoreVirtualMembersCustomization(Type reflectedType)
        {
            this.reflectedType = reflectedType;
        }

        public void Customize(IFixture fixture)
        {
            if (fixture == null)
            {
                throw new ArgumentNullException(nameof(fixture));
            }

            fixture.Customizations.Add(new IgnoreVirtualMembersSpecimenBuilder(this.reflectedType));
        }
    }
}