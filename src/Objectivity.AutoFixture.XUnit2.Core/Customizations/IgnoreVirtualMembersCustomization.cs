namespace Objectivity.AutoFixture.XUnit2.Core.Customizations
{
    using System;
    using Common;
    using global::AutoFixture;
    using global::AutoFixture.Kernel;
    using SpecimenBuilders;

    public class IgnoreVirtualMembersCustomization : ICustomization
    {
        public IgnoreVirtualMembersCustomization()
            : this(null)
        {
        }

        public IgnoreVirtualMembersCustomization(Type reflectedType)
        {
            this.ReflectedType = reflectedType;
        }

        public Type ReflectedType { get; private set; }

        public void Customize(IFixture fixture)
        {
            fixture.NotNull(nameof(fixture)).Customizations.Add(new IgnoreVirtualMembersSpecimenBuilder(this.ReflectedType));
        }
    }
}