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
    public sealed partial class CInfoStego : ContentDialog
    {
        DataProcess dp = new DataProcess();

        public CInfoStego()
        {
            this.InitializeComponent();
            InitializingPage();
        }

        private void InitializingPage()
        {
            if (System.Text.Encoding.ASCII.GetString(DataProcess.def) == "1")
            {
                txt_StegoType.Text = " = File";
            }
            else
            {
                txt_StegoType.Text = " = Text/Message";
            }

            if (System.Text.Encoding.ASCII.GetString(DataProcess.ext) == "0")
            {
                txt_DataExtension.Text = " = (Tidak didukung)";
            }
            else
            {
                txt_DataExtension.Text = String.Format(" = {0}", System.Text.Encoding.ASCII.GetString(DataProcess.ext));
            }

            tbox_DataPasswd.Text = System.Text.Encoding.ASCII.GetString(DataProcess.passwd_encrypt);

        }

        private void toggle_Data_Toggled(object sender, RoutedEventArgs e)
        {
            if (toggle_Data.IsOn == false)
            {
                tbox_DataPasswd.Text = System.Text.Encoding.ASCII.GetString(DataProcess.passwd_encrypt);
            }
            else
            {
                tbox_DataPasswd.Text = dp.Decrypt_BifidCipher(System.Text.Encoding.ASCII.GetString(DataProcess.passwd_encrypt));
            }
        }
    }
}
