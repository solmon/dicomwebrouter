using CorePacs.DataAccess.Contracts;
using CorePacs.Dicom.Contracts;
using Dicom;
using Dicom.IO;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;

namespace CorePacs.Dicom.Services
{
    public class ImageEncryption : IImageEncryptionService
    {
        private readonly IStorageRepository _storageRepository;
        private readonly IDicomEncrypter _dicomEncrypter;
        private readonly IPathFinder _pathFinder;
        public ImageEncryption(IStorageRepository storageRepository, IDicomEncrypter dicomEncrypter,IPathFinder pathFinder) {
            if (storageRepository == null) throw new ArgumentNullException(nameof(storageRepository));
            this._storageRepository = storageRepository;

            if (dicomEncrypter == null) throw new ArgumentNullException(nameof(dicomEncrypter));
            this._dicomEncrypter = dicomEncrypter;

            if (pathFinder == null) throw new ArgumentNullException(nameof(pathFinder));
            this._pathFinder = pathFinder;
        }
        private bool _isRunning=false;
        public bool isRunning => _isRunning;

        public void Start()
        {
            _isRunning = true;
            var foundJob = true;            
            while (foundJob && _isRunning)
            {
                var jobFound = _storageRepository.GetInstancesForEncryption().GetAwaiter().GetResult();
                if (jobFound.Count > 0 )
                {
                    try
                    {
                        foreach (var instance in jobFound) {
                            try
                            {
                                var dFile = DicomFile.Open(this._pathFinder.GetStoragePath(instance));
                                this._dicomEncrypter.Encrypt(dFile.Dataset);
                                dFile.Save(this._pathFinder.GetStoragePathForEncrypted(instance));
                                instance.isEncrypted = true;
                                this._storageRepository.UpdateInstance(instance).GetAwaiter().GetResult();

                            }
                            catch (Exception dEncry) {

                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        
                    }
                }
                else
                {
                    foundJob = false;
                    System.Threading.Thread.Sleep(1000);
                    foundJob = true;
                }
            }            
        }

        public void Stop()
        {
            _isRunning = false;
        }
    }
}
