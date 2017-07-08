using Windows.Storage;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

// The Content Dialog item template is documented at http://go.microsoft.com/fwlink/?LinkId=234238

namespace StegoPlusPlus.Views.Popup
{
    public sealed partial class Button_Single : ContentDialog
    {
        public string Detail;

        public Button_Single()
        {
            InitializeComponent();
        }

        //Function Check Theme Status
        private void GetThemes()
        {
            string value = (string)ApplicationData.Current.LocalSettings.Values["BG_set"];

            if (value == "Dark")
            {
                RequestedTheme = ElementTheme.Dark;
            }
            else
            {
                RequestedTheme = ElementTheme.Light;
            }
        }

        private void ContentDialog_Loaded(object sender, RoutedEventArgs e)
        {
            lbl_detail.Text = Detail;
            GetThemes();
        }

        private void ContentDialog_PrimaryButtonClick(ContentDialog sender, ContentDialogButtonClickEventArgs args)
        {
        }      
    }
}
