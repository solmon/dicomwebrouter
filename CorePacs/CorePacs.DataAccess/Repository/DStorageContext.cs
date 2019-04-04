using Microsoft.EntityFrameworkCore;
using CorePacs.DataAccess.Domain;
using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Extensions.Logging;
using System.Linq;

namespace CorePacs.DataAccess.Repository
{
    public class DStorageContext : DbContext
    {
        public DbSet<Study> Studies { get; set; }
        public DbSet<Series> Series { get; set; }
        public DbSet<Instance> Instances { get; set; }
        public DbSet<Settings> Settings { get; set; }
        public DbSet<AETitles> AETitles { get; set; }
        public DbSet<ProcessTracker> ProcessTrackers { get; set; }
        public DbSet<DicomSend> DicomSendClients { get; set; }
        public DbSet<LinkClient> LinkClients { get; set; }
        public DbSet<RoutingTable> RouteTable { get; set; }

        private readonly ILoggerFactory _loggerFactory;
        
        public DStorageContext(ILoggerFactory loggerFactory)
        {
            _loggerFactory = loggerFactory;
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlite("Filename=./pacs.db");
            optionsBuilder.UseLoggerFactory(_loggerFactory);
            //optionsBuilder.UseQueryTrackingBehavior(QueryTrackingBehavior.NoTracking);
        }
        public void DetachAllEntities()
        {
            var changedEntriesCopy = this.ChangeTracker.Entries()
                .Where(e => e.State == EntityState.Added ||
                            e.State == EntityState.Modified ||
                            e.State == EntityState.Deleted)
                .ToList();
            foreach (var entity in changedEntriesCopy)
            {
                this.Entry(entity.Entity).State = EntityState.Detached;
            }
        }
    }
}
