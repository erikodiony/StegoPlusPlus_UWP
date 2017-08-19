using Windows.Storage;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using StegoPlusPlus.Controls;

// The Content Dialog item template is documented at http://go.microsoft.com/fwlink/?LinkId=234238

namespace StegoPlusPlus.Views.Popup
{
    public sealed partial class Notification : ContentDialog
    {
        public string Icon;
        public string Detail;

        public Notification()
        {
            InitializeComponent();
        }

        //Function Check Theme Status
        private void Init_Theme()
        {
            string value = (string)ApplicationData.Current.LocalSettings.Values["BG_set"];
            var setTheme = Process.Theme.GetTheme(value) == true ? RequestedTheme = ElementTheme.Light : RequestedTheme = ElementTheme.Dark;
            Process.Theme.SetTheme(setTheme.ToString());
        }

        private void ContentDialog_Loading(FrameworkElement sender, object args)
        {
            Init_Theme();
            lbl_icon.Text = Icon;
            lbl_detail.Text = Detail;
        }
    }
}
