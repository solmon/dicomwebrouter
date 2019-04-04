using CorePacs.Dicom.Contracts;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace CorePacs.Dicom.Server
{
    public class LinkSendServer : CoreServer, ILinkSendServer
    {
        private readonly ILinkSendService _linkSendService;        
        public LinkSendServer(ILinkSendService linkSendService)
        {
            if (linkSendService == null) throw new ArgumentNullException(nameof(linkSendService));
            this._linkSendService = linkSendService;
            this._service = linkSendService;
        }        
    }
}
