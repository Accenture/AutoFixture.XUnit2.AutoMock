namespace Objectivity.AutoFixture.XUnit2.Core.Attributes
{
    using System;

    using Objectivity.AutoFixture.XUnit2.Core.SpeciminBuilders;

    [AttributeUsage(AttributeTargets.Parameter, AllowMultiple = true)]
    public sealed class FromRangeAttribute : BuildWithAttribute<RandomRangedNumberBuilder>
    {
        public FromRangeAttribute(byte minimum, byte maximum)
            : this((object)minimum, maximum)
        {
        }

        public FromRangeAttribute(short minimum, short maximum)
            : this((object)minimum, maximum)
        {
        }

        public FromRangeAttribute(int minimum, int maximum)
            : this((object)minimum, maximum)
        {
        }

        public FromRangeAttribute(long minimum, long maximum)
            : this((object)minimum, maximum)
        {
        }

        public FromRangeAttribute(double minimum, double maximum)
            : this((object)minimum, maximum)
        {
        }

        public FromRangeAttribute(float minimum, float maximum)
            : this((object)minimum, maximum)
        {
        }

        public FromRangeAttribute(object minimum, object maximum)
            : base(minimum, maximum)
        {
            this.IncludeParameterType = true;
            this.Minimum = minimum;
            this.Maximum = maximum;
        }

        public object Minimum { get; }

        public object Maximum { get; }
    }
}
