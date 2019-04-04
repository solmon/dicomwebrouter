using System;
using System.Collections.Generic;
using System.Text;

namespace CorePacs.Dicom.Contracts
{
    public interface IServer
    {
        void Start();
        void Stop();
        //string ServerName { get; }
    }

    public interface IDecryptionServer: IServer
    {
    }

    public interface IEncryptionServer : IServer
    {
    }

    public interface ILinkSendServer : IServer
    {
        
    }

    public interface IPacsServer : IServer
    {
        bool Build();      
    }

    public interface IDicomSendServer : IServer { }
}
