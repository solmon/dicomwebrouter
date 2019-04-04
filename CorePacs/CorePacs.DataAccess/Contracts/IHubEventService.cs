using CorePacs.DataAccess.Hub;
using System;
using System.Collections.Generic;
using System.Text;

namespace CorePacs.DataAccess.Contracts
{
    public interface IHubEventService
    {
        event EventHandler IncomingFile;
        void NewIncomingFile(IncomingFileEventArgs evtArgs);
    }
}
