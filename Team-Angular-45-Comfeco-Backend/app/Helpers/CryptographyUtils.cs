using System;
using System.Security.Cryptography;
using System.Text;

namespace BackendComfeco.Helpers
{
    public static class CryptographyUtils
    {
        public static string ComputeSHA256Hash(string text)
        {
            using var sha256 = new SHA256Managed();
            return BitConverter.ToString(sha256.ComputeHash(Encoding.UTF8.GetBytes(text))).Replace("-", "");
        }
        public static string Base64Decode(string base64EncodedData)
        {
            var base64EncodedBytes = Convert.FromBase64String(base64EncodedData);
            return Encoding.UTF8.GetString(base64EncodedBytes);
        }
        public static string Base64Encode(string plainText)
        {
            var plainTextBytes = Encoding.UTF8.GetBytes(plainText);
            return Convert.ToBase64String(plainTextBytes);
        }
    }
}
