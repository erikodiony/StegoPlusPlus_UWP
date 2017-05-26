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
            this.InitializeComponent();

            InitializingPage();
            check_transition_effect_status();
            check_theme_status();

            SetStatus_PickerSteg();
            SetStatus_Passwd();

            SetStatus_PickerStegCheck();

            btn_StegImage.Click += new RoutedEventHandler(btn_StegImage_Click);
            btn_StegImage_check_healthy.Click += new RoutedEventHandler(btn_StegImage_check_healthy_Click);
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

        //Function to Check Effect Transition
        private void check_transition_effect_status()
        {
            if ((string)ApplicationData.Current.LocalSettings.Values["Effect_set"] == "Continuum")
            {
                var info = new ContinuumNavigationTransitionInfo();
                theme.DefaultNavigationTransitionInfo = info;
                collection.Add(theme);
                this.Transitions = collection;
            }

            else if ((string)ApplicationData.Current.LocalSettings.Values["Effect_set"] == "Common")
            {
                var info = new CommonNavigationTransitionInfo();
                theme.DefaultNavigationTransitionInfo = info;
                collection.Add(theme);
                this.Transitions = collection;
            }

            else if ((string)ApplicationData.Current.LocalSettings.Values["Effect_set"] == "Slide")
            {
                var info = new SlideNavigationTransitionInfo();
                theme.DefaultNavigationTransitionInfo = info;
                collection.Add(theme);
                this.Transitions = collection;
            }

            else
            {
                var info = new SuppressNavigationTransitionInfo();
                theme.DefaultNavigationTransitionInfo = info;
                collection.Add(theme);
                this.Transitions = collection;
            }

        }

        //Function Check Theme Status
        private void check_theme_status()
        {
            if ((string)ApplicationData.Current.LocalSettings.Values["BG_set"] == "Dark")
            {
                this.RequestedTheme = ElementTheme.Dark;
            }
            else
            {
                this.RequestedTheme = ElementTheme.Light;
            }
        }

        //Initial Text Header Page
        private void InitializingPage()
        {
            HeaderInfo.Text = HeaderPage.ExtractPage;
        }

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

        //Initial Default Text PickerStego (EXTRACT MENU -> EXTRACT FILE)
        private void SetStatus_PickerSteg()
        {
            status_picker_steg.Text = "No Image";
            pathfile_picker_steg.Text = "-";
            sizefile_picker_steg.Text = "-";
            dimensionfile_picker_steg.Text = "-";
            ico_picker_steg.Visibility = Visibility.Collapsed;
        }

        //Initial Default Text PickerFolderStego (EXTRACT MENU -> EXTRACT FILE)
        private void SetStatus_Passwd()
        {
            Input_Password_file.Text = String.Empty;
            Input_Password_file.Header = NotifyDataText.Clearing_Header_Notify_Extract_File_pwd;
            Input_Password_file.IsReadOnly = false;
        }

        //Trigger Function From btn_StegImage_Click (Extract File -> Insert File)
        private async void btn_StegImage_Click(object sender, RoutedEventArgs e)
        {
            //Set an Extensions File Cover
            FileOpenPicker picker_steg = new FileOpenPicker();
            foreach (string extension in FileExtensions.Stego)
            {
                picker_steg.FileTypeFilter.Add(extension);
            }

            //Set Get a Name Property Image File Cover
            foreach (string propImg in propImage.All)
            {
                propImgList.Add(propImg);
            }

            file_steg = await picker_steg.PickSingleFileAsync();

            //Check File Picker
            if (file_steg != null)
            {
                //Get Property File Picker Selected (await)
                IDictionary<string, object> extraProperties = await file_steg.Properties.RetrievePropertiesAsync(propImgList);
                var propSize = extraProperties[propImage.Size];
                var propDimension = extraProperties[propImage.Dimensions];

                //Show Thumbnail File Picker
                using (StorageItemThumbnail thumbnail = await file_steg.GetThumbnailAsync(ThumbnailMode.PicturesView))
                {
                    if (thumbnail != null)
                    {
                        ico_picker_steg.Visibility = Visibility.Visible;
                        BitmapImage bitmapImage = new BitmapImage();
                        bitmapImage.SetSource(thumbnail);
                        ico_picker_steg.Source = bitmapImage;

                        Binary_embed_file_steg = await dp.Convert_FileImage_to_Byte(file_steg);

                        status_picker_steg.Text = file_steg.Name;
                        pathfile_picker_steg.Text = file_steg.Path.Replace("\\" + file_steg.Name, String.Empty);
                        sizefile_picker_steg.Text = String.Format("{0} bytes", propSize);
                        dimensionfile_picker_steg.Text = String.Format("{0}", propDimension);

                    }
                    else
                    {
                        ico_picker_steg.Visibility = Visibility.Collapsed;
                    }
                }
            }

            else
            {
                SetStatus_PickerSteg();
            }
        }

        //Trigger Function From btn_Clear_Password_Steg_Click (Extract File/Message -> Insert Password -> Clear)
        private async void btn_Clear_Password_Steg_Click(object sender, RoutedEventArgs e)
        {
            if (Input_Password_file.Text != String.Empty)
            {
                Input_Password_file.IsReadOnly = false;
                Input_Password_file.Header = NotifyDataText.Clearing_Header_Notify_Extract_File_pwd;
                Input_Password_file.Text = String.Empty;
                dlg_extract_file = new ContentDialog()
                {
                    Title = NotifyDataText.Clear_Input_Extract_File_pwd,
                    PrimaryButtonText = NotifyDataText.OK_Button
                };
                show_dlg_extract_file = await dlg_extract_file.ShowAsync();
            }
        }

        //Trigger Function From btn_Save_Password_Steg_Click (Extract File/Message -> Insert Password -> Save)
        private async void btn_Save_Password_Steg_Click(object sender, RoutedEventArgs e)
        {
            string notify = String.Empty;
            if (Input_Password_file.Text != String.Empty)
            {
                notify = dp.validatePasswdOrMessageInput(Input_Password_file.Text);
                if (notify == "Password Invalid")
                {
                    dlg_extract_file = new ContentDialog()
                    {
                        Title = NotifyDataText.Notify_Input_Passwd_Invalid,
                        PrimaryButtonText = NotifyDataText.OK_Button
                    };
                    show_dlg_extract_file = await dlg_extract_file.ShowAsync();
                }
                else
                {
                    Input_Password_file.IsReadOnly = true;
                    Input_Password_file.Header = NotifyDataText.Saving_Header_Notify_Extract_File_pwd;
                    Pwd_file_steg = dp.Encrypt_BifidCipher(Input_Password_file.Text); //Get Value as Public
                }
            }
            else
            {
                dlg_extract_file = new ContentDialog()
                {
                    Title = NotifyDataText.Err_Input_Null_Extract_File_pwd,
                    PrimaryButtonText = NotifyDataText.OK_Button
                };
                show_dlg_extract_file = await dlg_extract_file.ShowAsync();
            }
        }

        private async void Clear_FooterMenuExtractFile_Click(object sender, RoutedEventArgs e)
        {
            SetStatus_PickerSteg();
            SetStatus_Passwd();
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
            if (status_picker_steg.Text != "No Image" && Input_Password_file.IsReadOnly == true)
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
                            progBar_extract.Visibility = Visibility.Visible;
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
                            progBar_extract.Visibility = Visibility.Collapsed;
                        }

                    }

                    if (NotifyStegResult == "Steg Message")
                    {
                        try
                        {
                            dlg_extract_file.Hide();
                            progBar_extract.Visibility = Visibility.Visible;
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
                            progBar_extract.Visibility = Visibility.Collapsed;
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
        private void SetStatus_PickerStegCheck()
        {
            status_picker_steg_check.Text = "No Image";
            pathfile_picker_steg_check.Text = "-";
            sizefile_picker_steg_check.Text = "-";
            dimensionfile_picker_steg_check.Text = "-";
            ico_picker_steg_check.Visibility = Visibility.Collapsed;
        }

        //Trigger Function From btn_StegImage_check_healthy_Click
        private async void btn_StegImage_check_healthy_Click(object sender, RoutedEventArgs e)
        {
            //Set an Extensions File Steg Check Healthy
            FileOpenPicker picker_steg_check = new FileOpenPicker();
            foreach (string extension in FileExtensions.Stego)
            {
                picker_steg_check.FileTypeFilter.Add(extension);
            }

            //Set Get a Name Property Image File Cover
            foreach (string propImg in propImage.All)
            {
                propImgList.Add(propImg);
            }

            file_steg_check = await picker_steg_check.PickSingleFileAsync();

            //Check File Picker
            if (file_steg_check != null)
            {
                //Get Property File Picker Selected (await)
                IDictionary<string, object> extraProperties = await file_steg_check.Properties.RetrievePropertiesAsync(propImgList);
                var propSize = extraProperties[propImage.Size];
                var propDimension = extraProperties[propImage.Dimensions];

                //Show Thumbnail File Picker
                using (StorageItemThumbnail thumbnail = await file_steg_check.GetThumbnailAsync(ThumbnailMode.PicturesView))
                {
                    if (thumbnail != null)
                    {
                        status_picker_steg_check.Text = file_steg_check.Name;
                        pathfile_picker_steg_check.Text = file_steg_check.Path.Replace("\\" + file_steg_check.Name, String.Empty);
                        sizefile_picker_steg_check.Text = String.Format("{0} bytes", propSize);
                        dimensionfile_picker_steg_check.Text = String.Format("{0}", propDimension);

                        ico_picker_steg_check.Visibility = Visibility.Visible;
                        BitmapImage bitmapImage = new BitmapImage();
                        bitmapImage.SetSource(thumbnail);
                        ico_picker_steg_check.Source = bitmapImage;
                        Binary_embed_file_steg_check = await dp.Convert_FileImage_to_Byte(file_steg_check);
                    }
                    else
                    {
                        ico_picker_steg_check.Visibility = Visibility.Collapsed;
                    }
                }
            }

            else
            {
                SetStatus_PickerStegCheck();
            }
        }

        private async void Clear_FooterMenuCheckSteg_Click(object sender, RoutedEventArgs e)
        {
            SetStatus_PickerStegCheck();
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
            if (status_picker_steg_check.Text != "No Image")
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
