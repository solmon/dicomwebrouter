using CorePacs.DataAccess.Domain;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace CorePacs.Dicom.Contracts
{
    public interface IRouteFinder
    {
        Task<bool> Refresh();
        LinkClient LinkRoute(Instance instance);
        DicomSend DicomRoute(Instance instance);
    }
}
