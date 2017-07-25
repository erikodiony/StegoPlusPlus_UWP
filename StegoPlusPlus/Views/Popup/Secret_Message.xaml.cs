using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Text;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.Storage;
using Windows.UI.Text;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;
using System.Threading.Tasks;

// The Content Dialog item template is documented at http://go.microsoft.com/fwlink/?LinkId=234238

namespace StegoPlusPlus.Views.Popup
{
    public sealed partial class Secret_Message : ContentDialog
    {
        public Secret_Message()
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

        public async Task<string> Execute(string exec)
        {
            Task<string> x;
            string result;

            loading_Data.IsIndeterminate = true;

            if (exec == "Encrypt") x = Task.Run(() => Encrypt()); else x = Task.Run(() => Decrypt());
            result = await x;

            if (x.IsCompleted == true) loading_Data.IsIndeterminate = false;
            return result;
        }

        private async void toggle_Data_Toggled(object sender, RoutedEventArgs e)
        {
            if (toggle_Data.IsOn == false)
            {
                toggle_Data.IsEnabled = false;
                richeditbox_Data.IsEnabled = false;
                richeditbox_Data.IsReadOnly = false;
                richeditbox_Data.Document.SetText(TextSetOptions.None, await Execute("Encrypt"));
                richeditbox_Data.IsReadOnly = true;
                richeditbox_Data.IsEnabled = true;
                toggle_Data.IsEnabled = true;
            }
            else
            {
                toggle_Data.IsEnabled = false;
                richeditbox_Data.IsEnabled = false;
                richeditbox_Data.IsReadOnly = false;
                richeditbox_Data.Document.SetText(TextSetOptions.None, await Execute("Decrypt"));
                richeditbox_Data.IsReadOnly = true;
                richeditbox_Data.IsEnabled = true;
                toggle_Data.IsEnabled = true;
            }
        }

        private void ContentDialog_Loading(FrameworkElement sender, object args)
        {
            Init_Theme();
            richeditbox_Data.Document.SetText(TextSetOptions.None, Encrypt());
            richeditbox_Data.IsReadOnly = true;
        }

        private string Encrypt()
        {
            string result = String.Empty;
            foreach (var x in (List<byte>)Process.GetData.Extract[Data.Misc.DataSecret])
            {
                result += x + " ";
            }
            return result;
        }
        private async Task<string> Decrypt()
        {
            string result = String.Empty;
            string x = Encoding.ASCII.GetString(((List<byte>)Process.GetData.Extract[Data.Misc.DataSecret]).ToArray());
            result = await Process.Bifid_Cipher.Execute("Decrypt", x, String.Empty);
            return result;
        }


    }
}
