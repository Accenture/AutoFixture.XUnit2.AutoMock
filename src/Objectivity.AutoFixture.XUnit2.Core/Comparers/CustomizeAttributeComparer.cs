namespace Objectivity.AutoFixture.XUnit2.Core.Comparers
{
    using System.Collections.Generic;
    using global::AutoFixture;
    using global::AutoFixture.Xunit2;

    // Direct copy from the AutoFixture source code as the original class is internal.
    internal class CustomizeAttributeComparer : Comparer<IParameterCustomizationSource>
    {
        public override int Compare(IParameterCustomizationSource x, IParameterCustomizationSource y)
        {
            var xFrozen = x is FrozenAttribute;
            var yFrozen = y is FrozenAttribute;

            if (xFrozen && !yFrozen)
            {
                return 1;
            }

            if (yFrozen && !xFrozen)
            {
                return -1;
            }

            return 0;
        }
    }
}
