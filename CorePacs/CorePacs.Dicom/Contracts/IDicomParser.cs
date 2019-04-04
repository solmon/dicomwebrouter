using CorePacs.DataAccess.Contracts;
using CorePacs.DataAccess.Domain;
using Dicom.Network;
using System;
using System.Collections.Generic;
using System.Text;

namespace CorePacs.Dicom.Contracts
{
    public interface IDicomParser
    {
        DicomRequestAttrs Extract(DicomService service,DicomCStoreRequest request);
        DicomRequestAttrs Extract(Instance instance,IPathFinder pathFinder);
    }
}
