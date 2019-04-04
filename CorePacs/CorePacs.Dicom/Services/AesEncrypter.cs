using CorePacs.Dicom.Config;
using CorePacs.Dicom.Contracts;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.IO;
using System.Security.Cryptography;
using System.Text;

namespace CorePacs.Dicom.Services
{
    public class AesEncrypter : IEncrypter
    {
        public LinkKey Key { get; set; }
        private Byte[] _key;
        private Byte[] _iv;
        public AesEncrypter(IOptions<LinkKey> options) {
            if (options == null) throw new ArgumentNullException(nameof(options));
            this.Key = options.Value;
            buildKey();
        }

        private void buildKey() {
            _key = Convert.FromBase64String(this.Key.Key);
            _iv = Convert.FromBase64String(this.Key.IV);
        }
        public string Decrypt(string value)
        {
            string plaintext = null;
            using (RijndaelManaged rijAlg = new RijndaelManaged())
            {
                rijAlg.Key = _key;
                rijAlg.IV = _iv;
                
                ICryptoTransform decryptor = rijAlg.CreateDecryptor(rijAlg.Key, rijAlg.IV);

                using (MemoryStream msDecrypt = new MemoryStream(Convert.FromBase64String(value)))
                {
                    using (CryptoStream csDecrypt = new CryptoStream(msDecrypt, decryptor, CryptoStreamMode.Read))
                    {
                        using (StreamReader srDecrypt = new StreamReader(csDecrypt))
                        {                            
                            plaintext = srDecrypt.ReadToEnd();
                        }
                    }
                }

            }
            return plaintext;
        }

        public string Encrypt(string value)
        {            
            byte[] encrypted;            
            using (RijndaelManaged rijAlg = new RijndaelManaged())
            {
                rijAlg.Key = _key;
                rijAlg.IV = _iv;                
                ICryptoTransform encryptor = rijAlg.CreateEncryptor(rijAlg.Key, rijAlg.IV);
                using (MemoryStream msEncrypt = new MemoryStream())
                {
                    using (CryptoStream csEncrypt = new CryptoStream(msEncrypt, encryptor, CryptoStreamMode.Write))
                    {
                        using (StreamWriter swEncrypt = new StreamWriter(csEncrypt))
                        {                            
                            swEncrypt.Write(value);
                        }
                        encrypted = msEncrypt.ToArray();
                    }
                }
            }            
            return Convert.ToBase64String(encrypted);
        }

        public void GenerateKey()
        {
            if (string.IsNullOrEmpty(this.Key.Key) || string.IsNullOrEmpty(this.Key.IV))
            {
                using (RijndaelManaged myRijndael = new RijndaelManaged())
                {
                    myRijndael.GenerateKey();
                    myRijndael.GenerateIV();
                    
                    var fStream = File.Create(Environment.CurrentDirectory + "\\new.key");
                    fStream.Close();
                    File.AppendAllLines(Environment.CurrentDirectory + "\\new.key", 
                        new string[] { Convert.ToBase64String(myRijndael.Key) , Convert.ToBase64String(myRijndael.IV) });
                }
            }
        }
    }
}
