using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace CorePacs.DataAccess.Domain
{
    public class Study
    {
        [Key]
        public Guid StudyId { get; set; }
        public string CalledAE { get; set; }
        public string RemoteHostIP { get; set; }
        public DateTime RecievedTime { get; set; }
        public string PatientName { get; set; }
        public string Priority { get; set; }
        public DateTime AcquisitionDateTime { get; set; }
        public string StudyDescription { get; set; }
        public string StudyInstanceUID { get; set; }
        public int SeriesCount { get; set; }
        public bool isEncrypted { get; set; }
        public bool isLinkedTransmitted { get; set; }
        public bool isDecrypted { get; set; }
        public bool isDicomPushed { get; set; }

        [NotMapped]
        public int RecievedSeries {
            get {
                return this.Series==null? 0: this.Series.Count;
            }
        }

        [NotMapped]
        public int TotalImages {
           get {
                var totalImages = 0;
                this.Series.ForEach(x => totalImages += x.InstanceCount);
                return totalImages;
           }
        }

        [NotMapped]
        public int TotalImagesRecieved {
            get
            {
                var totalImages = 0;
                this.Series.ForEach(x => totalImages += x.InstanceRecieved);
                return totalImages;
            }
        }

        [NotMapped]
        public bool isComplete {
            get {
                return TotalImages == TotalImagesRecieved;
            }
        }

        [ForeignKey("StudyId")]
        public List<Series> Series {get;set;}
    }
}
