namespace Objectivity.AutoFixture.XUnit2.Core.Attributes
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;
    using AutoFixture;
    using global::AutoFixture;
    using global::AutoFixture.Xunit2;

    internal sealed class AutoDataAdapterAttribute : AutoDataAttribute
    {
        public AutoDataAdapterAttribute(IFixture fixture)
            : base(() => fixture)
        {
            this.AdaptedFixture = fixture;
        }

        public IFixture AdaptedFixture { get; }
    }
}
