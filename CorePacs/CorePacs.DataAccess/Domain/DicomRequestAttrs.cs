using System;
using System.Collections.Generic;
using System.Text;

namespace CorePacs.DataAccess.Domain
{
    public class DicomRequestAttrs
    {
        public string CalledAE { get; set; }
        public string RemoteHostIP { get; set; }
        public DateTime RecievedTime { get; set; }
        public string PatientName { get; set; }
        public string Priority { get; set; }
        public DateTime AcquisitionDateTime { get; set; }
        public string StudyDescription { get; set; }
        public string StudyInstanceUID { get; set; }
        public string SeriesInstanceUID { get; set; }
        public string SOPInstanceUID { get; set; }
        public string Modality { get; set; }
        public int SeriesCount { get; set; }
        public int ImageCount { get; set; }
    }
}
