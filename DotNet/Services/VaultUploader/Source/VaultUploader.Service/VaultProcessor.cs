using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VaultUploader.Data.Models;
using VaultUploader.Data.DbAccess;
using VaultUploader.Data.Repository;
using log4net;
using VaultUploader.WSAccess;

namespace VaultUploader.Service
{
   public class VaultProcessor
    {
       public void Process(List<VaultUploader.Data.Models.VaultedDocs> unPostedVaultedDocs)
       {
           foreach (VaultedDocs vd in unPostedVaultedDocs)
           {
               Process(vd);
           }
       }

       private void Process(VaultedDocs unPostedVaultedDoc)
       {
           Log.Info(String.Format("############ Processing the Vaulted Doc {0} #############", unPostedVaultedDoc.ToString()));
           //Validate record if required
           try
           {
               if (unPostedVaultedDoc != null)
               {
                   //Get BLOB
                   Log.Info("Capturing the file stream from blob");
                   DbContext.Instance().VaultedDocsRepository.CaptureVaultedDocsBlob(unPostedVaultedDoc);
                   Log.Info("Captured the file stream from blob");
                   //Upload              
                   string responseURL= Upload(unPostedVaultedDoc);
                   //Update the status
                   Log.Info("Updating the vaulted_doc's processed status to Y");
                   Log.Info("Updating the vaulted_doc's VAULT_GUID to " + responseURL);
                   DbContext.Instance().VaultedDocsRepository.Update(unPostedVaultedDoc, responseURL);
                   Log.Info("Updated the vaulted_doc's processed status to Y");
                   Log.Info("Updated the vaulted_doc's VAULT_GUID to " + responseURL);
               }
           }
           catch (Exception ex)
           {
               //TODO:may be we need to stop here for fixed iterations and then send mail... other wise it will be in loop .               
               Log.Error("Failed to process the Vaulted doc uplaoding ." + ex.Message);
           }    
       }

       private string Upload(VaultedDocs unPostedVaultedDoc)
       {
           Log.Info("Uploading the document to vault doc service");
           string response= ServiceContext.Instance().VaultSvcAccessor.UploadDocument(String.Empty, unPostedVaultedDoc.RqmtDocTypeDocTypeCode,unPostedVaultedDoc.TRDSYSTICKET,
                                            unPostedVaultedDoc.TRDSYSCODE, unPostedVaultedDoc.VaultedDocsBlobImageFileExt,unPostedVaultedDoc.VaultedDocsBlobDocBlob);
           string respURL = "";
           if (!String.IsNullOrEmpty(response))
           {
               Log.Info("Document uploaded successfully at -- " + response);
               string[] respArr = response.Split(':');
               if (respArr != null && respArr.Count() > 0)
                   respURL = respArr[respArr.Count() - 1];
               if (respURL.Length > 50)
                   respURL = respURL.Substring(0, 50);
           }
           else
           {
               Log.Info("Document uploaded failed");
           }
           return respURL;
       }

       private static ILog Log
       {
           get { return LogManager.GetLogger(typeof(VaultProcessor)); }
       }
    }
}
