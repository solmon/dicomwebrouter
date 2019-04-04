using CorePacs.DataAccess.Contracts;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Text;
using CorePacs.WebServer.Helpers;
using System.Threading.Tasks;
using Microsoft.AspNetCore.WebUtilities;
using Microsoft.Net.Http.Headers;
using Microsoft.AspNetCore.Http;
using System.Linq;
using System.IO;
using CorePacs.DataAccess.Domain;
using Microsoft.AspNetCore.Http.Features;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using CorePacs.Dicom.Contracts;

namespace CorePacs.WebServer.Controllers
{
    [Route("api/v1/[controller]")]
    public class DicomRecieveController : Controller
    {
        private readonly ILogger<DicomRecieveController> _logger;
        private readonly IStorageRepository _storageRepository;
        private readonly IPathFinder _pathFinder;
        private readonly IDicomParser _dicomParser;
        private static readonly FormOptions _defaultFormOptions = new FormOptions();
        public DicomRecieveController(IStorageRepository storageRepository,IPathFinder pathFinder, 
            ILogger<DicomRecieveController> logger, IDicomParser dicomParser)
        {
            if (storageRepository == null) throw new ArgumentNullException(nameof(storageRepository));
            if (pathFinder == null) throw new ArgumentNullException(nameof(pathFinder));
            if (dicomParser == null) throw new ArgumentNullException(nameof(dicomParser));
            this._storageRepository = storageRepository;
            this._pathFinder = pathFinder;
            this._logger = logger;
            this._dicomParser = dicomParser;
        }

        [HttpPost]        
        public async Task<IActionResult> RecieveFile()
        {
            Instance instance = null; 
            if (!MultipartRequestHelper.IsMultipartContentType(Request.ContentType))
            {
                return BadRequest($"Expected a multipart request, but got {Request.ContentType}");
            }

            //var formAccumulator = new KeyValueAccumulator();
            string targetFilePath = null;

            var boundary = MultipartRequestHelper.GetBoundary(
                MediaTypeHeaderValue.Parse(Request.ContentType),
                _defaultFormOptions.MultipartBoundaryLengthLimit);
            var reader = new MultipartReader(boundary, HttpContext.Request.Body);

            var section = await reader.ReadNextSectionAsync();
            while (section != null)
            {
                ContentDispositionHeaderValue contentDisposition;
                var hasContentDispositionHeader = ContentDispositionHeaderValue.TryParse(section.ContentDisposition, out contentDisposition);

                if (hasContentDispositionHeader)
                {
                    if (MultipartRequestHelper.HasFileContentDisposition(contentDisposition))
                    {
                        targetFilePath = this._pathFinder.GetStoragePathForLinkRecieve(instance); 
                        using (var targetStream = System.IO.File.Create(targetFilePath))
                        {
                            await section.Body.CopyToAsync(targetStream);

                            _logger.LogInformation($"Copied the uploaded file '{targetFilePath}'");
                        }
                    }
                    else if (MultipartRequestHelper.HasFormDataContentDisposition(contentDisposition))
                    {
                        // Content-Disposition: form-data; name="key"
                        // value
                        // Do not limit the key name length here because the 
                        // multipart headers length limit is already in effect.
                        var key = HeaderUtilities.RemoveQuotes(contentDisposition.Name).Value;
                        var encoding = GetEncoding(section);
                        using (var streamReader = new StreamReader(
                            section.Body,
                            encoding,
                            detectEncodingFromByteOrderMarks: true,
                            bufferSize: 1024,
                            leaveOpen: true))
                        {
                            // The value length limit is enforced by MultipartBodyLengthLimit
                            var value = await streamReader.ReadToEndAsync();
                            if (String.Equals(value, "undefined", StringComparison.OrdinalIgnoreCase))
                            {
                                value = String.Empty;
                            }
                            if (key.Equals("instance") && !string.IsNullOrEmpty(value)) {
                                instance = JsonConvert.DeserializeObject<Instance>(value);
                            }                            
                        }
                    }
                }
                section = await reader.ReadNextSectionAsync();
            }

            var dAttrs = this._dicomParser.Extract(instance, _pathFinder);
            this._storageRepository.AddNewStudy(dAttrs, true).GetAwaiter().GetResult();
            return Ok(new { count = 1});            
        }
        private static Encoding GetEncoding(MultipartSection section)
        {
            MediaTypeHeaderValue mediaType;
            var hasMediaTypeHeader = MediaTypeHeaderValue.TryParse(section.ContentType, out mediaType);
            // UTF-7 is insecure and should not be honored. UTF-8 will succeed in 
            // most cases.
            if (!hasMediaTypeHeader || Encoding.UTF7.Equals(mediaType.Encoding))
            {
                return Encoding.UTF8;
            }
            return mediaType.Encoding;
        }
    }
}
