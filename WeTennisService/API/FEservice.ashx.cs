using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Member;
using SMS;
using WeTour;
using Equip;
using OrderLib;
using Ranking;
using System.IO;
using System.Web.Script.Serialization;
using System.Runtime.Serialization.Json;

namespace WeTennisService.API
{
    /// <summary>
    /// FEservice 的摘要说明
    /// </summary>
    public class FEservice : IHttpHandler
    {
        string _Origin = "*";
        public void ProcessRequest(HttpContext context)
        {
            //添加记录
            
            //设定Request Header
            context.Response.AddHeader("Access-Control-Allow-Origin", _Origin);
            context.Response.ContentType = "application/json;charset=UTF-8";
            string typename = "";
            try
            {
                typename = context.Request.QueryString["method"].ToString();
            }
            catch {
                typename = context.Request.Params["method"].ToString();
            }

            //获取jsonstring.
            var jsonstr = string.Empty;
            context.Request.InputStream.Position = 0;
            using (var inputStream = new StreamReader(context.Request.InputStream))
            {
                jsonstr = inputStream.ReadToEnd();
            }  

            //写入log
            //LogHelper.instance.WriteLog("Service", typename + "|" + jsonstr);

            switch (typename)
            {
                #region 用户
                case "signup":
                    signup(context,jsonstr);
                    break;
                case "signin":
                    signin(context,jsonstr);
                    break;
                case "sendActivationCode":
                    sendActivationCode(context,jsonstr);
                    break;

                case "checkUserNameDuplicated":
                    checkUserNameDuplicated(context,jsonstr);
                    break;

                case "checkPhoneDuplicated":
                    checkPhoneDuplicated(context,jsonstr);
                    break;

                case "resetPassword":
                    resetPassword(context, jsonstr);
                    break;
                case "fetchUserInfo":
                    fetchUserInfo(context, jsonstr);
                    break;
                #endregion

                #region 赛事
                case "eventFilter":
                    eventFilter(context);
                    break;

                case "events":
                    events(context,jsonstr);
                    break;

                case "eventDetails":
                    eventDetails(context,jsonstr);
                    break;

                case "eventComments":
                    eventComments(context,jsonstr);
                    break;

                case "eventNotices":
                    eventNotices(context);
                    break;

                case "eventSendComment":
                    eventSendComment(context);
                    break;

                case "eventCommentLike":
                    eventCommentLike(context,jsonstr);
                    break;             

                case "eventSponsors":
                    eventSponsors(context,jsonstr);
                    break;

                case "eventFollow":
                    eventFollow(context,jsonstr);
                    break;
                case "cascadeFilter":
                    cascadeFilter(context, jsonstr);
                    break;

                case "eventDrawTableFilter":
                    eventDrawTableFilter(context, jsonstr);
                    break;

                case "eventDraw":
                    eventDraw(context,jsonstr);
                    break;

                case "eventDrawTable":
                    eventDrawTable(context,jsonstr);
                    break;

                case "eventDrawTableKnockOut":
                    eventDrawTableKnockOut(context,jsonstr);
                    break;

                case "eventMatchInfo":
                    eventMatchInfo(context, jsonstr);
                    break;

                case "eventScheduleFilter":
                    eventScheduleFilter(context,jsonstr);
                    break;

                case "eventSchedule":
                    eventSchedule(context, jsonstr);
                    break;

                case "eventScore":
                    eventScore(context, jsonstr);
                    break;

                case "eventScoreStateFilter":
                    eventScoreStateFilter(context, jsonstr);
                    break;
                case "eventMatchTechnicalStatistics":
                    eventMatchTechnicalStatistics(context, jsonstr);
                    break;
                #endregion

                #region 我
                case "fetchMyData":
                    fetchMyData(context,jsonstr);
                    break;

                case "updateMySettings":
                    updateMySettings(context, jsonstr);
                    break;

                case "fetchMySettings":
                    fetchMySettings(context,jsonstr);
                    break;

                case "fetchMyMatch":
                    fetchMyMatch(context,jsonstr);
                    break;

                case "fetchMyPractice":
                    fetchMyPractice(context, jsonstr);
                    break;

                case "updateBGImage":
                    updateBGImage(context, jsonstr);
                    break;

                case "addEquipment":
                    addEquipment(context, jsonstr);
                    break;
                case "editEquipment":
                    editEquipment(context, jsonstr);
                    break;
                case "deleteEquipment":
                    deleteEquipment(context, jsonstr);
                    break;

                #endregion

                #region 报名
                case "eventGroups":
                    eventGroups(context,jsonstr);
                    break;

                case "registeredUsers":
                    registeredUsers(context, jsonstr);
                    break;

                case "fetchPartners":
                    fetchPartners(context, jsonstr);
                    break;

                case "registerEvent":
                    registerEvent(context, jsonstr);
                    break;

                case "registerTeam":
                    registerTeam(context, jsonstr);
                    break;

                case "fetchRegisteredTeams":
                    fetchRegisteredTeams(context, jsonstr);
                    break;

                case "fetchRegisteredTeamMembers":
                    fetchRegisteredTeamMembers(context, jsonstr);
                    break;

                case "fetchRegisteredTeamSequence":
                    fetchRegisteredTeamSequence(context, jsonstr);
                    break;

                case "updateRegisteredTeamSequence":
                    updateRegisteredTeamSequence(context, jsonstr);
                    break;
                #endregion

                #region 排名

                case "rankingTypeFilter":
                    rankingTypeFilter(context);
                    break;
                case "rankingFilter":
                    rankingsFilter(context);
                    break;

                case "rankings":
                    rankings(context,jsonstr);
                    break;

                case "rankingDetailsInfo":
                    rankingDetailsInfo(context, jsonstr);
                    break;
                #endregion

                #region 时光
                case "uploadImage":
                    uploadImage(context, jsonstr);
                    break;
                #endregion

                #region 评论
                case "fetchComments":
                    fetchComments(context, jsonstr);
                    break;

                case "sendComment":
                    sendComment(context, jsonstr);
                    break;
                case "likeComment":
                    likeComment(context, jsonstr);
                    break;

                #endregion

                #region 赛程调整
                case "fetchProgram":
                    fetchProgram(context, jsonstr);
                    break;

                case "updateProgram":
                    updateProgram(context, jsonstr);
                    break;
                #endregion



                #region Default
                default:
                    Model_Return model = new Model_Return();
                    model.code = 1;
                    model.errorMsg = "请求方法名不存在，请确认方法名";
                    string res = JsonHelper.ToJson(model);
                    context.Response.Write(res);
                    break;
                #endregion
            }
        }

        #region 用户
        /// <summary>
        /// 注册
        /// </summary>
        /// <param name="context"></param>
        void signup(HttpContext context,string jsonstr)
        {
            Model_Return ret = new Model_Return();
            string resStr = "";
            try
            {
                Model_req_signup req = new Model_req_signup();
                req = JsonHelper.ParseFormJson<Model_req_signup>(jsonstr);

                string Pass = req.password;
                string UserName = req.username;
                string Phone = req.phone;
                string Code = req.activationCode;

                //验证验证码
                if (TeleCheckDll.instance.ValidateCode(Phone, Code))
                {
                    ret.code = 0;
                    ret.errorMsg = "";
                    //注册
                    WeMemberModel model = new WeMemberModel();
                    model.NAME = UserName;
                    model.USERNAME = UserName;
                    model.TELEPHONE = Phone;
                    model.PASSWORD = Pass;

                    model.SYSNO = WeMemberDll.instance.CreateUser(model);
                   
                    //
                    Model_Fe_SignUp signs = new Model_Fe_SignUp();
                    signs.id = model.SYSNO;
                    signs.username = UserName;
                    signs.password = Pass;
                    signs.phone = Phone;

                    ret.data = signs;                   
                }
                else
                {
                    ret.code = 1;
                    ret.errorMsg = "验证码已失效，请重新获取";
                    ret.data = "";
                }
            }
            catch (Exception e)
            {                
                ret.code = 2;
                ret.errorMsg = e.ToString();
                ret.data = "";
            }
            resStr = JsonHelper.ToJson(ret);
            context.Response.Write(resStr);
        }

        /// <summary>
        /// 用户登录
        /// </summary>
        /// <param name="context"></param>
        void signin(HttpContext context,string jsonstr)
        {
            Model_Return ret = new Model_Return();
            string _Ret="";
            try
            {
                Model_req_signup req = new Model_req_signup();
                req = JsonHelper.ParseFormJson<Model_req_signup>(jsonstr);

                string _Username = req.username;
                string _Pass = req.password;

                WeMemberModel model = new WeMemberModel();
                if (WeMemberDll.instance.ValidateUser(_Username, _Pass))
                {
                    //验证通过，返回用户信息
                    model = WeMemberDll.instance.GetModelbyName(_Username);
                    ret.code = 0;
                    ret.errorMsg = "";
                    Model_Fe_Signin sigins = new Model_Fe_Signin();
                    sigins.id = model.SYSNO;
                    sigins.username = model.NAME;
                    sigins.name = model.USERNAME;
                    sigins.password = model.PASSWORD;
                    sigins.phone = model.TELEPHONE;
                    sigins.gender = model.GENDER;
                    sigins.cardId = model.EXT2;
                    ret.data = sigins;
                }
                else
                {
                    ret.code = 1;
                    ret.errorMsg = "用户登录失败";
                    ret.data = "";
                }
            }
            catch (Exception e)
            {
                ret.code = 1;
                ret.errorMsg = e.ToString();
                ret.data = "";
            }
            _Ret = JsonHelper.ToJson(ret);
            context.Response.Write(_Ret);
        }

        /// <summary>
        /// 发送短信校验码
        /// </summary>
        /// <param name="context"></param>
        void sendActivationCode(HttpContext context,string jsonstr)
        {
            Model_Return ret = new Model_Return();            
            string mes = "";            
            try
            {
                Model_req_signup req = new Model_req_signup();
                req = JsonHelper.ParseFormJson<Model_req_signup>(jsonstr);
                string Phone = req.phone;
                string code= TeleCheckDll.instance.GreateCode(Phone);

                if (code == "FALSE")
                {
                    ret.code = 1;
                    ret.errorMsg = "验证码发送失败";
                    ret.data = "";
                }
                else
                {
                    ret.code = 0;
                    ret.errorMsg = "发送成功";
                    ret.data = "ok";
                }
            }
            catch (Exception e)
            {
                ret.code = 2;
                ret.errorMsg = e.ToString();
                ret.data = "";
            }
            mes = JsonHelper.ToJson(ret);
            context.Response.Write(mes);
        }


        /// <summary>
        /// 校验用户名是否重复
        /// </summary>
        /// <param name="context"></param>
        
        void checkUserNameDuplicated(HttpContext context,string jsonstr)
        {
            Model_Return ret = new Model_Return();
            
            string mes = "";
            try
            {
                Model_req_signup req = new Model_req_signup();
                req = JsonHelper.ParseFormJson<Model_req_signup>(jsonstr);

                string UserName =req.userName;
                if (WeMemberDll.instance.IsUserNameUniqu(UserName))
                {
                    ret.code = 0;
                    ret.errorMsg = "";
                    Model_Fe_userNameDuplicated ud = new Model_Fe_userNameDuplicated();
                    ud.userNameDuplicated = false;
                    ret.data = ud;
                }
                else
                {
                    ret.code = 0;
                    ret.errorMsg = "用户名不唯一，请重新输入";
                    Model_Fe_userNameDuplicated ud = new Model_Fe_userNameDuplicated();
                    ud.userNameDuplicated = true;
                    ret.data = ud;
                }
            }
            catch (Exception e)
            {
                ret.code = 2;
                ret.errorMsg = e.ToString();
                ret.data = "";
            }
            mes = JsonHelper.ToJson(ret);
            context.Response.Write(mes);
        }

        void checkPhoneDuplicated(HttpContext context,string jsonstr)
        {
           
            Model_Return ret = new Model_Return();
            string mes = "";
            try
            {
                Model_req_signup req = new Model_req_signup();
                req = JsonHelper.ParseFormJson<Model_req_signup>(jsonstr);

                string phone = req.phone;
                if (WeMemberDll.instance.IsphoneDuplicated(phone))
                {
                    ret.code = 0;
                    ret.errorMsg = "";
                    Model_Fe_phoneDuplicated pd = new Model_Fe_phoneDuplicated();
                    pd.phoneDuplicated = false;
                    ret.data = pd;
                }
                else
                {
                    ret.code = 0;
                    ret.errorMsg = "该电话号码已注册";
                    Model_Fe_phoneDuplicated pd = new Model_Fe_phoneDuplicated();
                    pd.phoneDuplicated = true;
                    ret.data = pd;
                    
                }
            }
            catch (Exception e){
                ret.code = 2;
                ret.errorMsg = e.ToString();
                ret.data = "";
            }
            mes = JsonHelper.ToJson(ret);
            context.Response.Write(mes);

        }

        /// <summary>
        /// 找回密码
        /// </summary>
        /// <param name="context"></param>
        void resetPassword(HttpContext context, string jsonstr)
        {
            Model_Return ret = new Model_Return();
            try
            {
                Fe_Model_Req_resetPassword reqmodel = new Fe_Model_Req_resetPassword();
                reqmodel = JsonHelper.ParseFormJson<Fe_Model_Req_resetPassword>(jsonstr);

                //验证code
                //验证验证码
                if (TeleCheckDll.instance.ValidateCode(reqmodel.phone, reqmodel.activationCode))
                {
                    //修改密码
                    WeMemberDll.instance.updatePasswordbyPhone(reqmodel.phone, reqmodel.password);
                    ret.code = 0;
                    ret.errorMsg = "";
                    Dictionary<string, bool> item = new Dictionary<string, bool>();
                    item.Add("resetPassword", true);
                    ret.data = item;
                }
                else
                {
                    ret.code = 2;
                    ret.errorMsg = "验证码无效";
                    Dictionary<string, bool> item = new Dictionary<string, bool>();
                    item.Add("resetPassword", false);
                    ret.data = item;
                }

            }
            catch (Exception e)
            {
                ret.code = 1;
                ret.errorMsg = e.ToString() ;
                ret.data = "";
            }
            context.Response.Write(JsonHelper.ToJson(ret));
        }

        /// <summary>
        /// 读取用户信息
        /// </summary>
        /// <param name="context"></param>
        void fetchUserInfo(HttpContext context,string json)
        {
            Model_Return model = new Model_Return();
            try
            {
                Model_req_eventDetail req = new Model_req_eventDetail();
                req = JsonHelper.ParseFormJson<Model_req_eventDetail>(json);
                string _userId = req.userId;               

                model.code = 0;
                model.errorMsg = "";
                model.data = Fe_Biz_Me.instance.Get_UserInfo(_userId);
            }
            catch (Exception e)
            {
                model.code = 0;
                model.errorMsg = e.ToString();
                model.data = "";
            }
            context.Response.Write(JsonHelper.ToJson(model));
        }
        #endregion

        #region 赛事，2016-7-16
        //赛事过滤条件，每一指明request
        void eventFilter(HttpContext context)
        {
            Model_Return res = new Model_Return();
            try
            {
                res.code = 0;
                res.data = Fe_Biz_TourList.instance.Fe_Get_Filters();
            }
            catch (Exception e)
            {
                res.code = 1;
                res.errorMsg = e.ToString();
            }
            string _Res = JsonHelper.ToJson(res);
            context.Response.Write(_Res);
        }
        //赛事列表
        void events(HttpContext context,string jsonstr)
        {          

            string _Res = "";
            Model_Return res = new Model_Return();
            try
            {
                Model_req_event req = new Model_req_event();
                req = JsonHelper.ParseFormJson<Model_req_event>(jsonstr);

                string _status = req.status;
                string _eventFilter = req.eventFilter;
                string _location = req.location;
                string _currentPage = req.currentPage;
                string _limit = req.limit;

                List<Fe_Model_EventList> list = Fe_Biz_TourList.instance.Fe_Get_Event_List(_status, _eventFilter, _location, _currentPage, _limit);
                res.code = 0;
                res.errorMsg = "";
                res.data = list;
            }
            catch (Exception e)
            {
                res.code = 1;
                res.errorMsg = e.ToString();
            }
            _Res = JsonHelper.ToJson(res);
            context.Response.Write(_Res);
        }

        /// <summary>
        /// 从cookie中获取登录人信息
        /// </summary>
        /// <param name="context"></param>
        /// <returns></returns>
        private string GetCookieUser(HttpContext context)
        {
            string _userid = "";
            try
            {
                _userid = context.Request.Headers["Cookie"];
                //LogHelper.instance.WriteLog("req", "current User in cookies is: "+_userid);
                //排除多个cookie的情况
                if (_userid.IndexOf(";") > 0)
                {
                    string[] cookies = _userid.Split(';');
                    for (int i = 0; i < cookies.Length; i++)
                    {
                        if (cookies[i].IndexOf("USER_ID") >= 0)
                        {
                            _userid = cookies[i].Split('=')[1];
                        }
                    }
                }
                else
                {
                    if (_userid.IndexOf("USER_ID") >= 0)
                    {
                        _userid = _userid.Split('=')[1];
                    }
                }

            }
            catch (Exception)
            {
                _userid = "";
            }
            return _userid;
        }
        //赛事详情
        void eventDetails(HttpContext context,string jsonstr)
        {          
            Model_Return model = new Model_Return();
            try
            {
                //获取userid
                string _userid = "";
                Model_req_eventDetail req = new Model_req_eventDetail();
                req = JsonHelper.ParseFormJson<Model_req_eventDetail>(jsonstr);
                string _id = req.id ;
                _userid = GetCookieUser(context);

                Fe_Model_EventInfo event1 = Fe_Biz_TourList.instance.Fe_Get_Event_Detail(_id,_userid);
                model.code = 0;
                model.data = event1;
            }
            catch (Exception e)
            {
                model.code = 1;
                model.errorMsg = e.ToString().Replace("\n","");
            }
            string _res = JsonHelper.ToJson(model);
            context.Response.Write(_res);
        }

        /// <summary>
        /// 赛事评论
        /// </summary>
        /// <param name="context"></param>
        void eventComments(HttpContext context,string jsonstr)
        {
            Model_Return model = new Model_Return();
            try
            {
                model.code = 0;
                model.errorMsg = "";
                Model_req_eventDetail req = new Model_req_eventDetail();
                req = JsonHelper.ParseFormJson<Model_req_eventDetail>(jsonstr);
                string id = req.id;
                string _userId = req.userId;
                Fe_Model_Comment comments = new Fe_Model_Comment();
                List<CommentModel> list= CommentBll.instance.GetComList(id, "Tour");
                comments.total = list.Count;
                List<Fe_comment> FeComlist = new List<Fe_comment>();
                foreach (CommentModel com in list)
                {
                    Fe_comment comment = new Fe_comment();
                    comment.id = com.ID;
                    comment.username = com.MEMNAME;
                    comment.userimage = com.MEMIMG;
                    comment.time = com.UPDATEDATE;
                    comment.context = com.COMMENTS;
                    comment.like = PraiseBll.instance.IsMemPraise("Comment", com.ID, _userId);
                    comment.likenumber = com.GoodQty;
                    FeComlist.Add(comment);
                }
                comments.comments = FeComlist;
                model.data = comments;
            }
            catch (Exception e)
            {
                model.code = 1;
                model.errorMsg = e.ToString();
                model.data = null;
            }
            string _Res = JsonHelper.ToJson(model);
            context.Response.Write(_Res);
        }

        /// <summary>
        /// 获取赛事消息
        /// </summary>
        /// <param name="context"></param>
        void eventNotices(HttpContext context)
        {
            Model_Return ret = new Model_Return();
            try
            {
                ret.code = 0;
                string _eventId=context.Request.Form["id"];
                List<Dictionary<string, string>> noticeList = new List<Dictionary<string, string>>();
                Dictionary<string, string> item = new Dictionary<string, string>();
                item.Add("time", "2016-08-08 22:55:59");
                item.Add("context", "由于天气高温，赛事延期举行");
                //noticeList.Add(item);
                ret.data = noticeList;
            }
            catch (Exception e)
            {
                ret.code = 1;
                ret.errorMsg = e.ToString();
                ret.data = "";
            }
            context.Response.Write(JsonHelper.ToJson(ret));
        }

        /// <summary>
        /// 请求签位，在允许抽签的时间范围内，请求一个签位
        /// </summary>
        /// <param name="context"></param>
        void eventDraw(HttpContext context,string jsonstr)
        {
            Model_Return model = new Model_Return();
            try
            {
                Model_req_eventDetail req = new Model_req_eventDetail();
                req = JsonHelper.ParseFormJson<Model_req_eventDetail>(jsonstr);
                string id = req.id;//event id
                string _userId = req.userId;//member id
                model.code = 0;
                model.errorMsg = "";
                model.data = "2";

            }
            catch (Exception e)
            {
                model.code = 1;
                model.errorMsg = e.ToString();
                model.data = "not ok";
            }
            string _Res = JsonHelper.ToJson(model);
            context.Response.Write(_Res);
        }

        /// <summary>
        /// 对评论点赞
        /// </summary>
        /// <param name="context"></param>
        void eventCommentLike(HttpContext context,string jsonstr)
        {
            Model_Return model = new Model_Return();
            try
            {
                Model_req_eventDetail req = new Model_req_eventDetail();
                req = JsonHelper.ParseFormJson<Model_req_eventDetail>(jsonstr);
                string _id = req.id;
                string _userId = req.userId;
                PraiseModel praise = new PraiseModel();
                praise.MEMSYS = _userId;
                praise.DTYPE = "Comment";
                praise.TYPESYSNO = _id;
                praise.ISGOOD = "1";
                if (PraiseBll.instance.AddPraise(praise))
                {
                    model.code = 0;
                    model.errorMsg = "";
                    model.data = "ok";
                }
                else
                {
                    model.code = 2;
                    model.errorMsg = "";
                    model.data = "不能重复点赞";
                }
            }
            catch (Exception e)
            {
                model.code = 0;
                model.errorMsg = e.ToString();
                model.data = "not ok";
            }
            string _Res = JsonHelper.ToJson(model);
            context.Response.Write(_Res);
        }

        /// <summary>
        /// 发表评论
        /// </summary>
        /// <param name="context"></param>
        void eventSendComment(HttpContext context)
        {
            context.Response.AddHeader("Access-Control-Allow-Origin", _Origin);
            Model_Return model = new Model_Return();
            try
            {
                string _eventId = context.Request.Form["eventId"];
                string _context = context.Request.Form["context"];
                string _memsys = context.Request.Form["userId"];
                if (string.IsNullOrEmpty(_memsys) || string.IsNullOrEmpty(_eventId) || string.IsNullOrEmpty(_context))
                {
                    model.code=2;
                    model.errorMsg = "信息不完整";
                    model.data = "";
                }
                else
                {
                    model.code = 0;
                    model.errorMsg = "";
                    CommentModel comm = new CommentModel();
                    comm.TYPE = "Tour";
                    comm.TYPESYSNO = _eventId;
                    comm.MEMSYSNO = _memsys;
                    comm.COMMENTS = _context;
                    CommentBll.instance.InsertComment(comm);
                    model.data = "ok";
                }
            }
            catch (Exception e)
            {
                model.code = 0;
                model.errorMsg = e.ToString();
                model.data = "not ok";
            }
            string _Res = JsonHelper.ToJson(model);
            context.Response.Write(_Res);
        }

        void eventSponsors(HttpContext context,string jsonstr)
        {
            Model_Return model = new Model_Return();
            try
            {
                Model_req_eventDetail req = new Model_req_eventDetail();
                req = JsonHelper.ParseFormJson<Model_req_eventDetail>(jsonstr);
                string _id = req.id;
                model.code = 0;
                model.errorMsg = "";
                model.data = Fe_Biz_TourList.instance.GetSponsers(_id) ;
            }
            catch (Exception e)
            {
                model.code = 1;
                model.errorMsg = e.ToString();
                model.data = "not ok";
            }
            string _Res = JsonHelper.ToJson(model);
            context.Response.Write(_Res);
        }

     
        /// <summary>
        /// 关注赛事
        /// </summary>
        /// <param name="context"></param>
        void eventFollow(HttpContext context,string jsonstr)
        {
            Model_Return model = new Model_Return();
            try
            {
                Model_req_event req = new Model_req_event();
                req = JsonHelper.ParseFormJson<Model_req_event>(jsonstr);
                PraiseModel praise=new PraiseModel();
                praise.DTYPE="Tour";
                praise.TYPESYSNO=req.eventId;
                praise.ISGOOD="1";                
                praise.MEMSYS=GetCookieUser(context);
                if (praise.MEMSYS.Length > 32)
                {
                    model.code = 0;
                    model.errorMsg = "请先登陆！";
                    model.data = "ok";
                }
                else
                {
                    if (PraiseBll.instance.AddPraise(praise))
                    {
                        model.code = 0;
                        model.errorMsg = "";
                        model.data = "ok";
                    }
                    else
                    {
                        model.code = 2;
                        model.errorMsg = "";
                        model.data = "not ok";
                    }
                }
            }
            catch (Exception e)
            {
                model.code = 1;
                model.errorMsg = e.ToString();
                model.data = "";
            }
            string _Res = JsonHelper.ToJson(model);
            context.Response.Write(_Res);
        }

        /// <summary>
        /// 获取指定项目，指定轮次的签表信息,
        /// </summary>
        /// <param name="context"></param>
        void eventDrawTable(HttpContext context,string jsonstr)
        {
            Model_Return model = new Model_Return();
            try
            {
                model.code = 0;
                model.errorMsg = "";
                Model_req_draw req = new Model_req_draw();
                req = JsonHelper.ParseFormJson<Model_req_draw>(jsonstr);
                string _itemId = req.itemId;
                string _round = req.round;
                
                //判断round是否存在
                if (string.IsNullOrEmpty(_round) || _round=="-1")
                { 
                    //未指定round，默认给出最新轮次
                    _round = WeTourContentDll.instance.GetLatestRound(_itemId);
                }
                
                //根据itemid和round获取签表信息
                Fe_Model_DrawTable drawt = Fe_Biz_TourList.instance.Getdrawtable(_itemId, Convert.ToInt32(_round));
                model.data = drawt;
            }
            catch (Exception e)
            {
                model.code = 1;
                model.errorMsg = e.ToString();
                model.data = "";
            }
            string _Res = JsonHelper.ToJson(model);
            context.Response.Write(_Res);
        }

        void cascadeFilter(HttpContext context, string jsonstr)
        {
            Model_Return ret = new Model_Return();
            try
            {
                ret.code = 0;
                Model_req_cascadeFilter req = new Model_req_cascadeFilter();
                req = JsonHelper.ParseFormJson<Model_req_cascadeFilter>(jsonstr);
                string _eventId = "";

                try
                {
                    _eventId = req.id;
                }
                catch (Exception e)
                { 
                
                }
                
                switch(req.type)
                {
                    case "event": 
                        //赛事签表过滤条件
                        ret.data = Fe_Biz_TourList.instance.GetDrawFilter(_eventId);
                        break;

                    case "eventScheduleFilter":
                        //赛程过滤条件
                        ret.data = Fe_Biz_TourList.instance.eventScheduleFilter(_eventId);
                        break;
                    case "eventScoreFilter":
                        //赛事得分过滤条件
                        ret.data = Fe_Biz_TourList.instance.GetDrawFilter(_eventId);
                        break;
                    case "rankings":
                        //排名筛选
                        ret.data = Fe_Biz_Ranking.instance.GetRankingFilters();
                        break;

                    default:
                        ret.data = Fe_Biz_TourList.instance.GetDrawFilter(_eventId);
                        break;
                }
                
            }
            catch (Exception e)
            {
                ret.code = 1;
                ret.errorMsg = e.ToString();
            }
            context.Response.Write(JsonHelper.ToJson(ret));
        }

        /// <summary>
        /// 获取赛程
        /// </summary>
        /// <param name="context"></param>
        /// <param name="jsonstr"></param>
        void eventSchedule(HttpContext context, string jsonstr)
        {
            Model_Return ret = new Model_Return();
            try
            {
                Model_req_schedule req = new Model_req_schedule();
                req = JsonHelper.ParseFormJson<Model_req_schedule>(jsonstr);                
                ret.code = 0;
                ret.errorMsg = "";
                ret.data = WeMatchDll.instance.GetEventSchedule(req.eventId,req.date,req.location);
            }
            catch (Exception e)
            {
                ret.code = 1;
                ret.errorMsg = e.ToString();
            }
            context.Response.Write(JsonHelper.ToJson(ret));
        }

        /// <summary>
        /// 赛事得分列表
        /// </summary>
        /// <param name="context"></param>
        /// <param name="jsonstr"></param>
        void eventScore(HttpContext context, string jsonstr)
        {
            Model_Return ret = new Model_Return();
            try
            {
                Model_req_result req = new Model_req_result();
                req = JsonHelper.ParseFormJson<Model_req_result>(jsonstr);
                ret.code = 0;
                ret.errorMsg = "";
                ret.data = WeMatchDll.instance.GeteventResults(req.itemId, req.status);
            }
            catch (Exception e)
            {
                ret.code = 1;
                ret.errorMsg = "";
                
            }
            context.Response.Write(JsonHelper.ToJson(ret));
        }

        void eventScoreStateFilter(HttpContext context, string jsonstr)
        {
            Model_Return ret = new Model_Return();
            try
            {
                ret.code = 0;
                ret.errorMsg = "";
                ret.data = "";
            }
            catch (Exception e)
            {
                ret.code = 1;
                ret.errorMsg = "";

            }
            context.Response.Write(JsonHelper.ToJson(ret));
        }


        void eventDrawTableFilter(HttpContext context, string jsonstr)
        {
            Model_Return ret = new Model_Return();
            try
            {
                ret.code = 0;
                Model_req_event req = new Model_req_event();
                req = JsonHelper.ParseFormJson<Model_req_event>(jsonstr);
                string _eventId = req.eventId;              
                ret.data = Fe_Biz_TourList.instance.GetDrawFilter(_eventId);                  

            }
            catch (Exception e)
            {
                ret.code = 1;
                ret.errorMsg = e.ToString();
            }
            context.Response.Write(JsonHelper.ToJson(ret));
        }

        /// <summary>
        /// 获取签表信息，淘汰赛
        /// </summary>
        /// <param name="context"></param>
        void eventDrawTableKnockOut(HttpContext context,string jsonstr)
        {
            Model_Return model = new Model_Return();
            try
            {
                Model_req_event req = new Model_req_event();
                req = JsonHelper.ParseFormJson<Model_req_event>(jsonstr);
                model.code = 0;
                model.errorMsg = "";
                string _eventId = req.eventId;
                //获取签表
                List<ContSignModel> list = WeContentSignsDll.instance.GetSignKnockOut(_eventId);
                string json = JsonHelper.ToJson(list);
                model.data = json;
            }
            catch (Exception e)
            {
                model.code = 1;
                model.errorMsg = e.ToString();
                model.data = "";
            }
            string _Res = JsonHelper.ToJson(model);
            context.Response.Write(_Res);
        }

        void eventScheduleFilter(HttpContext context,string jsonstr)
        {
            Model_Return ret = new Model_Return();
            try
            {
                ret.code = 0;
                ret.errorMsg = "";
                Model_req_eventDetail req = new Model_req_eventDetail();
                req = JsonHelper.ParseFormJson<Model_req_eventDetail>(jsonstr);
                string _id = req.id;
                //Fe_Model_eventScheduleFilter model = Fe_Biz_TourList.instance.eventScheduleFilter(_id);
                //ret.data = model;
                
            }
            catch (Exception e)
            {
                ret.code = 1;
                ret.errorMsg = e.ToString();
                ret.data = "系统错误";
            }
            context.Response.Write(JsonHelper.ToJson(ret));
        }



        /// <summary>
        /// 获取赛事结果
        /// </summary>
        /// <param name="context"></param>
        void eventMatchInfo(HttpContext context, string jsonstr)
        {
            Model_Return ret = new Model_Return();
            try
            {
                ret.code = 0;
                ret.errorMsg = "";
               
                //2016-9-29,增加比赛详情页。
                Model_req_Match req = new Model_req_Match();
                req = JsonHelper.ParseFormJson<Model_req_Match>(jsonstr);
                string sys=req.matchId;
                ret.data = Fe_Biz_TourList.instance.Fe_Match_GetInfo2(sys);
            }
            catch (Exception e)
            {
                ret.code = 1;
                ret.errorMsg = e.ToString();
                ret.data = "";
            }
            context.Response.Write(JsonHelper.ToJson(ret));
        }

        /// <summary>
        /// 获得比赛技术统计
        /// </summary>
        /// <param name="context"></param>
        /// <param name="jsonstr"></param>
        void eventMatchTechnicalStatistics(HttpContext context, string jsonstr)
        {
            Model_Return ret = new Model_Return();
            try
            {
                ret.code = 0;
                ret.errorMsg = "";
                Model_req_Match req = new Model_req_Match();
                req = JsonHelper.ParseFormJson<Model_req_Match>(jsonstr);
                ret.data = Fe_Biz_TourList.instance.GetMatchStatics(req.matchId);
            }
            catch (Exception e)
            {
                ret.code = 1;
                ret.errorMsg = e.ToString();
            }
            context.Response.Write(JsonHelper.ToJson(ret));
        }

        #endregion

        #region 报名
        void eventGroups(HttpContext context,string jsonstr)
        {
            Model_Return ret = new Model_Return();
            try
            {
                ret.code = 0;
                ret.errorMsg = "";
                Model_req_Register req = new Model_req_Register();
                req = JsonHelper.ParseFormJson<Model_req_Register>(jsonstr);
                string _Toursys = req.eventId;
                ret.data = Fe_Biz_TourList.instance.eventGroups(_Toursys);
            }
            catch (Exception e)
            {
                ret.code = 1;
                ret.errorMsg = e.ToString();
                ret.data = "";
            }
            context.Response.Write(JsonHelper.ToJson(ret));
        }

        void registeredUsers(HttpContext context,string jsonstr)
        {
            Model_Return ret = new Model_Return();
            try
            {
                ret.code = 0;
                ret.errorMsg = "";
                Model_req_Register req = new Model_req_Register();
                req = JsonHelper.ParseFormJson<Model_req_Register>(jsonstr);
                string id = req.itemId;
                ret.data = Fe_Biz_TourList.instance.registeredUsers(id);
            }
            catch (Exception e)
            {
                ret.code = 1;
                ret.errorMsg = e.ToString();
            }
            context.Response.Write(JsonHelper.ToJson(ret));          
        }

        void fetchPartners(HttpContext context,string jsonstr)
        {
            Model_Return ret = new Model_Return();
            try
            {
                ret.code = 0;
                ret.errorMsg = "";
                Model_req_Register req = new Model_req_Register();
                req = JsonHelper.ParseFormJson<Model_req_Register>(jsonstr);
                string userid=req.userid;
                ret.data = Fe_Biz_Me.instance.GetDoubleParterner(userid);
            }
            catch (Exception e)
            {
                ret.code = 1;
                ret.errorMsg = e.ToString();
                ret.data = "";
            }
            context.Response.Write(JsonHelper.ToJson(ret));
        }

        /// <summary>
        /// 提交报名信息
        /// </summary>
        /// <param name="context"></param>
        void registerEvent(HttpContext context,string jsonstr)
        {
            Model_Return ret = new Model_Return();
            try
            {
                Model_req_Register req = new Model_req_Register();
                req = JsonHelper.ParseFormJson<Model_req_Register>(jsonstr);
                
                string _ItemId = req.itemId;
                string _UserId=GetCookieUser(context);
                string _Gender = req.gender;
                string _Name = req.name;
                string _Phone = req.phone;
                string _PersonCard = req.personCard;
                string _parterner = req.partnerId;
                        
                //添加报名信息
                WeTourContModel cont=WeTourContentDll.instance.GetModelbyId(_ItemId);
                WeTourApplyModel apply = new WeTourApplyModel();
                apply.STATUS = "1";
                apply.MEMBERID = _UserId;
                if (cont.ContentType.IndexOf("双") > 0)
                {
                    apply.PATERNER = _parterner;
                }
                apply.CONTENTID = _ItemId;
                apply.TOURSYS = cont.Toursys;

                 //验证报名有效性
                string validatemsg=WeTourApplyDll.instance.ValidateContApply(apply, 1);
                if (validatemsg == "ok")
                {
                    //添加报名费用订单
                    string ordersys = WeTourApplyDll.instance.AddApplyOrder(cont.Toursys, _ItemId, _UserId, apply.PATERNER);

                    Dictionary<String, String> pList = new Dictionary<String, String>();
                    pList.Add("payUrl", "http://wetennis.cn/WeiPayWeb/wechatpay.html?orderNum=" + ordersys);
                    ret.data = pList;

                    apply.EXT2 = ordersys;

                    if (WeTourApplyDll.instance.InsertNewApply(apply))
                    {
                        ret.code = 0;
                        ret.errorMsg = "";
                    }
                    else
                    {
                        ret.code = 2;
                        ret.errorMsg = "添加报名不成功";
                    }
                }
                else
                {
                    ret.code = 2;
                    ret.errorMsg = validatemsg;
                    Dictionary<String, String> pList = new Dictionary<String, String>();
                    pList.Add("payUrl", "");
                    ret.data = pList;
                }

                //修改我的信息
                WeMemberModel mem = WeMemberDll.instance.GetModel(_UserId);
                if (_Gender == "male")
                {
                    mem.GENDER = "男";
                }
                else
                {
                    mem.GENDER = "女";
                }
                mem.NAME = _Name;
                mem.TELEPHONE = _Phone;
                mem.EXT2 = _PersonCard;
                mem.EXT4 = req.club;
                mem.EXT5 = req.company;
                mem.EXT6 = req.title;
                WeMemberDll.instance.UpdateMemberInfo(mem);
                
            }
            catch (Exception e)
            {
                ret.code = 1;
                ret.errorMsg = e.ToString();
                ret.data = "";
            }
            LogHelper.instance.WriteLog("req", "registerEvent||"+JsonHelper.ToJson(ret));
            context.Response.Write(JsonHelper.ToJson(ret));
        }

        /// <summary>
        /// 团体赛报名
        /// </summary>
        /// <param name="context"></param>
        /// <param name="jsonstr"></param>
        void registerTeam(HttpContext context, string jsonstr)
        {
            Model_Return ret = new Model_Return();
            try
            {
                //根据json获取信息
                Fe_Model_GroupApplicants req = JsonHelper.ParseFormJson<Fe_Model_GroupApplicants>(jsonstr);
                string _UserId = GetCookieUser(context);
                //string _UserId = "city02";
                if (req.members.Count < 1)
                {
                    ret.code = 1;
                    ret.errorMsg = "团队成员不能为空";
                    ret.data = "团队成员不能为空";
                }
                else
                {
                    if (req.groupId != "" && req.groupId != null)
                    {
                        //添加支付信息
                        WeTourContModel cont = WeTourContentDll.instance.GetModelbyId(req.groupId);
                        string ordersys = WeTourApplyDll.instance.AddApplyOrder(cont.Toursys, cont.id, _UserId, req.name);
                        Dictionary<String, String> pList = new Dictionary<String, String>();
                        pList.Add("payUrl", "http://wetennis.cn/WeiPayWeb/wechatpay.html?orderNum=" + ordersys);
                        ret.data = pList;

                        //添加group
                        string groupsys = weTeamdll.instance.AddGroups(req);

                        pList.Add("teamId",groupsys);
                        //添加报名信息
                        WeTourApplyModel apply = new WeTourApplyModel();
                        apply.STATUS = "1";
                        apply.memtype = "group";
                        apply.MEMBERID = groupsys;
                        apply.CONTENTID = req.groupId;
                        apply.TOURSYS = cont.Toursys;
                        apply.PATERNER = _UserId;
                        apply.EXT2 = ordersys;
                        //返回支付url
                        if (WeTourApplyDll.instance.InsertNewApply(apply))
                        {
                            ret.code = 0;
                            ret.errorMsg = "";
                        }
                        else
                        {
                            ret.code = 2;
                            ret.errorMsg = "添加报名不成功";
                        }
                    }
                }

            }
            catch (Exception e)
            {
                ret.code = 2;
                ret.errorMsg = "系统错误";
            }
            context.Response.Write(JsonHelper.ToJson(ret));
        }

        /// <summary>
        /// 根据groupdId获取报名的人员信息
        /// </summary>
        /// <param name="context"></param>
        /// <param name="jsonstr"></param>
        void fetchRegisteredTeams(HttpContext context, string jsonstr)
        {
            Model_Return ret = new Model_Return();
            try
            {
                Fe_Model_GroupApplicants req = JsonHelper.ParseFormJson<Fe_Model_GroupApplicants>(jsonstr);
                List<Fe_Model_GroupInfo> TeamList = new List<Fe_Model_GroupInfo>();
                List<WeTourContModel> contList = WeTourContentDll.instance.GetContentlist(req.eventId);
                foreach (WeTourContModel cont in contList)
                {
                    string contName = cont.ContentName;
                    List<WeTourApplyModel> applist = WeTourApplyDll.instance.GetApplyListbyCond(" and contentId='" + cont.id + "'");
                    foreach (WeTourApplyModel app in applist)
                    {
                        Fe_Model_GroupInfo team = new Fe_Model_GroupInfo();
                        weTeamModel tm = weTeamdll.instance.GetModel(app.MEMBERID);
                        team.name = tm.TEAMNAME;
                        team.coachName = tm.HEADMEMBER;
                        team.registerDate = app.APPLYDATE;
                        team.groupName = contName;

                        TeamList.Add(team);
                    }
                }
                ret.code = 0;
                ret.errorMsg = "";
                ret.data = TeamList;
               
            }
            catch (Exception e)
            {
                ret.code = 1;
                ret.errorMsg = e.ToString();
            }
            context.Response.Write(JsonHelper.ToJson(ret));
        }

        /// <summary>
        /// 根据赛事Id，查询已报名的团队成员信息
        /// </summary>
        /// <param name="context"></param>
        /// <param name="jsonstr"></param>
        void fetchRegisteredTeamMembers(HttpContext context, string jsonstr)
        { 
             Model_Return ret = new Model_Return();
             try
             {
                 string _UserId = GetCookieUser(context);//获取当前登陆人员
                 _UserId="city01";
                 Fe_Model_GroupApplicants req = JsonHelper.ParseFormJson<Fe_Model_GroupApplicants>(jsonstr);
                 string EventId = req.eventId;

                 //根据eventId和当前登陆人id，获取groupSys
                 List<WeTourApplyModel> app_list = WeTourApplyDll.instance.GetApplyListbyCond(" and toursys='" + EventId + "' and paterner='" + _UserId + "'");
                 if (app_list.Count > 0)
                 {
                     ret.data = weTeamMemberdll.instance.GetMemberList(app_list[0].MEMBERID);
                     ret.code = 0;
                 }
                 else
                 {
                     ret.code = 1;
                     ret.errorMsg = "未找到团体报名信息";
                 }
             }
             catch (Exception e)
             {
                 ret.code = 1;
                 ret.errorMsg = e.ToString() ;
             }
             context.Response.Write(JsonHelper.ToJson(ret));
        }

        /// <summary>
        /// 根据团体比赛主键，获取队员出场顺序
        /// </summary>
        /// <param name="context"></param>
        /// <param name="jsonstr"></param>
        void fetchRegisteredTeamSequence(HttpContext context, string jsonstr)
        { 
             Model_Return ret = new Model_Return();
             try
             {
                 string _UserId = GetCookieUser(context);//获取当前登陆人员
                 _UserId = "city01";

                 string matchId = "";
                 Fe_Model_MatchTeamSchedule req = JsonHelper.ParseFormJson<Fe_Model_MatchTeamSchedule>(jsonstr);
                 matchId = req.matchId;
                 WeMatchModel match=WeMatchDll.instance.GetModel(matchId);

                 string TeamSYs = Biz_match_sequence.instance.GetTeamSys(match.TOURSYS,_UserId);
                 //根据matchId和teamSys获取参赛顺序
                 ret.data = Biz_match_sequence.instance.fetchMatchTeamSeq(matchId, TeamSYs);
                 ret.code = 0;
             }
             catch (Exception e)
             {
                 ret.code = 1;
                 ret.errorMsg = e.ToString();
             }

             context.Response.Write(JsonHelper.ToJson(ret));
        }

        /// <summary>
        /// 修改队员出场顺序
        /// </summary>
        /// <param name="context"></param>
        /// <param name="jsonstr"></param>
        void updateRegisteredTeamSequence(HttpContext context, string jsonstr)
        {
            Model_Return ret = new Model_Return();
            try
            {
                Fe_Model_MatchTeamSchedule req = JsonHelper.ParseFormJson<Fe_Model_MatchTeamSchedule>(jsonstr);

                Biz_match_sequence.instance.UpdateTeamSchedule(req);
                ret.code = 0;
                ret.data = "success";
            }
            catch (Exception e)
            {
                ret.code = 1;
                ret.errorMsg = e.ToString();
            }
            context.Response.Write(JsonHelper.ToJson(ret));
        }

        #endregion

        #region 我
        void fetchMyData(HttpContext context,string jsonstr)
        {
            Model_Return model = new Model_Return();
            try
            {
                Model_req_me req = new Model_req_me();
                req = JsonHelper.ParseFormJson<Model_req_me>(jsonstr);

                //string _userId=req.userId;
                string _userId = GetCookieUser(context);
                model.code = 0;
                model.errorMsg = "";
                //
                Fe_Model_Me me = Fe_Biz_Me.instance.fetchMyData(_userId);
                //添加比赛数
                //添加装备数
                List<Fe_Model_equipment> equips = new List<Fe_Model_equipment>();
                //List<WareMainModel> wares = MyWaresBiz.instance.GetMyGearsbyType("球拍", _userId);
                List<MyWaresModel> wares = MyWaresBiz.instance.getMywareList(_userId);
                foreach (MyWaresModel ware in wares)
                {
                    Fe_Model_equipment equip = new Fe_Model_equipment();
                    equip.id = ware.id;
                    equip.imgUrl = ware.CustPro1;
                    equip.logo = ware.CustPro2;
                    try
                    {
                        equip.size = Convert.ToDecimal(ware.CustPro3);
                        equip.price = Convert.ToDecimal(ware.CustPro4);
                    }
                    catch (Exception)
                    {
                        
                    }                   
                    equips.Add(equip);
                }
                me.equipment = equips;

                model.data = me;
            }
            catch (Exception e)
            {
                model.code = 0;
                model.errorMsg = e.ToString();
                model.data = "";
            }
            string _Res = JsonHelper.ToJson(model);
            context.Response.Write(_Res);
        }

        /// <summary>
        /// 更新个人信息
        /// </summary>
        /// <param name="context"></param>
        void updateMySettings(HttpContext context, string jsonstr)
        {
            Model_Return model = new Model_Return();
            try
            {
                //ajax 传递参数格式为json，后端解析json数据
                Fe_Model_updateMySettings set = new Fe_Model_updateMySettings() ;                
                             
                set = JsonHelper.ParseFormJson<Fe_Model_updateMySettings>(jsonstr);
                Dictionary<string, bool> item = new Dictionary<string, bool>();
                if (Fe_Biz_Me.instance.UpdateMeSetting(set))
                {
                    model.code = 0;
                    model.errorMsg = "";
                    item.Add("result", true);
                    model.data = item;
                }
                else
                {
                    model.code = 2;
                    model.errorMsg = "修改失败";
                    item.Add("result", false);
                    model.data = item;
                }
            }
            catch (Exception e)
            {
                model.code = 1;
                model.errorMsg = e.ToString();
                model.data = "";                
            }
            context.Response.Write(JsonHelper.ToJson(model));
        }

        void fetchMySettings(HttpContext context,string jsonstr)
        {
            Model_Return ret = new Model_Return();
            try
            {
                Model_req_me req = new Model_req_me();
                req = JsonHelper.ParseFormJson<Model_req_me>(jsonstr);
                string _userId = req.userId;
                ret.data = Fe_Biz_Me.instance.fetchMySetting(_userId);
                ret.code = 0;
                ret.errorMsg = "获取成功";
            }
            catch (Exception e)
            {
                ret.code = 1;
                ret.errorMsg = e.ToString();
                ret.data = "获取失败";
            }
            context.Response.Write(JsonHelper.ToJson(ret));
        }

        /// <summary>
        /// 拉取我的比赛信息
        /// </summary>
        /// <param name="context"></param>
        /// <param name="jsonstr"></param>
        void fetchMyMatch(HttpContext context,string jsonstr)
        {
            Model_Return ret = new Model_Return();
            try
            {
                Model_req_me req = new Model_req_me();
                req = JsonHelper.ParseFormJson<Model_req_me>(jsonstr);
                string _userId=req.userId;
                ret.data = Fe_Biz_TourList.instance.GetMyMatch(_userId);
                ret.code = 0;
                ret.errorMsg = "";
            }
            catch (Exception e)
            {
                ret.code = 1;
                ret.errorMsg = e.ToString();
                ret.data = "";
            }
            context.Response.Write(JsonHelper.ToJson(ret));
        }

        /// <summary>
        /// 拉取我的约球信息
        /// </summary>
        /// <param name="context"></param>
        /// <param name="jsonstr"></param>
        void fetchMyPractice(HttpContext context, string jsonstr)
        {
            Model_Return model = new Model_Return();
            try
            {
                Model_req_me req = new Model_req_me();
                req = JsonHelper.ParseFormJson<Model_req_me>(jsonstr);
                model.data = Fe_Biz_TourList.instance.GetMyPractice(req.userId);
                model.code = 0;
                model.errorMsg = "";
            }
            catch (Exception e)
            {
                model.code = 1;
                model.errorMsg = e.ToString();
                model.data = "系统错误，请联系管理员";
            }
            string _Res = JsonHelper.ToJson(model);
            context.Response.Write(_Res);
        }

        /// <summary>
        /// 修改背景图
        /// </summary>
        /// <param name="context"></param>
        /// <param name="jsonstr"></param>
        void updateBGImage(HttpContext context, string jsonstr)
        {
            Model_Return model = new Model_Return();
            try
            {
                Model_req_me req = new Model_req_me();
                req = JsonHelper.ParseFormJson<Model_req_me>(jsonstr);
                Dictionary<string, string> item = new Dictionary<string, string>();
                if (WeMemberDll.instance.updateBGimage(req.userId, req.ImageUrl))
                {
                    item.Add("result", "success");
                }
                else
                {
                    item.Add("result","fail");
                }
                model.data = item;
                model.code = 0;
                model.errorMsg = "";
            }
            catch (Exception e)
            {
                model.code = 1;
                model.errorMsg = e.ToString();
                model.data = "系统错误，请联系管理员";
            }
            string _Res = JsonHelper.ToJson(model);
            context.Response.Write(_Res);
        }

        /// <summary>
        /// 根据用户id获取我的装备
        /// </summary>
        /// <param name="_userId"></param>
        /// <returns></returns>
        private List<Fe_Model_equipment> GetmyWares(string _userId)
        {
            List<Fe_Model_equipment> equips = new List<Fe_Model_equipment>();
            //List<WareMainModel> wares = MyWaresBiz.instance.GetMyGearsbyType("球拍", _userId);
            List<MyWaresModel> wares = MyWaresBiz.instance.getMywareList(_userId);
            foreach (MyWaresModel ware in wares)
            {
                Fe_Model_equipment equip = new Fe_Model_equipment();
                equip.id = ware.id;
                equip.imgUrl = ware.CustPro1;
                equip.logo = ware.CustPro2;
                try
                {
                    equip.size = Convert.ToDecimal(ware.CustPro3);
                }
                catch (Exception)
                {
                    equip.size = 0;
                }
                try
                {
                    equip.price = Convert.ToDecimal(ware.CustPro4);
                }
                catch {
                    equip.price = 0;
                }
                
                equips.Add(equip);
            }
            return equips;
        }

        /// <summary>
        /// 添加装备
        /// </summary>
        /// <param name="context"></param>
        /// <param name="jsonstr"></param>
        void addEquipment(HttpContext context, string jsonstr)
        {
            Model_Return ret = new Model_Return();
            try
            {
                ret.code = 0;
                ret.errorMsg = "";
                Model_req_Equipment equip = JsonHelper.ParseFormJson<Model_req_Equipment>(jsonstr);
                MyWaresModel myware = new MyWaresModel();
                myware.Memsysno = GetCookieUser(context);//从cookie获取用户id
                myware.CustPro1 = equip.imgUrl;
                myware.CustPro2 = equip.logo;
                myware.CustPro3 = equip.size;
                myware.CustPro4 = equip.price;
                if (MyWaresBiz.instance.InsertNew(myware))
                {
                    //返回所有equipment
                    ret.data = GetmyWares(myware.Memsysno);
                }
                else
                { 
                    //添加不成功
                    ret.code = 2;
                    ret.errorMsg = "添加不成功！";
                }

            }
            catch (Exception e)
            {
                ret.code = 1;
                ret.errorMsg = e.ToString();                
            }
            context.Response.Write(JsonHelper.ToJson(ret));
        }

        /// <summary>
        /// 编辑装备
        /// </summary>
        /// <param name="context"></param>
        /// <param name="jsonstr"></param>
        void editEquipment(HttpContext context, string jsonstr)
        {
            Model_Return ret = new Model_Return();
            try
            {
                ret.code = 0;
                ret.errorMsg = "";

                Model_req_Equipment equip = JsonHelper.ParseFormJson<Model_req_Equipment>(jsonstr);
                MyWaresModel myware = new MyWaresModel();
                myware.Memsysno = GetCookieUser(context);//从cookie获取用户id
                myware.id = equip.id;
                myware.CustPro1 = equip.imgUrl;
                myware.CustPro2 = equip.logo;
                myware.CustPro3 = equip.size;
                myware.CustPro4 = equip.price;
                if (MyWaresBiz.instance.updateMyware(myware))
                {
                    //返回所有equipment
                    ret.data = GetmyWares(myware.Memsysno);
                }
                else
                {
                    //添加不成功
                    ret.code = 2;
                    ret.errorMsg = "修改不成功！";
                }
            }
            catch (Exception e)
            {
                ret.code = 1;
                ret.errorMsg = e.ToString();
            }
            context.Response.Write(JsonHelper.ToJson(ret));
        }

        /// <summary>
        /// 删除装备
        /// </summary>
        /// <param name="context"></param>
        /// <param name="jsonstr"></param>
        void deleteEquipment(HttpContext context, string jsonstr)
        {
            Model_Return ret = new Model_Return();
            try
            {
                ret.code = 0;
                ret.errorMsg ="";
                Model_req_Equipment equip = JsonHelper.ParseFormJson<Model_req_Equipment>(jsonstr);
                MyWaresModel myware = new MyWaresModel();
                myware.Memsysno = GetCookieUser(context);//从cookie获取用户id
                myware.id = equip.id;
                if (MyWaresBiz.instance.DeleteMyWare(equip.id))
                {
                    //返回所有equipment
                    ret.data = GetmyWares(myware.Memsysno);
                }
                else
                {
                    //添加不成功
                    ret.code = 2;
                    ret.errorMsg = "修改不成功！";
                }
            }
            catch (Exception e)
            {
                ret.code = 1;
                ret.errorMsg = e.ToString();
            }
            context.Response.Write(JsonHelper.ToJson(ret));
        }
        #endregion

        #region 排名，2016-08-08
        void rankingTypeFilter(HttpContext context)
        {
            Model_Return ret = new Model_Return();
            try
            {
                ret.code = 0;
                ret.errorMsg = "";
                ret.data = Fe_Biz_Ranking.instance.GetRankTypes();
            }
            catch
            {
                ret.code = 1;
                ret.errorMsg = "系统错误，请联系管理员";
                ret.data = "";
            }
            context.Response.Write(JsonHelper.ToJson(ret));
        }

        /// <summary>
        /// 获取排名筛选条件
        /// </summary>
        /// <param name="context"></param>
        void rankingsFilter(HttpContext context)
        {
            Model_Return ret = new Model_Return();
            try
            {
                ret.code = 0;
                ret.errorMsg = "";
                ret.data = Fe_Biz_Ranking.instance.GetRankingFilters();
            }
            catch
            {
                ret.code = 1;
                ret.errorMsg = "系统错误，请联系管理员";
                ret.data = "";
            }
            context.Response.Write(JsonHelper.ToJson(ret));
        }

        /// <summary>
        /// 获取排名明细
        /// </summary>
        /// <param name="context"></param>
        void rankings(HttpContext context,string jsonstr)
        {
            Model_Return ret = new Model_Return();
            try
            {
                Model_req_ranking req=new Model_req_ranking();
                req = JsonHelper.ParseFormJson<Model_req_ranking>(jsonstr);
                //获取参数
                string typesys = req.rankId;
                string rankvalue = req.value;
                string _currentPage = req.currentPage;
                string _limit = req.limit;
                ret.code = 0;
                ret.errorMsg = "";
                ret.data = Fe_Biz_Ranking.instance.GetRankingDetail(typesys,rankvalue,_currentPage,_limit);
            }
            catch(Exception e)
            {
                ret.code = 1;
                ret.errorMsg = "系统错误，请联系管理员";
                ret.data = "";
            }
            context.Response.Write(JsonHelper.ToJson(ret));
        }

        /// <summary>
        /// 获取排名详情信息
        /// </summary>
        /// <param name="context"></param>
        /// <param name="jsonstr"></param>
        void rankingDetailsInfo(HttpContext context, string jsonstr)
        {
            Model_Return ret = new Model_Return();
            try
            {
                ret.code = 0;
                ret.errorMsg = "";
                Model_req_ranking req = JsonHelper.ParseFormJson<Model_req_ranking>(jsonstr);
                string _userId = req.userId;
                string _rankId = req.rankId;
                string _currentUser = GetCookieUser(context);
                ret.data = Fe_Biz_Ranking.instance.getRankDetails(_userId, _rankId, _currentUser);
            }
            catch (Exception e)
            {
                ret.code = 1;
                ret.errorMsg = e.ToString();                
            }
            context.Response.Write(JsonHelper.ToJson(ret));
        }
        #endregion

        #region 时光
        void uploadImage(HttpContext context,string jsonstr)
        {
            Model_Return model = new Model_Return();
            try
            {
                model.code = 0;
                model.errorMsg = "";
                Model_req_Image req = new Model_req_Image();
                req = JsonHelper.ParseFormJson<Model_req_Image>(jsonstr);
                string Imgstr = req.imgstr;
                
                int start = Imgstr.IndexOf("base64,");	
	            if(start>=0)
                {
                    Imgstr = Imgstr.Substring(start+7);
                }

                string FileName = Guid.NewGuid().ToString().ToUpper();
                string fileName = FileName + ".png";
                string SaveUrl = @"D:\\Resource\images\upload\" + fileName;
                string ImgUrl = "http://wetennis.cn:86/upload/" + fileName;
                
                System.IO.FileStream fs = new System.IO.FileStream(SaveUrl, System.IO.FileMode.Create);
                System.IO.BinaryWriter bw = new System.IO.BinaryWriter(fs);
                if (!string.IsNullOrEmpty(Imgstr) && File.Exists(SaveUrl))
                {
                    Imgstr = Imgstr.Replace(" ", "+");
                    byte[] data = Convert.FromBase64String(Imgstr);//将base64 转换为byte数组
                    bw.Write(data);//将二进制存储为图片
                }
                Dictionary<string, string> item = new Dictionary<string, string>();
                item.Add("imageUrl", ImgUrl);
                model.data = item;
            }
            catch (Exception e)
            {
                model.code = 1;
                model.errorMsg = e.ToString();
            }
            context.Response.Write(JsonHelper.ToJson(model));
        }
        #endregion

        #region 评论相关的接口

        /// <summary>
        /// 赛事评论
        /// </summary>
        /// <param name="context"></param>
        void fetchComments(HttpContext context, string jsonstr)
        {
            Model_Return model = new Model_Return();
            try
            {
                model.code = 0;
                model.errorMsg = "";
                Model_req_comment req = new Model_req_comment();
                req = JsonHelper.ParseFormJson<Model_req_comment>(jsonstr);
                string id = req.id;
                string _userId = req.userId;
                string _type = req.type;
                
                Fe_Model_Comment comments = new Fe_Model_Comment();
                List<CommentModel> list = CommentBll.instance.GetComList(id, _type);
                comments.total = list.Count;
                List<Fe_comment> FeComlist = new List<Fe_comment>();
                foreach (CommentModel com in list)
                {
                    Fe_comment comment = new Fe_comment();
                    comment.id = com.ID;
                    comment.username = com.MEMNAME;
                    comment.userimage = com.MEMIMG;
                    comment.time = com.UPDATEDATE;
                    comment.context = com.COMMENTS;
                    comment.like = PraiseBll.instance.IsMemPraise("Comment", com.ID, _userId);
                    comment.likenumber = com.GoodQty;
                    FeComlist.Add(comment);
                }
                comments.comments = FeComlist;
                model.data = comments;
            }
            catch (Exception e)
            {
                model.code = 1;
                model.errorMsg = e.ToString();
                model.data = null;
            }
            string _Res = JsonHelper.ToJson(model);
            LogHelper.instance.WriteLog("req", _Res);
            context.Response.Write(_Res);
        }

        /// <summary>
        /// 发表评论
        /// </summary>
        /// <param name="context"></param>
        void sendComment(HttpContext context,string jsonstr)
        {
            Model_Return model = new Model_Return();
            try
            {
                Model_req_comment req = new Model_req_comment();
                req = JsonHelper.ParseFormJson<Model_req_comment>(jsonstr);

                string _eventId = req.id;
                string _context = req.content;
                string _memsys = req.userId;
                string _type = req.type;
                if (string.IsNullOrEmpty(_memsys) || string.IsNullOrEmpty(_eventId) || string.IsNullOrEmpty(_context))
                {
                    model.code = 2;
                    model.errorMsg = "信息不完整";
                    model.data = "";
                }
                else
                {
                    model.code = 0;
                    model.errorMsg = "";
                    CommentModel comm = new CommentModel();
                    comm.TYPE = _type;
                    comm.TYPESYSNO = _eventId;
                    comm.MEMSYSNO = _memsys;
                    comm.COMMENTS = _context;
                    CommentBll.instance.InsertComment(comm);
                    model.data = "ok";
                }
            }
            catch (Exception e)
            {
                model.code = 0;
                model.errorMsg = e.ToString();
                model.data = "not ok";
            }
            string _Res = JsonHelper.ToJson(model);
            context.Response.Write(_Res);
        }

        /// <summary>
        /// 对评论点赞
        /// </summary>
        /// <param name="context"></param>
        void likeComment(HttpContext context, string jsonstr)
        {
            Model_Return model = new Model_Return();
            try
            {
                Model_req_comment req = new Model_req_comment();
                req = JsonHelper.ParseFormJson<Model_req_comment>(jsonstr);
                string _id = req.id;
                string _userId = req.userId;
                PraiseModel praise = new PraiseModel();
                praise.MEMSYS = _userId;
                praise.DTYPE = "Comment";
                praise.TYPESYSNO = _id;
                praise.ISGOOD = "1";
                if (PraiseBll.instance.AddPraise(praise))
                {
                    model.code = 0;
                    model.errorMsg = "";
                    model.data = "ok";
                }
                else
                {
                    model.code = 2;
                    model.errorMsg = "";
                    model.data = "不能重复点赞";
                }
            }
            catch (Exception e)
            {
                model.code = 0;
                model.errorMsg = e.ToString();
                model.data = "not ok";
            }
            string _Res = JsonHelper.ToJson(model);
            context.Response.Write(_Res);
        }

        #endregion

        #region 赛程调整
        /// <summary>
        /// 获取赛程调整信息
        /// </summary>
        /// <param name="context"></param>
        void fetchProgram(HttpContext context,string jsonstr)
        {
            Model_Return ret = new Model_Return();
            try
            {
                ret.code = 0;
                ret.errorMsg = "";
                Model_req_schedule req = new Model_req_schedule();
                req = JsonHelper.ParseFormJson<Model_req_schedule>(jsonstr);
                ret.data = Biz_ScheduleAdjust.instance.Get_Scheduel_TourDate(req.eventId, req.date);
            }
            catch (Exception e)
            {
                ret.code = 1;
                ret.errorMsg = e.ToString();
            }
            context.Response.Write(JsonHelper.ToJson(ret));
        }

        /// <summary>
        /// 更新赛程信息
        /// </summary>
        /// <param name="context"></param>
        void updateProgram(HttpContext context,string jsonstr)
        {
            Model_Return ret = new Model_Return();
            try
            {
                ret.code = 0;
                ret.errorMsg = "";
                //解析json字符串
                Model_req_adjsutSchedule req = new Model_req_adjsutSchedule();
                req = JsonHelper.ParseFormJson<Model_req_adjsutSchedule>(jsonstr);
                Biz_ScheduleAdjust.instance.AdjustProgram(req);
            }
            catch (Exception e)
            {
                ret.code = 1;
                ret.errorMsg = e.ToString();
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