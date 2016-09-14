using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.IO;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using VendorSite.Proxies;

namespace VendorSite
{
    public class Helper
    {

        internal static async Task async_SendEmailToPoOwner(string poNum, string vendorID)
        {
            List<string> eTo = new List<string>();
            using (var poClient = new PurchaseOrderClient())
            {
                eTo.Add(GetBusinessEmail(poClient.GetUser2EntByUnpostedPoNum(poNum)));
            }

            using (var utilityClient = new UtilityClient())
            {
                await utilityClient.SendMessageEmailAsync(string.Format("PO# {0} was just confirmed by vendor {1}", poNum, vendorID), "Vendor Confirmation Alert", eTo);
            }
        }

        internal static void SendEmailToPoOwner(string poNum, string vendorID)
        {
            List<string> eTo = new List<string>();

            using(var poClient = new PurchaseOrderClient())
            {
                eTo.Add(GetBusinessEmail(poClient.GetUser2EntByUnpostedPoNum(poNum)));
            }
            
            using(var utitlityClient = new UtilityClient())
            {
                utitlityClient.SendMessageEmail(string.Format("PO# {0} was just confirmed by vendor {1}", poNum, vendorID), "Vendor Confirmation Alert", eTo);
            }
        }

        internal static string GetBusinessEmail(string userName)
        {
            string rslt = "@chadwellsupply.com";
            switch (userName)
            {
                case "rick.brown":
                    rslt = "rick.brown" + rslt;
                    break;
                case "tbenjamin":
                    rslt = "theresa.benjamin" + rslt;
                    break;
                case "jdotson":
                    rslt = "john.dotson" + rslt;
                    break;
                case "bregister":
                    rslt = "blaine.register" + rslt;
                    break;
                case "ray.sheppard":
                    rslt = "ray.sheppard" + rslt;
                    break;
                default:
                    rslt = "theresa.benjamin" + rslt;
                    break;
            }

            return rslt;
        }

        internal static string Encrypt(int value)
        {
            string result = "A";
            while (value > 0)
            {
                result = (char)('A' + value % 26) + result;
                value /= 26;
            }
            return result;
        }


        internal static string Decrypt(string cipherText, string key)
        {
            char[] chars = new char[cipherText.Length];

            for (int i = 0; i < cipherText.Length; i++)
            {
                if (cipherText[i] == ' ')
                {
                    chars[i] = ' ';
                }
                else
                {
                    int j = key.IndexOf(cipherText[i]) - 97;
                    chars[i] = (char)j;
                }
            }

            return new string(chars);
        }
    }

}


namespace EncryptStringSample
{
    public static class StringCipher
    {
        // This constant string is used as a "salt" value for the PasswordDeriveBytes function calls.
        // This size of the IV (in bytes) must = (keysize / 8).  Default keysize is 256, so the IV must be
        // 32 bytes long.  Using a 16 character string here gives us 32 bytes when converted to a byte array.
        private static readonly byte[] initVectorBytes = Encoding.ASCII.GetBytes("tu89geji340t89u2");

        // This constant is used to determine the keysize of the encryption algorithm.
        private const int keysize = 256;

        //public static string Encrypt(string plainText, string passPhrase)
        //{
        //    byte[] plainTextBytes = Encoding.UTF8.GetBytes(plainText);
        //    using (PasswordDeriveBytes password = new PasswordDeriveBytes(passPhrase, null))
        //    {
        //        byte[] keyBytes = password.GetBytes(keysize / 8);
        //        using (RijndaelManaged symmetricKey = new RijndaelManaged())
        //        {
        //            symmetricKey.Mode = CipherMode.CBC;
        //            using (ICryptoTransform encryptor = symmetricKey.CreateEncryptor(keyBytes, initVectorBytes))
        //            {
        //                using (MemoryStream memoryStream = new MemoryStream())
        //                {
        //                    using (CryptoStream cryptoStream = new CryptoStream(memoryStream, encryptor, CryptoStreamMode.Write))
        //                    {
        //                        cryptoStream.Write(plainTextBytes, 0, plainTextBytes.Length);
        //                        cryptoStream.FlushFinalBlock();
        //                        byte[] cipherTextBytes = memoryStream.ToArray();
        //                        return Convert.ToBase64String(cipherTextBytes);
        //                    }
        //                }
        //            }
        //        }
        //    }
        //}

        public static byte[] AES_Encrypt(byte[] bytesToBeEncrypted, byte[] passwordBytes)
        {
            byte[] encryptedBytes = null;

            // Set your salt here, change it to meet your flavor:
            // The salt bytes must be at least 8 bytes.
            byte[] saltBytes = new byte[] { 1, 2, 3, 4, 5, 6, 7, 8 };

            using (MemoryStream ms = new MemoryStream())
            {
                using (RijndaelManaged AES = new RijndaelManaged())
                {
                    AES.KeySize = 256;
                    AES.BlockSize = 128;

                    var key = new Rfc2898DeriveBytes(passwordBytes, saltBytes, 1000);
                    AES.Key = key.GetBytes(AES.KeySize / 8);
                    AES.IV = key.GetBytes(AES.BlockSize / 8);

                    AES.Mode = CipherMode.CBC;

                    using (var cs = new CryptoStream(ms, AES.CreateEncryptor(), CryptoStreamMode.Write))
                    {
                        cs.Write(bytesToBeEncrypted, 0, bytesToBeEncrypted.Length);
                        cs.Close();
                    }
                    encryptedBytes = ms.ToArray();
                }
            }

            return encryptedBytes;
        }

        public static string EncryptText(string input, string password)
        {
            // Get the bytes of the string
            byte[] bytesToBeEncrypted = Encoding.UTF8.GetBytes(input);
            byte[] passwordBytes = Encoding.UTF8.GetBytes(password);

            // Hash the password with SHA256
            passwordBytes = SHA256.Create().ComputeHash(passwordBytes);

            byte[] bytesEncrypted = AES_Encrypt(bytesToBeEncrypted, passwordBytes);

            string result = Convert.ToBase64String(bytesEncrypted);

            return result;
        }


        //public static string Decrypt(string cipherText, string passPhrase)
        //{
        //    byte[] cipherTextBytes = Convert.FromBase64String(cipherText);
        //    using(PasswordDeriveBytes password = new PasswordDeriveBytes(passPhrase, null))
        //    {
        //        byte[] keyBytes = password.GetBytes(keysize / 8);
        //        using(RijndaelManaged symmetricKey = new RijndaelManaged())
        //        {
        //            symmetricKey.Mode = CipherMode.CBC;
        //            using(ICryptoTransform decryptor = symmetricKey.CreateDecryptor(keyBytes, initVectorBytes))
        //            {
        //                using(MemoryStream memoryStream = new MemoryStream(cipherTextBytes))
        //                {
        //                    using(CryptoStream cryptoStream = new CryptoStream(memoryStream, decryptor, CryptoStreamMode.Read))
        //                    {
        //                        byte[] plainTextBytes = new byte[cipherTextBytes.Length];
        //                        int decryptedByteCount = cryptoStream.Read(plainTextBytes, 0, plainTextBytes.Length);
        //                        return Encoding.UTF8.GetString(plainTextBytes, 0, decryptedByteCount);
        //                    }
        //                }
        //            }
        //        }
        //    }
        //}

        public static byte[] AES_Decrypt(byte[] bytesToBeDecrypted, byte[] passwordBytes)
        {
            byte[] decryptedBytes = null;

            // Set your salt here, change it to meet your flavor:
            // The salt bytes must be at least 8 bytes.
            byte[] saltBytes = new byte[] { 1, 2, 3, 4, 5, 6, 7, 8 };

            using (MemoryStream ms = new MemoryStream())
            {
                using (RijndaelManaged AES = new RijndaelManaged())
                {
                    AES.KeySize = 256;
                    AES.BlockSize = 128;

                    var key = new Rfc2898DeriveBytes(passwordBytes, saltBytes, 1000);
                    AES.Key = key.GetBytes(AES.KeySize / 8);
                    AES.IV = key.GetBytes(AES.BlockSize / 8);

                    AES.Mode = CipherMode.CBC;

                    using (var cs = new CryptoStream(ms, AES.CreateDecryptor(), CryptoStreamMode.Write))
                    {
                        cs.Write(bytesToBeDecrypted, 0, bytesToBeDecrypted.Length);
                        cs.Close();
                    }
                    decryptedBytes = ms.ToArray();
                }
            }

            return decryptedBytes;
        }
        public static string DecryptText(string input, string password)
        {
            // Get the bytes of the string
            byte[] bytesToBeDecrypted = Convert.FromBase64String(input);
            byte[] passwordBytes = Encoding.UTF8.GetBytes(password);
            passwordBytes = SHA256.Create().ComputeHash(passwordBytes);

            byte[] bytesDecrypted = AES_Decrypt(bytesToBeDecrypted, passwordBytes);

            string result = Encoding.UTF8.GetString(bytesDecrypted);

            return result;
        }
    }
}