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
        Stream imgstrm;
        BitmapDecoder dec;

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

                //imgstrm = await file_cover.OpenStreamForReadAsync();
                //dec = await BitmapDecoder.CreateAsync(imgstrm.AsRandomAccessStream());
                //var bytes = (await dec.GetPixelDataAsync()).DetachPixelData();

                WriteableBitmap writebmp;
                using (IRandomAccessStream strm = await file_cover.OpenAsync(FileAccessMode.Read))
                {
                    BitmapDecoder decoder = await BitmapDecoder.CreateAsync(strm);
                    writebmp = new WriteableBitmap((int)decoder.PixelWidth, (int)decoder.PixelHeight);
                    writebmp.SetSource(strm);
                    var width = writebmp.PixelWidth;
                    var height = writebmp.PixelHeight;
                    var stride = writebmp.ToByteArray();

                    int eee = 0;
                    foreach (byte fd in stride)
                    {
                        System.Diagnostics.Debug.WriteLine("{0} RGB : {1} | {2}",++eee, fd, stride.Length);
                    }

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



                //newpx = GetPixel(bytes, 1, 1, dec.PixelWidth, dec.PixelHeight);


                FileSavePicker fs = new FileSavePicker();
                fs.SuggestedStartLocation = PickerLocationId.PicturesLibrary;
                fs.FileTypeChoices.Add("Portable Image", new List<string>() { ".jpg" });
                StorageFile sf = await fs.PickSaveFileAsync();
                using (IRandomAccessStream fileStream = await sf.OpenAsync(FileAccessMode.ReadWrite))
                {
                    BitmapEncoder encoder = await BitmapEncoder.CreateAsync(BitmapEncoder.JpegEncoderId, fileStream);
                    //encoder.SetPixelData(BitmapPixelFormat.Bgra8, BitmapAlphaMode.Straight, dec.PixelWidth, dec.PixelHeight, dec.DpiX, dec.DpiY, bytes);
                    //await encoder.BitmapProperties
                    await encoder.FlushAsync();
                }
            }

            else
            {
                SetStatus_PickerCover();
            }

        }


        public byte[] GetPixel(byte[] pixels, int x, int y, uint w_img, uint h_img)
        {
            int i = x;
            int j = y;
            int k = (i * (int)w_img + j) * 3;
            var r = pixels[k + 0];
            var g = pixels[k + 1];
            var b = pixels[k + 2];
            string rbin = Convert.ToString(pixels[k + 0], 2).PadLeft(8, '0');
            string gbin = Convert.ToString(pixels[k + 1], 2).PadLeft(8, '0');
            string bbin = Convert.ToString(pixels[k + 2], 2).PadLeft(8, '0');

            //string zxc = String.Format("R: {0} | G: {1} | B: {2}", r,g,b);

            int full_k = (((int)w_img * (int)w_img + (int)h_img) * 3) + 2;
            int asd = -1;
            for (int zx = 0; zx <= full_k; zx++)
            {
                //System.Diagnostics.Debug.WriteLine("Array ke - {0} == {1} ||| {2}", ++asd, pixels[zx], full_k);
            }

            int asas = -1;
            foreach (var elemen in pixels.Take(full_k + 1))
            {
               // System.Diagnostics.Debug.WriteLine("Baris Ke-{0} | {1}", ++asas,  elemen);
            }

            byte[] newx = new byte[pixels.Length];
            Array.Copy(pixels, newx, pixels.Length);

            int s = -1;
            foreach (var el in newx.Take(full_k+1))
            {
                newx[++s] = 0;
            }

            return newx;
            //return zxc;
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
