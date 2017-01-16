using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Basic
{
   
        public class Model_Basic_Service_Log
        {
            public string gid { get; set; }
            public string ServiceName { get; set; }
            public string URL { get; set; }
            public string HostName { get; set; }
            public string ResponseStr { get; set; }
            public DateTime CreateDate { get; set; }
        }
   
}
