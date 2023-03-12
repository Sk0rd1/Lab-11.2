using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Security.Cryptography;
using System.IO;

namespace Lab_11._2
{
    internal class Program
    {
        static void Main(string[] args)
        {
            string exampleString = "Somebody once told me the world is gonna roll me.";
            byte[] key = CreateKey();
            byte[] iv = CreateIV();

            Console.WriteLine("Example of string:");
            Console.WriteLine(exampleString);

            string resultStr = AESEncrypt(exampleString, key, iv);

            Console.WriteLine("\nEncrypt string:");
            Console.WriteLine(resultStr);

            resultStr = AESDecrypt(resultStr, key, iv);

            Console.WriteLine("\nDecrypt string:");
            Console.WriteLine(resultStr);

            Console.Read();
        }

        public static byte[] CreateKey()
        {
            byte[] key = new byte[32];
            using (var rng = new RNGCryptoServiceProvider())
            {
                rng.GetBytes(key);
            }
            return key;
        }

        public static byte[] CreateIV()
        {
            byte[] iv = new byte[16];
            using (var rng = new RNGCryptoServiceProvider())
            {
                rng.GetBytes(iv);
            }
            return iv;
        }

        public static string AESEncrypt(string exampleString, byte[] key, byte[] iv)
        {
            byte[] exampleStringBytes = Encoding.UTF8.GetBytes(exampleString);

            using (Aes aes = Aes.Create())
            {
                aes.Key = key;
                aes.IV = iv;
                using (MemoryStream ms = new MemoryStream())
                {
                    using (CryptoStream cs = new CryptoStream(ms, aes.CreateEncryptor(), CryptoStreamMode.Write))
                    {
                        cs.Write(exampleStringBytes, 0, exampleStringBytes.Length);
                    }

                    byte[] encryptedBytes = ms.ToArray();
                    return Convert.ToBase64String(encryptedBytes);
                }
            }

        }

        public static string AESDecrypt(string encryptedString, byte[] key, byte[] iv)
        {
            byte[] encryptedBytes = Convert.FromBase64String(encryptedString);

            using (Aes aes = Aes.Create())
            {
                aes.Key = key;
                aes.IV = iv;
                using (MemoryStream ms = new MemoryStream())
                {
                    using (CryptoStream cs = new CryptoStream(ms, aes.CreateDecryptor(), CryptoStreamMode.Write))
                    {
                        cs.Write(encryptedBytes, 0, encryptedBytes.Length);
                    }

                    byte[] decryptedBytes = ms.ToArray();
                    return Encoding.UTF8.GetString(decryptedBytes);
                }
            }
        }
    }
}