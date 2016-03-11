using log4net;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using VaultUploader.Data.Models;
using VaultUploader.Data.DbAccess;
using VaultUploader.Data.Repository;

namespace VaultUploader.Service
{
    public class VaultScanner
    {
        private bool _stopRequest = false;
        private VaultProcessor _vaultProcessor;//TODO: we can run this tru new thread to not wait on process.

        public VaultScanner()
        {
            //TODO:some init stuff here
        }

        public void StartScan()
        {
            _stopRequest = false;
            int scanInterval = Int32.Parse(System.Configuration.ConfigurationManager.AppSettings["ScanInterval"]);
            _vaultProcessor = new VaultProcessor();
            try
            {
                while (!_stopRequest)
                {
                    //TODO:watch on vaulted_docs and handle that to porocess.
                    Log.Info("Scanning for un posted vaults..");
                    List<VaultUploader.Data.Models.VaultedDocs> unVaultedDocs = FindUnpostedVaults();
                    if (unVaultedDocs.Count > 0)
                    {
                        Log.Info(String.Format("Found {0} un posted vault docs", unVaultedDocs.Count));
                        _vaultProcessor.Process(unVaultedDocs);
                    }                    
                    Thread.Sleep(scanInterval);
                }
            }
            catch (Exception ex)
            {
                //TODO:Do mail kind of stuff on critical erros
                //and some fixed iterations..
                Log.Error(ex.Message + System.Environment.NewLine + ex.StackTrace);
            }
        }

        private List<VaultUploader.Data.Models.VaultedDocs> FindUnpostedVaults()
        {
            List<VaultUploader.Data.Models.VaultedDocs> unVaultedDocs = new List<Data.Models.VaultedDocs>();
            unVaultedDocs = DbContext.Instance().VaultedDocsRepository.GetAllUnposted();
            return unVaultedDocs;
        }

        public void StopScan()
        {
            _stopRequest = true;
        }

        private static ILog Log
        {
            get { return LogManager.GetLogger(typeof(VaultScanner)); }
        }
    }
}
