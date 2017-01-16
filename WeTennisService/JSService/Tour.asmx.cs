using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Services;
using WeTour;

namespace WeTennisService.JSService
{
    /// <summary>
    /// Tour 的摘要说明
    /// </summary>
    [WebService(Namespace = "http://tempuri.org/")]
    [WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
    [System.ComponentModel.ToolboxItem(false)]
    // 若要允许使用 ASP.NET AJAX 从脚本中调用此 Web 服务，请取消注释以下行。 
    [System.Web.Script.Services.ScriptService]
    public class Tour : System.Web.Services.WebService
    {

        [WebMethod]
        public string HelloWorld()
        {
            return "Hello World";
        }

        /// <summary>
        /// 根据赛事主键分配赛事资源
        /// </summary>
        /// <param name="_TourSys"></param>
        /// <returns></returns>
        [WebMethod]
        public void DistributeResource(string _TourSys)
        {
            ResTourDistridll.instance.DistributOne(_TourSys);
        }
    }
}
