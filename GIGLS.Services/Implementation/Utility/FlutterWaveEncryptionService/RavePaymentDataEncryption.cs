using System;
using System.Security.Cryptography;
using System.Text;

namespace GIGLS.Services.Implementation.Utility.FlutterWaveEncryptionService
{
    public class RavePaymentDataEncryption : IPaymentDataEncryption
    {
        /// <summary>
        /// Gets an encryption key from rave secret key.
        /// </summary>
        /// <param name="secretKey">The secret key generated from your rave dashboard</param>
        /// <returns>a string value encrypted</returns>
        /// 
        public string GetEncryptionKey(string secretKey)
        {
            //MD5 is the hash algorithm expected by rave to generate encryption key
            MD5CryptoServiceProvider md5 = new MD5CryptoServiceProvider();

            //MD5CryptoServiceProvider works with bytes so a conversion of plain secretKey to it bytes equivalent is required.
            //UTF8Encoding.UTF8.GetBytes(secretKey) can also be used.
            byte[] secretKeyBytes = ASCIIEncoding.UTF8.GetBytes(secretKey);

            byte[] hashedSecret = md5.ComputeHash(secretKeyBytes, 0, secretKeyBytes.Length);
            byte[] hashedSecretLast128Bytes = new byte[12];
            Array.Copy(hashedSecret, hashedSecret.Length - 12, hashedSecretLast128Bytes, 0, 12);
            String hashedSecretLast12HexString = BitConverter.ToString(hashedSecretLast128Bytes);
            hashedSecretLast12HexString = hashedSecretLast12HexString.ToLower().Replace("-", "");
            String secretKeyFirst12 = secretKey.Replace("FLWSECK-", "").Substring(0, 12);
            byte[] hashedSecretLast12HexBytes = ASCIIEncoding.UTF8.GetBytes(hashedSecretLast12HexString);
            byte

        }

        public string EncryptData(string encryptionKey, string data)
        {
            throw new NotImplementedException();
        }

        public string DecryptData(string encryptedData, string encryptionKey)
        {
            throw new NotImplementedException();
        }


    }
}
