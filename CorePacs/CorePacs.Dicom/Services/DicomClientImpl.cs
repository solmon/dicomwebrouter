using CorePacs.Dicom.Contracts;
using System;
using System.Collections.Generic;
using System.Text;
using CorePacs.DataAccess.Domain;
using Dicom;
using System.Threading.Tasks;
using Dicom.Network;
using CorePacs.Dicom.Messages;

namespace CorePacs.Dicom.Services
{  
    public class DicomClientImpl : IDicomClient
    {
        public Task<DicomSendResponse> Transmit(DicomSend dicomRoute, DicomFile dFile)
        {
            return SendRequestForCommitment(dicomRoute, dFile);            
        }        

        public Task<DicomSendResponse> SendRequestForCommitment(DicomSend dicomRoute,DicomFile dFile)
        {
            var res = new DicomSendResponse();
            var client = new DicomClient();
            //var nAction = new DicomNActionRequest(dSet);
            var tcs = new TaskCompletionSource<DicomSendResponse>();

            /*
            nAction.OnResponseReceived += (DicomNActionRequest request, DicomNActionResponse response) =>
            {
                if (response.Status.State == DicomState.Pending)
                {
                    Console.WriteLine("Sending is in progress. please wait");
                }
                else if (response.Status.State == DicomState.Success)
                {
                    res.isSuccess = true;
                    tcs.SetResult(res);
                }
                else if (response.Status.State == DicomState.Failure)
                {
                    res.isSuccess = false;
                    res.Error = response.Status.Description;
                    tcs.SetResult(res);
                    Console.WriteLine("Error sending datasets: " + response.Status.Description);                    
                }
                Console.WriteLine(response.Status);
            };*/
            /*
            var cMoveRequest = CreateCMoveBySeriesUID(dSet);
            cMoveRequest.OnResponseReceived += (DicomCMoveRequest requ, DicomCMoveResponse response) =>
            {
                if (response.Status.State == DicomState.Pending)
                {
                    Console.WriteLine("Sending is in progress. please wait");
                }
                else if (response.Status.State == DicomState.Success)
                {
                    res.isSuccess = true;
                    tcs.SetResult(res);
                    Console.WriteLine("Sending successfully finished");                    
                }
                else if (response.Status.State == DicomState.Failure)
                {
                    res.isSuccess = false;
                    res.Error = response.Status.Description;
                    tcs.SetResult(res);
                    Console.WriteLine("Error sending datasets: " + response.Status.Description);                    
                }
                Console.WriteLine(response.Status);
            };
            */
            var cStoreRequest = CreateCStoreBySeriesUID(dFile);
            cStoreRequest.OnResponseReceived += (DicomCStoreRequest requ, DicomCStoreResponse response) =>
            {
                if (response.Status.State == DicomState.Pending)
                {
                    Console.WriteLine("Sending is in progress. please wait");
                }
                else if (response.Status.State == DicomState.Success)
                {
                    res.isSuccess = true;
                    tcs.SetResult(res);
                    Console.WriteLine("Sending successfully finished");
                }
                else if (response.Status.State == DicomState.Failure)
                {
                    res.isSuccess = false;
                    res.Error = response.Status.Description;
                    tcs.SetResult(res);
                    Console.WriteLine("Error sending datasets: " + response.Status.Description);
                }
                Console.WriteLine(response.Status);
            };

            client.AddRequest(cStoreRequest);
            client.Send(dicomRoute.RemoteHost, Int32.Parse(dicomRoute.Port), false, dicomRoute.AETitle, dicomRoute.CallingAETitle);
            return tcs.Task;
        }
        public DicomCMoveRequest CreateCMoveBySeriesUID(string destination, string studyUID, string seriesUID)
        {
            var request = new DicomCMoveRequest(destination, studyUID, seriesUID);
            // no more dicomtags have to be set
            return request;
        }
        public DicomCMoveRequest CreateCMoveBySeriesUID(DicomDataset dSet)
        {
            dSet.AddOrUpdate(DicomTag.CommandField, (ushort)DicomCommandField.CMoveRequest);
            dSet.AddOrUpdate(DicomTag.AffectedSOPClassUID, DicomUID.StudyRootQueryRetrieveInformationModelMOVE);
            var request = new DicomCMoveRequest(dSet);            
            return request;
        }

        public DicomCStoreRequest CreateCStoreBySeriesUID(DicomFile dSet)
        {
            dSet.Dataset.AddOrUpdate(DicomTag.CommandField, (ushort)DicomCommandField.CMoveRequest);
            dSet.Dataset.AddOrUpdate(DicomTag.AffectedSOPClassUID, DicomUID.StudyRootQueryRetrieveInformationModelMOVE);
            var request = new DicomCStoreRequest(dSet);
            return request;
        }
    }
}
