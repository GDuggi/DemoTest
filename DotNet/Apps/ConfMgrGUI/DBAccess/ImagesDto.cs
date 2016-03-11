using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DBAccess
{
    public enum ImagesDtoType
    {
        Inbound,
        Associated
    }

    public class ImagesDto
    {
        public Int32 RowId { get; internal set; }
        public Int32 ImageId { get; internal set; }
        
        public byte[] OriginalImage { get; private set; }
        public byte[] MarkupImage { get; set; }
        public Int32 DocsId { get; private set; }
        public string OriginalImageFileExt { get; private set; }
        public string MarkupImageFileExt { get; set; }
        public ImagesDtoType Type { get; private set; }

        public bool CanSave { get { return ImageId > 0;  } }

        public ImagesDto(Int32 docsId, byte[] markupImage, string markupImageFileExt, byte[] originalImage, string originalImageFileExt, ImagesDtoType type, Int32 imageId = 0)
        {
            DocsId = docsId;
            MarkupImage = markupImage;
            MarkupImageFileExt = markupImageFileExt;
            OriginalImage = originalImage;
            OriginalImageFileExt = originalImageFileExt;
            Type = type;
            ImageId = imageId;
        }
    }
}
