using System;
using Xamarin.Forms;
using UIKit;
using ImageViewer.Services;

[assembly: Dependency(typeof(ImageViewer.iOS.Services.ImageService))]
namespace ImageViewer.iOS.Services {
    public class ImageService : IImageService {
        public void ShowImageGallery(Action<ImageSource> imageSelectedHandler) {
            UIViewController viewController;
            UIViewController parentViewController;

            viewController = new ImagePickerController(imageSelectedHandler);
            parentViewController = this.FindViewController();
            parentViewController.PresentViewController(viewController, true, null);
        }

        private UIViewController FindViewController() {
            foreach (var window in UIApplication.SharedApplication.Windows) {
                if (window.RootViewController == null) {
                    continue;
                } else {
                    return window.RootViewController;
                }
            }

            return null;
        }
    }
}
