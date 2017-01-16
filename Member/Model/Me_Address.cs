using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Member
{
    public class Model_Me_Address
    {
        //主键
        public string sysno { get; set; }
        //会员主键
        public string memsys { get; set; }
        public string name { get; set; }
        public string phone { get; set; }
        public string district { get; set; }
        public string detailAddress { get; set; }
        public string post { get; set; }
        public int isDefault { get; set; }
        public int is_Active { get; set; }
    }
}
