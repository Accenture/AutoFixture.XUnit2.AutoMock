namespace Objectivity.AutoFixture.XUnit2.Core.Attributes
{
    using System;
    using System.Reflection;

    using global::AutoFixture;
    using global::AutoFixture.Kernel;

    [AttributeUsage(AttributeTargets.Parameter, AllowMultiple = true)]
    public abstract class BuildWithAttribute : CustomizeAdapterAttribute<ISpecimenBuilder>
    {
        protected BuildWithAttribute(Type type, params object[] args)
            : base(type, args)
        {
        }

        public override ICustomization GetCustomization(ParameterInfo parameter)
        {
            var specimenBuilder = this.CreateInstance(parameter) as ISpecimenBuilder;
            var builder = new FilteringSpecimenBuilder(specimenBuilder, new EqualRequestSpecification(parameter));

            return builder.ToCustomization();
        }
    }
}
