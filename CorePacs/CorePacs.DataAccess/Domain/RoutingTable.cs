using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace CorePacs.DataAccess.Domain
{
    public class RoutingTable
    {
        [Key]
        public Guid RoutingId { get; set; }
        public DateTime RegisteredTime { get; set; }
        public bool isActive { get; set; }
        public string InComing { get; set; }
        public Guid OutGoing { get; set; }
        public bool isLinkRoute { get; set; }
        public bool isDSendRoute { get; set; }
    }
}
