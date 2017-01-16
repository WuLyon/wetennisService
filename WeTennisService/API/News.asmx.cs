using Basic;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Services;
using News;

namespace WeTennisService.API
{
    /// <summary>
    /// News 的摘要说明
    /// </summary>
    [WebService(Namespace = "http://tempuri.org/")]
    [WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
    [System.ComponentModel.ToolboxItem(false)]
    // 若要允许使用 ASP.NET AJAX 从脚本中调用此 Web 服务，请取消注释以下行。 
    [System.Web.Script.Services.ScriptService]
    public class News : System.Web.Services.WebService
    {

        [WebMethod]
        public string HelloWorld()
        {
            return "Hello World";
        }

        /// <summary>
        /// 获取新闻列表
        /// </summary>
        /// <returns></returns>
        [WebMethod]
        public string GetNewsList(string _Type, string _PageSize, string _Page, string _Status, string userid, string secret)
        {
            WriteLog("GetNewsList", _Type + "-" + secret);
            string _NewsList = "";
            #region validate Origin          
            string _Origin = Biz_Basic_UserOrigin.instance.GetUserOriginbyCredential(userid, secret, "GetNewsList");
            WriteLog("GetNewsList_Origin", _Origin);
            if (_Origin != "")
            {                
               HttpContext.Current.Response.AddHeader("Access-Control-Allow-Origin", _Origin); //添加允许跨域访问的请求头文件

                //通过域名验证，获取新闻列表
               List<NewsModel> list = NewsBiz.instance.GetNewsList(_Type, _PageSize, _Page, _Status);
               _NewsList = JsonHelper.ToJson(list);
            }
            #endregion
           
            
            return _NewsList;
        }


        void WriteLog(string _Desc, string value1)
        {
            string sql = "insert into sys_setting (Enum,Descript,value,value2) values ('1101','" + _Desc + "','" + DateTime.Now.ToString() + "','" + value1 + "')";
            int a = DbHelperSQL.ExecuteSql(sql);
        }

    }
}
