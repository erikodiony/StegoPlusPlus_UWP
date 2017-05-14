using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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

    class DataProses
    {
        public string KonversiMessage(string push_message)
        {
            string message = string.Empty;
            var ut8 = Encoding.UTF8;
            foreach(char ch in push_message)
            {
                message += Convert.ToString(ch,2).PadLeft(8, '0');
            }
            return message;
        }

    }
}
