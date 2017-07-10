using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.Graphics.Imaging;
using Windows.Storage;
using Windows.Storage.FileProperties;
using Windows.Storage.Pickers;
using Windows.Storage.Streams;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Media.Animation;
using Windows.UI.Xaml.Media.Imaging;
using static StegoPlusPlus.Data.Prop_Popup;
using static StegoPlusPlus.Data.Prop_Popup.Title;

namespace StegoPlusPlus
{
    class Process
    {
        #region Process Theme
        public class Theme
        {
            #region GetTheme from Storage
            public static bool GetTheme(string getTheme)
            {
                bool value = (getTheme == "Light") ? true : false;
                return value;
            }
            #endregion
            #region SetTheme to Storage
            public static void SetTheme(string value)
            {
                ApplicationData.Current.LocalSettings.Values["BG_set"] = value;
            }
            #endregion
        }
        #endregion
        #region Process Transition
        public class Transition
        {
            #region GetTransition from Storage
            public static TransitionCollection GetTransition(string getTransition)
            {
                NavigationThemeTransition theme = new NavigationThemeTransition();
                TransitionCollection collector = new TransitionCollection();

                if (getTransition == "Continuum")
                {
                    theme.DefaultNavigationTransitionInfo = new ContinuumNavigationTransitionInfo();
                }
                else if (getTransition == "Common")
                {
                    theme.DefaultNavigationTransitionInfo = new CommonNavigationTransitionInfo();
                }
                else if (getTransition == "Slide")
                {
                    theme.DefaultNavigationTransitionInfo = new SlideNavigationTransitionInfo();
                }
                else
                {
                    theme.DefaultNavigationTransitionInfo = new SuppressNavigationTransitionInfo();
                }
                collector.Add(theme);
                return collector;
            }
            #endregion
            #region SetTransition to Storage
            public static void SetTransition(string value)
            {
                ApplicationData.Current.LocalSettings.Values["Effect_set"] = value;
            }
            #endregion
        }
        #endregion
        public class Picker
        {
            public static async Task<bool> Embed(string[] extension, string type)
            {
                //Set an Extensions File Cover
                FileOpenPicker picker = new FileOpenPicker();
                List<string> propImgList = new List<string>();
                foreach (string ext in extension)
                {
                    picker.FileTypeFilter.Add(ext);
                }

                StorageFile file = await picker.PickSingleFileAsync();

                if (file != null)
                {
                    switch (type)
                    {
                        case "Image":
                            switch (await Picker_Property.Image(file))
                            {
                                case true:
                                    await Conversion.Image(file, "Cover");
                                    return true;
                                case false:
                                    return false;
                            }
                            break;
                        case "File":
                            Conversion.File(file);
                            return true;
                        case "Message":
                            Conversion.Message(file);
                            return true;
                    }
                    return false;
                }
                else
                {
                    await PopupDialog.Show(Status.Success, Detail.Embed_Message, Complete.Clear_Input_Message, Icon.Smile);
                    return false;
                }
            }

        }

        public class Picker_Property
        {
            public static async Task<bool> Image(StorageFile file)
            {
                IDictionary<string, object> prop = await file.Properties.RetrievePropertiesAsync(Data.Prop_File_Picker.All);

                if (prop[Data.Prop_File_Picker.BitDepth].ToString() == "32")
                {
                    //Get thumbnail icon && Reset Picker Data Object
                    StorageItemThumbnail thumbnail = await file.GetThumbnailAsync(ThumbnailMode.PicturesView);
                    BitmapImage bitmap = new BitmapImage();
                    bitmap.SetSource(thumbnail);

                    Reset_Picker();

                    GetData.Picker.Add(Data.Misc.Icon, bitmap);
                    GetData.Picker.Add(Data.Misc.Name, file.Name);
                    GetData.Picker.Add(Data.Misc.Path, file.Path.Replace("\\" + file.Name, String.Empty));
                    GetData.Picker.Add(Data.Misc.Size, prop[Data.Prop_File_Picker.Size]);
                    GetData.Picker.Add(Data.Misc.Dimensions, prop[Data.Prop_File_Picker.Dimensions]);
                    GetData.Picker.Add(Data.Misc.BitDepth, prop[Data.Prop_File_Picker.BitDepth]);
                    return true;
                }
                else
                {
                    await PopupDialog.Show(Status.Err, Detail.Image_Cover, Err.Invalid_32bitDepth, Icon.Sad);
                    return false;
                }                
            }

            public static void Reset_Picker()
            {
                GetData.Picker.Clear();
                GetData.Embed.Remove(Data.Misc.DataPixel);
            }

        }

        public class GetData
        {
            public static Dictionary<string, object> Picker = new Dictionary<string, object>();
            public static Dictionary<string, object> Embed = new Dictionary<string, object>();
        }

        public class Conversion
        {
            public static async Task Image(StorageFile img, string type)
            {
                IRandomAccessStream ram;
                BitmapDecoder decoder;
                byte[] pixel;
                using (ram = await img.OpenAsync(FileAccessMode.ReadWrite))
                {
                    switch (type)
                    {
                        case "Cover":
                            decoder = await BitmapDecoder.CreateAsync(ram);
                            pixel = (await decoder.GetPixelDataAsync()).DetachPixelData();

                            GetData.Embed.Add(Data.Misc.DataPixel, pixel);
                            GetData.Picker.Add(Data.Misc.Pixel, pixel.Length);
                            GetData.Picker.Add(Data.Misc.Eta, (pixel.Length / 8));
                            break;
                        case "Stego":
                            //Conversion.File(file);
                            break;
                    }
                }
            }
            public static byte[] File(StorageFile img)
            {
                byte[] value = null;
                return value;
            }
            public static byte[] Message(StorageFile img)
            {
                byte[] value = null;
                return value;
            }

            public static char[] ToBinary(List<int> value)
            {

            }
        }

        public class Bifid_Cipher
        {
            public static string Encrypt(string value)
            {
                char[] input_char = value.ToCharArray();
                List<int> list_x = new List<int>();
                List<int> list_y = new List<int>();
                List<int> list_xy = new List<int>();
                int[] x_y; //From list_xy Convert to Array
                string[] crypt_x;
                string[] crypt_y;
                string result = String.Empty; //Encrypt of Passwd
                List<int> passwd = new List<int>();

                foreach (var xx in input_char)
                {
                    for (int x = 0; x < Data.Misc.Matrix.GetLength(0); ++x) //Width
                    {
                        for (int y = 0; y < Data.Misc.Matrix.GetLength(1); ++y) //Height
                        {
                            if (Data.Misc.Matrix[x, y].Equals(xx))
                            {
                                list_x.Add(x);
                                list_y.Add(y);
                            }
                        }
                    }
                }

                list_xy = list_x;
                list_xy.AddRange(list_y);
                x_y = list_xy.ToArray();

                crypt_x = new string[x_y.Length / 2];
                crypt_y = new string[x_y.Length / 2];

                int tt = 0;
                for (int i = 0; i < x_y.Length; i += 2)
                {
                    crypt_x[tt] = x_y[i].ToString();
                    crypt_y[tt] = x_y[i + 1].ToString();
                    ++tt;
                }

                for (int i = 0; i < crypt_x.Length; i++)
                {
                    passwd.Add(Data.Misc.Matrix[Convert.ToInt32(crypt_x[i]), Convert.ToInt32(crypt_y[i])]);
                    result += (int)Data.Misc.Matrix[Convert.ToInt32(crypt_x[i]), Convert.ToInt32(crypt_y[i])] + " ";
                }

                GetData.Embed.Add(Data.Misc.DataPassword, passwd);
                return result;
            }
            public static string Decrypt(string value)
            {
                string asd = String.Empty;
                return asd;
            }
        }

        public class Validate
        {
            public static bool Input(string value)
            {
                bool result = true;
                //foreach (char c in value) if (Data.Misc.Character.Contains(c) == false) result = false;
                foreach (char c in value)
                {
                    if (Data.Misc.Character.Contains(c) == false)
                    {
                        result = false;
                    }
                    //System.Diagnostics.Debug.WriteLine(c);
                }
                return result;
            }
        }

    }
}
