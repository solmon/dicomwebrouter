using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace CorePacs.DataAccess.Domain
{
    public class ProcessTracker
    {
        [Key]
        public int Id { get; set; }
        public string Name { get; set; }
        public bool IsStop { get; set; }
        public bool IsRunning { get; set; }
        public DateTime HeartBeat { get; set; }
    }
}
