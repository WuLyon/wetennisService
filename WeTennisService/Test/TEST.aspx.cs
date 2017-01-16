using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using WeTour;

namespace WeTennisService.Test
{
    public partial class TEST : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            TourResDisDll.instance.DistributeTourRes("05E7E03727BD4B5294A481E023AA2023");
        }
    }
}