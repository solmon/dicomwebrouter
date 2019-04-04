using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.AspNetCore.SignalR;
using System.Threading.Tasks;
using CorePacs.DataAccess.Contracts;

namespace CorePacs.WebServer.Hubs
{
    public class IncomingHub : Hub
    {
        private readonly IStorageRepository _storageRepository;
        public IncomingHub(IStorageRepository storageRepository)
        {
            if (storageRepository == null) throw new ArgumentNullException(nameof(storageRepository));
            this._storageRepository = storageRepository;
        }        
        public Task NewFileIncoming()
        {
            return Clients.All.InvokeAsync("newincoming", this._storageRepository.GetStudies().GetAwaiter().GetResult());
        }
    }
}
