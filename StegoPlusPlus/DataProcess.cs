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
        public static readonly string Dialog_Exec_Footer_Menu_Confirm = "Confirm to Execute ?\nClick 'OK' to continue...";
        public static readonly string Dialog_Exec_Footer_Menu_Null = "Some field is empty or input not saved !\nPlease check again...";
        public static readonly string Dialog_Clear_Footer_Menu_Null = "All field was Cleared !\nProcess Successfully...";

        //EMBED MENU
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

        //EXTRACT MENU
        public static readonly string Saving_Header_Notify_Extract_File_pwd = "Input Password (saved)";
        public static readonly string Err_Input_Null_Extract_File_pwd = "Field ''Insert Password'' is empty !\nCan't Saving Password...";
        public static readonly string Clearing_Header_Notify_Extract_File_pwd = "Input Password";
        public static readonly string Clear_Input_Extract_File_pwd = "Field ''Insert Password'' was Cleared !\nProcess Successfully...";
        public static readonly string Notify_Extract_Menu_Invalid_File = "Invalid Image Stego !\nCan't Extract File / Message...";
        public static readonly string Notify_Extract_Menu_Invalid_Passwd = "Password Incorrect !\nCan't Extract File / Message...";
    }

    class DataProcess
    {
        public BitmapDecoder decoder;
        public IRandomAccessStream strm_Cover;
        char[] pwd_encoded; //Password Input
        char[] pwd_crypt_encoded; // Password Crypt
        char[] msg_encoded; //Text/Message



        //BEGIN
        //GET PIXEL DATA
        //--------------------------------------------------------------------------------//

        public async Task<byte[]> Convert_FileImage_to_Byte(StorageFile fileCover_or_fileSteg)
        {
            byte[] bin;
            using (strm_Cover = await fileCover_or_fileSteg.OpenAsync(FileAccessMode.ReadWrite))
            {
                decoder = await BitmapDecoder.CreateAsync(strm_Cover);
                bin = (await decoder.GetPixelDataAsync()).DetachPixelData();
            }

            int zxc = -1;
            foreach (var x in bin)
            {
                //System.Diagnostics.Debug.WriteLine("BARIS {0} || {1}", ++zxc, x);
            }
            return bin;
        }

        //--------------------------------------------------------------------------------//
        //GET PIXEL DATA
        //END

        
        //CONTROL FOR ENCRYPT
        //BEGIN

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

        public async Task<char[]> Convert_FileHiding_to_Byte(StorageFile fileHiding)
        {            
            using (Stream st = await fileHiding.OpenStreamForReadAsync())
            {
                byte[] bin_array;
                string bin_string = String.Empty;
                using (BinaryReader binaryReader = new BinaryReader(st))
                {
                    bin_array = binaryReader.ReadBytes((int)st.Length).ToArray();
                }

                foreach (byte x in bin_array)
                {
                    bin_string += Convert.ToString(x, 2).PadLeft(8, '0');
                }
                char[] bin = bin_string.ToCharArray();

                foreach(var t in bin_array)
                {
                    //System.Diagnostics.Debug.WriteLine(t);
                }

                return bin;
            }
        }

        public byte[] RUN_STEG(char[] fileOrMessage, byte[] coverImage, char[] passwd, char[] passwd_encrypt, char[] extension, char[] def)
        {
            byte[] steg_result = new byte[coverImage.Length];

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

            int length_encrypt = (pwd_crypt_encoded.Length + 8 + pwd_encoded.Length + 8 + def.Length + 8 + extension.Length + 8 + fileOrMessage.Length + 8);
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

            //int g = -1;
            //foreach (var x in all_encrypt)
            //{
            //    System.Diagnostics.Debug.WriteLine("{1} FILE BINARY == {0}", x, ++g);
            //}

            steg_result = coverImage;


            //string xd = new string(all_encrypt);
            //System.Diagnostics.Debug.WriteLine(xd);

            //List<string> groups = (from Match m in Regex.Matches(xd, @"\d{8}") select m.Value).ToList();

            //int eff = 0;
            //foreach (string v in groups)
            //{
            //    System.Diagnostics.Debug.WriteLine("{0} || {1}", ++eff, v);
            //}

            return steg_result;
        }

        //CONTROL FOR ENCRYPT
        //END


        //--------------------------------------------------------------------------------//


        //CONTROL FOR DECRYPT
        //BEGIN

        public byte[] passwd_encrypt;
        public byte[] passwd_input;
        public byte[] def;
        public byte[] ext;
        public byte[] data;


        public byte[] Convert_Passwd_to_Byte(string passwd)
        {
            string msg = string.Empty;
            foreach (char x in passwd)
            {
                msg += Convert.ToString(x, 2).PadLeft(8, '0');
            }
            int num = msg.Length / 8;
            byte[] passwd_Byte = new byte[num];
            for (int i = 0; i < num; ++i)
            {
                passwd_Byte[i] = Convert.ToByte(msg.Substring(8 * i, 8), 2);
            }
            return passwd_Byte;
        }


        public string RUN_UN_STEG(byte[] fileSteg, string passSteg)
        {
            string notify = String.Empty;
            string fileSteg_String;
            char[] fileSteg_Char = new char[fileSteg.Length];
            byte[] passwd_Byte = Convert_Passwd_to_Byte(passSteg);

            List<byte> pwd_enc_l = new List<byte>();
            List<byte> pwd_inp_l = new List<byte>();
            List<byte> def_l = new List<byte>();
            List<byte> ext_l = new List<byte>();
            List<byte> data_l = new List<byte>();

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

            //GET LSB VALUE from RGB (0 or 1) to CHAR Array
            for (int i = 0; i < fileSteg.Length; i++)
            {
                if (fileSteg[i] % 2 == 0)
                {
                    fileSteg_Char[i] = (char)48;
                }
                else
                {
                    fileSteg_Char[i] = (char)49;
                }
            } 

            //LSB VALUE to STRING
            fileSteg_String = new string(fileSteg_Char);

            //LSB Value to Byte
            int num = fileSteg_String.Length / 8;
            byte[] fileSteg_Byte = new byte[num];
            for (int i = 0; i < num; ++i)
            {
                fileSteg_Byte[i] = Convert.ToByte(fileSteg_String.Substring(8 * i, 8), 2);
            }



            foreach(var g in fileSteg_Byte)
            {
                //System.Diagnostics.Debug.WriteLine(g);
            }

            //SPLIT LSB Value
            if (fileSteg_Byte.Contains( (byte)0 ) == true)
            {
                for (int i = 0; i < fileSteg_Byte.Length; i++)
                {
                    if (fileSteg_Byte[i] == 0)
                    {
                        byte[] bin = new byte[i];
                        Array.Copy(fileSteg_Byte, 0, bin, 0, i);
                        pwd_enc_l = bin.ToList();
                    }
                    if (fileSteg_Byte[i] == 1)
                    {
                        byte[] bin = new byte[i - pwd_enc_l.Capacity - 1];
                        Array.Copy(fileSteg_Byte, pwd_enc_l.Capacity + 1, bin, 0, i - pwd_enc_l.Capacity - 1);
                        pwd_inp_l = bin.ToList();
                    }
                    if (fileSteg_Byte[i] == 2)
                    {
                        byte[] bin = new byte[i - pwd_enc_l.Capacity - pwd_inp_l.Capacity - 2];
                        Array.Copy(fileSteg_Byte, pwd_enc_l.Capacity + pwd_inp_l.Capacity + 2, bin, 0, i - pwd_enc_l.Capacity - pwd_inp_l.Capacity - 2);
                        def_l = bin.ToList();
                    }
                    if (fileSteg_Byte[i] == 3)
                    {
                        byte[] bin = new byte[i - pwd_enc_l.Capacity - pwd_inp_l.Capacity - def_l.Capacity - 3];
                        Array.Copy(fileSteg_Byte, pwd_enc_l.Capacity + pwd_inp_l.Capacity + def_l.Capacity + 3, bin, 0, i - pwd_enc_l.Capacity - pwd_inp_l.Capacity - def_l.Capacity - 3);
                        ext_l = bin.ToList();
                    }
                    if (fileSteg_Byte[i] == 4)
                    {
                        byte[] bin = new byte[i - pwd_enc_l.Capacity - pwd_inp_l.Capacity - def_l.Capacity - ext_l.Capacity - 4];
                        Array.Copy(fileSteg_Byte, pwd_enc_l.Capacity + pwd_inp_l.Capacity + def_l.Capacity + ext_l.Capacity + 4, bin, 0, i - pwd_enc_l.Capacity - pwd_inp_l.Capacity - def_l.Capacity - ext_l.Capacity - 4);
                        data_l = bin.ToList();
                    }
                }
                notify = "Valid File Steg";
            }
            else
            {
                notify = "Invalid File Steg";
            }

            passwd_encrypt = pwd_enc_l.ToArray();
            passwd_input = pwd_inp_l.ToArray();
            def = def_l.ToArray();
            ext = ext_l.ToArray();
            data = data_l.ToArray();

            if(notify == "Valid File Steg")
            {
                //Check Password Encrypt is Same
                if (passwd_encrypt.SequenceEqual(passwd_Byte) == true)
                {
                    //Check Def Steg (Text or File)
                    if (def != null)
                    {
                        if (def[0] == 49)
                        {
                            notify = "Steg File";
                        }
                        else
                        {
                            notify = "Steg Message";
                        }
                    }
                }
                else
                {
                    notify = "Password Incorrect";
                }
            }

            System.Diagnostics.Debug.WriteLine(notify);
            foreach (var g in passwd_encrypt)
            {
                //System.Diagnostics.Debug.WriteLine(g);
            }
            return notify;


        }

        //public byte[] Notify_Steg_Result(string notify)
        //{
        //    if (notify == "Steg File")
        //    {

        //    }
        //    else
        //    {

        //    }
        //    byte[] data_steg;
        //    return data_steg;
        //}


        //CONTROL FOR DECRYPT
        //END


        //--------------------------------------------------------------------------------//




    }
}
