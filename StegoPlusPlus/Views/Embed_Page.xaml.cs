using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using System.Text;
using Windows.Graphics.Imaging;
using Windows.Storage;
using Windows.Storage.FileProperties;
using Windows.Storage.Pickers;
using Windows.UI;
using Windows.UI.Popups;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Media.Animation;
using Windows.UI.Xaml.Media.Imaging;
using Windows.UI.Xaml.Navigation;
using System.Threading.Tasks;
using Windows.Storage.Streams;

namespace StegoPlusPlus.Views
{
    public sealed partial class Embed_Page : Page
    {
        public Embed_Page()
        {
            this.InitializeComponent();

            InitializingPage();
            check_transition_effect_status();
            check_theme_status();

            SetStatus_HidingFile(); //(Embed Menu -> Embed File -> Hiding File)
            SetStatus_PickerCover(); //(Embed Menu -> Embed File -> Insert Image File)
            SetStatus_PickerCover_2(); //(Embed Menu -> Embed Text/Message -> Insert Image File)
            SetStatus_Password(); //(Embed Menu -> Embed File -> Insert Passwd)
            SetStatus_Password_2(); //(Embed Menu -> Embed Text/Message ->Insert Passwd)

            Input_Password_file.Text = String.Empty; 
            Input_Password_msg.Text = String.Empty;

            btn_CoverImage.Click += new RoutedEventHandler(btn_CoverImage_Click); //Fungsi Click Cover ke Sinkron dengan Fungsi File Picker
            btn_CoverImage_2.Click += new RoutedEventHandler(btn_CoverImage_2_Click); //Fungsi Click Cover2 ke Sinkron dengan Fungsi File Picker
            btn_HidingFile.Click += new RoutedEventHandler(btn_HidingFile_Click); //Fungsi Click Hiding ke Sinkron dengan Fungsi Hiding Picker
        }


        //--------------------------------------------------------------------------------//


        //PAGE CONTROL FOR EMBED MENU
        //BEGIN

        byte[] Binary_STEG_RESULT; //FILE STEG RGB

        DataProcess dp = new DataProcess(); //Initializing Object DataProcess.cs
        List<string> propImgList = new List<string>(); //Create List Specific Property For File Cover Picker
        TransitionCollection collection = new TransitionCollection(); //Initializing Effect Transition Page
        NavigationThemeTransition theme = new NavigationThemeTransition(); //Initializing Theme Color Page

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

        //Function to Check Theme Status
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
            HeaderInfo.Text = HeaderPage.EmbedPage;
        }

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
                    var lis = new List<KeyValuePair<string, BitmapTypedValue>>();

                    var desc = new BitmapTypedValue(String.Format("{0}|{1}|{2}|{3}|{4}", dp.length_pwd_crypt_encoded, dp.length_pwd_encoded, dp.length_def_encoded, dp.length_ext_encoded, dp.length_data_encoded), PropertyType.String);
                    lis.Add(new KeyValuePair<string, BitmapTypedValue>("/tEXt/{str=Description}", desc));

                    await encoder.BitmapProperties.SetPropertiesAsync(lis);
                    await encoder.FlushAsync();
                }
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

        //Initial Default Text PickerCover File (EMBED MENU -> EMBED FILE)
        private void SetStatus_PickerCover()
        {
            status_picker_cover.Text = "No Image";
            pathfile_picker_cover.Text = "-";
            sizefile_picker_cover.Text = "-";
            dimensionfile_picker_cover.Text = "-";
            estimatefile_picker_cover.Text = "-";
            ico_picker_cover.Visibility = Visibility.Collapsed;
        }

        //Initial Default Text Hiding File (EMBED MENU -> EMBED FILE)
        private void SetStatus_HidingFile()
        {
            status_picker_hiding.Text = "No File";
            pathfile_picker_hiding.Text = "-";
            sizefile_picker_hiding.Text = "-";
            typefile_picker_hiding.Text = "-";
            ico_picker_hiding.Visibility = Visibility.Collapsed;
        }

        //Initial Default Text Password (EMBED MENU -> EMBED FILE)
        private void SetStatus_Password()
        {
            Input_Password_file.Text = String.Empty;
            Input_Password_file.Header = NotifyDataText.Clearing_Header_Notify_Embed_File_pwd;
            Input_Password_file.IsReadOnly = false;
        }

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

                //Show Thumbnail File Picker
                using (StorageItemThumbnail thumbnail = await file_cover.GetThumbnailAsync(ThumbnailMode.PicturesView))
                {
                    if (thumbnail != null)
                    {
                       
                        ico_picker_cover.Visibility = Visibility.Visible;
                        BitmapImage bitmapImage = new BitmapImage();
                        bitmapImage.SetSource(thumbnail);
                        ico_picker_cover.Source = bitmapImage;

                        Binary_embed_file_cover = await dp.Convert_FileImage_to_Byte(file_cover);

                        status_picker_cover.Text = file_cover.Name;
                        pathfile_picker_cover.Text = file_cover.Path.Replace("\\" + file_cover.Name, String.Empty);
                        sizefile_picker_cover.Text = String.Format("{0} bytes", propSize);
                        dimensionfile_picker_cover.Text = String.Format("{0} / ({1} Pixel)", propDimension, Binary_embed_file_cover.Length);
                        estimatefile_picker_cover.Text = String.Format("{0} bytes", Binary_embed_file_cover.Length / 8);
                    }
                    else
                    {
                        ico_picker_cover.Visibility = Visibility.Collapsed;
                    }
                }


            }

            else
            {
                SetStatus_PickerCover();
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

                status_picker_hiding.Text = file_hiding.Name;
                pathfile_picker_hiding.Text = file_hiding.Path.Replace("\\" + file_hiding.Name, String.Empty);
                sizefile_picker_hiding.Text = String.Format("{0} bytes", propSize);
                typefile_picker_hiding.Text = file_hiding.DisplayType;

                //Show Thumbnail File Picker
                using (StorageItemThumbnail thumbnail = await file_hiding.GetThumbnailAsync(ThumbnailMode.PicturesView))
                {
                    if (thumbnail != null)
                    {
                        ico_picker_hiding.Visibility = Visibility.Visible;
                        BitmapImage bitmapImage = new BitmapImage();
                        bitmapImage.SetSource(thumbnail);
                        ico_picker_hiding.Source = bitmapImage;
                        Binary_ext_embed_file = dp.Convert_FileType(file_hiding.FileType);
                        //Binary_embed_file_encoded = await dp.Convert_FileHiding_to_Byte(file_hiding);
                    }
                    else
                    {
                        ico_picker_hiding.Visibility = Visibility.Collapsed;
                    }
                }
            }

            else
            {
                SetStatus_HidingFile();
            }
        }
        
        //Trigger Function From btn_Save_Password_file_Click (Embed File -> Insert Password -> Save)
        private async void btn_Save_Password_file_Click(object sender, RoutedEventArgs e)
        {
            string notify = String.Empty;
            if (Input_Password_file.Text != String.Empty)
            {
                notify = dp.validatePasswdOrMessageInput(Input_Password_file.Text);
                if (notify == "Password Invalid")
                {
                    dlg_embed_file = new ContentDialog()
                    {
                        Title = NotifyDataText.Notify_Input_Passwd_Invalid,
                        PrimaryButtonText = NotifyDataText.OK_Button
                    };
                    show_dlg_embed_file = await dlg_embed_file.ShowAsync();
                }
                else
                {
                    Input_Password_file.IsReadOnly = true;
                    Input_Password_file.Header = NotifyDataText.Saving_Header_Notify_Embed_File_pwd;
                    Binary_pwd_embed_file = dp.Convert_Passwd(Input_Password_file.Text);
                    Binary_pwd_embed_file_encoded = dp.Convert_Passwd_Encrypt(dp.Encrypt_BifidCipher(Input_Password_file.Text));
                }
            }
            else
            {
                dlg_embed_file = new ContentDialog()
                {
                    Title = NotifyDataText.Err_Input_Null_Embed_File_pwd,
                    PrimaryButtonText = NotifyDataText.OK_Button
                };
                show_dlg_embed_file = await dlg_embed_file.ShowAsync();
            }
        }

        //Trigger Function From btn_Clear_Password_file_Click (Embed File -> Insert Password -> Clear)
        private async void btn_Clear_Password_file_Click(object sender, RoutedEventArgs e)
        {
            if (Input_Password_file.Text != String.Empty)
            {
                Input_Password_file.IsReadOnly = false;
                Input_Password_file.Header = NotifyDataText.Clearing_Header_Notify_Embed_File_pwd;
                Input_Password_file.Text = String.Empty;
                dlg_embed_file = new ContentDialog()
                {
                    Title = NotifyDataText.Clear_Input_Embed_File_pwd,
                    PrimaryButtonText = NotifyDataText.OK_Button
                };
                show_dlg_embed_file = await dlg_embed_file.ShowAsync();
            }
        }

        //Trigger Function From btn_Clear_FooterMenuEmbedFile_Click (Embed File -> Footer Menu -> Clear)
        private async void btn_Clear_FooterMenuEmbedFile_Click(object sender, RoutedEventArgs e)
        {
            SetStatus_PickerCover();
            SetStatus_HidingFile();
            SetStatus_Password();
            dlg_embed_file = new ContentDialog()
            {
                Title = NotifyDataText.Dialog_Clear_Footer_Menu_Null,
                PrimaryButtonText = NotifyDataText.OK_Button
            };
            show_dlg_embed_file = await dlg_embed_file.ShowAsync();
        }

        //Trigger Function From btn_Exec_FooterMenuEmbedFile_Click (Embed File -> Footer Menu -> Exec)
        private void btn_Exec_FooterMenuEmbedFile_Click(object sender, RoutedEventArgs e)
        {
            ExecSteg_File();
        }

        //Function of Steg File (From FooterMenu -> Exec)
        private async void ExecSteg_File()
        {
            if (status_picker_cover.Text != "No Image" && Input_Password_file.IsReadOnly == true)
            {
                dlg_embed_file = new ContentDialog()
                {
                    Title = NotifyDataText.Dialog_Exec_Footer_Menu_Confirm,
                    PrimaryButtonText = NotifyDataText.OK_Button,
                    SecondaryButtonText = NotifyDataText.Cancel_Button
                };

                show_dlg_embed_file = await dlg_embed_file.ShowAsync();

                if (show_dlg_embed_file == ContentDialogResult.Primary)
                {
                    try
                    {
                        dlg_embed_file.Hide();
                        progBar_file.Visibility = Visibility.Visible;
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
                        progBar_file.Visibility = Visibility.Collapsed;
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

        //Initial Default Text PickerCover Message
        private void SetStatus_PickerCover_2()
        {
            status_picker_cover_2.Text = "No Image";
            pathfile_picker_cover_2.Text = "-";
            sizefile_picker_cover_2.Text = "-";
            dimensionfile_picker_cover_2.Text = "-";
            charfile_picker_cover_2.Text = "-";
            ico_picker_cover_2.Visibility = Visibility.Collapsed;
        }

        private void SetStatus_Password_2()
        {
            Input_Password_msg.Text = String.Empty;
            Input_Password_msg.Header = NotifyDataText.Clearing_Header_Notify_Embed_Msg_pwd;
            Input_Password_msg.IsReadOnly = false;

            InputMessage.Text = String.Empty;
            InputMessage.Header = NotifyDataText.Clearing_Header_Notify_Embed_Msg_msg;
            InputMessage.IsReadOnly = false;
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

                //Show Thumbnail File Picker
                using (StorageItemThumbnail thumbnail = await file_cover_2.GetThumbnailAsync(ThumbnailMode.PicturesView))
                {
                    if (thumbnail != null)
                    {
                        ico_picker_cover_2.Visibility = Visibility.Visible;
                        BitmapImage bitmapImage = new BitmapImage();
                        bitmapImage.SetSource(thumbnail);
                        ico_picker_cover_2.Source = bitmapImage;

                        Binary_embed_file_cover_2 = await dp.Convert_FileImage_to_Byte(file_cover_2);

                        status_picker_cover_2.Text = file_cover_2.Name;
                        pathfile_picker_cover_2.Text = file_cover_2.Path.Replace("\\" + file_cover_2.Name, String.Empty);
                        sizefile_picker_cover_2.Text = String.Format("{0} bytes", propSize);
                        dimensionfile_picker_cover_2.Text = String.Format("{0} / ({1} Pixel)", propDimension, Binary_embed_file_cover_2.Length);
                        charfile_picker_cover_2.Text = String.Format("{0} Character", Binary_embed_file_cover_2.Length / 8);
                    }
                    else
                    {
                        ico_picker_cover_2.Visibility = Visibility.Collapsed;
                    }
                }
            }

            else
            {
                SetStatus_PickerCover_2();
            }
        }

        //Trigger Function From btn_Save_Message_Click (Embed Message -> Insert Text/Message -> Save)
        private async void btn_Save_Message_Click(object sender, RoutedEventArgs e)
        {
            string notify = String.Empty;
            if (InputMessage.Text != String.Empty)
            {
                notify = dp.validatePasswdOrMessageInput(InputMessage.Text);
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
                    InputMessage.IsReadOnly = true;
                    InputMessage.Header = NotifyDataText.Saving_Header_Notify_Embed_Msg_msg;
                    Binary_msg_embed_encoded = dp.Convert_Message_or_Text(dp.Encrypt_BifidCipher(InputMessage.Text));
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
            if (InputMessage.Text != String.Empty)
            {
                InputMessage.IsReadOnly = false;
                InputMessage.Header = NotifyDataText.Clearing_Header_Notify_Embed_Msg_msg;
                InputMessage.Text = String.Empty;
                dlg_embed_msg = new ContentDialog()
                {
                    Title = NotifyDataText.Clear_Input_Embed_Msg_msg,
                    PrimaryButtonText = NotifyDataText.OK_Button
                };
                show_dlg_embed_msg = await dlg_embed_msg.ShowAsync();
            }
        }

        //Trigger Function From btn_Save_Password_msg_Click (Embed Message -> Insert Password -> Save)
        private async void btn_Save_Password_msg_Click(object sender, RoutedEventArgs e)
        {
            string notify = String.Empty;
            if (Input_Password_msg.Text != String.Empty)
            {
                notify = dp.validatePasswdOrMessageInput(Input_Password_msg.Text);
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
                    Input_Password_msg.IsReadOnly = true;
                    Input_Password_msg.Header = NotifyDataText.Saving_Header_Notify_Embed_Msg_pwd;
                    Binary_pwd_embed_msg = dp.Convert_Passwd(Input_Password_msg.Text); //Uncrypt
                    Binary_pwd_embed_msg_encoded = dp.Convert_Passwd_Encrypt(dp.Encrypt_BifidCipher(Input_Password_msg.Text)); //Crypt with Bifid Cipher
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
            if (Input_Password_msg.Text != String.Empty)
            {
                Input_Password_msg.IsReadOnly = false;
                Input_Password_msg.Header = NotifyDataText.Clearing_Header_Notify_Embed_Msg_pwd;
                Input_Password_msg.Text = String.Empty;
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
            SetStatus_Password_2();
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
            if (status_picker_cover_2.Text != "No Image" && Input_Password_msg.IsReadOnly == true && InputMessage.IsReadOnly == true)
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
                        progBar_msg.Visibility = Visibility.Visible;
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
                        progBar_msg.Visibility = Visibility.Collapsed;
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

        //PAGE CONTROL FOR EMBED MESSAGE (EMBED MENU -> EMBED MESSAGE)
        //END


        //--------------------------------------------------------------------------------//


    }
}