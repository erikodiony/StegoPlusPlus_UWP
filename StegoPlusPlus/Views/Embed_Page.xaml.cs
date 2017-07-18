using System;
using System.Collections.Generic;
using Windows.Foundation;
using Windows.Graphics.Imaging;
using Windows.Storage;
using Windows.Storage.Pickers;
using Windows.UI.Text;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media.Animation;
using Windows.UI.Xaml.Media.Imaging;
using Windows.Storage.Streams;

using static StegoPlusPlus.Data.Prop_Popup;
using static StegoPlusPlus.Data.Prop_Popup.Title;

namespace StegoPlusPlus.Views
{
    public sealed partial class Embed_Page : Page
    {
        public Embed_Page()
        {
            InitializeComponent();
            F_btn_input_cover.Click += new RoutedEventHandler(F_btn_input_cover_CLICK); //Fungsi Click Cover ke Sinkron dengan Fungsi File Picker
            F_btn_input_file.Click += new RoutedEventHandler(F_btn_input_file_CLICK); //Fungsi Click Hiding ke Sinkron dengan Fungsi Hiding Picker
            MSG_btn_input_cover.Click += new RoutedEventHandler(MSG_btn_input_cover_CLICK); //Fungsi Click Cover2 ke Sinkron dengan Fungsi File Picker
            MSG_btn_input_message.Click += new RoutedEventHandler(MSG_btn_input_message_CLICK);
        }


        //--------------------------------------------------------------------------------//


        //PAGE CONTROL FOR EMBED MENU
        //BEGIN

        private void Page_Loading(FrameworkElement sender, object args)
        {
            Init_Transition();
            Init_Theme();
            Init_Page();
        }

        private void Init_CoverImage_NEW(string type)
        {
            switch(type)
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

        private void Init_File_NEW(string type)
        {
            switch(type)
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
                    MSG_richeditbox_message.Document.SetText(TextSetOptions.None, Process.GetData.Picker[Data.Misc.Message].ToString());
                    MSG_richeditbox_message.IsReadOnly = true;
                    MSG_picker_count_message.Text = (Process.GetData.Picker[Data.Misc.Message].ToString()).Length.ToString();
                    break;
            }
        }

        private void Init_Passwd_NEW(string value, string type)
        {
            switch (type)
            {
                case "FILE":
                    F_textbox_passwd.Text = value;
                    F_textbox_passwd.Header = Data.Prop_Passwd.head_save;
                    F_textbox_passwd.IsReadOnly = true;
                    F_btn_save_passwd.IsEnabled = false;
                    break;
                case "MESSAGE":
                    MSG_textbox_passwd.Text = value;
                    MSG_textbox_passwd.Header = Data.Prop_Passwd.head_save;
                    MSG_textbox_passwd.IsReadOnly = true;
                    MSG_btn_save_passwd.IsEnabled = false;
                    break;
            }
        }

        private void Init_Message_NEW(string value)
        {
            MSG_richeditbox_message.IsReadOnly = false;
            MSG_richeditbox_message.Document.SetText(TextSetOptions.None, value);
            MSG_richeditbox_message.IsReadOnly = true;
            MSG_richeditbox_message.Header = Data.Prop_Secret_Message.head_save;
            MSG_btn_save_message.IsEnabled = false;
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

            HeaderInfo.Text = HeaderPage.EmbedPage;
            MSG_btn_ClearAll_FooterMenu.Label = DataText_prop.Button.ClearAll;
            MSG_btn_Execute_FooterMenu.Label = DataText_prop.Button.Execute;
            F_btn_ClearAll_FooterMenu.Label = DataText_prop.Button.ClearAll;
            F_btn_Execute_FooterMenu.Label = DataText_prop.Button.Execute;
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

        byte[] Binary_STEG_RESULT; //FILE STEG RGB

        DataProcess dp = new DataProcess(); //Initializing Object DataProcess.cs
        List<string> propImgList = new List<string>(); //Create List Specific Property For File Cover Picker
        List<string> propMsgList = new List<string>(); //Create List Specific Property For File Text Picker
        TransitionCollection collection = new TransitionCollection(); //Initializing Effect Transition Page
        NavigationThemeTransition theme = new NavigationThemeTransition(); //Initializing Theme Color Page

        //Saving Image Steg
        public async void SaveImageAsPNG()
        {
            
            FileSavePicker fs = new FileSavePicker();
            fs.FileTypeChoices.Add("PNG Image", new List<string>() { ".png" });
            StorageFile sf = await fs.PickSaveFileAsync();
            if (sf != null)
            {
                using (IRandomAccessStream strm_save = await sf.OpenAsync(FileAccessMode.ReadWrite))
                {
                    BitmapEncoder encoder = await BitmapEncoder.CreateAsync(BitmapEncoder.PngEncoderId, strm_save);
                    encoder.SetPixelData(dp.decoder.BitmapPixelFormat, dp.decoder.BitmapAlphaMode, (uint)dp.decoder.PixelWidth, (uint)dp.decoder.PixelHeight, dp.decoder.DpiX, dp.decoder.DpiY, Binary_STEG_RESULT);
                    var listProp = new List<KeyValuePair<string, BitmapTypedValue>>();

                    var desc = new BitmapTypedValue(String.Format("{0}|{1}|{2}|{3}|{4}", dp.length_pwd_crypt_encoded, dp.length_pwd_encoded, dp.length_def_encoded, dp.length_ext_encoded, dp.length_data_encoded), PropertyType.String);
                    listProp.Add(new KeyValuePair<string, BitmapTypedValue>("/tEXt/{str=Description}", desc));

                    await encoder.BitmapProperties.SetPropertiesAsync(listProp);
                    await encoder.FlushAsync().AsTask();
                }
            }
            else
            {

            }
        }

        //PAGE CONTROL FOR EMBED MENU
        //END


        //--------------------------------------------------------------------------------//


        //PAGE CONTROL FOR EMBED FILE (EMBED MENU -> EMBED FILE)
        //BEGIN

        byte[] Binary_embed_file_cover; //FILE COVER (EMBED MENU -> EMBED FILE)
        char[] Binary_embed_file_encoded; //FILE HIDING (EMBED MENU -> EMBED FILE)
        char[] Binary_pwd_embed_file; //PASSWORD ASLI (EMBED MENU -> EMBED FILE)
        char[] Binary_pwd_embed_file_encoded; //PASSWORD (EMBED MENU -> EMBED FILE)
        char[] Binary_ext_embed_file; //Extension (EMBED MENU -> EMBED FILE)
        char[] Binary_def = new char[] { (char)48, (char)48, (char)49, (char)49, (char)48, (char)48, (char)48, (char)49 }; //TextOrFile (EMBED MENU -> EMBED FILE)

        ContentDialog dlg_embed_file;
        ContentDialogResult show_dlg_embed_file = new ContentDialogResult();

        StorageFile file_hiding;


        //(Embed File -> Cover Image)
        private async void F_btn_input_cover_CLICK(object sender, RoutedEventArgs e)
        {
            if (await Process.Picker.Embed(Data.File_Extensions.Png, "Image") == true) Init_CoverImage_NEW("FILE"); else Init_F_CoverImage();
        }

        //(Embed File -> Hiding File)
        private async void F_btn_input_file_CLICK(object sender, RoutedEventArgs e)
        {
            if (await Process.Picker.Embed(Data.File_Extensions.All, "File") == true) Init_File_NEW("FILE"); else Init_F_File();
        }

        //Trigger Function From btn_Save_Password_file_Click (Embed File -> Insert Password -> Save)
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
                    Init_Passwd_NEW(await Process.Bifid_Cipher.Execute(F_textbox_passwd.Text, "Passwd"), "FILE");
                }
            }
            else
            {
                await PopupDialog.Show(Status.Err, Detail.Insert_Password, Err.Input_Empty_Passwd, Icon.Sad);
            }
        }       


        //Trigger Function From btn_Clear_Password_file_Click (Embed File -> Insert Password -> Clear)
        private async void btn_Clear_Password_file_Click(object sender, RoutedEventArgs e)
        {
            if (F_textbox_passwd.Text != String.Empty)
            {
                Init_F_Passwd();
                Process.GetData.Reset_Data("Passwd");
                await PopupDialog.Show(Status.Success, Detail.Insert_Password, Complete.Clear_Input_Passwd, Icon.Smile);
            }
        }

        //Trigger Function From btn_Clear_FooterMenuEmbedFile_Click (Embed File -> Footer Menu -> Clear)
        private async void btn_Clear_FooterMenuEmbedFile_Click(object sender, RoutedEventArgs e)
        {
            Init_Page();
            Process.GetData.Picker.Clear();
            Process.GetData.Reset_Data("All");
            await PopupDialog.Show(Status.Success, Detail.Embed_File, Complete.Clear_All, Icon.Smile);
        }

        //Trigger Function From btn_Exec_FooterMenuEmbedFile_Click (Embed File -> Footer Menu -> Exec)
        private void btn_Exec_FooterMenuEmbedFile_Click(object sender, RoutedEventArgs e)
        {
            Exec("File");
            //ExecSteg_File();
        }

        //Function of Steg File (From FooterMenu -> Exec)
        private async void ExecSteg_File()
        {
            if (F_picker_status_cover.Text != "No Image" && F_textbox_passwd.IsReadOnly == true)
            {
                var dlg = await PopupDialog.ShowConfirm(Status.Confirm, Detail.Embed_File, Confirm.isExecute, Icon.Flat);
                if (dlg == true)
                {
                    System.Diagnostics.Debug.WriteLine("PrimaryClicked");
                }
                else
                {
                    System.Diagnostics.Debug.WriteLine("SecondaryClicked");
                }

                show_dlg_embed_file = await dlg_embed_file.ShowAsync();

                if (show_dlg_embed_file == ContentDialogResult.Primary)
                {
                    try
                    {
                        dlg_embed_file.Hide();
                        F_progBar.Visibility = Visibility.Visible;
                        Binary_embed_file_encoded = await dp.Convert_FileHiding_to_Byte(file_hiding);
                        Binary_STEG_RESULT = dp.RUN_STEG(Binary_embed_file_encoded, Binary_embed_file_cover, Binary_pwd_embed_file, Binary_pwd_embed_file_encoded, Binary_ext_embed_file, Binary_def);
                    }
                    finally
                    {
                        dlg_embed_file = new ContentDialog()
                        {
                            Title = NotifyDataText.Process_Complete_EmbedFile_File,
                            PrimaryButtonText = NotifyDataText.OK_Button,
                        };

                        show_dlg_embed_file = await dlg_embed_file.ShowAsync();
                        SaveImageAsPNG();
                        F_progBar.Visibility = Visibility.Collapsed;
                    }
                }
                else
                {
                    dlg_embed_file.Hide();
                }
            }

            else
            {

                dlg_embed_file = new ContentDialog()
                {
                    Title = NotifyDataText.Dialog_Exec_Footer_Menu_Null,
                    PrimaryButtonText = NotifyDataText.OK_Button
                };
                show_dlg_embed_file = await dlg_embed_file.ShowAsync();
            }
        }

        //PAGE CONTROL FOR EMBED FILE (EMBED MENU -> EMBED FILE)
        //END


        //--------------------------------------------------------------------------------//


        //PAGE CONTROL FOR EMBED MESSAGE (EMBED MENU -> EMBED MESSAGE)
        //BEGIN

        byte[] Binary_embed_file_cover_2; //File Cover 2
        char[] Binary_msg_embed_encoded; //Text/Message to Hide
        char[] Binary_pwd_embed_msg; //Input Passwd (Un-crypt)
        char[] Binary_pwd_embed_msg_encoded; //Input Password (Crypt)

        char[] Binary_ext_embed_msg = new char[] { (char)48, (char)48, (char)49, (char)49, (char)48, (char)48, (char)48, (char)48 };
        char[] Binary_def_msg = new char[] { (char)48, (char)48, (char)49, (char)49, (char)48, (char)48, (char)48, (char)48 };

        //byte[] Binary_STEG_RESULT_msg; //FILE STEG RGB

        ContentDialog dlg_embed_msg;
        ContentDialogResult show_dlg_embed_msg = new ContentDialogResult();

        StorageFile file_cover_2;
        StorageFile file_hidingText;

        //Initial Default Text PickerCover Message
        private void SetStatus_PickerCover_2()
        {
            MSG_picker_ico_cover.Visibility = Visibility.Collapsed;
        }

        //Trigger Function From btn_CoverImage_2_Click (Embed Message -> Choose Image)
        private async void MSG_btn_input_cover_CLICK(object sender, RoutedEventArgs e)
        {
            if (await Process.Picker.Embed(Data.File_Extensions.Png, "Image") == true) Init_CoverImage_NEW("MESSAGE"); else Init_MSG_CoverImage();
        }

        private async void MSG_btn_input_message_CLICK(object sender, RoutedEventArgs e)
        {
            if (MSG_btn_save_message.IsEnabled == true)
            {
                MSG_richeditbox_message.IsReadOnly = false;
                if (await Process.Picker.Embed(Data.File_Extensions.Txt, "Message") == true) Init_File_NEW("MESSAGE"); else Init_MSG_Message();
            }
            else
            {
                await PopupDialog.Show(Status.Err, Detail.Insert_Message, Err.Replace_Message, Icon.Sad);
            }
        }

        //Trigger Function From btn_Save_Message_Click (Embed Message -> Insert Text/Message -> Save)
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
                    Init_Message_NEW(await Process.Bifid_Cipher.Execute(text, "Message"));                    
                }
            }
            else
            {
                await PopupDialog.Show(Status.Err, Detail.Insert_Message, Err.Input_Empty_Message, Icon.Sad);
            }
        }

        //Trigger Function From btn_Clear_Message_Click (Embed Message -> Insert Text/Message -> Clear)
        private async void btn_Clear_Message_Click(object sender, RoutedEventArgs e)
        {
            string result;
            MSG_richeditbox_message.Document.GetText(TextGetOptions.UseCrlf, out result);
            if (result.Length > 0)
            {
                Process.GetData.Picker.Clear();
                Process.GetData.Reset_Data("File");
                Init_MSG_Message();
                await PopupDialog.Show(Status.Success, Detail.Insert_Message, Complete.Clear_Input_Message, Icon.Smile);
            }
        }

        //Trigger Function From btn_Save_Password_msg_Click (Embed Message -> Insert Password -> Save)
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
                    Init_Passwd_NEW(await Process.Bifid_Cipher.Execute(MSG_textbox_passwd.Text, "Passwd"), "MESSAGE");
                }
            }
            else
            {
                await PopupDialog.Show(Status.Err, Detail.Insert_Password, Err.Input_Empty_Passwd, Icon.Sad);
            }
        }

        //Trigger Function From btn_Clear_Password_msg_Click (Embed Message -> Insert Password -> Clear)
        private async void btn_Clear_Password_msg_Click(object sender, RoutedEventArgs e)
        {
            if (MSG_textbox_passwd.Text != String.Empty)
            {
                Init_MSG_Passwd();
                Process.GetData.Reset_Data("Passwd");
                await PopupDialog.Show(Status.Success, Detail.Insert_Password, Complete.Clear_Input_Passwd, Icon.Smile);
            }
        }

        //Trigger Function From btn_Clear_FooterMenuEmbedMessage_Click (Embed Message -> Footer Menu -> Clear)
        private async void btn_Clear_FooterMenuEmbedMessage_Click(object sender, RoutedEventArgs e)
        {
            Init_Page();
            Process.GetData.Picker.Clear();
            Process.GetData.Reset_Data("All");
            await PopupDialog.Show(Status.Success, Detail.Embed_Message, Complete.Clear_All, Icon.Smile);
        }

        //Trigger Function From btn_Exec_FooterMenuEmbedMessage_Click (Embed Message -> Footer Menu -> Save)
        private void btn_Exec_FooterMenuEmbedMessage_Click(object sender, RoutedEventArgs e)
        {
            //ExecSteg_Message();
            Exec("Message");
        }

        private async void Exec(string type)
        {
            switch (type)
            {
                case "File":
                    if (F_picker_status_cover.Text != "No Image" && F_btn_save_passwd.IsEnabled == false && F_picker_status_file.Text != "No Image")
                    {
                        if (await PopupDialog.ShowConfirm(Status.Confirm, Detail.Embed_File, Confirm.isExecute, Icon.Flat) == true) Process.Validate.Execute(type);
                    }
                    else
                    {
                        await PopupDialog.Show(Status.Err, Detail.Embed_File, Err.Input_isNull, Icon.Sad);
                    }
                    break;
                case "Message":
                    if (MSG_picker_status_cover.Text != "No Image" && MSG_btn_save_passwd.IsEnabled == false && MSG_btn_save_message.IsEnabled == false)
                    {
                        if (await PopupDialog.ShowConfirm(Status.Confirm, Detail.Embed_Message, Confirm.isExecute, Icon.Flat) == true) Process.Validate.Execute(type);
                    }
                    else
                    {
                        await PopupDialog.Show(Status.Err, Detail.Embed_Message, Err.Input_isNull, Icon.Sad);
                    }
                    break;
            }
        }

        //Function of Steg Message (From FooterMenu -> Exec)
        private async void ExecSteg_Message()
        {
            if (MSG_picker_status_cover.Text != "No Image" && MSG_textbox_passwd.IsReadOnly == true && MSG_richeditbox_message.IsReadOnly == true)
            {
                dlg_embed_msg = new ContentDialog()
                {
                    Title = NotifyDataText.Dialog_Exec_Footer_Menu_Confirm,
                    PrimaryButtonText = NotifyDataText.OK_Button,
                    SecondaryButtonText = NotifyDataText.Cancel_Button
                };

                show_dlg_embed_msg = await dlg_embed_msg.ShowAsync();

                if (show_dlg_embed_msg == ContentDialogResult.Primary)
                {
                    try
                    {
                        dlg_embed_msg.Hide();
                        MSG_progBar.Visibility = Visibility.Visible;
                        Binary_STEG_RESULT = dp.RUN_STEG(Binary_msg_embed_encoded, Binary_embed_file_cover_2, Binary_pwd_embed_msg, Binary_pwd_embed_msg_encoded, Binary_ext_embed_msg, Binary_def_msg);
                    }
                    finally
                    {
                        dlg_embed_msg = new ContentDialog()
                        {
                            Title = NotifyDataText.Process_Complete_EmbedFile_Message,
                            PrimaryButtonText = NotifyDataText.OK_Button,
                        };

                        show_dlg_embed_msg = await dlg_embed_msg.ShowAsync();
                        SaveImageAsPNG();
                        MSG_progBar.Visibility = Visibility.Collapsed;
                    }
                }
                else
                {
                    dlg_embed_msg.Hide();
                }
            }

            else
            {
                dlg_embed_msg = new ContentDialog()
                {
                    Title = NotifyDataText.Dialog_Exec_Footer_Menu_Null,
                    PrimaryButtonText = NotifyDataText.OK_Button
                };
                show_dlg_embed_msg = await dlg_embed_msg.ShowAsync();
            }
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

        //PAGE CONTROL FOR EMBED MESSAGE (EMBED MENU -> EMBED MESSAGE)
        //END


        //--------------------------------------------------------------------------------//


    }
}