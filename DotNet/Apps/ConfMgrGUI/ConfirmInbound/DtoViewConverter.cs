using System;
using DBAccess;
using OpsTrackingModel;

namespace ConfirmInbound
{
    static internal class DtoViewConverter
    {
        internal static InboundDocsDto CreateInboundDocsDto(InboundDocsView inbDocView, string newFileName = null)
        {            
            InboundDocsDto inboundDocsData = new InboundDocsDto
            {
                CallerRef = inbDocView.CallerRef,
                Cmt = inbDocView.Cmt,
                DocStatusCode = inbDocView.DocStatusCode,
                FileName = newFileName ?? inbDocView.FileName,
                HasAutoAsctedFlag = inbDocView.HasAutoAsctedFlag,
                Id = inbDocView.Id,
                MappedCptySn = inbDocView.MappedCptySn,
                Sender = inbDocView.Sender,
                SentTo = inbDocView.SentTo,
                RcvdTs = inbDocView.RcvdTs
            };
            return inboundDocsData;
        }

        internal static InboundDocsView CreateInboundDocsView(InboundDocsDto dto)
        {
            return new InboundDocsView
            {
                CallerRef = dto.CallerRef,
                Cmt = dto.Cmt,
                DocStatusCode = dto.DocStatusCode,
                FileName = dto.FileName,
                HasAutoAsctedFlag = dto.HasAutoAsctedFlag,
                Id = dto.Id,
                MappedCptySn = dto.MappedCptySn,
                Sender = dto.Sender,
                SentTo = dto.SentTo,
                RcvdTs = dto.RcvdTs
            };
        }

        public static AssociatedDocsDto CreateAssociatedDocsDto(AssociatedDoc assDoc)
        {
            var assocDocsData = new AssociatedDocsDto();
            assocDocsData.BrokerSn = assDoc.BrokerShortName;
            assocDocsData.CdtyGroupCode = assDoc.CdtyGroupCode;
            assocDocsData.CptySn = assDoc.CptyShortName;
            assocDocsData.FileName = assDoc.FileName;
            assocDocsData.Id = assDoc.Id;
            assocDocsData.InboundDocsId = assDoc.InboundDocsId;
            assocDocsData.IndexVal = Convert.ToInt16(assDoc.IndexVal);
            assocDocsData.TradeRqmtId = assDoc.TradeRqmtId;
            assocDocsData.TradeId = assDoc.TradeId;
            assocDocsData.DocTypeCode = assDoc.DocTypeCode; 
            assocDocsData.SecValidateReqFlag = assDoc.SecondValidateReqFlag;
            return assocDocsData;
        }
    }
}