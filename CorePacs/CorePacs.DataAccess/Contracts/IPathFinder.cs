using CorePacs.DataAccess.Domain;
using System;
using System.Collections.Generic;
using System.Text;

namespace CorePacs.DataAccess.Contracts
{
    public interface IPathFinder
    {
        string GetStoragePath(DicomRequestAttrs dicomAttrs);
        string GetStoragePath(Instance instance);
        string GetStoragePathForEncrypted(Instance instance);
        string GetStoragePathForLinkRecieve(Instance instance);
        string GetStoragePathForLinkSend(Instance instance);
        string GetStoragePathForDicomSend(Instance instance);
    }
}
