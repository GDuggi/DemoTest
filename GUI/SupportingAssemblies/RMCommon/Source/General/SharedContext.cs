using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Reflection;
using System.Text.RegularExpressions;
using com.amphora.cayenne.entity;
using DevExpress.XtraEditors.DXErrorProvider;
using java.util;
using NSRMLogging;
using org.apache.cayenne;
using org.apache.cayenne.exp;
using org.apache.cayenne.query;
using org.apache.log4j;
using JLIST = java.util.List;
using NSRiskManager.Constants;
using NSRiskMgrCtrls;

namespace NSRMCommon {
    public class SharedContext {
        #region constants
        const string FN_PARENT_PORT = "parentPortfolio";
        #endregion constants

        #region fields
        public static SharedContext sharedInstance = new SharedContext();
        static readonly IDictionary<string,List<WrappedErrorProvider>> portEntityData = new Dictionary<string,List<WrappedErrorProvider>>();

        static List<WrappedUser> traders;
        static List<WrappedLocation> officeLocations;
        static List<WrappedDepartment> profitCenters;
        static List<WrappedPortTagDef> portTagDefs;
        #endregion

        #region properties
        public static bool didSetupLogging { get; private set; }
        #endregion

        #region ctor
        SharedContext() {
            setupLogging();
            JavaConnection.makeConnection();
        }
        #endregion

        #region generic methods
        /// <summary>setup logging and jdbc connection-strings.</summary>
        /// <seealso cref="setupLogging"/>
        /// <seealso cref="JavaConnection.makeConnection()"/>
        public static void setup() {
            setupLogging();
            JavaConnection.makeConnection();
        }

        /// <summary>Establish java logging.</summary>
        static void setupLogging() {
            if (!didSetupLogging) {
                didSetupLogging = true;
                Logger.getRootLogger().getLoggerRepository().resetConfiguration();
                ConsoleAppender ca = new ConsoleAppender(new PatternLayout(PatternLayout.TTCC_CONVERSION_PATTERN));
                ca.setName("riktest");
                Logger.getRootLogger().addAppender(ca);
            }
        }

        public static void enableLogging(bool enable) {
            Logger.getRootLogger().setLevel(enable ? Level.ALL : Level.ERROR);
        }

        static T executeQuery<T>(Expression exp,ObjectContext ctx) where T : class {
            return executeQuery<T>(exp,ctx,false);
        }

        static T executeQuery<T>(Expression exp,bool doLog) where T : class {
            return executeQuery<T>(exp,LocalCayenneService.sharedInstance.sharedContext(),doLog);
        }

        /// <summary>Generic method to fetch database information.</summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="exp"></param>
        /// <param name="ctx"></param>
        /// <param name="logVerbose"></param>
        /// <returns></returns>
        static T executeQuery<T>(Expression exp,ObjectContext ctx,bool logVerbose) where T : class {
            SelectQuery query;
            java.util.List ret;
            T retValue = null;

            Level prevLevel = Level.OFF;

            if (logVerbose) {
                prevLevel = Logger.getRootLogger().getLevel();
                Logger.getRootLogger().setLevel(Level.ALL);
            }
            try {
                if ((ret = ctx.select(query = new SelectQuery(typeof(T),exp))) != null &&
                    ret.size() > 0)
                    retValue = (T) ret.get(0);
            } catch (Exception ex) {
                Util.show(MethodBase.GetCurrentMethod(),ex);
            } finally {
                if (logVerbose)
                    Logger.getRootLogger().setLevel(prevLevel);
            }
            return retValue;
        }

        /// <summary>read a generic list, from the database.</summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="ex"></param>
        /// <returns></returns>
        public static List<T> readList<T>(Expression ex) {
            return readList<T>(ex,false);
        }

        /// <summary>read a generic list, from the database.</summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="ex"></param>
        /// <param name="doLog"></param>
        /// <returns></returns>
        public static List<T> readList<T>(Expression ex,bool doLog) {
            return readList<T>(ex,LocalCayenneService.sharedInstance.sharedContext(),doLog);
        }

        /// <summary>Read a generic list from the database, storing objects into 
        /// a specific object-context.</summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="exp"></param>
        /// <param name="ctx"></param>
        /// <param name="logVerbose"></param>
        /// <returns></returns>
        public static List<T> readList<T>(Expression exp,ObjectContext ctx,bool logVerbose) {
            List<T> ret = new List<T>();
            java.util.List tmp;
            int n;
            Level prevLevel = Level.OFF;

            if (logVerbose) {
                prevLevel = Logger.getRootLogger().getLevel();
                Logger.getRootLogger().setLevel(Level.ALL);
            }
            try {
                if ((tmp = ctx.select(new SelectQuery(typeof(T),exp))) != null && (n = tmp.size()) > 0)
                    for (int i = 0 ;i < n ;i++)
                        ret.Add((T) tmp.get(i));
            } catch (Exception ex) {
                Util.show(MethodBase.GetCurrentMethod(),ex);
            } finally {
                if (logVerbose)
                    Logger.getRootLogger().setLevel(prevLevel);
            }
            return ret;
        }
        #endregion

        #region methods
        /// <summary>Find a specific Uom.</summary>
        /// <param name="uomCode">a <see cref="string"/> containing the uom-code to find.</param>
        /// <returns>a <see cref="Uom"/> if <paramref name="uomCode"/> is found, null otherwise.</returns>

        const string BASE_PREFIX = "#,##0";
        public static string getCustomFormatForDoubles(int numDecimals)
        {
            string tmp = new string('0', numDecimals);
            string fmt = BASE_PREFIX + "." + tmp + " ;(" + BASE_PREFIX + "." + tmp + ");";

            return fmt;
        }

        static readonly Dictionary<int, ILotInfo> lotSizeMap = new Dictionary<int, ILotInfo>();
        public static double getLotSize(string qtyUOM, int commktKey)
        {
            double lotSize = 0;
            ILotInfo lotInfo;


            if (!lotSizeMap.ContainsKey(commktKey))
            {
                if ((lotInfo = SharedContext.findFutureAttrLotInfo(commktKey)) != null)
                    lotSizeMap.Add(commktKey, lotInfo);

            }
            else
                lotInfo = lotSizeMap[commktKey];

            if (lotInfo != null)
            {
                lotSize = lotInfo.lotSize;
            }
            return lotSize;
        }


        public static bool isRightRiskMode(string positionType, bool riskQuantityView, bool physicalQuantityView, bool discountQuantityView)
        {
          bool retval = false;


          if (riskQuantityView || discountQuantityView)
          {
              if (positionType == PositionType.POS_TYPE_EXCH_IMBALANCE ||
                  positionType == PositionType.POS_TYPE_INVENTORY ||
                  positionType == PositionType.POS_TYPE_DERIVED_CURRENCY ||
                  positionType == PositionType.POS_TYPE_PHYS_EXCHANGE ||
                  positionType == PositionType.POS_TYPE_SYNTHETIC ||
                  positionType == PositionType.POS_TYPE_QUOTE ||
                  positionType == PositionType.POS_TYPE_RISK_FORMULA ||
                  positionType == PositionType.POS_TYPE_PHYSICAL ||
                  positionType == PositionType.POS_TYPE_MARKET_FORMULA ||
                  positionType == PositionType.POS_TYPE_FUTURE ||
                  positionType == PositionType.POS_TYPE_OTC_OPTION ||
                  positionType == PositionType.POST_TYPE_LISTED_OPTION)

                  return retval = true;

          }
          if (physicalQuantityView)
          {
              if (positionType == PositionType.POS_TYPE_PHYSICAL || positionType == PositionType.POS_TYPE_MARKET_FORMULA) 
                return retval = true;

          }

            return retval;
        }


        public static Uom findUom(string uomCode) {
            return executeQuery<Uom>(
                ExpressionFactory.matchExp("uomCode",uomCode),
                LocalCayenneService.sharedInstance.sharedContext());
        }

        public static List<Portfolio> readJavaPorts() {
            List<Portfolio> ret = readList<Portfolio>(ExpressionFactory.matchExp("portType","IW"));

            return ret;
        }

        public static List<WrappedUom> readUoms() {
            List<WrappedUom> ret = new List<WrappedUom>();
            var avar = readList<Uom>(ExpressionFactory.noMatchExp("uomType","I"),false);

            if (avar != null)
                foreach (Uom auom in avar)
                    ret.Add(new WrappedUom(auom));
            return ret;
        }

        public static List<Portfolio> findChildPortfolios(int parentPortNum) {
            List<Portfolio> ret = new List<Portfolio>();
            var avar = readList<PortfolioGroup>(ExpressionFactory.matchExp(FN_PARENT_PORT,new java.lang.Integer(parentPortNum)));

            foreach (var avar2 in avar)
                ret.Add(avar2.getPortfolio());

            return ret;
        }


        public static java.lang.Integer[] makeVector(int[] portNums) {
            java.lang.Integer[] ret;
            int n = portNums == null ? 0 : portNums.Length;

            // TODO: add another one!
            ret = new java.lang.Integer[n];
            if (n > 0)
                for (int i = 0 ;i < n ;i++)
                    ret[i] = new java.lang.Integer(portNums[i]);
            return ret;
        }

        public static List<WrappedDistribution> readDistributions(int[] positionNums, Dictionary<int, char> positionTypes, Dictionary<int, string> commodityCodes , Dictionary<int, string> originalUOMs,  int decimalPrecision, string UOM, bool equivIndChecked) 
        {
            List<WrappedDistribution> ret = new List<WrappedDistribution>();
            ObjectContext ctx;
            Level prevLevel = Level.OFF;
            bool doLog = true;
            int n;

            if (positionNums != null && positionNums.Length > 0) 
            {
                ctx = LocalCayenneService.sharedInstance.sharedContext();
                var someList = makeVector(positionNums);

                try {
                    if (doLog) 
                    {
                        prevLevel = Logger.getRootLogger().getLevel();
                        Logger.getRootLogger().setLevel(Level.ALL);
                    }
                    var distributions = ObjectSelect.
                        query(typeof(TradeItemDist)).
                            where(ExpressionFactory.inExp("posNum",someList)).
                                select(ctx);

                    if (distributions != null && (n = distributions.size()) > 0)
                        for (int i = 0; i < n; i++)
                        {


                            TradeItemDist tradeItemDist = distributions.get(i) as TradeItemDist;
                            WrappedDistribution distribution = new WrappedDistribution(tradeItemDist);

                            int positionNum = tradeItemDist.getPosNum().intValue();

                            if (equivIndChecked && distribution.EquivSourceInd) //we want to view underlying, but option is selected
                                continue;
                            else if (!equivIndChecked && distribution.isEquivInd) //we want to view options, but underlying is selected
                                continue;

                            // else this is an outright or an option or an underlying that we want to see


                            if (SharedContext.isRightRiskMode(positionTypes[positionNum].ToString(), true, true, true))
                            {

                                distribution.positionType = positionTypes[positionNum];
                                distribution.finalUomToConvertTo = UOM;
                                distribution.decimalPrecision = decimalPrecision;
                                distribution.commodityCode = commodityCodes[positionNum];
                                distribution.originalquantityUom = tradeItemDist.getQuantityUom().getUomCode().ToString();
                                distribution.originalUomToConvertTo = tradeItemDist.getQuantityConvUom().getUomCode().ToString();

                                if (distribution.originalquantityUom == "lots" || distribution.originalquantityUom == "LOTS")
                                {
                                    distribution.originalquantityUom = "BBL";
                                    distribution.lotSize = SharedContext.getLotSize(distribution.originalquantityUom, distribution.commktKey);
                                }
                                else
                                    distribution.lotSize = 1;

                                ret.Add(distribution);

                            }


                           
                        }
                } 
                catch (Exception ex) 
                {
                    Util.show(MethodBase.GetCurrentMethod(),ex);
                } 
                finally 
                {
                    if (doLog)
                        Logger.getRootLogger().setLevel(prevLevel);
                }
            }
            return ret;
        }


     

        public static IEnumerable<IPLRecord> readPLRecords(int[] p) {
            List<WrappedPL> ret = new List<WrappedPL>();
            List<PortfolioProfitLoss> blah;

            if (p != null)
                foreach (int aPortNum in p) {
                    blah = readList<PortfolioProfitLoss>(ExpressionFactory.matchExp("portNum",new java.lang.Integer(aPortNum)));
                    foreach (PortfolioProfitLoss appl in blah)
                        ret.Add(new WrappedPL(appl));
                }
            return ret;
        }

        /// <summary>This collection is used to populate the left-hand colum of 
        /// the PortfolioEditor grid.</summary>
        /// <returns>a <see cref="List&lt;T&gt;"/> of <see cref="WrappedPortTagDef"/>.</returns>
        /// <remarks ><para>This method is cached.</para></remarks>
        /// <seealso cref="portTagDefs"/>
        /// <seealso cref="WrappedPortTagDef"/>
        /// <seealso cref="PortfolioTagDefinition"/>
        public static List<WrappedPortTagDef> readPortTagDefs() {
            if (portTagDefs == null) {
                var avar = readList<PortfolioTagDefinition>(ExpressionFactory.expTrue());
                portTagDefs = new List<WrappedPortTagDef>();
                foreach (PortfolioTagDefinition ptd in avar)
                    portTagDefs.Add(new WrappedPortTagDef(ptd));
            }
            return portTagDefs;
        }

        public static List<WrappedCurrency> readCurrencies() {
            JLIST currencies;
            List<WrappedCurrency> ret;
            int n;

            currencies = ObjectSelect.query(typeof(Commodity)).
                where(
                    ExpressionFactory.matchExp("cmdtyType","C").andExp(
                        ExpressionFactory.noMatchExp("cmdtyStatus","I"))).
                select(LocalCayenneService.sharedInstance.sharedContext());
            ret = new List<WrappedCurrency>();
            if (currencies != null
                && (n = currencies.size()) > 0)
                for (int i = 0 ;i < n ;i++)
                    ret.Add(new WrappedCurrency(currencies.get(i) as Commodity));
            return ret;
        }

        public static Portfolio findPortfolio(int newPortNum) {
            var avar = readList<Portfolio>(
                ExpressionFactory.matchExp(
                "portNum",new java.lang.Integer(newPortNum)));
            if (avar != null && avar.Count > 0)
                return avar[0];
            return null;
        }

        public static List<WrappedUser> readTraders() {
            int n;

            if (traders == null) {
                traders = new List<WrappedUser>();
                var avar = readList<IctsUser>(
                    ExpressionFactory.matchExp(
                    "userJobTitle","TRADER").andExp(ExpressionFactory.noMatchExp("userStatus","I")),false);

                if (avar != null && (n = avar.Count) > 0)
                    foreach (IctsUser aUser in avar)
                        traders.Add(new WrappedUser(aUser));
            }
            return traders;
        }

        public static List<WrappedLocation> readOfficeLocations() {
            int n;

            if (officeLocations == null) {
                officeLocations = new List<WrappedLocation>();
                var avar = readList<Location>(
                    ExpressionFactory.matchExp(
                    "officeLocInd","Y"));

                if (avar != null && (n = avar.Count) > 0)
                    foreach (Location aloc in avar)
                        officeLocations.Add(new WrappedLocation(aloc));
            }
            return officeLocations;
        }

        public static List<WrappedDepartment> readProfitCenters() {
            int n;

            if (profitCenters == null) {
                profitCenters = new List<WrappedDepartment>();
                var avar = readList<Department>(
                    ExpressionFactory.matchExp(
                    "profitCenterInd","Y"));

                if (avar != null && (n = avar.Count) > 0)
                    foreach (Department aDept in avar)
                        profitCenters.Add(new WrappedDepartment(aDept));
            }
            return profitCenters;
        }

        public static List<WrappedAccount> readAccounts(string p) {
            /*
 * select acct_short_name from account where acct_type_code='PEICOMP' and acct_status<>'I'
 * */
            List<WrappedAccount> ret = new List<WrappedAccount>();
            int n;

            var avar = readList<Account>(
                ExpressionFactory.matchExp("acctTypeCode",p).
                    andExp(ExpressionFactory.noMatchExp("acctStatus","I")),false);

            if (avar != null && (n = avar.Count) > 0)
                foreach (Account aDept in avar)
                    ret.Add(new WrappedAccount(aDept));
            return ret;
        }

        public static List<WrappedEntityTagOption> readEntityTag(string key) {
            List<EntityTagDefinition> avar;
            EntityTagDefinition tagDef;
            List<WrappedEntityTagOption> ret = new List<WrappedEntityTagOption>();

            avar = readList<EntityTagDefinition>(
                ExpressionFactory.matchExp("entityTagName",key),false);
            if (avar.Count > 0) {
                tagDef = avar[0];
                var avar2 = readList<EntityTagOption>(
                    ExpressionFactory.matchExp("entityTagId",tagDef.getOid()),false);
                if (avar2.Count > 0)
                    foreach (EntityTagOption eto in avar2)
                        ret.Add(new WrappedEntityTagOption(eto));
            }
            return ret;
        }
        #endregion

        public static void loadPortfolioInspectorData() {
            loadPortfolioEntityDatum(readPortTagDefs());
        }

        public static object portfolioDataForEntityTag(string tagValue) {
            if (string.IsNullOrEmpty(tagValue))
                throw new ArgumentNullException(tagValue,"tagValue is null!");
            tryAddingSpecificPortTagDef(tagValue);
            if (portEntityData.ContainsKey(tagValue))
                return portEntityData[tagValue];
            return null;
        }

        static void tryAddingSpecificPortTagDef(string tagValue) {
            if (string.IsNullOrEmpty(tagValue))
                throw new ArgumentNullException(tagValue,"null!");
            if (portEntityData.ContainsKey(tagValue)) return;
            List<PortfolioTagDefinition> tmp = readList<PortfolioTagDefinition>(
                ExpressionFactory.matchExp("tagName",tagValue).andExp(
                ExpressionFactory.noMatchExp("refInspName",null)));
            portEntityData.Add(tagValue,convertToErrorClass(tmp));
            return;
        }

        static List<WrappedErrorProvider> convertToErrorClass(List<PortfolioTagDefinition> tmp) {
            List<WrappedErrorProvider> ret = new List<WrappedErrorProvider>();

            if (tmp != null)
                foreach (var avar in tmp)
                    ret.Add(new WrappedErrorProvider(avar));
            return ret;
        }

        static List<WrappedErrorProvider> convertToErrorClass(List<WrappedEntityTagOption> tmp) {
            List<WrappedErrorProvider> ret = new List<WrappedErrorProvider>();

            if (tmp != null)
                foreach (var avar in tmp)
                    ret.Add(new WrappedErrorProvider(avar));
            return ret;
        }

        static List<WrappedErrorProvider> convertToErrorClass(List<PriceSource> tmp) {
            List<WrappedErrorProvider> ret = new List<WrappedErrorProvider>();

            if (tmp != null)
                foreach (var avar in tmp)
                    ret.Add(new WrappedErrorProvider(avar));
            return ret;
        }

        static List<WrappedErrorProvider> convertToErrorClass(List<WrappedUser> tmp) {
            List<WrappedErrorProvider> ret = new List<WrappedErrorProvider>();

            if (tmp != null)
                foreach (var avar in tmp)
                    ret.Add(new WrappedErrorProvider(avar));
            return ret;
        }

        static List<WrappedErrorProvider> convertToErrorClass(List<WrappedAccount> tmp) {
            List<WrappedErrorProvider> ret = new List<WrappedErrorProvider>();

            if (tmp != null)
                foreach (var avar in tmp)
                    ret.Add(new WrappedErrorProvider(avar));
            return ret;
        }

        public static object convertToErrorClass(List<WrappedPortTagDef> tmp) {
            List<WrappedErrorProvider> ret = new List<WrappedErrorProvider>();

            if (tmp != null)
                foreach (var avar in tmp)
                    ret.Add(new WrappedErrorProvider(avar));
            return ret;
        }

        static void loadPortfolioEntityDatum(List<WrappedPortTagDef> list) 
        {
            foreach (WrappedPortTagDef wptd in portTagDefs)
                addPortTagDef(wptd);
        }

        public static object addPortTagDef(WrappedPortTagDef wptd) {
            return loadTagDef(wptd.inspectorName,wptd.tagName);
        }

        static object loadTagDef(string inspectorName,string tagName) {
            string key;
            List<WrappedErrorProvider> val;

            // FOR NOW, DON'T CARE ABOUT AN INSPECTOR FOR TRADERS
            if (!string.IsNullOrEmpty(key = inspectorName) || tagName.StartsWith("TRADER")) {
                if (!portEntityData.ContainsKey(key = tagName)) {
                    val = null;

                    if (!portEntityData.ContainsKey(key)) {
                        switch (key = key.ToUpper()) {
                            case "BOOKCOMP": val = convertToErrorClass(SharedContext.readAccounts("PEICOMP")); break;
                            case "TRADER":
                            case "TRADER2":
                            case "TRADER3":
                            case "TRADER4":
                            case "TRADER5":
                                val = convertToErrorClass(SharedContext.readTraders()); break;
                            case "INTRNSRC": val = convertToErrorClass(readList<PriceSource>(ExpressionFactory.matchExp("priceSourceType","S"))); break;
                            default: Util.show(MethodBase.GetCurrentMethod(),"no such query: " + key); break;
                        }
                        if (val != null)
                            portEntityData.Add(key, val);
                      
                           
                    }
                    if (portEntityData.ContainsKey(tagName))
                        return portEntityData[tagName];
                } else
                    return portEntityData[key];
            } else {
                if (!portEntityData.ContainsKey(key = tagName))
                    portEntityData[key] = convertToErrorClass(SharedContext.readEntityTag(key));
            }
            return portEntityData[tagName];
        }

        public void test() {
            const int PARENT = 16;
            const int CHILD = 1548916;

            removePortGroupEntry(LocalCayenneService.sharedInstance.sharedContext(),PARENT,CHILD);
        }

        static void removePortGroupEntry(ObjectContext ctx,int parentPortNum,int childPortNum) {
            // TODO: remove this

            JLIST alist = ctx.select(
                new SelectQuery(
                    typeof(PortfolioGroup),
                    ExpressionFactory.matchExp("parentPortNum",new java.lang.Integer(parentPortNum)).
                        andExp(
                            ExpressionFactory.matchExp("portNum",new java.lang.Integer(childPortNum)))));
            if (alist != null && alist.size() > 0) {
                Logger logger = Logger.getRootLogger();
                var prevLevel = logger.getLevel();
                logger.setLevel(Level.ALL);
                try {
                    for (int i = 0 ;i < alist.size() ;i++)
                        ctx.deleteObject(alist.get(i));
                    ctx.commitChanges();
                } 
                catch (Exception ex) {
                    Util.show(MethodBase.GetCurrentMethod(),ex);
                } finally {
                    logger.setLevel(prevLevel);
                }
            }
        }

        public static ILotInfo findFutureAttrLotInfo(int cmKey) {
            return CommonAttrLotInfo.readFutureAttr(cmKey);
        }

        internal CommktFutureAttr readFutAttr(int cmKey) {
            ObjectContext ctx = LocalCayenneService.sharedInstance.sharedContext();
            object tmp = ctx.selectOne(
                new SelectQuery(
                    typeof(CommktFutureAttr),
                    ExpressionFactory.matchExp("commktKey",new java.lang.Integer(cmKey))));
            return (CommktFutureAttr) tmp;
        }
    }

    public interface ILotInfo {
        int commktKey { get; }
        double lotSize { get; }
        bool isOption { get; }
    }

    public abstract class CommonAttrLotInfo : ILotInfo {

        internal static FutureAttrLotInfo readFutureAttr(int cmKey) {
            CommktFutureAttr cfa = SharedContext.sharedInstance.readFutAttr(cmKey);

            if (cfa == null)
                return null;
            return new FutureAttrLotInfo(cfa);
        }

        #region ILotInfo implementation
        public int commktKey { get; protected set; }
        public double lotSize { get; protected set; }
        public bool isOption { get; protected set; }
        #endregion ILotInfo implementation
    }

    public class FutureAttrLotInfo : CommonAttrLotInfo {
        public FutureAttrLotInfo(CommktFutureAttr cfa) {
            if (cfa == null)
                throw new ArgumentNullException("cfa",typeof(CommktFutureAttr).FullName + " is null!");
            base.commktKey = cfa.getCommktKey().intValue();
            base.lotSize = cfa.getCommktLotSize().intValue();
            base.isOption = false;
        }

        internal static ILotInfo findFutureAttrLotInfo(int cmKey) {
            return CommonAttrLotInfo.readFutureAttr(cmKey);
        }

        internal CommktFutureAttr readFutAttr(int cmKey) {
            ObjectContext ctx = LocalCayenneService.sharedInstance.sharedContext();
            object tmp = ctx.selectOne(
                new SelectQuery(
                    typeof(CommktFutureAttr),
                    ExpressionFactory.matchExp("commktKey",new java.lang.Integer(cmKey))));

            return (CommktFutureAttr) tmp;
        }
    }

    public class OptionAttrLotInfo : CommonAttrLotInfo { }

    public class WrappedEntityTagOption {
        #region ctor
        public WrappedEntityTagOption(EntityTagOption eto) {
            entityTagId = eto.getEntityTagId().intValue();
            tagOption = eto.getTagOption().Trim();
            tagOptionDesc = eto.getTagOptionDesc().Trim();
            tagOptionStatus = eto.getTagOptionStatus();
        }
        #endregion

        #region properties
        [Browsable(false)]
        public int entityTagId { get; private set; }
        [Browsable(false)]
        public string tagOption { get; private set; }
        [DisplayName("Tag Value")]
        public string tagOptionDesc { get; private set; }
        [Browsable(false)]
        public string tagOptionStatus { get; private set; }
        #endregion

        #region methods
        public override string ToString() {
            return GetType().Name + ": entityTagId=" + entityTagId + " tagOption=" + tagOption + " tagoptionDesc=" + tagOptionDesc;
        }
        #endregion
    }

    public class WrappedAccount {
        #region ctor
        public WrappedAccount(Account aDept) {
            this.acctNum = aDept.getAcctNum().intValue();
            this.acctShortName = aDept.getAcctShortName();
            this.acctFullname = aDept.getAcctFullName();
            this.customDescription = acctShortName + " [" + acctNum + "]";
        }
        #endregion

        #region properties

        [Browsable(false)]
        [DisplayName("Acct#")]
        public int acctNum { get; private set; }

        [DisplayName("AcctName")]
        public string acctShortName { get; private set; }

        [Browsable(false)]
        [DisplayName("AcctFullName")]
        public string acctFullname { get; private set; }

        [DisplayName("Account")]
        public string customDescription { get; private set; }
        #endregion

        #region methods
        public override string ToString() {
            return GetType().Name + ": acctNum=" + acctNum + " acctShortName=" + acctShortName + " customDescription=" + customDescription;
        }
        #endregion



    }

    public class WrappedDepartment {
        #region ctor
        public WrappedDepartment(Department aDept) {
            acctNum = -1;
            if (aDept.getAccount() != null)
                acctNum = aDept.getAccount().getAcctNum().intValue();
            deptCode = aDept.getDeptCode();
            deptName = aDept.getDeptName();
            deptNum = -1;
            if (aDept.getDeptNum() != null)
                deptNum = aDept.getDeptNum().shortValue();
        }
        #endregion
        #region properties
        public int acctNum { get; private set; }
        public string deptCode { get; private set; }
        public string deptName { get; private set; }
        public int deptNum { get; private set; }
        #endregion
        #region methods
        public override string ToString() {
            return base.ToString() + ":" + deptCode + " " + deptNum;
        }
        #endregion
    }

    public class WrappedLocation {
        #region ctor
        public WrappedLocation(Location aloc) {
            locCode = aloc.getLocCode();
            locName = aloc.getLocName();
            locNum = aloc.getLocNum().intValue();
        }
        #endregion

        #region properties
        public string locCode { get; private set; }
        public string locName { get; private set; }
        public int locNum { get; private set; }
        #endregion

        #region methods
        public override string ToString() {
            return base.ToString() + ":" + locCode + " " + locName;
        }
        #endregion
    }


    public class WrappedPL : IPLRecord {
        #region fields
        PortfolioProfitLoss ppl;
        #endregion

        #region ctor
        public WrappedPL(PortfolioProfitLoss appl) {
            ppl = appl;
        }
        #endregion

        #region methods
        bool makeBoolean(string tmp) {
            return string.IsNullOrEmpty(tmp) ? false : string.Compare(tmp.Substring(0,1),"Y",true) == 0;
        }
        double makeDouble(java.lang.Float p) { return p == null ? 0 : p.doubleValue(); }
        double makeDouble(java.math.BigDecimal p) { return p == null ? 0 : p.doubleValue(); }
        int makeInt(java.lang.Integer anInt) { return anInt == null ? 0 : anInt.intValue(); }
        DateTime makeDate(java.util.Date date) {
            string initial,tmp;
            const string JAVA_DATE_FORMAT = "MMM dd hh:mm:ss yyyy";

            if (date == null)

                return DateTime.MinValue;
            initial = date.toString();
            tmp = initial.Substring(4,16) + initial.Substring(24);
            return DateTime.ParseExact(tmp,JAVA_DATE_FORMAT,null);
        }
        #endregion

        #region IPLRecord
        bool IPLRecord.isCompYearEnd { get { return ppl == null ? false : makeBoolean(ppl.getIsCompyrEndInd()); } }
        bool IPLRecord.isMonthEnd { get { return ppl == null ? false : makeBoolean(ppl.getIsMonthEndInd()); } }
        bool IPLRecord.isOfficialRun { get { return ppl == null ? false : makeBoolean(ppl.getIsOfficialRunInd()); } }
        bool IPLRecord.isWeekend { get { return ppl == null ? false : makeBoolean(ppl.getIsWeekEndInd()); } }
        bool IPLRecord.isYearEnd { get { return ppl == null ? false : makeBoolean(ppl.getIsYearEndInd()); } }
        double IPLRecord.closedHedgePL { get { return ppl == null ? 0.0 : makeDouble(ppl.getClosedHedgePl()); } }
        double IPLRecord.closedPhysPL { get { return ppl == null ? 0.0 : makeDouble(ppl.getClosedPhysPl()); } }
        double IPLRecord.liqClosedHedgePL { get { return ppl == null ? 0.0 : makeDouble(ppl.getLiqClosedHedgePl()); } }
        double IPLRecord.liqClosedPhysPL { get { return ppl == null ? 0.0 : makeDouble(ppl.getLiqClosedPhysPl()); } }
        double IPLRecord.liqOpenHedgePL { get { return ppl == null ? 0.0 : makeDouble(ppl.getLiqOpenHedgePl()); } }
        double IPLRecord.liqOpenpPhysPL { get { return ppl == null ? 0.0 : makeDouble(ppl.getOpenPhysPl()); } }
        double IPLRecord.openHedgePL { get { return ppl == null ? 0.0 : makeDouble(ppl.getOpenHedgePl()); } }
        double IPLRecord.openPhysPL { get { return ppl == null ? 0.0 : makeDouble(ppl.getOpenPhysPl()); } }
        double IPLRecord.otherPL { get { return ppl == null ? 0.0 : makeDouble(ppl.getOtherPl()); } }
        double IPLRecord.totalPnLSecCost { get { return ppl == null ? 0.0 : makeDouble(ppl.getTotalPlNoSecCost()); } }
        int IPLRecord.passRunId { get { return ppl == null ? 0 : makeInt(ppl.getPassRunDetailId()); } }
        int IPLRecord.portNum { get { return ppl == null ? 0 : makeInt(ppl.getPortNum()); } }
        int IPLRecord.transId { get { return ppl == null ? 0 : makeInt(ppl.getTransId()); } }
        DateTime IPLRecord.plAsOfDate { get { return ppl == null ? DateTime.MinValue : makeDate(ppl.getPlAsOfDate()); } }
        DateTime IPLRecord.plCalcDate { get { return ppl == null ? DateTime.MinValue : makeDate(ppl.getPlCalcDate()); } }
        #endregion IPLRecord
    }

    public class WrappedUom {
        #region ctor
        public WrappedUom(Uom auom) { uom = auom; }
        #endregion

        #region properties
        [Browsable(false)]
        public Uom uom { get; private set; }

        public string uomShortName { get { return uom.getUomShortName(); } }
        public string uomCode { get { return uom.getUomCode(); } }
        #endregion

        #region methods
        public override string ToString() {
            return base.ToString() + ":" + "Code=" + uomCode + ", ShortName=" + uomShortName;
        }
        #endregion
    }

    public class WrappedPortTagDef {
        #region ctor
        internal WrappedPortTagDef(PortfolioTagDefinition ptd) {
            if (ptd == null)
                throw new ArgumentNullException("ptd",typeof(PortfolioTagDefinition).FullName + " is null!");
            portTag = ptd;
        }
        #endregion

        #region properties
        public PortfolioTagDefinition portTag { get; private set; }
        public string fkField { get { return portTag.getForeignKeyField(); } }
        public string fkTable { get { return portTag.getForeignKeyTable(); } }
        public string inspectorFormatter { get { return portTag.getRefInspFormatterKey(); } }
        public string inspectorName { get { return portTag.getRefInspName(); } }
        public string tagDescription { get { return portTag.getTagDesc(); } }
        public string tagName { get { return portTag.getTagName(); } }
        public bool required { get { return string.Compare(portTag.getTagRequiredInd(),"Y",true) == 0; } }
        public string status { get { return portTag.getTagStatus(); } }
        public int transId { get { return portTag.getTransId().intValue(); } }
        public string valueAttribute { get { return portTag.getValueAttribute(); } }
        public string valueEntityName { get { return portTag.getValueEntityName(); } }
        public string valueEntityType { get { return portTag.getValueTypeInd(); } }
        #endregion properties

        #region methods
        public override string ToString() {
            return GetType().Name + ": name=" + tagName + " description=" + tagDescription;
        }
        #endregion methods
    }

    public class WrappedCurrency {

        #region ctor
        public WrappedCurrency(Commodity commodity) {
            this.cmdty = commodity;

            cmdty.getCmdtyCode();
            cmdty.getCmdtyShortName();
            cmdty.getCmdtyFullName();
        }
        #endregion

        #region properties
        [Browsable(false)]
        public Commodity cmdty { get; private set; }

        [DisplayName("Code")]
        public string code { get { return cmdty.getCmdtyCode(); } }

        [DisplayName("ShortName")]
        public string shortName { get { return cmdty.getCmdtyShortName(); } }

        [DisplayName("FullName")]
        public string fullName { get { return cmdty.getCmdtyFullName(); } }

        #endregion
    }

    public class WrappedUser {
        #region ctor
        public WrappedUser(IctsUser aUser) {
            desk = aUser.getDesk().getDeskCode();
            emailAddress = aUser.getEmailAddress();
            locCode = aUser.getLocation().getLocCode();
            transId = aUser.getTransId().intValue();
            isUSCitizen = string.Compare(aUser.getUsCitizenInd(),"Y",true) == 0;
            userInit = aUser.getUser_init();
            userEmployeeNum = -1;
            if (aUser.getUserEmployeeNum() != null)
                userEmployeeNum = aUser.getUserEmployeeNum().intValue();
            userFirstName = aUser.getUserFirstName();
            userJobTitle = aUser.getUserJobTitle().getJobTitle();
            userLastName = aUser.getUserLastName();
            userLogonId = aUser.getUserLogonId();
            userStatus = aUser.getUserStatus();
            this.customDescription = userLastName + "," + userFirstName + " [" + userLogonId + "]";
        }
        #endregion

        #region properties
        public string userJobTitle { get; private set; }
        public bool isUSCitizen { get; private set; }
        public int transId { get; private set; }
        public int userEmployeeNum { get; private set; }
        public string emailAddress { get; private set; }
        public string locCode { get; private set; }
        public string userFirstName { get; private set; }
        public string userInit { get; private set; }
        public string userLastName { get; private set; }
        public string userLogonId { get; private set; }
        public string userStatus { get; private set; }
        public string desk { get; private set; }
        [DisplayName("Account")]
        public string customDescription { get; private set; }
        #endregion

        #region methods
        public override string ToString() {
            return base.ToString() + ":" + userInit + " " + userLastName + "," + userFirstName;
        }
        #endregion
    }

    public class WrappedErrorProvider : IDXDataErrorInfo {
        #region ctors
        WrappedErrorProvider() {
            this.keyValue = "key";
            this.displayValue = "disp";
        }

        public WrappedErrorProvider(PortfolioTagDefinition avar)
            : this() {
            this.keyValue = avar.getTagName();
            this.displayValue = avar.getTagDesc();
            this.originalClassName = avar.GetType().FullName;
        }

        public WrappedErrorProvider(WrappedEntityTagOption avar)
            : this() {
            this.keyValue = avar.tagOption;
            this.displayValue = avar.tagOptionDesc;
            this.originalClassName = avar.GetType().FullName;
        }

        public WrappedErrorProvider(PriceSource avar)
            : this() {
            throw new InvalidOperationException("please implemement " + Util.makeSig(MethodBase.GetCurrentMethod()));
        }

        public WrappedErrorProvider(WrappedUser avar1)
            : this() {
            this.originalClassName = avar1.GetType().FullName;
            this.keyValue = avar1.userInit;
            this.displayValue = avar1.userLastName + "," + avar1.userFirstName + " [" + avar1.userLogonId.Trim() + "]";
        }

        public WrappedErrorProvider(WrappedAccount avar)
            : this() {
            this.keyValue = avar.acctNum.ToString();
            this.displayValue = avar.acctShortName + " [" + avar.acctNum + "]";
            this.originalClassName = avar.GetType().FullName;
        }

        public WrappedErrorProvider(WrappedPortTagDef avar) {
            keyValue = avar.tagName;
            displayValue = avar.tagDescription;
            originalClassName = avar.GetType().FullName;
        }

        #endregion

        #region IDXDataErrorInfo
        void IDXDataErrorInfo.GetError(ErrorInfo info) {
            if (string.IsNullOrEmpty(this.keyValue)) {
                info.ErrorText = "Value cannot be empty!";
                info.ErrorType = ErrorType.Critical;
            }
        }
        void IDXDataErrorInfo.GetPropertyError(string propertyName,ErrorInfo info) {
            if (string.Compare(propertyName,"displayValue",true) == 0) {
                if (string.IsNullOrEmpty(this.keyValue)) {
                    info.ErrorText = "Value Cannot be empty!";
                    info.ErrorType = ErrorType.Critical;
                }
            }
        }
        #endregion IDXDataErrorInfo

        #region properties
        public string originalClassName { get; private set; }
        public string keyValue { get; private set; }
        public string displayValue { get; private set; }
        #endregion
    }
}