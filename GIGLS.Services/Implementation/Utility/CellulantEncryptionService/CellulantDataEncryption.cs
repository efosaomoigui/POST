using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace GIGLS.Services.Implementation.Utility.CellulantEncryptionService
{
    public class CellulantDataEncryption : ICellulantDataEncryption
    {
        private readonly string _ivKey;
        private readonly string _secretKey;

        public CellulantDataEncryption(string ivKey, string secretKey)
        {
            _ivKey = ivKey;
            _secretKey = secretKey;
        }

        public string EncryptData(string payload)
        {

            byte[] encrypted;

            using (AesCryptoServiceProvider aes = new AesCryptoServiceProvider())
            {
                aes.KeySize = 256;
                aes.BlockSize = 128;
                aes.Key = Encoding.UTF8.GetBytes(HashString(_ivKey).Substring(0, 16));
                aes.IV = Encoding.UTF8.GetBytes(HashString(_secretKey).Substring(0, 16));
                aes.Mode = CipherMode.CBC;
                aes.Padding = PaddingMode.PKCS7;

                ICryptoTransform enc = aes.CreateEncryptor(aes.Key, aes.IV);

                using (MemoryStream ms = new MemoryStream())
                {
                    using (CryptoStream cs = new CryptoStream(ms, enc, CryptoStreamMode.Write))
                    {
                        using (StreamWriter sw = new StreamWriter(cs))
                        {
                            sw.Write(payload);
                        }

                        encrypted = ms.ToArray();
                    }
                }
            }
            return Convert.ToBase64String(encrypted);
        }

        private string HashString(string str)
        {
            var stringBuilder = new StringBuilder();
            using (var hash = SHA256.Create())
            {
                var result = hash.ComputeHash(Encoding.UTF8.GetBytes(str));
                foreach (var x in result)
                {
                    stringBuilder.Append(x.ToString("x2"));
                }
            }

            return stringBuilder.ToString();
        }
    }
}

