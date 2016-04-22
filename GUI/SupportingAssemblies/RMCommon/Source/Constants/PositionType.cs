using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace NSRiskManager.Constants
{
    public class PositionType
    {
        public const string    POS_TYPE_FUTURE = "F";
        public const string    POS_TYPE_PHYSICAL = "P";
        public const string    POST_TYPE_LISTED_OPTION = "X";
        public const string    POS_TYPE_OTC_OPTION = "O";
        public const string    POS_TYPE_QUOTE = "Q";
        public const string    POS_TYPE_SWAP = "W";
        public const string    POS_TYPE_SWAP_QUOTE = "U";
        public const string    POS_TYPE_DERIVED_CURRENCY = "C";
        public const string    POS_TYPE_INVENTORY = "I";
        public const string    POS_TYPE_SPREAD = "R";
        public const string    POS_TYPE_SYNTHETIC = "S";
        public const string    POS_TYPE_TRADE_FORMULA = "T";
        public const string    POS_TYPE_RISK_FORMULA = "K";
        public const string    POS_TYPE_EXCH_IMBALANCE = "Z";
        public const string    POS_TYPE_PHYS_EXCHANGE = "Y";
        public const string    POS_TYPE_TRANS_VESSEL = "V";
        public const string    POS_TYPE_TRANS_BUNKER = "B";
        public const string    POS_TYPE_MARKET_FORMULA = "M";
        public const string    POS_TYPE_RIN_ACTIVE = "L";
        public const string    POS_TYPE_RIN_COMMITTED = "E";
        public const string    POS_TYPE_RIN_OBLIGATION = "N";

        //we don't seem to use this type of position in Risk Manager
        public static string POS_TYPE_FORECAST = "A";
   
    
        public static string getDisplayPositionSource(string posSource, bool isHedgeInd, string optionType)
        {
            string displayPosSource = "";


            switch (posSource)
            {
                case POS_TYPE_FUTURE: 
                    displayPosSource = "FUTURE";
                    break;
                case POS_TYPE_PHYSICAL:
                    displayPosSource = "PHYS";
                    break;

                case POST_TYPE_LISTED_OPTION:
                    displayPosSource = "LISTED OPTION";
                    break;

                case POS_TYPE_OTC_OPTION:
                    displayPosSource = "OTC OPTION";
                    break;

                case POS_TYPE_QUOTE:
                    if (optionType == null)
                        displayPosSource = "TRADE FORM";
                    else
                        displayPosSource = "SWAP FORM";
                    break;

                case POS_TYPE_SWAP:
                    if(optionType == "S")
                        displayPosSource = "SWAP";
                     else if (optionType=="C")
                         displayPosSource = "CFD";
                    break;

                case POS_TYPE_SWAP_QUOTE:
                    displayPosSource = "SWAP QUOTE";
                    break;

                case POS_TYPE_DERIVED_CURRENCY:
                    displayPosSource = "DERIVED CURRENCY";
                    break;

                case POS_TYPE_INVENTORY:
                    displayPosSource = "INVENTORY";
                    break;

                case POS_TYPE_SPREAD:
                    displayPosSource = "SPREAD";
                    break;

                case POS_TYPE_SYNTHETIC:
                    displayPosSource = "SYNTHETIC";
                    break;

                case POS_TYPE_TRADE_FORMULA:
                    displayPosSource = "TRADE FORMULA";
                    break;

                case POS_TYPE_RISK_FORMULA:
                    displayPosSource = "RISK FORMULA";
                    break;

                case POS_TYPE_EXCH_IMBALANCE:
                    displayPosSource = "PHYS EXCH IMBALANCE";
                    break;
                case POS_TYPE_PHYS_EXCHANGE:
                    displayPosSource = "PHYSICAL EXCHANGE";
                    break;

                case POS_TYPE_TRANS_VESSEL:
                case POS_TYPE_TRANS_BUNKER:
                    displayPosSource = "FREIGHT";
                    break;

                case POS_TYPE_MARKET_FORMULA:
                    displayPosSource = "MTM FORMULA";
                    break;

                case POS_TYPE_RIN_ACTIVE:
                    displayPosSource = "RIN ACTIVE";
                    break;
                case POS_TYPE_RIN_COMMITTED:
                    displayPosSource = "RIN COMMITTED";
                    break;
                case POS_TYPE_RIN_OBLIGATION:
                    displayPosSource = "RIN OBLIGATION";
                    break;
                default:
                    displayPosSource ="";
                    break;
            }

            if (displayPosSource == "")
            {
                displayPosSource = "UNKNOWN POS SOURCE";
            }
            else
            {
                if (isHedgeInd)
                    displayPosSource += " HEDGE";
                else
                    displayPosSource += " PRIM";
            }

            return displayPosSource;
        }
    }

    
}
