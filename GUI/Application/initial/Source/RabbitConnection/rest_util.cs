using System;
using System.Diagnostics;
using Newtonsoft.Json.Serialization;
using NSRMLogging;

namespace NSRiskManager {
    enum ThreadExitReason {
        UNKNOWN,
        NoPortfolio,
        NoPosition,
        Other,
    }

    interface IThreadController {
        int portfolioNumber { get; }
        ThreadExitReason exitReason { get; }
        IThreadController controller { get; }
    }
      
    class MyTraceWriter : ITraceWriter {
        #region ctors
        public MyTraceWriter() : this(TraceLevel.Error) { }
        public MyTraceWriter(TraceLevel aTraceLevel) { this.LevelFilter = aTraceLevel; }
        #endregion
        #region properties
        public TraceLevel LevelFilter { get; set; }
        #endregion
        #region methods
        void ITraceWriter.Trace(TraceLevel level,string message,Exception ex) {
            Trace.WriteLine("[" + level + "] " + message + ":" + Util.makeErrorMessage(ex,false));
        }
        #endregion
    }
}