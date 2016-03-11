using System;
using System.Collections.Generic;
using System.Text;
using System.Configuration;
using NHibernate;
using System.Collections;
using NHibernate.Criterion;

namespace OpsTrackingModel
{
    public sealed class OpsTrackingDAO
    {
        static private NHibernate.ISessionFactory _sessionFactory;
        private const string PROJ_FILE_NAME = "OpsTrackingDAO";

        public static String ConnectionString
        {
            get { return ConfigurationManager.AppSettings["Oracle.ConnectionString"]; }
        }

        public static NHibernate.ISessionFactory SessionFactory
        {
            get { return OpsTrackingDAO._sessionFactory; }
            set { OpsTrackingDAO._sessionFactory = value; }
        }

        static OpsTrackingDAO()
        {
            try
            {
                // Enable the logging of NHibernate operations
                log4net.Config.XmlConfigurator.Configure();
                // Create the object that will hold the configuration settings
                // and fill it with the information to access to the Database
                NHibernate.Cfg.Configuration cfg = new NHibernate.Cfg.Configuration();
                cfg.SetProperty(NHibernate.Cfg.Environment.ConnectionProvider, "NHibernate.Connection.DriverConnectionProvider");
                cfg.SetProperty(NHibernate.Cfg.Environment.Dialect, "NHibernate.Dialect.OracleDialect");
                cfg.SetProperty(NHibernate.Cfg.Environment.ConnectionDriver, "NHibernate.Driver.OracleClientDriver");
                cfg.SetProperty(NHibernate.Cfg.Environment.ConnectionString, ConnectionString);
                BuildSessionFactory(cfg);
            }
            catch (Exception e)
            {
                throw new Exception("An error occurred while setting up and calling the NHibernate BuildSessionFactory." + Environment.NewLine +
                    "Error CNF-369 in " + PROJ_FILE_NAME + ".OpsTrackingDAO(): " + e.Message);
            }
        }

        private static void BuildSessionFactory(NHibernate.Cfg.Configuration cfg)
        {
            try
            {
                // Use NHibernate.Mapping.Attributes to create information about our entities
                System.IO.MemoryStream stream = new System.IO.MemoryStream(); // Where the information will be written in
                NHibernate.Mapping.Attributes.HbmSerializer.Default.Validate = true; // Enable validation (optional)
                // Ask to NHibernate to use fields instead of properties
                NHibernate.Mapping.Attributes.HbmSerializer.Default.HbmDefaultAccess = "field.camelcase-underscore";
                // Gather information from this assembly (can also be done class by class)
                //System.Console.Out.WriteLine("NHibernate.Mapping.Attributes.HbmSerializer.Default.Serialize()...\n");
                NHibernate.Mapping.Attributes.HbmSerializer.Default.Serialize(stream, System.Reflection.Assembly.GetExecutingAssembly());
                stream.Position = 0;
                //               StreamReader sr = new StreamReader(stream);

                //            string IbDoc = sr.ReadToEnd();
                cfg.AddInputStream(stream); // Send the Mapping information to NHibernate Configuration
                stream.Close();
                // Create table(s) in the database for our entities
             //   System.Console.Out.WriteLine("new NHibernate.Tool.hbm2ddl.SchemaExport(cfg).Create()...");
                // Build the SessionFactory
             //   System.Console.Out.WriteLine("\n\nsessionFact = cfg.BuildSessionFactory();\n\n");
                SessionFactory = cfg.BuildSessionFactory();
            }
            catch (Exception e)
            {
                throw new Exception("An error occurred while building the NHibernate Session Factory used for internal data storage." + Environment.NewLine +
                    "Error CNF-370 in " + PROJ_FILE_NAME + ".BuildSessionFactory(): " + e.Message);
            }
        }

        public static IList GetAllTradeRqmt()
        {
            ISession session = SessionFactory.OpenSession();
            IList tradeRqmts = null;
            try
            {
                ICriteria criteria = session.CreateCriteria(typeof(RqmtData));
                criteria.Add(Expression.Eq("FinalApprovalFlag", "N"));
                tradeRqmts = criteria.List();
                return tradeRqmts;
            }
            catch (Exception e)
            {
                throw new Exception("An error occurred while retrieving a list of Trade Rqmts." + Environment.NewLine +
                    "Error CNF-371 in " + PROJ_FILE_NAME + ".GetAllTradeRqmt(): " + e.Message);
            }
        }

        public static IList GetTradeData()
        {
            ISession session = SessionFactory.OpenSession();
            IList tradeData = null;
            try
            {
                ICriteria criteria = session.CreateCriteria(typeof(SummaryData));
                criteria.Add(Expression.Eq("FinalApprovalFlag", "N"));
                tradeData = criteria.List();
                return tradeData;
            }
            catch (Exception e)
            {
                throw new Exception("An error occurred while retrieving a list of Trade Data." + Environment.NewLine +
                    "Error CNF-372 in " + PROJ_FILE_NAME + ".GetTradeData(): " + e.Message);
            }
        }

        public static IList GetTradeData<T1>()
        {
            return null;
        }

        public static IList GetAssociatedDocs()
        {
            ISession session = SessionFactory.OpenSession();
            IList myData = null;
            try
            {
                ICriteria criteria = session.CreateCriteria(typeof(AssociatedDoc));
                criteria.Add(Expression.Eq("DocStatusCode", AssociatedDoc.ASSOCIATED));
                myData = criteria.List();
                return myData;
            }
            catch (Exception e)
            {
                throw new Exception("An error occurred while retrieving a list of Associated Docs." + Environment.NewLine +
                    "Error CNF-373 in " + PROJ_FILE_NAME + ".GetAssociatedDocs(): " + e.Message);
            }
        }

        public static IList GetInboundDocs()
        {
            ISession session = SessionFactory.OpenSession();
            IList myData = null;
            try
            {
                ICriteria criteria = session.CreateCriteria(typeof(InboundDoc));
                criteria.Add(Expression.Eq("DocStatusCode", InboundDoc.OPEN));
                myData = criteria.List();
                return myData;
            }
            catch (Exception e)
            {
                throw new Exception("An error occurred while retrieving a list of Inbound Docs." + Environment.NewLine +
                    "Error CNF-374 in " + PROJ_FILE_NAME + ".GetInboundDocs(): " + e.Message);
            }
        }
    }
}
