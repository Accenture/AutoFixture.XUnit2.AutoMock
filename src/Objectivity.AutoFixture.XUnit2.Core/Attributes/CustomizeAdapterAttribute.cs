namespace Objectivity.AutoFixture.XUnit2.Core.Attributes
{
    using System;
    using System.Linq;
    using System.Reflection;
    using global::AutoFixture.Xunit2;

    using Objectivity.AutoFixture.XUnit2.Core.Common;

    //// TODO: Consider composition over inheritance
    public abstract class CustomizeAdapterAttribute<T> : CustomizeAttribute
    {
        protected CustomizeAdapterAttribute(Type type, params object[] args)
        {
            this.Type = type.NotNull(nameof(type));

            var builderType = typeof(T);
            if (!builderType.IsAssignableFrom(type))
            {
                throw new ArgumentException($"Specified argument {nameof(type)} must implement {builderType.Name}");
            }

            this.Args = args;
        }

        public Type Type { get; }

        public object[] Args { get; }

        /// <summary>
        /// Gets or sets a value indicating whether attribute target parameter type should included as a first argument when creating customization.
        /// </summary>
        /// <value>Indicates whether attribute target parameter type should included as a first argument when creating customization.</value>
        public bool IncludeParameterType { get; set; }

        protected object CreateInstance(ParameterInfo parameter)
        {
            var args = this.IncludeParameterType
                ? new object[] { parameter.NotNull(nameof(parameter)).ParameterType }
                    .Concat(this.Args ?? Array.Empty<object>())
                    .ToArray()
                : this.Args;

            return Activator.CreateInstance(this.Type, args);
        }
    }
}
