using CorePacs.DataAccess.Contracts;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Text;

namespace CorePacs.WebServer.Controllers
{
    public abstract class MasterBaseController<Entity, IdT> : Controller
        where Entity : class, new()
    {
        protected IMasterRepository<Entity, IdT> _masterRepository;

        [HttpGet]
        [Route("all")]
        public IActionResult Get()
        {
            return Ok(this._masterRepository.Get().GetAwaiter().GetResult());
        }
        
        [HttpPost]
        [Route("add")]
        public IActionResult Add([FromBody] Entity aETitles)
        {
            return Ok(this._masterRepository.Add(aETitles).GetAwaiter().GetResult());
        }

        [HttpPost]
        [Route("update")]
        public IActionResult Update([FromBody] Entity aETitles)
        {
            return Ok(this._masterRepository.Update(aETitles).GetAwaiter().GetResult());
        }
    }
}
