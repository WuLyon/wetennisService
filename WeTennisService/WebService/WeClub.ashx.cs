using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Club;

namespace WeTennisService.WebService
{
    /// <summary>
    /// WeClub 的摘要说明
    /// </summary>
    public class WeClub : IHttpHandler
    {

        public void ProcessRequest(HttpContext context)
        {
            context.Response.ContentType = "text/plain";
            string typename = "";
            try
            {
                typename = context.Request.QueryString["typename"].ToString();
            }
            catch 
            {
                typename = context.Request.Params["typename"].ToString();
            }

            switch (typename)
            {
                case "UpdateClubField":
                    UpdateClubField(context);
                    break;

            }
        }

        void UpdateClubField(HttpContext context)
        {
            context.Response.AddHeader("Access-Control-Allow-Origin", "*"); //添加允许跨域访问的请求头文件
            string _KeyName = context.Request.Params["fieldname"].ToString();
            string _KeyValue = context.Request.Params["keyvalue"].ToString();
            string _Clubsys = context.Request.Params["clubsys"].ToString();

            string result = "";
            try
            {
                if (ClubBiz.Get_Instance().UpdateClubField(_KeyName, _KeyValue, _Clubsys))
                {
                    result = "{code:0,erromsg:\"\",data:{status:\"success\"}}";
                }
                else
                {
                    result = "{code:0,erromsg:\"\",data:{status:\"false\"}}";
                }
            }
            catch(Exception e)
            {
                result = "{code:1,erromsg:\"系统执行错误\",data:{status:\""+e.ToString().Substring(0,100)+"\"}}";
            }
            context.Response.Write(result);
        }

        public bool IsReusable
        {
            get
            {
                return false;
            }
        }
    }
}