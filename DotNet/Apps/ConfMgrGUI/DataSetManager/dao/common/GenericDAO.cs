using System;
using System.Collections.Generic;
using System.Text;
//using GigaSpaces.Core;
using System.ComponentModel;
using System.Data;
using System.Reflection;
using OpsTrackingModel;
//using System.Data.OracleClient;
using CommonUtils;
using System.IO;

namespace DataManager.common
{
    public class GenericDAO<T>: IGenericDAO<T>
    {
        public static SettingsReader settingsReader = new SettingsReader(Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location) + @"\DataManager.dll.config");

        //public static OracleConnection oraConn = null;

        //private ISpaceProxy gsSpace;

        #region IGenericDAO<T> Members

        //public IList<T> gsCreateObjList(T template, ISpaceProxy space)
        //{
        //    IList<T> objList = null;
        //    try
        //    {
        //        objList = space.ReadMultiple<T>(template);
        //        return objList;
        //    }
        //    catch (Exception e)
        //    {
        //        throw e;
        //    }
        //}

        //#endregion

        //#region IGenericDAO<T> Members


        //public SortableSearchableList<T> gsFindByExampleAsSortable(T template, ISpaceProxy space)
        //{
        //    IList<T> objList = null;
        //    try
        //    {
        //        objList = space.ReadMultiple<T>(template);
        //        SortableSearchableList<T> test = new SortableSearchableList<T>(objList);
        //        return null;
        //    }
        //    catch (Exception e)
        //    {
        //        throw e;
        //    }
        //}

        #endregion

        public IList<T> dbCreateObjList(T template)
        {
            try
            {
                // THIS CAN BE CLEANED UP TO SUPPORT OTHER DATABASES...QUICK AND DIRTY RIGHT NOW TO SUPPORT ORACLE
                IList<T> objList;
                string dbms = GenericDAO<T>.settingsReader.GetSettingValue("dbms"); 
                string connectionString = GenericDAO<T>.settingsReader.GetSettingValue("ConnectionString");

                string select = (template as IOpsDataObj).getSelect();
                switch (dbms)
                {
                    //case "ORA":
                    //    OracleConnection conn = new OracleConnection(connectionString);
                    //    conn.Open();
                    //    //if (oraConn == null)
                    //    //{
                    //    //    oraConn = new OracleConnection(connectionString);
                    //    //    oraConn.Open();
                    //    //}
                    //    OracleCommand cmd = new OracleCommand();
                    //    cmd.Connection = conn;
                    //    //cmd.Connection = oraConn;
                    //    cmd.CommandType = CommandType.Text;
                    //    cmd.CommandText = select;
                    //    OracleDataReader dataReader = cmd.ExecuteReader();
                    //    DataTable dt = new DataTable();
                    //    dt.Load(dataReader);
                    //    objList = new List<T>();
                    //    foreach (DataRow row in dt.Rows)
                    //    {
                    //        objList.Add(CollectionHelper.CreateObjectFromDataRow<T>(row, true));
                    //    }
                    //    //conn.Close();
                    //    break;

                    default: objList = null;
                        break;
                }

                return objList;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}
