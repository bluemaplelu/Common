using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Security.Cryptography;
using System.Text;

namespace ECS.Common.EncryptCollection
{
    public sealed class Cryptographer
    {
        public static string Encrypt(string text)
        {
            using (RijndaelManaged rijndaelManaged = new RijndaelManaged())
            {
                rijndaelManaged.Key = Convert.FromBase64String(ConfigurationManager.AppSettings["Key"]);
                rijndaelManaged.IV = Convert.FromBase64String(ConfigurationManager.AppSettings["IV"]);
                ICryptoTransform encryptor = rijndaelManaged.CreateEncryptor();
                byte[] bytes = Encoding.UTF8.GetBytes(text);
                byte[] inArray = encryptor.TransformFinalBlock(bytes, 0, bytes.Length);
                encryptor.Dispose();
                return Convert.ToBase64String(inArray);
            }
        }

        public static string Decrypt(string text)
        {
            using (RijndaelManaged rijndaelManaged = new RijndaelManaged())
            {
                rijndaelManaged.Key = Convert.FromBase64String(ConfigurationManager.AppSettings["Key"]);
                rijndaelManaged.IV = Convert.FromBase64String(ConfigurationManager.AppSettings["IV"]);
                ICryptoTransform decryptor = rijndaelManaged.CreateDecryptor();
                byte[] inputBuffer = Convert.FromBase64String(text);
                byte[] bytes = decryptor.TransformFinalBlock(inputBuffer, 0, inputBuffer.Length);
                decryptor.Dispose();
                return Encoding.UTF8.GetString(bytes);
            }
        }

        public static string Decrypt(string text, string key, string iv)
        {
            using (RijndaelManaged rijndaelManaged = new RijndaelManaged())
            {
                rijndaelManaged.Key = Convert.FromBase64String(key);
                rijndaelManaged.IV = Convert.FromBase64String(iv);
                ICryptoTransform decryptor = rijndaelManaged.CreateDecryptor();
                byte[] inputBuffer = Convert.FromBase64String(text);
                byte[] bytes = decryptor.TransformFinalBlock(inputBuffer, 0, inputBuffer.Length);
                decryptor.Dispose();
                return Encoding.UTF8.GetString(bytes);
            }
        }

        private static byte[] CreateHash(byte[] plaintext)
        {
            return new MD5CryptoServiceProvider().ComputeHash(plaintext);
        }

        public static string CreateHash(string plaintext)
        {
            byte[] hash = Cryptographer.CreateHash(Encoding.ASCII.GetBytes(plaintext));
            StringBuilder stringBuilder = new StringBuilder();
            for (int index = 0; index < hash.Length; ++index)
                stringBuilder.Append(hash[index].ToString("x2"));
            return ((object)stringBuilder).ToString();
        }
    }
}
