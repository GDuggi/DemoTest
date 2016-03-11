using System;
using System.Collections.Generic;
using System.Text;
using System.Data;
using System.Windows.Forms;
using DevExpress.XtraGrid;

namespace ConfirmInbound
{
    interface ITabPanel
    {
        void SetGridDataSource(ref DataView dataTable);
        GridControl MainDataGrid { get; }
        void LoadImageInEditor<T>(T obj);
        string GetActiveDocumentFileName();
    }
}
