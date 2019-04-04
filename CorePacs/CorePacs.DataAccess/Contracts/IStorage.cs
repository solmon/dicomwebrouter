using CorePacs.DataAccess.Domain;
using System;
using System.Collections.Generic;
using System.Text;

namespace CorePacs.DataAccess.Contracts
{
    public interface IStorage
    {
        string GetStoragePath(DicomRequestAttrs dicomAttrs);
    }
}
