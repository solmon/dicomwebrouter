using CorePacs.Core.ApiInvoker;
using CorePacs.Core.ApiInvoker.Contracts;
using CorePacs.Core.ApiInvoker.Models;
using CorePacs.DataAccess.Contracts;
using CorePacs.Dicom.Contracts;
using System;
using System.Collections.Generic;
using System.Text;

namespace CorePacs.Dicom.Services
{
    public class LinkSendService : ILinkSendService
    {
        private readonly IStorageRepository _storageRepository;
        private readonly IPathFinder _pathFinder;
        private readonly IServiceInvoker _serviceInvoker;
        private readonly IRouteFinder _routeFinder;
        const string HUBURL = "FileUpload";
        public LinkSendService(IStorageRepository storageRepository, IPathFinder pathFinder, IServiceInvoker serviceInvoker,IRouteFinder routeFinder)
        {
            if (storageRepository == null) throw new ArgumentNullException(nameof(storageRepository));
            this._storageRepository = storageRepository;

            if (serviceInvoker == null) throw new ArgumentNullException(nameof(serviceInvoker));
            this._serviceInvoker = serviceInvoker;

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
                var jobFound = _storageRepository.GetInstancesForLinkSend().GetAwaiter().GetResult();
                if (jobFound.Count > 0)
                {
                    try
                    {
                        foreach (var instance in jobFound)
                        {
                            try
                            {
                                var filePath = this._pathFinder.GetStoragePathForEncrypted(instance);
                                var sendLink = this._routeFinder.LinkRoute(instance);
                                if (sendLink == null) throw new Exception("No Link Client Found");
                                this._serviceInvoker.ConfigContext(sendLink.UrlEndPoint, HUBURL);
                                var serviceMetadata = new ServiceMetaData()
                                {
                                    Method = ServiceMethod.POST,
                                    PayLoad = instance,
                                    PayLoadKey = "instance"
                                };
                                var response = this._serviceInvoker.InvokeUploadFile(serviceMetadata, filePath).GetAwaiter().GetResult();
                                instance.isLinkedTransmitted= true;
                                this._storageRepository.UpdateInstance(instance).GetAwaiter().GetResult();
                            }
                            catch (Exception dEncry)
                            {
                                instance.isLinkedTransmitted = false;
                                instance.isLinkTramitFail = true;
                                instance.ErrorMessage = dEncry.Message;
                                this._storageRepository.UpdateInstance(instance);
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
