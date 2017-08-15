using StegoPlusPlus.Controls;
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
using Windows.UI.Xaml.Navigation;

// The Content Dialog item template is documented at http://go.microsoft.com/fwlink/?LinkId=234238

namespace StegoPlusPlus.Views.Popup
{
    public sealed partial class Progress : ContentDialog
    {
        public string Message;
        public string Detail;
        public Progress()
        {
            InitializeComponent();
        }

        private void Init_Theme()
        {
            string value = (string)ApplicationData.Current.LocalSettings.Values["BG_set"];
            var setTheme = Process.Theme.GetTheme(value) == true ? RequestedTheme = ElementTheme.Light : RequestedTheme = ElementTheme.Dark;
            Process.Theme.SetTheme(setTheme.ToString());
        }

        private void ContentDialog_Loading(FrameworkElement sender, object args)
        {
            Init_Theme();
            lbl_msg.Text = Message;
            lbl_detail.Text = Detail;
        }
    }
}
