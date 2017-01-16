using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace WeTour
{
    public class Model_TourContScore
    {
        public string id { get; set; }
        public string toursys { get; set; }
        public string contentid { get; set; }
        public string round { get; set; }
        public string score { get; set; } 
    }

    public class PageS_ScoreSetting
    {
        public string contentId { get; set; }
        public string contentName { get; set; }
        public List<PageS_contentRounds> contentRounds { get; set; }

    }


    public class PageS_contentRounds
    {
        public string roundNum { get; set; }
        public string roundName { get; set; }
        public string score { get; set; }
    }
}
