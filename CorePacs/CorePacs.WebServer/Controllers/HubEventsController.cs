using CorePacs.DataAccess.Contracts;
using CorePacs.WebServer.Hubs;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
using System;
using System.Collections.Generic;
using System.Text;

namespace CorePacs.WebServer.Controllers
{
    [Route("api/v1/[controller]")]
    public class HubEventsController : Controller
    {
        private readonly IHubContext<IncomingHub> _hubContext;
        private readonly IStorageRepository _storageRepository;
        public HubEventsController(IStorageRepository storageRepository,IHubContext<IncomingHub> hubContext)
        {
            if (storageRepository == null) throw new ArgumentNullException(nameof(storageRepository));
            if (hubContext == null) throw new ArgumentNullException(nameof(hubContext));
            this._hubContext = hubContext;
            this._storageRepository = storageRepository;
        }

        [HttpGet]
        [Route("addnewfile")]
        public IActionResult GetNewFile()
        {
            _hubContext.Clients.All.InvokeAsync("newincoming", this._storageRepository.GetStudies().GetAwaiter().GetResult());
            return Ok();
        }
    }
}
