using Prism.Commands;
using Prism.Mvvm;
using System.Windows.Input;
using Xamarin.Forms;
using ImageViewer.Services;

namespace ImageViewer.ViewModels {
    public class MainPageViewModel : BindableBase {
        public ICommand SelectImageCommand { get; }

        private ImageSource mImageSource;
        public ImageSource ImageSource {
            get { return mImageSource; }
            set { SetProperty(ref mImageSource, value); }
        }

        public MainPageViewModel() {
            SelectImageCommand = new DelegateCommand(() => {
                DependencyService.Get<IImageService>().ShowImageGallery((imageSoruce) => {
                    this.ImageSource = imageSoruce;
                });
            });

        }
    }
}
