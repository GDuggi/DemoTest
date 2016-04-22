using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Diagnostics;
using System.Reflection;
using NSRiskManager;

class Portfolio {


    internal static List<Portfolio> fetch(SqlCommand cmd) {

        List<Portfolio> ret = new List<Portfolio>();

        using (SqlDataReader reader = cmd.ExecuteReader()) {
            while (reader.Read()) {
                ret.Add(new Portfolio(reader));
                // Util.show(MethodBase.GetCurrentMethod());
            }
            //            if (ret.Count>0)
            //          var avar=PortfolioGroup.findForParent()
        }
        return ret;
    }

    #region fields
    int _portNum;
    string _portType;
    string _desiredPlCurrCode;
    string _portShortName;
    string _portFullName;
    string _portClass;
    string _portRefKey;
    string _ownerInit;
    int _cmntNum;
    int _numHistoryDays;
    int _tradingEntityNum;
    short _portLocked;
    int _transId;
    #endregion

    #region ctors
#if false
    public Portfolio(SqlDataReader reader) {
        //        Util.showFields(reader,GetType());
        Util.generateCodeFor(GetType(),reader);
        Debug.Print("in portfolio-ctor");
    }
#else
    public Portfolio(IDataReader reader) {
        for (int i = 0;(i < reader.FieldCount);i++
        ) {
            if (reader.IsDBNull(i)) {
                continue;
            }
            switch (reader.GetName(i)) {
                case "port_num": this._portNum = reader.GetInt32(i); break;
                case "port_type": this._portType = reader.GetString(i).Trim(); break;
                case "desired_pl_curr_code": this._desiredPlCurrCode = reader.GetString(i).Trim(); break;
                case "port_short_name": this._portShortName = reader.GetString(i).Trim(); break;
                case "port_full_name": this._portFullName = reader.GetString(i).Trim(); break;
                case "port_class": this._portClass = reader.GetString(i).Trim(); break;
                case "port_ref_key": this._portRefKey = reader.GetString(i).Trim(); break;
                case "owner_init": this._ownerInit = reader.GetString(i).Trim(); break;
                case "cmnt_num": this._cmntNum = reader.GetInt32(i); break;
                case "num_history_days": this._numHistoryDays = reader.GetInt32(i); break;
                case "trading_entity_num": this._tradingEntityNum = reader.GetInt32(i); break;
                case "port_locked": this._portLocked = reader.GetInt16(i); break;
                case "trans_id": this._transId = reader.GetInt32(i); break;
            }
        }
    }
#endif
    #endregion

    #region properties
    public int portNum { get { return this._portNum; } set { this._portNum = value; } }
    public string portType { get { return this._portType; } set { this._portType = value; } }
    public string desiredPlCurrCode { get { return this._desiredPlCurrCode; } set { this._desiredPlCurrCode = value; } }
    public string portShortName { get { return this._portShortName; } set { this._portShortName = value; } }
    public string portFullName { get { return this._portFullName; } set { this._portFullName = value; } }
    public string portClass { get { return this._portClass; } set { this._portClass = value; } }
    public string portRefKey { get { return this._portRefKey; } set { this._portRefKey = value; } }
    public string ownerInit { get { return this._ownerInit; } set { this._ownerInit = value; } }
    public int cmntNum { get { return this._cmntNum; } set { this._cmntNum = value; } }
    public int numHistoryDays { get { return this._numHistoryDays; } set { this._numHistoryDays = value; } }
    public int tradingEntityNum { get { return this._tradingEntityNum; } set { this._tradingEntityNum = value; } }
    public short portLocked { get { return this._portLocked; } set { this._portLocked = value; } }
    public int transId { get { return this._transId; } set { this._transId = value; } }
    #endregion

    public override string ToString() {
        return GetType().Name + " : " + portNum + " " + portType + " " + portShortName;
    }
}