using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace WeTour
{
    public class WeTourSignModel
    {
        public string id { get; set; }
        public string SIGNORDER { get; set; }
        public string MEMBERSYS { get; set; }
        public string TOURSYS { get; set; }
        public string CONTENTID { get; set; }
        public string ROUND { get; set; }
        public string GROUPID { get; set; }
        public string EXT1 { get; set; }
        public string EXT2 { get; set; }
        public string EXT3 { get; set; }
        public string MATCHTYPE { get; set; }

        //
        public string PLAYER1NAME { get; set; }
        public string PLAYER1IMG { get; set; }
        public string PLAYER2NAME { get; set; }
        public string PLAYER2IMG { get; set; }
        public string SEED { get;set;}
    }
}
