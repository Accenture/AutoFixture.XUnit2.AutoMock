namespace Objectivity.AutoFixture.XUnit2.Core.Comparers
{
    using System.Collections.Generic;

    using global::AutoFixture;
    using global::AutoFixture.Xunit2;

    /// <summary>
    /// Direct copy from the AutoFixture source code as the original class is internal.
    /// </summary>
    internal class CustomizeAttributeComparer : Comparer<IParameterCustomizationSource>
    {
        public override int Compare(IParameterCustomizationSource x, IParameterCustomizationSource y)
        {
            var isXFrozen = x is FrozenAttribute;
            var isYFrozen = y is FrozenAttribute;

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
