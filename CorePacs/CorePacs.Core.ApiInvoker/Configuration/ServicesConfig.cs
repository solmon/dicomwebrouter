using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CorePacs.Core.ApiInvoker.Configuration
{
    public class ServicesConfig
    {
        public ServicesConfig()
        {
            this.ServiceInfo = new List<Configuration.ServiceInfo>();
        }
        public List<ServiceInfo> ServiceInfo { get; set; }
    }

    public class ServiceInfo {
        public string Name { get; set; }
        public string BaseUrl { get; set; }
    }
}
