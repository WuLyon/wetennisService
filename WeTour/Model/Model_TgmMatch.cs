using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace WeTour
{
    public class Model_TgmMatch
    {
        public string RoundName { get; set; }
        public List<Model_TgmRound> RoundMatch { get; set; }
    }

    public class Model_TgmRound
    {
        public string matchOrder { get; set; }
        public string p1name { get; set; }
        public string p2name { get; set; }
    }
}
