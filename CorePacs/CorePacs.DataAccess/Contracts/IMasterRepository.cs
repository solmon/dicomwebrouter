using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using CorePacs.DataAccess.Domain;

namespace CorePacs.DataAccess.Contracts
{
    public interface IMasterRepository<Entity,IdT> where Entity: class, new()                                                    
    {
        Task<Entity> Get(IdT id);
        Task<List<Entity>> Get();
        Task<bool> Add(Entity entity);
        Task<bool> Update(Entity entity);
        Task<bool> Delete(IdT id);
    }
    public interface IAETitleRepository: IMasterRepository<AETitles, Guid> { }
    public interface IDicomSendRepository : IMasterRepository<DicomSend, Guid> { }
    public interface ILinkClientRepository : IMasterRepository<LinkClient, Guid> { }
    public interface IRoutingTableRepository : IMasterRepository<RoutingTable, Guid> { }
    public interface ISettingsRepository : IMasterRepository<Settings, int> { }
}
