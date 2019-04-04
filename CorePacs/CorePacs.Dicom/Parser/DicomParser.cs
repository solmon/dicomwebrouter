using System;
using System.Collections.Generic;
using System.Text;
using Dicom.Network;
using Dicom;
using CorePacs.Dicom.Contracts;
using CorePacs.DataAccess.Domain;
using CorePacs.DataAccess.Contracts;

namespace CorePacs.Dicom
{
    public class DicomParser : IDicomParser
    {        
        public DicomRequestAttrs Extract(DicomService service, DicomCStoreRequest request)
        {
            var acqDate = request.Dataset.Get(DicomTag.AcquisitionDate, string.Empty);
            var acqDateTime = request.Dataset.Get(DicomTag.AcquisitionDateTime, string.Empty);
            var acqTime = request.Dataset.Get(DicomTag.AcquisitionTime, string.Empty);
            var acquisitionDate = buildAcquisitionDateTime(acqDate, acqTime);

            return new DicomRequestAttrs()
            {
                CalledAE = service.Association.CalledAE,
                AcquisitionDateTime = acquisitionDate,
                Modality = request.Dataset.Get(DicomTag.Modality, string.Empty),
                PatientName = request.Dataset.Get(DicomTag.PatientName, string.Empty),
                Priority = request.Priority.ToString(),
                RemoteHostIP = service.Association.RemoteHost,
                SeriesInstanceUID = request.Dataset.Get(DicomTag.SeriesInstanceUID, string.Empty),
                SOPInstanceUID = request.Dataset.Get(DicomTag.SOPInstanceUID, string.Empty),
                StudyDescription = request.Dataset.Get(DicomTag.StudyDescription, string.Empty),
                StudyInstanceUID = request.Dataset.Get(DicomTag.StudyInstanceUID, string.Empty),
                SeriesCount = request.Dataset.Get(DicomTag.NumberOfStudyRelatedSeries, 1),
                ImageCount = request.Dataset.Get(DicomTag.NumberOfSeriesRelatedInstances,1)
            };
        }

        public DicomRequestAttrs Extract(Instance instance, IPathFinder pathFinder)
        {
            var dFile = DicomFile.Open(pathFinder.GetStoragePathForLinkRecieve(instance));
            
                var acqDate = dFile.Dataset.Get(DicomTag.AcquisitionDate, string.Empty);
                var acqDateTime = dFile.Dataset.Get(DicomTag.AcquisitionDateTime, string.Empty);
                var acqTime = dFile.Dataset.Get(DicomTag.AcquisitionTime, string.Empty);
                var acquisitionDate = buildAcquisitionDateTime(acqDate, acqTime);

            return new DicomRequestAttrs()
            {
                CalledAE = instance.CalledAE,
                AcquisitionDateTime = acquisitionDate,
                Modality = dFile.Dataset.Get(DicomTag.Modality, string.Empty),
                PatientName = dFile.Dataset.Get(DicomTag.PatientName, string.Empty),
                Priority = instance.Priority,
                RemoteHostIP = instance.RemoteHostIP,
                SeriesInstanceUID = dFile.Dataset.Get(DicomTag.SeriesInstanceUID, string.Empty),
                SOPInstanceUID = dFile.Dataset.Get(DicomTag.SOPInstanceUID, string.Empty),
                StudyDescription = dFile.Dataset.Get(DicomTag.StudyDescription, string.Empty),
                StudyInstanceUID = dFile.Dataset.Get(DicomTag.StudyInstanceUID, string.Empty),
                SeriesCount = dFile.Dataset.Get(DicomTag.NumberOfStudyRelatedSeries, 1),
                ImageCount = dFile.Dataset.Get(DicomTag.NumberOfSeriesRelatedInstances, 1)
            };            
        }

        private DateTime buildAcquisitionDateTime(string date,string time) {
            Decimal timeTicks;
            var timeInstring = "12:00 AM";
            var sYear = DateTime.Now.Year.ToString();
            var sMonth = DateTime.Now.Month.ToString();
            var sDay = DateTime.Now.Day.ToString();

            if (Decimal.TryParse(time, out timeTicks)) {
                var timeTicksInLong = (long)timeTicks;
                timeInstring = DateTime.FromFileTime(timeTicksInLong).ToShortTimeString();
            }
            if (date.Length == 8) {
                sYear = date.Substring(0, 4);
                sMonth = date.Substring(4, 2);
                sDay = date.Substring(6, 2);
            }
            var sDate = string.Format("{0}/{1}/{2} {3}",sMonth,sDay,sYear,timeInstring);
            DateTime sReturn = DateTime.Now;
            DateTime.TryParse(sDate, out sReturn);
            return sReturn;
        }
    }
}
