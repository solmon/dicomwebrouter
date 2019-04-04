using CorePacs.Dicom.Contracts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace CorePacs
{
    public class App
    {
        public static bool ISSTOP = false;
        private static Timer _timer;
        private readonly IPacsServer _server;
        private readonly IEncryptionServer _encryptionServer;
        private readonly ILinkSendServer _linkSendServer;
        private readonly IRouteFinder _routeFinder;

        public App( IPacsServer server, 
                    IEncryptionServer encryptionServer,
                    ILinkSendServer linkSendServer,
                    IRouteFinder routeFinder)
        {
            if (server == null) throw new ArgumentNullException(nameof(server));
            if (encryptionServer == null) throw new ArgumentNullException(nameof(encryptionServer));
            if (linkSendServer == null) throw new ArgumentNullException(nameof(linkSendServer));
            if (routeFinder == null) throw new ArgumentNullException(nameof(routeFinder));

            this._server = server;
            this._encryptionServer = encryptionServer;
            this._linkSendServer = linkSendServer;
            this._routeFinder = routeFinder;
            this._server.Build();
            this._routeFinder.Refresh();
        }

        public Task<bool> Run(ManualResetEvent resetEvent)
        {
            try
            {
                Console.Write("CORE - INSIDE RUN");                
                _server.Start();
                _encryptionServer.Start();
                _linkSendServer.Start();
                _timer = new Timer(x => {
                    if (ISSTOP)
                    {
                        _server.Stop();
                        _encryptionServer.Stop();
                        _linkSendServer.Stop();
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
