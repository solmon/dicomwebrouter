using CorePacs.DataAccess.Contracts;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CorePacs.DataAccess.Repository
{
    public abstract class MasterRepository<Entity, IdT> 
        where Entity : class, new()
    {
        protected DStorageContext _storageDBContext;
        protected IHubEventService _hubService;
        protected ILoggerFactory _loggerFactory;

        public async Task<bool> Add(Entity entity)
        {
            using (var transaction = this._storageDBContext.Database.BeginTransaction())
            {
                try
                {
                    _storageDBContext.Add<Entity>(entity);
                    await this._storageDBContext.SaveChangesAsync().ConfigureAwait(false);
                    transaction.Commit();
                    return true;
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex);
                    transaction.Rollback();
                    throw ex;
                }
            }
        }

        public async Task<bool> Delete(IdT id)
        {
            using (var transaction = this._storageDBContext.Database.BeginTransaction())
            {
                try
                {
                    var entity = await this.Get(id);
                    _storageDBContext.Remove<Entity>(entity);
                    await this._storageDBContext.SaveChangesAsync().ConfigureAwait(false);
                    transaction.Commit();
                    return true;
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex);
                    transaction.Rollback();
                    throw ex;
                }
            }
        }

        public Task<Entity> Get(IdT id)
        {
           return Task.FromResult(_storageDBContext.Find<Entity>(id));
        }

        public abstract Task<List<Entity>> Get();

        public async Task<bool> Update(Entity entity)
        {
            using (var transaction = this._storageDBContext.Database.BeginTransaction())
            {
                try
                {
                    _storageDBContext.Update<Entity>(entity);
                    await this._storageDBContext.SaveChangesAsync().ConfigureAwait(false);
                    transaction.Commit();
                    return true;
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex);
                    transaction.Rollback();
                    throw ex;
                }
            }
        }
    }
}
