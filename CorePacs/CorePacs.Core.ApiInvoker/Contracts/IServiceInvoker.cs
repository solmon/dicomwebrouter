using CorePacs.Core.ApiInvoker.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;

namespace CorePacs.Core.ApiInvoker.Contracts
{
    public interface IServiceInvoker
    {
        void ConfigContext(string context);
        void ConfigContext(string baseUrl,string context);
        Task<T> Invoke<T>(ServiceMetaData metaData);
        Task<HttpResponseMessage> InvokeUploadFile(ServiceMetaData metaData,string filePath);
        //void ConfigContext(object pRODUCTS_URL);
    }
}
