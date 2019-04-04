using CorePacs.Dicom.Contracts;
using System;
using System.Collections.Generic;
using System.Text;
using CorePacs.DataAccess.Domain;
using CorePacs.DataAccess.Contracts;
using System.Threading.Tasks;
using System.Linq;

namespace CorePacs.Dicom.Services
{
    public class RouteFinder : IRouteFinder
    {
        private readonly IStorageRepository _storageRepository;
        List<DicomSend> _dicomSendClients;
        List<LinkClient> _linkClients;
        List<RoutingTable> _routeTable;
                
        public RouteFinder(IStorageRepository storageRepository) {
            if (storageRepository == null) throw new ArgumentNullException(nameof(storageRepository));
            this._storageRepository = storageRepository;
        }
        public DicomSend DicomRoute(Instance instance)
        {
            var route = this._routeTable.FirstOrDefault(x => x.isDSendRoute && x.InComing.Equals(instance.CalledAE, StringComparison.CurrentCultureIgnoreCase));
            if (route != null)
            {
                return this._dicomSendClients.FirstOrDefault(x => x.DicomSendId.Equals(route.OutGoing));
            }
            else {
                return null;
            }
        }

        public LinkClient LinkRoute(Instance instance)
        {
            var route = this._routeTable.FirstOrDefault(x => x.isLinkRoute && x.InComing.Equals(instance.CalledAE, StringComparison.CurrentCultureIgnoreCase));
            if (route != null)
            {
                return this._linkClients.FirstOrDefault(x => x.LinkClientId.Equals(route.OutGoing));
            }
            else
            {
                return null;
            }
        }

        public Task<bool> Refresh()
        {
            this._dicomSendClients = this._storageRepository.GetDicomSendClients().GetAwaiter().GetResult();
            this._linkClients = this._storageRepository.GetLinkClients().GetAwaiter().GetResult();
            this._routeTable = this._storageRepository.GetRouteTable().GetAwaiter().GetResult();
            return Task.FromResult(true);
        }
    }
}
