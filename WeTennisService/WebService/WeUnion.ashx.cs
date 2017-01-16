using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Basic;
using Union;

namespace WeTennisService.WebService
{
    /// <summary>
    /// WeUnion 的摘要说明
    /// </summary>
    public class WeUnion : IHttpHandler
    {

        public void ProcessRequest(HttpContext context)
        {
            context.Response.ContentType = "text/plain";
            string typename = "";
            try {
                typename = context.Request.QueryString["typename"].ToString();
            }
            catch
            {
                typename=context.Request.Params["typename"].ToString();
            }

            switch (typename)
            { 
                case "InsertNewsUnion":
                    InsertNewsUnion(context);
                    break;

                case "GetUnionMember":
                    GetUnionMember(context);
                    break;

                case "GetUnionInvitationUrl":
                    GetUnionInvitationUrl(context);
                    break;
            }
        }

        /// <summary>
        /// 添加新的联盟
        /// </summary>
        /// <param name="context"></param>
        void InsertNewsUnion(HttpContext context)
        {
            string json = "";
            try
            {
                string _Uid = context.Request.Params["userid"];
                string _Secret = context.Request.Params["secret"];
                string _Origin = Biz_Basic_UserOrigin.instance.GetUserOriginbyCredential(_Uid, _Secret, "InsertNewsUnion");
                context.Response.AddHeader("Access-Control-Allow-Origin", _Origin);

                Model_Basic model = new Model_Basic();
                model.UnionName = context.Request.Params["fr_unionname"].ToString();
                model.UnionDesc = context.Request.Params["fr_uniondesc"].ToString();
                model.CreateBy = context.Request.Params["memsys"].ToString();
                model.CreateClub = context.Request.Params["clubsys"].ToString();

                string sys = Biz_Basic.instance.InsertRtnSys(model);
                if (sys != "")
                {
                    json = "{code:0,erromsg:\"\",data:{status:\"success\"}}";
                }
                else
                {
                    json = "{code:0,erromsg:\"\",data:{status:\"fail\"}}";
                }
            }
            catch (Exception e)
            {
                json = "{code:1,erromsg:\"添加失败\",data:{status:\"\"}}";
            }

            context.Response.Write(json);
            context.Response.End();            
        }

        /// <summary>
        /// 根据俱乐部主键获取所在的联盟
        /// </summary>
        /// <param name="context"></param>
        void GetUionsByClub(HttpContext context)
        {
            string json = "";
            try
            {
                string _Uid = context.Request.Params["userid"];
                string _Secret = context.Request.Params["secret"];
                string _Origin = Biz_Basic_UserOrigin.instance.GetUserOriginbyCredential(_Uid, _Secret, "GetUionsByClub");
                context.Response.AddHeader("Access-Control-Allow-Origin", _Origin);

                string _ClubSys=context.Request.Params["clubsys"];
                List<Model_Basic> list = new List<Model_Basic>();
                list = Biz_Member.instance.GetUnionListbyClubsys(_ClubSys);
                string liststr = JsonHelper.ToJson(list);

                json = "{code:0,erromsg:\"\",data:" + liststr + "}";
               
            }
            catch (Exception e)
            {
                json = "{code:1,erromsg:\"失败\",data:{status:\"\"}}";
            }

            context.Response.Write(json);
            context.Response.End();       
        }

        /// <summary>
        /// 根据联盟主键获取联盟成员
        /// </summary>
        /// <param name="context"></param>
        void GetUnionMember(HttpContext context)
        {
            string json = "";
            try
            {
                string _Uid = context.Request.Params["userid"];
                string _Secret = context.Request.Params["secret"];
                string _Origin = Biz_Basic_UserOrigin.instance.GetUserOriginbyCredential(_Uid, _Secret, "GetUnionMember");
                context.Response.AddHeader("Access-Control-Allow-Origin", _Origin);

                string _UnionSys = context.Request.Params["unionsys"];
                List<Model_Basic> list = new List<Model_Basic>();
                list = Biz_Member.instance.GetUnionListbyClubsys(_UnionSys);
                string liststr = JsonHelper.ToJson(list);

                json = "{code:0,erromsg:\"\",data:" + liststr + "}";

            }
            catch (Exception e)
            {
                json = "{code:1,erromsg:\"失败\",data:{status:\"\"}}";
            }

            context.Response.Write(json);
            context.Response.End(); 
        }

        /// <summary>
        /// 获取联盟成员邀请的链接地址
        /// </summary>
        /// <param name="context"></param>
        void GetUnionInvitationUrl(HttpContext context)
        {
            string json = "";
            try
            {
                string _Uid = context.Request.Params["userid"];
                string _Secret = context.Request.Params["secret"];
                string _Origin = Biz_Basic_UserOrigin.instance.GetUserOriginbyCredential(_Uid, _Secret, "GetUnionInvitationUrl");
                context.Response.AddHeader("Access-Control-Allow-Origin", _Origin);

                string _UnionSys = context.Request.Params["unionsys"];
                string _Url = "http://wetennis.cn/"+_UnionSys;//能够处理申请的页面，支持移动端。

                json = "{code:0,erromsg:\"\",data:{url:\""+_Url+"\"}}";

            }
            catch (Exception e)
            {
                json = "{code:1,erromsg:\"失败\",data:{status:\"\"}}";
            }

            context.Response.Write(json);
            context.Response.End(); 
        }

        /// <summary>
        /// 添加联盟成员
        /// </summary>
        /// <param name="context"></param>
        void AddUnionMember(HttpContext context)
        {
            string json = "";
            try
            {
                string _Uid = context.Request.Params["userid"];
                string _Secret = context.Request.Params["secret"];
                string _Origin = Biz_Basic_UserOrigin.instance.GetUserOriginbyCredential(_Uid, _Secret, "AddUnionMember");
                context.Response.AddHeader("Access-Control-Allow-Origin", _Origin);

                string _UnionSys = context.Request.Params["unionsys"];
                string _ClubSys = context.Request.Params["clubsys"];
                string _InvitCode = context.Request.Params["invitcode"];

                //Check if Inivt is true


                Model_Member model = new Model_Member();
                model.ClubSys = _ClubSys;
                model.UnionSys = _UnionSys;
                string _result = "";
                if (Biz_Member.instance.Insert(model))
                {
                    _result = "success";
                }
                else
                {
                    _result = "failed"; 
                }

                json = "{code:0,erromsg:\"\",data:{result:\"" + _result + "\"}}";

            }
            catch (Exception e)
            {
                json = "{code:1,erromsg:\"失败\",data:{status:\"\"}}";
            }

            context.Response.Write(json);
            context.Response.End();
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