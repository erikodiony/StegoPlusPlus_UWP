using Windows.Storage;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using StegoPlusPlus.Controls;

// The Content Dialog item template is documented at http://go.microsoft.com/fwlink/?LinkId=234238

namespace StegoPlusPlus.Views.Popup
{
    public sealed partial class Secret_Passwd : ContentDialog
    {
        public string Detail;

        public Secret_Passwd()
        {
            InitializeComponent();
        }

        private void ContentDialog_Loaded(object sender, RoutedEventArgs e)
        {
            Init_Theme();
            //lbl_detail.Text = Detail;
        }

        #region Initializing Animation
        private void Init_Theme()
        {
            string value = (string)ApplicationData.Current.LocalSettings.Values["BG_set"];
            var setTheme = Process.Theme.GetTheme(value) == true ? RequestedTheme = ElementTheme.Light : RequestedTheme = ElementTheme.Dark;
            Process.Theme.SetTheme(setTheme.ToString());
        }
        #endregion

        private void ContentDialog_PrimaryButtonClick(ContentDialog sender, ContentDialogButtonClickEventArgs args)
        {

        }
    }
}
