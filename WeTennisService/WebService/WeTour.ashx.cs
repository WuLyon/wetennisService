using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using WeTour;
using OrderLib;
using Ranking;

namespace WeTennisService.WebService
{
    /// <summary>
    /// WeTour 的摘要说明
    /// </summary>
    public class WeTour : IHttpHandler
    {

        public void ProcessRequest(HttpContext context)
        {
            context.Response.ContentType = "text/plain";
            string typename = context.Request.QueryString["typename"].ToString();
            switch (typename)
            {
                #region 赛事基础
                case "GetTourInfo":
                    GetTourInfo(context);
                    break;
                case "GetContents":
                    GetContents(context);
                    break;

                case "GetContentModel":
                    GetContentModel(context);
                    break;

                case "GetTourList":
                    GetTourList(context);
                    break;
                //获得赛事城市
                case "GetTourCitys":
                    GetTourCitys(context);
                    break;
                case "GetTourListbyCity":
                    GetTourListbyCity(context);
                    break;
                #endregion
                #region 报名相关
                case "ApplyTourCont":
                    ApplyTourCont(context);
                    break;
                case "GetContentsForApply":
                    GetContentsForApply(context);
                    break;

                case "WriteTestMessage":
                    WriteTestMessage(context);
                    break;

                case "ConfirmMemIdentity":
                    ConfirmMemIdentity(context);
                    break;

                case "GetApplicants":
                    GetApplicants(context);
                    break;
                #endregion

                #region 处理签表
                case "GetTourContentSeed":
                    GetTourContentSeed(context);
                    break;
                #endregion

                #region 赛事主页
                case "GetContentMatches":
                    GetContentMatches(context);
                    break;
                case "GetContentRounds":
                    GetContentRounds(context);
                    break;

                case "GetContentSigns":
                    GetContentSigns(context);
                    break;

                case "GetTourSchedule":
                    GetTourSchedule(context);
                    break;

                case "GetTourResult":
                    GetTourResult(context);
                    break; 

                #endregion

                #region 获得比赛信息
                case "GetMatchInfo":
                    GetMatchInfo(context);
                    break;

                    //根据比赛主键获得赛事信息
                case "GetTourInfobyMatch":
                    GetTourInfobyMatch(context);
                    break;
                #endregion

                #region 分配赛事资源
                case "DistributeResource":
                    DistributeResource(context);
                    break;
                #endregion
            }
        }

        #region 赛事基础
        /// <summary>
        /// 获得赛事基础信息
        /// </summary>
        /// <param name="context"></param>
        void GetTourInfo(HttpContext context)
        {
            //处理回调函数
            context.Response.ContentType = "application/json;charset=utf-8";
            string jsonCallBackFunName = context.Request.Params["jsoncallback"].ToString();
            //业务处理
            string _Toursys = context.Request.Params["toursys"].ToString();
            WriteLog("GetTourInfo", _Toursys);
            WeTourModel model = WeTourDll.instance.GetModelbySys(_Toursys);
            string tourinfo = JsonHelper.ToJson(model);
            //处理查询结果
            context.Response.Write(string.Format("{0}({1})", jsonCallBackFunName, tourinfo));
            context.Response.End();
        }

        /// <summary>
        /// get Tour contents
        /// </summary>
        /// <param name="context"></param>
        void GetContents(HttpContext context)
        {
            //处理回调函数
            context.Response.ContentType = "application/json;charset=utf-8";
            string jsonCallBackFunName = context.Request.Params["jsoncallback"].ToString();
            //业务处理
            string _Toursys = context.Request.Params["toursys"].ToString();
            WriteLog("GetContents", _Toursys);
            List<WeTourContModel> list = WeTourContentDll.instance.GetContentlist(_Toursys);
            string contents = "";
            if (list.Count > 0)
            {
                contents = JsonHelper.ToJson(list);
            }

            //处理查询结果
            context.Response.Write(string.Format("{0}({1})", jsonCallBackFunName, contents));
            context.Response.End();
        }

        /// <summary>
        /// 获得子项内容
        /// </summary>
        /// <param name="context"></param>
        void GetContentModel(HttpContext context)
        {
            //处理回调函数
            context.Response.ContentType = "application/json;charset=utf-8";
            string jsonCallBackFunName = context.Request.Params["jsoncallback"].ToString();

            //业务处理
            string _ContentId = context.Request.Params["contid"].ToString();
            WriteLog("GetContentModel", _ContentId);
            WeTourContModel model = WeTourContentDll.instance.GetModelbyId(_ContentId);
            string json = JsonHelper.ToJson(model);

            //处理查询结果
            context.Response.Write(string.Format("{0}({1})", jsonCallBackFunName, json));
            context.Response.End();
        }

        /// <summary>
        /// 获得赛事列表
        /// </summary>
        /// <param name="context"></param>
        void GetTourList(HttpContext context)
        {
            //处理回调函数
            context.Response.ContentType = "application/json;charset=utf-8";
            string jsonCallBackFunName = context.Request.Params["jsoncallback"].ToString();

            WriteLog("GetTourList", "");
            List<WeTourModel> list = WeTourDll.instance.GetWeTennisTour();
            string json = JsonHelper.ToJson(list);

            //处理查询结果
            context.Response.Write(string.Format("{0}({1})", jsonCallBackFunName, json));
            context.Response.End();
        }

        /// <summary>
        /// 获得城市
        /// </summary>
        /// <param name="context"></param>
        void GetTourCitys(HttpContext context)
        {
            //处理回调函数
            context.Response.ContentType = "application/json;charset=utf-8";
            string jsonCallBackFunName = context.Request.Params["jsoncallback"].ToString();

            WriteLog("GetTourCitys", "");
            List<CityInfo> list = WeTourDll.instance.GetTourCities();
            string json = JsonHelper.ToJson(list);

            //处理查询结果
            context.Response.Write(string.Format("{0}({1})", jsonCallBackFunName, json));
            context.Response.End();
        }

        /// <summary>
        /// 根据城市id，获得该城市举办的赛事信息
        /// </summary>
        /// <param name="context"></param>
        void GetTourListbyCity(HttpContext context)
        {
            //处理回调函数
            context.Response.ContentType = "application/json;charset=utf-8";
            string jsonCallBackFunName = context.Request.Params["jsoncallback"].ToString();

            string cityId = context.Request.Params["cityid"].ToString();
            string PageSize = context.Request.Params["pagesize"].ToString();
            string Page = context.Request.Params["page"].ToString();
            WriteLog("GetTourListbyCity", cityId + "-" + PageSize + "-" + Page);
            List<WeTourModel> list = WeTourDll.instance.GetWeTennisTourPage(cityId, PageSize, Page);
            string json = JsonHelper.ToJson(list);

            //处理查询结果
            context.Response.Write(string.Format("{0}({1})", jsonCallBackFunName, json));
            context.Response.End();
        }

        #endregion

        #region 报名页面

        /// <summary>
        /// get Tour contents for apply page
        /// </summary>
        /// <param name="context"></param>
        void GetContentsForApply(HttpContext context)
        {
            //处理回调函数
            context.Response.ContentType = "application/json;charset=utf-8";
            string jsonCallBackFunName = context.Request.Params["jsoncallback"].ToString();
            //业务处理
            string _Toursys = context.Request.Params["toursys"].ToString();
            string _Memsys = context.Request.Params["memsys"].ToString();
            //莫名其妙出现_memsys的值为两个重复的员工号
            if (_Memsys.IndexOf(",") > 0)
            {
                _Memsys = _Memsys.Split(',')[0];
            }
            WriteLog("GetContentsForApply", _Toursys + "-" + _Memsys);
            List<WeTourContModel> list = WeTourApplyPage.instance.GetContentListForApply(_Toursys, _Memsys);
            string contents = "";
            if (list.Count > 0)
            {
                contents = JsonHelper.ToJson(list);
            }

            //处理查询结果
            context.Response.Write(string.Format("{0}({1})", jsonCallBackFunName, contents));
            context.Response.End();
        }

        /// <summary>
        /// 赛事报名
        /// </summary>
        /// <param name="context"></param>
        void ApplyTourCont(HttpContext context)
        {
            //处理回调函数
            context.Response.ContentType = "application/json;charset=utf-8";
            string jsonCallBackFunName = context.Request.Params["jsoncallback"].ToString();

            //业务处理
            string _Toursys = context.Request.Params["toursys"].ToString();
            string _Contid = context.Request.Params["contid"].ToString();
            string _Partner = context.Request.Params["partner"].ToString();
            string _memsys = context.Request.Params["MemberSysNo"].ToString();
            WriteLog("ApplyTourCont", _Toursys + "-" + _Contid + "-" + _Partner + "-" + _memsys);
            string _msg = "";
            //莫名其妙出现_memsys的值为两个重复的员工号
            if (_memsys.IndexOf(",") > 0)
            {
                _memsys = _memsys.Split(',')[0];
            }
            //计算报名人员的积分
            string _IsSin = "单打";
            try
            {
                int a = 0;
                WeTourContModel model = WeTourContentDll.instance.GetModelbyId(_Contid);
                if (model.ContentType.IndexOf("双") > 0)
                {
                    _IsSin = "双打";
                    a += Convert.ToInt32(Ranking.RPointDll.instance.GetMatchScor(_Partner, _IsSin));
                }
                a += Convert.ToInt32(Ranking.RPointDll.instance.GetMatchScor(_memsys, _IsSin));
                WriteLog("ApplyTourCont", a.ToString());
                _msg = WeTourApplyDll.instance.ApplyTour(_Toursys, _Contid, _Partner, _memsys, a.ToString());
            }
            catch (Exception e)
            {
                _msg = e.ToString();
                WriteLog("ApplyTourCont", e.ToString());
            }

            WeTourApplyModel appmodel = WeTourApplyDll.instance.GetModelbyContMem(_Contid, _memsys);

            //处理查询结果
            context.Response.Write(string.Format("{0}({1})", jsonCallBackFunName, "{message:\"" + _msg + "\",ordersys:\"" + appmodel.EXT2 + "\"}"));
            context.Response.End();
        }

        /// <summary>
        /// 确认报名信息
        /// </summary>
        /// <param name="context"></param>
        void ConfirmMemIdentity(HttpContext context)
        {
            //处理回调函数
            context.Response.ContentType = "application/json;charset=utf-8";
            string jsonCallBackFunName = context.Request.Params["jsoncallback"].ToString();
            string _msg = "";

            //处理业务
            string _Status = context.Request.Params["status"].ToString();
            string _ContentId = context.Request.Params["contid"].ToString();
            string _Memsys = context.Request.Params["memsys"].ToString();
            WriteLog("ConfirmMemIdentity", _Status + "-" + _ContentId + "-" + _Memsys);
            //莫名其妙出现_memsys的值为两个重复的员工号
            if (_Memsys.IndexOf(",") > 0)
            {
                _Memsys = _Memsys.Split(',')[0];
            }
            WeTourApplyModel model = WeTourApplyDll.instance.GetModelbyContMem(_ContentId, _Memsys);

            if (_Status == "3")
            {
                //直接修改状态
                OrderBll.Instance("wtf").UpdateBusinessOrder(model.EXT2);
                _msg = model.TOURSYS;
            }
            else if (_Status == "4")
            {
                //发送报名确认短信
                WeTourApplyDll.instance.SendApplyConfirm(_ContentId, _Memsys);
                _msg = model.EXT2;
            }

            //处理查询结果
            context.Response.Write(string.Format("{0}({1})", jsonCallBackFunName, "{message:\"" + _msg + "\"}"));
            context.Response.End();
        }

        void WriteTestMessage(HttpContext context)
        {
            //处理回调函数
            context.Response.ContentType = "application/json;charset=utf-8";
            string jsonCallBackFunName = context.Request.Params["jsoncallback"].ToString();
            WriteLog("WriteTestMessage", jsonCallBackFunName);
            string _Toursys = context.Request.Params["toursys"].ToString();
            string _Contid = context.Request.Params["contid"].ToString();
            string _Partner = context.Request.Params["partner"].ToString();
            string _mem = context.Request.Params["mem"].ToString();
            WriteLog("ApplyTourCont", _Toursys + "-" + _Contid + "-" + _Partner + "-" + _mem);

            string _msg = "";
            string _IsSin = "单打";
            try
            {
                int a = 0;
                WeTourContModel model = WeTourContentDll.instance.GetModelbyId(_Contid);
                if (model.ContentType.IndexOf("双") > 0)
                {
                    _IsSin = "双打";
                    a += Convert.ToInt32(Ranking.RPointDll.instance.GetMatchScor(_Partner, _IsSin));
                }
                a += Convert.ToInt32(Ranking.RPointDll.instance.GetMatchScor(_mem, _IsSin));
                _msg = WeTourApplyDll.instance.ApplyTour(_Toursys, _Contid, _Partner, _mem, a.ToString());
            }
            catch
            {
                _msg = "出错啦";
            }

            //处理查询结果
            context.Response.Write(string.Format("{0}({1})", jsonCallBackFunName, "{message:\"you get into the function\"}"));
            context.Response.End();
        }

        /// <summary>
        /// 获得清单
        /// </summary>
        /// <param name="context"></param>
        void GetApplicants(HttpContext context)
        {
            //处理回调函数
            context.Response.ContentType = "application/json;charset=utf-8";
            string jsonCallBackFunName = context.Request.Params["jsoncallback"].ToString();

            //业务处理
            string _ContentId = context.Request.Params["contid"].ToString();
            WriteLog("GetApplicants", _ContentId);
            string json = "";
            try
            {
                List<WeTourApplyModel> list = WeTourApplyDll.instance.getContentApplyByid(_ContentId);
                json = JsonHelper.ToJson(list);
            }
            catch
            { }

            //处理查询结果
            context.Response.Write(string.Format("{0}({1})", jsonCallBackFunName, json));
            context.Response.End();
        }

        #endregion

        #region 签表处理
        /// <summary>
        /// 获得子项赛事的种子
        /// </summary>
        /// <param name="context"></param>
        void GetTourContentSeed(HttpContext context)
        {
            //处理回调函数
            context.Response.ContentType = "application/json;charset=utf-8";
            string jsonCallBackFunName = context.Request.Params["jsoncallback"].ToString();

            string ContId = context.Request.Params["contid"].ToString();
            List<WeTourSeedModel> list = WeTourSeedDll.instance.GetContentSeed(ContId);
            string json = JsonHelper.ToJson(list);

            //处理查询结果
            context.Response.Write(string.Format("{0}({1})", jsonCallBackFunName, json));
            context.Response.End();
        }

        #endregion

        #region 赛事主页
        /// <summary>
        /// 根据项目id获取项目的所有比赛,比赛顺序根据matchorder 排序
        /// </summary>
        /// <param name="context"></param>
        void GetContentMatches(HttpContext context)
        {
            //处理回调函数
            context.Response.ContentType = "application/json;charset=utf-8";
            string jsonCallBackFunName = context.Request.Params["jsoncallback"].ToString();

            string contentid = context.Request.Params["contid"].ToString();
            WriteLog("GetContentMatches", contentid);
            List<WeMatchModel> list = WeMatchDll.instance.GetMatchlistbyCont(contentid);
            string json = JsonHelper.ToJson(list);

            //处理查询结果
            context.Response.Write(string.Format("{0}({1})", jsonCallBackFunName, json));
            context.Response.End();
        }

        /// <summary>
        /// 根据项目id获得所有的轮次
        /// </summary>
        /// <param name="context"></param>
        void GetContentRounds(HttpContext context)
        {
            //处理回调函数
            context.Response.ContentType = "application/json;charset=utf-8";
            string jsonCallBackFunName = context.Request.Params["jsoncallback"].ToString();

            string contentid = context.Request.Params["contid"].ToString();
            WriteLog("GetContentRounds", contentid);
            List<WeMatchModel> list = WeMatchDll.instance.GetContentRounds(contentid);
            string json = JsonHelper.ToJson(list);

            //处理查询结果
            context.Response.Write(string.Format("{0}({1})", jsonCallBackFunName, json));
            context.Response.End();
        }

        /// <summary>
        /// 根据项目id获得签表展示内容
        /// </summary>
        /// <param name="context"></param>
        void GetContentSigns(HttpContext context)
        {
            //处理回调函数
            context.Response.ContentType = "application/json;charset=utf-8";
            string jsonCallBackFunName = context.Request.Params["jsoncallback"].ToString();

            string _Toursys = context.Request.Params["toursys"].ToString();
            WriteLog("GetContentSigns", _Toursys);


            List<WeTourSignsModel> list = WeContentSignsDll.instance.GetToursSign(_Toursys);
            string json = JsonHelper.ToJson(list);

            //处理查询结果
            context.Response.Write(string.Format("{0}({1})", jsonCallBackFunName, json));
            context.Response.End();
        }

        /// <summary>
        /// 根据比赛主键获取赛事计划
        /// </summary>
        /// <param name="context"></param>
        void GetTourSchedule(HttpContext context)
        {
            //处理回调函数
            context.Response.ContentType = "application/json;charset=utf-8";
            string jsonCallBackFunName = context.Request.Params["jsoncallback"].ToString();

            string _Toursys = context.Request.Params["toursys"].ToString();
            WriteLog("GetTourSchedule", _Toursys);
            List<WeTourScheduleModel> list = WeTourSceduleDll.instance.GetTourSchedule(_Toursys);
            string json = JsonHelper.ToJson(list);

            //处理查询结果
            context.Response.Write(string.Format("{0}({1})", jsonCallBackFunName, json));
            context.Response.End();
        }
        /// <summary>
        /// 获得赛事的比赛结果
        /// </summary>
        /// <param name="context"></param>
        void GetTourResult(HttpContext context)
        {
            //处理回调函数
            context.Response.ContentType = "application/json;charset=utf-8";
            string jsonCallBackFunName = context.Request.Params["jsoncallback"].ToString();

            string _Toursys = context.Request.Params["toursys"].ToString();
            WriteLog("GetTourResult", _Toursys);
            List<WeTourResultModel> list = WeTourSceduleDll.instance.GetTourResult(_Toursys);
            string json = JsonHelper.ToJson(list);

            //处理查询结果
            context.Response.Write(string.Format("{0}({1})", jsonCallBackFunName, json));
            context.Response.End();
        }

        #endregion

        #region 获得比赛信息
        /// <summary>
        /// 根据比赛主键获得比赛信息
        /// </summary>
        /// <param name="context"></param>
        void GetMatchInfo(HttpContext context)
        {
            //处理回调函数
            context.Response.ContentType = "application/json;charset=utf-8";
            string jsonCallBackFunName = context.Request.Params["jsoncallback"].ToString();

            string Matchsys = context.Request.Params["sys"].ToString();
            WriteLog("GetMatchInfo", Matchsys);
            WeMatchModel model = WeMatchDll.instance.RenderMatch(Matchsys);
            string json = JsonHelper.ToJson(model);

            //处理查询结果
            context.Response.Write(string.Format("{0}({1})", jsonCallBackFunName, json));
            context.Response.End();
        }

        /// <summary>
        /// 根据比赛主键获得赛事信息
        /// </summary>
        /// <param name="context"></param>
        void GetTourInfobyMatch(HttpContext context)
        {
            //处理回调函数
            context.Response.ContentType = "application/json;charset=utf-8";
            string jsonCallBackFunName = context.Request.Params["jsoncallback"].ToString();

            string Matchsys = context.Request.Params["sys"].ToString();
            WriteLog("GetTourInfobyMatch", Matchsys);
            WeTourModel model = WeTourDll.instance.GetTourModelbyMatchsys(Matchsys);
            string json = JsonHelper.ToJson(model);

            //处理查询结果
            context.Response.Write(string.Format("{0}({1})", jsonCallBackFunName, json));
            context.Response.End();
        }
        #endregion

        #region 分配赛事资源
        void DistributeResource(HttpContext context)
        {
            //处理回调函数
            context.Response.ContentType = "application/json;charset=utf-8";
            string jsonCallBackFunName = context.Request.Params["jsoncallback"].ToString();

            string toursys = context.Request.QueryString["toursys"];
            ResTourDistridll.instance.DistributOne(toursys);

            //处理查询结果
            context.Response.Write(string.Format("{0}({1})", jsonCallBackFunName, "{\"message\":\"ok\"}"));
            context.Response.End();
        }
        #endregion 

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