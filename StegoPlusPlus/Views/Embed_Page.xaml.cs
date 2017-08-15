using StegoPlusPlus.Controls;
using System;
using Windows.Storage;
using Windows.UI.Text;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media.Imaging;
using static StegoPlusPlus.Controls.Data.Prop_Popup;
using static StegoPlusPlus.Controls.Data.Prop_Popup.Title;

namespace StegoPlusPlus.Views
{
    public sealed partial class Embed_Page : Page
    {
        public Embed_Page()
        {
            InitializeComponent();
        }

        private void InputMessage_KeyUp(object sender, KeyRoutedEventArgs e)
        {
            if (MSG_richeditbox_message.IsReadOnly == false)
            {
                string count_msg = String.Empty;
                MSG_richeditbox_message.Document.GetText(TextGetOptions.UseCrlf, out count_msg);
                MSG_picker_count_message.Text = count_msg.Length.ToString();
            }
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
            Init_F_CoverImage();
            Init_F_File();
            Init_F_Passwd();
            Init_MSG_CoverImage();
            Init_MSG_Message();
            Init_MSG_Passwd();

            HeaderInfo.Text = Data.Prop_Page.EmbedPage;
            MSG_btn_ClearAll_FooterMenu.Label = Data.Prop_Button.ClearAll;
            MSG_btn_Execute_FooterMenu.Label = Data.Prop_Button.Execute;
            F_btn_ClearAll_FooterMenu.Label = Data.Prop_Button.ClearAll;
            F_btn_Execute_FooterMenu.Label = Data.Prop_Button.Execute;
        }
        private void Init_MSG_CoverImage()
        {
            MSG_picker_ico_cover.Visibility = Visibility.Collapsed;
            MSG_lbl_title_cover.Text = Data.Prop_Image_Cover.title;
            MSG_lbl_subtitle_cover.Text = Data.Prop_Image_Cover.subtitle;
            MSG_picker_status_cover.Text = Data.Prop_Image_Cover.picker_status;
            MSG_lbl_path_cover.Text = Data.Prop_Image_Cover.picker_path;
            MSG_picker_path_cover.Text = "-";
            MSG_lbl_size_cover.Text = Data.Prop_Image_Cover.picker_size;
            MSG_picker_size_cover.Text = "-";
            MSG_lbl_dimension_cover.Text = Data.Prop_Image_Cover.picker_dimension;
            MSG_picker_dimension_cover.Text = "-";
            MSG_lbl_eta_cover.Text = Data.Prop_Image_Cover.picker_eta_msg;
            MSG_picker_eta_cover.Text = "-";
            MSG_btn_input_cover.Content = Data.Prop_Image_Cover.button;
        }
        private void Init_MSG_Passwd()
        {
            MSG_lbl_title_passwd.Text = Data.Prop_Passwd.title;
            MSG_lbl_subtitle_passwd.Text = Data.Prop_Passwd.subtitle;
            MSG_textbox_passwd.PlaceholderText = Data.Prop_Passwd.placeholder;
            MSG_textbox_passwd.Header = Data.Prop_Passwd.head_default;
            MSG_textbox_passwd.Text = String.Empty;
            MSG_textbox_passwd.IsReadOnly = false;
            MSG_btn_save_passwd.IsEnabled = true;
            MSG_btn_save_passwd.Label = Data.Prop_Button.Save;
            MSG_btn_clear_passwd.Label = Data.Prop_Button.Clear;
        }
        private void Init_MSG_Message()
        {
            MSG_picker_ico_message.Visibility = Visibility.Collapsed;
            MSG_lbl_title_message.Text = Data.Prop_Secret_Message.title;
            MSG_lbl_subtitle_message.Text = Data.Prop_Secret_Message.subtitle;
            MSG_lbl_subtitle2_message.Text = Data.Prop_Secret_Message.subtitle2;
            MSG_btn_input_message.Content = Data.Prop_Secret_Message.button;
            MSG_picker_status_message.Text = Data.Prop_Secret_Message.picker_status;
            MSG_lbl_path_message.Text = Data.Prop_Secret_Message.picker_path;
            MSG_picker_path_message.Text = "-";
            MSG_lbl_size_message.Text = Data.Prop_Secret_Message.picker_size;
            MSG_picker_size_message.Text = "-";
            MSG_lbl_type_message.Text = Data.Prop_Secret_Message.picker_type;
            MSG_picker_type_message.Text = "-";
            MSG_richeditbox_message.Header = Data.Prop_Secret_Message.head_default;
            MSG_richeditbox_message.PlaceholderText = Data.Prop_Secret_Message.placeholder;
            MSG_richeditbox_message.IsReadOnly = false;
            MSG_richeditbox_message.Document.SetText(TextSetOptions.None, String.Empty);
            MSG_lbl_counter_message.Text = Data.Prop_Secret_Message.counter;
            MSG_picker_count_message.Text = "0";
            MSG_btn_save_message.Label = Data.Prop_Button.Save;
            MSG_btn_save_message.IsEnabled = true;
            MSG_btn_clear_message.Label = Data.Prop_Button.Clear;
        }
        private void Init_F_CoverImage()
        {
            F_picker_ico_cover.Visibility = Visibility.Collapsed;
            F_lbl_title_cover.Text = Data.Prop_Image_Cover.title;
            F_lbl_subtitle_cover.Text = Data.Prop_Image_Cover.subtitle;
            F_picker_status_cover.Text = Data.Prop_Image_Cover.picker_status;
            F_lbl_path_cover.Text = Data.Prop_Image_Cover.picker_path;
            F_picker_path_cover.Text = "-";
            F_lbl_size_cover.Text = Data.Prop_Image_Cover.picker_size;
            F_picker_size_cover.Text = "-";
            F_lbl_dimension_cover.Text = Data.Prop_Image_Cover.picker_dimension;
            F_picker_dimension_cover.Text = "-";
            F_lbl_eta_cover.Text = Data.Prop_Image_Cover.picker_eta_file;
            F_picker_eta_cover.Text = "-";
            F_btn_input_cover.Content = Data.Prop_Image_Cover.button;
        }
        private void Init_F_Passwd()
        {
            F_lbl_title_passwd.Text = Data.Prop_Passwd.title;
            F_lbl_subtitle_passwd.Text = Data.Prop_Passwd.subtitle;
            F_textbox_passwd.PlaceholderText = Data.Prop_Passwd.placeholder;
            F_textbox_passwd.Header = Data.Prop_Passwd.head_default;
            F_textbox_passwd.Text = String.Empty;
            F_textbox_passwd.IsReadOnly = false;
            F_btn_save_passwd.IsEnabled = true;
            F_btn_save_passwd.Label = Data.Prop_Button.Save;
            F_btn_clear_passwd.Label = Data.Prop_Button.Clear;
        }
        private void Init_F_File()
        {
            F_picker_ico_file.Visibility = Visibility.Collapsed;
            F_lbl_title_file.Text = Data.Prop_Secret_File.title;
            F_lbl_subtitle_file.Text = Data.Prop_Secret_File.subtitle;
            F_picker_status_file.Text = Data.Prop_Secret_File.picker_status;
            F_lbl_path_file.Text = Data.Prop_Secret_File.picker_path;
            F_picker_path_file.Text = "-";
            F_lbl_size_file.Text = Data.Prop_Secret_File.picker_size;
            F_picker_size_file.Text = "-";
            F_lbl_type_file.Text = Data.Prop_Secret_File.picker_type;
            F_picker_type_file.Text = "-";
            F_btn_input_file.Content = Data.Prop_Secret_File.button;
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
        private void Init_CoverImage_NEW(string section)
        {
            switch(section)
            {
                case "FILE":
                    F_picker_status_cover.Text = Process.GetData.Picker[Data.Misc.Name].ToString();
                    F_picker_path_cover.Text = Process.GetData.Picker[Data.Misc.Path].ToString();
                    F_picker_size_cover.Text = String.Format("{0} bytes", Process.GetData.Picker[Data.Misc.Size].ToString());
                    F_picker_dimension_cover.Text = String.Format("{0} / ({1} BitDepth | {2} Pixel)", Process.GetData.Picker[Data.Misc.Dimensions].ToString(), Process.GetData.Picker[Data.Misc.BitDepth].ToString(), Process.GetData.Picker[Data.Misc.Pixel].ToString());
                    F_picker_eta_cover.Text = String.Format("± {0} bytes", Process.GetData.Picker[Data.Misc.Eta].ToString());
                    F_picker_ico_cover.Source = (BitmapImage)Process.GetData.Picker[Data.Misc.Icon];
                    F_picker_ico_cover.Visibility = Visibility.Visible;
                    break;
                case "MESSAGE":
                    MSG_picker_status_cover.Text = Process.GetData.Picker[Data.Misc.Name].ToString();
                    MSG_picker_path_cover.Text = Process.GetData.Picker[Data.Misc.Path].ToString();
                    MSG_picker_size_cover.Text = String.Format("{0} bytes", Process.GetData.Picker[Data.Misc.Size].ToString());
                    MSG_picker_dimension_cover.Text = String.Format("{0} / ({1} BitDepth | {2} Pixel)", Process.GetData.Picker[Data.Misc.Dimensions].ToString(), Process.GetData.Picker[Data.Misc.BitDepth].ToString(), Process.GetData.Picker[Data.Misc.Pixel].ToString());
                    MSG_picker_eta_cover.Text = String.Format("± {0} character", Process.GetData.Picker[Data.Misc.Eta].ToString());
                    MSG_picker_ico_cover.Source = (BitmapImage)Process.GetData.Picker[Data.Misc.Icon];
                    MSG_picker_ico_cover.Visibility = Visibility.Visible;
                    break;
            }
        }
        private void Init_PickerSecret_NEW(string section)
        {
            switch(section)
            {
                case "FILE":
                    F_picker_status_file.Text = Process.GetData.Picker[Data.Misc.Name].ToString();
                    F_picker_path_file.Text = Process.GetData.Picker[Data.Misc.Path].ToString();
                    F_picker_size_file.Text = String.Format("{0} bytes", Process.GetData.Picker[Data.Misc.Size].ToString());
                    F_picker_type_file.Text = Process.GetData.Picker[Data.Misc.Type].ToString();
                    F_picker_ico_file.Source = (BitmapImage)Process.GetData.Picker[Data.Misc.Icon];
                    F_picker_ico_file.Visibility = Visibility.Visible;
                    break;
                case "MESSAGE":
                    MSG_picker_status_message.Text = Process.GetData.Picker[Data.Misc.Name].ToString();
                    MSG_picker_path_message.Text = Process.GetData.Picker[Data.Misc.Path].ToString();
                    MSG_picker_size_message.Text = String.Format("{0} bytes", Process.GetData.Picker[Data.Misc.Size].ToString());
                    MSG_picker_type_message.Text = Process.GetData.Picker[Data.Misc.Type].ToString();
                    MSG_picker_ico_message.Source = (BitmapImage)Process.GetData.Picker[Data.Misc.Icon];
                    MSG_picker_ico_message.Visibility = Visibility.Visible;                    
                    MSG_richeditbox_message.Document.SetText(TextSetOptions.None, Process.GetData.Picker[Data.Misc.Text].ToString());
                    MSG_richeditbox_message.IsReadOnly = true;
                    MSG_picker_count_message.Text = (Process.GetData.Picker[Data.Misc.Text].ToString()).Length.ToString();
                    break;
            }
        }
        private void Init_Text_NEW(string type, string section)
        {
            if (type == "PASSWD")
            {
                switch (section)
                {
                    case "FILE":
                        F_textbox_passwd.Text = Process.GetData.Picker[Data.Misc.Text].ToString();
                        F_textbox_passwd.Header = Data.Prop_Passwd.head_save;
                        F_textbox_passwd.IsReadOnly = true;
                        F_btn_save_passwd.IsEnabled = false;
                        break;
                    case "MESSAGE":
                        MSG_textbox_passwd.Text = Process.GetData.Picker[Data.Misc.Text].ToString();
                        MSG_textbox_passwd.Header = Data.Prop_Passwd.head_save;
                        MSG_textbox_passwd.IsReadOnly = true;
                        MSG_btn_save_passwd.IsEnabled = false;
                        break;
                }
            }
            else
            {
                MSG_richeditbox_message.IsReadOnly = false;
                MSG_richeditbox_message.Document.SetText(TextSetOptions.None, Process.GetData.Picker[Data.Misc.Text].ToString());
                MSG_richeditbox_message.IsReadOnly = true;
                MSG_richeditbox_message.Header = Data.Prop_Secret_Message.head_save;
                MSG_btn_save_message.IsEnabled = false;
            }
        }
        #endregion

        //++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++//

        #region Trigger (Embed File)
        private async void F_btn_input_cover_CLICK(object sender, RoutedEventArgs e)
        {
            if (await Process.Picker.Embed(Data.File_Extensions.Png, "Image") == true) Init_CoverImage_NEW("FILE"); else Init_F_CoverImage();
        }
        private async void F_btn_input_file_CLICK(object sender, RoutedEventArgs e)
        {
            if (await Process.Picker.Embed(Data.File_Extensions.All, "File") == true) Init_PickerSecret_NEW("FILE"); else Init_F_File();
        }
        private async void btn_Save_Password_file_Click(object sender, RoutedEventArgs e)
        {
            if (F_textbox_passwd.Text != String.Empty)
            {
                if (Process.Validate.Input(F_textbox_passwd.Text) == false)
                {
                    await PopupDialog.Show(Status.Err, Detail.Insert_Password, Err.Input_Invalid_Passwd, Icon.Sad);
                }
                else
                {
                    await Process.Bifid_Cipher.Execute("Embed", F_textbox_passwd.Text, "Passwd");
                    Init_Text_NEW("PASSWD", "FILE");
                }
            }
            else
            {
                await PopupDialog.Show(Status.Err, Detail.Insert_Password, Err.Input_Empty_Passwd, Icon.Sad);
            }
        }       
        private async void btn_Clear_Password_file_Click(object sender, RoutedEventArgs e)
        {
            if (F_textbox_passwd.Text != String.Empty)
            {
                Init_F_Passwd();
                Process.GetData.Reset_Data("Embed","Passwd");
                await PopupDialog.Show(Status.Success, Detail.Insert_Password, Complete.Clear_Input_Passwd, Icon.Smile);
            }
        }
        private async void btn_Clear_FooterMenuEmbedFile_Click(object sender, RoutedEventArgs e)
        {
            Init_Page();
            Process.GetData.Picker.Clear();
            Process.GetData.Reset_Data("Embed","All");
            await PopupDialog.Show(Status.Success, Detail.Embed_File, Complete.Clear_All, Icon.Smile);
        }
        private void btn_Exec_FooterMenuEmbedFile_Click(object sender, RoutedEventArgs e)
        {
            Exec("File");
        }
        #endregion

        //++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++//

        #region Trigger (Embed Message)
        private async void MSG_btn_input_cover_CLICK(object sender, RoutedEventArgs e)
        {
            if (await Process.Picker.Embed(Data.File_Extensions.Png, "Image") == true) Init_CoverImage_NEW("MESSAGE"); else Init_MSG_CoverImage();
        }
        private async void MSG_btn_input_message_CLICK(object sender, RoutedEventArgs e)
        {
            if (MSG_btn_save_message.IsEnabled == true)
            {
                MSG_richeditbox_message.IsReadOnly = false;
                if (await Process.Picker.Embed(Data.File_Extensions.Txt, "Message") == true) Init_PickerSecret_NEW("MESSAGE"); else Init_MSG_Message();
            }
            else
            {
                await PopupDialog.Show(Status.Err, Detail.Insert_Message, Err.Replace_Message, Icon.Sad);
            }
        }
        private async void btn_Save_Message_Click(object sender, RoutedEventArgs e)
        {
            string text;
            MSG_richeditbox_message.Document.GetText(TextGetOptions.UseCrlf, out text);
            if (!(text.Length <= 0))
            {
                if (Process.Validate.Input(text) == false)
                {
                    await PopupDialog.Show(Status.Err, Detail.Insert_Message, Err.Input_Invalid_Message, Icon.Sad);
                }
                else
                {
                    await Process.Bifid_Cipher.Execute("Embed", text, "Message");
                    Init_Text_NEW("MESSAGE", String.Empty);
                }
            }
            else
            {
                await PopupDialog.Show(Status.Err, Detail.Insert_Message, Err.Input_Empty_Message, Icon.Sad);
            }
        }
        private async void btn_Clear_Message_Click(object sender, RoutedEventArgs e)
        {
            string result;
            MSG_richeditbox_message.Document.GetText(TextGetOptions.UseCrlf, out result);
            if (result.Length > 0)
            {
                Process.GetData.Picker.Clear();
                Process.GetData.Reset_Data("Embed", "File");
                Init_MSG_Message();
                await PopupDialog.Show(Status.Success, Detail.Insert_Message, Complete.Clear_Input_Message, Icon.Smile);
            }
        }
        private async void btn_Save_Password_msg_Click(object sender, RoutedEventArgs e)
        {
            if (MSG_textbox_passwd.Text != String.Empty)
            {
                if (Process.Validate.Input(MSG_textbox_passwd.Text) == false)
                {
                    await PopupDialog.Show(Status.Err, Detail.Insert_Password, Err.Input_Invalid_Passwd, Icon.Sad);
                }
                else
                {
                    await Process.Bifid_Cipher.Execute("Embed", MSG_textbox_passwd.Text, "Passwd");
                    Init_Text_NEW("PASSWD", "MESSAGE");
                }
            }
            else
            {
                await PopupDialog.Show(Status.Err, Detail.Insert_Password, Err.Input_Empty_Passwd, Icon.Sad);
            }
        }
        private async void btn_Clear_Password_msg_Click(object sender, RoutedEventArgs e)
        {
            if (MSG_textbox_passwd.Text != String.Empty)
            {
                Init_MSG_Passwd();
                Process.GetData.Reset_Data("Embed", "Passwd");
                await PopupDialog.Show(Status.Success, Detail.Insert_Password, Complete.Clear_Input_Passwd, Icon.Smile);
            }
        }
        private async void btn_Clear_FooterMenuEmbedMessage_Click(object sender, RoutedEventArgs e)
        {
            Init_Page();
            Process.GetData.Picker.Clear();
            Process.GetData.Reset_Data("Embed", "All");
            await PopupDialog.Show(Status.Success, Detail.Embed_Message, Complete.Clear_All, Icon.Smile);
        }
        private void btn_Exec_FooterMenuEmbedMessage_Click(object sender, RoutedEventArgs e)
        {
            Exec("Message");
        }
        #endregion

        //++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++//

        #region Function Controlling
        private async void Exec(string type)
        {
            switch (type)
            {
                case "File":
                    if (F_picker_status_cover.Text != "No Image" && F_btn_save_passwd.IsEnabled == false && F_picker_status_file.Text != "No Image")
                    {
                        if (await PopupDialog.ShowConfirm(Status.Confirm, Detail.Embed_File, Confirm.isExecute, Icon.Flat) == true) Process.Embed.Starting(type);
                    }
                    else
                    {
                        await PopupDialog.Show(Status.Err, Detail.Embed_File, Err.Input_isNull, Icon.Sad);
                    }
                    break;
                case "Message":
                    if (MSG_picker_status_cover.Text != "No Image" && MSG_btn_save_passwd.IsEnabled == false && MSG_btn_save_message.IsEnabled == false)
                    {
                        if (await PopupDialog.ShowConfirm(Status.Confirm, Detail.Embed_Message, Confirm.isExecute, Icon.Flat) == true) Process.Embed.Starting(type);
                    }
                    else
                    {
                        await PopupDialog.Show(Status.Err, Detail.Embed_Message, Err.Input_isNull, Icon.Sad);
                    }
                    break;
            }
        }
        #endregion

    }
}