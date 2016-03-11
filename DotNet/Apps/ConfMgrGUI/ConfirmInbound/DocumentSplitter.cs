using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using log4net;
using OpsTrackingModel;
using Sempra.Confirm.InBound.ImageEdit;

namespace ConfirmInbound
{
    internal class DocumentSplitResult
    {
        public TifImage SplitPages { get; private set; }
        public TifImage Remainder { get; private set; }

        internal DocumentSplitResult(TifImage splitPages, TifImage remainder = null)
        {
            Remainder = remainder;
            SplitPages = splitPages;
        }
    }

    internal class DocumentSplitter
    {
        private static readonly ILog Logger = LogManager.GetLogger(typeof (DocumentSplitter));        

        public DocumentSplitter()
        {            
        }

        /*public bool SplitDocuments(InboundDocsView ibDoc, 
                                   AssociatedDoc assDoc, 
                                   Func<IEnumerable<int>> pages,
                                   Action<int> onPageExtracted)
        {
            if (imageHolder.ImageModified)
            {
                if (MessageBox.Show(@"Save document image changes?", @"Save Image", MessageBoxButtons.YesNo, MessageBoxIcon.Question, MessageBoxDefaultButton.Button1, MessageBoxOptions.DefaultDesktopOnly) == DialogResult.Yes)
                {
                    ImagesEventManager.Instance.Raise(new ImagesSavingEventArgs());
                }
            }

            var assDocImage = imagesDal.GetByDocId(assDoc.Id, ImagesDtoType.Associated);
            if (assDocImage != null)
            {
                Logger.InfoFormat("Associated document for id={0} already exists. Nothing to do.", assDoc.Id);
                return true;
            }

            var sourceDto = imagesDal.GetByDocId(ibDoc.Id, ImagesDtoType.Inbound);
            var sourceImage = sourceDto.GetMarkupTifImage();
            if (imageHolder.TotalPages == 1)
            {
                var destImage = sourceImage.Clone();                
                return true;
            }

            var pages = pages();
            if (pages == null)
            {
                return false;
            }

            var pageList = pages.ToList();
            var assDocPages = sourceImage.ExtractPages(pageList, onPageExtracted);
            var remainder = sourceImage.ExtractPagesInverse(pageList);
            return true;
        }*/

        public DocumentSplitResult Split(TifImage sourceImage, IEnumerable<int> pages, Action<int> onPageExtracted)
        {            
            if (sourceImage.TotalPages == 1)
            {
                var destImage = sourceImage.Clone();
                return new DocumentSplitResult(destImage);
            }
            
            if (pages == null)
            {
                return null;
            }

            var pageList = pages.ToList();
            var splitPages = sourceImage.ExtractPages(pageList, onPageExtracted);
            var remainder = sourceImage.ExtractPagesInverse(pageList, onPageExtracted);
            return new DocumentSplitResult(splitPages, remainder);
        }

    }
}
