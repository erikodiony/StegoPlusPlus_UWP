using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Windows.Graphics.Imaging;
using Windows.Storage;
using Windows.Storage.FileProperties;
using Windows.Storage.Pickers;
using Windows.Storage.Streams;
using Windows.UI.Xaml.Media.Imaging;

namespace StegoPlusPlus
{
    internal static class FileExtensions
    {
        public static readonly string[] Stego = new string[] { ".png" };
        public static readonly string[] Document = new string[] { ".doc", ".xls", ".ppt", ".docx", ".xlsx", ".pptx", ".pdf", ".txt" };
        public static readonly string[] Image = new string[] { ".jpg", ".gif", ".png" };
        public static readonly string[] Other = new string[] { ".mp3", ".mp4", ".zip", ".rar" };
        public static readonly string[] All = new string[] { ".doc", ".xls", ".ppt", ".docx", ".xlsx", ".pptx", ".pdf", ".txt", ".jpg", ".gif", ".png", ".mp3", ".mp4", ".zip", ".rar" };
    }

    internal static class propImage
    {
        public static readonly string Size = "System.Size";
        public static readonly string Dimensions = "System.Image.Dimensions";
        public static readonly string[] All = new string[] { Size, Dimensions };
    }

    internal static class HeaderPage
    {
        public static readonly string HomePage = "WELCOME";
        public static readonly string EmbedPage = "Menu yang berfungsi untuk menjalankan fungsi Embed untuk penyisipan File (Embed File) beserta Embed untuk penyisipan Pesan Rahasia (Embed Message)";
        public static readonly string ExtractPage = "Menu yang berfungsi untuk menjalankan fungsi Ekstrak untuk File / Pesan Rahasia (Extract File / Message) beserta untuk pengecekan File Stego yang masih baik / bisa digunakan (Check Stego Healthy)";
        public static readonly string SettingsPage = "Menu yang berfungsi untuk mengganti Warna Latar Belakang (Change Background / Theme) beserta mengganti Efek Transisi Halaman (Change Effect Transition)";
        public static readonly string AboutPage = "Menu yang berisi tentang Detail Aplikasi (App Detail) beserta Info tentang Creator (About Me)";
    }

    internal static class NotifyDataText
    {
        public static readonly string OK_Button = "OK";
        public static readonly string Cancel_Button = "Cancel";
        public static readonly string Err_Input_Null_Embed_Msg_msg = "Field ''Insert Text/Message'' is empty !\nCan't Saving Text/Message...";
        public static readonly string Err_Input_Null_Embed_Msg_pwd = "Field ''Insert Password'' is empty !\nCan't Saving Password...";
        public static readonly string Err_Input_Null_Embed_File_pwd = "Field ''Insert Password'' is empty !\nCan't Saving Password...";
        public static readonly string Clear_Input_Embed_Msg_msg = "Field ''Insert Text/Message'' was Cleared !\nProcess Successfully...";
        public static readonly string Clear_Input_Embed_Msg_pwd = "Field ''Insert Password'' was Cleared !\nProcess Successfully...";
        public static readonly string Clear_Input_Embed_File_pwd = "Field ''Insert Password'' was Cleared !\nProcess Successfully...";
        public static readonly string Saving_Header_Notify_Embed_Msg_msg = "Input Text / Message (saved)";
        public static readonly string Saving_Header_Notify_Embed_Msg_pwd = "Input Password (saved)";
        public static readonly string Saving_Header_Notify_Embed_File_pwd = "Input Password (saved)";
        public static readonly string Clearing_Header_Notify_Embed_Msg_msg = "Input Text / Message";
        public static readonly string Clearing_Header_Notify_Embed_Msg_pwd = "Input Password";
        public static readonly string Clearing_Header_Notify_Embed_File_pwd = "Input Password";
        public static readonly string Dialog_Exec_Footer_Menu_Null = "Some field is empty or input not saved !\nPlease check again...";
        public static readonly string Dialog_Clear_Footer_Menu_Null = "All field was Cleared !\nProcess Successfully...";
    }

    class DataProcess
    {
        public BitmapDecoder decoder;
        public IRandomAccessStream strm_Cover;
        char[] pwd_encoded; //Password Asli
        char[] pwd_crypt_encoded; // Password Crypt
        char[] msg_encoded; //Text/Message
        //char[] ftype_encoded; //Extention Of File Hiding
        //char[] def_encoded; //Type of Hide (Message or File)

        public char[] Convert_Passwd(string passwd)
        {
            string pwd = string.Empty;
            foreach (char x in passwd)
            {
                pwd += Convert.ToString(x, 2).PadLeft(8, '0');
            }

            //string[] pwd_encoded = pwd.ToCharArray().Select(x => x.ToString()).ToArray();
            pwd_encoded = pwd.ToCharArray();
            return pwd_encoded;
        }

        public char[] Convert_Passwd_Encrypt(string passwd_crypt)
        {
            string pwd_crypt = string.Empty;
            foreach (char x in passwd_crypt)
            {
                pwd_crypt += Convert.ToString(x, 2).PadLeft(8, '0');
            }

            pwd_crypt_encoded = pwd_crypt.ToCharArray();
            return pwd_crypt_encoded;
        }

        public char[] Convert_Message_or_Text(string message)
        {
            string msg = string.Empty;
            foreach (char x in message)
            {
                msg += Convert.ToString(x, 2).PadLeft(8, '0');
            }

            msg_encoded = msg.ToCharArray();
            return msg_encoded;
        }

        public char[] Convert_FileType(string ext)
        {
            string ftype = string.Empty;
            foreach (char x in ext)
            {
                ftype += Convert.ToString(x, 2).PadLeft(8, '0');
            }

            char[] ftype_encoded = ftype.ToCharArray();
            return ftype_encoded;
        }

        //public char[] Convert_Definition(string definition)
        //{
        //    string def = string.Empty;
        //    foreach (char x in definition)
        //    {
        //        def += Convert.ToString(x, 2).PadLeft(8, '0');
        //    }

        //    char[] def_encoded = def.ToCharArray();
        //    return def_encoded;
        //}

        public async Task<char[]> Convert_FileHiding_to_Byte(StorageFile fileHiding)
        {
            char[] bin = null;
            byte[] bin_array = null;
            string bin_string = null;

            using (Stream st = await fileHiding.OpenStreamForReadAsync())
            {
                using (BinaryReader binaryReader = new BinaryReader(st))
                {
                    bin_array = binaryReader.ReadBytes((int)st.Length).ToArray();
                }

                foreach (byte x in bin_array)
                {
                    bin_string += Convert.ToString(x, 2).PadLeft(8, '0');
                }
                bin = bin_string.ToCharArray();
            }
            return bin;
        }

        public async Task<byte[]> Convert_FileCover_to_Byte(StorageFile fileCover)
        {
            byte[] bin;
            using (strm_Cover = await fileCover.OpenAsync(FileAccessMode.ReadWrite))
            {
                decoder = await BitmapDecoder.CreateAsync(strm_Cover);
                bin = (await decoder.GetPixelDataAsync()).DetachPixelData();
            }

            //System.Diagnostics.Debug.WriteLine(bin.Length);

            int zxc = -1;
            foreach (var x in bin)
            {
                System.Diagnostics.Debug.WriteLine("BARIS {0} || {1}", ++zxc, x);
            }


            return bin;
        }

        public byte[] RUN_STEG(char[] fileOrMessage, byte[] coverImage, char[] passwd, char[] passwd_encrypt, char[] extension, char[] def)
        {
            byte[] steg_result = new byte[coverImage.Length];
            char[] nw = new char[coverImage.Length];

            for (int i = 0; i < nw.Length; i++)
            {
                nw[i] = (char)49;
            }

            //Structure of Hiding Data {pwd_encoded + [0000] + pwd + [0001] + file||message + [0010] + extention + [0011] + data + [0100]}            
            //pwd_encoded
            //[0000]
            //pwd
            //[0001]
            //file||message
            //[0010]
            //extention
            //[0011]
            //data
            //[0100]

            char[] limiter = new char[] { (char)48, (char)48, (char)48, (char)48, (char)48, (char)48, (char)48, (char)48 };
            char[] limiter_2 = new char[] { (char)48, (char)48, (char)48, (char)48, (char)48, (char)48, (char)48, (char)49 };
            char[] limiter_3 = new char[] { (char)48, (char)48, (char)48, (char)48, (char)48, (char)48, (char)49, (char)48 };
            char[] limiter_4 = new char[] { (char)48, (char)48, (char)48, (char)48, (char)48, (char)48, (char)49, (char)49 };
            char[] limiter_5 = new char[] { (char)48, (char)48, (char)48, (char)48, (char)48, (char)49, (char)48, (char)48 };

            int length_encrypt = (pwd_crypt_encoded.Length + 8 + pwd_encoded.Length + 8 + def.Length + 8 + extension.Length + 8 + fileOrMessage.Length + 8 );
            char[] all_encrypt = new char[length_encrypt];

            Array.Copy(pwd_crypt_encoded, all_encrypt, pwd_crypt_encoded.Length);
            Array.Copy(limiter, 0, all_encrypt, pwd_crypt_encoded.Length, limiter.Length);
            Array.Copy(pwd_encoded, 0, all_encrypt, limiter.Length + pwd_crypt_encoded.Length, pwd_encoded.Length);
            Array.Copy(limiter_2, 0, all_encrypt, pwd_encoded.Length + limiter.Length + pwd_crypt_encoded.Length, limiter_2.Length);
            Array.Copy(def, 0, all_encrypt, limiter_2.Length + pwd_encoded.Length + limiter.Length + pwd_crypt_encoded.Length, def.Length);
            Array.Copy(limiter_3, 0, all_encrypt, def.Length + limiter_2.Length + pwd_encoded.Length + limiter.Length + pwd_crypt_encoded.Length, limiter_3.Length);
            Array.Copy(extension, 0, all_encrypt, limiter_3.Length + def.Length + limiter_2.Length + pwd_encoded.Length + limiter.Length + pwd_crypt_encoded.Length, extension.Length);
            Array.Copy(limiter_4, 0, all_encrypt, extension.Length + limiter_3.Length + def.Length + limiter_2.Length + pwd_encoded.Length + limiter.Length + pwd_crypt_encoded.Length, limiter_4.Length);
            Array.Copy(fileOrMessage, 0, all_encrypt, limiter_4.Length + extension.Length + limiter_3.Length + def.Length + limiter_2.Length + pwd_encoded.Length + limiter.Length + pwd_crypt_encoded.Length, fileOrMessage.Length);
            Array.Copy(limiter_5, 0, all_encrypt, fileOrMessage.Length + limiter_4.Length + extension.Length + limiter_3.Length + def.Length + limiter_2.Length + pwd_encoded.Length + limiter.Length + pwd_crypt_encoded.Length, limiter_5.Length);

            for (int i = 0; i < all_encrypt.Length; i++)
            {
                if (all_encrypt[i] == 49 && ((byte)(coverImage[i] % 2) == 1))
                {
                    coverImage[i] = (byte)(coverImage[i]);
                }

                if (all_encrypt[i] == 49 && ((byte)(coverImage[i] % 2) == 0))
                {
                    coverImage[i] = (byte)(coverImage[i] + 1);
                }

                if (all_encrypt[i] == 48 && ((byte)(coverImage[i] % 2) == 1))
                {
                    coverImage[i] = (byte)(coverImage[i] - 1);
                }

                if (all_encrypt[i] == 48 && ((byte)(coverImage[i] % 2) == 0))
                {
                    coverImage[i] = (byte)(coverImage[i]);
                }
            }

            int g = -1;
            foreach (var x in all_encrypt)
            {
               System.Diagnostics.Debug.WriteLine("{1} FILE BINARY == {0}", x, ++g);
            }

            steg_result = coverImage;


            string xd = new string(all_encrypt);
            System.Diagnostics.Debug.WriteLine(xd);

            List<string> groups = (from Match m in Regex.Matches(xd, @"\d{8}") select m.Value).ToList();

            int eff = 0;
            foreach (string v in groups)
            {
                System.Diagnostics.Debug.WriteLine("{0} || {1}", ++eff, v);
            }

            return steg_result;
        }              
    }
}
