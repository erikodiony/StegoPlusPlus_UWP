using StegoPlusPlus.Controls;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.Storage;
using Windows.UI;
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

        private void ContentDialog_Loading(FrameworkElement sender, object args)
        {
            lbl_msg.Text = Message;
            lbl_detail.Text = Detail;
            p_ring.Foreground = new SolidColorBrush(Colors.White);
            lbl_msg.Foreground = new SolidColorBrush(Colors.White);
            lbl_detail.Foreground = new SolidColorBrush(Colors.White);
        }
    }
}
