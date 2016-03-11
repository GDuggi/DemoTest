using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VaultUploader.Data.Models
{
    /// <summary>
    /// This is the model class to bind with vaulted docs view "V_DOCS_TO_VAULT"
    /// </summary>
   public class VaultedDocs
    {
 //vaulted_docs_id	int
       public int VaultedDocsId { get; set; }
//vaulted_docs_trade_rqmt_confirm_id	int
       public int VaultedDocsTradeRqmtConfirmId { get; set; }
//vaulted_docs_associated_docs_id	int
       public int VaultedDocsAssociatedDocsId {get;set;}
//vaulted_docs_sent_flag	varchar
       public string VaultedDocsSentFlag { get; set; }
//vaulted_docs_metadata	varchar
       public string VaultedDocsMetadata { get; set; }
//vaulted_docs_request_timestamp	datetime2
       public DateTime VaultedDocsRequestTimestamp { get; set; }
//vaulted_docs_blob_id	int
       public int VaultedDocsBlobId { get; set; }
//vaulted_docs_blob_image_file_ext	varchar
       public string VaultedDocsBlobImageFileExt { get; set; }
//rqmt_doc_type_rqmt_code	varchar
       public string RqmtDocTypeRqmtCode { get; set; }
//rqmt_doc_type_sent_flag	varchar
       public string RqmtDocTypeSentFlag { get; set; }
//rqmt_doc_type_doc_type_code	varchar
       public string RqmtDocTypeDocTypeCode { get; set; }
//TRD_SYS_CODE	varchar
       public string TRDSYSCODE { get; set; }
//TRD_SYS_TICKET	varchar
       public string TRDSYSTICKET { get; set; }

       public byte[] VaultedDocsBlobDocBlob { get; set; }

       public override string ToString()
       {
          string ret=
             "TRDSYSCODE :" + (this.TRDSYSCODE != null ? this.TRDSYSCODE : " " )+ "," +
                   "TRDSYSTICKET :" +(this.TRDSYSTICKET!=null?this.TRDSYSTICKET:" ")+","+
                   "VaultedDocsId :" +this.VaultedDocsId + "," +
                   "VaultedDocsTradeRqmtConfirmId :" + VaultedDocsTradeRqmtConfirmId + "," +
                   "VaultedDocsAssociatedDocsId :" + VaultedDocsAssociatedDocsId + "," +
                   "VaultedDocsSentFlag :" + (VaultedDocsSentFlag != null ? VaultedDocsSentFlag : " ") + "," +
                   "VaultedDocsMetadata :" + (VaultedDocsMetadata != null ? VaultedDocsMetadata : " " )+ "," +
                   "VaultedDocsRequestTimestamp :" + (VaultedDocsRequestTimestamp != null ? VaultedDocsRequestTimestamp.ToShortDateString() : " " )+ "," +                  
                   "VaultedDocsBlobId :" + VaultedDocsBlobId + "," +
                   "VaultedDocsBlobImageFileExt :" + (VaultedDocsBlobImageFileExt != null ? VaultedDocsBlobImageFileExt : " " )+ "," +
                   "RqmtDocTypeRqmtCode :" +( RqmtDocTypeRqmtCode != null ? RqmtDocTypeRqmtCode : " ") + "," +
                   "RqmtDocTypeSentFlag :" + (RqmtDocTypeSentFlag != null ? RqmtDocTypeSentFlag : " " )+ "," +
                   "RqmtDocTypeDocTypeCode :" +( RqmtDocTypeDocTypeCode != null ? RqmtDocTypeDocTypeCode : " ");
          return ret;
       }
    }
}
