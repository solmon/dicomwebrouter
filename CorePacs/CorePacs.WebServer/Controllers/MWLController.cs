using CorePacs.DataAccess.Contracts;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Text;

namespace CorePacs.WebServer.Controllers
{
    [Route("api/v1/[controller]")]
    public class MWLController: Controller
    {
        private readonly IStorageRepository _storageRepository;
        public MWLController(IStorageRepository storageRepository) {
            if (storageRepository == null) throw new ArgumentNullException(nameof(storageRepository));
            this._storageRepository = storageRepository;
        }

        [HttpGet]
        [Route("getMe")]
        public IActionResult Get()
        {
            return Ok(this._storageRepository.GetStudies().GetAwaiter().GetResult());
        }
    }
}
