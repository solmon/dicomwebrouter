using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace CorePacs.DataAccess.Domain
{
    public class DicomSend
    {
        [Key]
        public Guid DicomSendId { get; set; }
        public DateTime RegisteredTime { get; set; }
        public bool isActive { get; set; }        
        public string AETitle { get; set; }
        public string Port { get; set; }
        public string RemoteHost { get; set; }
        public string CallingAETitle { get; set; }
    }
}
