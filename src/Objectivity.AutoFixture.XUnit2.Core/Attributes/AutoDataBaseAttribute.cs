namespace Objectivity.AutoFixture.XUnit2.Core.Attributes
{
    using System;
    using System.Collections.Generic;
    using System.Reflection;
    using global::AutoFixture;
    using global::AutoFixture.Kernel;
    using Objectivity.AutoFixture.XUnit2.Core.Common;
    using Objectivity.AutoFixture.XUnit2.Core.Customizations;
    using Objectivity.AutoFixture.XUnit2.Core.Providers;
    using Xunit.Sdk;

    [AttributeUsage(AttributeTargets.Method, AllowMultiple = false, Inherited = true)]
    public abstract class AutoDataBaseAttribute : DataAttribute
    {
        protected AutoDataBaseAttribute(IFixture fixture, IAutoFixtureAttributeProvider provider)
        {
            this.Fixture = fixture.NotNull(nameof(fixture));
            this.Provider = provider.NotNull(nameof(provider));
        }

        public IFixture Fixture { get; }

        /// <summary>
        /// Gets or sets a value indicating whether virtual members should be ignored during object creation.
        /// </summary>
        public bool IgnoreVirtualMembers { get; set; }

        /// <summary>
        ///  Gets or sets array of <see cref="ICustomization"/>s or <see cref="ISpecimenBuilder"/> to apply.
        /// </summary>
        public Type[] Customizations { get; set; }

        public IAutoFixtureAttributeProvider Provider { get; }

        public override IEnumerable<object[]> GetData(MethodInfo testMethod)
        {
            this.Fixture.Customize(new AutoDataCommonCustomization(this.IgnoreVirtualMembers));
            this.Customize(this.Fixture);
            this.ApplyCustomizations();

            return this.Provider.GetAttribute(this.Fixture).GetData(testMethod);
        }

        protected abstract IFixture Customize(IFixture fixture);

        private void ApplyCustomizations()
        {
            foreach (var type in this.Customizations)
            {
                var instance = Activator.CreateInstance(type);
                switch (instance)
                {
                    case ISpecimenBuilder specimenBuilder:
                        this.Fixture.Customizations.Add(specimenBuilder);
                        break;

                    case ICustomization customization:
                        this.Fixture.Customize(customization);
                        break;

                    default:
                        throw new InvalidOperationException("Provided customization is not of supported type.");
                }
            }
        }
    }
}