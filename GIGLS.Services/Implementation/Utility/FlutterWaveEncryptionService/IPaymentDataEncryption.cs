using System;

namespace GIGLS.Services.Implementation.Utility.FlutterWaveEncryptionService
{
    public interface IPaymentDataEncryption
    {
        string GetEncryptionKey(string secretKey);
        string EncryptData(string encryptionKey, String data);
        string DecryptData(string encryptedData, string encryptionKey);
    }
}