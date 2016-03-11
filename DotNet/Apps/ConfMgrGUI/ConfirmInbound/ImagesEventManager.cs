using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using DBAccess;

namespace ConfirmInbound
{
    public delegate void ImagesSelectedEventHandler(ImagesSelectedEventArgs args);
    public delegate void ImagesSavingEventHandler(ImagesSavingEventArgs args);

    public class ImagesEventManager
    {
        private static readonly Lazy<ImagesEventManager> sInstance = 
            new Lazy<ImagesEventManager>(
                () => new ImagesEventManager() 
                );

        public static ImagesEventManager Instance
        {
            get { return sInstance.Value; }
        }

        public ImagesDto CurrentSelected { get; private set; }

        public event ImagesSelectedEventHandler OnInboundDocSelected;
        public event ImagesSavingEventHandler OnInboundDocSaving;

        public void Raise(ImagesSelectedEventArgs e)
        {
            CurrentSelected = e.Selected;
            if (OnInboundDocSelected != null)
            {
                OnInboundDocSelected(e);
            }
        }

        public void Raise(ImagesSavingEventArgs e)
        {
            if (OnInboundDocSaving != null)
            {
                OnInboundDocSaving(e);
            }
        }
    }
}
