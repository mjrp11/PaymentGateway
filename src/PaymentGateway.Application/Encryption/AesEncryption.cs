using System;
using System.IO;
using System.Security.Cryptography;
using System.Text;
using PaymentGateway.Application.Encryption.EncryptionKeys;

namespace PaymentGateway.Application.Encryption
{
    public class AesEncryption: ISymmetricEncryption
    {
        private Aes _aes;

        private ICryptoTransform _encryptor;
        private ICryptoTransform _decryptor;

        public AesEncryption(IEncryptionKeys encryptionKeys)
        {
            _aes = Aes.Create();
            _aes.Key = Encoding.UTF8.GetBytes(encryptionKeys.GetKey());
            _aes.IV = new byte[16];
            
            _encryptor = _aes.CreateEncryptor(_aes.Key, _aes.IV);
            _decryptor = _aes.CreateDecryptor(_aes.Key, _aes.IV);
        }

        public string Encrypt(string value)
        {
            byte[] array;
            
            using (var memoryStream = new MemoryStream())  
            {  
                using (var cryptoStream = new CryptoStream(memoryStream, _encryptor, CryptoStreamMode.Write))  
                {  
                    using (var streamWriter = new StreamWriter(cryptoStream))  
                    {  
                        streamWriter.Write(value);  
                    }  
  
                    array = memoryStream.ToArray();  
                }  
            }
            
            return Convert.ToBase64String(array);  
        }

        public string Decrypt(string value)
        {
            var buffer = Convert.FromBase64String(value);
            
            using (MemoryStream memoryStream = new MemoryStream(buffer))  
            {  
                using (CryptoStream cryptoStream = new CryptoStream(memoryStream, _decryptor, CryptoStreamMode.Read))  
                {  
                    using (StreamReader streamReader = new StreamReader(cryptoStream))  
                    {  
                        return streamReader.ReadToEnd();  
                    }  
                }  
            }  
        }
    }
}