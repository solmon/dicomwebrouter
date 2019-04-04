using CorePacs.DataAccess.Domain;
using CorePacs.DataAccess.Repository;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;

namespace CorePacs.DataAccess.Extensions
{
    public static class CorePacsDataAccessExtensions
    {
        public static bool AllMigrationsApplied(this DStorageContext context)
        {
            return true;
        }

        public static void EnsureSeeded(this DStorageContext context)
        {
            if (!context.AETitles.AnyAsync().GetAwaiter().GetResult())
            {
                context.AETitles.Add(
                    new AETitles()
                    {
                        AEId = Guid.NewGuid(),
                        AETitle = "SOLMON",
                        RemoteHost = "localhost",
                        isActive = true
                    }
                );
            }

            if (!context.Settings.AnyAsync().GetAwaiter().GetResult())
            {
                context.Settings.Add(
                    new Settings()
                    {
                        Id = 1,
                        Name = "BASEPATH",
                        Value = "E:\\Data\\DR"
                    }
                );

                context.Settings.Add(
                    new Settings()
                    {
                        Id = 2,
                        Name = "PORT",
                        Value = "12345"
                    }
                );
            }

            var linkCId1 = Guid.NewGuid();

            if (!context.LinkClients.AnyAsync().GetAwaiter().GetResult())
            {
                context.LinkClients.Add(
                    new LinkClient()
                    {
                        isActive = true,
                        LinkClientId = linkCId1,
                        RegisteredTime = DateTime.Now,
                        UrlEndPoint = "http://localhost:5001/"
                    }
                );
            }

            var dicomSendId1 = Guid.NewGuid();

            if (!context.DicomSendClients.AnyAsync().GetAwaiter().GetResult())
            {
                context.DicomSendClients.Add(
                    new DicomSend()
                    {                       
                        isActive = true,
                        DicomSendId = dicomSendId1,
                        RegisteredTime = DateTime.Now,
                        AETitle = "SOLEND",
                        Port = "5432",
                        RemoteHost = "localhost",
                        CallingAETitle = "SOLEND"
                    }
                );
            }

            if (!context.RouteTable.AnyAsync().GetAwaiter().GetResult())
            {
                context.RouteTable.Add(
                    new RoutingTable()
                    {
                        isActive = true,
                        RegisteredTime = DateTime.Now,
                        RoutingId = Guid.NewGuid(),
                        InComing = "SOLMON",
                        OutGoing = linkCId1,
                        isDSendRoute = false,
                        isLinkRoute = true
                    }
                );

                context.RouteTable.Add(
                    new RoutingTable()
                    {
                        isActive = true,
                        RegisteredTime = DateTime.Now,
                        RoutingId = Guid.NewGuid(),
                        InComing = "SOLMON",
                        OutGoing = dicomSendId1,
                        isDSendRoute = true,
                        isLinkRoute = false
                    }
                );
            }

            context.SaveChanges();
        }
    }
}
