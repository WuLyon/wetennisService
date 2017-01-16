using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;

namespace WeTour
{
    public class ResTourDistridll
    {
        public static ResTourDistridll instance = new ResTourDistridll();

        /// <summary>
        /// 赛事资源分配方案一
        /// </summary>
        /// <param name="_Toursys"></param>
        public void DistributOne(string _Toursys)
        { 
            //根据赛事主键获得赛事日期资源
            List<ResDate_TourDateModel> TourDateList = ResDateDll.instance.GetTourDatelist(_Toursys);

            List<Model_TourDate> list = Biz_TourDate.instance.GetTourDate(_Toursys);
            foreach (Model_TourDate date in list)
            {
                ResDate_TourDateModel ndate = new ResDate_TourDateModel();
                ndate.DATE = date.TourDate;
                TourDateList.Add(ndate);                   
            }
            
            if (TourDateList.Count > 0)
            {
                //分配赛事日期资源
                foreach (ResDate_TourDateModel model in TourDateList)
                {
                    //按顺序分配某一天的赛事日期资源
                    //--根据日期获得当天的赛事项目及轮次.修改对应比赛的日期资源 StartDate
                    List<ResDate_ContentRoundModel> ContentRoundlist = ResDateDll.instance.GetContentRoundlist(model.ID);
                    if (ContentRoundlist.Count > 0)
                    {
                        //已指定赛事日期资源
                        foreach (ResDate_ContentRoundModel rmodel in ContentRoundlist)
                        {
                            UpdateDatebyContentRound(rmodel.CONTENTID, rmodel.ROUND, model.DATE);
                        }
                    }
                    else
                    {
                        //未指定赛事日期资源，默认所有的比赛在当天完成。
                        UpdateDatebyTour(_Toursys, model.DATE);
                        
                    }//end of content resource if

                    //修改指定日期的赛事资源
                    DistributeTourRes(_Toursys, model.DATE);
                }//end of foreach tour date
            }//end of tourdate if
            else
            { 
                //赛事未分配日期资源
            }
        }//end of funciton

        #region 修改赛事日期资源的方法
        /// <summary>
        /// 根据赛事主键修改所有赛事的日期
        /// </summary>
        /// <param name="_Toursys"></param>
        private void UpdateDatebyTour(string _Toursys,string _Date)
        {
            string sql = "update wtf_match set matchdate='" + _Date + "' where toursys='" + _Toursys + "'";
            int a=DbHelperSQL.ExecuteSql(sql);            
        }

        private void UpdateDatebyContentRound(string _Contentid, string _Round,string _Date)
        {
            string sql = string.Format("update wtf_match set matchdate='{0}' where contentid='{1}' and round='{2}'",_Date,_Contentid,_Round);
            int a = DbHelperSQL.ExecuteSql(sql);
        }
        #endregion

        /// <summary>
        /// 根据日期资源，场地资源来分配比赛资源
        /// </summary>
        /// <param name="_Toursys"></param>
        /// <param name="_Date"></param>
        /// <param name="CourtList"></param>
        private void DistributeTourRes(string _Toursys, string _Date)
        {
            //清空指定日期的资源分配情况
            DeleteResourceDis(_Toursys, _Date);

            //--获取赛事场地资源
            //List<ResTour_TourGymModel> TourGymList = ResGymDll.instance.GetTourGymList(_Toursys);

            List<TourGymList> TourGymList = Biz_TourGyms.instance.GetTourGyms(_Toursys);
            if (TourGymList.Count > 0)
            {
                //1判断赛事是否分场馆进行,并修改gymid信息
                if (TourGymList.Count == 1)
                {
                    //赛事全部在一个场馆进行
                    updateMatchGymidbyTourys(_Toursys, TourGymList[0].gymSys);

                }
                else
                {
                    //赛事已分配场地资源
                    List<ResTour_GymContentModel> ContentGymlist = ResGymDll.instance.GetGymContentList(_Toursys);
                    foreach (ResTour_GymContentModel cmodel in ContentGymlist)
                    {
                        updatematchgymbyContentid(cmodel.CONTENTID, cmodel.GYMID);
                    }
                }
                //2.按照日期，场馆，来分配比赛的场地资源及顺序
                foreach (TourGymList gymmodel in TourGymList)
                {
                    //分配方案一。按照轮次，将同时进行的比赛填充到场地。
                    //DistriTourbyDateGym(_Toursys, gymmodel.GYMID, _Date);

                    //分配方案二，按照各个轮次的比赛数量情况，将场地分配资源写入分配方案表，之后，再按照资源分配方案表来进行分配。
                    DistributCourtStratagy(_Toursys, gymmodel.gymSys, _Date);
                }
            }
            else
            {
                //赛事未分配场地资源
            }//end of tour gym if

            
            //根据指定日期，获取比赛的轮次

        }

        #region 修改比赛场馆信息
        /// <summary>
        /// 修改比赛的体育场馆信息
        /// </summary>
        /// <param name="_Toursys"></param>
        /// <param name="_Gymid"></param>
        private void updateMatchGymidbyTourys(string _Toursys, string _Gymid)
        {
            string sql = "update wtf_match set etc4='"+_Gymid+"' where toursys='"+_Toursys+"'";
            int a = DbHelperSQL.ExecuteSql(sql);
        }

        /// <summary>
        /// 根据项目修改gymid
        /// </summary>
        /// <param name="_ContentId"></param>
        /// <param name="_Gymid"></param>
        private void updatematchgymbyContentid(string _ContentId, string _Gymid)
        {
            string sql = "update wtf_match set etc4='" + _Gymid + "' where contentid='" + _ContentId + "'";
            int a = DbHelperSQL.ExecuteSql(sql);
        }


        #endregion


        private void DeleteResourceDis(string _Toursys, string _Date)
        {
            string sql = "update wtf_match set place='',courtid='',etc4='' where toursys='" + _Toursys + "' and matchdate='" + _Date + "'";
            int a=DbHelperSQL.ExecuteSql(sql);
        }

        /// <summary>
        /// 分配日期场馆资源
        /// </summary>
        /// <param name="_Toursys"></param>
        /// <param name="_Gymid"></param>
        /// <param name="_Date"></param>
        private void DistriTourbyDateGym(string _Toursys, string _Gymid, string _Date)
        {
            //获取体育馆的场地信息
            List<ResTour_TourCourtModel> courtlist = ResGymDll.instance.GetTourCourtList(_Toursys, _Gymid);
            int _PlaceOrder = 1;//默认从第一场开始排序
            int _Courtorder = 0;//默认从第一片场地开始分

            //获得要分配的比赛属于哪些项目：单打，双打，混双。
            if(1==1)//是否兼项，若允许兼项，则需要开启按顺序
            {
                List<WeMatchModel> list = GetMatchContent(_Toursys, _Gymid, _Date);
                string _SinContents="";//单打项目
                string _DouContents="";//双打项目
                string _MixContnets="";//混合双打
                //将项目分类
                foreach (WeMatchModel model in list)
                {
                    if (model.ContentName=="男单"||model.ContentName=="女单")
                    {
                        _SinContents += ",'" + model.ContentID+"'";
                    }
                    else if (model.ContentName == "男双" || model.ContentName == "女双")
                    {
                        _DouContents += ",'"+model.ContentID+"'";
                    }
                    else if (model.ContentName == "混双")
                    {
                        _MixContnets += ",'" + model.ContentID+"'";
                    }
                }
                //先排单打
                #region 分配单打
                if (_SinContents.Length > 0)
                {
                    //指定日期内包含单打的项目
                    //--处理项目清单
                    _SinContents = _SinContents.TrimStart(',');
                    //--获得当天，这些项目要打的轮次数
                    List<WeMatchModel> Rlist = GetMatchRoundbyContent(_Date, _SinContents);
                    foreach (WeMatchModel rmodel in Rlist)
                    {
                        //分轮次来分配场地资源
                        if (rmodel.ROUND == 0)
                        {
                            //分配小组赛
                            //--查询小组赛的轮次
                            List<WeMatchModel> grlist = GetMatchesGroupRound(_Date, _SinContents);
                            for (int i = 0; i < grlist.Count; i++)
                            {
                                //获取本轮小组的比赛
                                List<WeMatchModel> rglist = GetMatchesbyGroupRound(_Date, _SinContents, (i + 1).ToString());
                                foreach (WeMatchModel rgmodel in rglist)
                                {
                                    //修改比赛的场地及顺序
                                    string courtid = courtlist[_Courtorder].COURTID;
                                    string place = _PlaceOrder.ToString();
                                    UpdateMatchCourtOrder(rgmodel.SYS, place, courtid);

                                    //修改顺序
                                    _Courtorder += 1;
                                    if (_Courtorder > courtlist.Count)
                                    {
                                        _Courtorder = 0;
                                        _PlaceOrder += 1;
                                    }
                                }
                            }
                        }
                        else
                        {
                            //分配淘汰赛
                            List<WeMatchModel> knlist = GetMatchesbyRound(_Date, _SinContents, rmodel.ROUND.ToString());
                            foreach (WeMatchModel rgmodel in knlist)
                            {
                                //修改比赛的场地及顺序
                                string courtid = courtlist[_Courtorder].COURTID;
                                string place = _PlaceOrder.ToString();
                                UpdateMatchCourtOrder(rgmodel.SYS, place, courtid);

                                _Courtorder += 1;
                                if (_Courtorder > courtlist.Count)
                                {
                                    _Courtorder = 0;
                                    _PlaceOrder += 1;
                                }
                            }
                        }
                    }//end of foreach
                }//end of if single
                #endregion 

                //排双打
                #region 分配单打
                if (_DouContents.Length > 0)
                { 
                    //指定日期内包含单打的项目
                    //--处理项目清单
                    _DouContents = _DouContents.TrimStart(',');
                   //--获得当天，这些项目要打的轮次数
                    List<WeMatchModel> DRlist = GetMatchRoundbyContent(_Date, _DouContents);
                    foreach (WeMatchModel rmodel in DRlist)
                    { 
                        //分轮次来分配场地资源
                        if (rmodel.ROUND == 0)
                        {
                            //分配小组赛
                            //--查询小组赛的轮次
                            List<WeMatchModel> grlist = GetMatchesGroupRound(_Date, _DouContents);
                            for (int i = 0; i < grlist.Count; i++)
                            { 
                                //获取本轮小组的比赛
                                List<WeMatchModel> rglist = GetMatchesbyGroupRound(_Date, _DouContents, (i + 1).ToString());
                                foreach(WeMatchModel rgmodel in rglist)
                                {
                                    //修改比赛的场地及顺序
                                    string courtid = courtlist[_Courtorder].COURTID;
                                    string place = _PlaceOrder.ToString();                                    
                                    UpdateMatchCourtOrder(rgmodel.SYS, place, courtid);

                                    _Courtorder+=1;
                                    if (_Courtorder > courtlist.Count)
                                    {
                                        _Courtorder = 0;
                                        _PlaceOrder += 1;
                                    }
                                }
                            }
                        }
                        else
                        { 
                            //分配淘汰赛
                            List<WeMatchModel> knlist = GetMatchesbyRound(_Date, _DouContents, rmodel.ROUND.ToString());
                            foreach (WeMatchModel rgmodel in knlist)
                            {
                                //修改比赛的场地及顺序
                                string courtid = courtlist[_Courtorder].COURTID;
                                string place = _PlaceOrder.ToString();
                                UpdateMatchCourtOrder(rgmodel.SYS, place, courtid);

                                _Courtorder += 1;
                                if (_Courtorder > courtlist.Count)
                                {
                                    _Courtorder = 0;
                                    _PlaceOrder += 1;
                                }
                            }
                        }
                    }//end of foreach                
                }//end if double
                #endregion 
                //排混双
                #region 分配单打
                if (_DouContents.Length > 0)
                {
                    //指定日期内包含单打的项目
                    //--处理项目清单
                    _MixContnets = _MixContnets.TrimStart(',');
                    //--获得当天，这些项目要打的轮次数
                    List<WeMatchModel> DRlist = GetMatchRoundbyContent(_Date, _MixContnets);
                    foreach (WeMatchModel rmodel in DRlist)
                    {
                        //分轮次来分配场地资源
                        if (rmodel.ROUND == 0)
                        {
                            //分配小组赛
                            //--查询小组赛的轮次
                            List<WeMatchModel> grlist = GetMatchesGroupRound(_Date, _MixContnets);
                            for (int i = 0; i < grlist.Count; i++)
                            {
                                //获取本轮小组的比赛
                                List<WeMatchModel> rglist = GetMatchesbyGroupRound(_Date, _MixContnets, (i + 1).ToString());
                                foreach (WeMatchModel rgmodel in rglist)
                                {
                                    //修改比赛的场地及顺序
                                    string courtid = courtlist[_Courtorder].COURTID;
                                    string place = _PlaceOrder.ToString();
                                    UpdateMatchCourtOrder(rgmodel.SYS, place, courtid);

                                    _Courtorder += 1;
                                    if (_Courtorder > courtlist.Count)
                                    {
                                        _Courtorder = 0;
                                        _PlaceOrder += 1;
                                    }
                                }
                            }
                        }
                        else
                        {
                            //分配淘汰赛
                            List<WeMatchModel> knlist = GetMatchesbyRound(_Date, _MixContnets, rmodel.ROUND.ToString());
                            foreach (WeMatchModel rgmodel in knlist)
                            {
                                //修改比赛的场地及顺序
                                string courtid = courtlist[_Courtorder].COURTID;
                                string place = _PlaceOrder.ToString();
                                UpdateMatchCourtOrder(rgmodel.SYS, place, courtid);

                                _Courtorder += 1;
                                if (_Courtorder > courtlist.Count)
                                {
                                    _Courtorder = 0;
                                    _PlaceOrder += 1;
                                }
                            }
                        }
                    }//end of foreach                
                }//end if double
                #endregion 
            }
        }
        #region 分配场地
        /// <summary>
        /// 根据赛事主键，场馆id，比赛日期，获取赛事类型
        /// </summary>
        /// <param name="_Toursys"></param>
        /// <param name="_Gymid"></param>
        /// <param name="_Date"></param>
        /// <returns></returns>
        private List<WeMatchModel> GetMatchContent(string _Toursys, string _Gymid, string _Date)
        {
            List<WeMatchModel> list = new List<WeMatchModel>();
            string sql = "select distinct(contentid) from wtf_match where toursys='"+_Toursys+"' and etc4='"+_Gymid+"' and matchdate='"+_Date+"'";
            DataTable dt = DbHelperSQL.Query(sql).Tables[0];
            if (dt.Rows.Count > 0)
            {
                foreach (DataRow dr in dt.Rows)
                {
                    WeMatchModel model = new WeMatchModel();
                    model.ContentID = dr[0].ToString();
                    model.ContentName = WeTourContentDll.instance.GetModelbyId(model.ContentID).ContentType;
                    list.Add(model);
                }
            }
            return list;
        }

        /// <summary>
        /// 获得指定内容，在指定日期内的轮次。
        /// </summary>
        /// <param name="_Date"></param>
        /// <param name="_Contents"></param>
        /// <returns></returns>
        private List<WeMatchModel> GetMatchRoundbyContent(string _Date, string _Contents)
        {
            List<WeMatchModel> list = new List<WeMatchModel>();
            string sql = "select distinct(round) from wtf_match where matchdate='" + _Date + "' and contentid in (" + _Contents + ")";
            DataTable dt = DbHelperSQL.Query(sql).Tables[0];
            if (dt.Rows.Count > 0)
            {
                foreach (DataRow dr in dt.Rows)
                {
                    WeMatchModel model = new WeMatchModel();
                    model.ROUND = Convert.ToInt32(dr[0].ToString());
                    list.Add(model);
                }
            }
            return list;
        }

        /// <summary>
        /// 根据日期，项目来获取比赛清单
        /// </summary>
        /// <param name="_Date"></param>
        /// <param name="_Contents"></param>
        /// <returns></returns>
        private List<WeMatchModel> GetMatchesbyDateContent(string _Date, string _Contents)
        {
            List<WeMatchModel> list = new List<WeMatchModel>();
            string sql = "select * from wtf_match where matchdate='"+_Date+"' and contentid in ("+_Contents+")";
            DataTable dt = DbHelperSQL.Query(sql).Tables[0];
            if (dt.Rows.Count > 0)
            {
                list = JsonHelper.ParseDtModelList<List<WeMatchModel>>(dt);
            }
            return list;
        }

        /// <summary>
        /// 获取小组赛的轮次
        /// </summary>
        /// <param name="_Date"></param>
        /// <param name="_Contents"></param>
        /// <returns></returns>
        private List<WeMatchModel> GetMatchesGroupRound(string _Date, string _Contents)
        {
            List<WeMatchModel> list = new List<WeMatchModel>();
            string sql = "select distinct(etc2) from wtf_match where matchdate='" + _Date + "' and contentid in (" + _Contents + ") and Round=0";
            DataTable dt = DbHelperSQL.Query(sql).Tables[0];
            if (dt.Rows.Count > 0)
            {
                foreach (DataRow dr in dt.Rows)
                {
                    WeMatchModel model = new WeMatchModel();
                    model.etc2 = dr[0].ToString();
                    list.Add(model);
                }
            }
            return list;
        }

        /// <summary>
        /// 获得小组赛指定轮次的比赛
        /// </summary>
        /// <param name="_Date"></param>
        /// <param name="_Contents"></param>
        /// <param name="_etc2"></param>
        /// <returns></returns>
        private List<WeMatchModel> GetMatchesbyGroupRound(string _Date, string _Contents, string _etc2)
        {
            List<WeMatchModel> list = new List<WeMatchModel>();
            string sql = "select * from wtf_match where matchdate='" + _Date + "' and contentid in (" + _Contents + ") and Round=0 and etc2='"+_etc2+"'";
            DataTable dt = DbHelperSQL.Query(sql).Tables[0];
            if (dt.Rows.Count>0)
            {
                list = JsonHelper.ParseDtModelList<List<WeMatchModel>>(dt);
            }
            return list;
        }

        /// <summary>
        /// 根据Round获得比赛
        /// </summary>
        /// <param name="_Date"></param>
        /// <param name="_Contents"></param>
        /// <param name="_Round"></param>
        /// <returns></returns>
        private List<WeMatchModel> GetMatchesbyRound(string _Date, string _Contents, string _Round)
        {
            List<WeMatchModel> list = new List<WeMatchModel>();
            string sql = "select * from wtf_match where matchdate='" + _Date + "' and contentid in (" + _Contents + ") and Round='" + _Round + "'";
            DataTable dt = DbHelperSQL.Query(sql).Tables[0];
            if (dt.Rows.Count > 0)
            {
                list = JsonHelper.ParseDtModelList<List<WeMatchModel>>(dt);
            }
            return list;
        }

        /// <summary>
        /// 修改场地的
        /// </summary>
        /// <param name="_Sys"></param>
        /// <param name="_Place"></param>
        /// <param name="_Courtid"></param>
        private void UpdateMatchCourtOrder(string _Sys, string _Place, string _Courtid)
        {
            string sql = string.Format("update wtf_match set place='{0}',courtid='{1}' where sys='{2}'",_Place,_Courtid,_Sys);
            int a = DbHelperSQL.ExecuteSql(sql);
        }
        
        #endregion 

        #region 方案二，先分配场地资源策略，再按策略来分配场地
        private void DistributCourtStratagy(string _Toursys, string _Gymid, string _Date)
        {
            //清除此前的策略
            ResCourt_DisStratagyDll.instance.DeleteStratagebyTourSys(_Toursys);

            //获取体育馆的场地信息
            List<ResTour_TourCourtModel> courtlist = ResGymDll.instance.GetTourCourtList(_Toursys, _Gymid);

            List<Model_Dist_GymCourts> tourcourt_list = Biz_TourGyms.instance.GetTourGymCourts(_Toursys, _Gymid);
            foreach (Model_Dist_GymCourts tcourt in tourcourt_list)
            {
                ResTour_TourCourtModel rcourt = new ResTour_TourCourtModel();
                rcourt.COURTID = tcourt.courtId;
                courtlist.Add(rcourt);
            }

            string CourtsStr = "";
            foreach (ResTour_TourCourtModel courmodel in courtlist)
            {
                CourtsStr += ","+courmodel.COURTID;
            }
            CourtsStr = CourtsStr.TrimStart(',');

            //获得要分配的比赛属于哪些项目：单打，双打，混双。
            if (1 == 1)//是否兼项，若允许兼项，则需要开启按顺序
            {
                List<WeMatchModel> list = GetMatchContent(_Toursys, _Gymid, _Date);
                string _SinContents = "";//单打项目
                string _DouContents = "";//双打项目
                string _MixContnets = "";//混合双打
                //将项目分类
                foreach (WeMatchModel model in list)
                {
                    if (model.ContentName == "男单" || model.ContentName == "女单")
                    {
                        _SinContents += ",'" + model.ContentID + "'";
                    }
                    else if (model.ContentName == "男双" || model.ContentName == "女双")
                    {
                        _DouContents += ",'" + model.ContentID + "'";
                    }
                    else if (model.ContentName == "混双")
                    {
                        _MixContnets += ",'" + model.ContentID + "'";
                    }
                }//end of foreach match content

                //根据项目情况，进行策略分配
                #region 分配单打比赛策略

                if (_SinContents.Length > 0)
                {
                    //指定日期内包含单打的项目
                    //--处理项目清单
                    _SinContents = _SinContents.TrimStart(',');
                    //--获得当天，这些项目要打的轮次数
                    List<WeMatchModel> Rlist = GetMatchRoundbyContent(_Date, _SinContents);
                    foreach (WeMatchModel model in Rlist)
                    {
                        if (model.ROUND == 0)
                        {
                            //小组赛分配，继续按照小组赛的轮次来细分
                            List<WeMatchModel> grlist = GetMatchesGroupRound(_Date, _SinContents);
                            for (int i = 0; i < grlist.Count;i++ )
                            {
                                int courtDisQty = 0;
                                //分配小组赛，逐轮比赛进行分配
                                // grmodel.etc2,小组赛轮次
                                // 1.获得总比赛数量
                                List<WeMatchModel> rglist = GetMatchesbyGroupRound(_Date, _SinContents, (i + 1).ToString());
                                int GRmatchQty = rglist.Count;
                                if (_SinContents.IndexOf(",") > 0)
                                {
                                    //同时有多个单打项目，逐个项目分配场地策略。
                                    string[] Contents = _SinContents.Split(',');
                                    for (int j = 0; j < Contents.Length; j++)
                                    {
                                        string _Courts = "";
                                        if (j == Contents.Length - 1)
                                        {
                                            //已分配完最后一个项目
                                            int _ContentsMatQ = GetMatchesbyGroupRound(_Date, Contents[j], (i + 1).ToString()).Count;
                                            int a = 0;
                                            double _contentCourtQ =_ContentsMatQ * courtlist.Count / GRmatchQty;
                                            if (_contentCourtQ < 1)
                                            {
                                                a = 1;
                                            }
                                            else
                                            {
                                                a = Convert.ToInt32(Math.Floor(_contentCourtQ));
                                            }
                                            for (int n = 0; n < a; n++)
                                            {
                                                _Courts = "," + courtlist[n].COURTID;
                                            }
                                        }
                                        else
                                        {
                                            for (int m = courtDisQty; m < courtlist.Count; m++)
                                            {
                                                _Courts = "," + courtlist[m].COURTID;
                                            }
                                        }
                                        _Courts = _Courts.TrimStart(',');
                                        ResCourt_DisStratagyModel rsmodel = new ResCourt_DisStratagyModel();
                                        rsmodel.TOURSYS = _Toursys;
                                        rsmodel.GYMID = _Gymid;
                                        rsmodel.COURTIDS = _Courts;
                                        rsmodel.CONTENTID = Contents[j];
                                        rsmodel.ISGROUP = "1";
                                        rsmodel.GROUPID = "all";//表示所有的小组        
                                        rsmodel.ROUND = (i + 1).ToString();
                                        rsmodel.MATCHDATE = _Date;

                                        ResCourt_DisStratagyDll.instance.InsertNew(rsmodel);
                                    }// end of content for
                                }
                                else
                                { 
                                    //只有一个项目，策略分配
                                    ResCourt_DisStratagyModel rsmodel = new ResCourt_DisStratagyModel();
                                    rsmodel.TOURSYS = _Toursys;
                                    rsmodel.GYMID = _Gymid;
                                    rsmodel.COURTIDS = CourtsStr;
                                    rsmodel.CONTENTID = _SinContents;
                                    rsmodel.ISGROUP = "1";
                                    rsmodel.GROUPID = "all";//表示所有的小组        
                                    rsmodel.ROUND = (i + 1).ToString();
                                    rsmodel.MATCHDATE = _Date;
                                    ResCourt_DisStratagyDll.instance.InsertNew(rsmodel);
                                }
                            }       
                        }//end of Group distribution
                        else
                        { 
                            //分配淘汰赛
                            ResCourt_DisStratagyModel rsmodel = new ResCourt_DisStratagyModel();
                            rsmodel.TOURSYS = _Toursys;
                            rsmodel.GYMID = _Gymid;
                            rsmodel.COURTIDS = CourtsStr;//所有场地
                            rsmodel.CONTENTID = _SinContents;//所有项目
                            rsmodel.ISGROUP = "0";
                            rsmodel.GROUPID = "";
                            rsmodel.ROUND = model.ROUND.ToString();
                            rsmodel.MATCHDATE = _Date;
                            ResCourt_DisStratagyDll.instance.InsertNew(rsmodel);
                        }                        
                    }
                }
#endregion
                
                #region 分配双打比赛策略

                if (_DouContents.Length > 0)
                {
                    //指定日期内包含单打的项目
                    //--处理项目清单
                    _DouContents = _DouContents.TrimStart(',');
                    //--获得当天，这些项目要打的轮次数
                    List<WeMatchModel> Rlist = GetMatchRoundbyContent(_Date, _DouContents);
                    foreach (WeMatchModel model in Rlist)
                    {
                        if (model.ROUND == 0)
                        {
                            //小组赛分配，继续按照小组赛的轮次来细分
                            List<WeMatchModel> grlist = GetMatchesGroupRound(_Date, _DouContents);
                            for (int i = 0; i < grlist.Count; i++)
                            {
                                int courtDisQty = 0;
                                //分配小组赛，逐轮比赛进行分配
                                // grmodel.etc2,小组赛轮次
                                // 1.获得总比赛数量
                                List<WeMatchModel> rglist = GetMatchesbyGroupRound(_Date, _DouContents, (i + 1).ToString());
                                int GRmatchQty = rglist.Count;
                                if (_DouContents.IndexOf(",") > 0)
                                {
                                    //同时有多个双打项目，逐个项目分配场地策略。
                                    string[] Contents = _DouContents.Split(',');
                                    int _CourtAl = 0;
                                    for (int j = 0; j < Contents.Length; j++)
                                    {
                                        string _Courts = "";
                                        
                                        if (j == Contents.Length - 1)
                                        {
                                            //只剩最后一个项目，分配剩余的场地
                                            int b = courtlist.Count-_CourtAl;
                                            for (int n = 0; n < b; n++)
                                            {
                                                _Courts += "," + courtlist[n+_CourtAl].COURTID;
                                            }
                                            _CourtAl += b;
                                        }
                                        else
                                        {
                                            //还有多个项目，按照多个项目比赛数量比
                                            int _ContentsMatQ = GetMatchesbyGroupRound(_Date, Contents[j], (i + 1).ToString()).Count;
                                            int a = 0;
                                            double _contentCourtQ = _ContentsMatQ * courtlist.Count / GRmatchQty;
                                            //2016-4-1：刘涛；这种分配策略所分配的场地还是粗略的，会出同一轮的比赛不齐套的问题。使用这种分配方式，可以首先满足同一个项目尽量在相同的场地；要使用这种分配策略，就需要在排布完一轮比赛后，再增加一个平均场地比赛的方法。
                                            if (_contentCourtQ < 1)
                                            {
                                                a = 1;
                                            }
                                            else
                                            {
                                                a = Convert.ToInt32(Math.Floor(_contentCourtQ));
                                            }
                                            for (int n = 0; n < a; n++)
                                            {
                                                _Courts += "," + courtlist[n+_CourtAl].COURTID;                                               
                                            }
                                            _CourtAl += a;
                                        }
                                        _Courts = _Courts.TrimStart(',');
                                        ResCourt_DisStratagyModel rsmodel = new ResCourt_DisStratagyModel();
                                        rsmodel.TOURSYS = _Toursys;
                                        rsmodel.GYMID = _Gymid;
                                        rsmodel.COURTIDS = _Courts;
                                        rsmodel.CONTENTID = Contents[j];
                                        rsmodel.ISGROUP = "1";
                                        rsmodel.GROUPID = "all";//表示所有的小组        
                                        rsmodel.ROUND = (i + 1).ToString();
                                        rsmodel.MATCHDATE = _Date;

                                        ResCourt_DisStratagyDll.instance.InsertNew(rsmodel);
                                    }// end of content for
                                }
                                else
                                {
                                    //只有一个项目，策略分配
                                    ResCourt_DisStratagyModel rsmodel = new ResCourt_DisStratagyModel();
                                    rsmodel.TOURSYS = _Toursys;
                                    rsmodel.GYMID = _Gymid;
                                    rsmodel.COURTIDS = CourtsStr;
                                    rsmodel.CONTENTID = _DouContents;
                                    rsmodel.ISGROUP = "1";
                                    rsmodel.GROUPID = "all";//表示所有的小组        
                                    rsmodel.ROUND = (i + 1).ToString();
                                    rsmodel.MATCHDATE = _Date;
                                    ResCourt_DisStratagyDll.instance.InsertNew(rsmodel);
                                }
                            }
                        }//end of Group distribution
                        else
                        {
                            //分配淘汰赛
                            ResCourt_DisStratagyModel rsmodel = new ResCourt_DisStratagyModel();
                            rsmodel.TOURSYS = _Toursys;
                            rsmodel.GYMID = _Gymid;
                            rsmodel.COURTIDS = CourtsStr;//所有场地
                            rsmodel.CONTENTID = _DouContents;//所有项目
                            rsmodel.ISGROUP = "0";
                            rsmodel.GROUPID = "";
                            rsmodel.ROUND = model.ROUND.ToString();
                            rsmodel.MATCHDATE = _Date;
                            ResCourt_DisStratagyDll.instance.InsertNew(rsmodel);
                        }
                    }
                }
                #endregion
                
                #region 分配混双比赛策略

                if (_MixContnets.Length > 0)
                {
                    //指定日期内包含单打的项目
                    //--处理项目清单
                    _MixContnets = _MixContnets.TrimStart(',');
                    //--获得当天，这些项目要打的轮次数
                    List<WeMatchModel> Rlist = GetMatchRoundbyContent(_Date, _MixContnets);
                    foreach (WeMatchModel model in Rlist)
                    {
                        if (model.ROUND == 0)
                        {
                            //小组赛分配，继续按照小组赛的轮次来细分
                            List<WeMatchModel> grlist = GetMatchesGroupRound(_Date, _MixContnets);
                            for (int i = 0; i < grlist.Count; i++)
                            {
                                int courtDisQty = 0;
                                //分配小组赛，逐轮比赛进行分配
                                // grmodel.etc2,小组赛轮次
                                // 1.获得总比赛数量
                                List<WeMatchModel> rglist = GetMatchesbyGroupRound(_Date, _MixContnets, (i + 1).ToString());
                                int GRmatchQty = rglist.Count;
                                if (_MixContnets.IndexOf(",") > 0)
                                {
                                    //同时有多个双打项目，逐个项目分配场地策略。
                                    string[] Contents = _MixContnets.Split(',');
                                    for (int j = 0; j < Contents.Length; j++)
                                    {
                                        string _Courts = "";
                                        if (j == Contents.Length - 1)
                                        {
                                            //已分配完最后一个项目
                                            int _ContentsMatQ = GetMatchesbyGroupRound(_Date, Contents[j], (i + 1).ToString()).Count;
                                            int a = 0;
                                            double _contentCourtQ = _ContentsMatQ * courtlist.Count / GRmatchQty;
                                            if (_contentCourtQ < 1)
                                            {
                                                a = 1;
                                            }
                                            else
                                            {
                                                a = Convert.ToInt32(Math.Floor(_contentCourtQ));
                                            }
                                            for (int n = 0; n < a; n++)
                                            {
                                                _Courts = "," + courtlist[n].COURTID;
                                            }
                                        }
                                        else
                                        {
                                            for (int m = courtDisQty; m < courtlist.Count; m++)
                                            {
                                                _Courts = "," + courtlist[m].COURTID;
                                            }
                                        }
                                        _Courts = _Courts.TrimStart(',');
                                        ResCourt_DisStratagyModel rsmodel = new ResCourt_DisStratagyModel();
                                        rsmodel.TOURSYS = _Toursys;
                                        rsmodel.GYMID = _Gymid;
                                        rsmodel.COURTIDS = _Courts;
                                        rsmodel.CONTENTID = Contents[j];
                                        rsmodel.ISGROUP = "1";
                                        rsmodel.GROUPID = "all";//表示所有的小组        
                                        rsmodel.ROUND = (i + 1).ToString();
                                        rsmodel.MATCHDATE = _Date;

                                        ResCourt_DisStratagyDll.instance.InsertNew(rsmodel);
                                    }// end of content for
                                }
                                else
                                {
                                    //只有一个项目，策略分配
                                    ResCourt_DisStratagyModel rsmodel = new ResCourt_DisStratagyModel();
                                    rsmodel.TOURSYS = _Toursys;
                                    rsmodel.GYMID = _Gymid;
                                    rsmodel.COURTIDS = CourtsStr;
                                    rsmodel.CONTENTID = _MixContnets;
                                    rsmodel.ISGROUP = "1";
                                    rsmodel.GROUPID = "all";//表示所有的小组        
                                    rsmodel.ROUND = (i + 1).ToString();
                                    rsmodel.MATCHDATE = _Date;
                                    ResCourt_DisStratagyDll.instance.InsertNew(rsmodel);
                                }
                            }
                        }//end of Group distribution
                        else
                        {
                            //分配淘汰赛
                            ResCourt_DisStratagyModel rsmodel = new ResCourt_DisStratagyModel();
                            rsmodel.TOURSYS = _Toursys;
                            rsmodel.GYMID = _Gymid;
                            rsmodel.COURTIDS = CourtsStr;//所有场地
                            rsmodel.CONTENTID = _MixContnets;//所有项目
                            rsmodel.ISGROUP = "0";
                            rsmodel.GROUPID = "";
                            rsmodel.ROUND = model.ROUND.ToString();
                            rsmodel.MATCHDATE = _Date;
                            ResCourt_DisStratagyDll.instance.InsertNew(rsmodel);
                        }
                    }
                }
                #endregion
            }//end of if 兼项

            //按照分配策略，进行场地赛事资源分配
            DistriResbyStratagy(_Toursys, _Gymid, _Date);
        }


        /// <summary>
        /// 根据赛事资源分配的策略来进行场地资源分配
        /// </summary>
        /// <param name="_Toursys"></param>
        /// <param name="_Gymid"></param>
        /// <param name="_Date"></param>
        private void DistriResbyStratagy(string _Toursys, string _Gymid, string _Date)
        {
            List<ResCourt_DisStratagyModel> list = ResCourt_DisStratagyDll.instance.GetCourtsStratagy(string.Format(" and toursys='{0}' and gymid='{1}' and matchdate='{2}' order by id", _Toursys, _Gymid, _Date));//按照添加顺序排序
            if (list.Count > 0)
            {
                int _Place = 1;
                //存在分配策略
                foreach (ResCourt_DisStratagyModel model in list)
                {
                    List<WeMatchModel> matchlist = new List<WeMatchModel>();
                    if (model.ISGROUP == "1")
                    {
                        //是小组赛，根据contentid 和小组赛轮次来获得比赛
                        matchlist = GetMatchesbyGroupRound(_Date, model.CONTENTID, model.ROUND);
                    }
                    else
                    { 
                        //是淘汰赛
                        matchlist = GetMatchesbyRound(_Date, model.CONTENTID, model.ROUND);
                    }

                    if (model.COURTIDS.IndexOf(',') > 0)
                    {
                        //有多个场地
                        string[] _Courts = model.COURTIDS.Split(',');//分配的场地
                        int CourtO = 0;
                        foreach (WeMatchModel mmodel in matchlist)
                        {
                            UpdateMatchCourtOrder(mmodel.SYS, _Place.ToString(), _Courts[CourtO]);
                            CourtO += 1;
                            if (CourtO == _Courts.Length)
                            {
                                CourtO = 0;
                                _Place += 1;
                            }
                        }
                    }
                    else
                    { 
                        //只有一片场地
                        foreach (WeMatchModel mmodel in matchlist)
                        {
                            UpdateMatchCourtOrder(mmodel.SYS, _Place.ToString(), model.COURTIDS);
                            _Place += 1;
                        }
                    }
                }
            }
        }

        #endregion 
    }
}
