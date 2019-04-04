using CorePacs.DataAccess.Contracts;
using System;
using System.Collections.Generic;
using System.Text;
using CorePacs.DataAccess.Domain;
using System.IO;
using CorePacs.DataAccess.Config;

namespace CorePacs.DataAccess.Storage
{
    public class PathFinder : IPathFinder
    {
        private readonly CorePacsSettings _coreSettings;
        public PathFinder(CorePacsSettings coreSettings) {
            if (coreSettings == null) throw new ArgumentNullException(nameof(coreSettings));
            this._coreSettings = coreSettings;
        }
        public string GetStoragePath(DicomRequestAttrs dicomAttrs)
        {
            var folderPath = this._coreSettings.BasePath + "\\" + dicomAttrs.CalledAE + "\\" + dicomAttrs.StudyInstanceUID + "\\" + dicomAttrs.SeriesInstanceUID;
            isFolderExistsElseCreate(folderPath);
            return folderPath + "\\" + dicomAttrs.SOPInstanceUID + ".dcm";
        }

        public string GetStoragePath(Instance instance)
        {
            var folderPath = this._coreSettings.BasePath + "\\" + instance.CalledAE + "\\" + instance.StudyInstanceUID + "\\" + instance.SeriesInstanceUID;
            isFolderExistsElseCreate(folderPath);
            return folderPath + "\\" + instance.SOPInstanceUID + ".dcm";            
        }

        public string GetStoragePathForDicomSend(Instance instance)
        {
            var folderPath = this._coreSettings.BasePath + "\\ForDicomSend\\" + instance.CalledAE + "\\" + instance.StudyInstanceUID + "\\" + instance.SeriesInstanceUID;
            isFolderExistsElseCreate(folderPath);
            return folderPath + "\\" + instance.SOPInstanceUID + ".dcm";
        }

        public string GetStoragePathForEncrypted(Instance instance)
        {
            var folderPath = this._coreSettings.BasePath + "\\Encrpted\\" + instance.CalledAE + "\\" + instance.StudyInstanceUID + "\\" + instance.SeriesInstanceUID;
            isFolderExistsElseCreate(folderPath);
            return folderPath + "\\" + instance.SOPInstanceUID + ".dcm";
        }

        public string GetStoragePathForLinkRecieve(Instance instance)
        {
            var folderPath = this._coreSettings.BasePath + "\\LinkRecieve\\" + instance.CalledAE + "\\" + instance.StudyInstanceUID + "\\" + instance.SeriesInstanceUID;
            isFolderExistsElseCreate(folderPath);
            return folderPath + "\\" + instance.SOPInstanceUID + ".dcm";
        } 
        
        public string GetStoragePathForLinkSend(Instance instance)
        {
            var folderPath = this._coreSettings.BasePath + "\\LinkSend\\" + instance.CalledAE + "\\" + instance.StudyInstanceUID + "\\" + instance.SeriesInstanceUID;
            isFolderExistsElseCreate(folderPath);
            return folderPath + "\\" + instance.SOPInstanceUID + ".dcm";
        }

        private bool isFolderExistsElseCreate(string folder)
        {
            var isExists = Directory.Exists(folder);
            if (!isExists) Directory.CreateDirectory(folder);
            return true;
        }
    }
}
