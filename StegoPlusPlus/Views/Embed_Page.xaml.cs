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
            SetStatus_HidingFile(); 
            SetStatus_PickerCover();
            SetStatus_PickerCover_2();
            Input_Password_file.Text = String.Empty;
            Input_Password_msg.Text = String.Empty;

            btn_CoverImage.Click += new RoutedEventHandler(btn_CoverImage_Click); //Fungsi Click Cover ke Sinkron dengan Fungsi File Picker
            btn_HidingFile.Click += new RoutedEventHandler(btn_HidingFile_Click); //Fungsi Click Hiding ke Sinkron dengan Fungsi Hiding Picker
            btn_CoverImage_2.Click += new RoutedEventHandler(btn_CoverImage_2_Click); //Fungsi Click Cover2 ke Sinkron dengan Fungsi File Picker
        }


        //--------------------------------------------------------------------------------//


        //PAGE CONTROL FOR EMBED MENU
        //BEGIN

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

        //PAGE CONTROL FOR EMBED MENU
        //END


        //--------------------------------------------------------------------------------//


        //PAGE CONTROL FOR EMBED FILE (EMBED MENU -> EMBED FILE)
        //BEGIN

        byte[] newpx;
        BitmapDecoder dec;
        IRandomAccessStream files;
        char[] pwd_file_encoded;
        ContentDialog dlg_embed_file;
        ContentDialogResult show_dlg_embed_file = new ContentDialogResult();

        StorageFile file_cover;
        StorageFile file_hiding;
        StorageFile file_embed;

        //Initial Default Text PickerCover File
        private void SetStatus_PickerCover()
        {
            status_picker_cover.Text = "No Image";
            pathfile_picker_cover.Text = "-";
            sizefile_picker_cover.Text = "-";
            dimensionfile_picker_cover.Text = "-";
            ico_picker_cover.Visibility = Visibility.Collapsed;
        }

        //Initial Default Text Hiding File
        private void SetStatus_HidingFile()
        {
            status_picker_hiding.Text = "No File";
            pathfile_picker_hiding.Text = "-";
            sizefile_picker_hiding.Text = "-";
            typefile_picker_hiding.Text = "-";
            ico_picker_hiding.Visibility = Visibility.Collapsed;
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
             
                status_picker_cover.Text = file_cover.Name;
                pathfile_picker_cover.Text = file_cover.Path.Replace("\\" + file_cover.Name, String.Empty);
                sizefile_picker_cover.Text = String.Format("{0} bytes", propSize);
                dimensionfile_picker_cover.Text = String.Format("{0}", propDimension);


                //Show Thumbnail File Picker
                using (StorageItemThumbnail thumbnail = await file_cover.GetThumbnailAsync(ThumbnailMode.PicturesView))
                {
                    if (thumbnail != null)
                    {
                        ico_picker_cover.Visibility = Visibility.Visible;
                        BitmapImage bitmapImage = new BitmapImage();
                        bitmapImage.SetSource(thumbnail);
                        ico_picker_cover.Source = bitmapImage;
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

        //Trigger Function From btn_Clear_Password_file_Click (Embed File -> Insert Password -> Clear)
        private void btn_Clear_Password_file_Click(object sender, RoutedEventArgs e)
        {

        }

        //Trigger Function From btn_Save_Password_file_Click (Embed File -> Insert Password -> Save)
        private void btn_Save_Password_file_Click(object sender, RoutedEventArgs e)
        {

        }

        //Trigger Function From btn_Clear_FooterMenuEmbedFile_Click (Embed File -> Footer Menu -> Clear)
        private void btn_Clear_FooterMenuEmbedFile_Click(object sender, RoutedEventArgs e)
        {
            SetStatus_PickerCover();
            SetStatus_HidingFile();
        }

        //Trigger Function From btn_Exec_FooterMenuEmbedFile_Click (Embed File -> Footer Menu -> Exec)
        private void btn_Exec_FooterMenuEmbedFile_Click(object sender, RoutedEventArgs e)
        {
            ExecSteg_File();
        }

        private async void ExecSteg_File()
        {
            if (status_picker_cover.Text != "No Image" && Input_Password_file.IsReadOnly == true)
            {
                dlg_embed_file = new ContentDialog()
                {
                    Title = "Apakah yakin akan melanjutkan proses Embedding Message ?",
                    PrimaryButtonText = NotifyDataText.OK_Button,
                    SecondaryButtonText = NotifyDataText.Cancel_Button
                };

                show_dlg_embed_file = await dlg_embed_file.ShowAsync();

                if (show_dlg_embed_file == ContentDialogResult.Primary)
                {
                    dlg_embed_file = new ContentDialog()
                    {
                        Title = "Proses",
                        PrimaryButtonText = NotifyDataText.OK_Button
                    };
                    show_dlg_embed_file = await dlg_embed_file.ShowAsync();
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
                    Title = "Some Field is Empty or Not Saved !\nPlease check again...",
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

        char[] msg_encoded;
        char[] pwd_msg_encoded;
        ContentDialog dlg_embed_msg;
        ContentDialogResult show_dlg_embed_msg = new ContentDialogResult();

        StorageFile file_cover_2;
        StorageFile file_message;
        StorageFile file_embed2;

        //Initial Default Text PickerCover Message
        private void SetStatus_PickerCover_2()
        {
            status_picker_cover_2.Text = "No Image";
            pathfile_picker_cover_2.Text = "-";
            sizefile_picker_cover_2.Text = "-";
            dimensionfile_picker_cover_2.Text = "-";
            ico_picker_cover_2.Visibility = Visibility.Collapsed;
        }

        //Trigger Function From btn_CoverImage_2_Click
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

                status_picker_cover_2.Text = file_cover_2.Name;
                pathfile_picker_cover_2.Text = file_cover_2.Path.Replace("\\" + file_cover_2.Name, String.Empty);
                sizefile_picker_cover_2.Text = String.Format("{0} bytes", propSize);
                dimensionfile_picker_cover_2.Text = String.Format("{0}", propDimension);

                //Show Thumbnail File Picker
                using (StorageItemThumbnail thumbnail = await file_cover_2.GetThumbnailAsync(ThumbnailMode.PicturesView))
                {
                    if (thumbnail != null)
                    {
                        ico_picker_cover_2.Visibility = Visibility.Visible;
                        BitmapImage bitmapImage = new BitmapImage();
                        bitmapImage.SetSource(thumbnail);
                        ico_picker_cover_2.Source = bitmapImage;
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

        private async void btn_Save_Message_Click(object sender, RoutedEventArgs e)
        {
            if (InputMessage.Text != String.Empty)
            {
                InputMessage.IsReadOnly = true;
                InputMessage.Header = NotifyDataText.Saving_Header_Notify_Embed_Msg_msg;
                msg_encoded = dp.KonversiBinary(InputMessage.Text);
            }
            else
            {
                dlg_embed_msg = new ContentDialog()
                {
                    Title = NotifyDataText.Err_Input_Null_Embed_Msg_msg,
                    PrimaryButtonText = "OK"
                };
                show_dlg_embed_msg = await dlg_embed_msg.ShowAsync();
            }
        }

        private async void btn_Clear_Message_Click(object sender, RoutedEventArgs e)
        {
            if (InputMessage.Text != String.Empty)
            {
                InputMessage.IsReadOnly = false;
                InputMessage.Header = "Input Text / Message";
                InputMessage.Text = String.Empty;
                dlg_embed_msg.Title = NotifyDataText.Clear_Input_Embed_Msg_msg;
                dlg_embed_msg.PrimaryButtonText = NotifyDataText.OK_Button;
                show_dlg_embed_msg = await dlg_embed_msg.ShowAsync();
            }           
        }

        private async void btn_Save_Password_msg_Click(object sender, RoutedEventArgs e)
        {
            Input_Password_msg.IsReadOnly = true;
        }

        private async void btn_Clear_Password_msg_Click(object sender, RoutedEventArgs e)
        {
            Input_Password_msg.IsReadOnly = false;
            Input_Password_msg.Text = String.Empty;
        }

        private async void btn_Clear_FooterMenuEmbedMessage_Click(object sender, RoutedEventArgs e)
        {
            SetStatus_PickerCover_2();
        }

        private async void btn_Exec_FooterMenuEmbedMessage_Click(object sender, RoutedEventArgs e)
        {
            ExecSteg_Message();
        }


        private async void ExecSteg_Message()
        {
            if (status_picker_cover_2.Text != "No Image" && Input_Password_msg.IsReadOnly == true && InputMessage.IsReadOnly == true)
            {
                dlg_embed_msg = new ContentDialog()
                {
                    Title = "Apakah yakin akan melanjutkan proses Embedding Message ?",
                    PrimaryButtonText = NotifyDataText.OK_Button,
                    SecondaryButtonText = NotifyDataText.Cancel_Button
                };

                show_dlg_embed_msg = await dlg_embed_msg.ShowAsync();

                if (show_dlg_embed_msg == ContentDialogResult.Primary)
                {
                    dlg_embed_msg = new ContentDialog()
                    {
                        Title = "Proses",
                        PrimaryButtonText = NotifyDataText.OK_Button
                    };
                    show_dlg_embed_msg = await dlg_embed_msg.ShowAsync();
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
