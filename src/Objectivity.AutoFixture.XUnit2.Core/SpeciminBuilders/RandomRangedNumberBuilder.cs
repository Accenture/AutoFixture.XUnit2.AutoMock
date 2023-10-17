namespace Objectivity.AutoFixture.XUnit2.Core.SpeciminBuilders
{
    using System;

    using global::AutoFixture;
    using global::AutoFixture.Kernel;

    using Objectivity.AutoFixture.XUnit2.Core.Common;

    public class RandomRangedNumberBuilder : ISpecimenBuilder
    {
        private readonly RandomRangedNumberGenerator randomRangedNumberGenerator = new();

        public RandomRangedNumberBuilder(Type operandType, object minimum, object maximum)
        {
            this.Request = new RangedNumberRequest(
                operandType.NotNull(nameof(operandType)),
                minimum.NotNull(nameof(minimum)),
                maximum.NotNull(nameof(maximum)));
        }

        public RangedNumberRequest Request { get; }

        public object Create(object request, ISpecimenContext context)
        {
            return this.randomRangedNumberGenerator.Create(this.Request, context);
        }
    }
}
