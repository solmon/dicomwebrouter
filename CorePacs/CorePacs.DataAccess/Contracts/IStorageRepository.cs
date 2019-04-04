using System;
using System.Collections.Generic;
using System.Text;
using CorePacs.DataAccess.Domain;
using System.Threading.Tasks;

namespace CorePacs.DataAccess.Contracts
{
    public interface IStorageRepository:IDisposable
    {
        Task<bool> AddNewStudy(Study study, Series series, Instance instance, bool isNewStudy, bool isNewSeries, bool isNewImage);
        //Task<bool> AddNewStudy(DicomRequestAttrs study);
        Task<bool> AddNewStudy(DicomRequestAttrs study,bool isLinkReceive);
        Task<bool> UpdateInstance(Instance instance);
        Task<Study> GetStudy(Guid studyId);
        Task<List<Study>> GetStudies();
        Task<List<Settings>> GetSettings();
        Task<List<AETitles>> GetAETitles();
        Task<List<Instance>> GetInstancesForEncryption();
        Task<List<Instance>> GetInstancesForDecryption();
        Task<List<Instance>> GetInstancesForLinkSend();
        Task<List<Instance>> GetInstancesForLinkDicomSend();
        Task<List<DicomSend>> GetDicomSendClients();
        Task<List<LinkClient>> GetLinkClients();
        Task<List<RoutingTable>> GetRouteTable();
        Task<bool> RegisterServer(ProcessTracker server);
        Task<bool> ServerHeartBeat(ProcessTracker server);
        Task<bool> StopServer(ProcessTracker server);
        Task<bool> StartServer(ProcessTracker server);
    }
}
