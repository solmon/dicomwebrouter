using CorePacs.DataAccess.Config;
using CorePacs.DataAccess.Contracts;
using CorePacs.Dicom.Contracts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace CorePacs.LinkClient
{
    public class App
    {
        public static bool ISSTOP = false;
        private static Timer _timer;
        private readonly IDecryptionServer _decryptionServer;
        private readonly IStorageRepository _storageRepository;
        private readonly CorePacsSettings _coreSettings;
        private readonly IRouteFinder _routeFinder;
        private readonly IDicomSendServer _dicomSendServer;
        public App( IDecryptionServer decryptionServer,
                    IStorageRepository storageRepository, 
                    CorePacsSettings coreSettings,
                    IRouteFinder routeFinder,
                    IDicomSendServer dicomSendServer)
        {
            if (decryptionServer == null) throw new ArgumentNullException(nameof(decryptionServer));
            this._decryptionServer = decryptionServer;

            if (storageRepository == null) throw new ArgumentNullException(nameof(storageRepository));
            if (coreSettings == null) throw new ArgumentNullException(nameof(coreSettings));
            if (routeFinder == null) throw new ArgumentNullException(nameof(routeFinder));
            if (dicomSendServer == null) throw new ArgumentNullException(nameof(dicomSendServer));
            this._storageRepository = storageRepository;
            this._coreSettings = coreSettings;
            this._routeFinder = routeFinder;
            this._dicomSendServer = dicomSendServer;
            Build();
            this._routeFinder.Refresh();
        }

        private bool Build()
        {
            var settings = this._storageRepository.GetSettings().GetAwaiter().GetResult();
            var aeTitles = this._storageRepository.GetAETitles().GetAwaiter().GetResult();
            this._coreSettings.Build(settings, aeTitles);
            this._storageRepository.Dispose();
            return true;
        }

        public Task<bool> Run(ManualResetEvent resetEvent)
        {
            try
            {
                Console.Write("CORE - INSIDE RUN");                
                _decryptionServer.Start();
                _dicomSendServer.Start();
                _timer = new Timer(x => {
                    if (ISSTOP)
                    {
                        _decryptionServer.Stop();
                        _dicomSendServer.Stop();
                        resetEvent.Set();                        
                    }
                }, null, Timeout.Infinite, Timeout.Infinite);
                Setup_Timer();

            }
            catch (Exception ex) {
                Console.Write(ex);
                resetEvent.Set();
                throw ex;
            }            
            return Task.FromResult(true);
        }

        private static void Setup_Timer()
        {
            DateTime timerRunningTime = DateTime.Now.AddMinutes(1);
            double tickTime = (double)(timerRunningTime - DateTime.Now).TotalSeconds;
            _timer.Change(TimeSpan.FromSeconds(tickTime), TimeSpan.FromSeconds(tickTime));
        }
    }
}
