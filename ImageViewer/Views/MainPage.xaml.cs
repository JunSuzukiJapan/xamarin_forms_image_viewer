using Xamarin.Forms;
using ImageViewer.ViewModels;

namespace ImageViewer {
    public partial class MainPage : ContentPage {
        MainPageViewModel vm;

        public MainPage() {
            vm = new MainPageViewModel();
            BindingContext = vm;
            InitializeComponent();
        }
    }
}
