using System;
using System.Collections.Generic;
using System.Text;
using OpsTrackingModel;

namespace ConfirmInbound
{
    interface IInbound
    {
        void PrintDocument();
        void DiscardDocument();
        void CopyDocument();        
        void MapCallerReference();
        void UnMapCallerReference();
        void BookmarkDocument();
        void CreateUserComment();
        void IgnoreDocument();
        void AssociateDocument(bool xmitAfter);
        void CreateDocumentComment();        
        void LocateAndDisplayTradeRqmtDocument();
        void DisplayInboundDocument();
        void BeginDataGridUpdates();
        void EndGridDataUpdates();        
    }
}
