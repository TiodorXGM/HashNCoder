using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net;
using System.Drawing.Imaging;
using System.Windows.Forms;
using System.Security.Cryptography;
using static System.Net.Mime.MediaTypeNames;

namespace HashNCoder
{
    public static class Coding
    {

        public static string EncodeBase64(string input)
        {
            byte[] bytes = Encoding.UTF8.GetBytes(input);
            return Convert.ToBase64String(bytes);
        }

        public static string DecodeBase64(string input)
        {
            try
            {
                byte[] bytes = Convert.FromBase64String(input);
                return Encoding.UTF8.GetString(bytes);
            }
            catch (FormatException)
            {
                MessageBox.Show(
                                "The string is not a valid Base64 string.\n\n" +
                                "- Contain only letters (A-Z, a-z), digits (0-9), and the characters '+' and '/';\n" +
                                "- Have a length that is a multiple of 4 (including '=' padding characters);",
                                "Warning", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return null;
            }
        }

        public static string URLEncode(string input)
        {
            string encoded = WebUtility.UrlEncode(input);
            return encoded;         
        }

        public static string URLDecode(string input)
        {
            string decoded = WebUtility.UrlDecode(input);
            return decoded;            
        }       

        public static string HTMLEncode(string input)
        {            
            string encoded = WebUtility.HtmlEncode(input);
            return encoded;
        }

        public static string HTMLDecode(string input)
        {
            string decode = WebUtility.HtmlDecode(input);
            return decode;
        }

        public static string UnescapeEncode(string input)
        {
            string encoded = Uri.EscapeDataString(input);
            return encoded;
        }

        public static string UnescapeDecode(string input)
        {
            string decoded = Uri.UnescapeDataString(input);
            return decoded;
        }

        public static string AES_Encrypt(string plainText, string keyText, CipherMode mode, int keySize)
        {          
            byte[] key = GenerateKeyFromText(keyText, keySize);

            using (Aes aes = Aes.Create())
            {
                aes.Key = key;
                aes.Mode = mode;
                aes.Padding = PaddingMode.PKCS7;

                byte[] iv = null;

                if (mode != CipherMode.ECB)
                {
                    aes.GenerateIV();
                    iv = aes.IV;
                }

                using (ICryptoTransform encryptor = aes.CreateEncryptor())
                {
                    byte[] plainBytes = Encoding.UTF8.GetBytes(plainText);
                    byte[] encryptedBytes = encryptor.TransformFinalBlock(plainBytes, 0, plainBytes.Length);

                    if (mode == CipherMode.ECB)
                    {
                        return Convert.ToBase64String(encryptedBytes);
                    }

                    byte[] result = new byte[iv.Length + encryptedBytes.Length];
                    Array.Copy(iv, 0, result, 0, iv.Length);
                    Array.Copy(encryptedBytes, 0, result, iv.Length, encryptedBytes.Length);

                    return Convert.ToBase64String(result); 
                }
            }
        }

        public static string AES_Decrypt(string cipherText, string keyText, CipherMode mode, int keySize)
        {
            byte[] cipherBytes = null;

            try
            {
                cipherBytes = Convert.FromBase64String(cipherText);
            }
            catch (FormatException)
            {
                MessageBox.Show(
                                "The string is not a valid Base64 string.\n\n" +
                                "- Contain only letters (A-Z, a-z), digits (0-9), and the characters '+' and '/';\n" +
                                "- Have a length that is a multiple of 4 (including '=' padding characters);",
                                "Warning", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return null;
            }

            byte[] key = GenerateKeyFromText(keyText, keySize); 

            using (Aes aes = Aes.Create())
            {
                aes.Key = key;
                aes.Mode = mode;
                aes.Padding = PaddingMode.PKCS7;

                byte[] iv = null;
                byte[] encryptedBytes;

                if (mode != CipherMode.ECB)
                {
                    iv = new byte[16]; 
                    Array.Copy(cipherBytes, 0, iv, 0, iv.Length);
                    aes.IV = iv;

                    encryptedBytes = new byte[cipherBytes.Length - iv.Length];
                    Array.Copy(cipherBytes, iv.Length, encryptedBytes, 0, encryptedBytes.Length);
                }
                else
                {
                    encryptedBytes = cipherBytes;
                }

                using (ICryptoTransform decryptor = aes.CreateDecryptor())
                {
                    byte[] plainBytes = decryptor.TransformFinalBlock(encryptedBytes, 0, encryptedBytes.Length);
                    return Encoding.UTF8.GetString(plainBytes); 
                }
            }
        }

        private static byte[] GenerateKeyFromText(string keyText, int keySize)
        {
            byte[] keyBytes = Encoding.UTF8.GetBytes(keyText);
            byte[] key = new byte[keySize / 8];

            for (int i = 0; i < key.Length; i++)
            {
                key[i] = keyBytes[i % keyBytes.Length];
            }

            return key;
        }


        public static byte[] AES_GenerateKey(int keySize)
        {
            using (Aes aes = Aes.Create())
            {
                aes.KeySize = keySize;
                aes.GenerateKey();
                return aes.Key;
            }
        }

        public static bool IsBase64String(string base64)
        {
            if (string.IsNullOrWhiteSpace(base64)) return false;

            if (base64.Length % 4 != 0) return false;

            return System.Text.RegularExpressions.Regex.IsMatch(base64, @"^[a-zA-Z0-9+/]*={0,2}$");
        }

    }
}
