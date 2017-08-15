using System;
using Windows.Storage;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Media.Imaging;
using StegoPlusPlus.Controls;
using static StegoPlusPlus.Controls.Data.Prop_Popup;
using static StegoPlusPlus.Controls.Data.Prop_Popup.Title;

// The Blank Page item template is documented at http://go.microsoft.com/fwlink/?LinkId=234238

namespace StegoPlusPlus.Views
{
    public sealed partial class Extract_Page : Page
    {
        public Extract_Page()
        {
            InitializeComponent();
        }

        private void Page_Loading(FrameworkElement sender, object args)
        {
            Init_Transition();
            Init_Theme();
            Init_Page();
            Init_Tips();
        }

        #region Initializing Property
        private void Init_Page()
        {
            Init_STEG_StegoImage();
            Init_STEG_Passwd();
            Init_CHK_StegoImage();

            HeaderInfo.Text = Data.Prop_Page.ExtractPage;
            STEG_btn_ClearAll_FooterMenu.Label = Data.Prop_Button.ClearAll;
            STEG_btn_Execute_FooterMenu.Label = Data.Prop_Button.Execute;
            CHK_btn_ClearAll_FooterMenu.Label = Data.Prop_Button.ClearAll;
            CHK_btn_Execute_FooterMenu.Label = Data.Prop_Button.Execute;
        }
        private void Init_STEG_StegoImage()
        {
            STEG_picker_ico_stego.Visibility = Visibility.Collapsed;
            STEG_lbl_title_stego.Text = Data.Prop_Image_Stego.title;
            STEG_lbl_subtitle_stego.Text = Data.Prop_Image_Stego.subtitle;
            STEG_picker_status_stego.Text = Data.Prop_Image_Stego.picker_status;
            STEG_lbl_path_stego.Text = Data.Prop_Image_Stego.picker_path;
            STEG_picker_path_stego.Text = "-";
            STEG_lbl_size_stego.Text = Data.Prop_Image_Stego.picker_size;
            STEG_picker_size_stego.Text = "-";
            STEG_lbl_dimension_stego.Text = Data.Prop_Image_Stego.picker_dimension;
            STEG_picker_dimension_stego.Text = "-";
            STEG_btn_input_stego.Content = Data.Prop_Image_Stego.button;
        }
        private void Init_STEG_Passwd()
        {
            STEG_lbl_title_passwd.Text = Data.Prop_Passwd.title;
            STEG_lbl_subtitle_passwd.Text = Data.Prop_Passwd.subtitle;
            STEG_textbox_passwd.PlaceholderText = Data.Prop_Passwd.placeholder;
            STEG_textbox_passwd.Header = Data.Prop_Passwd.head_default;
            STEG_textbox_passwd.Password = String.Empty;
            STEG_textbox_passwd.IsEnabled = true;
            STEG_btn_save_passwd.IsEnabled = true;
            STEG_btn_save_passwd.Label = Data.Prop_Button.Save;
            STEG_btn_clear_passwd.Label = Data.Prop_Button.Clear;
        }
        private void Init_CHK_StegoImage()
        {
            CHK_picker_ico_stego.Visibility = Visibility.Collapsed;
            CHK_lbl_title_stego.Text = Data.Prop_Image_Stego.title;
            CHK_lbl_subtitle_stego.Text = Data.Prop_Image_Stego.subtitle;
            CHK_picker_status_stego.Text = Data.Prop_Image_Stego.picker_status;
            CHK_lbl_path_stego.Text = Data.Prop_Image_Stego.picker_path;
            CHK_picker_path_stego.Text = "-";
            CHK_lbl_size_stego.Text = Data.Prop_Image_Stego.picker_size;
            CHK_picker_size_stego.Text = "-";
            CHK_lbl_dimension_stego.Text = Data.Prop_Image_Stego.picker_dimension;
            CHK_picker_dimension_stego.Text = "-";
            CHK_btn_input_stego.Content = Data.Prop_Image_Stego.button;
        }
        #endregion
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
        #region Initializing Tips
        private void Init_Tips()
        {
            string value = (string)ApplicationData.Current.LocalSettings.Values["Tips_set"];
            if (value == "True")
            {
                Tips_Prop.Visibility = Visibility.Visible;
                Tips_Prop2.Visibility = Visibility.Visible;
                Tips_Prop3.Margin = new Thickness(0, 0, 0, 0);
            }
            else
            {
                Tips_Prop.Visibility = Visibility.Collapsed;
                Tips_Prop2.Visibility = Visibility.Collapsed;
                Tips_Prop3.Margin = new Thickness(0, -5, 0, 0);
            }
        }
        #endregion
        #region Initializing Result
        private void Init_StegoImage_NEW(string type)
        {
            switch(type)
            {
                case "STEG":
                    STEG_picker_status_stego.Text = Process.GetData.Picker[Data.Misc.Name].ToString();
                    STEG_picker_path_stego.Text = Process.GetData.Picker[Data.Misc.Path].ToString();
                    STEG_picker_size_stego.Text = String.Format("{0} bytes", Process.GetData.Picker[Data.Misc.Size].ToString());
                    STEG_picker_dimension_stego.Text = Process.GetData.Picker[Data.Misc.Dimensions].ToString();
                    STEG_picker_ico_stego.Source = (BitmapImage)Process.GetData.Picker[Data.Misc.Icon];
                    STEG_picker_ico_stego.Visibility = Visibility.Visible;
                    break;
                case "CHK":
                    CHK_picker_status_stego.Text = Process.GetData.Picker[Data.Misc.Name].ToString();
                    CHK_picker_path_stego.Text = Process.GetData.Picker[Data.Misc.Path].ToString();
                    CHK_picker_size_stego.Text = String.Format("{0} bytes", Process.GetData.Picker[Data.Misc.Size].ToString());
                    CHK_picker_dimension_stego.Text = Process.GetData.Picker[Data.Misc.Dimensions].ToString();
                    CHK_picker_ico_stego.Source = (BitmapImage)Process.GetData.Picker[Data.Misc.Icon];
                    CHK_picker_ico_stego.Visibility = Visibility.Visible;
                    break;
            }
        }
        #endregion

        //++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++//

        #region Trigger (Extract File / Message)
        private async void STEG_btn_input_stego_Click(object sender, RoutedEventArgs e)
        {
            if (await Process.Picker.Embed(Data.File_Extensions.Png, "Stego") == true) Init_StegoImage_NEW("STEG"); else Init_STEG_StegoImage();
        }
        private async void STEG_btn_clear_passwd_Click(object sender, RoutedEventArgs e)
        {
            if (STEG_textbox_passwd.Password != String.Empty)
            {
                Init_STEG_Passwd();
                Process.GetData.Reset_Data("Extract", "Passwd");
                await PopupDialog.Show(Status.Success, Detail.Insert_Password, Complete.Clear_Input_Passwd, Icon.Smile);
            }
        }
        private async void STEG_btn_save_passwd_Click(object sender, RoutedEventArgs e)
        {
            if (STEG_textbox_passwd.Password != String.Empty)
            {
                if (Process.Validate.Input(STEG_textbox_passwd.Password) == false)
                {
                    await PopupDialog.Show(Status.Err, Detail.Insert_Password, Err.Input_Invalid_Passwd, Icon.Sad);
                }
                else
                {
                    STEG_textbox_passwd.IsEnabled = false;
                    STEG_btn_save_passwd.IsEnabled = false;
                }
            }
            else
            {
                await PopupDialog.Show(Status.Err, Detail.Insert_Password, Err.Input_Empty_Passwd, Icon.Sad);
            }
        }
        private async void STEG_btn_ClearAll_FooterMenu_Click(object sender, RoutedEventArgs e)
        {
            Init_Page();
            Process.GetData.Picker.Clear();
            Process.GetData.Reset_Data("Extract", "All");
            await PopupDialog.Show(Status.Success, Detail.Extract_FileMessage, Complete.Clear_All, Icon.Smile);
        }
        private void STEG_btn_Execute_FooterMenu_Click(object sender, RoutedEventArgs e)
        {
            Exec("STEG");
        }
        #endregion

        //++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++//

        #region Trigger (Check Stego Info)
        private async void CHK_btn_input_stego_Click(object sender, RoutedEventArgs e)
        {
            if (await Process.Picker.Embed(Data.File_Extensions.Png, "Stego") == true) Init_StegoImage_NEW("CHK"); else Init_CHK_StegoImage();
        }
        private async void CHK_btn_ClearAll_FooterMenu_Click(object sender, RoutedEventArgs e)
        {
            Init_Page();
            Process.GetData.Picker.Clear();
            Process.GetData.Reset_Data("Embed", "All");
            await PopupDialog.Show(Status.Success, Detail.Extract_Check, Complete.Clear_All, Icon.Smile);
        }
        private void CHK_btn_Execute_FooterMenu_Click(object sender, RoutedEventArgs e)
        {
            Exec("CHK");
        }
        #endregion

        //++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++//

        #region Function Controlling
        private async void Exec(string type)
        {
            switch(type)
            {
                case "STEG":
                    if (STEG_picker_status_stego.Text != "No Image" && STEG_btn_save_passwd.IsEnabled == false)
                    {
                        if (await PopupDialog.ShowConfirm(Status.Confirm, Detail.Extract_FileMessage, Confirm.isExecute, Icon.Flat) == true) Process.Extract.Starting(type, STEG_textbox_passwd.Password);
                    }
                    else
                    {
                        await PopupDialog.Show(Status.Err, Detail.Extract_FileMessage, Err.Input_isNull, Icon.Sad);
                    }
                    break;
                case "CHK":
                    if (CHK_picker_status_stego.Text != "No Image")
                    {
                        if (await PopupDialog.ShowConfirm(Status.Confirm, Detail.Extract_Check, Confirm.isExecute, Icon.Flat) == true) Process.Extract.Starting(type, String.Empty);
                    }
                    else
                    {
                        await PopupDialog.Show(Status.Err, Detail.Extract_Check, Err.Input_isNull, Icon.Sad);
                    }
                    break;
            }
        }
        #endregion

    }
}
