using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net;
using System.Drawing.Imaging;
using System.Windows.Forms;

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
    }
}
