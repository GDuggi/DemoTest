using System;
using System.Collections.Generic;
using Leadtools.Codecs;
using System.IO;
using Leadtools;

namespace Sempra.Confirm.InBound.ImageEdit
{
    class ImageLoader
    {

        private RasterImage _image;
        private string fileName;
        private Stream stream;

        public Stream Stream
        {
            get { return stream; }
            set { stream = value; }
        }

        public string FileName
        {
            get { return fileName; }
            set { fileName = value; }
        }


        public RasterImage Image
        {
            get { return _image; }
        }


        internal void Load(RasterCodecs codec)
        {
            if (fileName != null)
            {
                LoadFile(codec);
            }
            else if (stream != null)
            {
                LoadStream(codec);
            }
            
        }
        private void LoadFile(RasterCodecs codec)
        {

            if (fileName == null || "".Equals(fileName))
            {
                _image = null;
                return;
            }

            CodecsTiffLoadOptions options = codec.Options.Tiff.Load;

            /*
            codec.Options.Load.Compressed = true;
             */

            //    codec.Options.Load.XResolution = 204;
            //    codec.Options.Load.YResolution = 196;
            //    


            CodecsImageInfo info = codec.GetInformation(fileName, true);
            int firstPage = 1;
            int lastPage = info.TotalPages;
            _image = codec.Load(fileName, 0, CodecsLoadByteOrder.BgrOrGray, firstPage, lastPage);
            //codec.Options.Load.

        }
        private void LoadStream(RasterCodecs codec)
        {
            if (stream == null )
            {
                _image = null;
                return;
            }

            CodecsTiffLoadOptions options = codec.Options.Tiff.Load;

            /*
            codec.Options.Load.Compressed = true;
             */

            //    codec.Options.Load.XResolution = 204;
            //    codec.Options.Load.YResolution = 196;
            //    


            CodecsImageInfo info = codec.GetInformation(stream, true);
            int firstPage = 1;
            int lastPage = info.TotalPages;
            _image = codec.Load(stream, 0, CodecsLoadByteOrder.BgrOrGray, firstPage, lastPage);

        }

    }
}
