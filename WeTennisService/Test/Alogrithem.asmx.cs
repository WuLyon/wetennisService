using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Services;

namespace WeTennisService.Test
{
    /// <summary>
    /// Alogrithem 的摘要说明
    /// </summary>
    [WebService(Namespace = "http://tempuri.org/")]
    [WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
    [System.ComponentModel.ToolboxItem(false)]
    // 若要允许使用 ASP.NET AJAX 从脚本中调用此 Web 服务，请取消注释以下行。 
    // [System.Web.Script.Services.ScriptService]
    public class Alogrithem : System.Web.Services.WebService
    {

        [WebMethod]
        public string GetMaoPao()
        {
            return Algorithm.instance.MaoPao();
        }

        [WebMethod]
        public string GetXuanZe()
        {
            return Algorithm.instance.XuanZe();
        }

        [WebMethod]
        public string GetChaRu()
        {
            return Algorithm.instance.ChaRu();
        }

        [WebMethod]
        public string GetKaiSu()
        {
            int[] shuJu = new int[] { 12, 43, 2, 34, 87, 54, 32, 16, 67, 49 };
            return Algorithm.instance.KaiSu(shuJu,2,4);
        }

    }
}
