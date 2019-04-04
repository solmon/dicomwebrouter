using CorePacs.DataAccess.Contracts;
using CorePacs.Dicom.Contracts;
using Dicom;
using System;
using System.Collections.Generic;
using System.Text;

namespace CorePacs.Dicom.Services
{
    public class DecryptionService : IDecryptionService
    {
        private readonly IStorageRepository _storageRepository;
        private readonly IDicomEncrypter _dicomEncrypter;
        private readonly IPathFinder _pathFinder;
        public DecryptionService(IStorageRepository storageRepository, IDicomEncrypter dicomEncrypter, IPathFinder pathFinder)
        {
            if (storageRepository == null) throw new ArgumentNullException(nameof(storageRepository));
            this._storageRepository = storageRepository;

            if (dicomEncrypter == null) throw new ArgumentNullException(nameof(dicomEncrypter));
            this._dicomEncrypter = dicomEncrypter;

            if (pathFinder == null) throw new ArgumentNullException(nameof(pathFinder));
            this._pathFinder = pathFinder;
        }
        private bool _isRunning = false;
        public bool isRunning => _isRunning;

        public void Start()
        {
            _isRunning = true;
            var foundJob = true;
            while (foundJob && _isRunning)
            {
                var jobFound = _storageRepository.GetInstancesForDecryption().GetAwaiter().GetResult();
                if (jobFound.Count > 0)
                {
                    try
                    {
                        foreach (var instance in jobFound)
                        {
                            try
                            {
                                var dFile = DicomFile.Open(this._pathFinder.GetStoragePathForLinkRecieve(instance));
                                this._dicomEncrypter.Decrypt(dFile.Dataset);
                                dFile.Save(this._pathFinder.GetStoragePathForDicomSend(instance));
                                instance.isDecrypted = true;
                                this._storageRepository.UpdateInstance(instance).GetAwaiter().GetResult();
                            }
                            catch (Exception dEncry)
                            {

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
