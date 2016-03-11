using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using DBAccess;

namespace ConfirmInbound
{
    public class ImagesSelectedEventArgs : EventArgs
    {
        public ImagesDto Selected { get; private set; }
        public bool CanEditImage { get; private set; }

        public ImagesSelectedEventArgs(ImagesDto selected, bool canEditImage = true)
        {
            Selected = selected;
            CanEditImage = canEditImage;
        }
    }
}
