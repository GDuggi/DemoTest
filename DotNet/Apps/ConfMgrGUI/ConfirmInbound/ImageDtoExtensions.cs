using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using DBAccess;
using Sempra.Confirm.InBound.ImageEdit;

namespace ConfirmInbound
{
    /// <summary>
    /// Utility methods to help processing the images contained as TifImages, but without making DBAccess dependent on ImageEdit or Confirms Inbound
    /// </summary>
    public static class ImageDtoExtensions
    {
        public static TifImage GetMarkupTifImage(this ImagesDto thisDto)
        {
            return new TifImage(thisDto.MarkupImage);
        }
    }
}
