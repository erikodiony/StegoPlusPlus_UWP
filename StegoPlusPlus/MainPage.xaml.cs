using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.Storage;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Media.Animation;
using Windows.UI.Xaml.Navigation;

// The Blank Page item template is documented at http://go.microsoft.com/fwlink/?LinkId=402352&clcid=0x409

namespace StegoPlusPlus
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>

    public sealed partial class MainPage : Page
    {
        public MainPage()
        {
            InitializeComponent();
            ShowStatusBar();
        }
                
        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            MyFrame.Navigate(typeof(Views.Home_Page));
            Header.Text = "Home";
            Header.FontSize = 17;
            HomeRadioBtn.IsChecked = true;
        }


        private void Menu_Click(object sender, RoutedEventArgs e)
        {
            var button = sender as RadioButton;
            if (button != null)
            {
                switch (button.Content.ToString())
                {
                    case "Home":
                        Info_Page.Visibility = Visibility.Collapsed;
                        MyFrame.Navigate(typeof(Views.Home_Page));
                        break;
                    case "Embed":
                        Info_Page.Visibility = Visibility.Visible;
                        MyFrame.Navigate(typeof(Views.Embed_Page));
                        break;
                    case "Extract":
                        Info_Page.Visibility = Visibility.Visible;
                        MyFrame.Navigate(typeof(Views.Extract_Page));
                        break;
                    case "About":
                        Info_Page.Visibility = Visibility.Visible;
                        MyFrame.Navigate(typeof(Views.About_Page));
                        break;
                    case "Settings":
                        Info_Page.Visibility = Visibility.Visible;
                        MyFrame.Navigate(typeof(Views.Settings_Page));
                        break;
                }
                Header.Text = button.Content.ToString();
                if (int.Parse(((Frame)Window.Current.Content).ActualWidth.ToString()) < 1024)
                {
                    MenuList.IsPaneOpen = false;
                }
            }
        }

        private void SplitTogleBtn_Click(object sender, RoutedEventArgs e)
        {
            MenuList.IsPaneOpen =! MenuList.IsPaneOpen;
        }

        // show the StatusBar
        private async void ShowStatusBar()
        {
            // turn on SystemTray for mobile
            // don't forget to add a Reference to Windows Mobile Extensions For The UWP
            if (Windows.Foundation.Metadata.ApiInformation.IsTypePresent("Windows.UI.ViewManagement.StatusBar"))
            {
                var statusbar = Windows.UI.ViewManagement.StatusBar.GetForCurrentView();
                await statusbar.ShowAsync();
                statusbar.BackgroundColor = Windows.UI.Colors.Transparent;
                statusbar.BackgroundOpacity = 1;
                statusbar.ForegroundColor = Windows.UI.Colors.Black;
            }
        }

        #region Initializing Tips
        private void Init_Tips()
        {
            string value = (string)ApplicationData.Current.LocalSettings.Values["Tips_set"];
            var setTips = Process.Tips.GetTips(value) == false ? Toggle_Tips.IsOn = false : Toggle_Tips.IsOn = true;
            Process.Tips.SetTips(setTips.ToString());
        }
        #endregion

        private void Toggle_Tips_Toggled(object sender, RoutedEventArgs e)
        {
            string value = String.Empty;
            if (Toggle_Tips.IsOn == true) value = "True"; else value = "False";
            Process.Tips.SetTips(value);
            MyFrame.Navigate(MyFrame.SourcePageType);
        }

        private void Page_Loaded(object sender, RoutedEventArgs e)
        {
            Init_Tips();
        }
    }
}
