using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace CorePacs.DataAccess.Domain
{
    public class AETitles
    {
        [Key]
        public Guid AEId { get; set; }
        public string AETitle { get; set; }
        public string RemoteHost { get; set; }
        public bool isActive { get; set; }
    }
}
