﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StegoPlusPlus
{
    class Data
    {
        #region FileExtensions
        public static class File_Extensions
        {
            public static readonly string[] Png = new string[] { ".png" };
            public static readonly string[] Txt = new string[] { ".txt" };
            public static readonly string[] Document = new string[] { ".doc", ".xls", ".ppt", ".docx", ".xlsx", ".pptx", ".pdf", ".txt" };
            public static readonly string[] Image = new string[] { ".jpg", ".gif", ".png" };
            public static readonly string[] Other = new string[] { ".mp3", ".mp4", ".zip", ".rar" };
            public static readonly string[] All = new string[] { ".doc", ".xls", ".ppt", ".docx", ".xlsx", ".pptx", ".pdf", ".txt", ".jpg", ".gif", ".png", ".mp3", ".mp4", ".zip", ".rar" };
        }
        #endregion

        #region Property Popup
        public static class Prop_Popup
        {
            #region Popup Title
            public static class Title
            {
                public static class Status
                {
                    public static readonly string Success = "SUCCESS";
                    public static readonly string Err = "ERROR";
                    public static readonly string Confirm = "CONFIRMATION";
                }
                public static class Detail
                {
                    public static readonly string Embed_File = "Embed File";
                    public static readonly string Embed_Message = "Embed Message";
                    public static readonly string Extract_File = "Extract File";
                    public static readonly string Extract_Message = "Extract Message";
                    public static readonly string Insert_File = "Choose File";
                    public static readonly string Insert_Password = "Insert Password";
                    public static readonly string Insert_Message = "Choose Text / Message";
                    public static readonly string Image_Cover = "Choose Image Cover";
                }
                public static class Icon
                {
                    public static readonly string Smile = "J";
                    public static readonly string Sad = "L";
                    public static readonly string Flat = "K";
                }
            }
            #endregion
            #region Popup Complete
            public static class Complete
            {
                public static readonly string Clear_All = "All field was Cleared !\nProcess Successfully...";
                public static readonly string Clear_Input_Passwd = "Field ''Insert Password'' was Cleared !\nProcess Successfully...";
                public static readonly string Clear_Input_Message = "Field ''Insert Text/Message'' was Cleared !\nProcess Successfully...";
            }
            #endregion
            #region Popup Error
            public static class Err
            {
                public static readonly string Input_isNull = "Some field is empty or input not saved !\nPlease check again...";
                public static readonly string Input_Invalid_Passwd = "Password Invalid !\nCan't Saving Password...";
                public static readonly string Input_Invalid_Message = "Text/Message Invalid !\nCan't Saving Text/Message...";
                public static readonly string Input_Empty_Passwd = "Field ''Insert Password'' is empty !\nCan't Saving Password...";
                public static readonly string Input_Empty_Message = "Field ''Insert Text/Message'' is empty !\nCan't Saving Text/Message...";
                public static readonly string Invalid_Stego = "Invalid Image Stego !\nCan't Extract File / Message...";
                public static readonly string Invalid_Passwd = "Password Incorrect !\nCan't Extract File / Message...";
                public static readonly string Invalid_32bitDepth = "Only Image Cover 32BitDepth was supported !\nPlease check again...";
                public static readonly string Overload_Size = "Overload size quota of File Hiding !\nCan't Saving File Hiding...";
            }
            #endregion
            #region Popup Confirm
            public static class Confirm
            {
                public static readonly string isExecute = "Confirm to Execute ?\nClick 'OK' to continue...";
                public static readonly string isSave_EmbedFile = "Embedding File was Completed !\nClick 'OK' to saving Stego Image...";
                public static readonly string isSave_EmbedMessage = "Embedding Text/Message was Completed !\nClick 'OK' to saving Stego Image...";
                public static readonly string isSave_ExtractFile = "Extracting File was Completed !\nClick 'OK' to saving Secret File...";
                public static readonly string isSave_ExtractMessage = "Extracting Message was Completed !\nClick 'OK' to view Secret Text/Message...";
            }
            #endregion
        }
        #endregion

        #region Property Page
        public static class Prop_Page
        {
            public static readonly string HomePage = "WELCOME";
            public static readonly string EmbedPage = "Menu yang berfungsi untuk menjalankan fungsi Embed untuk penyisipan File (Embed File) beserta Embed untuk penyisipan Pesan Rahasia (Embed Message)";
            public static readonly string ExtractPage = "Menu yang berfungsi untuk menjalankan fungsi Ekstrak untuk File / Pesan Rahasia (Extract File / Message) beserta untuk pengecekan File Stego yang masih baik / bisa digunakan (Check Stego Healthy)";
            public static readonly string SettingsPage = "Menu yang berfungsi untuk mengganti Warna Latar Belakang (Change Background / Theme) beserta mengganti Efek Transisi Halaman (Change Transition Effect)";
            public static readonly string AboutPage = "Menu yang berisi tentang Detail Aplikasi (App Detail) beserta Info tentang Creator (About Me)";
        }
        #endregion

        #region Property FilePicker
        public static class Prop_File_Picker
        {
            public static readonly string Size = "System.Size";
            public static readonly string Dimensions = "System.Image.Dimensions";
            public static readonly string BitDepth = "System.Image.BitDepth";
            public static readonly List<string> All = new List<string>() { Size, Dimensions, BitDepth };
        }
        #endregion
     
        #region Property Button
        public static class Prop_Button
        {
            public static readonly string OK = "OK";
            public static readonly string Cancel = "Cancel";
            public static readonly string Save = "Save";
            public static readonly string Clear = "Clear";
            public static readonly string ClearAll = "Clear All";
            public static readonly string Execute = "Execute";
        }
        #endregion

        #region Property Passwd
        public static class Prop_Passwd
        {
            public static readonly string title = "Insert Password";
            public static readonly string subtitle = "(Mendukung Input huruf / angka dan beberapa simbol)";
            public static readonly string placeholder = "Secret Password";
            public static readonly string head_default = "Input Password";
            public static readonly string head_save = "Input Password (Saved & Encrypted)";
        }
        #endregion

        #region Property SecretMessage
        public static class Prop_Secret_Message
        {
            public static readonly string title = "Choose Text / Message";
            public static readonly string subtitle = "(Mendukung Input huruf / angka dan beberapa simbol)";
            public static readonly string subtitle2 = "(Mendukung Input dari File / Input secara Manual)";
            public static readonly string placeholder = "Secret Text / Message";
            public static readonly string head_default = "Input Text / Message";
            public static readonly string head_save = "Input Text / Message (Saved & Encrypted)";
            public static readonly string button = "Choose Text";
            public static readonly string counter = "Counter : ";
            public static readonly string picker_status = "No Text";
            public static readonly string picker_path = "Path : ";
            public static readonly string picker_size = "Size : ";
            public static readonly string picker_type = "Type : ";
        }
        #endregion

        #region Property SecretFile
        public static class Prop_Secret_File
        {
            public static readonly string title = "Choose File";
            public static readonly string subtitle = "(Mendukung format File Dokumen, Gambar, dll)";
            public static readonly string picker_status = "No File";
            public static readonly string picker_path = "Path : ";
            public static readonly string picker_size = "Size : ";
            public static readonly string picker_type = "Type : ";
            public static readonly string button = "Choose File";
        }
        #endregion

        #region Property CoverImage
        public static class Prop_Image_Cover
        {
            public static readonly string title = "Choose Image Cover";
            public static readonly string subtitle = "(Format ekstensi file gambar yang didukung *.png)";
            public static readonly string button = "Choose Image";
            public static readonly string picker_status = "No Image";
            public static readonly string picker_path = "Path : ";
            public static readonly string picker_size = "Size : ";
            public static readonly string picker_dimension = "Dimension : ";
            public static readonly string picker_eta_msg = "Text/Message can hide : ";
            public static readonly string picker_eta_file = "File can hide : ";
        }
        #endregion

        #region Property StegoImage
        public static class Prop_Image_Stego
        {
            public static readonly string title = "";
            public static readonly string subtitle = "";
            public static readonly string placeholder = "";
            public static readonly string head_default = "";
            public static readonly string head_save = "";
        }
        #endregion

        public static class Misc
        {
            public static readonly string Name = "Name";
            public static readonly string Path = "Path";
            public static readonly string Size = "Size";
            public static readonly string Dimensions = "Dimensions";
            public static readonly string BitDepth = "BitDepth";
            public static readonly string Pixel = "Pixel";
            public static readonly string Eta = "Eta";
            public static readonly string Type = "Type";
            public static readonly string Icon = "Icon";
            public static readonly string DataPixel = "DataPixel";
            public static readonly char[] Character = { 'A', 'a', 'B', 'b', 'C', 'c', 'D', 'd', 'E', 'e', 'F', 'f', 'G', 'g', 'H', 'h', 'I', 'i', 'J', 'j', 'K', 'k', 'L', 'l', 'M', 'm', 'N', 'n', 'O', 'o', 'P', 'p', 'Q', 'q', 'R', 'r', 'S', 's', 'T', 't', 'U', 'u', 'V', 'v', 'W', 'w', 'X', 'x', 'Y', 'y', 'Z', 'z', '0', '1', '2', '3', '4', '5', '6', '7', '8', '9', (char)32, (char)33, (char)34, (char)35, (char)36, (char)37, (char)38, (char)39, (char)40, (char)41, (char)42, (char)43, (char)44, (char)45, (char)46, (char)47, (char)58, (char)59, (char)60, (char)61, (char)62, (char)63, (char)64, (char)91, (char)92, (char)93, (char)94, (char)95, (char)96, (char)123, (char)124, (char)125, (char)126, (char)9, (char)10, (char)11, (char)12, (char)13 };
            public static readonly char[,] Matrix = new char[,]
            {
                { 'A' , 'a' , 'B' , 'b' , 'C' , 'c' , 'D' , 'd' , 'E' , 'e' },
                { 'F' , 'f' , 'G' , 'g' , 'H' , 'h' , 'I' , 'i' , 'J' , 'j' },
                { 'K' , 'k' , 'L' , 'l' , 'M' , 'm' , 'N' , 'n' , 'O' , 'o' },
                { 'P' , 'p' , 'Q' , 'q' , 'R' , 'r' , 'S' , 's' , 'T' , 't' },
                { 'U' , 'u' , 'V' , 'v' , 'W' , 'w' , 'X' , 'x' , 'Y' , 'y' },
                { 'Z' , 'z' , '0' , '1' , '2' , '3' , '4' , '5' , '6' , '7' },
                { '8' , '9' , (char)32 , (char)33, (char)34, (char)35, (char)36, (char)37, (char)38, (char)39 },
                { (char)40 , (char)41, (char)42, (char)43, (char)44, (char)45, (char)46, (char)47 , (char)58, (char)59 },
                { (char)60 , (char)61, (char)62, (char)63, (char)64, (char)91, (char)92, (char)93 , (char)94, (char)95 },
                { (char)96 , (char)123, (char)124, (char)125, (char)126, (char)9, (char)10, (char)11 , (char)12, (char)13 }
            };
        }
    }
}