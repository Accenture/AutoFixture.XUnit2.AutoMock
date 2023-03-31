namespace Objectivity.AutoFixture.XUnit2.Core.Attributes
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Reflection;

    using global::AutoFixture;
    using global::AutoFixture.Kernel;
    using global::AutoFixture.Xunit2;
    using Objectivity.AutoFixture.XUnit2.Core.Common;
    using Objectivity.AutoFixture.XUnit2.Core.Comparers;

    [AttributeUsage(AttributeTargets.Method, AllowMultiple = false, Inherited = true)]
    internal sealed class AutoDataAdapterAttribute : AutoDataAttribute
    {
        public AutoDataAdapterAttribute(IFixture fixture, params object[] inlineValues)
            : base(() => fixture)
        {
            this.AdaptedFixture = fixture.NotNull(nameof(fixture));
            this.InlineValues = Array.AsReadOnly(inlineValues ?? Array.Empty<object>());
        }

        public IFixture AdaptedFixture { get; }

        public IReadOnlyCollection<object> InlineValues { get; }

        public override IEnumerable<object[]> GetData(MethodInfo testMethod)
        {
            // This is an extension of AutoDataAttribute.GetData method
            // with the ability to skip already provided inline values
            var parameters = testMethod.NotNull(nameof(testMethod)).GetParameters();
            var specimens = new List<object>(this.InlineValues);
            foreach (var p in parameters.Skip(this.InlineValues.Count))
            {
                this.CustomizeFixture(p);

                var specimen = this.Resolve(p);
                specimens.Add(specimen);
            }

            return new[] { specimens.ToArray() };
        }

        private void CustomizeFixture(ParameterInfo p)
        {
            var customizeAttributes = p.GetCustomAttributes()
                .OfType<IParameterCustomizationSource>()
                .OrderBy(x => x, new CustomizeAttributeComparer());

            foreach (var ca in customizeAttributes)
            {
                var c = ca.GetCustomization(p);
                this.AdaptedFixture.Customize(c);
            }
        }

        private object Resolve(ParameterInfo p)
        {
            var context = new SpecimenContext(this.AdaptedFixture);
            return context.Resolve(p);
        }
    }
}
