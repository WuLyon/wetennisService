using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data;
using WeTour;
using Ranking;
using Club;
using Member;
using Basic;

namespace WeTennisService.BackService
{
    /// <summary>
    /// TourMgmJson 的摘要说明
    /// </summary>
    public class TourMgmJson : IHttpHandler
    {

        public void ProcessRequest(HttpContext context)
        {
            context.Response.ContentType = "application/json;charset=UTF-8";
            context.Response.AddHeader("Access-Control-Allow-Origin","*");
            string typename = "";
            try
            {
                typename = context.Request.Params["typename"].ToString();
            }
            catch
            {
                typename = context.Request.QueryString["typename"].ToString();
            }
            //添加记录
            Model_Basic_Service_Log serivce = new Model_Basic_Service_Log();
            serivce.ServiceName = typename;
            serivce.URL = context.Request.Url.ToString();
            Biz_Basic_Service_log.instance.AddNewLog(serivce);

            switch (typename)
            {

                #region 俱乐部管理员
                case "RegisterClub":
                    RegisterClub(context);
                    break;
                #endregion
            }
        }

        #region 俱乐部管理员
        void RegisterClub(HttpContext context)
        {
            Model_Return ret = new Model_Return();
            try
            {
                string _phone = context.Request.Form["phone"];
                string _username = context.Request.Form["username"];
                string _password = context.Request.Form["password"];
                string _clubName = context.Request.Form["clubname"];
                #region 业务操作
                //判断俱乐部是否已被注册
                string _ClubSys = "";
                if (!ClubBiz.Get_Instance().IsClubNameDuplicated(_clubName))
                {
                    //添加俱乐部
                    ClubModel newClub = new ClubModel();
                    newClub.CLUBNAME = _clubName;
                    _ClubSys = ClubBiz.Get_Instance().AddNewClub(newClub);

                    //添加俱乐部管理员
                    string _Memsys = "";
                    if (!WeMemberDll.instance.IsphoneDuplicated(_phone))
                    {
                        //电话已注册，通过电话获取主键
                        WeMemberModel mem = WeMemberDll.instance.GetModelbyTelephone(_phone);
                        _Memsys = mem.SYSNO;
                    }
                    else
                    {
                        //电话不存在，重新注册会员
                        WeMemberModel mem = new WeMemberModel();
                        mem.USERNAME = _username;
                        mem.NAME = _username;
                        mem.TELEPHONE = _phone;
                        mem.PASSWORD = _password;
                        _Memsys = WeMemberDll.instance.CreateUser(mem);
                    }

                    //添加俱乐部管理员
                    ClubMemModel cMem = new ClubMemModel();
                    cMem.CLUBSYS = _ClubSys;
                    cMem.MEMSYS = _Memsys;
                    cMem.JOB = "管理员";
                    cMem.EXT1 = "1";
                    ClubMemBiz.instance.AddClubMember(cMem);

                    ret.code = 0;
                    ret.errorMsg = "";
                    Dictionary<string, string> dic = new Dictionary<string, string>();
                    dic.Add("Clubsys", _ClubSys);
                    dic.Add("Memsys", _Memsys);
                    //添加成员信息
                    WeMemberModel member = WeMemberDll.instance.GetModel(_Memsys);
                    dic.Add("username", member.NAME);
                    dic.Add("password",member.PASSWORD);

                    ret.data = dic;
                }
                else
                {
                    ret.code = 1;
                    ret.errorMsg = "俱乐部名称已被注册，请更改";
                    ret.data = "";
                }
                #endregion

            }
            catch (Exception e)
            {
                ret.code = 1;
                ret.errorMsg = e.ToString();
                ret.data = "";
            }
            context.Response.Write(JsonHelper.ToJson(ret));
        }
        #endregion

        public bool IsReusable
        {
            get
            {
                return false;
            }
        }
    }
}