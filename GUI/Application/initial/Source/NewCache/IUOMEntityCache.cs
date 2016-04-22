using com.amphora.cayenne.main;
using com.amphora.entities.router;

// https://github.com/Amphora2015/RiskManager/wiki/UOM-Cache-%5B.NET-JAVA%5D-Updated

namespace NSRiskManager {
    public interface IUOMEntityCache {
        /**
          *  Setups this class and initialize stuff and load UOM.
          */
        void init(ICayenneService cayenneRuntime,IRabbitRouter router);
    }

    public interface IUOMContainer {
        /// <summary>Notification method when <see cref="IUomEntity"/> objects are updated.</summary>
        /// <param name="allPositions">a <see cref="java.util.List"/> of <see cref="IUomEntity"/> instances.</param>
        void updatedUOMs(java.util.List allPositions);
    }

    public interface IUomEntity { }
}