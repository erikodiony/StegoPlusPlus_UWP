using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.Foundation;
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
                FileOpenPicker picker = new FileOpenPicker();
                PopupDialog.Loading pl = new PopupDialog.Loading();
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
                                    Task x = Task.Run(() => Conversion.File(file));
                                    pl.Show(true, Data.Misc.PleaseWait, Data.Misc.PleaseWaitDetail);
                                    await x;
                                    if (x.IsCompleted == true) pl.Show(false, String.Empty, String.Empty);
                                    return true;
                                case false:
                                    return false;
                            }
                            break;
                        case "Message":
                            switch(await Picker_Property.GetPicker(file, type))
                            {
                                case true:
                                    await Conversion.Message(file);
                                    return true;
                                case false:
                                    return false;
                            }
                            break;
                        case "Stego":
                            switch(await Picker_Property.GetPicker(file, type))
                            {
                                case true:
                                    await Conversion.Image(file, type);
                                    await Validate.SecretData(file);
                                    return true;
                                case false:
                                    return false;
                            }
                            break;
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
                    case "Stego":
                        bitmap.SetSource(thumbnail);

                        Reset_Picker("Image");

                        GetData.Picker.Add(Data.Misc.Icon, bitmap);
                        GetData.Picker.Add(Data.Misc.Name, file.Name);
                        GetData.Picker.Add(Data.Misc.Path, file.Path.Replace("\\" + file.Name, String.Empty));
                        GetData.Picker.Add(Data.Misc.Size, prop[Data.Prop_File_Picker.Size]);
                        GetData.Picker.Add(Data.Misc.Dimensions, prop[Data.Prop_File_Picker.Dimensions]);
                        return true;
                    default:
                        if (GetData.Embed.ContainsKey(Data.Misc.DataPixel) == false)
                        {
                            switch(type)
                            {
                                case "File":
                                    await PopupDialog.Show(Status.Err, Detail.Insert_File, Err.Null_Size, Icon.Sad);
                                    return false;
                                case "Message":
                                    await PopupDialog.Show(Status.Err, Detail.Insert_Message, Err.Null_Size, Icon.Sad);
                                    return false;
                                default:
                                    return false;
                            }
                        }
                        else
                        {
                            byte[] cover = (byte[])GetData.Embed[Data.Misc.DataPixel];

                            if (int.Parse(prop[Data.Prop_File_Picker.Size].ToString()) < cover.Length / 8)
                            {
                                if(int.Parse(prop[Data.Prop_File_Picker.Size].ToString()) < 150000)
                                {
                                    switch(type)
                                    {
                                        case "File":
                                            if (await PopupDialog.ShowConfirm(Status.Confirm, Detail.Insert_File, Confirm.isLargeFile, Icon.Flat) == false)
                                            {
                                                return false;
                                            }
                                            break;
                                        case "Message":
                                            if (await PopupDialog.ShowConfirm(Status.Confirm, Detail.Insert_Message, Confirm.isLargeFile, Icon.Flat) == false)
                                            {
                                                return false;
                                            }
                                            break;
                                    }

                                    bitmap.SetSource(thumbnail);

                                    Reset_Picker("File");

                                    GetData.Picker.Add(Data.Misc.Icon, bitmap);
                                    GetData.Picker.Add(Data.Misc.Name, file.Name);
                                    GetData.Picker.Add(Data.Misc.Path, file.Path.Replace("\\" + file.Name, String.Empty));
                                    GetData.Picker.Add(Data.Misc.Size, prop[Data.Prop_File_Picker.Size]);
                                    GetData.Picker.Add(Data.Misc.Type, file.DisplayType);

                                    //Encrypt NameFile & Extension
                                    GetData.Embed.Add(Data.Misc.DataNameFile, Conversion.ToBinary(await Bifid_Cipher.Execute("Embed", file.Name.Replace(file.FileType, String.Empty), String.Empty), "String"));
                                    GetData.Embed.Add(Data.Misc.DataExtension, Conversion.ToBinary(await Bifid_Cipher.Execute("Embed", file.FileType.ToLower(), String.Empty), "String"));
                                    switch (type)
                                    {
                                        case "File":
                                            GetData.Embed.Add(Data.Misc.DataType, Conversion.ToBinary("0", "String"));
                                            break;
                                        case "Message":
                                            GetData.Embed.Add(Data.Misc.DataType, Conversion.ToBinary("1", "String"));
                                            break;
                                    }
                                    return true;
                                }
                                else
                                {
                                    switch (type)
                                    {
                                        case "File":
                                            await PopupDialog.Show(Status.Err, Detail.Insert_File, Err.More150Kb, Icon.Sad);
                                            return false;
                                        case "Message":
                                            await PopupDialog.Show(Status.Err, Detail.Insert_Message, Err.More150Kb, Icon.Sad);
                                            return false;
                                        default:
                                            return false;
                                    }
                                }
                            }
                            else
                            {
                                switch(type)
                                {
                                    case "File":
                                        await PopupDialog.Show(Status.Err, Detail.Insert_File, Err.Overload_Size, Icon.Sad);
                                        return false;
                                    case "Message":
                                        await PopupDialog.Show(Status.Err, Detail.Insert_Message, Err.Overload_Size, Icon.Sad);
                                        return false;
                                    default:
                                        return false;
                                }
                            }
                        }
                }
            }
            public static void Reset_Picker(string type)
            {
                switch(type)
                {
                    case "Image":
                        GetData.Reset_Data("Embed", type);
                        GetData.Reset_Data("Extract", type);
                        GetData.Picker.Clear();
                        break;
                    case "File":
                        GetData.Reset_Data("Embed", type);
                        GetData.Picker.Clear();
                        break;
                    case "Text":
                        GetData.Picker.Remove(Data.Misc.Text);
                        break;
                }
            }
        }

        public class GetData
        {
            public static BitmapDecoder Decoder;
            public static Dictionary<string, object> Picker = new Dictionary<string, object>();
            public static Dictionary<string, object> Embed = new Dictionary<string, object>();
            public static Dictionary<string, object> Extract = new Dictionary<string, object>();
            public static Dictionary<string, object> SecretData = new Dictionary<string, object>();
            public static void Reset_Data(string type, string type2)
            {
                switch(type)
                {
                    case "Embed":
                        switch(type2)
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
                                Embed.Clear();
                                break;
                        }
                        break;
                    case "Extract":
                        switch(type2)
                        {
                            case "Image":
                                Extract.Remove(Data.Misc.DataPixel);
                                break;
                            case "Passwd":
                                Extract.Remove(Data.Misc.DataPassword);
                                Extract.Remove(Data.Misc.DataType);
                                Extract.Remove(Data.Misc.DataNameFile);
                                Extract.Remove(Data.Misc.DataExtension);
                                Extract.Remove(Data.Misc.DataSecret);
                                break;
                            case "Secret":
                                SecretData.Clear();
                                break;
                            case "All":
                                SecretData.Clear();
                                Extract.Clear();
                                break;
                        }
                        break;
                }
            }
        }

        public class Conversion
        {
            public static async Task Image(StorageFile file, string type)
            {
                IRandomAccessStream ram;
                byte[] pixel;
                using (ram = await file.OpenAsync(FileAccessMode.ReadWrite))
                {
                    switch (type)
                    {
                        case "Cover":
                            GetData.Decoder = await BitmapDecoder.CreateAsync(ram);
                            pixel = (await GetData.Decoder.GetPixelDataAsync()).DetachPixelData();

                            GetData.Embed.Add(Data.Misc.DataPixel, pixel);
                            GetData.Picker.Add(Data.Misc.Pixel, pixel.Length);
                            GetData.Picker.Add(Data.Misc.Eta, (pixel.Length / 8));
                            break;
                        case "Stego":
                            GetData.Decoder = await BitmapDecoder.CreateAsync(ram);
                            pixel = (await GetData.Decoder.GetPixelDataAsync()).DetachPixelData();
                            GetData.Extract.Add(Data.Misc.DataPixel, pixel);
                            break;
                    }
                }
            }
            public static async Task File(StorageFile file)
            {
                byte[] bin;
                using (Stream st = await file.OpenStreamForReadAsync())
                {
                    using (BinaryReader binaryReader = new BinaryReader(st))
                    {
                        bin = binaryReader.ReadBytes((int)st.Length).ToArray();
                        GetData.Embed.Add(Data.Misc.DataSecret, ToBinary(bin, "File/Cipher"));
                    }
                }
            }
            public static async Task Message(StorageFile file)
            {
                Picker_Property.Reset_Picker("Text");
                GetData.Picker.Add(Data.Misc.Text, await FileIO.ReadTextAsync(file));
            }
            public static char[] ToBinary(object value, string type)
            {
                string result = String.Empty;
                switch (type)
                {
                    case "String":
                        foreach (var x in (string)value)
                        {
                            result += Convert.ToString(x, 2).PadLeft(8, '0');
                        }
                        break;
                    case "File/Cipher":
                        foreach (var x in (byte[])value)
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
            public static async Task<string> Execute(string exec, string value, string type)
            {
                if (exec == "Embed")
                {
                    PopupDialog.Loading pl = new PopupDialog.Loading();
                    if (value.Length > 1000) pl.Show(true, Data.Misc.PleaseWait, Data.Misc.PleaseWaitDetail);

                    Picker_Property.Reset_Picker(Data.Misc.Text);

                    var x = Task.Run(() => Encrypt(value, type));
                    var result = await x;

                    if (x.IsCompleted == true) pl.Show(false, String.Empty, String.Empty);                    
                    return result;
                }
                else
                {
                    var result = await Task.Run(() => Decrypt(value));
                    return result;
                }
            }
            public static string Encrypt(string value, string type)
            {
                char[] input_char = value.ToCharArray();
                List<int> list_x = new List<int>();
                List<int> list_y = new List<int>();
                List<int> list_xy = new List<int>();
                int[] x_y; //From list_xy Convert to Array
                string[] crypt_x;
                string[] crypt_y;
                List<byte> result_lst = new List<byte>();
                string result_txt = String.Empty;
                string result = String.Empty;

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
                    result_lst.Add(Convert.ToByte(Data.Misc.Matrix[Convert.ToInt32(crypt_x[i]), Convert.ToInt32(crypt_y[i])]));
                    result_txt += (int)Data.Misc.Matrix[Convert.ToInt32(crypt_x[i]), Convert.ToInt32(crypt_y[i])] + " ";
                }

                switch (type)
                {
                    case "Passwd":
                        GetData.Embed.Add(Data.Misc.DataPassword, Conversion.ToBinary(result_lst.ToArray(), "File/Cipher"));
                        GetData.Picker.Add(Data.Misc.Text, result_txt);
                        result = String.Empty;
                        break;
                    case "Message":
                        GetData.Embed.Add(Data.Misc.DataSecret, Conversion.ToBinary(result_lst.ToArray(), "File/Cipher"));
                        GetData.Picker.Add(Data.Misc.Text, result_txt);
                        result = String.Empty;
                        break;
                    default:
                        result = Encoding.ASCII.GetString(result_lst.ToArray());
                        break;
                }
                return result;
            }
            public static string Decrypt(string value)
            {
                char[] input_char = value.ToCharArray();
                List<int> list_x = new List<int>();
                List<int> list_y = new List<int>();
                int[] arr_x;
                int[] arr_y;
                string xy = String.Empty;
                string result = String.Empty;

                foreach (var xx in input_char)
                {
                    for (int x = 0; x < Data.Misc.Matrix.GetLength(0); ++x)
                    {
                        for (int y = 0; y < Data.Misc.Matrix.GetLength(1); ++y)
                        {
                            if (Data.Misc.Matrix[x, y].Equals(xx))
                            {
                                list_x.Add(x);
                                list_y.Add(y);
                            }
                        }
                    }
                }

                arr_x = new int[list_x.Capacity];
                arr_y = new int[list_y.Capacity];

                arr_x = list_x.ToArray();
                arr_y = list_y.ToArray();

                for (int i = 0; i < arr_x.Length; i++)
                {
                    xy += arr_x[i].ToString() + arr_y[i].ToString();
                }

                char[] char_xy = xy.ToCharArray();
                string[] str_x = new string[char_xy.Length / 2];
                string[] str_y = new string[char_xy.Length / 2];

                int tt = 0;

                for (int i = 0; i < char_xy.Length / 2; i++)
                {
                    str_x[i] = char_xy[i].ToString();
                    str_y[i] = char_xy[i + char_xy.Length / 2].ToString();
                    ++tt;
                }

                for (int i = 0; i < str_x.Length; i++)
                {
                    result += Data.Misc.Matrix[Convert.ToInt32(str_x[i]), Convert.ToInt32(str_y[i])].ToString();
                }
                return result;
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
            public static async Task SecretData(StorageFile file)
            {
                IRandomAccessStream ram;
                using (ram = await file.OpenAsync(FileAccessMode.ReadWrite))
                {
                    GetData.Decoder = await BitmapDecoder.CreateAsync(ram);
                    BitmapPropertySet prop = await GetData.Decoder.BitmapProperties.GetPropertiesAsync(new string[] { Data.Misc.Secret });
                    if (prop.ContainsKey(Data.Misc.Secret) == true)
                    {
                        GetData.Reset_Data("Extract", "Secret");
                        string[] secret = ((string)prop[Data.Misc.Secret].Value).Split('|');

                        GetData.SecretData.Add(Data.Misc.DataPassword, secret[0]);
                        GetData.SecretData.Add(Data.Misc.DataType, secret[1]);
                        GetData.SecretData.Add(Data.Misc.DataNameFile, secret[2]);
                        GetData.SecretData.Add(Data.Misc.DataExtension, secret[3]);
                        GetData.SecretData.Add(Data.Misc.DataSecret, secret[4]);
                    }
                    else
                    {
                        GetData.Reset_Data("Extract", "Secret");
                    }
                }                
            }
            public static bool Password(string passwd)
            {
                string x = Encoding.ASCII.GetString(((List<byte>)GetData.Extract[Data.Misc.DataPassword]).ToArray());
                if (Bifid_Cipher.Decrypt(x) == passwd) return true; else return false;
            }
            public static string IsType(string typefile)
            {
                string result = String.Empty;

                foreach (var x in Data.File_Extensions.Txt)
                {
                    if (typefile.Contains(x)) result = "Text Files";
                }

                foreach (var x in Data.File_Extensions.Image)
                {
                    if (typefile.Contains(x)) result = "Image Files";
                }

                foreach (var xx in Data.File_Extensions.Document)
                {
                    if (typefile.Contains(xx)) result = "Document Files";
                }

                foreach (var xxx in Data.File_Extensions.Other)
                {
                    if (typefile.Contains(xxx)) result = "Other Files";
                }

                return result;
            }
        }

        public class Embed
        {
            public static async void Starting(string type)
            {
                bool a = GetData.Embed.ContainsKey(Data.Misc.DataPixel);
                bool b = GetData.Embed.ContainsKey(Data.Misc.DataSecret);
                bool c = GetData.Embed.ContainsKey(Data.Misc.DataPassword);
                bool d = GetData.Embed.ContainsKey(Data.Misc.DataType);
                bool e = GetData.Embed.ContainsKey(Data.Misc.DataNameFile);
                bool f = GetData.Embed.ContainsKey(Data.Misc.DataExtension);

                switch (type)
                {
                    case "File":
                        if ((a == true) && (b == true) && (c == true) && (d == true) && (e == true) && (f == true))
                        {
                            Execute(type);
                            break;
                        }
                        else
                        {
                            break;
                        }
                    case "Message":
                        if ((d == false) && (e == false) && (f == false))
                        {
                            GetData.Embed.Add(Data.Misc.DataNameFile, Conversion.ToBinary("0", "String"));
                            GetData.Embed.Add(Data.Misc.DataExtension, Conversion.ToBinary(await Bifid_Cipher.Execute("Embed", ".txt", String.Empty), "String"));
                            GetData.Embed.Add(Data.Misc.DataType, Conversion.ToBinary("1", "String"));
                            Execute(type);
                            break;
                        }
                        else
                        {
                            Execute(type);
                            break;
                        }
                }
            }
            public static async void Execute(string type)
            {
                PopupDialog.Loading pl = new PopupDialog.Loading();
                pl.Show(true, Data.Misc.WorkingOnIt, String.Empty);
                var x = Task.Run(() => Run());
                var xx = await x;
                if (x.IsCompleted == true)
                {
                    pl.Show(false, String.Empty, String.Empty);
                    await Embed.Save(xx, type);
                }
            }
            public static async Task<Dictionary<string, object>> Run()
            {
                Dictionary<string, object> result = new Dictionary<string, object>();

                var dataPixel = (byte[])GetData.Embed[Data.Misc.DataPixel];

                var dataPasswd = (char[])GetData.Embed[Data.Misc.DataPassword];
                var dataType = (char[])GetData.Embed[Data.Misc.DataType];
                var dataNameFile = (char[])GetData.Embed[Data.Misc.DataNameFile];
                var dataExtension = (char[])GetData.Embed[Data.Misc.DataExtension];
                var dataSecret = (char[])GetData.Embed[Data.Misc.DataSecret];

                var l_dataPasswd = dataPasswd.Length;
                var l_dataType = dataType.Length;
                var l_dataNameFile = dataNameFile.Length;
                var l_dataExtension = dataExtension.Length;
                var l_dataSecret = dataSecret.Length;

                var l_data = l_dataPasswd + l_dataType + l_dataNameFile + l_dataExtension + l_dataSecret;
                char[] data = new char[l_data];

                Array.Copy(dataPasswd, data, l_dataPasswd);
                Array.Copy(dataType, 0, data, l_dataPasswd, l_dataType);
                Array.Copy(dataNameFile, 0, data, l_dataPasswd + l_dataType, l_dataNameFile);
                Array.Copy(dataExtension, 0, data, l_dataPasswd + l_dataType + l_dataNameFile, l_dataExtension);
                Array.Copy(dataSecret, 0, data, l_dataPasswd + l_dataType + l_dataNameFile + l_dataExtension, l_dataSecret);

                for (int i = 0; i < data.Length; i++)
                {
                    if (data[i] == 49 && ((byte)(dataPixel[i] % 2) == 1))
                    {
                        dataPixel[i] = dataPixel[i];
                    }

                    if (data[i] == 49 && ((byte)(dataPixel[i] % 2) == 0))
                    {
                        dataPixel[i] = (byte)(dataPixel[i] + 1);
                    }

                    if (data[i] == 48 && ((byte)(dataPixel[i] % 2) == 1))
                    {
                        dataPixel[i] = (byte)(dataPixel[i] - 1);
                    }

                    if (data[i] == 48 && ((byte)(dataPixel[i] % 2) == 0))
                    {
                        dataPixel[i] = dataPixel[i];
                    }
                }

                await Task.Delay(6000);

                result.Add(Data.Misc.DataPixel, dataPixel);
                result.Add(Data.Misc.DataPassword, l_dataPasswd);
                result.Add(Data.Misc.DataType, l_dataType);
                result.Add(Data.Misc.DataNameFile, l_dataNameFile);
                result.Add(Data.Misc.DataExtension, l_dataExtension);
                result.Add(Data.Misc.DataSecret, l_dataSecret);

                return result;
            }
            public static async Task Save(Dictionary<string,object> value, string type)
            {
                FileSavePicker fs = new FileSavePicker();
                fs.FileTypeChoices.Add("Stego Image Files", new List<string>() { ".png" });
                StorageFile sf = await fs.PickSaveFileAsync();
                if (sf != null)
                {
                    using (IRandomAccessStream ram = await sf.OpenAsync(FileAccessMode.ReadWrite))
                    {
                        BitmapEncoder encoder = await BitmapEncoder.CreateAsync(BitmapEncoder.PngEncoderId, ram);
                        encoder.SetPixelData(GetData.Decoder.BitmapPixelFormat, GetData.Decoder.BitmapAlphaMode, (uint)GetData.Decoder.PixelWidth, (uint)GetData.Decoder.PixelHeight, GetData.Decoder.DpiX, GetData.Decoder.DpiY, (byte[])value[Data.Misc.DataPixel]);
                        var prop = new List<KeyValuePair<string, BitmapTypedValue>>();

                        var desc = new BitmapTypedValue(String.Format("{0}|{1}|{2}|{3}|{4}", (int)value[Data.Misc.DataPassword], (int)value[Data.Misc.DataType], (int)value[Data.Misc.DataNameFile], (int)value[Data.Misc.DataExtension], (int)value[Data.Misc.DataSecret]), PropertyType.String);
                        prop.Add(new KeyValuePair<string, BitmapTypedValue>(Data.Misc.Secret, desc));

                        await encoder.BitmapProperties.SetPropertiesAsync(prop);
                        await encoder.FlushAsync();
                    }
                    switch (type)
                    {
                        case "File":
                            await PopupDialog.Show(Status.Success, Detail.Embed_File, Complete.Saved_StegoImage, Icon.Smile);
                            break;
                        case "Message":
                            await PopupDialog.Show(Status.Success, Detail.Embed_Message, Complete.Saved_StegoImage, Icon.Smile);
                            break;
                    }
                }
                else
                {
                    switch(type)
                    {
                        case "File":
                            await PopupDialog.Show(Status.Err, Detail.Embed_File, Err.NotSaved_StegoImage, Icon.Sad);
                            break;
                        case "Message":
                            await PopupDialog.Show(Status.Err, Detail.Embed_Message, Err.NotSaved_StegoImage, Icon.Sad);
                            break;
                    }

                }
            }
        }

        public class Extract
        {
            public static async void Starting(string type, string passwd)
            {
                if (GetData.SecretData.ContainsKey(Data.Misc.DataPassword) == false)
                {
                    switch(type)
                    {
                        case "STEG":
                            await PopupDialog.Show(Status.Err, Detail.Extract_FileMessage, Err.Invalid_Stego, Icon.Sad);
                            break;
                        case "CHK":
                            await PopupDialog.Show(Status.Err, Detail.Extract_Check, Err.Invalid_Stego, Icon.Sad);
                            break;
                    }
                }
                else
                {
                    Execute(type, passwd);  
                }
            }
            public static async void Execute(string type, string passwd)
            {
                PopupDialog.Loading pl = new PopupDialog.Loading();
                pl.Show(true, Data.Misc.WorkingOnIt, String.Empty);
                var x = Task.Run(() => GetValue());
                await x;
                if (x.IsCompleted == true)
                {
                    pl.Show(false, String.Empty, String.Empty);
                    await Run(type, passwd);
                }
            }
            public static async Task GetValue()
            {
                string dataPixel_str;
                List<byte> dataLSB = new List<byte>();
                List<char> dataPixel_char = new List<char>();
                int l_passwd = int.Parse((string)GetData.SecretData[Data.Misc.DataPassword]) / 8;
                int l_type = int.Parse((string)GetData.SecretData[Data.Misc.DataType]) / 8;
                int l_name = int.Parse((string)GetData.SecretData[Data.Misc.DataNameFile]) / 8;
                int l_ext = int.Parse((string)GetData.SecretData[Data.Misc.DataExtension]) / 8;
                int l_data = int.Parse((string)GetData.SecretData[Data.Misc.DataSecret]) / 8;

                for (int i = 0; i < ((byte[])GetData.Extract[Data.Misc.DataPixel]).Length; i++)
                {
                    if (((byte[])GetData.Extract[Data.Misc.DataPixel])[i] % 2 == 0) dataPixel_char.Add((char)48); else dataPixel_char.Add((char)49);
                }

                dataPixel_str = new string(dataPixel_char.ToArray());

                for (int i = 0; i < (dataPixel_str.Length / 8); ++i)
                {
                    dataLSB.Add(Convert.ToByte(dataPixel_str.Substring(8 * i, 8), 2));
                }

                GetData.Reset_Data("Extract", "Passwd");
                GetData.Extract.Add(Data.Misc.DataPassword, dataLSB.GetRange(0, l_passwd));
                GetData.Extract.Add(Data.Misc.DataType, dataLSB.GetRange(l_passwd, l_type));
                GetData.Extract.Add(Data.Misc.DataNameFile, dataLSB.GetRange(l_passwd + l_type, l_name));
                GetData.Extract.Add(Data.Misc.DataExtension, dataLSB.GetRange(l_passwd + l_type + l_name, l_ext));
                GetData.Extract.Add(Data.Misc.DataSecret, dataLSB.GetRange(l_passwd + l_type + l_name + l_ext, l_data));

                await Task.Delay(6000);
            }
            public static async Task Run(string type, string passwd)
            {
                if(Validate.Password(passwd) == false)
                {
                    await PopupDialog.Show(Status.Err, Detail.Extract_FileMessage, Err.Invalid_Passwd, Icon.Sad);
                }
                else
                {
                    if(Encoding.ASCII.GetString(((List<byte>)GetData.Extract[Data.Misc.DataType]).ToArray()) == "1")
                    {
                        switch(type)
                        {
                            case "STEG":
                                PopupDialog.Message pm = new PopupDialog.Message();
                                pm.Show();
                                break;
                            case "CHK":
                                break;
                        }
                    }
                    else
                    {
                        switch (type)
                        {
                            case "STEG":
                                await Save("File");
                                break;
                            case "CHK":
                                break;
                        }
                    }
                }
            }
            public static async Task Save(string type)
            {
                FileSavePicker fs = new FileSavePicker();
                string ext = Bifid_Cipher.Decrypt(Encoding.ASCII.GetString(((List<byte>)GetData.Extract[Data.Misc.DataExtension]).ToArray()));
                fs.FileTypeChoices.Add(Validate.IsType(ext), new List<string>() { ext });
                fs.SuggestedFileName = Bifid_Cipher.Decrypt(Encoding.ASCII.GetString(((List<byte>)GetData.Extract[Data.Misc.DataNameFile]).ToArray()));
                IStorageFile sf = await fs.PickSaveFileAsync();

                if (sf != null)
                {
                    switch (type)
                    {
                        case "File":
                            await FileIO.WriteBytesAsync(sf, ((List<byte>)GetData.Extract[Data.Misc.DataSecret]).ToArray());
                            break;
                        case "Message":
                            await FileIO.WriteTextAsync(sf, await Bifid_Cipher.Execute("Extract", Encoding.ASCII.GetString(((List<byte>)GetData.Extract[Data.Misc.DataSecret]).ToArray()), String.Empty));
                            break;
                    }
                    await PopupDialog.Show(Status.Success, Detail.Extract_FileMessage, Complete.Saved_SecretFile, Icon.Smile);
                }
                else
                {
                    await PopupDialog.Show(Status.Err, Detail.Extract_FileMessage, Err.NotSaved_SecretFile, Icon.Sad);
                }
            }
        }
    }
}