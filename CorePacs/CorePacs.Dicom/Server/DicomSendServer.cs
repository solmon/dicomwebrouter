using CorePacs.Dicom.Contracts;
using System;
using System.Collections.Generic;
using System.Text;

namespace CorePacs.Dicom.Server
{
    public class DicomSendServer : CoreServer, IDicomSendServer
    {
        private readonly IDicomSendService _dicomSendService;
        public DicomSendServer(IDicomSendService dicomSendService)
        {
            if (dicomSendService == null) throw new ArgumentNullException(nameof(dicomSendService));
            this._dicomSendService = dicomSendService;
            this._service = this._dicomSendService;
        }        
    }
}
