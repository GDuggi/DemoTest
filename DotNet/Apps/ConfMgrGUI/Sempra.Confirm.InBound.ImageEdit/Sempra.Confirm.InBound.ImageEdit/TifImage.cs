using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using Leadtools;
using Leadtools.Codecs;

namespace Sempra.Confirm.InBound.ImageEdit
{
    public class TifImage : IImageHolder
    {
        private readonly IDictionary<int, RasterTagMetadata> tagsByPageNumber = new Dictionary<int, RasterTagMetadata>();               
        private RasterCodecs codec;

        public RasterImage ImageData { get; private set; }

        public TifImage(Stream stream)
        {
            InitializeFromStream(stream);
        }

        public TifImage(byte[] buffer)
        {
            using (var stream = new MemoryStream(buffer))
            {
                InitializeFromStream(stream);
            }
        }

        private TifImage()
        {
            
        }

        private void InitializeFromStream(Stream stream)
        {
            codec = new RasterCodecs();
            codec.Options.Txt.Load.Enabled = true;
            CodecsImageInfo info = codec.GetInformation(stream, true);
            int lastPage = info.TotalPages;
            ImageData = codec.Load(stream, 0, CodecsLoadByteOrder.BgrOrGray, 1, lastPage);
            codec.Options.Load.XResolution = ImageData.XResolution;
            codec.Options.Load.YResolution = ImageData.YResolution;
            LoadTagsMetaData(stream);
        }

        private void LoadTagsMetaData(Stream stream)
        {
            for (int i = 0; i < TotalPages; i++)
            {
                var pageNumber = i + 1;
                RasterTagMetadata tagMetaData = codec.ReadTag(stream, pageNumber, RasterTagMetadata.AnnotationTiff);
                if (tagMetaData != null)
                {
                    tagsByPageNumber.Add(pageNumber, tagMetaData);
                }                
            }
        }

        public int TotalPages
        {
            get { return ImageData.PageCount; }
        }

        public bool ImageModified
        {
            get { throw new NotImplementedException(); }
        }

        public byte[] GetImageBytes()
        {
            using (var stream = new MemoryStream())
            {
                codec.Save(ImageData, stream, RasterImageFormat.CcittGroup4, 0, 1,
                    TotalPages, 1, CodecsSavePageMode.Overwrite);

                foreach (var entry in tagsByPageNumber)
                {
                    codec.WriteTag(stream, entry.Key, entry.Value);
                }
                
                return stream.GetBuffer();
            }
        }

        public TifImage ExtractPages(IEnumerable<int> pages, Action<int> onPageExtracted = null)
        {            
            using (var stream = new MemoryStream())
            {
                foreach (var pageNumber in pages)
                {
                    codec.Save(ImageData, stream, RasterImageFormat.CcittGroup4, 1, pageNumber, pageNumber, 0,
                        CodecsSavePageMode.Append);
                    RasterTagMetadata tagMetaData;
                    if (tagsByPageNumber.TryGetValue(pageNumber, out tagMetaData))
                    {
                        codec.WriteTag(stream, pageNumber, tagMetaData);
                    }
                    if (onPageExtracted != null)
                    {
                        onPageExtracted(pageNumber);
                    }
                }
                return new TifImage(stream);
            }
        }

        public TifImage ExtractPagesInverse(IEnumerable<int> pagesToSkip, Action<int> onPageExtracted = null)
        {
            var pages = new SortedSet<int>(Enumerable.Range(1, TotalPages).Where(n => !pagesToSkip.Contains(n)));
            return pages.Count < 1 ? null : ExtractPages(pages, onPageExtracted);
        }

        public TifImage Clone()
        {
            return new TifImage(GetImageBytes());
        }

        /// <summary>
        /// Creates a new TifImage that is "this" with the passed in image appended to the end of it.
        /// </summary>
        /// <param name="toAppend">The image you want to append to this one</param>
        /// <returns>A brand new image that contains "this" + toAppend</returns>
        public TifImage Merge(TifImage toAppend)
        {
            using (var stream = new MemoryStream())
            {                
                foreach (var image in new[] { this, toAppend} )
                {
                    for (var i = 1; i <= image.TotalPages; i++)
                    {
                        codec.Save(image.ImageData, 
                                   stream, 
                                   RasterImageFormat.CcittGroup4, 
                                   1, 
                                   i, 
                                   i, 
                                   0,
                                   CodecsSavePageMode.Append);

                        RasterTagMetadata tagMetaData;
                        if (image.tagsByPageNumber.TryGetValue(i, out tagMetaData))
                        {
                            codec.WriteTag(stream, i, tagMetaData);
                        }                    
                    }
                }
                
                return new TifImage(stream);
            }
        }
    }
}
