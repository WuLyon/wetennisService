using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace WeTour
{
    public class Model_ApiTourList
    {
        public string TourSys { get; set; }
        public string TourName { get; set; }
        public string TourImg { get; set; }
        public string TourType { get; set; }
        public string TourTypeImg { get; set; }
        public string TourDate { get; set; }
        public string Province { get; set; }
        public string City { get; set; }
        public string TourAddress { get; set; }
        public string StatusDesc { get; set; }
        public string Status { get; set; }
        public string host { get; set; }
        public string asso_host { get; set; }

        public List<Model_WeTourAdvertiser> advertise { get; set; }

        public List<Model_TourControl> Tour_Controls { get; set; }
    }

    public class Model_TourControl {
        public string ControlName { get; set; }
        public string ControlUrl { get; set; }
        public string ControlFun { get; set; }
    }

}
