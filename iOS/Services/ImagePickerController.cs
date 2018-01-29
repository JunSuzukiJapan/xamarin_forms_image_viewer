using System;
using System.IO;
using Xamarin.Forms;
using Foundation;
using UIKit;

namespace ImageViewer.iOS.Services {
    public class ImagePickerController : UIImagePickerController {
        UIImage image;
        Action<ImageSource> mImageSelectedHander;

        public ImagePickerController(Action<ImageSource> imageSelectedHandler) : base() {
            this.mImageSelectedHander = imageSelectedHandler;
        }

        public override void ViewDidLoad() {
            base.ViewDidLoad();
            View.BackgroundColor = UIColor.White;
            var closeButtonItem = new UIBarButtonItem("Close", UIBarButtonItemStyle.Plain, this, null);
            this.NavigationItem.LeftBarButtonItem = closeButtonItem;

            // set our source to the photo library
            this.SourceType = UIImagePickerControllerSourceType.PhotoLibrary;

            // set what media types
            this.MediaTypes = UIImagePickerController.AvailableMediaTypes(UIImagePickerControllerSourceType.PhotoLibrary);

            this.FinishedPickingMedia += Handle_FinishedPickingMedia;
            this.Canceled += Handle_Canceled;
        }

        // Do something when the
        void Handle_Canceled(object sender, EventArgs e) {
            Console.WriteLine("picker cancelled");
            this.DismissModalViewController(true);
        }

        // This is a sample method that handles the FinishedPickingMediaEvent
        protected void Handle_FinishedPickingMedia(object sender, UIImagePickerMediaPickedEventArgs e) {
            // determine what was selected, video or image
            bool isImage = false;
            switch (e.Info[UIImagePickerController.MediaType].ToString()) {
            case "public.image":
                Console.WriteLine("Image selected");
                isImage = true;
                break;

            case "public.video":
                Console.WriteLine("Video selected");
                break;
            }

            Console.Write("Reference URL: [" + UIImagePickerController.ReferenceUrl + "]");

            // get common info (shared between images and video)
            NSUrl referenceURL = e.Info[new NSString("UIImagePickerControllerReferenceUrl")] as NSUrl;
            if (referenceURL != null)
                Console.WriteLine(referenceURL.ToString());

            // if it was an image, get the other image info
            if (isImage) {
                // get the original image
                UIImage originalImage = e.Info[UIImagePickerController.OriginalImage] as UIImage;
                if (originalImage != null) {
                    // do something with the image
                    Console.WriteLine("got the original image");
                    image = originalImage;
                }
                // get the edited image
                UIImage editedImage = e.Info[UIImagePickerController.EditedImage] as UIImage;
                if (editedImage != null) {
                    // do something with the image
                    Console.WriteLine("got the edited image");
                    image = editedImage;
                }
                //- get the image metadata
                NSDictionary imageMetadata = e.Info[UIImagePickerController.MediaMetadata] as NSDictionary;
                if (imageMetadata != null) {
                    // do something with the metadata
                    Console.WriteLine("got image metadata");
                }
            }
            // if it's a video
            else {
                // get video url
                NSUrl mediaURL = e.Info[UIImagePickerController.MediaURL] as NSUrl;
                if (mediaURL != null) {
                    //
                    Console.WriteLine(mediaURL.ToString());
                }
            }

            if (image != null) {
                //取得した画像をバイト配列にコピーして別ストリームで読み直す
                byte[] byteArray = null;
                using (Stream imageStream = image.AsPNG().AsStream()) {
                    imageStream.Position = 0;
                    using (MemoryStream ms = new MemoryStream()) {
                        imageStream.CopyTo(ms);
                        byteArray = ms.ToArray();
                    }
                }
                var imageSource = ImageSource.FromStream(() => new MemoryStream(byteArray));
                if (this.mImageSelectedHander != null) {
                    this.mImageSelectedHander(imageSource);
                }
            }

            // dismiss the picker
            this.DismissModalViewController(true);
        }
    }

}
