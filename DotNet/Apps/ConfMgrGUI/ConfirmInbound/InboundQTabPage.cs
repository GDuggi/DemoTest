using DevExpress.XtraTab;
using DevExpress.XtraEditors;
using System.Windows.Forms;

namespace ConfirmInbound
{
    public class InboundQTabPage : XtraTabPage , IInbound
    {
        private readonly InboundQTabPnll inboundQPnl;


        public InboundQTabPage(string pSqlConnectionString, string caption, string pUserId)
        {
            Text = caption;
            inboundQPnl = new InboundQTabPnll(pSqlConnectionString, pUserId);
            inboundQPnl.Dock = DockStyle.Fill;            
            Controls.Add(inboundQPnl);
        }

        public InboundQTabPnll InboundQPnl
        {
            get { return inboundQPnl; }           
        }

        #region IInbound Members


        public void PrintDocument()
        {
            inboundQPnl.PrintDocument();
        }

        public void DiscardDocument()
        {
            DialogResult result = XtraMessageBox.Show("Discard current Inbound Document?", "Discard Document",
   MessageBoxButtons.YesNo, MessageBoxIcon.Question, MessageBoxDefaultButton.Button2);
            if (result == DialogResult.Yes)
            {
                inboundQPnl.DiscardDocument();
            }
        }

        public void CopyDocument()
        {
            inboundQPnl.CopyDocument();
        }

        public void MapCallerReference()
        {
            inboundQPnl.MapCallerReference();
        }

        public void BookmarkDocument()
        {
            inboundQPnl.BookmarkDocument();
        }

        public void CreateUserComment()
        {
            inboundQPnl.CreateUserComment();
        }

        public void IgnoreDocument()
        {
            inboundQPnl.IgnoreDocument();
        }

        public void AssociateDocument(bool xmitAfter)
        {
            inboundQPnl.AssociateDocument(xmitAfter);
        }

        public void CreateDocumentComment()
        {
            inboundQPnl.CreateUserComment();
        }

        public void UnMapCallerReference()
        {
            inboundQPnl.UnMapCallerReference();
        }

        public void LocateAndDisplayTradeRqmtDocument()
        {
            inboundQPnl.LocateAndDisplayTradeRqmtDocument();
        }
        
        public void DisplayInboundDocument()
        {
            inboundQPnl.DisplayInboundDocument();
        }

        public void BeginDataGridUpdates()
        {
            inboundQPnl.BeginDataGridUpdates();
        }

        public void EndGridDataUpdates()
        {
            inboundQPnl.EndGridDataUpdates();
        }
        
        internal void LoadGridSettings(string p)
        {
            inboundQPnl.LoadGridSettings(p);
        }

        internal void SaveGridSettings(string p)
        {
            inboundQPnl.SaveGridSettings(p);
        }        

        #endregion
    }
}
