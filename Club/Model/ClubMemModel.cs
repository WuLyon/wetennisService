using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Club
{
    [Serializable]
    public class ClubMemModel
    {
        public string ID { get; set; }
        public string CLUBSYS { get; set; }
        public string MEMSYS { get; set; }
        public string JOB { get; set; }
        public string EXT1 { get; set; }
        public string EXT2 { get; set; }
        public string EXT3 { get; set; }
    }
}
