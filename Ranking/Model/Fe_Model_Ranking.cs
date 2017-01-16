using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Ranking
{
   
    public class Fe_Model_rankingFilter
    {
        public string text { get; set; }
        public string value { get; set; }
        public object children { get; set; }
    }

    public class Fe_Model_rankings
    {
        public int type { get; set; }
        public int ranking { get; set; }
        public string username { get; set; }
        public string userimage { get; set; }
        public string userid { get; set; }
        public int pts { get; set; }
        public string clubname { get; set; }

    }

    public class Fe_Model_rankenum
    {
        public string id { get; set; }
        public string groupname { get; set; }
        public string item { get; set; }
        public string issingle { get; set; }
        public string gender { get; set; }
    }

    public class Fe_Model_rankingDetailsInfo
    {
        public int usersex { get; set; }
        public string username { get; set; }
        public string userimage { get; set; }
        public string birthday { get; set; }
        public string constellation { get; set; }
        public int singleNumber { get; set; }
        public int doubleNumber { get; set; }
        public bool like { get; set; }
        public int singleAccumulatePoints { get; set; }
        public int doubleAccumulatePoints { get; set; }
        public string account { get; set; }
        public string year { get; set; }
        public string city { get; set; }
        public string stature { get; set; }
        public string weight { get; set; }
        public string front { get; set; }
        public string back { get; set; }
        public bool singleTabInfo { get; set; }
        public bool doublTabInfo { get; set; }

    }
}
