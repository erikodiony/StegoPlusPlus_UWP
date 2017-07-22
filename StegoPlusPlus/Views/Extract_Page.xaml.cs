using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.Graphics.Imaging;
using Windows.Storage;
using Windows.Storage.FileProperties;
using Windows.Storage.Pickers;
using Windows.Storage.Streams;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Media.Animation;
using Windows.UI.Xaml.Media.Imaging;
using Windows.UI.Xaml.Navigation;
using static StegoPlusPlus.Data.Prop_Popup;
using static StegoPlusPlus.Data.Prop_Popup.Title;

// The Blank Page item template is documented at http://go.microsoft.com/fwlink/?LinkId=234238

namespace StegoPlusPlus.Views
{
    public sealed partial class Extract_Page : Page
    {
        public Extract_Page()
        {
            InitializeComponent();
            
            STEG_btn_input_stego.Click += new RoutedEventHandler(STEG_btn_input_stego_Click);
            CHK_btn_input_stego.Click += new RoutedEventHandler(CHK_btn_input_stego_Click);
        }

        
        //++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++//


        //BEGIN
        //PAGE CONTROL FOR EXTRACT MENU
        //--------------------------------------------------------------------------------//

        DataProcess dp = new DataProcess(); //Initializing Object DataProcess.cs
        List<string> propImgList = new List<string>(); //Create List Specific Property For File Picker Stego
        TransitionCollection collection = new TransitionCollection(); //Initializing Effect Transition Page
        NavigationThemeTransition theme = new NavigationThemeTransition(); //Initializing Theme Color Page
        string typefile = String.Empty;


        private void Page_Loading(FrameworkElement sender, object args)
        {
            Init_Transition();
            Init_Theme();
            Init_Page();
        }

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
        #region Initializing Property
        private void Init_Page()
        {
            Init_STEG_StegoImage();
            Init_STEG_Passwd();
            Init_CHK_StegoImage();

            HeaderInfo.Text = HeaderPage.ExtractPage;
            STEG_btn_ClearAll_FooterMenu.Label = DataText_prop.Button.ClearAll;
            STEG_btn_Execute_FooterMenu.Label = DataText_prop.Button.Execute;
            CHK_btn_ClearAll_FooterMenu.Label = DataText_prop.Button.ClearAll;
            CHK_btn_Execute_FooterMenu.Label = DataText_prop.Button.Execute;
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


        //Initial Check FileType Known
        public void CheckingFileType()
        {
            foreach (var x in FileExtensions.Image)
            {
                if (System.Text.Encoding.ASCII.GetString(DataProcess.ext).Contains(x))
                {
                    typefile = "Image Files";
                }
            }

            foreach (var xx in FileExtensions.Document)
            {
                if (System.Text.Encoding.ASCII.GetString(DataProcess.ext).Contains(xx))
                {
                    typefile = "Document Files";
                }
            }

            foreach (var xxx in FileExtensions.Other)
            {
                if (System.Text.Encoding.ASCII.GetString(DataProcess.ext).Contains(xxx))
                {
                    typefile = "Other Files";
                }
            }
        }

        //Saving Image Steg
        public async void SaveStegoAsFile()
        {
            CheckingFileType();

            FileSavePicker fs = new FileSavePicker();
            fs.FileTypeChoices.Add(typefile, new List<string>() { System.Text.Encoding.ASCII.GetString(DataProcess.ext) });
            IStorageFile sf = await fs.PickSaveFileAsync();
            if (sf != null)
            {
                await FileIO.WriteBytesAsync(sf, DataProcess.data);
            }
        }

        //--------------------------------------------------------------------------------//
        //PAGE CONTROL FOR EXTRACT MENU
        //END

        
        //++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++//

        
        //BEGIN
        //PAGE CONTROL FOR EXTRACT FILE (EXTRACT MENU -> EXTRACT FILE)
        //--------------------------------------------------------------------------------//

        byte[] Binary_embed_file_steg; //FILE STEG (EXTRACT MENU -> EXTRACT FILE/MESSAGE)
        string Pwd_file_steg; //PASSWORD FILE STEG
        string NotifyStegResult; //Notify Result Extract File Steg
        
        ContentDialog dlg_extract_file;
        ContentDialogResult show_dlg_extract_file = new ContentDialogResult();

        StorageFile file_steg;

        //Trigger Function From btn_StegImage_Click (Extract File -> Insert File)
        private async void STEG_btn_input_stego_Click(object sender, RoutedEventArgs e)
        {
            if (await Process.Picker.Embed(Data.File_Extensions.Png, "Stego") == true) Init_StegoImage_NEW("STEG"); else Init_STEG_StegoImage();
        }

        //Trigger Function From btn_Clear_Password_Steg_Click (Extract File/Message -> Insert Password -> Clear)
        private async void STEG_btn_clear_passwd_Click(object sender, RoutedEventArgs e)
        {
            if (STEG_textbox_passwd.Password != String.Empty)
            {
                Init_STEG_Passwd();
                Process.GetData.Reset_Data("Extract", "Passwd");
                await PopupDialog.Show(Status.Success, Detail.Insert_Password, Complete.Clear_Input_Passwd, Icon.Smile);
            }
        }

        //Trigger Function From btn_Save_Password_Steg_Click (Extract File/Message -> Insert Password -> Save)
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
           //ExecSteg();
        }

        //Function of Steg Message (From FooterMenu -> Exec)
        private async void ExecSteg()
        {
            if (STEG_picker_status_stego.Text != "No Image" && STEG_textbox_passwd.Password != String.Empty)
            {
                dlg_extract_file = new ContentDialog()
                {
                    Title = NotifyDataText.Dialog_Exec_Footer_Menu_Confirm,
                    PrimaryButtonText = NotifyDataText.OK_Button,
                    SecondaryButtonText = NotifyDataText.Cancel_Button
                };

                show_dlg_extract_file = await dlg_extract_file.ShowAsync();

                if (show_dlg_extract_file == ContentDialogResult.Primary)
                {
                    NotifyStegResult = dp.RUN_UN_STEG(Binary_embed_file_steg, Pwd_file_steg);

                    if (NotifyStegResult == "Invalid File Steg")
                    {
                        dlg_extract_file = new ContentDialog()
                        {
                            Title = NotifyDataText.Notify_Extract_Menu_Invalid_File,
                            PrimaryButtonText = NotifyDataText.OK_Button
                        };
                        show_dlg_extract_file = await dlg_extract_file.ShowAsync();
                    }

                    if (NotifyStegResult == "Password Incorrect")
                    {
                        dlg_extract_file = new ContentDialog()
                        {
                            Title = NotifyDataText.Notify_Extract_Menu_Invalid_Passwd,
                            PrimaryButtonText = NotifyDataText.OK_Button
                        };
                        show_dlg_extract_file = await dlg_extract_file.ShowAsync();
                    }

                    if (NotifyStegResult == "Steg File")
                    {
                        try
                        {
                            dlg_extract_file.Hide();
                        }
                        finally
                        {
                            dlg_extract_file = new ContentDialog()
                            {
                                Title = NotifyDataText.Process_Complete_ExtractFile,
                                PrimaryButtonText = NotifyDataText.OK_Button,
                            };

                            show_dlg_extract_file = await dlg_extract_file.ShowAsync();
                            SaveStegoAsFile();
                        }

                    }

                    if (NotifyStegResult == "Steg Message")
                    {
                        try
                        {
                            dlg_extract_file.Hide();
                        }
                        finally
                        {
                            dlg_extract_file = new ContentDialog()
                            {
                                Title = NotifyDataText.Process_Complete_ExtractMessage,
                                PrimaryButtonText = NotifyDataText.OK_Button,
                            };

                            show_dlg_extract_file = await dlg_extract_file.ShowAsync();
                            CSecretMessage dlg = new CSecretMessage();
                            ContentDialogResult result = await dlg.ShowAsync();
                        }
                    }

                }
                else
                {
                    dlg_extract_file.Hide();
                }

            }

            else
            {
                dlg_extract_file = new ContentDialog()
                {
                    Title = NotifyDataText.Dialog_Exec_Footer_Menu_Null,
                    PrimaryButtonText = NotifyDataText.OK_Button
                };
                show_dlg_extract_file = await dlg_extract_file.ShowAsync();
            }
        }

        //--------------------------------------------------------------------------------//
        //PAGE CONTROL FOR EXTRACT FILE (EXTRACT MENU -> EXTRACT FILE)
        //BEGIN


        //++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++//


        //BEGIN
        //PAGE CONTROL FOR EXTRACT FILE (EXTRACT MENU -> EXTRACT CHECK HEALTH)
        //--------------------------------------------------------------------------------//

        byte[] Binary_embed_file_steg_check; //FILE STEG (EXTRACT MENU -> EXTRACT FILE/MESSAGE)
        string NotifyStegResultCheck;
        ContentDialog dlg_extract_file_check;
        ContentDialogResult show_dlg_extract_file_check = new ContentDialogResult();

        StorageFile file_steg_check;

        //Initial Default Text PickerStegoCheck (EXTRACT MENU -> EXTRACT CHECK HEALTH)

        //Trigger Function From btn_StegImage_check_healthy_Click
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
            //ExecSteg_Check();
        }

        //Function of Steg Message (From FooterMenu -> Exec)
        private async void ExecSteg_Check()
        {
            if (CHK_picker_status_stego.Text != "No Image")
            {
                dlg_extract_file_check = new ContentDialog()
                {
                    Title = NotifyDataText.Dialog_Exec_Footer_Menu_Confirm,
                    PrimaryButtonText = NotifyDataText.OK_Button,
                    SecondaryButtonText = NotifyDataText.Cancel_Button
                };

                show_dlg_extract_file_check = await dlg_extract_file_check.ShowAsync();

                if (show_dlg_extract_file_check == ContentDialogResult.Primary)
                {
                    NotifyStegResultCheck = dp.RUN_UN_STEG_CHECKINFO(Binary_embed_file_steg_check);

                    if (NotifyStegResultCheck == "Invalid File Steg")
                    {
                        dlg_extract_file_check = new ContentDialog()
                        {
                            Title = NotifyDataText.Notify_Extract_Menu_Invalid_File,
                            PrimaryButtonText = NotifyDataText.OK_Button
                        };
                        show_dlg_extract_file_check = await dlg_extract_file_check.ShowAsync();
                    }
                    else
                    {
                        CInfoStego dlg_info = new CInfoStego();
                        show_dlg_extract_file_check = await dlg_info.ShowAsync();
                    }
                }

                else
                {
                    dlg_extract_file_check.Hide();
                }
            }

            else
            {
                dlg_extract_file_check = new ContentDialog()
                {
                    Title = NotifyDataText.Dialog_Exec_Footer_Menu_Null,
                    PrimaryButtonText = NotifyDataText.OK_Button
                };
                show_dlg_extract_file_check = await dlg_extract_file_check.ShowAsync();
            }
        }


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

        //--------------------------------------------------------------------------------//
        //PAGE CONTROL FOR EXTRACT FILE (EXTRACT MENU -> EXTRACT CHECK HEALTH)
        //END


    }
}
