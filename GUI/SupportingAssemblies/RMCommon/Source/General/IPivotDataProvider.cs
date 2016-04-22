using  System.Collections.Generic;

namespace NSRMCommon
{
    public interface IPivotDataProvider {
        object pivotGridData2 { get; }

        object findDistributions(int[] positionNums, Dictionary<int, char> positionTypes, Dictionary<int, string> commodityCodes, Dictionary<int, string> originalUOMs, int decimalPrecision, string UOM, bool equivIndChecked);
    }
}