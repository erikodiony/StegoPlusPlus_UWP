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
using Windows.UI.Xaml;
using Windows.UI.Xaml.Media.Animation;
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
        public static readonly string BitDepth = "System.Image.BitDepth";
        public static readonly string[] All = new string[] { Size, Dimensions, BitDepth };
    }

    internal static class HeaderPage
    {
        public static readonly string HomePage = "WELCOME";
        public static readonly string EmbedPage = "Menu yang berfungsi untuk menjalankan fungsi Embed untuk penyisipan File (Embed File) beserta Embed untuk penyisipan Pesan Rahasia (Embed Message)";
        public static readonly string ExtractPage = "Menu yang berfungsi untuk menjalankan fungsi Ekstrak untuk File / Pesan Rahasia (Extract File / Message) beserta untuk pengecekan File Stego yang masih baik / bisa digunakan (Check Stego Healthy)";
        public static readonly string SettingsPage = "Menu yang berfungsi untuk mengganti Warna Latar Belakang (Change Background / Theme) beserta mengganti Efek Transisi Halaman (Change Transition Effect)";
        public static readonly string AboutPage = "Menu yang berisi tentang Detail Aplikasi (App Detail) beserta Info tentang Creator (About Me)";
    }

    internal static class NotifyDataText
    {
        public static readonly string OK_Button = "OK";
        public static readonly string Cancel_Button = "Cancel";
        public static readonly string Dialog_Exec_Footer_Menu_Confirm = "Confirm to Execute ?\nClick 'OK' to continue...";
        public static readonly string Dialog_Exec_Footer_Menu_Null = "Some field is empty or input not saved !\nPlease check again...";
        public static readonly string Dialog_Clear_Footer_Menu_Null = "All field was Cleared !\nProcess Successfully...";
        public static readonly string Notify_Input_Passwd_Invalid = "Password Invalid !\nCan't Saving Password...";
        public static readonly string Notify_Input_Message_Invalid = "Text/Message Invalid !\nCan't Saving Text/Message...";
        public static readonly string Process_Complete_EmbedFile_File = "Embedding File was Completed !\nClick 'OK' to saving Stego Image...";
        public static readonly string Process_Complete_EmbedFile_Message = "Embedding Text/Message was Completed !\nClick 'OK' to saving Stego Image...";
        public static readonly string Process_Complete_ExtractFile = "Extracting File was Completed !\nClick 'OK' to saving Secret File...";
        public static readonly string Process_Complete_ExtractMessage = "Extracting Message was Completed !\nClick 'OK' to view Secret Text/Message...";

        //EMBED MENU
        public static readonly string Err_Input_32bitDepth = "Only Image Cover 32BitDepth was supported !\nPlease check again...";
        public static readonly string Err_FileHiding_Overload_Size = "Overload size quota of File Hiding !\nCan't Saving File Hiding...";
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
        public int[] secretData; //Save Specific Length of Byte Stego for Extract Data
        public int length_pwd_encoded; //Length of passwd (array)
        public int length_pwd_crypt_encoded; //Length of passwd encrypt (array)
        public int length_def_encoded; //Length of Definition/Type Stego (Text or File) (array)
        public int length_ext_encoded; //Length of Extension (array)
        public int length_data_encoded; //Length of Data Stego (array)
        public char[] validate = { 'A', 'a', 'B', 'b', 'C', 'c', 'D', 'd', 'E', 'e', 'F', 'f', 'G', 'g', 'H', 'h', 'I', 'i', 'J', 'j', 'K', 'k', 'L', 'l', 'M', 'm', 'N', 'n', 'O', 'o', 'P', 'p', 'Q', 'q', 'R', 'r', 'S', 's', 'T', 't', 'U', 'u', 'V', 'v', 'W', 'w', 'X', 'x', 'Y', 'y', 'Z', 'z', '0', '1', '2', '3', '4', '5', '6', '7', '8', '9', ' ', ',', '.', '@', '<', ')', '(', '?', '!', '&', '/', '*', '+', '-', ':', '^', '_', '=', '>' };
        public char[,] matrix = new char[,]
        {
            {'A' , 'a' , 'B' , 'b' , 'C' , 'c' , 'D' , 'd' , 'E'},
            {'e' , 'F' , 'f' , 'G' , 'g' , 'H' , 'h' , 'I' , 'i'},
            {'J' , 'j' , 'K' , 'k' , 'L' , 'l' , 'M' , 'm' , 'N'},
            {'n' , 'O' , 'o' , 'P' , 'p' , 'Q' , 'q' , 'R' , 'r'},
            {'S' , 's' , 'T' , 't' , 'U' , 'u' , 'V' , 'v' , 'W'},
            {'w' , 'X' , 'x' , 'Y' , 'y' , 'Z' , 'z' , '0' , '1'},
            {'2' , '3' , '4' , '5' , '6' , '7' , '8' , '9' , ' '},
            {',' , '.' , '@' , '<' , ')' , '(' , '?' , '!' , '&'},
            {'/' , '*' , '+' , '-' , ':' , '^' , '_' , '=' , '>'}
        };


        //BEGIN
        //GET PIXEL DATA
        //--------------------------------------------------------------------------------//

        public async Task<byte[]> Convert_FileImage_to_Byte(StorageFile fileCover_or_fileSteg)
        {
            byte[] bin;
            string Val = String.Empty;
            using (strm_Cover = await fileCover_or_fileSteg.OpenAsync(FileAccessMode.ReadWrite))
            {
                decoder = await BitmapDecoder.CreateAsync(strm_Cover);
                bin = (await decoder.GetPixelDataAsync()).DetachPixelData();
                BitmapPropertySet props = await decoder.BitmapProperties.GetPropertiesAsync(
                    new string[]
                    {
                        "/tEXt/{str=Description}"
                    });
                BitmapTypedValue data;
                if (props.TryGetValue("/tEXt/{str=Description}", out data))
                {
                    Val = (string)data.Value;
                    string[] sData = Val.Split('|');
                    secretData = new int[sData.Length];
                    int xx = -1;
                    foreach (var x in sData)
                    {
                        secretData[++xx] = int.Parse(x);
                    }
                }
            }
            return bin;
        }

        //--------------------------------------------------------------------------------//
        //GET PIXEL DATA
        //END

        
        //CONTROL FOR ENCRYPT
        //BEGIN

        public string Encrypt_BifidCipher(string MessageOrPasswdToEncrypt)
        {
            char[] input_char = MessageOrPasswdToEncrypt.ToCharArray();
            List<int> list_x = new List<int>();
            List<int> list_y = new List<int>();
            List<int> list_xy = new List<int>();
            int[] x_y; //From list_xy Convert to Array
            string[] crypt_x;
            string[] crypt_y;
            string result = String.Empty; //Encrypt of Passwd

            foreach (var xx in input_char)
            {
                for (int x = 0; x < matrix.GetLength(0); ++x) //Width
                {
                    for (int y = 0; y < matrix.GetLength(1); ++y) //Height
                    {
                        if (matrix[x, y].Equals(xx))
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
                result += matrix[Convert.ToInt32(crypt_x[i]), Convert.ToInt32(crypt_y[i])].ToString();
            }
            return result;
        }

        public string Decrypt_BifidCipher(string MessageOrPasswdToDecrypt)
        {
            char[] input_char = MessageOrPasswdToDecrypt.ToCharArray();
            List<int> list_x = new List<int>();
            List<int> list_y = new List<int>();
            int[] arr_x;
            int[] arr_y;
            string xy = String.Empty;
            string result = String.Empty;

            foreach (var xx in input_char)
            {
                for (int x = 0; x < matrix.GetLength(0); ++x)
                {
                    for (int y = 0; y < matrix.GetLength(1); ++y)
                    {
                        if (matrix[x, y].Equals(xx))
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
                result += matrix[Convert.ToInt32(str_x[i]), Convert.ToInt32(str_y[i])].ToString();
            }
            return result;
        }

        public string validatePasswdOrMessageInput(string passwd)
        {
            string notify_result = String.Empty;
            foreach (char c in passwd)
            {
                if (validate.Contains(c) != true)
                {
                  notify_result   = "Password Invalid";
                }
            }
            return notify_result;
        }

        public char[] Convert_Passwd(string passwd)
        {
            string pwd = string.Empty;
            foreach (char x in passwd)
            {
                pwd += Convert.ToString(x, 2).PadLeft(8, '0');
            }

            char[] pwd_encoded = pwd.ToCharArray();
            return pwd_encoded;
        }

        public char[] Convert_Passwd_Encrypt(string passwd_crypt)
        {
            string pwd_crypt = string.Empty;
            foreach (char x in passwd_crypt)
            {
                pwd_crypt += Convert.ToString(x, 2).PadLeft(8, '0');
            }

            char[] pwd_crypt_encoded = pwd_crypt.ToCharArray();
            return pwd_crypt_encoded;
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
            byte[] bin_array;
            string bin_string = String.Empty;
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
                char[] bin = bin_string.ToCharArray();
                return bin;
            }
        }

        public byte[] RUN_STEG(char[] fileOrMessage, byte[] coverImage, char[] passwd, char[] passwd_encrypt, char[] extension, char[] def)
        {
            byte[] steg_result = new byte[coverImage.Length];

            //Structure of Hiding Data {pwd_encoded + pwd + file||message + extention + data}            
            //pwd_encoded
            //pwd
            //file||message
            //extention
            //data

            //Initialize length of Data Array
            length_pwd_crypt_encoded = passwd_encrypt.Length;
            length_pwd_encoded = passwd.Length;
            length_def_encoded = def.Length;
            length_ext_encoded = extension.Length;
            length_data_encoded = fileOrMessage.Length;

            int length_encrypt = (passwd_encrypt.Length + passwd.Length + def.Length + extension.Length + fileOrMessage.Length);
            char[] all_encrypt = new char[length_encrypt];

            Array.Copy(passwd_encrypt, all_encrypt, passwd_encrypt.Length);
            Array.Copy(passwd, 0, all_encrypt, length_pwd_crypt_encoded, passwd.Length);
            Array.Copy(def, 0, all_encrypt, length_pwd_crypt_encoded + length_pwd_encoded, def.Length);
            Array.Copy(extension, 0, all_encrypt, length_pwd_crypt_encoded + length_pwd_encoded + length_def_encoded, extension.Length);
            Array.Copy(fileOrMessage, 0, all_encrypt, length_pwd_crypt_encoded + length_pwd_encoded + length_def_encoded + length_ext_encoded, fileOrMessage.Length);
 
            //Inserting Hiding File/Message to Stego Image
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

            steg_result = coverImage;
            return steg_result;
        }

        //CONTROL FOR ENCRYPT
        //END


        //--------------------------------------------------------------------------------//


        //CONTROL FOR DECRYPT
        //BEGIN

        public static byte[] passwd_encrypt; //Passwd Input (Encrypt with Bifid Cipher)
        public static byte[] passwd_input; //Passwd Input (Raw / Non Encrypt)
        public static byte[] def; //Definition Type Stego (text or File)
        public static byte[] ext; //Extensions of File Hiding (Non Support on Stego Text or Message)
        public static byte[] data; //Data Hiding

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

            //Structure of Hiding Data {pwd_encoded + pwd + file||message + extention + data}            
            //pwd_encoded
            //pwd
            //file||message
            //extention
            //data

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

            //SPLIT LSB Value
            if (secretData != null)
            {
                //Inizialize Array Data
                passwd_encrypt = new byte[secretData[0] / 8];
                passwd_input = new byte[secretData[1] / 8];
                def = new byte[secretData[2] / 8];
                ext = new byte[secretData[3] / 8];
                data = new byte[secretData[4] / 8];

                //Spliting LSB Value to Array Byte
                Array.Copy(fileSteg_Byte, 0, passwd_encrypt, 0, secretData[0] / 8);
                Array.Copy(fileSteg_Byte, (secretData[0] / 8), passwd_input, 0, secretData[1] / 8);
                Array.Copy(fileSteg_Byte, (secretData[0] / 8) + (secretData[1] / 8), def, 0, secretData[2] / 8);
                Array.Copy(fileSteg_Byte, (secretData[0] / 8) + (secretData[1] / 8) + (secretData[2] / 8), ext, 0, secretData[3] / 8);
                Array.Copy(fileSteg_Byte, (secretData[0] / 8) + (secretData[1] / 8) + (secretData[2] / 8) + (secretData[3] / 8), data, 0, secretData[4] / 8);

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
            else
            {
                notify = "Invalid File Steg";
            }

            return notify;
        }


        public string RUN_UN_STEG_CHECKINFO(byte[] fileStegCheck)
        {
            string notify = String.Empty;
            string fileSteg_String;
            char[] fileSteg_Char = new char[fileStegCheck.Length];

            List<byte> pwd_enc_l = new List<byte>();
            List<byte> pwd_inp_l = new List<byte>();
            List<byte> def_l = new List<byte>();
            List<byte> ext_l = new List<byte>();
            List<byte> data_l = new List<byte>();

            //Structure of Hiding Data {pwd_encoded + pwd + file||message + extention + data}            
            //pwd_encoded
            //pwd
            //file||message
            //extention
            //data

            //GET LSB VALUE from RGB (0 or 1) to CHAR Array
            for (int i = 0; i < fileStegCheck.Length; i++)
            {
                if (fileStegCheck[i] % 2 == 0)
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

            //SPLIT LSB Value
            if (secretData != null)
            {
                //Inizialize Array Data
                passwd_encrypt = new byte[secretData[0] / 8];
                passwd_input = new byte[secretData[1] / 8];
                def = new byte[secretData[2] / 8];
                ext = new byte[secretData[3] / 8];
                data = new byte[secretData[4] / 8];

                //Spliting LSB Value to Array Byte
                Array.Copy(fileSteg_Byte, 0, passwd_encrypt, 0, secretData[0] / 8);
                Array.Copy(fileSteg_Byte, (secretData[0] / 8), passwd_input, 0, secretData[1] / 8);
                Array.Copy(fileSteg_Byte, (secretData[0] / 8) + (secretData[1] / 8), def, 0, secretData[2] / 8);
                Array.Copy(fileSteg_Byte, (secretData[0] / 8) + (secretData[1] / 8) + (secretData[2] / 8), ext, 0, secretData[3] / 8);
                Array.Copy(fileSteg_Byte, (secretData[0] / 8) + (secretData[1] / 8) + (secretData[2] / 8) + (secretData[3] / 8), data, 0, secretData[4] / 8);
            }
            else
            {
                notify = "Invalid File Steg";
            }
            return notify;
        }

            //CONTROL FOR DECRYPT
            //END


            //--------------------------------------------------------------------------------//




        }
}
