namespace Objectivity.AutoFixture.XUnit2.Core.SpecimenBuilders
{
    using System;
    using System.Reflection;

    using global::AutoFixture;
    using global::AutoFixture.Kernel;

    using Objectivity.AutoFixture.XUnit2.Core.Common;

    public class RandomRangedNumberParameterBuilder : ISpecimenBuilder
    {
        private readonly RandomRangedNumberGenerator randomRangedNumberGenerator = new();

        public RandomRangedNumberParameterBuilder(object minimum, object maximum)
        {
            this.Minimum = minimum.NotNull(nameof(minimum));
            this.Maximum = maximum.NotNull(nameof(maximum));

            if (minimum is not IComparable comparableMin)
            {
                throw new ArgumentException($"Parameter {nameof(minimum)} is expected to be comparable to parameter {nameof(maximum)}.", nameof(minimum));
            }

            if (maximum is not IComparable comparableMax)
            {
                throw new ArgumentException($"Parameter {nameof(maximum)} is expected to be comparable to parameter {nameof(maximum)}.", nameof(maximum));
            }

            if (comparableMin.CompareTo(comparableMax) > 0)
            {
                throw new ArgumentOutOfRangeException(nameof(minimum), $"Parameter {nameof(minimum)} must be lower or equal to parameter {nameof(maximum)}.");
            }
        }

        public object Minimum { get; }

        public object Maximum { get; }

        public object Create(object request, ISpecimenContext context)
        {
            var pi = request as ParameterInfo;
            if (pi is not null) //// is a parameter
            {
                var rangeRequest = new RangedNumberRequest(pi.ParameterType, this.Minimum, this.Maximum);
                return this.randomRangedNumberGenerator.Create(rangeRequest, context);
            }

            return new NoSpecimen();
        }
    }
}
