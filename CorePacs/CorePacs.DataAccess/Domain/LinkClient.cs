using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace CorePacs.DataAccess.Domain
{
    public class LinkClient
    {
        [Key]
        public Guid LinkClientId { get; set; }
        public DateTime RegisteredTime { get; set; }
        public bool isActive { get; set; }        
        public string UrlEndPoint { get; set; }
        public bool ErrorMessage { get; set; }        
    }    
}
