using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Gym
{
    public class CourtOrderModel
    {
        public string ID { get; set; }
        public string COURTSYS { get; set; }
        public string MEMBERSYS { get; set; }
        public string ORDERNO { get; set; }
        public string ORDERDATE { get; set; }
        public string STARTHOUR { get; set; }
        public string ENDHOUR { get; set; }
        public string TOTALHOUR { get; set; }
        public string UNITPRICE { get; set; }
        public string TOTALMONEY { get; set; }
        public string STATUS { get; set; }
        public string UPDATEDATE { get; set; }
        public string REMARK { get; set; }
        public string EXT1 { get; set; }
        public string EXT2 { get; set; }

        //附加属性
        public string UserName { get; set; }
        public string Telephone { get; set; }
        public string CourtName { get; set; }
    }
}
