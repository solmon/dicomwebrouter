using CorePacs.DataAccess.Contracts;
using CorePacs.DataAccess.Domain;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CorePacs.DataAccess.Repository
{    
    public class StorageRepository: IStorageRepository
    {
        private readonly DStorageContext _storageDBContext;
        private readonly IHubEventService _hubService;
        private readonly ILoggerFactory _loggerFactory;
                
        public StorageRepository(IHubEventService hubService, ILoggerFactory loggerFactory)
        {
            _loggerFactory = loggerFactory;
            //if (storageDBContext == null) throw new ArgumentNullException(nameof(storageDBContext));
            if (hubService == null) throw new ArgumentNullException(nameof(hubService));
            _hubService = hubService;
            _storageDBContext = new DStorageContext(loggerFactory);            
        }

        private Task<Study> getStudy(string studyId)
        {
            return Task.FromResult(this._storageDBContext.Studies.Where(x => x.StudyInstanceUID == studyId.ToString()).FirstOrDefault());
        }

        private Task<Series> getSeries(Guid studyId,string seriesId)
        {
            return Task.FromResult(this._storageDBContext.Series.Where(x => x.StudyId == studyId && x.SeriesInstanceUID == seriesId).FirstOrDefault());
        }

        private Task<Instance> getInstance(Guid seriesId, string instanceId)
        {
            return Task.FromResult(this._storageDBContext.Instances.Where(x => x.SeriesId == seriesId && x.SOPInstanceUID == instanceId).FirstOrDefault());
        }

        public async Task<bool> AddNewStudy(Study study,Series series,Instance instance,bool isNewStudy,bool isNewSeries,bool isNewImage)
        {
            //this._storageDBContext.DetachAllEntities();
            using (var transaction = this._storageDBContext.Database.BeginTransaction())
            {
                try
                {
                    if(isNewStudy) this._storageDBContext.Studies.Add(study);
                    else this._storageDBContext.Studies.Update(study);

                    if (isNewSeries) this._storageDBContext.Series.Add(series);
                    else this._storageDBContext.Series.Update(series);

                    if (isNewImage) this._storageDBContext.Instances.Add(instance);
                    else this._storageDBContext.Instances.Update(instance);

                    await this._storageDBContext.SaveChangesAsync().ConfigureAwait(false);
                    transaction.Commit();
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex);
                    transaction.Rollback();
                    throw ex;
                }
            }
            _hubService.NewIncomingFile(new Hub.IncomingFileEventArgs());
            return (true);
        }        
        public Task<bool> AddNewStudy(DicomRequestAttrs dAttrs, bool isLinkReceive)
        {
            bool isNewStudy = false;
            bool isNewSeries = false;
            bool isNewImage = false;
            Series series = null;
            Instance instance = null;

            var studyFound = this.getStudy(dAttrs.StudyInstanceUID).GetAwaiter().GetResult();
            if (studyFound == null)
            {
                isNewStudy = true;
                isNewSeries = true;
                isNewImage = true;
                studyFound = buildStudy(dAttrs);
                series = buildSeries(dAttrs, studyFound);
                instance = buildInstance(dAttrs, series, isLinkReceive);
            }
            else
            {
                series = this.getSeries(studyFound.StudyId, dAttrs.SeriesInstanceUID).GetAwaiter().GetResult();
                if (series == null)
                {
                    isNewSeries = true;
                    isNewImage = true;
                    series = buildSeries(dAttrs, studyFound);
                    instance = buildInstance(dAttrs, series, isLinkReceive);
                }
                else
                {
                    instance = this.getInstance(series.SeriesId, dAttrs.SOPInstanceUID).GetAwaiter().GetResult();
                    if (instance == null)
                    {
                        isNewImage = true;
                        instance = buildInstance(dAttrs, series, isLinkReceive);
                    }
                    else
                    {
                        instance.CalledAE = dAttrs.CalledAE;
                        instance.StudyInstanceUID = dAttrs.StudyInstanceUID;
                        instance.SeriesInstanceUID = dAttrs.SeriesInstanceUID;
                        instance.isEncrypted = false;
                        instance.isLinkedTransmitted = false;
                        instance.isLinkTramitFail = false;
                        instance.isDicomPushed = false;
                        instance.isDecrypted = false;
                        instance.isLinkRecieved = isLinkReceive;
                    }
                }
            }

            return this.AddNewStudy(studyFound, series, instance, isNewStudy, isNewSeries, isNewImage);
        }
        private Instance buildInstance(DicomRequestAttrs dAttrs, Series series,bool isLinkReceive)
        {
            return new Instance()
            {
                InstanceId = Guid.NewGuid(),
                SeriesId = series.SeriesId,
                SOPInstanceUID = dAttrs.SOPInstanceUID,
                CalledAE = dAttrs.CalledAE,
                StudyInstanceUID =dAttrs.StudyInstanceUID,
                SeriesInstanceUID =dAttrs.SeriesInstanceUID,
                isEncrypted = false,
                isLinkedTransmitted = false,
                isLinkTramitFail = false,
                isDicomPushed = false,
                isDecrypted = false,
                isLinkRecieved = isLinkReceive
            };
        }

        private Series buildSeries(DicomRequestAttrs dAttrs,Study study)
        {
            return new Series()
            {
                InstanceCount = dAttrs.ImageCount,
                Modality = dAttrs.Modality,
                SeriesId = Guid.NewGuid(),
                SeriesInstanceUID = dAttrs.SeriesInstanceUID,
                StudyId = study.StudyId
            };
        }

        private Study buildStudy(DicomRequestAttrs dAttrs) {
            return new Study()
            {
                AcquisitionDateTime = dAttrs.AcquisitionDateTime,
                CalledAE = dAttrs.CalledAE,
                PatientName = dAttrs.PatientName,
                Priority = dAttrs.Priority,
                RecievedTime = dAttrs.RecievedTime,
                RemoteHostIP = dAttrs.RemoteHostIP,
                StudyDescription = dAttrs.StudyDescription,
                StudyId = Guid.NewGuid(),
                StudyInstanceUID = dAttrs.StudyInstanceUID,
                SeriesCount = dAttrs.SeriesCount,
                isEncrypted = false
            };
        }

        public void Dispose()
        {
            this._storageDBContext.Dispose();
        }

        public Task<List<AETitles>> GetAETitles()
        {
            return Task.FromResult(this._storageDBContext.AETitles.AsNoTracking().ToList());
        }

        public Task<List<Settings>> GetSettings()
        {
            return Task.FromResult(this._storageDBContext.Settings.AsNoTracking().ToList());
        }

        public Task<List<Study>> GetStudies()
        {
            var studies = this._storageDBContext.Studies
                            .Include(x => x.Series)
                            .ThenInclude(y => y.Instances)
                            .AsNoTracking()
                            .ToList();
            /*var studiesCalc = from s in studies
                              select s;*/
            
            return Task.FromResult(studies);
        }

        public Task<Study> GetStudy(Guid studyId)
        {
            return Task.FromResult(this._storageDBContext.Studies.AsNoTracking().FirstOrDefault(x => x.StudyId.Equals(studyId)));
        }

        public Task<List<Instance>> GetInstancesForEncryption()
        {
            return Task.FromResult(this._storageDBContext.Instances.Where(x=>!x.isEncrypted).Take(20).ToList());
        }

        public async Task<bool> UpdateInstance(Instance instance)
        {
            //this._storageDBContext.DetachAllEntities();
            using (var transaction = this._storageDBContext.Database.BeginTransaction())
            {
                try
                {
                    this._storageDBContext.Instances.Update(instance);
                    //this._storageDBContext.DetachAllEntities();
                    await this._storageDBContext.SaveChangesAsync().ConfigureAwait(false);
                    transaction.Commit();
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex);
                    transaction.Rollback();
                    throw ex;
                }
            }
            //_hubService.NewIncomingFile(new Hub.IncomingFileEventArgs());
            return (true);
        }

        public Task<List<Instance>> GetInstancesForLinkSend()
        {
            return Task.FromResult(this._storageDBContext.Instances.Where(x => x.isEncrypted && !x.isLinkedTransmitted).Take(20).ToList());
        }

        public Task<List<Instance>> GetInstancesForDecryption()
        {
            return Task.FromResult(this._storageDBContext.Instances.Where(x => x.isLinkRecieved).Take(20).ToList());
        }

        public Task<List<Instance>> GetInstancesForLinkDicomSend()
        {
            return Task.FromResult(this._storageDBContext.Instances.Where(x => x.isLinkRecieved && x.isDecrypted && !x.isDicomPushed).Take(20).ToList());
        }

        public Task<List<DicomSend>> GetDicomSendClients()
        {
            return Task.FromResult(this._storageDBContext.DicomSendClients.AsNoTracking().ToList());
        }

        public Task<List<LinkClient>> GetLinkClients()
        {
            return Task.FromResult(this._storageDBContext.LinkClients.AsNoTracking().ToList());
        }

        public Task<List<RoutingTable>> GetRouteTable()
        {
            return Task.FromResult(this._storageDBContext.RouteTable.AsNoTracking().ToList());
        }

        public Task<bool> RegisterServer(ProcessTracker server)
        {
            throw new NotImplementedException();
        }

        public Task<bool> ServerHeartBeat(ProcessTracker server)
        {
            throw new NotImplementedException();
        }

        public Task<bool> StopServer(ProcessTracker server)
        {
            throw new NotImplementedException();
        }

        public Task<bool> StartServer(ProcessTracker server)
        {
            throw new NotImplementedException();
        }
    }
}
