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

// The Blank Page item template is documented at http://go.microsoft.com/fwlink/?LinkId=234238

namespace StegoPlusPlus.Views
{
    public sealed partial class Extract_Page : Page
    {
        public Extract_Page()
        {
            InitializeComponent();
            
            STEG_btn_input_stego.Click += new RoutedEventHandler(btn_StegImage_Click);
            CHK_btn_input_stego.Click += new RoutedEventHandler(btn_StegImage_check_healthy_Click);
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
            STEG_textbox_passwd.Text = String.Empty;
            STEG_textbox_passwd.IsReadOnly = false;
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
        private void btn_StegImage_Click(object sender, RoutedEventArgs e)
        {

        }

        //Trigger Function From btn_Clear_Password_Steg_Click (Extract File/Message -> Insert Password -> Clear)
        private void btn_Clear_Password_Steg_Click(object sender, RoutedEventArgs e)
        {
          //  if (Input_Password_file.Text != String.Empty)
          //  {
          //      Input_Password_file.IsReadOnly = false;
          //      Input_Password_file.Header = NotifyDataText.Clearing_Header_Notify_Extract_File_pwd;
          //      Input_Password_file.Text = String.Empty;
          //      btn_Save_Password_Steg.IsEnabled = true;
          //      dlg_extract_file = new ContentDialog()
          //      {
          //          Title = NotifyDataText.Clear_Input_Extract_File_pwd,
          //          PrimaryButtonText = NotifyDataText.OK_Button
          //      };
          //      show_dlg_extract_file = await dlg_extract_file.ShowAsync();
          //  }
        }

        //Trigger Function From btn_Save_Password_Steg_Click (Extract File/Message -> Insert Password -> Save)
        private void btn_Save_Password_Steg_Click(object sender, RoutedEventArgs e)
        {
        //    string notify = String.Empty;
        //    if (Input_Password_file.Text != String.Empty)
        //    {
        //        notify = dp.validatePasswdOrMessageInput(Input_Password_file.Text);
        //        if (notify == "Password Invalid")
        //        {
        //            dlg_extract_file = new ContentDialog()
        //            {
        //                Title = NotifyDataText.Notify_Input_Passwd_Invalid,
        //                PrimaryButtonText = NotifyDataText.OK_Button
        //            };
        //            show_dlg_extract_file = await dlg_extract_file.ShowAsync();
        //        }
        //        else
        //        {
        //            string enc = Input_Password_file.Text;
        //            Input_Password_file.IsReadOnly = true;
        //            Input_Password_file.Header = NotifyDataText.Saving_Header_Notify_Extract_File_pwd;
        //            Pwd_file_steg = dp.Encrypt_BifidCipher(Input_Password_file.Text); //Get Value as Public
        //            Input_Password_file.Text = dp.Encrypt_BifidCipher(enc);
        //            btn_Save_Password_Steg.IsEnabled = false;
        //        }
        //    }
        //    else
        //    {
        //        dlg_extract_file = new ContentDialog()
        //        {
        //            Title = NotifyDataText.Err_Input_Null_Extract_File_pwd,
        //            PrimaryButtonText = NotifyDataText.OK_Button
        //        };
        //        show_dlg_extract_file = await dlg_extract_file.ShowAsync();
        //    }
        }

        private async void Clear_FooterMenuExtractFile_Click(object sender, RoutedEventArgs e)
        {
        //    SetStatus_PickerSteg();
        //    SetStatus_Passwd();
            dlg_extract_file = new ContentDialog()
            {
                Title = NotifyDataText.Dialog_Clear_Footer_Menu_Null,
                PrimaryButtonText = NotifyDataText.OK_Button
            };
            show_dlg_extract_file = await dlg_extract_file.ShowAsync();
        }

        private void Exec_FooterMenuExtractFile_Click(object sender, RoutedEventArgs e)
        {
            ExecSteg();
        }

        //Function of Steg Message (From FooterMenu -> Exec)
        private async void ExecSteg()
        {
            if (STEG_picker_status_stego.Text != "No Image" && STEG_textbox_passwd.IsReadOnly == true)
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
        private void btn_StegImage_check_healthy_Click(object sender, RoutedEventArgs e)
        {

        }

        private async void Clear_FooterMenuCheckSteg_Click(object sender, RoutedEventArgs e)
        {
            dlg_extract_file_check = new ContentDialog()
            {
                Title = NotifyDataText.Dialog_Clear_Footer_Menu_Null,
                PrimaryButtonText = NotifyDataText.OK_Button
            };
            show_dlg_extract_file_check = await dlg_extract_file_check.ShowAsync();
        }

        private void Exec_FooterMenuCheckSteg_Click(object sender, RoutedEventArgs e)
        {
            ExecSteg_Check();
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

        //--------------------------------------------------------------------------------//
        //PAGE CONTROL FOR EXTRACT FILE (EXTRACT MENU -> EXTRACT CHECK HEALTH)
        //END


    }
}
