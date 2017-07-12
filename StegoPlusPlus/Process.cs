using System;
using System.Collections.Generic;
using System.IO;
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
                            switch (await Picker_Property.GetPicker(file, type))
                            {
                                case true:
                                    await Conversion.Image(file, "Cover");
                                    return true;
                                case false:
                                    return false;
                            }
                            break;
                        case "File":
                            switch (await Picker_Property.GetPicker(file, type))
                            {
                                case true:
                                    await Conversion.File(file);
                                    return true;
                                case false:
                                    return false;
                            }
                            break;
                        case "Message":
                            Conversion.Message(file);
                            return true;
                    }
                    return false;
                }
                else
                {
                    return false;
                }
            }
        }

        public class Picker_Property
        {
            public static async Task<bool> GetPicker(StorageFile file, string type)
            {
                IDictionary<string, object> prop = await file.Properties.RetrievePropertiesAsync(Data.Prop_File_Picker.All);
                StorageItemThumbnail thumbnail = await file.GetThumbnailAsync(ThumbnailMode.PicturesView);
                BitmapImage bitmap = new BitmapImage();

                switch (type)
                {
                    case "Image":
                        if (prop[Data.Prop_File_Picker.BitDepth].ToString() == "32")
                        {
                            bitmap.SetSource(thumbnail);

                            Reset_Picker("Image");

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
                    case "File":
                        if (GetData.Embed.ContainsKey(Data.Misc.DataPixel) == false)
                        {
                            await PopupDialog.Show(Status.Err, Detail.Insert_File, Err.Null_Size, Icon.Sad);
                            return false;
                        }
                        else
                        {
                            byte[] cover = (byte[])GetData.Embed[Data.Misc.DataPixel];

                            if (int.Parse(prop[Data.Prop_File_Picker.Size].ToString()) < cover.Length / 8)
                            {
                                bitmap.SetSource(thumbnail);

                                Reset_Picker("File");

                                GetData.Picker.Add(Data.Misc.Icon, bitmap);
                                GetData.Picker.Add(Data.Misc.Name, file.Name);
                                GetData.Picker.Add(Data.Misc.Path, file.Path.Replace("\\" + file.Name, String.Empty));
                                GetData.Picker.Add(Data.Misc.Size, prop[Data.Prop_File_Picker.Size]);
                                GetData.Picker.Add(Data.Misc.Type, file.DisplayType);

                                GetData.Embed.Add(Data.Misc.DataNameFile, Conversion.ToBinary(file.Name.Replace(file.FileType, String.Empty), "String"));
                                GetData.Embed.Add(Data.Misc.DataExtension, Conversion.ToBinary(file.FileType.ToLower(), "String"));
                                GetData.Embed.Add(Data.Misc.DataType, Conversion.ToBinary("0", "String"));
                                return true;
                            }
                            else
                            {
                                await PopupDialog.Show(Status.Err, Detail.Insert_File, Err.Overload_Size, Icon.Sad);
                                return false;
                            }
                        }
                }
                return false;
            }

            public static void Reset_Picker(string type)
            {
                switch(type)
                {
                    case "Image":
                        GetData.Reset_Data(type);
                        GetData.Picker.Clear();
                        break;
                    case "File":
                        GetData.Reset_Data(type);
                        GetData.Picker.Clear();
                        break;
                }
            }
        }

        public class GetData
        {
            public static Dictionary<string, object> Picker = new Dictionary<string, object>();
            public static Dictionary<string, object> Embed = new Dictionary<string, object>();
            public static void Reset_Data(string type)
            {
                switch(type)
                {
                    case "Image":
                        Embed.Remove(Data.Misc.DataPixel);
                        break;
                    case "File":
                        Embed.Remove(Data.Misc.DataType);
                        Embed.Remove(Data.Misc.DataSecret);
                        Embed.Remove(Data.Misc.DataNameFile);
                        Embed.Remove(Data.Misc.DataExtension);
                        break;
                    case "Passwd":
                        Embed.Remove(Data.Misc.DataPassword);
                        break;
                    case "All":
                        Embed.Remove(Data.Misc.DataType);
                        Embed.Remove(Data.Misc.DataPixel);
                        Embed.Remove(Data.Misc.DataSecret);
                        Embed.Remove(Data.Misc.DataPassword);
                        Embed.Remove(Data.Misc.DataNameFile);
                        Embed.Remove(Data.Misc.DataExtension);
                        break;
                }
            }
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

            public static async Task File(StorageFile value)
            {
                byte[] bin;
                using (Stream st = await value.OpenStreamForReadAsync())
                {
                    using (BinaryReader binaryReader = new BinaryReader(st))
                    {
                        bin = binaryReader.ReadBytes((int)st.Length).ToArray();
                        GetData.Embed.Add(Data.Misc.DataSecret, ToBinary(bin, "File"));
                    }
                }
            }

            public static byte[] Message(StorageFile img)
            {
                byte[] value = null;
                return value;
            }

            public static char[] ToBinary(object value, string type)
            {
                string result = String.Empty;
                switch (type)
                {
                    case "String":
                        foreach (char x in (string)value)
                        {
                            result += Convert.ToString(x, 2).PadLeft(8, '0');
                        }
                        break;
                    case "Passwd":
                        foreach (char x in (List<int>)value)
                        {
                            result += Convert.ToString(x, 2).PadLeft(8, '0');
                        }
                        break;
                    case "File":
                        foreach (char x in (byte[])value)
                        {
                            result += Convert.ToString(x, 2).PadLeft(8, '0');
                        }
                        break;
                }
                return result.ToCharArray();
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

                GetData.Embed.Add(Data.Misc.DataPassword, Conversion.ToBinary(passwd, "Passwd"));
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
                foreach (char c in value) if (Data.Misc.Character.Contains(c) == false) result = false;
                return result;
            }
        }

    }
}