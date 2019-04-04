using CorePacs.DataAccess.Config;
using CorePacs.DataAccess.Contracts;
using CorePacs.Dicom.Contracts;
using Dicom.Network;
using System;
using System.Collections.Generic;
using System.Text;

namespace CorePacs.Dicom.Server
{
    public class PacsServer : IPacsServer
    {
        private IDicomServer _server;
        public string ServerName { get; protected set; }
        private readonly IStorageRepository _storageRepository;
        private readonly CorePacsSettings _coreSettings;
        public PacsServer(IStorageRepository storageRepository, CorePacsSettings coreSettings) {
            if (storageRepository == null) throw new ArgumentNullException(nameof(storageRepository));
            if (coreSettings == null) throw new ArgumentNullException(nameof(coreSettings));
            this._storageRepository = storageRepository;
            this._coreSettings = coreSettings;
        }

        public bool Build() {
            var settings = this._storageRepository.GetSettings().GetAwaiter().GetResult();
            var aeTitles = this._storageRepository.GetAETitles().GetAwaiter().GetResult();
            this._coreSettings.Build(settings, aeTitles);
            this._storageRepository.Dispose();
            return true;
        }

        public void Start()
        {
            _server = DicomServer.Create<SimpleCStoreProvider>(12345);            
        }

        public void Stop()
        {
            _server.Stop();            
        }
    }
}
