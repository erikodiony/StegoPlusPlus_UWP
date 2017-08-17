using System;
using System.Collections.Generic;
using System.Text;
using Windows.Storage;
using Windows.UI.Text;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using System.Threading.Tasks;
using StegoPlusPlus.Controls;
using Windows.UI.Xaml.Media;
using Windows.UI;

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
            if (setTheme == ElementTheme.Dark) loading_Data.Foreground = new SolidColorBrush(Colors.White); else loading_Data.Foreground = new SolidColorBrush(Colors.Black);
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
                btn_SaveAs.IsEnabled = false;
                richeditbox_Data.Document.SetText(TextSetOptions.None, await Execute("Encrypt"));
                btn_SaveAs.IsEnabled = true;
                richeditbox_Data.IsReadOnly = true;
                richeditbox_Data.IsEnabled = true;
                toggle_Data.IsEnabled = true;
            }
            else
            {
                toggle_Data.IsEnabled = false;
                richeditbox_Data.IsEnabled = false;
                richeditbox_Data.IsReadOnly = false;
                btn_SaveAs.IsEnabled = false;
                richeditbox_Data.Document.SetText(TextSetOptions.None, await Execute("Decrypt"));
                btn_SaveAs.IsEnabled = true;
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

        private async void btn_SaveAs_Click(object sender, RoutedEventArgs e)
        {
            Hide();
            await Process.Extract.Save("Message");
        }

        private void ContentDialog_SizeChanged(object sender, SizeChangedEventArgs e)
        {
            richeditbox_Data.Height = grid_CDialog.ActualHeight - 120;
        }

        private void ContentDialog_Loaded(object sender, RoutedEventArgs e)
        {
            richeditbox_Data.Height = grid_CDialog.ActualHeight - 120;
        }
    }
}
