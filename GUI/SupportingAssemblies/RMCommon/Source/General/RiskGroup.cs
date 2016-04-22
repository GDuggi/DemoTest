using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Reflection;
using com.amphora.cayenne.entity.support;
using NSRMCommon;
using NSRMLogging;
using com.amphora.entities;
using com.amphora.cayenne.entity.service;
using org.apache.cayenne;
using org.apache.cayenne.map;
using org.apache.cayenne.query;
using com.amphora.cayenne.entity;
using NSRiskManager.Constants;

namespace NSRMCommon
{
    public class RiskGroup : IEquatable<IPositionEntity>,IEquatable<RiskGroup> {
        #region constants
        const int HASH_CODE = 66668;
        public const string DEFAULT_NUMERIC_FORMAT = "##,###,###,##0.00 ;(###,###,###,##0.00);";
        #endregion

        #region fields
        public static bool verbose = false;
        static int nInstance = 0;
        static readonly IDictionary<int,RiskGroupAtom> posNumMap = new Dictionary<int,RiskGroupAtom>();

        public readonly List<RiskGroupAtom> atoms = new List<RiskGroupAtom>();
        readonly IDictionary<string,QuantityContainer> UnitsToQuantityTupleDictionary = new Dictionary<string,QuantityContainer>();
        readonly int instance;
        DateTime _dtRecalc;
        QuantityContainer _qt;
        int posNumToFind = -1;
        double PRECISION = 0.000001;
        private CommodityEntityService commodityEntityService = new CommodityEntityService(LocalCayenneService.sharedInstance);

        #endregion




        public void setAsOfRiskQty(double qty)
        {
            this.asOfRiskQty = qty;
        }
        public void setAsOfPhysicalQty(double qty)
        {
            this.asOfPhysicalQty = qty;
        }
        #region ctors
        public RiskGroup() {
            instance = ++nInstance;
            this.positionNumber = 0;
            _qt = new QuantityContainer();
            isNull = true;
        }

        public RiskGroup(IPositionEntity iPosEnty)
            : this()
        {

          

            RiskGroupAtom rga;
            DateTime dt;
            if (iPosEnty == null)
                throw new ArgumentNullException("iPosEnty", typeof(IPositionEntity).FullName + " is null!");
            isNull = false;

            try
            {
                this.qtyUom = ((IPositionEntity)iPosEnty).getQtyUomCode();
            }

            catch (Exception e)
            {
                this.qtyUom = "";
            }

            rga = addAtom(iPosEnty);
            this.cmdtyName = rga.cmdtyCode;
            dt = rga.tradingPrd;
            this.yearOfPeriod = dt.Year;
            this.dayOfPeriod = dt.Year * 10000 + dt.Month * 100 + dt.Day;
            this.monthOfPeriod = dt.Year * 10000 + dt.Month * 100 + dt.Day;
            this.hedge = rga.isHedgeInd;
            this.mktName = rga.mktCode;
            this.portfolio = rga.portNum;
            this.posSource = rga.posType;

            if (iPosEnty.getLastMtmPrice() != null)
                this.lastMtmPrice = iPosEnty.getLastMtmPrice().doubleValue();
            else
                this.lastMtmPrice = 0;

            double purchasePrice =0;
            if(iPosEnty.getAvgPurchPrice()!=null)
                 purchasePrice = iPosEnty.getAvgPurchPrice().doubleValue();

            double sellPrice = 0;
            if(iPosEnty.getAvgSalePrice() !=null)
                sellPrice = iPosEnty.getAvgSalePrice().doubleValue();
            
            if (purchasePrice > 0)
            {
                this.avgPurchaseSellPrice = purchasePrice;
            }
            else if (sellPrice < 0)
            {
                this.avgPurchaseSellPrice = sellPrice;
            }
            else
                this.avgPurchaseSellPrice = 0;

            this.displayPosSource = PositionType.getDisplayPositionSource(this.posSource, rga.isHedgeInd, rga.optionType);
          
           
            this.putCall = rga.putCallInd;
            this.strike = rga.strikePrice;
            this.tradingPeriod = rga.tradingPrd;
            resetRecalc();
        }

        #endregion

        #region overridden methods
        public override int GetHashCode() {
            return HASH_CODE;
        }
        public override string ToString() {
            return instance + " : " + this.posSource + " " + this.cmdtyName + " " + this.mktName + " " +

         this.tradingPeriod.ToString("MMM-yy")

 + (string.Compare(posSource,"X",true) == 0 ?
                    (" " + this.strike + " " + this.putCall) :
                    string.Empty) +
                " " + this.riskQty + " QtyUom=" + qtyUom + " Diff=" + timeDifference();
        }
        #endregion

        #region properties

        public int decimalPrecision { get; set; }

        /// <summary>
        /// Quantity from the distribution.
        /// </summary>
        /// <remarks>
        /// <para>Data-field only.</para>
        /// <para>Needs to be adjusted during display to match the selected UOM.</para>
        /// <para>Calculated as position.long_qty - position.short_qty.</para>
        /// <para>S,T,QKFWOX and ZIYP.</para>
        /// <para>Use primary or secondary quantities depending upon the selected UOM.</para>
        /// <para>e.g.: user selected volume UOM, use primary or secondary based upon volume-type.</para>
        /// </remarks>
        [DesiredPivotGridField("Risk Quantity",IsDataField = true,NumericFormat = DEFAULT_NUMERIC_FORMAT,DisplayFolder = "Quantity")]
        public double riskQty { get; set; }

       
        /// <summary>Delivery quantity</summary>
        /// <remarks>
        /// <para>Calculated as position.long_qty - postion.short_qty when pos_type is ZIYP.</para>
        /// <para>Be sure to use UOM-specific quantity.</para>
        /// <para></para>
        /// <para></para>
        /// <para></para>
        /// </remarks>
        [DesiredPivotGridField("Physical Quantity",IsDataField = true,DisplayFolder = "Quantity",NumericFormat = DEFAULT_NUMERIC_FORMAT)]
        public double physQty  { get; set; }
      

        /// <summary>Discounted quantity</summary>
        /// <remarks>
        /// <para>Calculated as riskQty - position.discount_qty</para>
        /// <para>Use primary or secondary quantites depending upon the UOM.</para>
        /// </remarks>
        [DesiredPivotGridField("Discount Quantity",IsDataField = true,DisplayFolder = "Quantity",NumericFormat = DEFAULT_NUMERIC_FORMAT)]
        public double discountQty { get; set; }

        [DesiredPivotGridField("Risk Rolling Total", IsDataField = true, IsRunningTotal = true, NumericFormat = DEFAULT_NUMERIC_FORMAT, DisplayFolder = "Quantity")]
        public double riskQtyRollingTotal
        {
            get
            {
                return riskQty;
            }
        }


        [DesiredPivotGridField("As Of Risk Qty", IsDataField = true, NumericFormat = DEFAULT_NUMERIC_FORMAT, DisplayFolder = "Historical Quantity", Alignment = PivotFieldAlignment.CENTER)]
        public double asOfRiskQty { get; private set; }


        [DesiredPivotGridField("As Of Physical Qty", IsDataField = true, NumericFormat = DEFAULT_NUMERIC_FORMAT, DisplayFolder = "Historical Quantity", Alignment = PivotFieldAlignment.CENTER)]
        public double asOfPhysicalQty { get; private set; }


        /// <summary></summary>
        /// <para>source: position.cmdty_code</para>
        [DesiredPivotGridField("Commodity",DisplayFolder = "Attributes",Alignment = PivotFieldAlignment.CENTER)]
        public string cmdtyName { get; set; }

        /// <summary></summary>
        /// <para>source: position.mkt_code</para>
        [DesiredPivotGridField("Market",DisplayFolder = "Attributes",Alignment = PivotFieldAlignment.CENTER)]
        public string mktName { get; set; }

        /// <summary>Trading period.</summary>
        /// <remarks>
        /// <para>The trading period, converted to reasonable dates for display, 
        /// but should be sortable.</para>
        /// <para>source: position.trading_prd</para>
        /// </remarks>
        [DesiredPivotGridField("Period",Alignment = PivotFieldAlignment.RIGHT,DisplayFolder = "Time")]

        public DateTime tradingPeriod { get; set; }

        /// <summary>Extracted month.</summary>
        /// <remarks>
        /// <para>sorted month/year order?</para>
        /// <para>display as MMM-yy</para>
        /// <para>source: position.trading_prd</para>
        /// </remarks>
        [DesiredPivotGridField("Month of Period",DisplayFolder = "Time",Alignment = PivotFieldAlignment.CENTER)]
        public int monthOfPeriod { get; private set; }

        /// <summary>Extracted year.</summary>
        /// <remarks>
        /// <para>sorted month/year order?</para>
        /// <para>display as YYYY</para>
        /// <para>source: position.trading_prd</para>
        /// </remarks>
        [DesiredPivotGridField("Year of Period",DisplayFolder = "Time",Alignment = PivotFieldAlignment.CENTER)]
        public int yearOfPeriod { get; private set; }

        /// <summary>Extracted day.</summary>
        /// <remarks>
        /// <para>sorted day/month/year order?</para>
        /// <para>display as dd-MMM</para>
        /// <para>source: position.trading_prd</para>
        /// </remarks>
        [DesiredPivotGridField("Day of Period",DisplayFolder = "Time",Alignment = PivotFieldAlignment.CENTER)]
        public int dayOfPeriod { get; private set; }


        private bool? _isZero = null;
        public bool isZero
        {
            get
            {
                if (_isZero == null)
                {
                    double longQuantityTotal = 0;
                    double shortQuantityTotal = 0;

                    foreach (RiskGroupAtom atom in atoms)
                    {
                        
                        longQuantityTotal += atom.longQty;
                        shortQuantityTotal += atom.shortQty;

                       
                    }

               
                 if ( Math.Abs((longQuantityTotal - shortQuantityTotal)) < PRECISION)
                        _isZero = true;

                    else
                        _isZero = false;
                }
                

                return Convert.ToBoolean(_isZero);
            }
          
        }
    
        public string EquivalentPositions
        {
            get
            {
                foreach (RiskGroupAtom atom in atoms)
                {
                   string returnString = "";

                   if (atom.isEquivInd == true)
                       returnString = "Underlying";
                   else if (atom.equivSource == true)
                       returnString = "Option";
                   else
                       returnString = "Outright";

                   return returnString;
                }
                return "";
            }
            
        }


        public double lastMtmPrice { get; set; }
        public double avgPurchaseSellPrice { get; set; }

        private string _formattedTradePeriod = "";
        public string formattedTradePeriod
        {
            get
            {
                if (_formattedTradePeriod.Length > 0)
                    return _formattedTradePeriod;

                _formattedTradePeriod = tradingPeriod.ToString("MMM-yy");

                return _formattedTradePeriod;
            }
        }


        /// <summary></summary>
        /// <para>source: position.port_num</para>
        [DesiredPivotGridField("Portfolio",DisplayFolder = "Misc",Alignment = PivotFieldAlignment.CENTER)]
        public int portfolio { get; private set; }

      

        /// <summary></summary>
        /// <para>source: position.is_hedge_ind</para>
        
        public bool hedge { get; private set; }

        [DesiredPivotGridField("Hedge State", DisplayFolder = "Misc", Alignment = PivotFieldAlignment.CENTER)]
        public string hedgeState 
        {
            get 
            {
                if (hedge == true)
                    return "Hedge";
                return "Unassigned";
            }
        }

        /// <summary>Position type.</summary>
        /// <remarks>
        /// <para>As shownin <b>PortfolioManager</b>, e.g. <i>Swap Formula Hedge</i></para>
        /// <para>source: position.pos_type</para>
        /// </remarks>
        [DesiredPivotGridField("Position Source",DisplayFolder = "Misc",Alignment = PivotFieldAlignment.CENTER)]
        public string displayPosSource { get; private set; }
        public string posSource { get; private set; }

        private string _commodityGroup = "";

        [DesiredPivotGridField("Commodity Group", DisplayFolder = "Misc", Alignment = PivotFieldAlignment.CENTER)]
        public string commodityGroup
        {

            get
            {
                try
                {
                    if (_commodityGroup.Length > 0)
                        return _commodityGroup;


                    _commodityGroup = "";

                    ObjectContext cntx = commodityEntityService.createObjectContext();


                    string sqlQuery =
                            "SELECT parent_cmdty_code as parentCommodity FROM commodity_group where cmdty_code='" + cmdtyName + "' AND cmdty_group_type_code = 'POSITION'";

                    SQLTemplate sqlTemplateQuery = new SQLTemplate();
                    sqlTemplateQuery.setDefaultTemplate(sqlQuery);
                  

                    SQLResult resultDescriptor = new SQLResult();
                    resultDescriptor.addColumnResult("parentCommodity");

                    sqlTemplateQuery.setResult(resultDescriptor);

                    var x = sqlTemplateQuery.toString();

                    object [] theResult = cntx.performQuery(sqlTemplateQuery).toArray();

                    if (theResult.Length > 0)
                    {
                        //just take the first result
                        _commodityGroup = theResult[0].ToString();
                    }

                }
                catch (Exception ex)
                { 
                }

                return _commodityGroup;
		
            }
        }
   
        /// <summary>Put / Call, for an option.</summary>
        /// <para>source: position.put_call_ind</para>
        [DesiredPivotGridField("Put / Call",DisplayFolder = "Attributes",Alignment = PivotFieldAlignment.CENTER)]
        public string putCall { get; private set; }

        /// <summary>Option strike price.</summary>
        /// <remarks>
        /// <para>source: position.strike_price</para>
        /// </remarks>
        [DesiredPivotGridField("Strike Price",DisplayFolder = "Attributes",Alignment = PivotFieldAlignment.RIGHT)]
        public double strike { get; private set; }

        [Browsable(false)]
        public bool showAgain { get; set; }

        [Browsable(false)]
        bool needRecalc { get; set; }

         
        [Browsable(false)]
        public int positionNumber { get; private set; }

        [Browsable(false)]
        public string qtyUom { get; private set; }

        [Browsable(false)]
        public bool isNull { get; private set; }
        #endregion

        #region methods
        void resetRecalc() {
            needRecalc = true;
            _dtRecalc = DateTime.MinValue;
            // TODO: remove this, after testing is complete.
            UnitsToQuantityTupleDictionary.Clear();
        }

        RiskGroupAtom addAtom(IPositionEntity iPE)
        {
            if (iPE == null)
                throw new ArgumentNullException("iPE", typeof(IPositionEntity).FullName + "-instance is null!");
            return addAtom(new RiskGroupAtom(iPE));
        }
        RiskGroupAtom addAtom(RiskGroupAtom rga) {
            int key;

            if (rga == null)
                throw new ArgumentNullException("rga",typeof(RiskGroupAtom).FullName + "-instance is null!");
            if (posNumMap.ContainsKey(key = rga.posNum)) 
            {
                posNumToFind = key;
                var atomsByPositonNum = atoms.FindAll(findByPositionNmber);
                posNumToFind = -1;
                if (atomsByPositonNum.Count > 0)
                    foreach (var atom in atomsByPositonNum)
                        atoms.Remove(atom);
                posNumMap.Remove(key);
            }

            if (string.Compare(rga.qtyUom,this.qtyUom,true) != 0)
                Util.show(MethodBase.GetCurrentMethod(),"Different UOM's!");

            atoms.Add(rga);

            if (posNumMap.ContainsKey(key))
            {
                posNumMap[key] = rga;
                Util.show(System.Reflection.MethodBase.GetCurrentMethod(), "pos-num " + key + " already exists!");
            }
            else
              posNumMap.Add(key, rga);

            

            rebuildPosnumVector();
            resetRecalc();
            return rga;
        }

        void rebuildPosnumVector() {
            List<int> tmpNums = new List<int>();

            foreach (var var0 in this.atoms)
                tmpNums.Add(var0.posNum);

            //position numbers should really be the same for same risk group
            //so simplify this code later
            this.positionNumber = tmpNums.ToArray()[0];
        }


        private Dictionary<string, bool> TabDirtyStatus = new Dictionary<string, bool>();


        public void setTabDirtyStatus(string fieldKey, bool value)
        {


            bool isTabDirty = false; ;
            bool found = TabDirtyStatus.TryGetValue(fieldKey, out isTabDirty);

            if (found)
                TabDirtyStatus.Remove(fieldKey);

            TabDirtyStatus.Add(fieldKey, value);
        }



        public bool getTabDirtyStatus(string fieldKey)
        {
            bool isTabDirty = false; ;
            bool found = TabDirtyStatus.TryGetValue(fieldKey, out isTabDirty);

            if (found)
                return isTabDirty;

            return false;
        }


        bool findByPositionNmber(RiskGroupAtom rga) {
            if (posNumToFind < 0)
                throw new InvalidOperationException("invalid 'posNumMap' value!");
            return rga.posNum == posNumToFind;
        }

        public bool Equals(IPositionEntity other)
        {
            DateTime dt;
            
            if (this.posSource.CompareTo(other.getPosType()) != 0) return false;
            if (string.Compare(this.cmdtyName, other.getCommodityCode(), true) != 0) return false;
            if (string.Compare(this.mktName, other.getMarketCode(), true) != 0) return false;
            dt = LocalPeriodUtil.datetimeFrom(other.getTradingPeriod());
            if (string.IsNullOrEmpty(other.getTradingPeriod()))
            {
                return dt.Equals(DateTime.MinValue);
            }
            else if (dt.Date != this.tradingPeriod.Date) return false;
            if (string.Compare(this.qtyUom, other.getQtyUomCode(), true) != 0) return false;
            if (this.posSource.CompareTo("X") == 0)
            {
                if (this.strike != other.getStrikePrice().doubleValue()) return false;
                if (this.putCall.CompareTo(other.getPutCallInd()) != 0) return false;
            }
            return true;
        }

        public string HashKey()
        {
            string posSourceLocal = posSource;

            if (posSource == null)
            {
                posSourceLocal = "";
            }

            string cmdtyNameLocal = cmdtyName;

            if (cmdtyName == null)
            {
                cmdtyNameLocal = "";
            }

            string mktNameLocal = mktName;
            if (mktName == null)
            {
                mktNameLocal = "";
            }


            string hashString =  posSourceLocal + ":"+cmdtyNameLocal+":"+mktNameLocal+":"+tradingPeriod.ToString()+":"+
                qtyUom+":"+ positionNumber.ToString() + ":"+portfolio.ToString();


            if (posSourceLocal== "X")
            {
                hashString += (":" + strike.ToString() + ":" + putCall.ToString());
            }

            return hashString;
        }


        
        public bool Equals(RiskGroup other)
        {
            if (posSource == null)
            {
                return false;
            }
            if (other == null)
            {
                return false;
            }

            if (cmdtyName == null)
            {
                return false;
            }
            if (mktName == null)
            {
                return false;
            }

            if (
                (posSource.CompareTo(other.posSource) == 0) &&
                (cmdtyName.CompareTo(other.cmdtyName) == 0) &&
                (mktName.CompareTo(other.mktName) == 0) &&
                (this.tradingPeriod.Date.CompareTo(other.tradingPeriod.Date) == 0) &&
                (string.Compare(qtyUom, other.qtyUom, true) == 0) &&
                (this.positionNumber == other.positionNumber) && //we should only use one position number per risk group!
                (this.portfolio == other.portfolio))
            {
                if (posSource.CompareTo(other.posSource) == 0 && string.Compare(posSource, "X", true) == 0)
                {
                    if (strike.CompareTo(other.strike) == 0 && putCall.CompareTo(other.putCall) == 0)
                        return true;
                    return false;
                }
                return true;
            }
            return false;
        }

        public void merge(RiskGroup rg) {
            foreach (var atom in rg.atoms)
                addAtom(atom);
            resetRecalc();
        }



        public int timeDifference() {
            if (_dtRecalc == DateTime.MinValue) return Int32.MaxValue - 1;
            return Convert.ToInt32((DateTime.Now - this._dtRecalc).TotalMilliseconds);
        }

        
        public void  resetCalc() {
            resetRecalc();
        }



        public object calculateHistoricalValueOfField(string fieldName, string toUnits, java.util.ArrayList arrayListPositionHistory)
        {

            object ret = double.MinValue;
            QuantityContainer quantityHelper = null;

            if (string.Compare("asOfRiskQty", fieldName, true) == 0 || string.Compare("asOfPhysicalQty", fieldName, true) == 0)
            {

                List<RiskGroupAtom> pHisAtoms = new List<RiskGroupAtom>();
                foreach (PositionHistory phis in arrayListPositionHistory)
                {
                    pHisAtoms.Add(addAtom(new RiskGroupAtom(phis)));
                }
                quantityHelper = new QuantityContainer();
                if (pHisAtoms != null && pHisAtoms.Count > 0)
                {
                    quantityHelper.convertQuantityToUnits(this.posSource, pHisAtoms, verbose, toUnits, true);
                    UnitsToQuantityTupleDictionary.Add(toUnits, quantityHelper);
                }
                if (string.Compare("asOfRiskQty", fieldName, true) == 0)
                {
                    ret = quantityHelper.asOfRiskQty;
                }
                else if (string.Compare("asOfPhysicalQty", fieldName, true) == 0)
                {
                    ret = quantityHelper.asOfphysicalQty;
                }

            }

            return ret;
        }

        public object calculateRegularValueOfField(string fieldName, string toUnits)
        {
            object ret = double.MinValue;
            QuantityContainer quantityHelper = null;

            if (string.Compare("riskQty", fieldName, true) == 0 || string.Compare("physQty", fieldName, true) == 0 || string.Compare("discountQty", fieldName, true) == 0)
                {
                    if (!UnitsToQuantityTupleDictionary.ContainsKey(toUnits))
                    {
                        quantityHelper = new QuantityContainer();
                        quantityHelper.convertQuantityToUnits(this.posSource, this.atoms,  verbose, toUnits, false);
                        UnitsToQuantityTupleDictionary.Add(toUnits, quantityHelper);
                    }
                    else
                        quantityHelper = UnitsToQuantityTupleDictionary[toUnits];
                    if (string.Compare("riskQty", fieldName, true) == 0)
                        ret = quantityHelper.riskQty;
                    else if (string.Compare("physQty", fieldName, true) == 0)
                        ret = quantityHelper.physicalQty;
                    else if (string.Compare("discountQty", fieldName, true) == 0)
                        ret = quantityHelper.discountQty;
            }

            return ret;
        }

        public object calculateValueOfField(string fieldName, string toUnits,  bool forAsOfDate, java.util.ArrayList arrayListPositionHistory)
        {
            object ret = double.MinValue;

            if (forAsOfDate)
            {
                ret = calculateHistoricalValueOfField(fieldName, toUnits, arrayListPositionHistory);
            }
            else
            {
                ret = calculateRegularValueOfField(fieldName, toUnits);
                
            }
            return ret;
        }
        #endregion
    }

 
}