using CorePacs.Dicom.Contracts;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace CorePacs.Dicom.Server
{
    public class DecryptionServer: CoreServer, IDecryptionServer
    {
        private readonly IDecryptionService _imageDecryption;        
        public DecryptionServer(IDecryptionService imageDecryption)
        {
            if (imageDecryption == null) throw new ArgumentNullException(nameof(imageDecryption));
            this._imageDecryption = imageDecryption;
            this._service = (IService)this._imageDecryption;
        }        
    }
}
