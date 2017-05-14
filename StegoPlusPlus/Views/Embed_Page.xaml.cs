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
        SoftwareBitmap sftbmp;

        public Embed_Page()
        {
            this.InitializeComponent();
            check_transition_effect_status();
            check_theme_status();
            InitializingPage();
            btn_CoverImage.Click += new RoutedEventHandler(btn_CoverImage_Click); //Fungsi Click Cover ke Sinkron dengan Fungsi File Picker
            btn_HidingFile.Click += new RoutedEventHandler(btn_HidingFile_Click); //Fungsi Click Hiding ke Sinkron dengan Fungsi Hiding Picker
            btn_CoverImage_2.Click += new RoutedEventHandler(btn_CoverImage_2_Click); //Fungsi Click Cover2 ke Sinkron dengan Fungsi File Picker
            Exec_FooterMenuEmbedFile.Click += new RoutedEventHandler(Exec_FooterMenuEmbedFile_Click);
        }

        StorageFile file_cover;
        StorageFile file_hiding;
        StorageFile file_embed;

        StorageFile file_cover_2;
        StorageFile file_message;
        StorageFile file_embed2;
        //string R, G, B;

        //Create List Specific Property File Cover Picker
        List<string> propImgList = new List<string>();
        
        //Function Check Effect Transition
        TransitionCollection collection = new TransitionCollection();
        NavigationThemeTransition theme = new NavigationThemeTransition();

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
            HeaderInfo.Text = HeaderPage.EmbedPage;
        }

        //Prop Text
        private void SetStatus_PickerCover()
        {
            status_picker_cover.Text = "No Image";
            pathfile_picker_cover.Text = "-";
            sizefile_picker_cover.Text = "-";
            dimensionfile_picker_cover.Text = "-";
            ico_picker_cover.Visibility = Visibility.Collapsed;
        }

        private void SetStatus_HidingFile()
        {
            status_picker_hiding.Text = "No File";
            pathfile_picker_hiding.Text = "-";
            sizefile_picker_hiding.Text = "-";
            typefile_picker_hiding.Text = "-";
            ico_picker_hiding.Visibility = Visibility.Collapsed;
        }

        private void SetStatus_PickerCover_2()
        {
            status_picker_cover_2.Text = "No Image";
            pathfile_picker_cover_2.Text = "-";
            sizefile_picker_cover_2.Text = "-";
            dimensionfile_picker_cover_2.Text = "-";
            ico_picker_cover_2.Visibility = Visibility.Collapsed;
        }


        byte[] newpx;
        BitmapDecoder dec;
        IRandomAccessStream files;


        //Trigger Function From btn_CoverImage_Click
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

                WriteableBitmap writebmp;
                using (IRandomAccessStream strm = await file_cover.OpenAsync(FileAccessMode.Read))
                {
                    dec = await BitmapDecoder.CreateAsync(strm);
                    writebmp = new WriteableBitmap((int)dec.PixelWidth, (int)dec.PixelHeight);
                    writebmp.SetSource(strm);
                    var width = writebmp.PixelWidth;
                    var height = writebmp.PixelHeight;
                    newpx = writebmp.ToByteArray();                    
                }
                

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

                FileSavePicker fs = new FileSavePicker();
                fs.SuggestedStartLocation = PickerLocationId.PicturesLibrary;
                fs.FileTypeChoices.Add("JPEG Image", new List<string>() { ".jpg" });
                fs.FileTypeChoices.Add("PNG Image", new List<string>() { ".png" });
                StorageFile sf = await fs.PickSaveFileAsync();
                using (files = await sf.OpenAsync(FileAccessMode.ReadWrite))
                {
                    BitmapEncoder encoder = await BitmapEncoder.CreateAsync(BitmapEncoder.)
                    encoder.SetPixelData(dec.BitmapPixelFormat, dec.BitmapAlphaMode, (uint)dec.PixelWidth, (uint)dec.PixelHeight, dec.DpiX, dec.DpiY, newpx);
                    var lis = new List<KeyValuePair<string, BitmapTypedValue>>();
                    //var art = new BitmapTypedValue("Hello World", PropertyType.String);


                    var author = new BitmapTypedValue("Some Author", PropertyType.String);
                    var title = new BitmapTypedValue("00103 My Company", PropertyType.String);
                    lis.Add(new KeyValuePair<string, BitmapTypedValue>("System.Author", author));
                    lis.Add(new KeyValuePair<string, BitmapTypedValue>("System.Title", title));

                    //lis.Add(new KeyValuePair<string, BitmapTypedValue>("System.Photo.Orientation", art));
                    await encoder.BitmapProperties.SetPropertiesAsync(lis);
                    await encoder.FlushAsync();
                    //await files.FlushAsync();
                }
            }

            else
            {
                SetStatus_PickerCover();
            }
        }



       
        //Trigger Function From btn_HidingImage_Click
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

        private void Clear_FooterMenuEmbedFile_Click(object sender, RoutedEventArgs e)
        {
            SetStatus_PickerCover();
            SetStatus_HidingFile();
        }

        private async void Exec_FooterMenuEmbedFile_Click(object sender, RoutedEventArgs e)
        {
            var res = new FileStream(Path.Combine(ApplicationData.Current.LocalFolder.Path, "tmp.png"), FileMode.Create);
            var enc = await BitmapEncoder.CreateAsync(BitmapEncoder.PngEncoderId, res.AsRandomAccessStream());
            enc.SetPixelData(BitmapPixelFormat.Bgra8, BitmapAlphaMode.Ignore, dec.PixelWidth, dec.PixelHeight, 96, 96, newpx);
            //enc.SetSoftwareBitmap(SoftwareBitmap)
            //await enc.FlushAsync();
            res.Dispose();




        }

        private void Clear_FooterMenuEmbedMessage_Click(object sender, RoutedEventArgs e)
        {
            SetStatus_PickerCover_2();
        }

        private void Exec_FooterMenuEmbedMessage_Click(object sender, RoutedEventArgs e)
        {
            ExecSteg_Message();
        }

        private void btn_Save_Message_Click(object sender, RoutedEventArgs e)
        {
            string msg_bef = InputMessage.Text;
            DataProses dp = new DataProses();
            string msg_aft = dp.KonversiMessage(msg_bef);
            hasil.Text = msg_aft;
        }

        private void btn_Clear_Message_Click(object sender, RoutedEventArgs e)
        {
            hasil.Text = string.Empty;
        }

        private async void ExecSteg_Message()
        {
            TextBox inputpasswd = new TextBox();
            inputpasswd.AcceptsReturn = false;
            inputpasswd.Height = 32;
            ContentDialog dlg = new ContentDialog();
            dlg.Title = "Ini adalah Title";
            dlg.IsSecondaryButtonEnabled = true;
            dlg.PrimaryButtonText = "OK";
            dlg.SecondaryButtonText = "Cancel";
            //if (await dlg.ShowAsync() == ContentDialogResult.Primary)
            //    return inputpasswd.Text;
            ContentDialogResult resu = await dlg.ShowAsync();
        }

        private void btn_Save_Password_msg_Click(object sender, RoutedEventArgs e)
        {

        }

        private void btn_Clear_Password_msg_Click(object sender, RoutedEventArgs e)
        {

        }

        private void btn_Save_Password_file_Click(object sender, RoutedEventArgs e)
        {

        }

        private void btn_Clear_Password_file_Click(object sender, RoutedEventArgs e)
        {

        }

    }
}
