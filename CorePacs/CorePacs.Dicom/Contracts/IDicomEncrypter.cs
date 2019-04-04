using CorePacs.DataAccess.Domain;
using Dicom;
using System;
using System.Collections.Generic;
using System.Text;

namespace CorePacs.Dicom.Contracts
{
    public interface IDicomEncrypter
    {
        void Encrypt(DicomDataset instance);
        void Decrypt(DicomDataset instance);
    }
}
