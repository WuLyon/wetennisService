using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Bet;

namespace WeTennisService.WebService
{
    /// <summary>
    /// WeBet 的摘要说明
    /// </summary>
    public class WeBet : IHttpHandler
    {

        public void ProcessRequest(HttpContext context)
        {
            context.Response.ContentType = "text/plain";
            string typename = context.Request.QueryString["typename"].ToString();
            switch (typename)
            { 
                case "GetRecentDates":
                    GetRencentDates(context);
                    break;

                case "GetBetWinList":
                    GetBetWinList(context);
                    break;
            }
        }

        /// <summary>
        /// 获取最近一个月的日期
        /// </summary>
        /// <param name="context"></param>
        void GetRencentDates(HttpContext context)
        {
            //处理回调函数
            context.Response.ContentType = "application/json;charset=utf-8";
            string jsonCallBackFunName = context.Request.Params["jsoncallback"].ToString();

            WriteLog("GetRencentDates", "");
            List<BetWinModel> list=BetWinDll.instance.GetRencentEnddates();
            string json=JsonHelper.ToJson(list);

            //处理查询结果
            context.Response.Write(string.Format("{0}({1})", jsonCallBackFunName, json));
            context.Response.End();
        }

        /// <summary>
        /// 获得竞猜获胜者的信息
        /// </summary>
        /// <param name="context"></param>
        void GetBetWinList(HttpContext context)
        {
            //处理回调函数
            context.Response.ContentType = "application/json;charset=utf-8";
            string jsonCallBackFunName = context.Request.Params["jsoncallback"].ToString();

            string _EndTime = context.Request.Params["endtime"].ToString();
            WriteLog("GetBetWinList", _EndTime);
            List<BetDateModel> list = BetWinDll.instance.GetBetWinlist(_EndTime);
            string json = JsonHelper.ToJson(list);

            //处理查询结果
            context.Response.Write(string.Format("{0}({1})", jsonCallBackFunName, json));
            context.Response.End();
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