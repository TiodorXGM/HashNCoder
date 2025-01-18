using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net;
using System.Drawing.Imaging;

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
            byte[] bytes = Convert.FromBase64String(input);
            return Encoding.UTF8.GetString(bytes);
        }

        public static string URLEncode(string input)
        {

            string encoded = WebUtility.UrlEncode(input);
            return encoded;
            //StringBuilder encoded = new StringBuilder();

            //foreach (char c in input)
            //{
            //    if (IsSafe(c))
            //    {
            //        encoded.Append(c); 
            //    }
            //    else if (c == ' ')
            //    {
            //        encoded.Append("%20"); 
            //    }
            //    else
            //    {
            //        encoded.AppendFormat("%{0:X2}", (int)c); 
            //    }
            //}

            //return encoded.ToString();
        }

        public static string URLDecode(string input)
        {
            string decoded = WebUtility.UrlDecode(input);
            return decoded;
            //return Uri.UnescapeDataString(input);
        }

        //private static bool IsSafe(char c)
        //{            
        //    return char.IsLetterOrDigit(c) || c == '-' || c == '_' || c == '.' || c == '~';
        //}

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
