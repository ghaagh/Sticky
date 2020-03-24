using System;
using System.Text;
using System.Web;

namespace Sticky.Repositories.Common.Implementions
{
    public  class EncodeDecodeManager:IEncodeDecodeManager
    {
        public Random rnd = new Random(DateTime.Now.Millisecond);
        public string GetLetter()
        {
            string text = "abcdefghijklmnopqrstuvwxyz1234567890ABCDEFGHIJKLMNOPQRSTUVWXYZ";

            int index = rnd.Next(text.Length);
            return text[index].ToString();
        }
        public  string Base64Encode(string plainText, bool hasExtraChars = false)
        {
            byte[] plainTextBytes = Encoding.UTF8.GetBytes(plainText);
            string encodedString = Convert.ToBase64String(plainTextBytes);
            if (hasExtraChars)
                encodedString = encodedString.Replace("=", ":");
            string result = encodedString;
            if (hasExtraChars && encodedString.Length > 3)
            {
                string part1 = encodedString[0] + GetLetter();
                string part2 = encodedString[1] + GetLetter();
                string part3 = encodedString[2] + GetLetter();
                result = part1 + part2 + part3 + encodedString.Substring(3);
            }

            return result;
        }
        public string Base64Decode(string base64EncodedData, bool hasExtraChars = false)
        {
            base64EncodedData = HttpUtility.UrlDecode(base64EncodedData); // replace : with =
            base64EncodedData = base64EncodedData.Replace(" ", "+").Replace(":", "=");

            if (hasExtraChars && base64EncodedData.Length > 5)
            {
                base64EncodedData = base64EncodedData.Remove(5, 1);
                base64EncodedData = base64EncodedData.Remove(3, 1);
                base64EncodedData = base64EncodedData.Remove(1, 1);
            }
            var base64EncodedBytes = Convert.FromBase64String(base64EncodedData);
            return Encoding.UTF8.GetString(base64EncodedBytes);
        }
    }
}
