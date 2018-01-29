using System;
using Xamarin.Forms;
using Android.App;
using Android.Content;
using ImageViewer.Services;

[assembly: Dependency(typeof(ImageViewer.Droid.Services.ImageService))]
namespace ImageViewer.Droid.Services {
    public class ImageService : IImageService {
        public static Action<ImageSource> ImageSelectedHandler { get; private set; }
        public const int RESULT_PICK_IMAGEFILE = 1000;

        public void ShowImageGallery(Action<ImageSource> imageSelectedHandler) {
            ImageService.ImageSelectedHandler = imageSelectedHandler;

            // アクティビティを取得する
            var activity = (Activity)(Forms.Context);

            // ギャラリーを表示する
            var imageIntent = new Intent();
            imageIntent.SetType("image/*");
            imageIntent.SetAction(Intent.ActionGetContent);
            activity.StartActivityForResult(
                Intent.CreateChooser(imageIntent, "Select photo"), RESULT_PICK_IMAGEFILE);
        }
    }
}
