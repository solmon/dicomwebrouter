using System;
using System.Collections.Generic;
using System.Text;

namespace CorePacs.DataAccess.Domain
{
    public enum ServerProcess
    {
        DICOMRECIEVE = 0,
        ENCRYPTION = 1,
        HTTPSEND = 2,
        HTTPRECIEVE=3,
        DECRYPT = 4,
        DICOMPUSH = 5
    }
}
