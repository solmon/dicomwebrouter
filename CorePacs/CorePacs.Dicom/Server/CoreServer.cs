using CorePacs.Dicom.Contracts;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace CorePacs.Dicom.Server
{
    public abstract class CoreServer:IServer
    {
        public string ServerName { get; protected set; }
        protected IService _service;
        CancellationTokenSource _ts;
        public void Start()
        {
            try
            {
                if (_ts != null) _ts.Cancel();
            }
            catch (Exception ex) {

            }
            _ts = new CancellationTokenSource();
            CancellationToken ct = _ts.Token;
            Task.Factory.StartNew(() =>
            {
                this._service.Start();
            }, ct);
        }

        public void Stop()
        {
            try
            {
                if (_ts != null) _ts.Cancel();
            }
            catch (Exception ex) { }
        }
    }
}
