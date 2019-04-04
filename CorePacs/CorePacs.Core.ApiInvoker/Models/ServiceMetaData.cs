using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CorePacs.Core.ApiInvoker.Models
{
    public class ServiceMetaData
    {        
        public object PathSegment { get; set; }
        public List<Object> PathSegments { get; set; }
        public object QueryParameters { get; set; }
        public object PayLoad { get; set; }
        public string PayLoadKey { get; set; }
        public ServiceMethod Method { get; set; }
    }
}
