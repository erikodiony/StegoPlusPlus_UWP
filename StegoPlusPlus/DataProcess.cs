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
    }

    class DataProcess
    {
        public char[] KonversiBinary(string push_message)
        {
            string message = string.Empty;
            foreach(char ch in push_message)
            {
                message += Convert.ToString(ch,2).PadLeft(8, '0');
            }

            char[] msg = message.ToCharArray();
            return msg;
        }
              
    }
}
