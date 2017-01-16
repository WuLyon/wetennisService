using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Club
{
    public class Model_ClubGym
    {
        public string id { get; set; }
        public string ClubSys { get; set; }
        public string GymSys { get; set; }
        public string Status { get; set; }
    }

    public class Model_Dist_Gyms
    {
        public string gymSys { get; set; }
        public string gymName { get; set; }
    }

  
}
