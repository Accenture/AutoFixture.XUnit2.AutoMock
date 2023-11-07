namespace Objectivity.AutoFixture.XUnit2.Core.SpecimenBuilders
{
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.Diagnostics.CodeAnalysis;
    using System.Linq;
    using System.Reflection;

    using global::AutoFixture;
    using global::AutoFixture.Kernel;

    using Objectivity.AutoFixture.XUnit2.Core.Common;

    internal class RandomValuesParameterBuilder : ISpecimenBuilder
    {
        private readonly Dictionary<Type, IEnumerator> enumerators = new();
        private readonly object syncRoot = new();

        public RandomValuesParameterBuilder(params object[] args)
        {
            this.Args = args.NotNull(nameof(args));

            if (this.Args.Length == 0)
            {
                throw new ArgumentException("At least one argument is expected to be specified.", nameof(args));
            }
        }

        public object[] Args { get; }

        public object Create(object request, ISpecimenContext context)
        {
            if (request is ParameterInfo pi) //// is a parameter
            {
                lock (this.syncRoot)
                {
                    return this.CreateValue(pi.ParameterType);
                }
            }

            return new NoSpecimen();
        }

        private object CreateValue(Type t)
        {
            var generator = this.EnsureGenerator(t);
            generator.MoveNext();
            return generator.Current;
        }

        private IEnumerator EnsureGenerator(Type t)
        {
            if (!this.enumerators.TryGetValue(t, out var enumerator))
            {
                enumerator = new RoundRobinCollection(t, this.Args).GetEnumerator();
                this.enumerators.Add(t, enumerator);
            }

            return enumerator;
        }

        private sealed class RoundRobinCollection : IEnumerable<object>
        {
            private readonly IEnumerable<object> values;

            [SuppressMessage("Security", "CA5394:Do not use insecure randomness", Justification = "It is good enought for collection randomisation.")]
            internal RoundRobinCollection(Type targetType, object[] args)
            {
                var type = targetType.NotNull(nameof(targetType));
                var random = new Random();

                if (type.IsEnum)
                {
                    // TODO: Should we
                    // 1. Allow values outside enum?
                    // 2. Filter out values outside enum?
                    // 3. Throw exception when values outside enum?
                    var enumValues = Enum.GetValues(type).Cast<object>().ToList();
                    this.values = args.Where(enumValues.Contains)
                        .OrderBy((_) => random.Next());
                }
                else
                {
                    // TODO: Check nullable
                    // TODO: Check reference types
                    // TODO: Check min/max of numeric types
                    this.values = args.OrderBy((_) => random.Next());
                }

                if (!this.values.Any())
                {
                    var message = $"AutoFixture was unable to create a value for {targetType.FullName} since it is an enum containing no values. Please add at least one value to the enum.";
                    throw new ObjectCreationException(message);
                }
            }

            [SuppressMessage("Blocker Bug", "S2190:Loops and recursions should not be infinite", Justification = "This is a round robin implementation.")]
            public IEnumerator<object> GetEnumerator()
            {
                while (true)
                {
                    foreach (var obj in this.values)
                    {
                        yield return obj;
                    }
                }
            }

            IEnumerator IEnumerable.GetEnumerator()
            {
                return this.GetEnumerator();
            }
        }
    }
}
