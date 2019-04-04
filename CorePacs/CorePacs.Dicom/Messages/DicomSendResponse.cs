using System;
using System.Collections.Generic;
using System.Text;

namespace CorePacs.Dicom.Messages
{
    public class DicomSendResponse
    {
        public string Error { get; set; }
        public bool isSuccess { get; set; }
    }
}
