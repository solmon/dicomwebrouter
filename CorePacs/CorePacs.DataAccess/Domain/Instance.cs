using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace CorePacs.DataAccess.Domain
{
    public class Instance
    {
        [Key]
        public Guid InstanceId { get; set; }
        public Guid SeriesId { get; set; }
        public string SOPInstanceUID { get; set; }
        public bool isEncrypted { get; set; }
        public bool isLinkedTransmitted { get; set; }
        public bool isLinkTramitFail { get; set; }
        public string ErrorMessage { get; set; }
        public bool isDecrypted { get; set; }
        public bool isLinkRecieved { get; set; }
        public bool isDicomPushed { get; set; }
        public string CalledAE { get; set; }
        public string Priority { get; set; }
        public string RemoteHostIP { get; set; }
        public string StudyInstanceUID { get; set; }
        public string SeriesInstanceUID { get; set; }
    }
}
