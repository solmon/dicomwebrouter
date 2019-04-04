using CorePacs.DataAccess.Contracts;
using CorePacs.Dicom.Contracts;
using Dicom;
using System;
using System.Collections.Generic;
using System.Text;

namespace CorePacs.Dicom.Services
{   
    public class DicomSendService : IDicomSendService
    {
        private readonly IStorageRepository _storageRepository;
        private readonly IPathFinder _pathFinder;
        private readonly IDicomClient _dicomClient;
        private readonly IRouteFinder _routeFinder;
        public DicomSendService(IStorageRepository storageRepository, IDicomClient dicomClient, IPathFinder pathFinder,IRouteFinder routeFinder)
        {
            if (storageRepository == null) throw new ArgumentNullException(nameof(storageRepository));
            this._storageRepository = storageRepository;

            if (dicomClient == null) throw new ArgumentNullException(nameof(dicomClient));
            this._dicomClient = dicomClient;

            if (pathFinder == null) throw new ArgumentNullException(nameof(pathFinder));
            this._pathFinder = pathFinder;

            if (routeFinder == null) throw new ArgumentNullException(nameof(routeFinder));
            this._routeFinder = routeFinder;
        }
        private bool _isRunning = false;
        public bool isRunning => _isRunning;

        public void Start()
        {
            _isRunning = true;
            var foundJob = true;
            while (foundJob && _isRunning)
            {
                var jobFound = _storageRepository.GetInstancesForLinkDicomSend().GetAwaiter().GetResult();
                if (jobFound.Count > 0)
                {
                    try
                    {
                        foreach (var instance in jobFound)
                        {
                            try
                            {
                                var dFile = DicomFile.Open(this._pathFinder.GetStoragePathForDicomSend(instance));
                                var dSendRoute = this._routeFinder.DicomRoute(instance);
                                var resp = this._dicomClient.Transmit(dSendRoute, dFile).GetAwaiter().GetResult();
                                //Clean up the images that have been sent to dicom push.
                                instance.isDicomPushed = resp.isSuccess;
                                instance.ErrorMessage = resp.Error;
                                this._storageRepository.UpdateInstance(instance).GetAwaiter().GetResult();
                            }
                            catch (Exception dEncry)
                            {
                                instance.isDicomPushed = false;
                                instance.ErrorMessage = dEncry.Message;
                                this._storageRepository.UpdateInstance(instance).GetAwaiter().GetResult();
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
