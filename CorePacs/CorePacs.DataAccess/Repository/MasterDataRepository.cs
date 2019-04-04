using CorePacs.DataAccess.Contracts;
using CorePacs.DataAccess.Domain;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CorePacs.DataAccess.Repository
{
    public class AETitlesRepository : MasterRepository<AETitles, Guid>, IAETitleRepository
    {
        public AETitlesRepository(IHubEventService hubService, ILoggerFactory loggerFactory)
        {
            _loggerFactory = loggerFactory;
            if (hubService == null) throw new ArgumentNullException(nameof(hubService));
            _hubService = hubService;
            _storageDBContext = new DStorageContext(loggerFactory);
        }

        public override Task<List<AETitles>> Get()
        {
            return Task.FromResult( this._storageDBContext.AETitles.ToList());
        }        
    }
    public class DicomSendRepository : MasterRepository<DicomSend, Guid>, IDicomSendRepository
    {
        public DicomSendRepository(IHubEventService hubService, ILoggerFactory loggerFactory)
        {
            _loggerFactory = loggerFactory;
            if (hubService == null) throw new ArgumentNullException(nameof(hubService));
            _hubService = hubService;
            _storageDBContext = new DStorageContext(loggerFactory);
        }

        public override Task<List<DicomSend>> Get()
        {
            return Task.FromResult(this._storageDBContext.DicomSendClients.ToList());
        }
    }
    public class LinkClientRepository : MasterRepository<LinkClient, Guid>, ILinkClientRepository
    {
        public LinkClientRepository(IHubEventService hubService, ILoggerFactory loggerFactory)
        {
            _loggerFactory = loggerFactory;
            if (hubService == null) throw new ArgumentNullException(nameof(hubService));
            _hubService = hubService;
            _storageDBContext = new DStorageContext(loggerFactory);
        }

        public override Task<List<LinkClient>> Get()
        {
            return Task.FromResult(this._storageDBContext.LinkClients.ToList());
        }
    }
    public class RoutingTableRepository : MasterRepository<RoutingTable, Guid>, IRoutingTableRepository
    {
        public RoutingTableRepository(IHubEventService hubService, ILoggerFactory loggerFactory)
        {
            _loggerFactory = loggerFactory;
            if (hubService == null) throw new ArgumentNullException(nameof(hubService));
            _hubService = hubService;
            _storageDBContext = new DStorageContext(loggerFactory);
        }

        public override Task<List<RoutingTable>> Get()
        {
            return Task.FromResult(this._storageDBContext.RouteTable.ToList());
        }
    }
    public class SettingsRepository : MasterRepository<Settings, int>, ISettingsRepository
    {
        public SettingsRepository(IHubEventService hubService, ILoggerFactory loggerFactory)
        {
            _loggerFactory = loggerFactory;
            if (hubService == null) throw new ArgumentNullException(nameof(hubService));
            _hubService = hubService;
            _storageDBContext = new DStorageContext(loggerFactory);
        }

        public override Task<List<Settings>> Get()
        {
            return Task.FromResult(this._storageDBContext.Settings.ToList());
        }
    }
}
