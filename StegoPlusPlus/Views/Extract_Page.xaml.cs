using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.Storage;
using Windows.Storage.FileProperties;
using Windows.Storage.Pickers;
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
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class Extract_Page : Page
    {
        public Extract_Page()
        {
            this.InitializeComponent();
            check_transition_effect_status();
            check_theme_status();
            InitializingPage();
            btn_StegImage.Click += new RoutedEventHandler(btn_StegImage_Click);
            btn_StegImage_check_healthy.Click += new RoutedEventHandler(btn_StegImage_check_healthy_Click);
            btn_StegImage_folder.Click += new RoutedEventHandler(btn_StegImage_folder_Click);
        }

        //Function Check Effect Transition
        TransitionCollection collection = new TransitionCollection();
        NavigationThemeTransition theme = new NavigationThemeTransition();

        StorageFile file_steg;
        StorageFile file_steg_check;

        StorageFolder steg_folder;

        //Create List Specific Property File Cover Picker
        List<string> propImgList = new List<string>();

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

        //Initial Text
        private void InitializingPage()
        {
            HeaderInfo.Text = HeaderPage.ExtractPage;
        }

        //Prop Text
        private void SetStatus_PickerSteg()
        {
            status_picker_steg.Text = "No Image";
            pathfile_picker_steg.Text = "-";
            sizefile_picker_steg.Text = "-";
            dimensionfile_picker_steg.Text = "-";
            ico_picker_steg.Visibility = Visibility.Collapsed;
        }

        private void SetStatus_PickerFolderSteg()
        {
            status_picker_steg_folder.Text = "No Folder";
            pathfile_picker_steg_folder.Text = "-";
        }

        private void SetStatus_PickerStegCheck()
        {
            status_picker_steg_check.Text = "No Image";
            pathfile_picker_steg_check.Text = "-";
            sizefile_picker_steg_check.Text = "-";
            dimensionfile_picker_steg_check.Text = "-";
            ico_picker_steg_check.Visibility = Visibility.Collapsed;
        }

        //Trigger Function From btn_StegImage_Click
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

                status_picker_steg.Text = file_steg.Name;
                pathfile_picker_steg.Text = file_steg.Path.Replace("\\" + file_steg.Name, String.Empty);
                sizefile_picker_steg.Text = String.Format("{0} bytes", propSize);
                dimensionfile_picker_steg.Text = String.Format("{0}", propDimension);

                //Show Thumbnail File Picker
                using (StorageItemThumbnail thumbnail = await file_steg.GetThumbnailAsync(ThumbnailMode.PicturesView))
                {
                    if (thumbnail != null)
                    {
                        ico_picker_steg.Visibility = Visibility.Visible;
                        BitmapImage bitmapImage = new BitmapImage();
                        bitmapImage.SetSource(thumbnail);
                        ico_picker_steg.Source = bitmapImage;
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

        //Trigger Function From btn_StegImage_folder_Click
        private async void btn_StegImage_folder_Click(object sender, RoutedEventArgs e)
        {
            //Set an Extensions File Cover
            FolderPicker picker_steg_folder = new FolderPicker();
            foreach (string extension in FileExtensions.Stego)
            {
                picker_steg_folder.FileTypeFilter.Add(extension);
            }

            //Set Get a Name Property Image File Cover
            foreach (string propImg in propImage.All)
            {
                propImgList.Add(propImg);
            }

            steg_folder = await picker_steg_folder.PickSingleFolderAsync();

            //Check File Picker
            if (steg_folder != null)
            {
                //Get Property File Picker Selected (await)
                status_picker_steg_folder.Text = "";
                pathfile_picker_steg_folder.Text = steg_folder.Path;
            }

            else
            {
                SetStatus_PickerSteg();
            }
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

                status_picker_steg_check.Text = file_steg_check.Name;
                pathfile_picker_steg_check.Text = file_steg_check.Path.Replace("\\" + file_steg_check.Name, String.Empty);
                sizefile_picker_steg_check.Text = String.Format("{0} bytes", propSize);
                dimensionfile_picker_steg_check.Text = String.Format("{0}", propDimension);

                //Show Thumbnail File Picker
                using (StorageItemThumbnail thumbnail = await file_steg_check.GetThumbnailAsync(ThumbnailMode.PicturesView))
                {
                    if (thumbnail != null)
                    {
                        ico_picker_steg_check.Visibility = Visibility.Visible;
                        BitmapImage bitmapImage = new BitmapImage();
                        bitmapImage.SetSource(thumbnail);
                        ico_picker_steg_check.Source = bitmapImage;
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

        private void Exec_FooterMenuExtractFile_Click(object sender, RoutedEventArgs e)
        {

        }

        private void Clear_FooterMenuExtractFile_Click(object sender, RoutedEventArgs e)
        {
            SetStatus_PickerSteg();
            SetStatus_PickerFolderSteg();
        }

        private void Exec_FooterMenuCheckSteg_Click(object sender, RoutedEventArgs e)
        {

        }

        private void Clear_FooterMenuCheckSteg_Click(object sender, RoutedEventArgs e)
        {
            SetStatus_PickerStegCheck();
        }
    }
}
