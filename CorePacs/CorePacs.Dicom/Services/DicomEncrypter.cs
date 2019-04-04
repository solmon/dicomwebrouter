using CorePacs.Dicom.Contracts;
using System;
using System.Collections.Generic;
using System.Text;
using CorePacs.DataAccess.Domain;
using Dicom;
using CorePacs.DataAccess.Contracts;

namespace CorePacs.Dicom.Services
{
    public class DicomEncrypter : IDicomEncrypter
    {
        private readonly IEncrypter _encrypter;
        public DicomEncrypter(IEncrypter encrypter)
        {
            if (encrypter == null) throw new ArgumentNullException(nameof(encrypter));
            this._encrypter = encrypter;
        }
        public void Decrypt(DicomDataset instance)
        {
            var patientName = instance.Get(DicomTag.PatientName, string.Empty);
            instance.AddOrUpdate(DicomTag.PatientName, this._encrypter.Decrypt(patientName));
        }

        public void Encrypt(DicomDataset instance)
        {
            var patientName = instance.Get(DicomTag.PatientName,string.Empty);
            instance.AddOrUpdate(DicomTag.PatientName, this._encrypter.Encrypt(patientName));
        }
    }
}
