using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;

// The Content Dialog item template is documented at http://go.microsoft.com/fwlink/?LinkId=234238

namespace StegoPlusPlus.Views
{
    public sealed partial class CDialog : ContentDialog
    {
        DataProcess dp = new DataProcess();

        public CDialog()
        {
            this.InitializeComponent();
            txt_DataMessage.Text = System.Text.Encoding.ASCII.GetString(DataProcess.data);
        }

        private void Toggle_Data_Toggled(object sender, RoutedEventArgs e)
        {
            if (toggle_Data.IsOn == false)
            {
                txt_DataMessage.Text = System.Text.Encoding.ASCII.GetString(DataProcess.data);
            }
            else
            {
                txt_DataMessage.Text = dp.Decrypt_BifidCipher(System.Text.Encoding.ASCII.GetString(DataProcess.data));
            }
        }
        

    }
}
