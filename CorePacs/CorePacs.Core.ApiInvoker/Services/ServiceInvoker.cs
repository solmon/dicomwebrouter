using CorePacs.Core.ApiInvoker.Contracts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CorePacs.Core.ApiInvoker.Models;
using Flurl;
using Flurl.Util;
using Flurl.Http;
using CorePacs.Core.ApiInvoker.Configuration;
using Microsoft.Extensions.Options;
using System.Net.Http;

namespace CorePacs.Core.ApiInvoker.Services
{
    public class ServiceInvoker : IServiceInvoker
    {
        private string _baseurl;
        private readonly ServicesConfig _servicesConfig;
        public ServiceInvoker(IOptions<ServicesConfig> configOptions)
        {
            if (configOptions == null) throw new ArgumentNullException(nameof(configOptions));
            this._servicesConfig = configOptions.Value;
        }

        public void ConfigContext(string baseUrl, string context)
        {
            var serviceInfo = this._servicesConfig.ServiceInfo.FirstOrDefault(x => x.Name.Equals(context, StringComparison.CurrentCultureIgnoreCase));
            if (serviceInfo == null) throw new Exception("No Base Url Setup");
            this._baseurl = baseUrl + serviceInfo.BaseUrl;
        }

        public void ConfigContext(string context)
        {
            var serviceInfo = this._servicesConfig.ServiceInfo.FirstOrDefault(x => x.Name.Equals(context, StringComparison.CurrentCultureIgnoreCase));
            if (serviceInfo == null) throw new Exception("No Base Url Setup");
            this._baseurl = serviceInfo.BaseUrl;
        }

        public Task<T> Invoke<T>(ServiceMetaData metaData)
        {
            if (string.IsNullOrEmpty(this._baseurl)) throw new Exception("No Base Url Setup");
            Url fUrl = null;
            string url = string.Empty;
            url = this._baseurl;

            if (metaData.PathSegment != null)
            {
                //fUrl = this._baseurl
                //.AppendPathSegment(metaData.PathSegment);
                url = string.Format("{0}/{1}", this._baseurl, metaData.PathSegment.ToString());
            }

            if (metaData.PathSegments != null)
            {
                foreach (var seg in metaData.PathSegments)
                {
                    var token = string.Empty;
                    if (seg is DateTime)
                    {
                        token = System.Net.WebUtility.UrlEncode(((DateTime)seg).ToString("yyyy-MM-dd"));
                    }
                    else
                    {

                        token = System.Net.WebUtility.UrlEncode(seg.ToString());
                    }
                    url = string.Format("{0}/{1}", url, token);
                }
                //.AppendPathSegments(metaData.PathSegments.ToArray());
            }
            fUrl = url;
            if (metaData.QueryParameters != null)
            {
                fUrl.SetQueryParams(metaData.QueryParameters);
            }

            if (metaData.Method == ServiceMethod.GET)
            {
                return fUrl.GetJsonAsync<T>();
            }
            else if (metaData.Method == ServiceMethod.POST)
            {
                return fUrl.PostJsonAsync(metaData.PayLoad).ReceiveJson<T>();
            }
            else
            {
                throw new NotImplementedException("The http method is not implemented");
            }

        }

        public Task<HttpResponseMessage> InvokeUploadFile(ServiceMetaData metaData, string filePath)
        {
            if (string.IsNullOrEmpty(this._baseurl)) throw new Exception("No Base Url Setup");
            Url fUrl = null;
            string url = string.Empty;
            url = this._baseurl;

            if (metaData.PathSegment != null)
            {
                //fUrl = this._baseurl
                //.AppendPathSegment(metaData.PathSegment);
                url = string.Format("{0}/{1}", this._baseurl, metaData.PathSegment.ToString());
            }

            if (metaData.PathSegments != null)
            {
                foreach (var seg in metaData.PathSegments)
                {
                    var token = string.Empty;
                    if (seg is DateTime)
                    {
                        token = System.Net.WebUtility.UrlEncode(((DateTime)seg).ToString("yyyy-MM-dd"));
                    }
                    else
                    {

                        token = System.Net.WebUtility.UrlEncode(seg.ToString());
                    }
                    url = string.Format("{0}/{1}", url, token);
                }
                //.AppendPathSegments(metaData.PathSegments.ToArray());
            }
            fUrl = url;
            if (metaData.QueryParameters != null)
            {
                fUrl.SetQueryParams(metaData.QueryParameters);
            }

            if (metaData.Method == ServiceMethod.POST)
            {
                return fUrl.PostMultipartAsync(mp=>mp.AddJson(metaData.PayLoadKey, metaData.PayLoad)
                                                     .AddFile("file1",filePath)
                                               );
            }
            else
            {
                throw new NotImplementedException("The http method is not implemented");
            }
        }
    }
}
