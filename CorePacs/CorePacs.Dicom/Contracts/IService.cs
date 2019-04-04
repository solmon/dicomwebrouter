using System;
using System.Collections.Generic;
using System.Text;

namespace CorePacs.Dicom.Contracts
{
    public interface IService:IServer
    {
        bool isRunning { get; }
    }
    public interface ILinkSendService: IService
    {
    }

    public interface IImageEncryptionService : IService
    {        
    }
    public interface IDecryptionService : IService
    {         
    }
    public interface IDicomSendService : IService { }
}
