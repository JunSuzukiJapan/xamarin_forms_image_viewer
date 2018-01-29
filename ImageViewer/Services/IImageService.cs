using System;
using Xamarin.Forms;

namespace ImageViewer.Services {
    public interface IImageService {
        void ShowImageGallery(Action<ImageSource> imageSelectedHandler);
    }
}
