using Windows.Storage;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using StegoPlusPlus.Controls;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Text;
using Windows.UI.Xaml.Media;
using Windows.UI;
using static StegoPlusPlus.Controls.Process;

// The Content Dialog item template is documented at http://go.microsoft.com/fwlink/?LinkId=234238

namespace StegoPlusPlus.Views.Popup
{
    public sealed partial class Secret_Passwd : ContentDialog
    {
        public string Detail;

        public Secret_Passwd()
        {
            InitializeComponent();
        }

        #region Initializing Animation
        private void Init_Theme()
        {
            string value = (string)ApplicationData.Current.LocalSettings.Values["BG_set"];
            var setTheme = Theme.GetTheme(value) == true ? RequestedTheme = ElementTheme.Light : RequestedTheme = ElementTheme.Dark;
            if (setTheme == ElementTheme.Dark) loading_Bar.Foreground = new SolidColorBrush(Colors.White); else loading_Bar.Foreground = new SolidColorBrush(Colors.Black);
            Theme.SetTheme(setTheme.ToString());
        }
        #endregion

        private string Encrypt(List<byte> list_value)
        {
            string result = String.Empty;
            foreach (var x in list_value)
            {
                result += x + " ";
            }
            return result;
        }

        private async Task<string> Decrypt(List<byte> list_value)
        {
            string result = String.Empty;
            string x = Encoding.ASCII.GetString(list_value.ToArray());
            result = await Bifid_Cipher.Execute("Decrypt", x, String.Empty);
            return result;
        }

        private async Task SetEncrypt()
        {
            await Task.Delay(2500);

            Timer t = new Timer();
            t.Run(true, String.Empty);

            tbox_type.Text = Encrypt((List<byte>)GetData.Extract[Data.Misc.DataType]);
            tbox_name_file.Text = Encrypt((List<byte>)GetData.Extract[Data.Misc.DataNameFile]);
            tbox_ext_file.Text = Encrypt((List<byte>)GetData.Extract[Data.Misc.DataExtension]);
            tbox_passwd_stego.Text = Encrypt((List<byte>)GetData.Extract[Data.Misc.DataPassword]);

            if (tbox_type.Text == "49 ")
            {
                if (tbox_name_file.Text == "48 ")
                {
                    tbox_type.Text = "Text / Message (Input Manual)";
                    tbox_name_file.Text = "-";
                    tbox_ext_file.Text = "-";
                }
                else
                {
                    tbox_type.Text = "Text / Message (Input File)";
                }
            }
            else
            {
                tbox_type.Text = "File";
            }

            t.Run(false, Data.Misc.T_Encrypt);
        }

        private async Task SetDecrypt()
        {
            await Task.Delay(2500);

            Timer t = new Timer();
            t.Run(true, String.Empty);

            if (tbox_type.Text == "Text / Message (Input Manual)")
            {
                tbox_passwd_stego.Text = await Decrypt((List<byte>)GetData.Extract[Data.Misc.DataPassword]);
            }
            else
            {
                tbox_name_file.Text = await Decrypt((List<byte>)GetData.Extract[Data.Misc.DataNameFile]);
                tbox_ext_file.Text = await Decrypt((List<byte>)GetData.Extract[Data.Misc.DataExtension]);
                tbox_passwd_stego.Text = await Decrypt((List<byte>)GetData.Extract[Data.Misc.DataPassword]);
            }

            t.Run(false, Data.Misc.T_Decrypt);

        }

        private void SetTbox(bool type)
        {
            switch(type)
            {
                case true:
                    tbox_type.IsReadOnly = true;
                    tbox_name_file.IsReadOnly = true;
                    tbox_ext_file.IsReadOnly = true;
                    tbox_passwd_stego.IsReadOnly = true;
                    break;
                case false:
                    tbox_type.IsReadOnly = false;
                    tbox_name_file.IsReadOnly = false;
                    tbox_ext_file.IsReadOnly = false;
                    tbox_passwd_stego.IsReadOnly = false;
                    break;
            }
        }

        private void ContentDialog_Loading(FrameworkElement sender, object args)
        {
            Init_Theme();
        }


        private void ContentDialog_Loaded(object sender, RoutedEventArgs e)
        {
            toggle_Data.IsOn = false;
            Init();
        }

        private void toggle_Data_Toggled(object sender, RoutedEventArgs e)
        {
            Init();
        }

        private async void Init()
        {
            Task x;
            loading_Bar.IsIndeterminate = true;
            toggle_Data.IsEnabled = false;
            SetTbox(true);
            if (toggle_Data.IsOn == false) await(x = SetEncrypt()); else await(x = SetDecrypt());
            if (x.IsCompleted == true) loading_Bar.IsIndeterminate = false;
            toggle_Data.IsEnabled = true;
        }
    }
}