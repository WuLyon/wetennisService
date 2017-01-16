using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WeChat
{
    [System.Runtime.Serialization.DataContract]
    public class Para_wxconfig
    {
        [System.Runtime.Serialization.DataMember]
        public string appid { get; set; }

        [System.Runtime.Serialization.DataMember]
        public int timestamp { get; set; }

        [System.Runtime.Serialization.DataMember]
        public string nonceStr { get; set; }

        [System.Runtime.Serialization.DataMember]
        public string signature { get; set; }
    }
}