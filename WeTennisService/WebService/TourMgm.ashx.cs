using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using WeTour;

namespace WeTennisService.WebService
{
    /// <summary>
    /// TourMgm 的摘要说明
    /// </summary>
    public class TourMgm : IHttpHandler
    {

        public void ProcessRequest(HttpContext context)
        {
            context.Response.ContentType = "text/plain";
            string typename = "";
            try
            {
                typename = context.Request.QueryString["typename"];
            }
            catch {
                typename = context.Request.Params["typename"];
            }

            switch (typename)
            { 
                case "GetTourList":
                    GetTourList(context);
                    break;
            }
        }

        void GetTourList(HttpContext context)
        {
            context.Response.AddHeader("Access-Control-Allow-Origin", "*"); //添加允许跨域访问的请求头文件

            string _ClubSys = context.Request.Params["ClubSys"];
            string _Status = context.Request.Params["status"];
            string _TourType = context.Request.Params["tourtype"];
            string json = "";
            try
            {
                List<Model_ApiTourList> list = Biz_TourMgm.instance.GetTourList(_ClubSys, _Status, _TourType);
                string datajson = JsonHelper.ToJson(list);
                json = "{code:0,errormsg:\"\",data:" + datajson + ",requestdate:\""+DateTime.Now.ToString()+"\"}";
            }
            catch(Exception e)
            {
                json = "{code:1,errormsg:\"服务器错误，请联系管理员\",data:\""+e.ToString().Substring(0,100)+"\"}";
            }
            context.Response.Write(json);
        }

        void WriteLog(string _Desc, string value1)
        {
            string sql = "insert into sys_setting (Enum,Descript,value,value2) values ('1101','" + _Desc + "','" + DateTime.Now.ToString() + "','" + value1 + "')";
            int a = DbHelperSQL.ExecuteSql(sql);
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