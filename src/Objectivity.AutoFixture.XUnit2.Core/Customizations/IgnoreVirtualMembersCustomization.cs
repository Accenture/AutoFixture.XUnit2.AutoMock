namespace Objectivity.AutoFixture.XUnit2.Core.Customizations
{
    using System;

    using global::AutoFixture;

    using Objectivity.AutoFixture.XUnit2.Core.Common;
    using Objectivity.AutoFixture.XUnit2.Core.SpecimenBuilders;

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

        public Type ReflectedType { get; }

        public void Customize(IFixture fixture)
        {
            fixture.NotNull(nameof(fixture)).Customizations.Add(new IgnoreVirtualMembersSpecimenBuilder(this.ReflectedType));
        }
    }
}
