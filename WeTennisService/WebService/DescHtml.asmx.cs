using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Services;
using SMS;
using News;

namespace WeTennisService.WebService
{
    /// <summary>
    /// DescHtml 的摘要说明
    /// </summary>
    [WebService(Namespace = "http://tempuri.org/")]
    [WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
    [System.ComponentModel.ToolboxItem(false)]
    // 若要允许使用 ASP.NET AJAX 从脚本中调用此 Web 服务，请取消注释以下行。 
    // [System.Web.Script.Services.ScriptService]
    public class DescHtml : System.Web.Services.WebService
    {

        [WebMethod]
        public string HelloWorld()
        {
            return "Hello World";
        }

        /// <summary>
        /// 添加新的描述
        /// </summary>
        /// <param name="_type"></param>
        /// <param name="_typesys"></param>
        /// <param name="_Content"></param>
        [WebMethod]        
        public void InsertNewDesc(string _type, string _typesys, string _Content)
        {
            DescBll.instance.InsertNew(_type, _typesys, _Content);
        }

        [WebMethod]
        public string  GetNewsContent(string _type, string _typesys)
        {
             return DescBll.instance.GetContent(_type, _typesys);
        }

        [WebMethod]
        public string UpdateNews(NewsModel model)
        { 
            string result="";
            if(!string.IsNullOrEmpty(model.SYSNO))
            {
                //修改新闻内容
                NewsBiz.instance.UpdateNews(model);
                result = "修改成功";
            }
            else
            {
                //创建新闻
                result=NewsBiz.instance.InsertNews(model);
            }
            return result;
        }

    }
}
