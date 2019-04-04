using System;
using System.Collections.Generic;
using System.Text;
using System.IO;
using CorePacs.DataAccess.Contracts;
using CorePacs.DataAccess.Config;
using CorePacs.DataAccess.Domain;

namespace CorePacs.DataAccess.Storage
{
    public class FileStorage : IStorage
    {
        private readonly IPathFinder _pathFinder;
        public FileStorage(IPathFinder pathFinder) {
            if (pathFinder == null) throw new ArgumentNullException(nameof(pathFinder));
            this._pathFinder = pathFinder;
        }
        public string GetStoragePath(DicomRequestAttrs dicomAttrs)
        {
            return this._pathFinder.GetStoragePath(dicomAttrs);            
        }               
    }
}
