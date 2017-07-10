using System;
using System.Collections.Generic;
using Windows.Foundation;
using Windows.Graphics.Imaging;
using Windows.Storage;
using Windows.Storage.FileProperties;
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
            F_btn_input_cover.Click += new RoutedEventHandler(btn_CoverImage_Click); //Fungsi Click Cover ke Sinkron dengan Fungsi File Picker
            F_btn_input_file.Click += new RoutedEventHandler(btn_HidingFile_Click); //Fungsi Click Hiding ke Sinkron dengan Fungsi Hiding Picker
            MSG_btn_input_cover.Click += new RoutedEventHandler(btn_CoverImage_2_Click); //Fungsi Click Cover2 ke Sinkron dengan Fungsi File Picker
            MSG_btn_input_message.Click += new RoutedEventHandler(btn_HidingText_Click);
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

        private void Init_F_CoverImage_NEW()
        {
            F_picker_status_cover.Text = Process.GetData.Picker[Data.Misc.Name].ToString();
            F_picker_path_cover.Text = Process.GetData.Picker[Data.Misc.Path].ToString();
            F_picker_size_cover.Text = String.Format("{0} bytes", Process.GetData.Picker[Data.Misc.Size].ToString());
            F_picker_dimension_cover.Text = String.Format("{0} / ({1} BitDepth | {2} Pixel)", Process.GetData.Picker[Data.Misc.Dimensions].ToString(), Process.GetData.Picker[Data.Misc.BitDepth].ToString(), Process.GetData.Picker[Data.Misc.Pixel].ToString());
            F_picker_eta_cover.Text = String.Format("± {0} bytes", Process.GetData.Picker[Data.Misc.Eta].ToString());
            F_picker_ico_cover.Source = (BitmapImage)Process.GetData.Picker[Data.Misc.Icon];
            F_picker_ico_cover.Visibility = Visibility.Visible;
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
            MSG_lbl_counter_message.Text = Data.Prop_Secret_Message.counter;
            MSG_btn_save_message.Label = Data.Prop_Button.Save;
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

        StorageFile file_cover;
        StorageFile file_hiding;


        //Trigger Function From btn_CoverImage_Click (Embed File -> Cover Image)
        private async void btn_CoverImage_Click(object sender, RoutedEventArgs e)
        {
            //Set an Extensions File Cover
            FileOpenPicker picker_cover = new FileOpenPicker();
            foreach (string extension in FileExtensions.Stego)
            {
                picker_cover.FileTypeFilter.Add(extension);
            }

            //Set Get a Name Property Image File Cover
            foreach (string propImg in propImage.All)
            {
                propImgList.Add(propImg);
            }

            file_cover = await picker_cover.PickSingleFileAsync();
            
            //Check File Picker
            if (file_cover != null)
            {
                //Get Property File Picker Selected (await)
                IDictionary<string, object> extraProperties = await file_cover.Properties.RetrievePropertiesAsync(propImgList);
                var propSize = extraProperties[propImage.Size];
                var propDimension = extraProperties[propImage.Dimensions];
                var propBitDepth = extraProperties[propImage.BitDepth];


                //Show Thumbnail File Picker
                using (StorageItemThumbnail thumbnail = await file_cover.GetThumbnailAsync(ThumbnailMode.PicturesView))
                {
                    if (thumbnail != null && propBitDepth.ToString() == "32")
                    {
                        F_picker_ico_cover.Visibility = Visibility.Visible;
                        BitmapImage bitmapImage = new BitmapImage();
                        bitmapImage.SetSource(thumbnail);
                        F_picker_ico_cover.Source = bitmapImage;

                        Binary_embed_file_cover = await dp.Convert_FileImage_to_Byte(file_cover);

                        F_picker_status_cover.Text = file_cover.Name;
                        F_picker_path_cover.Text = file_cover.Path.Replace("\\" + file_cover.Name, String.Empty);
                        F_picker_size_cover.Text = String.Format("{0} bytes", propSize);
                        F_picker_dimension_cover.Text = String.Format("{0} / ({1} BitDepth | {2} Pixel)", propDimension, propBitDepth, Binary_embed_file_cover.Length);
                        F_picker_eta_cover.Text = String.Format("± {0} bytes", (Binary_embed_file_cover.Length / 8));
                    }
                    else
                    {
                        dlg_embed_file = new ContentDialog()
                        {
                            Title = NotifyDataText.Err_Input_32bitDepth,
                            PrimaryButtonText = NotifyDataText.OK_Button,
                        };

                        show_dlg_embed_file = await dlg_embed_file.ShowAsync();
                    }
                }


            }

            else
            {
                Init_F_CoverImage();
            }
        }

        //Trigger Function From btn_HidingImage_Click (Embed File -> Hiding File)
        private async void btn_HidingFile_Click(object sender, RoutedEventArgs e)
        {
            //Set an Extensions File Hiding
            FileOpenPicker picker_hiding = new FileOpenPicker();
            foreach (string extension in FileExtensions.All)
            {
                picker_hiding.FileTypeFilter.Add(extension);
            }

            //Set Get a Name Property Image File Hiding
            foreach (string propImg in propImage.All)
            {
                propImgList.Add(propImg);
            }

            file_hiding = await picker_hiding.PickSingleFileAsync();

            //Check File Hiding
            if (file_hiding != null)
            {
                //Get Property File Picker Selected (await)
                IDictionary<string, object> extraProperties = await file_hiding.Properties.RetrievePropertiesAsync(propImgList);
                var propSize = extraProperties[propImage.Size];

                //Show Thumbnail File Picker
                using (StorageItemThumbnail thumbnail = await file_hiding.GetThumbnailAsync(ThumbnailMode.PicturesView))
                {
                    if (thumbnail != null && int.Parse(propSize.ToString()) < Binary_embed_file_cover.Length / 8)
                    {
                        F_picker_status_file.Text = file_hiding.Name;
                        F_picker_path_file.Text = file_hiding.Path.Replace("\\" + file_hiding.Name, String.Empty);
                        F_picker_size_file.Text = String.Format("{0} bytes", propSize);
                        F_picker_type_file.Text = file_hiding.DisplayType;

                        F_picker_ico_file.Visibility = Visibility.Visible;
                        BitmapImage bitmapImage = new BitmapImage();
                        bitmapImage.SetSource(thumbnail);
                        F_picker_ico_file.Source = bitmapImage;
                        Binary_ext_embed_file = dp.Convert_FileType(file_hiding.FileType.ToLower());
                        //Binary_embed_file_encoded = await dp.Convert_FileHiding_to_Byte(file_hiding);
                    }
                    else
                    {
                        dlg_embed_file = new ContentDialog()
                        {
                            Title = NotifyDataText.Err_FileHiding_Overload_Size,
                            PrimaryButtonText = NotifyDataText.OK_Button,
                        };

                        show_dlg_embed_file = await dlg_embed_file.ShowAsync();
                    }
                }
            }

            else
            {
                Init_F_File();
            }
        }

        //Trigger Function From btn_Save_Password_file_Click (Embed File -> Insert Password -> Save)
        private async void btn_Save_Password_file_Click(object sender, RoutedEventArgs e)
        {
            string notify = String.Empty;
            if (F_textbox_passwd.Text != String.Empty)
            {
                if (Process.Validate.Input(F_textbox_passwd.Text) == false)
                {
                    await PopupDialog.Show(Status.Err, Detail.Insert_Password, Err.Input_Invalid_Passwd, Icon.Sad);
                }
                else
                {
                    string enc = F_textbox_passwd.Text;
                    F_textbox_passwd.IsReadOnly = true;
                    F_textbox_passwd.Header = NotifyDataText.Saving_Header_Notify_Embed_File_pwd;
                    Binary_pwd_embed_file = dp.Convert_Passwd(F_textbox_passwd.Text);
                    Binary_pwd_embed_file_encoded = dp.Convert_Passwd_Encrypt(dp.Encrypt_BifidCipher(F_textbox_passwd.Text));
                    F_textbox_passwd.Text = dp.Encrypt_BifidCipher(enc);
                    F_btn_save_passwd.IsEnabled = false;
                }
            }
            else
            {
                //await PopupDialog.Show(Status.Err, Detail.Insert_Password, Err.Input_Empty_Passwd, Icon.Sad);               
                //if (await Process.Picker.Embed(Data.File_Extensions.Png, "Image") == true) Init_F_CoverImage_NEW(); else Init_F_CoverImage();
            }
        }       


        //Trigger Function From btn_Clear_Password_file_Click (Embed File -> Insert Password -> Clear)
        private async void btn_Clear_Password_file_Click(object sender, RoutedEventArgs e)
        {
            if (F_textbox_passwd.Text != String.Empty)
            {
                F_textbox_passwd.IsReadOnly = false;
                F_textbox_passwd.Header = NotifyDataText.Clearing_Header_Notify_Embed_File_pwd;
                F_textbox_passwd.Text = String.Empty;
                F_btn_save_passwd.IsEnabled = true;
                await PopupDialog.Show(Status.Success, Detail.Insert_Password, Complete.Clear_Input_Passwd, Icon.Smile);
            }
        }

        //Trigger Function From btn_Clear_FooterMenuEmbedFile_Click (Embed File -> Footer Menu -> Clear)
        private async void btn_Clear_FooterMenuEmbedFile_Click(object sender, RoutedEventArgs e)
        {
            Init_Page();
            await PopupDialog.Show(Status.Success, Detail.Embed_File, Complete.Clear_All, Icon.Smile);
        }

        //Trigger Function From btn_Exec_FooterMenuEmbedFile_Click (Embed File -> Footer Menu -> Exec)
        private void btn_Exec_FooterMenuEmbedFile_Click(object sender, RoutedEventArgs e)
        {
            ExecSteg_File();
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
        private async void btn_CoverImage_2_Click(object sender, RoutedEventArgs e)
        {
            //Set an Extensions File Cover
            FileOpenPicker picker_cover = new FileOpenPicker();
            foreach (string extension in FileExtensions.Stego)
            {
                picker_cover.FileTypeFilter.Add(extension);
            }

            //Set Get a Name Property Image File Cover
            foreach (string propImg in propImage.All)
            {
                propImgList.Add(propImg);
            }

            file_cover_2 = await picker_cover.PickSingleFileAsync();

            //Check File Picker
            if (file_cover_2 != null)
            {
                //Get Property File Picker Selected (await)
                IDictionary<string, object> extraProperties = await file_cover_2.Properties.RetrievePropertiesAsync(propImgList);
                var propSize = extraProperties[propImage.Size];
                var propDimension = extraProperties[propImage.Dimensions];
                var propBitDepth = extraProperties[propImage.BitDepth];

                //Show Thumbnail File Picker
                using (StorageItemThumbnail thumbnail = await file_cover_2.GetThumbnailAsync(ThumbnailMode.PicturesView))
                {
                    if (thumbnail != null && propBitDepth.ToString() == "32")
                    {
                        MSG_picker_ico_cover.Visibility = Visibility.Visible;
                        BitmapImage bitmapImage = new BitmapImage();
                        bitmapImage.SetSource(thumbnail);
                        MSG_picker_ico_cover.Source = bitmapImage;

                        Binary_embed_file_cover_2 = await dp.Convert_FileImage_to_Byte(file_cover_2);

                        MSG_picker_status_cover.Text = file_cover_2.Name;
                        MSG_picker_path_cover.Text = file_cover_2.Path.Replace("\\" + file_cover_2.Name, String.Empty);
                        MSG_picker_size_cover.Text = String.Format("{0} bytes", propSize);
                        MSG_picker_dimension_cover.Text = String.Format("{0} / ({1} BitDepth | {2} Pixel)", propDimension, propBitDepth, Binary_embed_file_cover_2.Length);
                        MSG_picker_eta_cover.Text = String.Format("± {0} Character", Binary_embed_file_cover_2.Length / 8);
                    }
                    else
                    {
                        dlg_embed_msg = new ContentDialog()
                        {
                            Title = NotifyDataText.Err_Input_32bitDepth,
                            PrimaryButtonText = NotifyDataText.OK_Button,
                        };
                        show_dlg_embed_msg = await dlg_embed_msg.ShowAsync();
                    }
                }
            }

            else
            {
                SetStatus_PickerCover_2();
            }
        }

        private async void btn_HidingText_Click(object sender, RoutedEventArgs e)
        {
            //Set an Extensions File Cover
            FileOpenPicker picker_cover = new FileOpenPicker();
            foreach (string extension in FileExtensions.SecretMessage)
            {
                picker_cover.FileTypeFilter.Add(extension);
            }

            //Set Get a Name Property Image File Cover
            foreach (string propText in propImage.All)
            {
                propMsgList.Add(propText);
            }

            file_hidingText = await picker_cover.PickSingleFileAsync();

            if (file_hidingText != null)
            {
                MSG_richeditbox_message.IsReadOnly = false;
                //Get Property File Picker Selected (await)
                IDictionary<string, object> extraProperties = await file_hidingText.Properties.RetrievePropertiesAsync(propMsgList);
                var propSize = extraProperties["System.Size"];

                MSG_picker_status_message.Text = file_hidingText.Name;
                MSG_picker_path_message.Text = file_hidingText.Path.Replace("\\" + file_hidingText.Name, String.Empty);
                MSG_picker_size_message.Text = String.Format("{0} bytes", propSize);
                MSG_picker_type_message.Text = file_hidingText.DisplayType;

                string txt_hidingText;
                var lines = await FileIO.ReadTextAsync(file_hidingText);
                MSG_richeditbox_message.Document.SetText(TextSetOptions.None, lines);
                MSG_richeditbox_message.Document.GetText(TextGetOptions.None, out txt_hidingText);
                MSG_picker_count_message.Text = (txt_hidingText.Length - 1).ToString();
                MSG_richeditbox_message.IsReadOnly = true;
            }
        }

        //Trigger Function From btn_Save_Message_Click (Embed Message -> Insert Text/Message -> Save)
        private async void btn_Save_Message_Click(object sender, RoutedEventArgs e)
        {
            MSG_richeditbox_message.IsReadOnly = false;
            string notify = String.Empty;
            string temp_msg = String.Empty;
            MSG_richeditbox_message.Document.GetText(TextGetOptions.None, out temp_msg);
            temp_msg = temp_msg.TrimEnd('\r');
            if (temp_msg.Length > 0)
            {
                notify = dp.validatePasswdOrMessageInput(temp_msg);
                if (notify == "Password Invalid")
                {
                    dlg_embed_msg = new ContentDialog()
                    {
                        Title = NotifyDataText.Notify_Input_Message_Invalid,
                        PrimaryButtonText = NotifyDataText.OK_Button
                    };
                    show_dlg_embed_msg = await dlg_embed_msg.ShowAsync();
                }
                else
                {

                    MSG_richeditbox_message.Header = NotifyDataText.Saving_Header_Notify_Embed_Msg_msg;
                    Binary_msg_embed_encoded = dp.Convert_Message_or_Text(dp.Encrypt_BifidCipher(temp_msg));
                    MSG_richeditbox_message.Document.SetText(TextSetOptions.None, dp.Encrypt_BifidCipher(temp_msg));
                    MSG_richeditbox_message.Header = dp.Encrypt_BifidCipher(temp_msg).Length;
                    MSG_richeditbox_message.IsReadOnly = true;
                    MSG_btn_save_message.IsEnabled = false;
                }
            }
            else
            {
                dlg_embed_msg = new ContentDialog()
                {
                    Title = NotifyDataText.Err_Input_Null_Embed_Msg_msg,
                    PrimaryButtonText = NotifyDataText.OK_Button
                };
                show_dlg_embed_msg = await dlg_embed_msg.ShowAsync();
            }
        }

        //Trigger Function From btn_Clear_Message_Click (Embed Message -> Insert Text/Message -> Clear)
        private async void btn_Clear_Message_Click(object sender, RoutedEventArgs e)
        {
            string temp_msg = String.Empty;
            MSG_richeditbox_message.Document.GetText(TextGetOptions.None, out temp_msg);
            temp_msg = temp_msg.TrimEnd('\r');
            if (temp_msg.Length > 0)
            {
                MSG_richeditbox_message.IsReadOnly = false;
                MSG_richeditbox_message.Header = NotifyDataText.Clearing_Header_Notify_Embed_Msg_msg;
                MSG_richeditbox_message.Document.SetText(TextSetOptions.None, dp.Decrypt_BifidCipher(temp_msg));
                //InputMessage.Document.SetText(TextSetOptions.None, String.Empty);
                MSG_btn_save_message.IsEnabled = true;
                dlg_embed_msg = new ContentDialog()
                {
                    Title = NotifyDataText.Clear_Input_Embed_Msg_msg,
                    PrimaryButtonText = DataText_prop.Button.OK
                };
                show_dlg_embed_msg = await dlg_embed_msg.ShowAsync();
            }
        }

        //Trigger Function From btn_Save_Password_msg_Click (Embed Message -> Insert Password -> Save)
        private async void btn_Save_Password_msg_Click(object sender, RoutedEventArgs e)
        {
            string notify = String.Empty;
            if (MSG_textbox_passwd.Text != String.Empty)
            {
                notify = dp.validatePasswdOrMessageInput(MSG_textbox_passwd.Text);
                if (notify == "Password Invalid")
                {
                    dlg_embed_msg = new ContentDialog()
                    {
                        Title = NotifyDataText.Notify_Input_Passwd_Invalid,
                        PrimaryButtonText = NotifyDataText.OK_Button
                    };
                    show_dlg_embed_msg = await dlg_embed_msg.ShowAsync();
                }
                else
                {
                    string enc = MSG_textbox_passwd.Text;
                    MSG_textbox_passwd.IsReadOnly = true;
                    MSG_textbox_passwd.Header = DataText_prop.Passwd.head_save;
                    Binary_pwd_embed_msg = dp.Convert_Passwd(MSG_textbox_passwd.Text); //Uncrypt
                    Binary_pwd_embed_msg_encoded = dp.Convert_Passwd_Encrypt(dp.Encrypt_BifidCipher(MSG_textbox_passwd.Text)); //Crypt with Bifid Cipher
                    MSG_textbox_passwd.Text = dp.Encrypt_BifidCipher(enc);
                    MSG_btn_save_passwd.IsEnabled = false;
                }
            }
            else
            {
                dlg_embed_msg = new ContentDialog()
                {
                    Title = NotifyDataText.Err_Input_Null_Embed_Msg_pwd,
                    PrimaryButtonText = NotifyDataText.OK_Button
                };
                show_dlg_embed_msg = await dlg_embed_msg.ShowAsync();
            }
        }

        //Trigger Function From btn_Clear_Password_msg_Click (Embed Message -> Insert Password -> Clear)
        private async void btn_Clear_Password_msg_Click(object sender, RoutedEventArgs e)
        {
            if (MSG_textbox_passwd.Text != String.Empty)
            {
                MSG_textbox_passwd.IsReadOnly = false;
                MSG_textbox_passwd.Header = NotifyDataText.Clearing_Header_Notify_Embed_Msg_pwd;
                MSG_textbox_passwd.Text = String.Empty;
                MSG_btn_save_passwd.IsEnabled = true;
                dlg_embed_msg = new ContentDialog()
                {
                    Title = NotifyDataText.Clear_Input_Embed_Msg_pwd,
                    PrimaryButtonText = NotifyDataText.OK_Button
                };
                show_dlg_embed_msg = await dlg_embed_msg.ShowAsync();
            }
        }

        //Trigger Function From btn_Clear_FooterMenuEmbedMessage_Click (Embed Message -> Footer Menu -> Clear)
        private async void btn_Clear_FooterMenuEmbedMessage_Click(object sender, RoutedEventArgs e)
        {
            Init_Page();
            SetStatus_PickerCover_2();
            dlg_embed_msg = new ContentDialog()
            {
                Title = NotifyDataText.Dialog_Clear_Footer_Menu_Null,
                PrimaryButtonText = NotifyDataText.OK_Button
            };
            show_dlg_embed_msg = await dlg_embed_msg.ShowAsync();
        }

        //Trigger Function From btn_Exec_FooterMenuEmbedMessage_Click (Embed Message -> Footer Menu -> Save)
        private void btn_Exec_FooterMenuEmbedMessage_Click(object sender, RoutedEventArgs e)
        {
            ExecSteg_Message();
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
            string count_msg = String.Empty;
            MSG_richeditbox_message.Document.GetText(TextGetOptions.None, out count_msg);
            count_msg = count_msg.TrimEnd('\r');
            MSG_picker_count_message.Text = count_msg.Length.ToString();
        }

        //PAGE CONTROL FOR EMBED MESSAGE (EMBED MENU -> EMBED MESSAGE)
        //END


        //--------------------------------------------------------------------------------//


    }
}