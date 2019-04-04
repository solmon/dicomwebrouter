using CorePacs.Dicom.Contracts;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace CorePacs.Dicom.Server
{
    public class EncryptionServer : CoreServer, IEncryptionServer
    {
        private readonly IImageEncryptionService _imageEncryption;
        
        public EncryptionServer(IImageEncryptionService imageEncryption) {
            if (imageEncryption == null) throw new ArgumentNullException(nameof(imageEncryption));
            this._imageEncryption = imageEncryption;
            this._service = imageEncryption;
        }        
    }
}
