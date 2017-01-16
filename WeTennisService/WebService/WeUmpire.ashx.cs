using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using WeTour;
using WeUmpire;

namespace WeTennisService.WebService
{
    /// <summary>
    /// WeUmpire 的摘要说明
    /// </summary>
    public class WeUmpire : IHttpHandler
    {

        public void ProcessRequest(HttpContext context)
        {
            context.Response.ContentType = "text/plain";
            string typename = context.Request.QueryString["typename"].ToString();
            switch (typename)
            {
                #region 裁判主页
                case "GetUmpireGreets":
                    GetUmpireGreets(context);
                    break;

                case "GetUmpireTourList":
                    GetUmpireTourList(context);
                    break;
                #endregion

                #region 裁判赛事主页
                case "GetUmpTourMain":
                    GetUmpTourMain(context);
                    break;

                case "GetUmpireMatches":
                    GetUmpireMatches(context);
                    break;
                #endregion

                #region 裁判计分板
                    //开始比赛
                case "StartMatch":
                    StartMatch(context);
                    break;
                    //获得当前发球方和裁判左手方球员
                case "GetGameSerLeft":
                    GetGameSerLeft(context);
                    break;
                //获得比赛的当前比分
                case "GetMatchScores":
                    GetMatchScores(context);
                    break;
                    //添加比分
                case "SetAddPoint":
                    SetAddPoint(context);
                    break;
                    //撤销比分
                case "CancelPoint":
                    CancelPoint(context);
                    break;
                    //批量提交比赛结果
                case "SetBatchMatchResult":
                    SetBatchMatchResult(context);
                    break;
                #endregion 
            }
        }

        #region 裁判主页

        /// <summary>
        /// 获取裁判欢迎语，根据裁判的id，获得裁判姓名和裁判的未裁比赛总数
        /// </summary>
        /// <param name="context"></param>
        void GetUmpireGreets(HttpContext context)
        {
            //处理回调函数
            context.Response.ContentType = "application/json;charset=utf-8";
            string jsonCallBackFunName = context.Request.Params["jsoncallback"].ToString();

            string UmpireSys = context.Request.Params["memsys"].ToString();
            WriteLog("GetUmpireGreets", UmpireSys);
            //莫名奇妙地出现重复
            if (UmpireSys.IndexOf(",") > 0)
            {
                UmpireSys = UmpireSys.Split(',')[0];
            }
            UmpireGreetsModel model = WeTourUmpireDll.instance.GetUmpModel(UmpireSys);
            string json = JsonHelper.ToJson(model);

            //处理查询结果
            context.Response.Write(string.Format("{0}({1})", jsonCallBackFunName, json));
            context.Response.End();
        }

        /// <summary>
        /// 获得裁判的赛事列表
        /// </summary>
        /// <param name="context"></param>
        void GetUmpireTourList(HttpContext context)
        {
            //处理回调函数
            context.Response.ContentType = "application/json;charset=utf-8";
            string jsonCallBackFunName = context.Request.Params["jsoncallback"].ToString();

            string UmpireSys = context.Request.Params["memsys"].ToString();
            WriteLog("GetUmpireTourList", UmpireSys);
            //莫名奇妙地出现重复
            if (UmpireSys.IndexOf(",") > 0)
            {
                UmpireSys = UmpireSys.Split(',')[0];
            }
            List<WeTourModel> list = WeTourUmpireDll.instance.GetUnFinishedMatchbyUmpire(UmpireSys);
            string json = JsonHelper.ToJson(list);

            //处理查询结果
            context.Response.Write(string.Format("{0}({1})", jsonCallBackFunName, json));
            context.Response.End();
        }

        #endregion

        #region 裁判赛事主页
        /// <summary>
        /// 根据赛事主键和裁判id获得赛事的主要信息
        /// </summary>
        /// <param name="context"></param>
        void GetUmpTourMain(HttpContext context)
        {
            //处理回调函数
            context.Response.ContentType = "application/json;charset=utf-8";
            string jsonCallBackFunName = context.Request.Params["jsoncallback"].ToString();

            string TourSys = context.Request.Params["toursys"].ToString();
            string MemSys = context.Request.Params["memsys"].ToString();
            WriteLog("GetUmpTourMain", TourSys + "-" + MemSys);
            //莫名奇妙地出现重复
            if (MemSys.IndexOf(",") > 0)
            {
                MemSys = MemSys.Split(',')[0];
            }
            WeTourModel model = WeTourUmpireDll.instance.GetTourInfo(TourSys, MemSys);
            string json = JsonHelper.ToJson(model);

            //处理查询结果
            context.Response.Write(string.Format("{0}({1})", jsonCallBackFunName, json));
            context.Response.End();
        }

        /// <summary>
        /// 根据赛事主键和裁判主键获得未裁赛事信息
        /// </summary>
        /// <param name="context"></param>
        void GetUmpireMatches(HttpContext context)
        {
            //处理回调函数
            context.Response.ContentType = "application/json;charset=utf-8";
            string jsonCallBackFunName = context.Request.Params["jsoncallback"].ToString();

            string TourSys = context.Request.Params["toursys"].ToString();
            string MemSys = context.Request.Params["memsys"].ToString();
            WriteLog("GetUmpireMatches", TourSys + "-" + MemSys);
            //莫名奇妙地出现重复
            if (MemSys.IndexOf(",") > 0)
            {
                MemSys = MemSys.Split(',')[0];
            }
            List<CourtMatchModel> list = WeTourUmpireDll.instance.GetUmpireMatchbyCourt(TourSys,MemSys);
            string json = JsonHelper.ToJson(list);

            //处理查询结果
            context.Response.Write(string.Format("{0}({1})", jsonCallBackFunName, json));
            context.Response.End();
        }
        #endregion

        #region 裁判计分板
        /// <summary>
        /// 根据比赛主键，首先发球的球员，和首局在左侧的球员，来开启比赛
        /// </summary>
        /// <param name="context"></param>
        void StartMatch(HttpContext context)
        {
            //处理回调函数
            context.Response.ContentType = "application/json;charset=utf-8";
            string jsonCallBackFunName = context.Request.Params["jsoncallback"].ToString();

            string Matchsys = context.Request.Params["sys"].ToString();
            string Server = context.Request.Params["server"].ToString();
            string LeftSide = context.Request.Params["leftside"].ToString();
            WriteLog("StartMatch", Matchsys + "|" + Server + "|" + LeftSide);
            //WeUmpire.GameDll.Get_Instance().ServerDecidedNew(Server, Matchsys, LeftSide);

            //处理查询结果
            context.Response.Write(string.Format("{0}({1})", jsonCallBackFunName, "{\"message\":\"ok\"}"));
            context.Response.End();
        }
        /// <summary>
        /// 获得当前比赛的发球方和左手方球员
        /// </summary>
        /// <param name="context"></param>
        void GetGameSerLeft(HttpContext context)
        {
            //处理回调函数
            context.Response.ContentType = "application/json;charset=utf-8";
            string jsonCallBackFunName = context.Request.Params["jsoncallback"].ToString();

            string MatchSys = context.Request.Params["sys"].ToString();
            WriteLog("GetGameSerLeft",MatchSys);
            GameServerLeftModel model = UmpireDll.instance.UmpGameServerLeft(MatchSys);
            string json = JsonHelper.ToJson(model);

            //处理查询结果
            context.Response.Write(string.Format("{0}({1})", jsonCallBackFunName, json));
            context.Response.End();
        }

        /// <summary>
        /// 获得比赛的当前比分
        /// </summary>
        /// <param name="context"></param>
        void GetMatchScores(HttpContext context)
        {
            //处理回调函数
            context.Response.ContentType = "application/json;charset=utf-8";
            string jsonCallBackFunName = context.Request.Params["jsoncallback"].ToString();

            string MatchSys = context.Request.Params["sys"].ToString();
            WriteLog("GetMatchScores", MatchSys);
            UmpScoreModel model = UmpireDll.instance.GetMatchScores(MatchSys);
            string json = JsonHelper.ToJson(model);

            //处理查询结果
            context.Response.Write(string.Format("{0}({1})", jsonCallBackFunName, json));
            context.Response.End();
        }

        /// <summary>
        /// 添加比分
        /// </summary>
        /// <param name="context"></param>
        void SetAddPoint(HttpContext context)
        {
            //处理回调函数
            context.Response.ContentType = "application/json;charset=utf-8";
            string jsonCallBackFunName = context.Request.Params["jsoncallback"].ToString();

            string PointsInfo = context.Request.Params["ptsinfo"].ToString();
            WriteLog("SetAddPoint", PointsInfo);
            UmpPointModel model = JsonHelper.ParseInfo<UmpPointModel>(PointsInfo);
            //PointsDll.Get_Instance().AddAnewRecord(model);

            //处理查询结果
            context.Response.Write(string.Format("{0}({1})", jsonCallBackFunName, "{\"message\":\"ok\"}"));
            context.Response.End();
        }

        /// <summary>
        /// 撤销比分
        /// </summary>
        /// <param name="context"></param>
        void CancelPoint(HttpContext context)
        {
            //处理回调函数
            context.Response.ContentType = "application/json;charset=utf-8";
            string jsonCallBackFunName = context.Request.Params["jsoncallback"].ToString();

            string MatchSys = context.Request.Params["sys"].ToString();
            WriteLog("CancelPoint", MatchSys);
            //PointsDll.Get_Instance().CancelPoints(MatchSys);

            //处理查询结果
            context.Response.Write(string.Format("{0}({1})", jsonCallBackFunName, "{\"message\":\"ok\"}"));
            context.Response.End();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="context"></param>
        void SetBatchMatchResult(HttpContext context)
        {
            //处理回调函数
            context.Response.ContentType = "application/json;charset=utf-8";
            string jsonCallBackFunName = context.Request.Params["jsoncallback"].ToString();

            string scorejson = context.Request.Params["scores"].ToString();
            WriteLog("BatchMatchResult", scorejson);
            List<UmpMatchResult> list = JsonHelper.ParseInfo<List<UmpMatchResult>>(scorejson);
            string Res = UmpireDll.instance.SubmitMatchResults(list);

            //处理查询结果
            context.Response.Write(string.Format("{0}({1})", jsonCallBackFunName, "{\"message\":\"" + Res + "\"}"));
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