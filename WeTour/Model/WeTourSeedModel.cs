using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace WeTour
{
    public class WeTourSeedModel
    {
        public string ID { get; set; }
        public string CONTENTID { get; set; }
        public string MEMBERSYS { get; set; }
        public string SEED { get; set; }

        //extra information
        public string P1LNAME { get; set; }
        public string P1LIMGURL { get; set; }
        public string P1RNAME { get; set; }
        public string P1RIMGURL { get; set; }
        public string P2LNAME { get; set; }
        public string P2LIMGURL { get; set; }
        public string P2RNAME { get; set; }
        public string P2RIMGURL { get; set; }
    }
}
