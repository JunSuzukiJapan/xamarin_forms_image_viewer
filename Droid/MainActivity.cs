using System;

using Android.App;
using Android.Content;
using Android.Content.PM;
using Android.OS;
using Android.Database;
using Android.Provider;
using Xamarin.Forms;
using ImageViewer.Droid.Services;

namespace ImageViewer.Droid {
    [Activity(Label = "ImageViewer.Droid", Icon = "@drawable/icon", Theme = "@style/MyTheme", MainLauncher = true, ConfigurationChanges = ConfigChanges.ScreenSize | ConfigChanges.Orientation)]
    public class MainActivity : global::Xamarin.Forms.Platform.Android.FormsAppCompatActivity {
        protected override void OnCreate(Bundle bundle) {
            TabLayoutResource = Resource.Layout.Tabbar;
            ToolbarResource = Resource.Layout.Toolbar;

            base.OnCreate(bundle);

            global::Xamarin.Forms.Forms.Init(this, bundle);

            LoadApplication(new App());
        }

        protected override void OnActivityResult(int requestCode, Result resultCode, global::Android.Content.Intent data) {
            try {
                base.OnActivityResult(requestCode, resultCode, data);

                // ImageGalleryからの戻り値
                if (requestCode == ImageService.RESULT_PICK_IMAGEFILE && resultCode == Result.Ok) {
                    string filePath = this.GetSelectedItemPath(data);
                    ImageSource source = ImageSource.FromFile(filePath);
                    if (ImageService.ImageSelectedHandler != null) {
                        ImageService.ImageSelectedHandler(source);
                    }
                }
            } catch (Exception ex) {
                System.Diagnostics.Debug.WriteLine(ex.Message + System.Environment.NewLine + ex.StackTrace);
            }
        }

        private string GetSelectedItemPath(Intent data) {
            string filePath = String.Empty;
            // 選択した画像のパスを取得する.
            String strDocId = DocumentsContract.GetDocumentId(data.Data);
            String[] strSplittedDocId = strDocId.Split(':');
            String strId = strSplittedDocId[strSplittedDocId.Length - 1];

            ICursor crsCursor = this.ContentResolver.Query(
                MediaStore.Images.Media.ExternalContentUri
                , new String[] { MediaStore.MediaColumns.Data }
                , "_id=?"
                , new String[] { strId }
                , null);
            if (crsCursor.MoveToFirst()) {
                filePath = crsCursor.GetString(0);
            }
            crsCursor.Close();

            return filePath;
        }    
    }
}
