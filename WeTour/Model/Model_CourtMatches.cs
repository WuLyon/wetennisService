using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace WeTour
{
    public class Model_CourtMatches
    {
        public string matchDate { get; set; }
        public string dateMatchQty { get; set; }
        public List<Model_DateMatches> dateMatches { get; set; }
    }

    public class Model_DateMatches
    {
        public string courtName { get; set; }
        public string courtMatchQty { get; set; }
        public List<Model_courtDateMatch> courtMatches { get; set; }
    }

    public class Model_courtDateMatch
    {
        public string matchSys { get; set; }
        public string courtMatchOrder { get; set; }
        public string matchOrder { get; set; }
        public string matchDesc { get; set; }
        public string player1 { get; set; }
        public string player2 { get; set; } 
    }
}
