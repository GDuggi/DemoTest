using System;
using System.Collections.Generic;
using Leadtools.Annotations;
using System.IO;
using Leadtools.Codecs;
using System.Windows.Forms;
using System.Windows.Forms.VisualStyles;
using CommonUtils;
using Leadtools;

namespace Sempra.Confirm.InBound.ImageEdit
{
    
    public class Annotation
    {
        private TifEditor tifEditor;
        private AnnCodecs annCodec;
        private RasterTagMetadata[] tagsData;
        public Annotation()
        {
            annCodec = new AnnCodecs();
            
        }

        public void SaveToFile(string fileName,bool saveAs)
        {

            System.Windows.Forms.Cursor cursor = tifEditor.Cursor;

            this.tifEditor.Cursor = Cursors.WaitCursor;
            try
            {
                if (saveAs)
                {
                    tifEditor.Codecs.Save(tifEditor.Viewer.Image, fileName, RasterImageFormat.CcittGroup4, 0, 1, tifEditor.Viewer.Image.PageCount, 1, CodecsSavePageMode.Overwrite);

                }
                if (tagsData != null)
                {
                    int numPages = tagsData.Length;
                    for (int i = 0; i < numPages; ++i)
                    {
                        
                        if (tagsData[i] != null)
                        {
                            try
                            {
                                tifEditor.Codecs.WriteTag(fileName, i + 1, tagsData[i]);
                            }
                            catch (Exception e)
                            {

                            }

                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Exception in Annotation SaveToFile: " + ex.Message);
            }
            finally
            {
                this.tifEditor.Cursor = cursor;
            }
        }


        public byte[] SaveToBytes()
        {            
            using (var stream = new MemoryStream())
            {
                tifEditor.Codecs.Save(tifEditor.Viewer.Image, stream, RasterImageFormat.CcittGroup4, 0, 1,
                    tifEditor.Viewer.Image.PageCount, 1, CodecsSavePageMode.Overwrite);

                if (tagsData != null)
                {
                    int numPages = tagsData.Length;
                    for (int i = 0; i < numPages; ++i)
                    {
                        if (tagsData[i] != null)
                        {
                            try
                            {
                                tifEditor.Codecs.WriteTag(stream, i + 1, tagsData[i]);
                            }
                            catch (Exception e)
                            {

                            }
                        }
                    }                        
                }
                return stream.GetBuffer();
            }
        }      

        public void RemoteAnnotation(string fileName, int totalPages)
        {/*
            for ( int i=totalPages;i>0;++i){
                annCodec.DeletePage(fileName, i);
            }
           */
            int length = tagsData.Length;
            for (int i = 0; i < length; ++i)
            {
                tagsData[i] = null;
            }




        }
        public void LoadFromFile(string fileName)
        {
            
            RasterCodecs codecs = TifEditor.Codecs;
            if (tifEditor.Viewer.IsImageAvailable) {
                int totalPageCount = tifEditor.Viewer.Image.PageCount;
                tagsData = new RasterTagMetadata[totalPageCount];
                for (int i=0;i<totalPageCount;++i){
                    tagsData[i] = codecs.ReadTag(fileName, i + 1, RasterTagMetadata.AnnotationTiff);
                }

            }


        }
        public void LoadFromStream(Stream stream)
        {
            RasterCodecs codecs = TifEditor.Codecs;
            if (tifEditor.Viewer.IsImageAvailable)
            {
                int totalPageCount = tifEditor.Viewer.Image.PageCount;
                tagsData = new RasterTagMetadata[totalPageCount];
                for (int i = 0; i < totalPageCount; ++i)
                {
                    tagsData[i] = codecs.ReadTag(stream, i + 1, RasterTagMetadata.AnnotationTiff);
                }

            }
        }
        public TifEditor TifEditor
        {
            get { return tifEditor; }
            set { tifEditor = value; }
        }
        public void SaveToMemory(int pageNumber)
        {

            AnnContainer container = TifEditor.AnnAutomation.Container;
            
            if (tagsData != null)
            {
          //      container.UnitConverter.DpiX = TifEditor.Viewer.Image.XResolution;
          //      container.UnitConverter.DpiY = TifEditor.Viewer.Image.YResolution;
                tagsData[pageNumber - 1] = annCodec.SaveToTag(container, AnnCodecsTagFormat.Tiff);
                
            }

            
        }
        public void LoadFromMemory(int pageNumber)
        {

            AnnContainer container = TifEditor.AnnAutomation.Container;
            
            container.Objects.Clear();
            try
            {
                TifEditor.AnnAutomation.SelectNone();
            }
            catch (Exception e)
            {
            }
            if (tagsData != null)
            {
                RasterTagMetadata tag = tagsData[pageNumber - 1];
                if (tag != null)
                {
              //      container.UnitConverter.DpiX = TifEditor.Viewer.Image.XResolution;
              //      container.UnitConverter.DpiY = TifEditor.Viewer.Image.YResolution;
                   // tag.
                    annCodec.LoadFromTag(tag, container);
                }
            }            
        }

        public void Publish(string fileName)
        {
            RasterCodecs codec = tifEditor.Codecs;
            RasterImage image = tifEditor.Viewer.Image;

            System.Windows.Forms.Cursor cursor = tifEditor.Cursor;
            
            this.tifEditor.Cursor = Cursors.WaitCursor;

            int totalPages = tifEditor.Viewer.Image.PageCount;
            RasterImageFormat format = tifEditor.Viewer.Image.OriginalFormat;
            AnnAutomation annAutomation = tifEditor.AnnAutomation;
            for (int i = 1; i <= totalPages; ++i)
            {
                tifEditor.SetPageNumber(i);
                tifEditor.Viewer.Refresh();
                annAutomation.Realize();
            }
            codec.Save(image, fileName, RasterImageFormat.TifxFaxG4, 0, 1, totalPages, 1, CodecsSavePageMode.Overwrite);
            annAutomation.Container.Objects.Clear();
            this.tifEditor.Cursor = cursor;
        }

        public byte[] PublishToBytes()
        {
            RasterCodecs codec = tifEditor.Codecs;
            RasterImage image = tifEditor.Viewer.Image;

            using (new CursorBlock(Cursors.WaitCursor))
            {
                using (var stream = new MemoryStream())
                {
                    int totalPages = tifEditor.Viewer.Image.PageCount;                    
                    AnnAutomation annAutomation = tifEditor.AnnAutomation;
                    for (int i = 1; i <= totalPages; ++i)
                    {
                        tifEditor.SetPageNumber(i);
                        tifEditor.Viewer.Refresh();
                        annAutomation.Realize();
                    }
                    codec.Save(image, stream, RasterImageFormat.TifxFaxG4, 0, 1, totalPages, 1,
                        CodecsSavePageMode.Overwrite);
                    // do I need this?
                    // annAutomation.Container.Objects.Clear();
                    return stream.GetBuffer();
                }
            }                                   
        }
    }

}
