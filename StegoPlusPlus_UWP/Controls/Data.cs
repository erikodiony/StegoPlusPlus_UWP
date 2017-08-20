using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StegoPlusPlus.Controls
{
    class Data
    {
        #region FileExtensions
        public static class File_Extensions
        {
            public static readonly string[] Png = new string[] { ".png" };
            public static readonly string[] Txt = new string[] { ".txt" };
            public static readonly string[] Document = new string[] { ".doc", ".xls", ".ppt", ".docx", ".xlsx", ".pptx", ".pdf" };
            public static readonly string[] Image = new string[] { ".jpg", ".gif", ".png" };
            public static readonly string[] Other = new string[] { ".mp3", ".mp4", ".zip", ".rar" };
            public static readonly string[] All = new string[] { ".doc", ".xls", ".ppt", ".docx", ".xlsx", ".pptx", ".pdf", ".jpg", ".gif", ".png", ".mp3", ".mp4", ".zip", ".rar" };
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
                    public static readonly string Result = "RESULT";
                }
                public static class Detail
                {
                    public static readonly string Embed_File = "Embed File";
                    public static readonly string Embed_Message = "Embed Message";
                    public static readonly string Extract_FileMessage = "Extract File / Message";
                    public static readonly string Extract_Check = "Check Stego Info";
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
                public static readonly string Clear_Input_Passwd = "Field ''Input Password'' was Cleared !\nProcess Successfully...";
                public static readonly string Clear_Input_Message = "Field ''Input Text/Message'' was Cleared !\nProcess Successfully...";
                public static readonly string Saved_StegoImage = "Stego Image was Saved !\nProcess Successfully...";
                public static readonly string Saved_SecretFile = "Secret File was Saved !\nProcess Successfully...";
            }
            #endregion
            #region Popup Error
            public static class Err
            {
                public static readonly string Input_isNull = "Some field is empty or input not saved !\nPlease check again...";
                public static readonly string Input_Invalid_Passwd = "Password Invalid !\nCan't Saving Password...";
                public static readonly string Input_Invalid_Message = "Text/Message Invalid !\nCan't Saving Text/Message...";
                public static readonly string Input_Empty_Passwd = "Field ''Input Password'' is empty !\nCan't Saving Password...";
                public static readonly string Input_Empty_Message = "Field ''Input Text/Message'' is empty !\nCan't Saving Text/Message...";
                public static readonly string Invalid_Stego = "Invalid Image Stego !\nCan't Extract File / Message...";
                public static readonly string Invalid_Passwd = "Password Incorrect !\nCan't Extract File / Message...";
                public static readonly string Invalid_32bitDepth = "Only Image Cover 32BitDepth was supported !\nPlease check again...";
                public static readonly string Overload_Size = "Overload size quota of File !\nPlease check again...";
                public static readonly string Null_Size = "Field ''Choose Image Cover'' must be filled !\nPlease check again...";
                public static readonly string Replace_Message = "Field ''Input Text/Message'' is Saved !\nClick 'Clear' to Replace with new Text/Message...";
                public static readonly string NotSaved_StegoImage = "Stego Image not saved !\nProcess cancelling...";
                public static readonly string NotSaved_SecretFile = "Secret File not saved !\nProcess cancelling...";
                public static readonly string More150Kb = "Size more than 150Kb !\nCan't Saving File...";
            }
            #endregion
            #region Popup Confirm
            public static class Confirm
            {
                public static readonly string isExecute = "Confirm to Execute ?\nClick 'OK' to continue...";
                public static readonly string isLargeFile = "Large size will take a several minute !\nClick 'OK' to continue...";
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
            public static readonly string SettingsPage = "Menu yang berfungsi untuk mengganti Warna Latar Belakang (Change Background) beserta mengganti Efek Transisi Halaman (Change Transition Effect)";
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
            public static readonly string placeholder = "Secret Password (Case Sensitive)";
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
            public static readonly string title = "Choose Image Stego";
            public static readonly string subtitle = "(Format ekstensi file gambar yang didukung *.png)";
            public static readonly string button = "Choose Image";
            public static readonly string head_default = "";
            public static readonly string head_save = "";
            public static readonly string picker_status = "No Image";
            public static readonly string picker_path = "Path : ";
            public static readonly string picker_size = "Size : ";
            public static readonly string picker_dimension = "Dimension : ";
        }
        #endregion

        #region Property About
        public static class Prop_AppDetail
        {
            public static readonly string title = "Stego++ with Password";
            public static readonly string version = "Version 1.1 (Build 201708)";
            public static readonly string source_code = "Source Code (Github)";
            public static readonly string source_code_link = "http://github.com/Erikodiony/StegoPlusPlus";
            public static readonly string bug_support = "Bug / Support (Twitter)";
            public static readonly string bug_support_link = "https://twitter.com/erikodiony";
        }
        public static class Prop_AboutMe
        {
            public static readonly string title = "''Make your dreams come true with working hardly and tawakkal''";
            public static readonly string head1 = "Biodata";
            public static readonly string name = "Erikodiony Ariessa Wahyudi";
            public static readonly string birth = "Balikpapan, 24 September 1994";
            public static readonly string domicile = "Kertosono, Nganjuk";
            public static readonly string head2 = "Riwayat Pendidikan";
            public static readonly string school_tk = "TK Pertiwi Kudu";
            public static readonly string school_sd = "SD Negeri 1 Kudu";
            public static readonly string school_smp = "SMP Negeri 1 Kertosono";
            public static readonly string school_smk = "SMK Negeri 1 Kertosono (Teknik Otomasi Industri)";
            public static readonly string university = "Universitas Nusantara PGRI Kediri (Teknik Informatika)";
        }
        #endregion

        #region Property Misc
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
            public static readonly string Text = "Text";

            public static readonly string PleaseWait = "Please Wait...";
            public static readonly string PleaseWaitDetail = "This process might take a while or several minutes...";
            public static readonly string WorkingOnIt = "Working on it...";

            public static readonly string DataPixel = "DataPixel";
            public static readonly string DataNameFile = "DataNameFile";
            public static readonly string DataPassword = "DataPassword";
            public static readonly string DataType = "DataType";
            public static readonly string DataExtension = "DataExtension";
            public static readonly string DataSecret = "DataSecret";

            public static readonly string Secret = "/tEXt/{str=Description}";

            public static readonly string ToastData = "ToastData";

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
        #endregion
    }
}
