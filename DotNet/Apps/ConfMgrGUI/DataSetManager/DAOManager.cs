using System;
using System.Collections.Generic;
//using GigaSpaces.Core;
using DataManager.common;
using OpsTrackingModel;
using System.Data;
using System.Data.SqlClient;
//using System.Data.OracleClient;
using System.Data.OleDb;

namespace DataManager
{
    public sealed class DAOManager : MarshalByRefObject
    {
        private const string PROJ_FILE_NAME = "DAOManager";
        //static public ISpaceProxy gsSpace = null;
        //private bool useGs = false;
        //private string gigaspaceURL = "";

        //public DAOManager(string gigaspaces_url)
        //{
        //    // TODO: Complete member initialization
        //    connectToSpace(gigaspaces_url);
        //}

        //public bool UseGs
        //{
        //    get { return this.useGs; }
        //    set { this.useGs = value; }
        //}

        public DAOManager()
        {
            //this.useGs = Properties.Settings.Default.usGigaSpace;
            //this.gigaspaceURL = Properties.Settings.Default.gigaspaceURL;
            //connectToSpace(this.gigaspaceURL);
        }

        public IList<T1> CreateObjList<T1>(T1 template)
        {
            try
            {
                IList<T1> objList = null;
                GenericDAO<T1> dao = new GenericDAO<T1>();
                objList = dao.dbCreateObjList(template);

                //if (useGs)
                //{
                //    if (gsSpace == null)
                //    {
                //        connectToSpace(this.gigaspaceURL);
                //    }
                //    objList = dao.gsCreateObjList(template, gsSpace);
                //}
                //else  // we are not using gigaspace to load any data.  get objects from database
                //{
                //    objList = dao.dbCreateObjList(template);
                //}
                
                return objList;
            }
            catch (Exception e)
            {
                throw new Exception("An error occurred while creating and populating internal data storage." + Environment.NewLine +
                    "Error CNF-344 in " + PROJ_FILE_NAME + ".CreateObjList(): " + e.Message);
            }
        }

        //public string GigaspaceURL
        //{
        //    get { return gigaspaceURL; }
        //    set { gigaspaceURL = value; }
        //}

        //internal void connectToSpace(string gigaspaceURL)
        //{
        //    try
        //    {
        //        if (UseGs)
        //        {
        //            gsSpace = GigaSpacesFactory.FindSpace(gigaspaceURL);
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        throw ex;
        //    }
        //}

    }
}

