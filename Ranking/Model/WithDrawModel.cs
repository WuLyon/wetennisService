using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Ranking
{
    [Serializable]
    public class WithDrawModel
    {
        public string ID { get; set; }

        public string Memsys { get; set; }

        public string Matchsys { get; set; }

        public string Toursys { get; set; }

        public string Contentid { get; set; }

        public string UpdateDate { get; set; }

        //附加字段
        public string withQty { get; set; }
    }
}
