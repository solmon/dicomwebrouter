using System;
using System.Collections.Generic;
using System.Text;

namespace CorePacs.Dicom.Contracts
{
    public interface IEncrypter
    {
        void GenerateKey();
        string Encrypt(string value);
        string Decrypt(string value);
    }
}
