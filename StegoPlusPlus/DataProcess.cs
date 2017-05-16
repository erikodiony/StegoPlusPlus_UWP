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
using Windows.UI.Xaml.Media.Imaging;

namespace StegoPlusPlus
{
    internal static class FileExtensions
    {
        public static readonly string[] Stego = new string[] { ".jpg", ".png" };
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
        public string[] Convert_Passwd(string passwd)
        {
            string pwd = string.Empty;
            foreach(char x in passwd)
            {
                pwd += Convert.ToString(x,2).PadLeft(8, '0');
            }

            string[] pwd_encoded = pwd.ToCharArray().Select(x => x.ToString()).ToArray();
            return pwd_encoded;
        }

        public char[] Convert_Message_or_Text(string message)
        {
            string msg = string.Empty;
            foreach (char x in message)
            {
                msg += Convert.ToString(x, 2).PadLeft(8, '0');
            }

            char[] msg_encoded = msg.ToCharArray();
            return msg_encoded;
        }

        public async Task<char[]> Convert_FileHiding_to_Byte(StorageFile fileHiding)
        {
            char[] bin;
            byte[] bin_array;
            string bin_string = String.Empty;
            using (Stream st = await fileHiding.OpenStreamForReadAsync())
            {
                using (var memst = new MemoryStream())
                {
                    st.CopyTo(memst);
                    bin_array = memst.ToArray();

                    foreach (byte x in bin_array)
                    {
                        bin_string += Convert.ToString(x, 2).PadLeft(8, '0');
                    }
                    bin = bin_string.ToCharArray();
                }
            }
            return bin;
        }

        public async Task<byte[]> Convert_FileCover_to_Byte(StorageFile fileCover)
        {
            byte[] bin;
            using (IRandomAccessStream stream = await fileCover.OpenAsync(FileAccessMode.Read))
            {
                BitmapDecoder decoder = await BitmapDecoder.CreateAsync(stream);
                bin = (await decoder.GetPixelDataAsync()).DetachPixelData();
            }
            return bin;
        }


        public string RUN_STEG(char[] fileOrMessage, byte[] coverImage, string[]passwd)
        {
            string notify = String.Empty;
            byte[] steg_in = new byte[fileOrMessage.Length];

            for (int i = 0; i < fileOrMessage.Length; i++)
            {
                if (fileOrMessage[i] == 0 && coverImage[i] % 2 == 0)
                {
                    byte x = 0;
                    steg_in[i] = (byte)(coverImage[i] + x);
                }

                if (fileOrMessage[i] == 0 && coverImage[i] % 2 == 1)
                {
                    byte x = 1;
                    steg_in[i] = (byte)(coverImage[i] - x);
                }

                if (fileOrMessage[i] == 1 && coverImage[i] % 2 == 0)
                {
                    byte x = 1;
                    steg_in[i] = (byte)(coverImage[i] + x);
                }

                if (fileOrMessage[i] == 1 && coverImage[i] % 2 == 1)
                {
                    byte x = 0;
                    steg_in[i] = (byte)(coverImage[i] + x);
                }
            }

            byte[] steg_result = new byte[coverImage.Length];
            Array.Copy(steg_in, 0, steg_result, 0, steg_in.Length);
            Array.Copy(coverImage, steg_in.Length, steg_result, steg_in.Length, coverImage.Length - fileOrMessage.Length);

            return notify;
        }
              
    }
}
