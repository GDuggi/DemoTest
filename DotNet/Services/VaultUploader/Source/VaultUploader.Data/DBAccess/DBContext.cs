using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VaultUploader.Data.Repository;

namespace VaultUploader.Data.DbAccess
{
   public class DbContext
    {
       private string _connectionString;
       private static DbContext _dbContext;
       public static readonly string SCHEMA_NAME = "ConfirmMgr.";
       public static readonly string NO_LOCK = " with(nolock)";

       private DbContext(string connectionString)
       {
           _connectionString = connectionString;
       }

       public static DbContext Instance(string connectionString=null)
       {
           if (_dbContext == null)
           {
               _dbContext = new DbContext(connectionString);
           }
           return _dbContext;
       }

       public VaultedDocsRepository VaultedDocsRepository
       {
           get { return VaultedDocsRepository.Instance(_connectionString); }
       }
       
    }
}
