using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace CorePacs.DataAccess.Domain
{
    public class Series
    {
        [Key]
        public Guid SeriesId { get; set; }
        public Guid StudyId { get; set; }
        public string SeriesInstanceUID { get; set; }
        public int InstanceCount { get; set; }
        public string Modality { get; set; }
        public bool isEncrypted { get; set; }
        public bool isLinkedTransmitted { get; set; }

        [NotMapped]
        public int InstanceRecieved {
            get {
                return this.Instances==null? 0: this.Instances.Count;
            }
        }

        [ForeignKey("SeriesId")]
        public List<Instance> Instances { get; set; }
    }
}
