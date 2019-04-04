// Copyright (c) 2012-2017 fo-dicom contributors.
// Licensed under the Microsoft Public License (MS-PL).

using System;
using System.IO;
using System.Text;
using System.Threading.Tasks;

using Dicom.Log;
using Dicom.Network;
using Dicom;
using Microsoft.Extensions.DependencyInjection;
using CorePacs.Dicom;
using CorePacs.DataAccess.Contracts;
using CorePacs.DataAccess.Config;
using CorePacs.Dicom.Contracts;

namespace CorePacs.Dicom.Server
{
    public class SimpleCStoreProvider : DicomService, IDicomServiceProvider, IDicomCStoreProvider
    {
        private static readonly DicomTransferSyntax[] AcceptedTransferSyntaxes =
        {
            DicomTransferSyntax.ExplicitVRLittleEndian,
            DicomTransferSyntax.ExplicitVRBigEndian,
            DicomTransferSyntax.ImplicitVRLittleEndian
        };

        private static readonly DicomTransferSyntax[] AcceptedImageTransferSyntaxes =
        {
            // Lossless
            DicomTransferSyntax.JPEGLSLossless,
            DicomTransferSyntax.JPEG2000Lossless,
            DicomTransferSyntax.JPEGProcess14SV1,
            DicomTransferSyntax.JPEGProcess14,
            DicomTransferSyntax.RLELossless,

            // Lossy
            DicomTransferSyntax.JPEGLSNearLossless,
            DicomTransferSyntax.JPEG2000Lossy,
            DicomTransferSyntax.JPEGProcess1,
            DicomTransferSyntax.JPEGProcess2_4,

            // Uncompressed
            DicomTransferSyntax.ExplicitVRLittleEndian,
            DicomTransferSyntax.ExplicitVRBigEndian,
            DicomTransferSyntax.ImplicitVRLittleEndian
        };

        private IStorageRepository _pStorage { get; set; }
        private IStorage _storage { get; set; }
        private IDicomParser _dParser { get; set; }
        private CorePacsSettings _settings { get; set; }

        static SimpleCStoreProvider() {
            
        }
       
        public SimpleCStoreProvider(INetworkStream stream, Encoding fallbackEncoding, Logger log): base(stream, fallbackEncoding, log)            
        {
            build();
        }
        
        private void build() {
            if (this._pStorage == null)
            {
                this._pStorage = ServiceLocator.GetInstance<IStorageRepository>();
                this._storage = ServiceLocator.GetInstance<IStorage>();
                this._dParser = ServiceLocator.GetInstance<IDicomParser>();
                this._settings = ServiceLocator.GetInstance<CorePacsSettings>();
            }
        }

        public void OnReceiveAbort(DicomAbortSource source, DicomAbortReason reason)
        {
        }

        public void OnConnectionClosed(Exception exception)
        {
        }

        public DicomCStoreResponse OnCStoreRequest(DicomCStoreRequest request)
        {
            var dicomAttrs = this._dParser.Extract(this, request);
            var fileName = this._storage.GetStoragePath(dicomAttrs);
            Logger.Info(fileName);
            request.File.Save(fileName);

            if (_pStorage != null) {
                try
                {
                    this._pStorage.AddNewStudy(dicomAttrs,false).GetAwaiter().GetResult();
                    Console.WriteLine("Adding to the DB");                    
                }
                catch (Exception ex) {
                    Console.WriteLine(ex.StackTrace);
                }
            }

            return new DicomCStoreResponse(request, DicomStatus.Success);
        }

        public void OnCStoreRequestException(string tempFileName, Exception e)
        {
        }

        public void OnReceiveAssociationRequest(DicomAssociation association)
        {
            build();
            foreach (var pc in association.PresentationContexts)
            {
                if (pc.AbstractSyntax == DicomUID.Verification) pc.AcceptTransferSyntaxes(AcceptedTransferSyntaxes);
                else if (pc.AbstractSyntax.StorageCategory != DicomStorageCategory.None) pc.AcceptTransferSyntaxes(AcceptedImageTransferSyntaxes);
            }

            if (!this._settings.IsValidAssociation(association.CalledAE)) {

                SendAssociationReject(DicomRejectResult.Transient, DicomRejectSource.ServiceProviderACSE, DicomRejectReason.CalledAENotRecognized);
            }
            else {
                SendAssociationAccept(association);
            }            
        }

        public void OnReceiveAssociationReleaseRequest()
        {
            SendAssociationReleaseResponse();
        }
    }
}
