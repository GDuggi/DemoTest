using System;
using System.Collections.Generic;
using System.Reflection;
using DevExpress.Utils;
using DevExpress.XtraPivotGrid;
using DevExpress.Data.PivotGrid;

namespace NSRMCommon {
    public static class DXUtil {
        #region constants
        public const string PGF_PREFIX = "pgf";
        #endregion

        public static void setupPivotGrid(PivotGridControl pgc,Type type) {
            setupPivotGrid(pgc,type,false);
        }

        public static void setupPivotGrid(PivotGridControl pgc,Type type,bool addIfNotDefined) {
            pgc.OptionsBehavior.BestFitMode = PivotGridBestFitMode.FieldValue | PivotGridBestFitMode.FieldHeader | PivotGridBestFitMode.Cell;
            while (pgc.Fields.Count > 0)
                pgc.Fields.RemoveAt(0);
        }

      

        public static List<PivotGridField> findPivotFields(Type t) {
            return findPivotFields(t,true);
        }

        public static List<PivotGridField> findPivotFields(Type t,bool addIfNotDefined) {
            object[] attrs;
            List<PivotGridField> ret = new List<PivotGridField>();
            PivotGridField pgf = null;
            DesiredPivotGridFieldAttribute defAttr = new DesiredPivotGridFieldAttribute(string.Empty);
            Type tattr = typeof(DesiredPivotGridFieldAttribute);

            foreach (PropertyInfo pi in t.GetProperties()) 
            {
                

                if ((attrs = pi.GetCustomAttributes(tattr,true)) != null &&
                    attrs.Length > 0) 
                {
                    pgf = addPivotField(pi.Name,attrs[0] as DesiredPivotGridFieldAttribute);
                } 
                else if (addIfNotDefined) 
                {
                    defAttr.Caption = pi.Name;
                    pgf = addPivotField(pi.Name,defAttr);
                }
                if (pgf != null) {
                    pgf.Visible = false;
                    ret.Add(pgf);
                }
            }
            return ret;
        }

        public static PivotGridField addPivotField(string pivotFieldName,DesiredPivotGridFieldAttribute dpgfa) {
            PivotGridField field = null;

            if (string.IsNullOrEmpty(dpgfa.Caption))
                throw new ArgumentNullException("dpgfa.Caption","caption is null!");

            PivotArea area;

            if (dpgfa.IsDataField)
                area = PivotArea.DataArea;
            else if (dpgfa.IsFilterField)
                area = PivotArea.FilterArea;
            else
                area = PivotArea.ColumnArea;


            field = new PivotGridField(pivotFieldName, area);

            if (dpgfa.IsRunningTotal)
            {
                field.SummaryType = PivotSummaryType.Custom;
                field.Options.ShowValues = false;
                field.Options.ShowTotals = false;
            }



            field.Name = PGF_PREFIX + dpgfa.Caption; 
         
            field.Caption = dpgfa.Caption;
           

            if (!string.IsNullOrEmpty(dpgfa.DisplayFolder))
                field.DisplayFolder = dpgfa.DisplayFolder;
            else
                field.DisplayFolder = "dummy";
            
            
            if (!string.IsNullOrEmpty(dpgfa.NumericFormat)) 
            {
                field.CellFormat.FormatType = dpgfa.IsDate ? FormatType.DateTime : FormatType.Numeric;
                field.CellFormat.FormatString = dpgfa.NumericFormat;

                field.ValueFormat.FormatType = field.CellFormat.FormatType;
                field.ValueFormat.FormatString = field.CellFormat.FormatString;
            }

            if (dpgfa.Alignment == PivotFieldAlignment.CENTER) {
                field.Appearance.Value.TextOptions.HAlignment = HorzAlignment.Center;
                field.Appearance.Value.Options.UseTextOptions = true;
            } else if (dpgfa.Alignment == PivotFieldAlignment.RIGHT) {
                field.Appearance.Value.TextOptions.HAlignment = HorzAlignment.Far;
                field.Appearance.Value.Options.UseTextOptions = true;
            } else if (dpgfa.Alignment == PivotFieldAlignment.LEFT) {
                field.Appearance.Value.TextOptions.HAlignment = HorzAlignment.Near;
                field.Appearance.Value.Options.UseTextOptions = true;
            } else
                field.Appearance.Value.TextOptions.HAlignment = HorzAlignment.Default;
            return field;


        }
    }
}