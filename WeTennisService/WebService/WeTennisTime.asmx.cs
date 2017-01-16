using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Services;
using Time;

namespace WeTennisService.WebService
{
    /// <summary>
    /// WeTennisTime 的摘要说明
    /// </summary>
    [WebService(Namespace = "http://tempuri.org/")]
    [WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
    [System.ComponentModel.ToolboxItem(false)]
    // 若要允许使用 ASP.NET AJAX 从脚本中调用此 Web 服务，请取消注释以下行。 
    // [System.Web.Script.Services.ScriptService]
    public class WeTennisTime : System.Web.Services.WebService
    {

        [WebMethod]
        public string HelloWorld()
        {
            return "Hello World";
        }

        /// <summary>
        /// 添加新的时光
        /// </summary>
        /// <param name="memsys"></param>
        /// <returns></returns>
        [WebMethod]
        public string InsertTimeMain(string memsys,string _Sysno)
        {
            //新建时光
            TimeModel model = new TimeModel();
            model.SYS = _Sysno;
            model.MEMSYS = memsys;
            model.TYPE = "0";
            return TimeDAL.instance.InsertNewwithSys(model);
        }

        [WebMethod]
        public void AddTimePic(string TypeSys,string NewUrl)
        {
            TimePicsModel model = new TimePicsModel();
            model.TIMESYS = TypeSys;
            model.PICURL = NewUrl;
            TimePicDal.instance.InsertNewTimePic(model);
        }

        [WebMethod]
        public void UploadImg(string ImgStr)
        { 
            
        }
    }
}
