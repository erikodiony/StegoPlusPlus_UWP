using StegoPlusPlus.Controls;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Threading.Tasks;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.Storage;
using Windows.Storage.FileProperties;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Media.Animation;
using Windows.UI.Xaml.Media.Imaging;
using Windows.UI.Xaml.Navigation;

// The Blank Page item template is documented at http://go.microsoft.com/fwlink/?LinkId=234238

namespace StegoPlusPlus.Views
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class Home_Page : Page
    {

        public Home_Page()
        {
            InitializeComponent();
        }

        private void Page_Loading(FrameworkElement sender, object args)
        {
            Init_Theme();
            Init_Transition();
        }

        #region Initializing Animation
        private void Init_Transition()
        {
            string value = (string)ApplicationData.Current.LocalSettings.Values["Effect_set"];
            Transitions = Process.Transition.GetTransition(value);
            Process.Transition.SetTransition(value);
        }
        private void Init_Theme()
        {
            string value = (string)ApplicationData.Current.LocalSettings.Values["BG_set"];
            var setTheme = Process.Theme.GetTheme(value) == true ? RequestedTheme = ElementTheme.Light : RequestedTheme = ElementTheme.Dark;
            Process.Theme.SetTheme(setTheme.ToString());
        }
        #endregion

        private void Page_SizeChanged(object sender, SizeChangedEventArgs e)
        {
            HeaderBanner.MaxHeight = this.ActualHeight / 4;
        }
    }
}
