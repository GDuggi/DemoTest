using System;
using System.Collections.Generic;
using NSRMCommon;
using NSRMLogging;
using com.amphora.cayenne.entity.support;
using System.Reflection;

namespace NSRMCommon
{
    class QuantityContainer : IEquatable<QuantityContainer>
    {

      
        internal QuantityContainer()
        {
            this.riskQty = this.physicalQty = this.discountQty = 0;
            this.asOfRiskQty = this.asOfphysicalQty = double.MinValue;
        }
      
        public double riskQty { get; private set; }
        public double physicalQty { get; private set; }
        public double discountQty { get; private set; }
        public bool isZero { get; private set; }
        public double asOfRiskQty { get; private set; }
        public double asOfphysicalQty { get; private set; }
   

        double PRECISION = 0.000001;


        /// <summary>
        /// To conver the LOTS into BBL copied this from the CSPositions file while re-factoring the code.
        /// </summary>
        /// <returns>Converted BBL value from LOTS</returns>
        public double ConversionFromLotsToBBL(double quantity, string quantityUom, int commktKey)
        {
            double finalQuantity;
            double lotSize = SharedContext.getLotSize(quantityUom, commktKey);
            finalQuantity = quantity * lotSize;

            return finalQuantity;
        }


        private double convertUnitsForQuantity(double quantity, string commodityCode, int commktKey, string quantityUOM, string toUnits, out string finalQuantityUOM)
        {
            double finalQuantity = quantity;
            finalQuantityUOM = quantityUOM;
            IUOMEntityCache entCache = UOMEntityCacheImpl.Builder.uomCache();

            if (quantityUOM != toUnits)
            {
                if (quantityUOM == "lots" || quantityUOM == "LOTS")
                {
                    finalQuantity = (double)ConversionFromLotsToBBL(quantity, quantityUOM, commktKey);
                    finalQuantityUOM = "BBL";
                }

                if (finalQuantityUOM != toUnits)
                    finalQuantity = entCache.convertUom(quantity, commodityCode, finalQuantityUOM, toUnits).doubleValue();
            }

            return finalQuantity;
        }


        #region methods
        internal void convertQuantityToUnits(string posSource, List<RiskGroupAtom> atoms, bool verbose, string toUnits, bool forAsOfDate)
        {
            double tempLongQty, tempShortQuantity, tempDiscountQuantity, asOfRiskQuantity, asOfPhysicalQuantity;

            char cPosType = string.IsNullOrEmpty(posSource) ? char.MinValue : posSource[0];

            asOfRiskQuantity = asOfRiskQty;
            asOfPhysicalQuantity = asOfphysicalQty;

            foreach (RiskGroupAtom riskGroupAtom in atoms)
            {
                tempLongQty = riskGroupAtom.longQty;
                tempShortQuantity = riskGroupAtom.shortQty;
                tempDiscountQuantity = riskGroupAtom.discountQty;    

                try
                {
                    string finalQuantityUOM = "";

                    tempLongQty = convertUnitsForQuantity(tempLongQty, riskGroupAtom.cmdtyCode, riskGroupAtom.commktKey, riskGroupAtom.qtyUom, toUnits, out finalQuantityUOM);
                    tempShortQuantity = convertUnitsForQuantity(tempShortQuantity, riskGroupAtom.cmdtyCode, riskGroupAtom.commktKey, riskGroupAtom.qtyUom, toUnits, out finalQuantityUOM);
                    tempDiscountQuantity = convertUnitsForQuantity(tempDiscountQuantity, riskGroupAtom.cmdtyCode, riskGroupAtom.commktKey, riskGroupAtom.qtyUom, toUnits, out finalQuantityUOM);
                    riskGroupAtom.qtyUom = finalQuantityUOM;
                }
                catch (Exception ex)
                {
                    Util.show(MethodBase.GetCurrentMethod(), ex);
                    tempLongQty = tempShortQuantity = tempDiscountQuantity = double.MinValue;
                }

                if (SharedContext.isRightRiskMode(cPosType.ToString(), true, false, false))
                {
                    if (tempLongQty != double.MinValue && tempShortQuantity != double.MinValue && this.riskQty!=Double.MinValue)
                        this.riskQty += (tempLongQty - tempShortQuantity);
                    else
                        this.riskQty = double.MinValue;
                }

                if (SharedContext.isRightRiskMode(cPosType.ToString(), false, true, false))
                {
                    if (tempLongQty != double.MinValue && tempShortQuantity != double.MinValue && this.physicalQty!=Double.MinValue)
                        this.physicalQty += (tempLongQty - tempShortQuantity);
                    else
                        this.physicalQty = double.MinValue;
                }

                if (SharedContext.isRightRiskMode(cPosType.ToString(), false, false, true))
                {
                    if (this.discountQty != Double.MinValue && tempDiscountQuantity!=double.MinValue)
                        this.discountQty += tempDiscountQuantity;
                    else
                        this.discountQty = Double.MinValue;
                }
            }

            if (forAsOfDate)
            {
                this.asOfRiskQty = this.riskQty;
                this.asOfphysicalQty = this.physicalQty;
            }

            if ((Math.Abs(riskQty) < PRECISION || riskQty == double.MinValue) &&
                 (Math.Abs(physicalQty) < PRECISION || physicalQty == double.MinValue) &&
                 (Math.Abs(discountQty) < PRECISION || discountQty == double.MinValue))
                isZero = true;
            else
                isZero = false;
        }

        public bool Equals(QuantityContainer other)
        {
            return
                this.discountQty == other.discountQty &&
                this.physicalQty == other.physicalQty &&
                this.riskQty == other.riskQty;
        }
        #endregion



    }
}
