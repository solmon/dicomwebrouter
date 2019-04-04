using CorePacs.DataAccess.Domain;
using CorePacs.Dicom.Messages;
using Dicom;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace CorePacs.Dicom.Contracts
{
    public interface IDicomClient
    {
        Task<DicomSendResponse> Transmit(DicomSend dicomRoute, DicomFile dFile);
    }
}
