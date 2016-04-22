using System.Drawing;
namespace NSRiskManager {
    interface IWindowLayoutProvider {
        int portSplitPosition { get; set; }
        int lowerSplitPosition { get; set; }
        int pnlSplitPosition { get; set; }
        Rectangle windowFrame { get; set; }
        string selectedPortfolioPath { get; set; }
    }
}