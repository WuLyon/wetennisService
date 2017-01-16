using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Member;
using WeTour;

namespace WeTennisService.BackService
{
    /// <summary>
    /// AutoComp 的摘要说明
    /// </summary>
    public class AutoComp : IHttpHandler
    {
        string _AllowOrigin = "*";
        public void ProcessRequest(HttpContext context)
        {
            context.Response.ContentType = "text/plain";
            string typename = "";
            try {
                typename = context.Request.QueryString["typename"];
            }
            catch
            {
                typename=context.Request.Params["typename"];
            }
            switch (typename)
            { 
                case "Auto_Member":
                    Auto_Member(context);
                    break;

                case "Auto_Apply":
                    Auto_Apply(context);
                    break;
            }

        }

        /// <summary>
        /// 查询用户,根据用户名，姓名，电话
        /// </summary>
        /// <param name="context"></param>
        void Auto_Member(HttpContext context)
        {
            context.Response.AddHeader("Access-Control-Allow-Origin", _AllowOrigin);
            string _Res = "";
            try
            {
                string _Cond = context.Request.Params["q"];
               // DbHelperSQL.WriteLog("Auto_Member", _Cond);
                List<WeMemberModel> list = WeMemberDll.instance.GetMemberlistbyCond(_Cond);
                string json = JsonHelper.ToJson(list);
                _Res = "{\"code\":0,\"qty\":"+list.Count+",\"data\":" + json + "}";
            }
            catch(Exception e)
            {
                _Res = "{code:2,errormsg:\""+e.ToString().Substring(0,100)+"\",data:\"\"}";
            }
            //DbHelperSQL.WriteLog("Auto_Member_res", _Res);
            context.Response.Write(_Res);
        }

        /// <summary>
        /// 获取报名名单
        /// </summary>
        /// <param name="context"></param>
        void Auto_Apply(HttpContext context)
        {
            context.Response.AddHeader("Access-Control-Allow-Origin", _AllowOrigin);
            string _Res = "";
            try
            {
                string _Cond = context.Request.Params["q"];
                string _ContentId = context.Request.Params["cont"];

                List<WeMemberModel> list = WeTourApplyDll.instance.GetSeedApplyMember(_Cond,_ContentId);
                string json = JsonHelper.ToJson(list);
                _Res = "{\"code\":0,\"qty\":" + list.Count + ",\"data\":" + json + "}";
            }
            catch (Exception e)
            {
                _Res = "{code:2,errormsg:\"" + e.ToString().Substring(0, 100) + "\",data:\"\"}";
            }
            //DbHelperSQL.WriteLog("Auto_Member_res", _Res);
            context.Response.Write(_Res);
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