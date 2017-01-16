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
using System.IO;

namespace WeTennisService.BackService
{
    /// <summary>
    /// TourMgm 的摘要说明
    /// </summary>
    public class TourMgm : IHttpHandler
    {
        string _AllowOrigin = "*";

        public void ProcessRequest(HttpContext context)
        {
            context.Response.ContentType = "text/plain";            
            //context.Response.AddHeader("Access-Control-Allow-Origin", _AllowOrigin);
            
            string typename = "";
            try
            {
                typename = context.Request.Params["typename"].ToString();
            }
            catch
            {
                typename = context.Request.QueryString["typename"].ToString();
            }

            //获取jsonstring.
            var jsonstr = string.Empty;
            context.Request.InputStream.Position = 0;
            using (var inputStream = new StreamReader(context.Request.InputStream))
            {
                jsonstr = inputStream.ReadToEnd();
            }  

            //添加记录
            Model_Basic_Service_Log serivce = new Model_Basic_Service_Log();
            serivce.ServiceName = typename;
            serivce.URL = context.Request.Url.ToString();
            serivce.ResponseStr = jsonstr;
            //Biz_Basic_Service_log.instance.AddNewLog(serivce);

            switch (typename)
            {
                case "HelloKitty":
                    HelloKitty(context);
                    break;

                #region Tour

                case "GetTourList":
                    GetTourList(context);
                    break;
                case "CreateNewTour":
                    CreateNewTour(context);
                    break;

                case "GetTourInfobySys":
                    GetTourInfobySys(context);
                    break;

                case "UpdateTourBasic":
                    UpdateTourBasic(context);
                    break;
                    
                case "UpdateTourStatus":
                    UpdateTourStatus(context);
                    break;

                case "RollBackTour":
                    RollBackTour(context);
                    break;
                    
                #endregion

                #region TourContent
                case "CreateTourContent":
                    CreateTourContent(context);
                    break;
                    
                case "UpdateTourContent":
                    UpdateTourContent(context);
                    break;

                case "UpdateTourContentSigns":
                    UpdateTourContentSigns(context);
                    break;

                case "DeleteTourContent":
                    DeleteTourContent(context);
                    break;

                case "GetTourContents":
                    GetTourContents(context);
                    break;

                case "GetTourContentModel":
                    GetTourContentModel(context);
                    break;
                #endregion

                #region Tour Apply
                case "AddNewApply":
                    AddNewApply(context);
                    break;

                case "AddDirectApply":
                    AddDirectApply(context,jsonstr);
                    break;

                case "addDirectApply2":
                    addDirectApply2(context, jsonstr);
                    break;

                case "TourApply_GetContentApplicant":
                    TourApply_GetContentApplicant(context);
                    break;

                case "TourApply_GetApplistbyContid":
                    TourApply_GetApplistbyContid(context);
                    break;

                case "TourApp_DeleteApp":
                    TourApp_DeleteApp(context);
                    break;

                case "TourApplyFeeInfo":
                    TourApplyFeeInfo(context);
                    break;

                case "GetTourApplyCon":
                    GetTourApplyCon(context);
                    break;

                case "ExcelApplicants":
                    ExcelApplicants(context);
                    break;

                case "ExcelApplicants_group":
                    ExcelApplicants_group(context);
                    break;
                #endregion

                #region TourSeed
                case "GetTourSeed":
                    GetTourSeed(context);
                    break;

                case "AddContentSeed":
                    AddContentSeed(context);
                    break;

                case "UpdateSeedOrder":
                    UpdateSeedOrder(context);
                    break;

                case "DeleteSeed":
                    DeleteSeed(context);
                    break;

                case "GetUnSeededApp":
                    GetUnSeededApp(context);
                    break;

                case "SubmitContentSeed":
                    SubmitContentSeed(context);
                    break;
                #endregion

                #region TourSign
                case "RandowSign":
                    RandowSign(context);
                    break;

                case "GetContentSign":
                    GetContentSign(context);
                    break;

                case "GetUnSignedApp":
                    GetUnSignedApp(context);
                    break;

                case "UpdateContentSign":
                    UpdateContentSign(context);
                    break;

                case "GenerateContentMatch":
                    GenerateContentMatch(context);
                    break;

                case "GetContentMatches":
                    GetContentMatches(context);
                    break;
                #endregion

                #region Distribution
                    #region Distri_Gym
                    case "GetMyGyms":
                        GetMyGyms(context);
                        break;

                    case "GetGymsbyToursys":
                        GetGymsbyToursys(context);
                        break;

                    case "AddGymToTour":
                        AddGymToTour(context);
                        break;

                    case "DeleteTourGym":
                        DeleteTourGym(context);
                        break;

                    case "GetTourCourt":
                        GetTourCourt(context);
                        break;

                    case "UpdateTourGymCourts":
                        UpdateTourGymCourts(context);
                        break;

                    case "LoadGymConts":
                        LoadGymConts(context);
                        break;

                    case "AddDistriGymCont":
                        AddDistriGymCont(context);
                        break;

                    case "DeleteContid":
                        DeleteContid(context);
                        break;
                    #endregion

                    #region Distri_Date

                case "AddTour_Date":
                        AddTour_Date(context);
                        break;

                case "AddTour_DateRound":
                        AddTour_DateRound(context);
                        break;

                case "GetTourDate":
                        GetTourDate(context);
                        break;

                case "Delete_TourDate":
                        Delete_TourDate(context);
                        break;
                case "GetDateRounds":
                        GetDateRounds(context);
                        break;
                case "SubmitContRound":
                        SubmitContRound(context);
                        break;

                case "RandomDistributeRes":
                        RandomDistributeRes(context);
                        break;

                case "LoadCourtMatches":
                        LoadCourtMatches(context);
                        break;

                    #endregion

                case "ScoreSet_Get":
                        ScoreSet_Get(context);
                        break;
                case "ScoreSet_Update":
                        ScoreSet_Update(context);
                        break;
                #region Distri_Resource
                case "Dis_TourResource":
                        Dis_TourResource(context);
                        break;
                #endregion


                #endregion

                #region TourRanking
                case "UpdateUnionRanking":
                    UpdateUnionRanking(context);
                    break;

                case "GetUnionRank":
                    GetUnionRank(context);
                    break;
                #endregion

                #region 比赛结果维护
                case "GetContentRounds":
                    GetContentRounds(context);
                    break;

                case "GetMatchesByContRound":
                    GetMatchesByContRound(context);
                    break;

                case "RecordMatchRes":
                    RecordMatchRes(context,jsonstr);
                    break;

                case "RecordMatchRes2":
                    RecordMatchRes2(context);
                    break;

                case "endTour":
                    endTour(context);
                    break;
                #endregion
            }
        }

        void HelloKitty(HttpContext context)
        {
            context.Response.AddHeader("Access-Control-Allow-Origin", _AllowOrigin);
            context.Response.Write("hello");

        }

      

        #region Tour
        /// <summary>
        /// 根据状态，获取赛事列表
        /// </summary>
        /// <param name="context"></param>
        void GetTourList(HttpContext context)
        {
            context.Response.AddHeader("Access-Control-Allow-Origin", _AllowOrigin);
            string _ClubSys = context.Request.Params["ClubSys"];
            string _Status = context.Request.Params["status"];
            string _TourType = context.Request.Params["tourtype"];
            string json = "";
            try
            {
                List<Model_ApiTourList> list = Biz_TourMgm.instance.GetTourList_Sep(_ClubSys, _Status, _TourType);//使用6状态赛事
                string datajson = JsonHelper.ToJson(list);
                json = "{code:0,errormsg:\"\",data:" + datajson + ",requestdate:\"" + DateTime.Now.ToString() + "\"}";
            }
            catch (Exception e)
            {
                json = "{code:1,errormsg:\"服务器错误，请联系管理员\",data:\"" + e.ToString().Substring(0, 100) + "\"}";
            }
            context.Response.Write(json);
        }

        /// <summary>
        /// 创建新的赛事
        /// </summary>
        /// <param name="context"></param>
        void CreateNewTour(HttpContext context)
        {
            context.Response.AddHeader("Access-Control-Allow-Origin", _AllowOrigin);
            string Res = "";
            WeTourModel model = new WeTourModel();
            try
            {
                model.NAME = context.Request.Form["tourName"].ToString();
                model.CAPACITY = context.Request.Form["tourType"].ToString();
                model.UnionSys = context.Request.Form["unionName"].ToString();
                model.TOURSYS = context.Request.Form["tourLevel"].ToString();
                model.TOURIMG = "/Lib/img/tour/" + model.TOURSYS + "A.png";
                model.STARTDATE = context.Request.Form["tourDate"].ToString();
                model.ENDDATE = context.Request.Form["tourEndDate"].ToString();
                model.ADDRESS = context.Request.Form["tourAddress"].ToString();
                model.SETTYPE = context.Request.Form["setType"].ToString();
                model.EXT2 = context.Request.Form["ext2"].ToString();
                model.GAMETYPE = context.Request.Form["gameType"].ToString();
                model.EXT1 = context.Request.Form["ext1"].ToString();
                model.CITYID = context.Request.Form["tourCity"];
                model.ext8 = context.Request.Form["ext8"].ToString();

                if (model.CAPACITY == "公开赛")
                {
                    //model.MGRSYS = context.Request.Params["memsys"].ToString();                 
                    model.MGRSYS = context.Request.Params["ClubSys"];
                }
                else
                {
                    model.MGRSYS = context.Request.Params["ClubSys"];
                    if (model.CAPACITY == "俱乐部赛")
                    {
                        model.CITYTYPE = "Club";
                    }
                    else
                    {
                        model.CITYTYPE = "Union";
                    }
                }

                //string RtnUrl = context.Request.Form["rtnUrl"].ToString();//读取填写的返回URL

                string TourSys = WeTourDll.instance.AddNewTour(model);
                //Res = "code=0&toursys="+TourSys;
                Res = "{code:0,errormsg:\"\",data:{toursys:\"" + TourSys + "\"}}";

            }
            catch(Exception e)
            {
                //Res = "code=1&toursys=";
                Res = "{code:1,errormsg:\"添加失败\",data:{toursys:\""+e.ToString().Substring(0,100)+"\"}}";
            }
            //context.Response.Redirect(RtnUrl + Res);
            context.Response.Write(Res);
        }

        void GetTourInfobySys(HttpContext context)
        {
            context.Response.AddHeader("Access-Control-Allow-Origin", _AllowOrigin);

            string Res = "";
            try
            {
                string sysno = context.Request.Params["sysno"];
                if (!string.IsNullOrEmpty(sysno))
                {
                    //根据对应的sysno获得赛事实体
                    WeTourModel model = WeTourDll.instance.GetModelbySys(sysno);
                    string json = JsonHelper.ToJson(model);
                    Res = "{code:0,errormsg:\"\",data:"+json+"}";
                }
                else
                {
                    Res = "{code:1,errormsg:\"获取失败，请输入有效sysno\",data:{toursys:\"\"}}";
                }
            }
            catch (Exception e)
            {
                Res = "{code:2,errormsg:\"系统异常\",data:{toursys:\"" + e.ToString().Substring(0, 100) + "\"}}";
            }

            context.Response.Write(Res);
        }

        /// <summary>
        /// 修改赛事基本信息
        /// </summary>
        /// <param name="context"></param>
        void UpdateTourBasic(HttpContext context)
        {
            context.Response.AddHeader("Access-Control-Allow-Origin", _AllowOrigin);
            string Res = "";
            string _Sysno = context.Request.Params["sysno"];
            WeTourModel model = WeTourDll.instance.GetModelbySys(_Sysno);

            try
            {
                model.NAME = context.Request.Params["tourName"].ToString();
                model.CAPACITY = context.Request.Params["tourType"].ToString();
                model.UnionSys = context.Request.Params["unionName"].ToString();
                model.TOURSYS = context.Request.Params["tourLevel"].ToString();
                model.STARTDATE = context.Request.Params["tourDate"].ToString();
                model.ADDRESS = context.Request.Params["tourAddress"].ToString();
                model.SETTYPE = context.Request.Params["setType"].ToString();
                model.EXT2 = context.Request.Params["ext2"].ToString();
                model.GAMETYPE = context.Request.Params["gameType"].ToString();
                model.EXT1 = context.Request.Params["ext1"].ToString();
                model.ext8 = context.Request.Params["ext8"].ToString();
                model.ENDDATE = context.Request.Params["applyEnd"];
                model.TourBackImg = context.Request.Params["BackImg"];
                if (model.CAPACITY == "公开赛")
                {
                    model.MGRSYS = context.Request.Params["memsys"].ToString();
                }
                else
                {
                    model.MGRSYS = context.Request.Params["ClubSys"];
                    if (model.CAPACITY == "俱乐部赛")
                    {
                        model.CITYTYPE = "Club";
                    }
                    else
                    {
                        model.CITYTYPE = "Union";
                    }
                }

                //string RtnUrl = context.Request.Form["rtnUrl"].ToString();//读取填写的返回URL

                if (WeTourDll.instance.UpdateTourInfo(model))
                {
                    Res = "{code:0,errormsg:\"\",data:{status:\"success\"}}";
                }
                else
                {
                    Res = "{code:1,errormsg:\"更新失败\",data:{status:\"false\"}}";
                }

            }
            catch (Exception e)
            {
                //Res = "code=1&toursys=";
                Res = "{code:1,errormsg:\"程序异常，更新失败\",data:{toursys:\"" + e.ToString().Substring(0, 100) + "\"}}";
            }

            context.Response.Write(Res);

        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="context"></param>
        void UpdateTourStatus(HttpContext context)
        {
            context.Response.AddHeader("Access-Control-Allow-Origin", _AllowOrigin);
            string _Res = "";
            try
            {
                string _TourSys = context.Request.Params["sysno"];
                string _Status = context.Request.Params["status"];

                //在修改状态之前，应该先验证赛事状态是否满足更新的条件。
                //
                if (WeTourDll.instance.UpdateTourStatus(_TourSys, _Status))
                {
                    _Res = "{code:0,errormsg:\"\"}";
                }
                else
                {
                    _Res = "{code:1,errormsg:\"修改失败\"}";
                }
            }
            catch (Exception e)
            {
                _Res = "{code:1,errormsg:\""+e.ToString().Substring(0,100)+"\"}";
            }
            context.Response.Write(_Res);
        }

        /// <summary>
        /// 回滚赛事状态
        /// </summary>
        /// <param name="context"></param>
        void RollBackTour(HttpContext context)
        { 
            context.Response.AddHeader("Access-Control-Allow-Origin",_AllowOrigin);
            string _Res = "";
            try{
                string _TourSys = context.Request.Params["sysno"];
                _Res = WeTourDll.instance.TourStatusRollBack(_TourSys);
                _Res = "{code:0,errormsg:\"" + _Res + "\"}";
            }
            catch(Exception e)
            {
                _Res = "{code:1,errormsg:\""+e.ToString().Substring(0,150)+"\"}";
            }
            context.Response.Write(_Res);
        }

        #endregion

        #region TourContent

        /// <summary>
        /// 创建新的比赛项目
        /// </summary>
        /// <param name="context"></param>
        void CreateTourContent(HttpContext context)
        {
            context.Response.AddHeader("Access-Control-Allow-Origin", _AllowOrigin);
            WeTourContModel model = new WeTourContModel();
            string _Res = "";
            try
            {
                model.Toursys = context.Request.Params["toursys"];
                model.ContentName = context.Request.Params["name"];
                model.ContentType = context.Request.Params["selType"];
                model.TourDate = context.Request.Params["group"];//组别
                model.ext3 = context.Request.Params["applyfee"];
                model.SignQty = context.Request.Params["signqty"];
                model.AllowGroup = context.Request.Params["allowgroup"];
                model.GroupType = context.Request.Params["grouptype"];
                if (WeTourContentDll.instance.InsertNewContent(model))
                {
                    _Res = "{code:0,errormsg:\"\"}";
                }
                else
                {
                    _Res = "{code:1,errormsg:\"添加新项目失败\"}";
                }
            }
            catch (Exception e)
            {
                _Res = "{code:2,errormsg:\""+e.ToString().Substring(0,100)+"\"}";
            }
            context.Response.Write(_Res);
        }

        /// <summary>
        /// 修改赛事项目具体信息
        /// </summary>
        /// <param name="context"></param>
        void UpdateTourContent(HttpContext context)
        {
            context.Response.AddHeader("Access-Control-Allow-Origin", _AllowOrigin);
            string _Id = context.Request.Params["id"];
            WeTourContModel model = WeTourContentDll.instance.GetModelbyId(_Id);
            string _Res = "";
            try
            {
                
                model.ContentName = context.Request.Params["name"];
                model.ContentType = context.Request.Params["selType"];
                model.TourDate = context.Request.Params["group"];//组别
                model.ext3 = context.Request.Params["applyfee"];               
                model.SignQty = context.Request.Params["signqty"];
                model.AllowGroup = context.Request.Params["allowgroup"];
                model.GroupType = context.Request.Params["grouptype"];
                if (WeTourContentDll.instance.UpdateContent(model))
                {
                    _Res = "{code:0,errormsg:\"\"}";
                }
                else
                {
                    _Res = "{code:1,errormsg:\"修改项目失败\"}";
                }
            }
            catch (Exception e)
            {
                _Res = "{code:2,errormsg:\"" + e.ToString().Substring(0, 100) + "\"}";
            }
            context.Response.Write(_Res);
        }

        /// <summary>
        /// 修改赛事项目的签表信息
        /// </summary>
        /// <param name="context"></param>
        void UpdateTourContentSigns(HttpContext context)
        {
            context.Response.AddHeader("Access-Control-Allow-Origin", _AllowOrigin);
            string _Id = context.Request.Params["id"];
            WeTourContModel model = WeTourContentDll.instance.GetModelbyId(_Id);
            string _Res = "";
            try
            {
                model.SignQty = context.Request.Params["signqty"];
                model.AllowGroup = context.Request.Params["allowgroup"];
                model.GroupType = context.Request.Params["grouptype"];
                if (WeTourContentDll.instance.UpdateContent(model))
                {
                    _Res = "{code:0,errormsg:\"\"}";
                }
                else
                {
                    _Res = "{code:1,errormsg:\"修改项目失败\"}";
                }
            }
            catch (Exception e)
            {
                _Res = "{code:2,errormsg:\"" + e.ToString().Substring(0, 100) + "\"}";
            }
            context.Response.Write(_Res);
        }

        /// <summary>
        /// 删除项目
        /// </summary>
        /// <param name="context"></param>
        void DeleteTourContent(HttpContext context)
        {
            context.Response.AddHeader("Access-Control-Allow-Origin", _AllowOrigin);
            string _Id=context.Request.Params["id"];
            string _Res = "";
            try
            {

                if (WeTourContentDll.instance.DeleteContent(_Id))
                {
                    _Res = "{code:0,errormsg:\"\"}";
                }
                else
                {
                    _Res = "{code:1,errormsg:\"删除项目失败\"}";
                }
            }
            catch (Exception e)
            {
                _Res = "{code:2,errormsg:\"" + e.ToString().Substring(0, 100) + "\"}";
            }
            context.Response.Write(_Res);
        }

        /// <summary>
        /// 获取所有比赛项目
        /// </summary>
        /// <param name="context"></param>
        void GetTourContents(HttpContext context)
        {
            context.Response.AddHeader("Access-Control-Allow-Origin", _AllowOrigin);

            string _Toursys = context.Request.Params["sysno"];
            List<WeTourContModel> list = WeTourContentDll.instance.GetContentlist(_Toursys);
            string json = JsonHelper.ToJson(list);
            context.Response.Write("{\"code\":0,\"errormsg\":\"\",\"data\":" + json + "}");
        }

        /// <summary>
        /// 获得单个比赛项目实体
        /// </summary>
        /// <param name="context"></param>
        void GetTourContentModel(HttpContext context)
        {
            context.Response.AddHeader("Access-Control-Allow-Origin", _AllowOrigin);
            try
            {
                string _Id = context.Request.Params["id"];
                WeTourContModel model = WeTourContentDll.instance.GetModelbyId(_Id);
                string json = JsonHelper.ToJson(model);
                context.Response.Write("{\"code\":0,\"errormsg\":\"\",\"data\":" + json + "}");
            }
            catch(Exception e) {
                context.Response.Write("{\"code\":1,\"errormsg\":\""+e.ToString().Substring(0,100)+"\"}");
            }
        }

      

        #endregion

        #region TourApply
        /// <summary>
        /// 添加新的报名信息
        /// </summary>
        /// <param name="context"></param>
        void AddNewApply(HttpContext context)
        {
            context.Response.AddHeader("Access-Control-Allow-Origin", _AllowOrigin);
            WeTourApplyModel model = new WeTourApplyModel();
            string _Res = "";
            try
            {
                model.TOURSYS = context.Request.Params["toursys"];
                model.CONTENTID = context.Request.Params["contid"];
                if (WeTourApplyDll.instance.InsertNewApply(model))
                {
                    _Res = "{code:0,errormsg=\"\"}";
                }
                else
                {
                    _Res = "{code:1,errormsg=\"添加失败\"}";
                }
            }
            catch (Exception e)
            {
                _Res = "{code:2,errormsg=\""+e.ToString().Substring(0,100)+"\"}";
            }
            context.Response.Write(_Res);
        }

        /// <summary>
        /// 添加直通车报名
        /// </summary>
        /// <param name="context"></param>
        void AddDirectApply(HttpContext context,string jsonstr)
        {
            context.Response.AddHeader("Access-Control-Allow-Origin", _AllowOrigin);
            List<WeTourApplyModel> list = new List<WeTourApplyModel>();
            string _Res = "";
            try
            {
                //获取参数
                //string applylist = context.Request.Params["ApplyList"];
                list = JsonHelper.ParseFormJson<List<WeTourApplyModel>>(jsonstr);
                string json = "[";
                if (list.Count > 0)
                {                    
                    for (int i = 0; i < list.Count; i++)
                    {
                        list[i].STATUS="1";
                        list[i].EXT1 = "赛事直通报名";
                        string Validate = WeTourApplyDll.instance.ValidateContApply(list[i], 1);//验证到性别
                        if (Validate != "ok")
                        {
                            json += "{\"id\":" + i + ",\"status\":\"" + Validate + "\"},";
                        }
                        else
                        {
                            if (WeTourApplyDll.instance.InsertNewApply(list[i]))
                            {
                                json += "{\"id\":" + i + ",\"status\":\"添加成功\"},";//表示添加成功
                            }
                            else
                            {
                                json += "{\"id\":" + i + ",\"status\":\"添加失败\"},";//表示添加失败
                            }
                        }
                    }
                    json = json.TrimEnd(',');
                }
                json += "]";
                _Res = "{\"code\":0,\"errormsg\":\""+list.Count+"\",\"data\":"+json+"}";
            }
            catch (Exception e)
            {
                _Res = "{\"code\":1,\"errormsg\"=\"" + e.ToString().Substring(0, 100) + "\"}";
            }
            context.Response.Write(_Res);
        }

        void addDirectApply2(HttpContext context, string jsonstr)
        { 
            Model_Return ret=new Model_Return();
            ret.errorMsg=jsonstr;
            context.Response.AddHeader("Access-Control-Allow-Origin", _AllowOrigin);
            List<WeTourApplyModel> list = new List<WeTourApplyModel>();
            string _Res = "";
            try
            {
                ret.code=0;
                ret.errorMsg = jsonstr;
            }
            catch (Exception e)
            {
                ret.errorMsg = e.ToString();
            }
            context.Response.Write(JsonHelper.ToJson(ret));
        }

        /// <summary>
        /// 获取各个项目的报名情况
        /// </summary>
        /// <param name="context"></param>
        void TourApply_GetContentApplicant(HttpContext context)
        {
            context.Response.AddHeader("Access-Control-Allow-Origin", _AllowOrigin);
            string _Res = "";
            try
            {
                string _Toursys = context.Request.Params["toursys"];
                List<WeTourContModel> list = WeTourApplyDll.instance.GetContApplicant(_Toursys);
                string json = JsonHelper.ToJson(list);
                _Res = "{\"code\":0,\"errormsg\":\"\",\"data\":" + json + "}";
            }
            catch (Exception e)
            {
                _Res = "{code:1,errormsg:\""+e.ToString().Substring(0,100)+"\"}";
            }
            context.Response.Write(_Res);
        }

        /// <summary>
        /// 获取具体的项目的报名情况
        /// </summary>
        /// <param name="context"></param>
        void TourApply_GetApplistbyContid(HttpContext context)
        {
            context.Response.AddHeader("Access-Control-Allow-Origin", _AllowOrigin);
            string _Res = "";
            try
            {
                string _Contid = context.Request.Params["contid"];
                List<WeTourApplyModel> list = WeTourApplyDll.instance.GetContAppMemberInfo(_Contid);
                string json = JsonHelper.ToJson(list);
                _Res = "{\"code\":0,\"errormsg\":\"\",\"data\":" + json + "}";
            }
            catch (Exception e)
            {
                _Res = "{code:1,errormsg:\"" + e.ToString().Substring(0, 100) + "\"}";
            }
            context.Response.Write(_Res);

        }

        /// <summary>
        /// 删除一条报名信息
        /// </summary>
        /// <param name="context"></param>
        void TourApp_DeleteApp(HttpContext context)
        {
            context.Response.AddHeader("Access-Control-Allow-Origin", _AllowOrigin);
            string _Res = "";
            try
            {
                string _id = context.Request.Params["id"];
                if (WeTourApplyDll.instance.DeleteApply(_id))
                {
                    _Res = "{\"code\":0,\"errormsg\":\"\"}";
                }
                else
                {
                    _Res = "{\"code\":1,\"errormsg\":\"删除失败\"}";
                }
            }
            catch (Exception e)
            {
                _Res = "{code:2,errormsg:\"" + e.ToString().Substring(0, 100) + "\"}";
            }
            context.Response.Write(_Res);
        }

        /// <summary>
        /// Get Apply Fee Info
        /// </summary>
        /// <param name="context"></param>
        void TourApplyFeeInfo(HttpContext context)
        {
            context.Response.AddHeader("Access-Control-Allow-Origin", _AllowOrigin);

            string _Toursys = context.Request.QueryString["toursys"].ToString();
            string AppFee = WeTourApplyDll.instance.GetTourApplyFeeInfo(_Toursys);
            context.Response.Write(AppFee);
        }


        /// <summary>
        /// 获取各项目的明细
        /// </summary>
        /// <param name="context"></param>
        void GetTourApplyCon(HttpContext context)
        {
            context.Response.AddHeader("Access-Control-Allow-Origin", _AllowOrigin);
            string _Res = "";
            try
            {
                string _Toursys = context.Request.QueryString["toursys"].ToString();
                List<TourApplyCon> list = WeTourApplyDll.instance.GetApplyCon(_Toursys);
                string json = JsonHelper.ToJson(list);
                _Res = "{code:0,data:"+json+"}";
            }
            catch(Exception e)
            {
                _Res = "{code:1,errormsg:" + e.ToString().Substring(0,150) + "}";
            }
            context.Response.Write(_Res);
        }

        /// <summary>
        /// 使用EXCEL导入
        /// </summary>
        /// <param name="context"></param>
        void ExcelApplicants(HttpContext context)
        {
            context.Response.AddHeader("Access-Control-Allow-Origin", _AllowOrigin);
            string _Res = "";
            string _BackUrl = "";
            try
            {
                string _Toursys=context.Request.Form["toursys"];
                _BackUrl = context.Request.Form["backUrl"];
                
                //保存文件到本地
                HttpPostedFile file = context.Request.Files[0];
                string filepath = @"D:\Resource\UploadFiles\"+DateTime.Now.ToString("yyyyMMdd hhmmss")+_Toursys+".xlsx";
                file.SaveAs(filepath);//保存到本地

                //使用excelHelper读取本地文件
                //DataTable dt = ExcelHelper.InputFromExcel(filepath, "Sheet1");
                DataTable dt = MyExcelHelper.ExcelSqlConnection(filepath, "Sheet1").Tables[0];

                //将读取到的内容写入到touapply,并获取返回结果
                List<WeTourApplyModel> list = WeTourApplyDll.instance.ExcelApplicants(dt, _Toursys);
                string json = JsonHelper.ToJson(list);

                _Res = "{code:0,errormsg:\"" + dt.Rows.Count + "\",data:" + json + "}";
                
            }
            catch(Exception e)
            {
                _Res = "{code:1,errormsg:\"" + e.ToString().Substring(0, 150) + "\"}";
            }
            //Form提交以后的页面跳转
            context.Response.Write(_Res);
            context.Response.Redirect(_BackUrl);
            
        }

        void ExcelApplicants_group(HttpContext context)
        {
            context.Response.AddHeader("Access-Control-Allow-Origin", _AllowOrigin);
            string _Res = "";
            string _BackUrl = "";
            try
            {
                string _Toursys = context.Request.Form["toursys"];
                _BackUrl = context.Request.Form["backUrl"];

                //保存文件到本地
                HttpPostedFile file = context.Request.Files[0];
                string filepath = @"D:\Resource\UploadFiles\" + DateTime.Now.ToString("yyyyMMdd hhmmss") + _Toursys + ".xlsx";
                file.SaveAs(filepath);//保存到本地

                //使用excelHelper读取本地文件
                //DataTable dt = ExcelHelper.InputFromExcel(filepath, "Sheet1");
                DataTable dt = MyExcelHelper.ExcelSqlConnection(filepath, "Sheet1").Tables[0];

                //将读取到的内容写入到touapply,并获取返回结果
                List<WeTourApplyModel> list = WeTourApplyDll.instance.ExcelApplicants(dt, _Toursys);
                string json = JsonHelper.ToJson(list);

                _Res = "{code:0,errormsg:\"" + dt.Rows.Count + "\",data:" + json + "}";

            }
            catch (Exception e)
            {
                _Res = "{code:1,errormsg:\"" + e.ToString().Substring(0, 150) + "\"}";
            }
            //Form提交以后的页面跳转
            context.Response.Write(_Res);
            context.Response.Redirect(_BackUrl);

        }
        #endregion

        #region TourSeed
        /// <summary>
        /// 获取种子
        /// </summary>
        /// <param name="context"></param>
        void GetTourSeed(HttpContext context)
        {
            context.Response.AddHeader("Access-Control-Allow-Origin", _AllowOrigin);           

            string _Res = "";
            try
            {
                string _ContentId = context.Request.Params["contid"];
                List<WeTourSeedModel> list = WeTourSeedDll.instance.GetContentSeed(_ContentId);
                string json = JsonHelper.ToJson(list);

                List<WeTourApplyModel> list1 = WeTourSeedDll.instance.GetTourSeedApp(_ContentId);
                string json1 = JsonHelper.ToJson(list1);

                _Res = "{code:0,data:"+json+",data1:"+json1+"}";
            }
            catch (Exception e)
            {
                _Res = "{code:2,errormsg:\"" + e.ToString().Substring(0, 100) + "\"}";
            }
            context.Response.Write(_Res);
        }

        /// <summary>
        /// 添加种子
        /// </summary>
        /// <param name="context"></param>
        void AddContentSeed(HttpContext context)
        {
            context.Response.AddHeader("Access-Control-Allow-Origin", _AllowOrigin);
            string _Res = "";
            try
            {
                string jsonstr = context.Request.Params["seed"];
                WeTourSeedModel model = JsonHelper.ParseFormJson<WeTourSeedModel>(jsonstr);
                if (WeTourSeedDll.instance.InsertNew(model))
                {
                    _Res = "{code:0,errormsg:\"添加成功\"}";
                }
                else
                {
                    _Res = "{code:1,errormsg:\"添加失败\"}";
                }
            }
            catch (Exception e)
            {
                _Res = "{code:2,errormsg:\""+e.ToString().Substring(0,100)+"\"}";
            }
            context.Response.Write(_Res);
        }

        /// <summary>
        /// 批量提交种子
        /// </summary>
        /// <param name="context"></param>
        void SubmitContentSeed(HttpContext context)
        {
            context.Response.AddHeader("Access-Control-Allow-Origin", _AllowOrigin);
            string _Res = "";
            try
            {
                string json = context.Request.Params["json"].ToString();
                string _id=context.Request.Params["id"].ToString();
                List<WeTourApplyModel> list = JsonHelper.ParseInfo<List<WeTourApplyModel>>(json);
                WeTourSeedDll.instance.BatchAddSeed(list, _id);
                _Res = "{code:0,errormsg:\"\"}";
            }
            catch (Exception e)
            {
                _Res = "{code:2,errormsg:\"" + e.ToString().Substring(0, 100) + "\"}";
            }
            context.Response.Write(_Res);
        }

        /// <summary>
        /// 修改种子序号
        /// </summary>
        /// <param name="context"></param>
        void UpdateSeedOrder(HttpContext context)
        {
            context.Response.AddHeader("Access-Control-Allow-Origin", _AllowOrigin);
            string _Res = "";
            try
            {
                string id = context.Request.Params["id"].ToString();
                string _Order = context.Request.Params["order"].ToString();
                if (WeTourSeedDll.instance.UpdateSeedOrder(id, _Order))
                {
                    _Res = "{code:0,errormsg:\"删除成功\"}";
                }
                else
                {
                    _Res = "{code:1,errormsg:\"删除失败\"}";
                }
            }
            catch (Exception e)
            {
                _Res = "{code:2,errormsg:\"" + e.ToString().Substring(0, 100) + "\"}";
            }
            context.Response.Write(_Res);
        }

        /// <summary>
        /// 删除种子
        /// </summary>
        /// <param name="context"></param>
        void DeleteSeed(HttpContext context)
        {
            context.Response.AddHeader("Access-Control-Allow-Origin", _AllowOrigin);
            string _Res = "";
            try
            {
                string id = context.Request.Params["id"].ToString();
                if (WeTourSeedDll.instance.DeleteSeed(id))
                {
                    _Res = "{code:0,errormsg:\"删除成功\"}";
                }
                else
                {
                    _Res = "{code:1,errormsg:\"删除失败\"}";
                }
            }
            catch (Exception e)
            {
                _Res = "{code:2,errormsg:\"" + e.ToString().Substring(0, 100) + "\"}";
            }
            context.Response.Write(_Res);
        }

        /// <summary>
        /// 获取非种子的报名人员
        /// </summary>
        /// <param name="context"></param>
        void GetUnSeededApp(HttpContext context)
        {
            context.Response.AddHeader("Access-Control-Allow-Origin", _AllowOrigin);
            string _Res = "";
            try
            {
                string _ContId = context.Request.Params["id"];
                List<WeTourApplyModel> list = WeTourSeedDll.instance.GetUnseedApplicant(_ContId);
                string json = JsonHelper.ToJson(list);
                _Res = "{code:0,errormsg:\"\",data:"+json+"}";
            }
            catch (Exception e)
            {
                _Res = "{code:2,errormsg:\"" + e.ToString().Substring(0, 100) + "\"}";
            }
            context.Response.Write(_Res);
        }
        #endregion

        #region TourSign
        /// <summary>
        /// 随机排布签表
        /// </summary>
        /// <param name="context"></param>
        void RandowSign(HttpContext context)
        {
            context.Response.AddHeader("Access-Control-Allow-Origin", _AllowOrigin);
            string _Res = "";
            try
            {
                string _ContentId = context.Request.Params["contid"];
                WeTourSignDll.instance.RandomSign(_ContentId);
                _Res = "已排布签表";
            }
            catch (Exception e)
            {
                _Res = "{\"code\":1,errormsg:\""+e.ToString().Substring(0,100)+"\"}";
            }
            context.Response.Write(_Res);
        }

        /// <summary>
        /// 获取项目签表
        /// </summary>
        /// <param name="context"></param>
        void GetContentSign(HttpContext context)
        {
            context.Response.AddHeader("Access-Control-Allow-Origin", _AllowOrigin);
            string _Res = "";
            try
            {
                string _Contid=context.Request.Params["contid"];
                List<Model_SignedApp> list = WeTourSignDll.instance.Tgm_GetSignApp(_Contid);
                string json = JsonHelper.ToJson(list);
                _Res = "{code:0,data:"+json+"}";
            }
            catch (Exception e)
            {
                _Res = "{\"code\":1,errormsg:\"" + e.ToString().Substring(0, 100) + "\"}";
            }
            context.Response.Write(_Res);
        }

        /// <summary>
        /// 获取未分配至签表的报名名单
        /// </summary>
        /// <param name="context"></param>
        void GetUnSignedApp(HttpContext context)
        {
            context.Response.AddHeader("Access-Control-Allow-Origin", _AllowOrigin);
            string _Res = "";
            try
            {
                string _ContentId = context.Request.Params["contid"];
                List<Model_SignMember> list=WeTourSignDll.instance.GetUnSignedApp(_ContentId);
                string json = JsonHelper.ToJson(list);
                _Res = "{code:0,data:" + json + "}";
            }
            catch (Exception e)
            {
                _Res = "{\"code\":1,errormsg:\"" + e.ToString().Substring(0, 100) + "\"}";
            }
            context.Response.Write(_Res);
        }

        /// <summary>
        /// 保存修改后的签表
        /// </summary>
        /// <param name="context"></param>
        void UpdateContentSign(HttpContext context)
        {
            context.Response.AddHeader("Access-Control-Allow-Origin", _AllowOrigin);
            string _Res = "";
            try
            {
                string _SignApp = context.Request.Params["signapp"];
                string _ContentId = context.Request.Params["contid"];
                List<Model_SignedApp> list = JsonHelper.ParseFormJson<List<Model_SignedApp>>(_SignApp);
                WeTourSignDll.instance.Tgm_UpdateContSign(list, _ContentId);
                _Res = "{\"code\":0,errormsg:\"更新成功\"}";
            }
            catch (Exception e)
            {
                _Res = "{\"code\":1,errormsg:\"" + e.ToString().Substring(0, 100) + "\"}";
            }
            context.Response.Write(_Res);
        }

        /// <summary>
        /// 生成项目的比赛
        /// </summary>
        /// <param name="context"></param>
        void GenerateContentMatch(HttpContext context)
        {
            context.Response.AddHeader("Access-Control-Allow-Origin", _AllowOrigin);
            string _Res = "";
            try
            {
                string _ContentId = context.Request.Params["contid"];
                WeTourDll.instance.AddTourMatchbyContent(_ContentId);
                _Res = "{\"code\":0,errormsg:\"更新成功\"}";
            }
            catch (Exception e)
            {
                _Res = "{\"code\":1,errormsg:\"" + e.ToString().Substring(0, 100) + "\"}";
            }
            context.Response.Write(_Res);
        }

        /// <summary>
        /// 加载签表的比赛
        /// </summary>
        /// <param name="context"></param>
        void GetContentMatches(HttpContext context)
        {
            context.Response.AddHeader("Access-Control-Allow-Origin", _AllowOrigin);
            string _Res = "";
            try
            {
                string _ContentId = context.Request.Params["contid"];
                List<Model_TgmMatch> listM = WeMatchDll.instance.GetMatchesByContentState(_ContentId);
                string json = JsonHelper.ToJson(listM);
                _Res = "{\"code\":0,data:" + json + "}";
            }
            catch (Exception e)
            {
                _Res = "{\"code\":1,errormsg:\"" + e.ToString().Substring(50, 150) + "\"}";
            }
            context.Response.Write(_Res);
        }

       
        #endregion 

        #region Distribution
                 #region Dist_Gym
        /// <summary>
        /// 获取场馆
        /// </summary>
        /// <param name="context"></param>
        void GetMyGyms(HttpContext context)
        {
            context.Response.AddHeader("Access-Control-Allow-Origin", _AllowOrigin);
            string _Res = "";
            try
            {
                string _ClubSys = context.Request.Params["clubsys"];
                List<Model_Dist_Gyms> list = Biz_ClubSys.instance.GetGymsByClub(_ClubSys);
                string json = JsonHelper.ToJson(list);
                _Res = "{code:0,data:" + json + "}";
            }
            catch (Exception e)
            {
                _Res = "{\"code\":1,errormsg:\"" + e.ToString().Substring(0, 100) + "\"}";
            }
            context.Response.Write(_Res);
        }
        /// <summary>
        /// 根据赛事主键获取场馆主键
        /// </summary>
        /// <param name="context"></param>
        void GetGymsbyToursys(HttpContext context)
        {
            context.Response.AddHeader("Access-Control-Allow-Origin", _AllowOrigin);
            string _Res = "";
            try
            {
                string _Toursys = context.Request.Params["toursys"];
                List<TourGymList> list = Biz_TourGyms.instance.GetTourGyms(_Toursys);
                string json = JsonHelper.ToJson(list);
                _Res = "{code:0,data:" + json + "}";
            }
            catch (Exception e)
            {
                _Res = "{\"code\":1,errormsg:\"" + e.ToString().Substring(0, 100) + "\"}";
            }
            context.Response.Write(_Res);
        }

        /// <summary>
        /// 为赛事添加Gym
        /// </summary>
        /// <param name="context"></param>
        void AddGymToTour(HttpContext context)
        {
            context.Response.AddHeader("Access-Control-Allow-Origin", _AllowOrigin);
            string _Res = "";
            try
            {
                string _Toursys = context.Request.Params["toursys"];
                string _Gym=context.Request.Params["gymsys"];
                Model_TourGym model = new Model_TourGym();
                model.TourSys = _Toursys;
                model.GymSys = _Gym;
                if (Biz_TourGyms.instance.AddNewGym(model))
                {
                    _Res = "{code:0,status:0}";
                }
                else
                {
                    _Res = "{code:0,status:1}";
                }
               
            }
            catch (Exception e)
            {
                _Res = "{code:1,errormsg:\"" + e.ToString().Substring(0, 100) + "\"}";
            }
            context.Response.Write(_Res);
        }

        /// <summary>
        /// 删除赛事绑定的场馆
        /// </summary>
        /// <param name="context"></param>
        void DeleteTourGym(HttpContext context)
        {
            context.Response.AddHeader("Access-Control-Allow-Origin", _AllowOrigin);
            string _Res = "";
            try
            {
                string _Toursys = context.Request.Params["toursys"];
                string _Gym=context.Request.Params["gymsys"];
                Biz_TourGyms.instance.DeleteTourGym(_Toursys, _Gym);
                _Res = "{code:0,data:}";
            }
            catch (Exception e)
            {
                _Res = "{code:1,errormsg:\"" + e.ToString().Substring(0, 100) + "\"}";
            }
            context.Response.Write(_Res);
        }

        /// <summary>
        /// 获取场地
        /// </summary>
        /// <param name="context"></param>
        void GetTourCourt(HttpContext context)
        {
            context.Response.AddHeader("Access-Control-Allow-Origin", _AllowOrigin);
            string _Res = "";
            try
            {
                string _Toursys = context.Request.Params["toursys"];
                string _Gym = context.Request.Params["gymsys"];
               List<Model_Dist_GymCourts> list= Biz_TourGyms.instance.GetTourGymCourts(_Toursys, _Gym);
               string json = JsonHelper.ToJson(list);
                _Res = "{code:0,data:"+json+"}";
            }
            catch (Exception e)
            {
                _Res = "{code:1,errormsg:\"" + e.ToString().Substring(0, 100) + "\"}";
            }
            context.Response.Write(_Res);
        }

        /// <summary>
        /// 修改赛事的场馆场地绑定
        /// </summary>
        /// <param name="context"></param>
        void UpdateTourGymCourts(HttpContext context)
        {
            context.Response.AddHeader("Access-Control-Allow-Origin", _AllowOrigin);
            string _Res = "";
            try
            {
                string _Toursys = context.Request.Params["toursys"];
                string _Gym = context.Request.Params["gymsys"];
                string _courtstr = context.Request.Params["courtstr"];

                List<Model_Dist_GymCourts> list = JsonHelper.ParseFormJson<List<Model_Dist_GymCourts>>(_courtstr);
                Biz_TourGyms.instance.UpdateTourGymCourts(_Toursys, _Gym,list);

                _Res = "{code:0,data:}";
            }
            catch (Exception e)
            {
                _Res = "{code:1,errormsg:\"" + e.ToString().Substring(0, 100) + "\"}";
            }
            context.Response.Write(_Res);
        }

        void AddDistriGymCont(HttpContext context)
        {
            context.Response.AddHeader("Access-Control-Allow-Origin", _AllowOrigin);
            string _Res = "";
            try
            {
                string _Toursys = context.Request.Params["toursys"];
                string _Gym = context.Request.Params["gymsys"];
                string _ContId = context.Request.Params["contid"];
                Model_GymContents model = new Model_GymContents();
                model.TourSys = _Toursys;
                model.GymSys = _Gym;
                model.ContId = _ContId;
                Biz_TourGyms.instance.AddNewGymCont(model);
                _Res = "{code:0,data:}";
            }
            catch (Exception e)
            {
                _Res = "{code:1,errormsg:\"" + e.ToString().Substring(0, 100) + "\"}";
            }
            context.Response.Write(_Res);
        }

        void LoadGymConts(HttpContext context)
        {
            context.Response.AddHeader("Access-Control-Allow-Origin", _AllowOrigin);
            string _Res = "";
            try
            {
                string _Toursys = context.Request.Params["toursys"];
                string _Gym = context.Request.Params["gymsys"];
                List<Model_Dist_GymCont> list = Biz_TourGyms.instance.GetGymContent(_Toursys, _Gym);
                string str = JsonHelper.ToJson(list);
                _Res = "{code:0,data:"+str+"}";
            }
            catch (Exception e)
            {
                _Res = "{code:1,errormsg:\"" + e.ToString().Substring(0, 100) + "\"}";
            }
            context.Response.Write(_Res);
        }

        void DeleteContid(HttpContext context)
        {
            context.Response.AddHeader("Access-Control-Allow-Origin", _AllowOrigin);
            string _Res = "";
            try
            {
                string _Toursys = context.Request.Params["toursys"];
                string _Gym = context.Request.Params["gymsys"];
                string _ContId = context.Request.Params["contid"];
                Biz_TourGyms.instance.DeleteGymCont(_Toursys, _Gym, _ContId);
                _Res = "{code:0,data:}";
            }
            catch (Exception e)
            {
                _Res = "{code:1,errormsg:\"" + e.ToString().Substring(0, 100) + "\"}";
            }
            context.Response.Write(_Res);
        }
        #endregion

                 #region Distri_Date
        //添加比赛日期
        void AddTour_Date(HttpContext context)
        {
            context.Response.AddHeader("Access-Control-Allow-Origin", _AllowOrigin);
            string _Res = "";
            try
            {
                string _TourSys = context.Request.Params["toursys"];
                string _Date = context.Request.Params["date"];

                Model_TourDate model = new Model_TourDate();
                model.TourSys = _TourSys;
                model.TourDate = _Date;

                Biz_TourDate.instance.AddTourDate(model);
                _Res = "{code:0,data:}";
            }
            catch (Exception e)
            {
                _Res = "{\"code\":1,errormsg:\"" + e.ToString().Substring(0, 100) + "\"}";
            }
            context.Response.Write(_Res);
        }

        void Delete_TourDate(HttpContext context)
        {
            context.Response.AddHeader("Access-Control-Allow-Origin", _AllowOrigin);
            string _Res = "";
            try
            {
                string _Id = context.Request.Params["id"];

                Biz_TourDate.instance.DeleteTourDate(_Id);
                _Res = "{code:0,data:}";
            }
            catch (Exception e)
            {
                _Res = "{\"code\":1,errormsg:\"" + e.ToString().Substring(0, 100) + "\"}";
            }
            context.Response.Write(_Res);
        }

        void AddTour_DateRound(HttpContext context)
        {
            context.Response.AddHeader("Access-Control-Allow-Origin", _AllowOrigin);
            string _Res = "";
            try
            {
                string _ContentId = context.Request.Params["contid"];
                Model_TourDateRound model = new Model_TourDateRound();

                Biz_TourDate.instance.AddTourDateRound(model);
                _Res = "{code:0,data:}";
            }
            catch (Exception e)
            {
                _Res = "{\"code\":1,errormsg:\"" + e.ToString().Substring(0, 100) + "\"}";
            }
            context.Response.Write(_Res);
        }

        /// <summary>
        /// 加载赛事分配的日期资源
        /// </summary>
        /// <param name="context"></param>
        void GetTourDate(HttpContext context)
        {
            context.Response.AddHeader("Access-Control-Allow-Origin", _AllowOrigin);
            string _Res = "";
            try
            {
                string _TourSys = context.Request.Params["toursys"];
               
                List<Model_TourDate> list = Biz_TourDate.instance.GetTourDate(_TourSys);
               string str = JsonHelper.ToJson(list);

               _Res = "{code:0,data:" + str + "}";
            }
            catch (Exception e)
            {
                _Res = "{\"code\":1,errormsg:\"" + e.ToString().Substring(0, 100) + "\"}";
            }
            context.Response.Write(_Res);
        }

        /// <summary>
        /// 获取日期轮次
        /// </summary>
        /// <param name="context"></param>
        void GetDateRounds(HttpContext context)
        {
            context.Response.AddHeader("Access-Control-Allow-Origin", _AllowOrigin);
            string _Res = "";
            try
            {
                string _TourSys = context.Request.Params["toursys"];
                string _TourDate = context.Request.Params["tourdate"];

                List<Model_DistriTDR> list = Biz_TourDate.instance.GetDisTourDate(_TourSys, _TourDate);
                string str = JsonHelper.ToJson(list);

                _Res = "{code:0,data:" + str + "}";
            }
            catch (Exception e)
            {
                _Res = "{\"code\":1,errormsg:\"" + e.ToString().Substring(0, 100) + "\"}";
            }
            context.Response.Write(_Res);
        }

        /// <summary>
        /// 提交toursignround
        /// </summary>
        /// <param name="context"></param>
        void SubmitContRound(HttpContext context)
        {
            context.Response.AddHeader("Access-Control-Allow-Origin", _AllowOrigin);
            string _Res = "";
            try
            {
                string _TourSys = context.Request.Params["toursys"];
                string _TourDate = context.Request.Params["tourdate"];
                string _RoundSign = context.Request.Params["rsstr"];
                
                List<Model_DistriTDR> list = JsonHelper.ParseFormJson<List<Model_DistriTDR>>(_RoundSign);
               // DbHelperSQL.WriteLog("SubmitContRound", "提交日期轮次指派时报错" + list.Count);
                Biz_TourDate.instance.UpdateTourSignRound(_TourSys, _TourDate, list);
                _Res = "{code:0,data:}";
            }
            catch (Exception e)
            {
                _Res = "{\"code\":1,errormsg:\"" + e.ToString().Substring(0, 100) + "\"}";
            }
            context.Response.Write(_Res);
        }



         #endregion

        #region Distri_Algorithm
        void RandomDistributeRes(HttpContext context)
        {
            context.Response.AddHeader("Access-Control-Allow-Origin", _AllowOrigin);
            string _Res = "";
            try
            {
                string _TourSys = context.Request.Params["toursys"];
                TourResDisDll.instance.DistributeTourRes(_TourSys);
                _Res = "{code:0,data:}";
            }
            catch (Exception e)
            {
                _Res = "{\"code\":1,errormsg:\"" + e.ToString().Substring(0, 100) + "\"}";
            }
            context.Response.Write(_Res);
        }
        #endregion

            #region Distri_CourtMatches

        /// <summary>
        /// 展示赛程列表内容
        /// </summary>
        /// <param name="context"></param>
        void LoadCourtMatches(HttpContext context)
        {
            context.Response.AddHeader("Access-Control-Allow-Origin", _AllowOrigin);
            string _Res = "";
            try
            {
                string _TourSys = context.Request.Params["toursys"];
                List<Model_CourtMatches> list = WeMatchDll.instance.GetCourtMatchs(_TourSys);
                string jsonstr = JsonHelper.ToJson(list);
                _Res = "{code:0,data:"+jsonstr+"}";
            }
            catch (Exception e)
            {
                _Res = "{\"code\":1,errormsg:\"" + e.ToString().Substring(0, 200) + "\"}";
            }
            context.Response.Write(_Res);
        }
        #endregion
        
            #region Distri_ContentScore
        /// <summary>
        /// 获取轮次奖励分数
        /// </summary>
        /// <param name="context"></param>
        void ScoreSet_Get(HttpContext context)
        {
            context.Response.AddHeader("Access-Control-Allow-Origin", _AllowOrigin);
            string _Res = "";
            try
            {
                string _TourSys = context.Request.Params["toursys"];

                List<PageS_ScoreSetting> list = Biz_TourContentScore.instance.GetRoundScore(_TourSys) ;
                string json = JsonHelper.ToJson(list);
                _Res = "{code:0,data:" + json + "}";
            }
            catch (Exception e)
            {
                _Res = "{\"code\":1,errormsg:\"" + e.ToString().Substring(0, 100) + "\"}";
            }
            context.Response.Write(_Res);
        }

        /// <summary>
        /// 更新分数
        /// </summary>
        /// <param name="context"></param>
        void ScoreSet_Update(HttpContext context)
        {
            context.Response.AddHeader("Access-Control-Allow-Origin", _AllowOrigin);
            string _Res = "";
            try
            {
                string _TourSys = context.Request.Params["toursys"];
                string _Scorelist = context.Request.Params["jsonstr"];
                List<PageS_ScoreSetting> list = JsonHelper.ParseFormJson<List<PageS_ScoreSetting>>(_Scorelist);
                Biz_TourContentScore.instance.UpdateContentScore(list, _TourSys);
                _Res = "{code:0,data:}";
            }
            catch (Exception e)
            {
                _Res = "{\"code\":1,errormsg:\"" + e.ToString().Substring(0, 100) + "\"}";
            }
            context.Response.Write(_Res);
        }
            #endregion

        #region Distri_Resource
        /// <summary>
        /// 分配赛事资源
        /// </summary>
        /// <param name="context"></param>
        void Dis_TourResource(HttpContext context)
        {
            context.Response.AddHeader("Access-Control-Allow-Origin", _AllowOrigin);
            Model_Return ret = new Model_Return();
            try
            {
                string _TourSys = context.Request.Params["toursys"];
                
                //updatewinto
                WeTourDll.instance.UpdateTourLogic(_TourSys);

                //修改资源分配

                TourResDisDll.instance.DistributeTourRes(_TourSys);
                //ResTourDistridll.instance.DistributOne(_TourSys);
                ret.code = 0;
                ret.errorMsg = "";
                ret.data = "分配成功";
            }
            catch (Exception e)
            {
                ret.code = 1;
                ret.errorMsg = e.ToString();
            }
            context.Response.Write(JsonHelper.ToJson(ret));
        }
        #endregion

        #endregion

        #region TourRanking
        /// <summary>
        /// 获取联盟排名
        /// </summary>
        /// <param name="context"></param>
        void GetUnionRank(HttpContext context)
        {
            context.Response.AddHeader("Access-Control-Allow-Origin", _AllowOrigin);
            string _Res = "";
            try
            {
                string _UnionSys = context.Request.Params["unionsys"];
                //DbHelperSQL.WriteLog("GetUnionRank", _UnionSys);
                List<Model_GroupType> list1 = RankDll.instance.GetUnionRankGroups(_UnionSys);
                string _GroupType = JsonHelper.ToJson(list1);
                List<Model_GroupDetail> list2 = RankDll.instance.GetUnionGroupDetail(_UnionSys);
                string _MemRank = JsonHelper.ToJson(list2);
                _Res = "{code:0,errormsg:\"\",GroupType:" + _GroupType + ",GroupDetail:"+_MemRank+"}";
            }
            catch (Exception e)
            {
                _Res = "{\"code\":1,errormsg:\"" + e.ToString().Substring(0, 100) + "\"}";
            }
            context.Response.Write(_Res);
        }

        /// <summary>
        /// 更新联盟排名
        /// </summary>
        /// <param name="context"></param>
        void UpdateUnionRanking(HttpContext context)
        {
            context.Response.AddHeader("Access-Control-Allow-Origin", _AllowOrigin);
            string _Res = "";
            try
            {
                string _UnionSys=context.Request.Params["unionsys"];
                RankDll.instance.UpdateUnionRanking(_UnionSys);
                _Res = "{code:0,errormsg:\"\"}";
            }
            catch (Exception e)
            {
                _Res = "{code:1,errormsg:\""+e.ToString().Substring(0,100)+"\"}";
            }
            context.Response.Write(_Res);
        }
#endregion

        #region MatchMaintain
        /// <summary>
        /// 获取项目轮次
        /// </summary>
        /// <param name="context"></param>
        void GetContentRounds(HttpContext context)
        {
            context.Response.AddHeader("Access-Control-Allow-Origin", _AllowOrigin);
            Model_Return ret = new Model_Return();
            try
            {
                ret.code = 0;
                ret.errorMsg = "";
                string _contId = context.Request.Params["cont"];
                ret.data = WeTourContentDll.instance.GetcontentRounds(_contId);
            }
            catch (Exception e)
            {
                ret.code = 1;
                ret.errorMsg = e.ToString();
            }
            context.Response.Write(JsonHelper.ToJson(ret));
        }

        /// <summary>
        /// 根据项目id，及round获取比赛信息
        /// </summary>
        /// <param name="context"></param>
        void GetMatchesByContRound(HttpContext context)
        {
            context.Response.AddHeader("Access-Control-Allow-Origin", _AllowOrigin);
            Model_Return ret = new Model_Return();
            try
            {
                ret.code = 0;
                ret.errorMsg = "";
                string _contId = context.Request.Params["cont"];
                string _round = context.Request.Params["round"];
                if (string.IsNullOrEmpty(_round))
                {
                    _round = WeTourContentDll.instance.GetLatestRound(_contId);//获取当前round
                }
                ret.data = WeMatchDll.instance.GetMatchlistbyContRound(_contId,_round);
            }
            catch (Exception e)
            {
                ret.code = 1;
                ret.errorMsg = e.ToString();
            }
            context.Response.Write(JsonHelper.ToJson(ret)); 
        }

        /// <summary>
        /// 记录比赛结果
        /// 2016-9-26
        /// </summary>
        /// <param name="context"></param>
        /// <param name="jsonstr"></param>
        void RecordMatchRes(HttpContext context,string jsonstr)
        {
            context.Response.AddHeader("Access-Control-Allow-Origin", _AllowOrigin);
            Model_Return ret = new Model_Return();
            try
            {
                ret.code = 0;
                ret.errorMsg = "";
                Model_req_MatchScore req = new Model_req_MatchScore();
                req = JsonHelper.ParseFormJson<Model_req_MatchScore>(jsonstr);

                //更新比分
                WeMatchModel match = WeMatchDll.instance.GetModel(req.sys);

                match.SCORE = req.p1s + req.p2s;
                int scoreP1 = Convert.ToInt32(req.p1s);
                int scoreP2 = Convert.ToInt32(req.p2s);
                if (scoreP1 > scoreP2)
                {
                    match.WINNER = match.PLAYER1;
                    match.LOSER = match.PLAYER2;
                }
                else
                {
                    match.LOSER = match.PLAYER1;
                    match.WINNER = match.PLAYER2;
                }

                match.STATE = 2;

                WeMatchDll.instance.RecoreMatchScore(match);

                ret.data = "更新成功";

            }
            catch (Exception e)
            {
                ret.code = 1;
                ret.errorMsg = e.ToString();
            }

            context.Response.Write(JsonHelper.ToJson(ret));
        }


        void RecordMatchRes2(HttpContext context)
        {
            context.Response.AddHeader("Access-Control-Allow-Origin", _AllowOrigin);
            Model_Return ret = new Model_Return();
            try
            {
                ret.code = 0;
                ret.errorMsg = "";
                string sys = context.Request.Params["sys"];
                string p1s = context.Request.Params["p1s"];
                string p2s = context.Request.Params["p2s"];
                WeMatchDll.instance.RecoreMatchScore2(sys,p1s,p2s);

                ret.data = "更新成功";

            }
            catch (Exception e)
            {
                ret.code = 1;
                ret.errorMsg = e.ToString();
            }

            context.Response.Write(JsonHelper.ToJson(ret));
        }

        void ComputeTourScore(HttpContext context)
        { 
        
        }

        /// <summary>
        /// 结束赛事，计算赛事积分，更新排名
        /// </summary>
        /// <param name="context"></param>
        void endTour(HttpContext context)
        {
            context.Response.AddHeader("Access-Control-Allow-Origin", _AllowOrigin);
            Model_Return ret = new Model_Return();
            try
            {
                ret.code = 0;
                ret.errorMsg = "";
                string sys = context.Request.Params["toursys"];
                Biz_TourScore.instance.UpdateScore(sys);

                ret.data = "更新成功";

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