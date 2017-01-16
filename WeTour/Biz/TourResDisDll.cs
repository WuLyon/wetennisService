using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;

namespace WeTour
{
    /// <summary>
    /// 分配赛事资源
    /// </summary>
    public class TourResDisDll
    {
        public static TourResDisDll instance = new TourResDisDll();

        #region Old Distribute strategy
        /// <summary>
        /// Adjust a match to another court at sepcific order
        /// adjust former courtid orders, update destination court orders.
        /// </summary>
        /// <param name="Matchsys"></param>
        /// <param name="_CourtId"></param>
        /// <param name="_Place"></param>
        public void AdjustMatchCourt(string Matchsys, string _CourtId, string _Place)
        {
            WeMatchModel model = WeMatchDll.instance.GetModel(Matchsys);
            int DesOrder = Convert.ToInt32(_Place);

            //Empty the order where the match goes to 
            List<WeMatchModel> list = WeMatchDll.instance.GetMatchbyCourtidPlace(model.TOURSYS, _CourtId);
            if (DesOrder <= list.Count)
            {
                //the order has already exist, need to move down the exsiting matches' order from the specific order
                for (int i = DesOrder - 1; i < list.Count; i++)
                {
                    int NewOrder = Convert.ToInt32(list[i].PLACE)+1;
                    WeMatchDll.instance.UpdateMatch_Place(list[i].SYS, NewOrder.ToString());
                }
            }

            //Update Former Court Match Order
            int FormerOrder = Convert.ToInt32(model.PLACE);
            List<WeMatchModel> formerlist = WeMatchDll.instance.GetMatchbyCourtidPlace(model.TOURSYS, model.COURTID);
            if (FormerOrder < formerlist.Count)
            {
                //the former order is not the last one, so update those behind this match
                for (int i = FormerOrder; i < formerlist.Count; i++)
                {
                    int NewOrder = Convert.ToInt32(formerlist[i].PLACE) - 1;
                    WeMatchDll.instance.UpdateMatch_Place(formerlist[i].SYS, NewOrder.ToString());
                }
            }

            //Update Destination Match
            WeMatchDll.instance.UpdateMatch_CourtId(Matchsys, _CourtId);
            WeMatchDll.instance.UpdateMatch_Place(Matchsys, _Place);
        }

        /// <summary>
        /// 分配场地资源
        /// 这个方法还不能够处理不同项目的小组组合在一起的情况。原因是小组并没有可识别的主键
        /// </summary>
        /// <param name="_TourSys"></param>
        /// <param name="_ContId">不为空时，表示为一个或多个赛事分配</param>
        /// <param name="Groups">当小组不为空时，表示为一个或多个小组分配</param>
        /// <param name="_Courts">分配到的资源</param>
        public void DistributeCourtResource(string _TourSys, string _ContId, string Groups, string _Courts)
        { 
            
            //Decide level
            if (Groups != "")
            { 
                //多个小组或单个小组独立分配资源。
                //应用场景：给一个子项赛事的场地资源数量不等于子项赛事小组的时候（大于和小于均需要）
                List<WeMatchModel> list = WeMatchDll.instance.GetMatchesbyContGroup(_ContId, Groups);
                if (list.Count > 0)
                {
                    foreach (WeMatchModel model in list)
                    {
                        if (_Courts.IndexOf(",") > 0)
                        {
                            string[] courts = _Courts.Split(',');
                            for (int i = 0; i < courts.Length; i++)
                            {
                                //循环将场地资源添加到比赛
                                WeTourDll.instance.AssignMatchToCourt(model.SYS, courts[i]);
                            }
                        }
                        else
                        {
                            WeTourDll.instance.AssignMatchToCourt(model.SYS, _Courts);
                        }
                    }
                }

            }
            else if (_ContId != "")
            {
                //将资源分分配到对应的子项，子项的比赛按照轮次，排满场地
                //应用场景：部分赛事项目的比赛加起来不多，可以混合来进行排列
                List<WeMatchModel> list = WeMatchDll.instance.GetMatchesbyConts(_ContId);
                if (list.Count > 0)
                {
                    foreach (WeMatchModel model in list)
                    {
                        if (_Courts.IndexOf(",") > 0)
                        {
                            string[] courts = _Courts.Split(',');
                            for (int i = 0; i < courts.Length; i++)
                            {
                                //循环将场地资源添加到比赛
                                WeTourDll.instance.AssignMatchToCourt(model.SYS, courts[i]);

                            }
                        }
                        else
                        {
                            WeTourDll.instance.AssignMatchToCourt(model.SYS, _Courts);
                        }
                    }
                }
            }
            else
            { 
                //将资源分配到赛事,赛事
                //应用场景：整个赛事的比赛数量并不多，且不用考虑各个项目的各小组要尽量在同一片场地。
                List<WeMatchModel> list = WeMatchDll.instance.GetMatchesbyToursys4Dis(_TourSys);
                if (list.Count > 0)
                {
                    foreach (WeMatchModel model in list)
                    {
                        if (_Courts.IndexOf(",") > 0)
                        {
                            string[] courts = _Courts.Split(',');
                            for (int i = 0; i < courts.Length; i++)
                            {
                                //循环将场地资源添加到比赛
                                WeTourDll.instance.AssignMatchToCourt(model.SYS, courts[i]);
                            }
                        }
                        else
                        {
                            WeTourDll.instance.AssignMatchToCourt(model.SYS, _Courts);
                        }
                    }
                }

            }
        }

        /// <summary>
        /// 根据赛事主键来分配资源，前提是已经做好赛事分配方案
        /// </summary>
        /// <param name="_TourSys"></param>
        public void DistributeResourceByTour(string _TourSys)
        { 
            //首先，清空之前的赛事资源分配方案
            string sql = "update wtf_match set courtid='',place='' where toursys='"+_TourSys+"'";
            int a = DbHelperSQL.ExecuteSql(sql);

            //根据赛事主键查询已经分配的赛事资源分配方案
            List<CourtResourceModel> list = CourtResourceDll.instance.GetResSysbyToursys(_TourSys);
            if (list.Count > 0)
            { 
                //存在多个赛事分配方案
                foreach (CourtResourceModel model in list)
                { 
                    //根据分配方案主键获得具体的分配方案
                    List<CourtResourceModel> listR = CourtResourceDll.instance.GetModellistbyResSys(model.ResSys);
                    if (listR.Count > 1)
                    {
                        //该分配方案涉及多个项目
                        //查询比赛
                        string sqlR = "select * from wtf_match where 1=1";
                        foreach (CourtResourceModel Rmodel in listR)
                        {
                            sqlR += " or sys in (select sys from wtf_match where ContentID='"+Rmodel.ContentId+"' and etc1 in ("+Rmodel.Groups+")) ";
                        }
                        DataTable dt = DbHelperSQL.Query(sqlR).Tables[0];
                        //为查询到的比赛分配场地资源
                        foreach (DataRow dr in dt.Rows)
                        {
                            if (listR[0].Courts.IndexOf(",") > 0)
                            {
                                string[] courts = listR[0].Courts.Split(',');
                                for (int i = 0; i < courts.Length; i++)
                                {
                                    //循环将场地资源添加到比赛
                                    WeTourDll.instance.AssignMatchToCourt(dr["sys"].ToString(), courts[i]);
                                }
                            }
                            else
                            {
                                WeTourDll.instance.AssignMatchToCourt(dr["sys"].ToString(), listR[0].Courts);
                            }
                        }
                    }
                    else
                    { 
                        //该分配方案只涉及一个项目
                        DistributeCourtResource(_TourSys, listR[0].ContentId, listR[0].Groups, listR[0].Courts);
                    }
                }
            }
        }
        #endregion


		///remark
		///设置两个算法按钮：不同项目是否分配不同的场地；考虑兼项则先进行单打，再进行双打
        #region 分配赛事资源，新增日期和场馆分配策略，2016-6-5
        public void DistributeByStratagy(List<Model_TourDateRound> rlist, List<Model_Dist_GymCourts> clist,string _Stratage)
        {
            switch (_Stratage)
            { 
                case "A":
                    //比赛各个轮次交替进行
                    SimpleDistribute(rlist, clist);
                    break;
                case "B":
                    //按项目分场地，然后按轮次先后进行
                    SameGroup_Strategy(rlist, clist);
                    break;
            }
        }

        /// <summary>
        /// 进行简单的排序分配方案
        /// </summary>
        /// <param name="_tourSys"></param>
        public void SimpleDistribute(List<Model_TourDateRound> rlist, List<Model_Dist_GymCourts> clist)
        { 
            //render contenttype by stratage
            #region 处理轮次信息
            foreach (Model_TourDateRound round in rlist)
            {
                WeTourContModel cont = WeTourContentDll.instance.GetModelbyId(round.ContentId);
                round.ContentType = RenderPriorityOfType(cont.ContentType,"A");
                round.GroupType = cont.TourDate;
            }
            #endregion

            //step 1, rearrange tour date round list，利用算法来进行排布，1.轮次，2.项目类型，3.组别。
            //考虑三个因素。
            #region 重新排序
            RearrangeTour(rlist);
            #endregion

            //step 2,按照优先级先后，来排列比赛

        }

        /// <summary>
        /// 同组项目同理对待，按照轮次先后来进行排布。
        /// 2016-09-14，liutao，
        /// </summary>
        /// <param name="rlist"></param>
        /// <param name="clist"></param>
        public void SameGroup_Strategy(List<Model_TourDateRound> rlist, List<Model_Dist_GymCourts> clist)
        {            
            //循环分配场地
            int court_index = 0;

            //相同组别，按照轮次先后来进行排布
            List<Model_TourDateRound> nlist = RearrangeTour(rlist);
            
            //将roundlist按照同组别，不同项目的相同轮次放到一起。
            foreach (Model_TourDateRound rounds in nlist)
            {
                //根据项目id，和round来获取比赛。
                List<WeMatchModel> matchlist = WeMatchDll.instance.GetMatchlistbyContRound(rounds.ContentId, rounds.Round);
                foreach (WeMatchModel match in matchlist)
                {
                    //检查场地序号
                    if (court_index >= clist.Count)
                    {
                        court_index = 0;
                    }

                    //是绑定场地再添加
                  
                    //修改比赛场地信息
                    WeMatchDll.instance.UpdateMatchResource(match.SYS,rounds.MatchDate, clist[court_index].courtId);
                   
                    court_index += 1;
                }
            }
        }
                                                                                                          /// <summary>
                                                                                                          /// 重新排布项目列表,将相同轮次排在一起
                                                                                                          /// 使用冒泡算法，2016-09-15，刘涛
                                                                                                          /// </summary>
                                                                                                          /// <param name="rlist"></param>
                                                                                                          /// <returns></returns>
        private List<Model_TourDateRound> RearrangeTour(List<Model_TourDateRound> rlist)
        {
            List<Model_TourDateRound> n_list = new List<Model_TourDateRound>();
            //外循环，控制次数，总次数为数组长度-1
            for (int i = 0; i <rlist.Count-1; i++)
            {
                //内循环，对比大小
                for (int j = rlist.Count-1; j > i; j--)
                { 
                    //数组重新排序
                    
                    int a = Convert.ToInt32(rlist[j].Round);
                    int b = Convert.ToInt32(rlist[j - 1].Round);
                    if (b > a)
                    { 
                        //需将二者互换
                        Model_TourDateRound temp = rlist[j-1];
                        rlist[j - 1] = rlist[j];
                        rlist[j] = temp;
                    }
                }
            }
            return rlist;
        }

        /// <summary>
        /// 分配赛事资源，供外部调用
        /// update，刘涛，更新单场馆资源分配策略，新增B策略。2016-09-14
        /// </summary>
        /// <param name="_Toursys"></param>
        public void DistributeTourRes(string _Toursys)
        {
            //清楚资源分配
            WeMatchDll.instance.Clear_Resource(_Toursys);

            //修改逻辑
            WeTourDll.instance.UpdateTourLogic(_Toursys);

            //根据赛事主键，获取比赛日期
            List<Model_TourDate> dlist = Biz_TourDate.instance.GetTourDate(_Toursys);
            foreach (Model_TourDate dmodel in dlist)
            { 
                //根据日期获取要进行的项目及轮次，如果未选择，则应默认添加所有轮次。
                List<Model_TourDateRound> rlist = Biz_TourDate.instance.GetTourDateAssRound(_Toursys, dmodel.TourDate);

                //获取场地信息
                List<TourGymList> glist = Biz_TourGyms.instance.GetTourGyms(_Toursys);
                if (glist.Count > 1)
                {
                    //有多个场馆，分多次，先一个场馆分完，再分剩下的场馆
                }
                else
                { 
                    //只有一个场馆
                    string _GymSys = glist[0].gymSys;
                    //根据场馆获得场地.只获取已选中的场地信息
                    List<Model_Dist_GymCourts> clist = Biz_TourGyms.instance.GetTourGymCourts(_Toursys, _GymSys);

                    //只保留选中的场地
                    List<Model_Dist_GymCourts> n_clist = new List<Model_Dist_GymCourts>();
                    foreach (Model_Dist_GymCourts court in clist)
                    { 
                        if(court.isCheck=="1")
                        {
                            n_clist.Add(court);
                        }
                    }

                    //GetDistriStategy(rlist, clist);
                    DistributeByStratagy(rlist, n_clist, "B");
                }


            }//end of tourdate foreach
        }

        private string RenderPriorityOfType(string _ContType,string Type)
        {
            string _TypePrio = "";
            switch (Type)
            {
                case "A":
                    switch (_ContType)
                    {
                        case "男单":
                            _TypePrio = "1";
                            break;
                        case "女单":
                            _TypePrio = "2";
                            break;
                        case "男双":
                            _TypePrio = "3";
                            break;
                        case "女双":
                            _TypePrio = "4";
                            break;
                        case "混双":
                            _TypePrio = "5";
                            break;
                    }
                    break;
            }
            return _TypePrio;
        }

        /// <summary>
        /// 系统自动根据各组别，各项目的比赛情况，来决定分配策略
        /// </summary>
        /// <param name="rlist"></param>
        /// <param name="clist"></param>
        private void GetDistriStategy(List<Model_TourDateRound> rlist, List<Model_Dist_GymCourts> clist)
        { 
            //按照TourdateRound的比赛数量，来分配场地
            int _TotalMatch = 0;

            //定义数组来存放首轮项目的比赛数量
            //ContFstRound,示例：{contid,contype,matchqty},二维数组，定义方法：[,]
            List<Model_TourDateRound> contFstRound = new List<Model_TourDateRound>();


            foreach (Model_TourDateRound model in rlist)
            { 
                //为轮次添加比赛数量
                model.TourMatchQty = WeMatchDll.instance.CountMatchQtybyContRound(model.ContentId, model.Round);
                _TotalMatch += model.TourMatchQty;

                //添加项目的类型
                WeTourContModel cont = WeTourContentDll.instance.GetModelbyId(model.ContentId);
                model.ContentType = cont.ContentType;
                model.GroupType = cont.TourDate;//组别

               //将每个项目第一个轮次添加FstRound 数组。默认各项目都是按轮次先后顺序排列。
                
                bool _IfAdded=false;
                foreach (Model_TourDateRound model1 in contFstRound)
                {
                    if (model.ContentId == model1.ContentId)
                    {
                        _IfAdded = true;
                    }
                }
                if (!_IfAdded)
                {
                    contFstRound.Add(model);
                }
            }

            #region 优先级判断
            //判断项目是并行，还是串行
            //先不考虑组别的区别，仅考虑男单，男双，女单，女双的情况。把单打与双打分开。杜绝单打和双打同时进行的情况。
            //根据项目首轮，判断优先级
            List<Model_PriorityCont> prioList = new List<Model_PriorityCont>();
            //初始化，添加三个优先级
            Model_PriorityCont prio = new Model_PriorityCont();
            prio.priority = 1;
            prioList.Add(prio);

            Model_PriorityCont prio2 = new Model_PriorityCont();
            prio2.priority = 2;
            prioList.Add(prio2);

            Model_PriorityCont prio3 = new Model_PriorityCont();
            prio3.priority = 3;
            prioList.Add(prio3);

            string contNames = "";
            //先处理单打
            foreach (Model_TourDateRound model1 in contFstRound)
            {
                if (model1.ContentType=="男单")
                {
                    prioList[0].contlist.Add(model1);
                    contNames += ",男单";
                }
                if (model1.ContentType == "女单")
                {
                    prioList[0].contlist.Add(model1);
                    contNames += ",女单";
                }   
            }

            //再处理双打
            foreach (Model_TourDateRound model1 in contFstRound)
            {
                if (model1.ContentType == "男双")
                {
                    contNames += ",男双";
                    //判断有无男单
                    if (contNames.IndexOf("男单") > 0)
                    {
                        //有男单，男双排到优先级2
                        prioList[1].contlist.Add(model1);
                    }
                    else
                    { 
                        //无男单，男双排到优先级1
                        prioList[0].contlist.Add(model1);
                    }
                    
                }
                if (model1.ContentType == "女双")
                {
                    contNames += ",女双";
                    if (contNames.IndexOf("女单") > 0)
                    {
                        //有男单，男双排到优先级2
                        prioList[1].contlist.Add(model1);
                    }
                    else
                    {
                        //无男单，男双排到优先级1
                        prioList[0].contlist.Add(model1);
                    }
                }
            }

            //处理混双，混双和性别有关，直接放在最后一个优先级
            foreach (Model_TourDateRound model1 in contFstRound)
            {
                if (model1.ContentType == "混双")
                {
                    contNames += ",混双";                   
                    prioList[2].contlist.Add(model1);                   
                }                
            }

            #endregion

            #region 判断各个优先级的项目数量，并分配场地
            foreach (Model_PriorityCont prios in prioList)
            {
                int PrioTotalMatch = 0;
                foreach (Model_TourDateRound rm in prios.contlist)
                {
                    PrioTotalMatch += rm.TourMatchQty;
                }
                //计算场地数量，并分配场地资源

                foreach (Model_TourDateRound rm in prios.contlist)
                {
                    

                }


            }
            #endregion

        }

        /// <summary>
        /// 分配比赛和场地
        /// </summary>
        /// <param name="Matches"></param>
        /// <param name="CoutIds"></param>
        /// <param name="couIndex"></param>
        private void DistributeMatch2(string[] Matches, string[] CoutIds, int couIndex,  out int _LastCouIndex)
        {
            for (int i = 0; i < Matches.Length; i++)
            {
                //为场地分配
                WeTourDll.instance.AssignMatchToCourt(Matches[i], CoutIds[couIndex]);
                if (couIndex == CoutIds.Length - 1)
                {
                    couIndex = 0;
                }
                else
                {
                    couIndex += 1;
                }
            }
            _LastCouIndex = couIndex;
        }

        /// <summary>
        /// 分配比赛和场地
        /// </summary>
        /// <param name="Matches"></param>
        /// <param name="CoutIds"></param>
        /// <param name="couIndex"></param>
        private void DistributeMatch(string[] Matches, string[] CoutIds, int couIndex,int _courRound,out int _LastCouIndex,out int _LastcourRound)
        {
            for (int i = 0; i < Matches.Length; i++)
            {
                //为场地分配
                WeTourDll.instance.UpdateMatchCourtInfo(Matches[i], CoutIds[couIndex], _courRound.ToString());
                if (couIndex == CoutIds.Length - 1)
                {
                    couIndex = 0;
                    _courRound += 1;
                }
                else
                {
                    couIndex += 1;
                }
            }
            _LastCouIndex = couIndex;
            _LastcourRound = _courRound;
        }
        #endregion
    }
}
