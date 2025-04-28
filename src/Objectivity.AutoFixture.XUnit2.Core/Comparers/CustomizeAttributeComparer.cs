namespace Objectivity.AutoFixture.XUnit2.Core.Comparers
{
    using System.Collections.Generic;

    using global::AutoFixture;

    internal class CustomizeAttributeComparer<T> : Comparer<IParameterCustomizationSource>
        where T : IParameterCustomizationSource
    {
        public override int Compare(IParameterCustomizationSource x, IParameterCustomizationSource y)
        {
            var isXFrozen = x is T;
            var isYFrozen = y is T;

            if (isXFrozen && !isYFrozen)
            {
                return 1;
            }

            if (isYFrozen && !isXFrozen)
            {
                return -1;
            }

            return 0;
        }
    }
}
