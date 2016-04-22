namespace NSRiskManager {
    class PortFetchData {
        #region ctor
        public PortFetchData(int aPortNum) {
            portNum = aPortNum;
        }
        #endregion

        #region properties
        public int portNum { get; private set; }
        #endregion
    }
}