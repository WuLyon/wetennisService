using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Member;
using Club;

namespace WeTennisService.WebService
{
    /// <summary>
    /// WeMember 的摘要说明
    /// </summary>
    public class WeMember : IHttpHandler
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
                case "GetMemberInfo":
                    GetMemberInfo(context);
                    break;

                case "UpdateMemberID":
                    UpdateMemberID(context);
                    break;

                case "GetMemberFriends":
                    GetMemberFriends(context);
                    break;

                case "ValideMemberInfo":
                    ValideMemberInfo(context);
                    break;

                case "ValidateLogin":
                    ValidateLogin(context);
                    break;

                case "RegisterNewMember":
                    RegisterNewMember(context);
                    break;

                case "RapidRegister":
                    RapidRegister(context);
                    break;

                case "UpdateMemberTele":
                    UpdateMemberTele(context);
                    break;

                case "ValidateClubLogin":
                    ValidateClubLogin(context);
                    break;
            }
        }
        /// <summary>
        /// 获得用户基本信息
        /// </summary>
        /// <param name="context"></param>
        void GetMemberInfo(HttpContext context)
        {
            //处理回调函数
            context.Response.ContentType = "application/json;charset=utf-8";
            string jsonCallBackFunName = context.Request.Params["jsoncallback"].ToString();

            string Memsys = context.Request.Params["memsys"].ToString();
            //莫名其妙出现_memsys的值为两个重复的员工号
            if (Memsys.IndexOf(",") > 0)
            {
                Memsys = Memsys.Split(',')[0];
            }
            WriteLog("GetMemberInfo", Memsys);
            WeMemberModel model = WeMemberDll.instance.GetModel(Memsys);
            string json = JsonHelper.ToJson(model);

            //处理查询结果
            context.Response.Write(string.Format("{0}({1})", jsonCallBackFunName, json));
            context.Response.End();
        }

        void UpdateMemberID(HttpContext context)
        {
            //处理回调函数
            context.Response.ContentType = "application/json;charset=utf-8";
            string jsonCallBackFunName = context.Request.Params["jsoncallback"].ToString();

            WeMemberModel model = new WeMemberModel();
            model.SYSNO = context.Request.Params["memsys"].ToString();
            model.USERNAME = context.Request.Params["username"].ToString();
            model.TELEPHONE = context.Request.Params["telephone"].ToString();
            model.EXT2 = context.Request.Params["idcard"].ToString();

            //莫名其妙出现_memsys的值为两个重复的员工号
            if (model.SYSNO.IndexOf(",") > 0)
            {
                model.SYSNO = model.SYSNO.Split(',')[0];
            }

            WriteLog("UpdateMemberID", model.SYSNO + "-" + model.USERNAME + "-" + model.TELEPHONE + "-" + model.EXT2);
            WeMemberDll.instance.UpdateMemberInfo(model);

            //处理查询结果
            context.Response.Write(string.Format("{0}({1})", jsonCallBackFunName, ""));
            context.Response.End();
        }

        /// <summary>
        /// 获得关注的人
        /// </summary>
        /// <param name="context"></param>
        void GetMemberFriends(HttpContext context)
        {
            //处理回调函数
            context.Response.ContentType = "application/json;charset=utf-8";
            string jsonCallBackFunName = context.Request.Params["jsoncallback"].ToString();

            string Memsys = context.Request.Params["memsys"].ToString();
            WriteLog("GetMemberFriends", Memsys);
            //莫名其妙出现_memsys的值为两个重复的员工号
            if (Memsys.IndexOf(",") > 0)
            {
                Memsys = Memsys.Split(',')[0];
            }
            List<WeMemberModel> list = WeMemberDll.instance.GetFollowerModellist(Memsys);
            string json = JsonHelper.ToJson(list);

            //处理查询结果
            context.Response.Write(string.Format("{0}({1})", jsonCallBackFunName, json));
            context.Response.End();
        }

        
        /// <summary>
        /// 验证微网球账户登录情况，并返回用户主键
        /// </summary>
        /// <param name="context"></param>
        void ValideMemberInfo(HttpContext context)
        {
            context.Response.AddHeader("Access-Control-Allow-Origin", "http://wetennis.cn"); //添加允许跨域访问的请求头文件
            //接收参数
            string _Username = context.Request.Form["username"];
            string _Pass = context.Request.Form["pass"];
            string _OpenId = context.Request.Form["openid"];

            string _Sys = "";
            //验证正确性
            if (WeMemberDll.instance.ValidateUser(_Username, _Pass))
            {
                //验证通过
                WeMemberModel model = WeMemberDll.instance.GetModelbyName(_Username);
                WeMemberDll.instance.BindUser(model.SYSNO, _OpenId);
                if (model.TELEPHONE.Trim().Length == 11)
                {
                    //用户电话号码正确
                    _Sys = "ok";
                }
                else {
                    //用户电话号码不正确
                    _Sys = "ok1";
                }
            }
            context.Response.Write(_Sys);
        }

        /// <summary>
        /// 用户信息认证，返回用户信息
        /// 输入：用户名，电话号码
        /// 输出：用户信息
        /// </summary>
        /// <param name="context"></param>
        void ValidateLogin(HttpContext context)
        {
            context.Response.AddHeader("Access-Control-Allow-Origin", "*"); //添加允许跨域访问的请求头文件
            string _Username = context.Request.Params["username"];
            string _Pass = context.Request.Params["pass"];
            WeMemberModel model = new WeMemberModel();
            if (WeMemberDll.instance.ValidateUser(_Username, _Pass))
            { 
                //验证通过，返回用户信息
                model = WeMemberDll.instance.GetModelbyName(_Username);
                if (string.IsNullOrEmpty(model.SYSNO))
                {
                    model = WeMemberDll.instance.GetModelbyTelephone(_Username);
                }
            }
            string json = JsonHelper.ToJson(model);
            context.Response.Write(json);
        }

        void ValidateClubLogin(HttpContext context)
        {
            context.Response.AddHeader("Access-Control-Allow-Origin", "*"); //添加允许跨域访问的请求头文件
            string _Username = context.Request.Params["username"];
            string _Pass = context.Request.Params["pass"];
            string result = "";
            WeMemberModel model = new WeMemberModel();
            if (WeMemberDll.instance.ValidateUser(_Username, _Pass))
            {
                //验证通过，返回用户信息
                model = WeMemberDll.instance.GetModelbyName(_Username);
                if (string.IsNullOrEmpty(model.SYSNO))
                {
                    model = WeMemberDll.instance.GetModelbyTelephone(_Username);
                }
                //根据登陆人信息，获取所属俱乐部信息
                try
                {
                    List<ClubModel> list = ClubMemBiz.instance.GetListbyMemsys(model.SYSNO);
                    
                    if (list.Count > 0)
                    {
                        list[0].EXT3 = model.SYSNO;//暂时用来存放当前登录人主键
                        if (list[0].EXT2.IndexOf("wetennis.cn") < 0)
                        {
                            list[0].EXT2 = "http://wetennis.cn" + list[0].EXT2;
                        }
                        string json = JsonHelper.ToJson(list[0]);
                        result = "{code:\"0\",errormsg:\"\",data:"+json+"}";
                    }
                    else
                    {
                        result = "{code:\"2\",errormsg:\"非俱乐部管理员,请使用会员登陆通道\",data:[{}]}";
                    }
                }
                catch (Exception e)
                {
                    result = e.ToString().Substring(0, 200);
                }
            }
            else
            {
                result = "{code:\"1\",errormsg:\"用户名或密码错误\",data:[{}]}";
            }           
            context.Response.Write(result);
        }

        /// <summary>
        /// 注册新会员
        /// </summary>
        /// <param name="context"></param>
        void RegisterNewMember(HttpContext context)
        {
            context.Response.AddHeader("Access-Control-Allow-Origin", "http://wetennis.cn"); //添加允许跨域访问的请求头文件

            //接受参数
            WeMemberModel model = new WeMemberModel();
            model.USERNAME = context.Request.Form["nickname"];
            model.NAME = context.Request.Form["nickname"];
            model.GENDER = context.Request.Form["sex"] == "1" ? "男" : "女";
            model.PROVINCE = context.Request.Form["province"];
            model.CITY = context.Request.Form["City"];
            model.EXT1 = context.Request.Form["headimgurl"];
            model.EXT3 = context.Request.Form["openid"];

            //插入记录
            if (model.USERNAME != "")
            {
                WeMemberDll.instance.CreateUser(model);
                context.Response.Write("ok");
            }
            else
            {
                context.Response.Write("no");
            }
        }

        void RapidRegister(HttpContext context)
        {
            context.Response.AddHeader("Access-Control-Allow-Origin", "*"); //添加允许跨域访问的请求头文件

            string _Res = "";
            try
            {
                WeMemberModel model = new WeMemberModel();
                string Name = context.Request.Params["name"];
                string Telephone = context.Request.Params["tele"];
                model.NAME = Name;
                model.USERNAME = Name;
                model.TELEPHONE = Telephone;
                model.PASSWORD = "123";
                string _sysno = WeMemberDll.instance.CreateUser(model);
                _Res = "{code:0,errormsg:\"\",data:\"" + _sysno + "\"}";
            }
            catch (Exception e)
            {
                _Res = "{code:1,errormsg:\""+e.ToString().Substring(0,100)+"\"}";
            }

            context.Response.Write(_Res);
        }

        /// <summary>
        /// 修改用户的电话号码
        /// </summary>
        /// <param name="context"></param>
        void UpdateMemberTele(HttpContext context)
        {
            //处理回调函数
            context.Response.ContentType = "application/json;charset=utf-8";
            string jsonCallBackFunName = context.Request.Params["jsoncallback"].ToString();

            string UserName = context.Request.Params["username"];
            string OpenId = context.Request.Params["openid"];
            string Tele = context.Request.Params["Tele"];
            WriteLog("UpdateMemberTele", UserName + "-" + OpenId + "-" + Tele);

            string mes = "";
            if (WeMemberDll.instance.UpdateUserTelephone(UserName, OpenId, Tele))
            {
                mes = "ok";
            }
            else
            {
                mes = "no";
            }
            //处理查询结果
            context.Response.Write(string.Format("{0}({1})", jsonCallBackFunName, "{\"message\":\"" + mes + "\"}"));
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