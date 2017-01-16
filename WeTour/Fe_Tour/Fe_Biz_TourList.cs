using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Member;
using SMS;
using Gym;
using Club;

namespace WeTour
{
    public class Fe_Biz_TourList
    {
        public static Fe_Biz_TourList instance = new Fe_Biz_TourList();

        #region 赛事
        public Fe_Model_Filters Fe_Get_Filters()
        {
            Fe_Model_Filters list = new Fe_Model_Filters();

            List<Fe_Model_Key_Value> status = new List<Fe_Model_Key_Value>();
            #region 添加状态            
            string _strStatus = "-1,1,2,3,4,5,6";
            string[] stats = _strStatus.Split(',');
            for (int i = 0; i < stats.Length; i++)
            {
                Fe_Model_Key_Value keyv = new Fe_Model_Key_Value();
                keyv.value = stats[i];
                switch (stats[i])
                { 
                    case "-1":
                        keyv.text = "所有状态";
                        break;
                    case "1":
                        keyv.text = "正在报名";
                        break;
                    case "2":
                        keyv.text = "报名完成";
                        break;
                    case "3":
                        keyv.text = "分配签表";
                        break;
                    case "4":
                        keyv.text = "编排赛程";
                        break;
                    case "5":
                        keyv.text = "正在进行";
                        break;
                    case "6":
                        keyv.text = "已经完成";
                        break;
                }
                status.Add(keyv);
            }
            #endregion

            List<Fe_Model_Key_Value> citykeys = new List<Fe_Model_Key_Value>();
            #region 添加城市            
            Fe_Model_Key_Value cityk = new Fe_Model_Key_Value();
            cityk.value = "0";
            cityk.text = "所有城市";
            citykeys.Add(cityk);

            List<CityInfo> citys = WeTourDll.instance.GetTourCity();
            foreach (CityInfo ci in citys)
            {
                Fe_Model_Key_Value cit = new Fe_Model_Key_Value();
                cit.value = ci.CityId;
                cit.text = ci.CityId;
                citykeys.Add(cit);
            }
            #endregion

            list.location = citykeys;
            list.status = status;

            return list;
        }

        /// <summary>
        /// 根据筛选条件，获取带分页的赛事列表
        /// </summary>
        /// <param name="_status"></param>
        /// <param name="_eventFilter"></param>
        /// <param name="_location"></param>
        /// <param name="_currentPage"></param>
        /// <param name="_limit"></param>
        /// <returns></returns>
        public List<Fe_Model_EventList> Fe_Get_Event_List(string _status, string _eventFilter, string _location, string _currentPage, string _limit)
        {
            List<Fe_Model_EventList> list = new List<Fe_Model_EventList>();
            List<WeTourModel> tourlist = WeTourDll.instance.GetCityTours(_status,_location);
            for (int i = 0; i < tourlist.Count;i++ )
            {
                Fe_Model_EventList model = new Fe_Model_EventList();
                WeTourModel tour = tourlist[i];
                model.id = tour.SYSNO;
                model.name = tour.NAME;
                model.startDate = tour.STARTDATE;
                model.endDate = tour.ENDDATE;
                //加载类型
                switch (tour.CITYTYPE)
                {
                    case "":
                        model.type = "公开赛";
                        break;
                    case "Club":
                        model.type = "俱乐部赛";
                        break;
                    case "Union":
                        model.type = "联盟赛";
                        break;
                    default:
                        model.type = "公开赛";
                        break;
                }
                //加载地址
                model.location = tour.ADDRESS;
                string thumburl="http://wetennis.cn"+tour.TOURIMG;
                ClubModel club = ClubBiz.Get_Instance().GetModel(tour.MGRSYS);
                if (!string.IsNullOrEmpty(club.EXT2))
                {
                    if (club.EXT2.IndexOf("http://") < 0)
                    {
                        thumburl = "http://wetennis.cn" + club.EXT2;
                    }
                    else
                    {
                        thumburl = club.EXT2;
                    }
                }
                model.thumb = thumburl;

                list.Add(model);
            }
            return list;
        }

        /// <summary>
        /// 根据赛事主键获取赛事详情
        /// </summary>
        /// <param name="_id"></param>
        /// <returns></returns>
        public Fe_Model_EventInfo Fe_Get_Event_Detail(string _id,string _userid)
        {
            Fe_Model_EventInfo model = new Fe_Model_EventInfo();
            WeTourModel tour = WeTourDll.instance.GetModelbySys(_id);
            model.id = _id;
            model.name = tour.NAME;
            model.startDate = tour.STARTDATE;
            model.endDate = tour.ENDDATE;
            model.state = tour.STATUS;

            //判断是否关注
            if (PraiseBll.instance.IsMemPraise("Tour", _id, _userid))
            {
                model.follow = true;
            }
            else
            {
                model.follow = false;
            }

            //加载类型
            switch(tour.CITYTYPE)
            {
                case "":
                    model.type = "公开赛";
                    break;
                case "club":
                    model.type = "俱乐部赛";
                    break;
                case "union":
                    model.type = "联盟赛";
                    break;
                default:
                    model.type = "公开赛";
                    break;
            }
            model.location = tour.ADDRESS;

            //赛事背景
            string bannerUrl = "";
            if (!string.IsNullOrEmpty(tour.TourBackImg))
            {
                bannerUrl = tour.TourBackImg;
            }

            model.banner = bannerUrl;

            int timeSpan = Biz_Time.instance.ComputeDateDiff(DateTime.Now.ToString(), tour.ENDDATE);
            model.countdown = timeSpan*1000;//单位为毫秒

            //赛事举办俱乐部
            string clubThumb = "";
            ClubModel club = ClubBiz.Get_Instance().GetModel(tour.MGRSYS);
            if (!string.IsNullOrEmpty(club.EXT2))
            {
                if (club.EXT2.IndexOf("http://") < 0)
                {
                    clubThumb = "http://wetennis.cn/" + club.EXT2;
                }
            }

            model.thumb = clubThumb;

            //赛事状态
            string state = tour.STATUS;
                          
            //签表倒计时
            model.drawCountdown = 0;

            if (_userid != "")
            {             
                //朋友报名人数
                model.friendRegisterCount = 1;
                //添加已报名项目
                List<WeTourContModel> contlist = WeTourApplyDll.instance.Get_Applied_Content(_id,_userid);
                List<rigisterCont> reg_list = new List<rigisterCont>();
               
                for (int i = 0; i < contlist.Count; i++)
                {
                    rigisterCont cont = new rigisterCont();
                    cont.name = contlist[i].TourDate+"|"+contlist[i].ContentName;
                    int tour_State=Convert.ToInt32(model.state);

                    //决定何时显示现实报名项目情况
                    if (tour_State > 2&&tour_State<6)
                    {
                        //获取当前人员的项目签位号
                        cont.id = "1";
                        reg_list.Add(cont);
                    }
                }
                model.registerList = reg_list;
            }
            else
            {
                model.friendRegisterCount = 0;
                //model.register = false;
            }


            //添加团体赛
            if (tour.MATCHCONTENT == "group")
            {
                model.isGroup = true;
            }
            else
            {
                model.isGroup = false;
            }

            //赛事链接,2016-12-17
            model.eventDetailLink = tour.ext8;

            return model;
        }

        /// <summary>
        /// 根据赛事主键获取赞助商信息
        /// </summary>
        /// <param name="_Toursys"></param>
        /// <returns></returns>
        public List<Fe_Model_eventSponsors> GetSponsers(string _Toursys)
        {
            List<Fe_Model_eventSponsors> list = new List<Fe_Model_eventSponsors>();
            Fe_Model_eventSponsors model = new Fe_Model_eventSponsors();
            model.name = "劲美锋体育";
            model.thumb = "http://img5.imgtn.bdimg.com/it/u=4320219,563834066&fm=21&gp=0.jpg";
            list.Add(model);
            return list;
        }

        /// <summary>
        /// 获取签表筛选
        /// 2016-9-23
        /// </summary>
        /// <param name="_eventId"></param>
        /// <returns></returns>
        public List<Fe_Model_Filter> GetDrawFilter(string _eventId)
        {
            List<Fe_Model_Filter> filter_list = new List<Fe_Model_Filter>();

            //查询组别
            List<WeTourContModel> group_list= WeTourContentDll.instance.GetGroups(_eventId);
            foreach (WeTourContModel group in group_list)
            {                
                Fe_Model_Filter filter = new Fe_Model_Filter();
                filter.text = group.TourDate;
                filter.value = group.TourDate;
                //根据组别添加项目
                List<Fe_Model_Filter_child> child_list = new List<Fe_Model_Filter_child>();
                List<WeTourContModel> cont_list = WeTourContentDll.instance.GetcontentbyGroup(_eventId, group.TourDate);
                foreach (WeTourContModel cont in cont_list) 
                {
                    Fe_Model_Filter_child child = new Fe_Model_Filter_child();
                    child.text = cont.ContentName;
                    child.value = cont.id;
                    child_list.Add(child);
                }
                filter.children = child_list;
                filter_list.Add(filter);
            }           

            return filter_list;
        }


        public Fe_Model_DrawTable Getdrawtable(string _ItemId, int _Round)
        {
            Fe_Model_DrawTable Draw = new Fe_Model_DrawTable();
            WeTourContModel cont = WeTourContentDll.instance.GetModelbyId(_ItemId);
            Draw.gamename = cont.TourDate + "|" + cont.ContentName;
            //项目轮次情况
            Draw.matchs = GetItemsRounds(_ItemId,_Round.ToString());//
            if (_Round == 0)
            {
                //小组赛阶段
                Draw.qualify = true;
                Draw.details = GetQualifyGames(_ItemId);
            }
            else
            {
                //淘汰赛阶段
                Draw.qualify = false;
                Draw.details = GetKnockOut2(_ItemId, _Round);
            }
            return Draw;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="_ItemId"></param>
        /// <returns></returns>
        private List<Dictionary<string, string>> GetItemsRounds(string _ItemId,string _Round)
        {
            List<Dictionary<string, string>> items_list = new List<Dictionary<string, string>>();
            List<WeMatchModel> round_list= WeMatchDll.instance.GetContentRoundsAsc(_ItemId);
            foreach (WeMatchModel round in round_list)
            {
                Dictionary<string, string> item = new Dictionary<string, string>();
                item.Add("id", round.ROUND.ToString());
                item.Add("text", round.RoundName);
                if (round.ROUND.ToString() == _Round)
                {
                    item.Add("current", "true");
                }
                items_list.Add(item);
            }

            return items_list;
        }


        private List<Fe_Model_DrawTable_Qualify> GetQualifyGames(string _ItemId)
        {
            WeTourContModel wcmodel = WeTourContentDll.instance.GetModelbyId(_ItemId);
            List<Fe_Model_DrawTable_Qualify> list = new List<Fe_Model_DrawTable_Qualify>();
            List<ContGroupModel> groupslist = WeContentSignsDll.instance.GetContentGroups(_ItemId);
            foreach (ContGroupModel group in groupslist)
            {
                Fe_Model_DrawTable_Qualify qualify = new Fe_Model_DrawTable_Qualify();
                //判断当前轮次是否最新
                if (WeTourContentDll.instance.IfRoundLatest(_ItemId, "0"))
                {
                    qualify.complete = true;
                }
                else
                {
                    qualify.complete = false;
                }

                qualify.groupname = group.GroupName;
                //添加小组内容
                List<Fe_Model_DrawTable_Qualify_games> qgames = new List<Fe_Model_DrawTable_Qualify_games>();
                List<GroupMemberModel> users = group.GroupMembers;
                foreach (GroupMemberModel user in users)
                {
                    Fe_Model_DrawTable_Qualify_games qgame = new Fe_Model_DrawTable_Qualify_games();
                    //添加成员
                    List<Fe_Model_DrawTable_Qualify_games_member> members = new List<Fe_Model_DrawTable_Qualify_games_member>();
                    if (wcmodel.ContentType == "团体")
                    {
                        //单打
                        Fe_Model_DrawTable_Qualify_games_member member1 = new Fe_Model_DrawTable_Qualify_games_member();
                        //WeMemberModel mem = WeMemberDll.instance.GetModel(user.Memsys);
                        weTeamModel team = weTeamdll.instance.GetModel(user.Memsys);
                        member1.username = team.TEAMNAME;
                        member1.userimage = "";
                        members.Add(member1);
                    }
                    else
                    {
                        if (user.Memsys.IndexOf(",") > 0)
                        {
                            //双打
                            Fe_Model_DrawTable_Qualify_games_member member1 = new Fe_Model_DrawTable_Qualify_games_member();
                            WeMemberModel mem = WeMemberDll.instance.GetModel(user.Memsys.Split(',')[0]);
                            member1.username = mem.USERNAME;
                            member1.userimage = mem.EXT1;
                            members.Add(member1);

                            Fe_Model_DrawTable_Qualify_games_member member2 = new Fe_Model_DrawTable_Qualify_games_member();
                            WeMemberModel mem1 = WeMemberDll.instance.GetModel(user.Memsys.Split(',')[1]);
                            member2.username = mem1.USERNAME;
                            member2.userimage = mem1.EXT1;
                            members.Add(member2);
                        }
                        else
                        {
                            //单打
                            Fe_Model_DrawTable_Qualify_games_member member1 = new Fe_Model_DrawTable_Qualify_games_member();
                            WeMemberModel mem = WeMemberDll.instance.GetModel(user.Memsys);
                            member1.username = mem.USERNAME;
                            member1.userimage = mem.EXT1;
                            members.Add(member1);
                        }
                    }
                    qgame.users = members;

                    //先设定小组第一出线
                    if (user.Order == "1")
                    {
                        qgame.qulified = true;
                    }
                    else
                    {
                        qgame.qulified = false;
                    }

                    qgame.gameNumber = Convert.ToInt32(user.MatchQty);
                    qgame.criticalGame = user.WinLoseGame;
                    qgame.criticalSpot = user.WinLoseMatch;
                    //添加单个小组成员
                    qgames.Add(qgame);
                }

                qualify.games = qgames;

                //添加单个小组
                list.Add(qualify);
            }//end of group
            return list;
        }

        /// <summary>
        /// 获取淘汰赛的签表
        /// 需要将games去掉。
        /// </summary>
        /// <param name="_ItemId"></param>
        /// <param name="_Round"></param>
        /// <returns></returns>
        private List<Fe_Model_DrawTable_knockOut_game_pair> GetKnockOut(string _ItemId, int _Round)
        {
            List<Fe_Model_DrawTable_knockOut_game_pair> KnockOuts = new List<Fe_Model_DrawTable_knockOut_game_pair>();
            List<WeMatchModel> matches = WeMatchDll.instance.GetMatchlistbyContRound(_ItemId, _Round.ToString());
           if(matches.Count==0)
           {
               return KnockOuts;
           }
            if (matches.Count >= 2)
            {
                //非决赛
                for (int i = 0; i < matches.Count; i += 2)
                {
                    Fe_Model_DrawTable_knockOut_game_pair pair = new Fe_Model_DrawTable_knockOut_game_pair();
                    List<Fe_Model_DrawTable_knockOut_game> gamelist = new List<Fe_Model_DrawTable_knockOut_game>();
                    #region 添加第一场比赛
                    Fe_Model_DrawTable_knockOut_game knockout = new Fe_Model_DrawTable_knockOut_game();
                    List<Fe_Model_DrawTable_knockOut_Player> game1 = new List<Fe_Model_DrawTable_knockOut_Player>();

                    #region 添加player1人员信息
                    Fe_Model_DrawTable_knockOut_Player leftplayer = new Fe_Model_DrawTable_knockOut_Player();

                    List<Fe_Model_DrawTable_Qualify_games_member> users = new List<Fe_Model_DrawTable_Qualify_games_member>();
                    if (matches[i].PLAYER1.IndexOf(",") > 0)
                    {
                        //双打
                        WeMemberModel memL1 = WeMemberDll.instance.GetModel(matches[i].PLAYER1.Split(',')[0]);
                        Fe_Model_DrawTable_Qualify_games_member user1 = new Fe_Model_DrawTable_Qualify_games_member();
                        user1.userimage = memL1.EXT1;
                        user1.username = memL1.USERNAME;

                        users.Add(user1);

                        WeMemberModel memL2 = WeMemberDll.instance.GetModel(matches[i].PLAYER1.Split(',')[1]);
                        Fe_Model_DrawTable_Qualify_games_member user2 = new Fe_Model_DrawTable_Qualify_games_member();
                        user2.userimage = memL2.EXT1;
                        user2.username = memL2.USERNAME;

                        users.Add(user2);
                    }
                    else
                    { 
                        //单打
                        WeMemberModel memL1 = WeMemberDll.instance.GetModel(matches[i].PLAYER1);
                        Fe_Model_DrawTable_Qualify_games_member user1 = new Fe_Model_DrawTable_Qualify_games_member();
                        user1.userimage = memL1.EXT1;
                        user1.username = memL1.USERNAME;

                        users.Add(user1);
                    }
                    leftplayer.users = users;
                    if (matches[i].STATE == 2)
                    {
                        leftplayer.score = Convert.ToInt32(matches[i].SCORE[0].ToString());
                        //判断是否获胜
                        if (matches[i].PLAYER1 == matches[i].WINNER)
                        {
                            leftplayer.win = true;
                        }
                        else
                        {
                            leftplayer.win = false;
                        }
                    }
                    game1.Add(leftplayer);
	                #endregion

                    #region 添加player2人员信息
                    Fe_Model_DrawTable_knockOut_Player rightplayer = new Fe_Model_DrawTable_knockOut_Player();

                    List<Fe_Model_DrawTable_Qualify_games_member> users2 = new List<Fe_Model_DrawTable_Qualify_games_member>();
                    if (matches[i].PLAYER2.IndexOf(",") > 0)
                    {
                        //双打
                        WeMemberModel memR1 = WeMemberDll.instance.GetModel(matches[i].PLAYER2.Split(',')[0]);
                        Fe_Model_DrawTable_Qualify_games_member user1 = new Fe_Model_DrawTable_Qualify_games_member();
                        user1.userimage = memR1.EXT1;
                        user1.username = memR1.USERNAME;

                        users2.Add(user1);

                        WeMemberModel memR2 = WeMemberDll.instance.GetModel(matches[i].PLAYER2.Split(',')[1]);
                        Fe_Model_DrawTable_Qualify_games_member user2 = new Fe_Model_DrawTable_Qualify_games_member();
                        user2.userimage = memR2.EXT1;
                        user2.username = memR2.USERNAME;

                        users2.Add(user2);
                    }
                    else
                    {
                        //单打
                        WeMemberModel memL1 = WeMemberDll.instance.GetModel(matches[i].PLAYER2);
                        Fe_Model_DrawTable_Qualify_games_member user1 = new Fe_Model_DrawTable_Qualify_games_member();
                        user1.userimage = memL1.EXT1;
                        user1.username = memL1.USERNAME;

                        users2.Add(user1);
                    }
                    rightplayer.users = users2;
                    if (matches[i].STATE == 2)
                    {
                        rightplayer.score = Convert.ToInt32(matches[i].SCORE[1].ToString());
                        //判断是否获胜
                        if (matches[i].PLAYER2 == matches[i].WINNER)
                        {
                            rightplayer.win = true;
                        }
                        else
                        {
                            rightplayer.win = false;
                        }
                    }
                    game1.Add(rightplayer);
                    #endregion
                    //构造一场比赛
                    knockout.game = game1;

                    //将比赛添加pair
                    gamelist.Add(knockout);
                    #endregion


                    #region 添加第二场比赛
                    Fe_Model_DrawTable_knockOut_game knockout2 = new Fe_Model_DrawTable_knockOut_game();
                    List<Fe_Model_DrawTable_knockOut_Player> game2 = new List<Fe_Model_DrawTable_knockOut_Player>();

                    #region 添加player1人员信息
                    Fe_Model_DrawTable_knockOut_Player leftplayer2 = new Fe_Model_DrawTable_knockOut_Player();

                    List<Fe_Model_DrawTable_Qualify_games_member> p2users = new List<Fe_Model_DrawTable_Qualify_games_member>();
                    if (matches[i+1].PLAYER1.IndexOf(",") > 0)
                    {
                        //双打
                        WeMemberModel memL1 = WeMemberDll.instance.GetModel(matches[i+1].PLAYER1.Split(',')[0]);
                        Fe_Model_DrawTable_Qualify_games_member user1 = new Fe_Model_DrawTable_Qualify_games_member();
                        user1.userimage = memL1.EXT1;
                        user1.username = memL1.USERNAME;

                        p2users.Add(user1);

                        WeMemberModel memL2 = WeMemberDll.instance.GetModel(matches[i+1].PLAYER1.Split(',')[1]);
                        Fe_Model_DrawTable_Qualify_games_member user2 = new Fe_Model_DrawTable_Qualify_games_member();
                        user2.userimage = memL2.EXT1;
                        user2.username = memL2.USERNAME;

                        p2users.Add(user2);
                    }
                    else
                    {
                        //单打
                        WeMemberModel memL1 = WeMemberDll.instance.GetModel(matches[i+1].PLAYER1);
                        Fe_Model_DrawTable_Qualify_games_member user1 = new Fe_Model_DrawTable_Qualify_games_member();
                        user1.userimage = memL1.EXT1;
                        user1.username = memL1.USERNAME;

                        p2users.Add(user1);
                    }
                    leftplayer2.users = p2users;
                    if (matches[i + 1].STATE == 2)
                    {
                        leftplayer2.score = Convert.ToInt32(matches[i + 1].SCORE[0].ToString());
                        //判断是否获胜
                        if (matches[i + 1].PLAYER1 == matches[i + 1].WINNER)
                        {
                            leftplayer2.win = true;
                        }
                        else
                        {
                            leftplayer2.win = false;
                        }
                    }
                    game2.Add(leftplayer2);
                    #endregion

                    #region 添加player2人员信息
                    Fe_Model_DrawTable_knockOut_Player rightplayer2 = new Fe_Model_DrawTable_knockOut_Player();

                    List<Fe_Model_DrawTable_Qualify_games_member> p2users2 = new List<Fe_Model_DrawTable_Qualify_games_member>();
                    if (matches[i+1].PLAYER2.IndexOf(",") > 0)
                    {
                        //双打
                        WeMemberModel memR1 = WeMemberDll.instance.GetModel(matches[i+1].PLAYER2.Split(',')[0]);
                        Fe_Model_DrawTable_Qualify_games_member user1 = new Fe_Model_DrawTable_Qualify_games_member();
                        user1.userimage = memR1.EXT1;
                        user1.username = memR1.USERNAME;

                        p2users2.Add(user1);

                        WeMemberModel memR2 = WeMemberDll.instance.GetModel(matches[i+1].PLAYER2.Split(',')[1]);
                        Fe_Model_DrawTable_Qualify_games_member user2 = new Fe_Model_DrawTable_Qualify_games_member();
                        user2.userimage = memR2.EXT1;
                        user2.username = memR2.USERNAME;

                        p2users2.Add(user2);
                    }
                    else
                    {
                        //单打
                        WeMemberModel memL1 = WeMemberDll.instance.GetModel(matches[i+1].PLAYER2);
                        Fe_Model_DrawTable_Qualify_games_member user1 = new Fe_Model_DrawTable_Qualify_games_member();
                        user1.userimage = memL1.EXT1;
                        user1.username = memL1.USERNAME;

                        p2users2.Add(user1);
                    }
                    rightplayer2.users = p2users2;

                    if (matches[i + 1].STATE == 2)
                    {
                        rightplayer2.score = Convert.ToInt32(matches[i + 1].SCORE[1].ToString());
                        //判断是否获胜
                        if (matches[i + 1].PLAYER2 == matches[i + 1].WINNER)
                        {
                            rightplayer2.win = true;
                        }
                        else
                        {
                            rightplayer2.win = false;
                        }
                    }

                    game2.Add(rightplayer2);
                    #endregion
                    knockout2.game = game2;
                    gamelist.Add(knockout2);
                    #endregion
                    pair.games = gamelist;
                    KnockOuts.Add(pair);
                }
            }
            else
            { 
                //决赛
                Fe_Model_DrawTable_knockOut_game_pair pair = new Fe_Model_DrawTable_knockOut_game_pair();
                List<Fe_Model_DrawTable_knockOut_game> gamelist = new List<Fe_Model_DrawTable_knockOut_game>();
                #region 添加第一场比赛
                Fe_Model_DrawTable_knockOut_game knockout = new Fe_Model_DrawTable_knockOut_game();
                List<Fe_Model_DrawTable_knockOut_Player> game1 = new List<Fe_Model_DrawTable_knockOut_Player>();

                #region 添加player1人员信息
                Fe_Model_DrawTable_knockOut_Player leftplayer = new Fe_Model_DrawTable_knockOut_Player();

                List<Fe_Model_DrawTable_Qualify_games_member> users = new List<Fe_Model_DrawTable_Qualify_games_member>();
                if (matches[0].PLAYER1.IndexOf(",") > 0)
                {
                    //双打
                    WeMemberModel memL1 = WeMemberDll.instance.GetModel(matches[0].PLAYER1.Split(',')[0]);
                    Fe_Model_DrawTable_Qualify_games_member user1 = new Fe_Model_DrawTable_Qualify_games_member();
                    user1.userimage = memL1.EXT1;
                    user1.username = memL1.USERNAME;

                    users.Add(user1);

                    WeMemberModel memL2 = WeMemberDll.instance.GetModel(matches[0].PLAYER1.Split(',')[1]);
                    Fe_Model_DrawTable_Qualify_games_member user2 = new Fe_Model_DrawTable_Qualify_games_member();
                    user2.userimage = memL2.EXT1;
                    user2.username = memL2.USERNAME;

                    users.Add(user2);
                }
                else
                {
                    //单打
                    WeMemberModel memL1 = WeMemberDll.instance.GetModel(matches[0].PLAYER1);
                    Fe_Model_DrawTable_Qualify_games_member user1 = new Fe_Model_DrawTable_Qualify_games_member();
                    user1.userimage = memL1.EXT1;
                    user1.username = memL1.USERNAME;

                    users.Add(user1);
                }
                leftplayer.users = users;
                leftplayer.score = Convert.ToInt32(matches[0].SCORE[0].ToString());
                //判断是否获胜
                if (matches[0].PLAYER1 == matches[0].WINNER)
                {
                    leftplayer.win = true;
                }
                else
                {
                    leftplayer.win = false;
                }
                game1.Add(leftplayer);
                #endregion

                #region 添加player2人员信息
                Fe_Model_DrawTable_knockOut_Player rightplayer = new Fe_Model_DrawTable_knockOut_Player();

                List<Fe_Model_DrawTable_Qualify_games_member> users2 = new List<Fe_Model_DrawTable_Qualify_games_member>();
                if (matches[0].PLAYER2.IndexOf(",") > 0)
                {
                    //双打
                    WeMemberModel memR1 = WeMemberDll.instance.GetModel(matches[0].PLAYER2.Split(',')[0]);
                    Fe_Model_DrawTable_Qualify_games_member user1 = new Fe_Model_DrawTable_Qualify_games_member();
                    user1.userimage = memR1.EXT1;
                    user1.username = memR1.USERNAME;

                    users2.Add(user1);

                    WeMemberModel memR2 = WeMemberDll.instance.GetModel(matches[0].PLAYER2.Split(',')[1]);
                    Fe_Model_DrawTable_Qualify_games_member user2 = new Fe_Model_DrawTable_Qualify_games_member();
                    user2.userimage = memR2.EXT1;
                    user2.username = memR2.USERNAME;

                    users2.Add(user2);
                }
                else
                {
                    //单打
                    WeMemberModel memL1 = WeMemberDll.instance.GetModel(matches[0].PLAYER2);
                    Fe_Model_DrawTable_Qualify_games_member user1 = new Fe_Model_DrawTable_Qualify_games_member();
                    user1.userimage = memL1.EXT1;
                    user1.username = memL1.USERNAME;

                    users2.Add(user1);
                }
                rightplayer.users = users2;
                rightplayer.score = Convert.ToInt32(matches[0].SCORE[1].ToString());
                //判断是否获胜
                if (matches[0].PLAYER2 == matches[0].WINNER)
                {
                    rightplayer.win = true;
                }
                else
                {
                    rightplayer.win = false;
                }

                game1.Add(rightplayer);
                #endregion
                //构造一场比赛
                knockout.game = game1;

                //将比赛添加pair
                gamelist.Add(knockout);
                #endregion
                pair.games = gamelist;
                KnockOuts.Add(pair);
            }

            return KnockOuts;
        }

        /// <summary>
        /// 去掉games和game
        /// </summary>
        /// <param name="_ItemId"></param>
        /// <param name="_Round"></param>
        /// <returns></returns>
        private List<object> GetKnockOut2(string _ItemId, int _Round)
        {
            List<object> KnockOuts = new List<object>();
            List<WeMatchModel> matches = WeMatchDll.instance.GetMatchlistbyContRound(_ItemId, _Round.ToString());

            WeTourContModel tcModel = WeTourContentDll.instance.GetModelbyId(_ItemId);
            if (matches.Count == 0)
            {
                return KnockOuts;
            }
            if (matches.Count >= 2)
            {
                //非决赛
                for (int i = 0; i < matches.Count; i += 2)
                {
                    Fe_Model_DrawTable_knockOut_game_pair pair = new Fe_Model_DrawTable_knockOut_game_pair();
                    List<object> gamelist = new List<object>();
                    #region 添加第一场比赛
                    Fe_Model_DrawTable_knockOut_game knockout = new Fe_Model_DrawTable_knockOut_game();
                    List<Fe_Model_DrawTable_knockOut_Player> game1 = new List<Fe_Model_DrawTable_knockOut_Player>();

                    #region 添加player1人员信息
                    Fe_Model_DrawTable_knockOut_Player leftplayer = new Fe_Model_DrawTable_knockOut_Player();

                    List<Fe_Model_DrawTable_Qualify_games_member> users = new List<Fe_Model_DrawTable_Qualify_games_member>();
                    if (tcModel.ContentType == "团体")
                    {
                        weTeamModel team1 = weTeamdll.instance.GetModel(matches[i].PLAYER1);
                        Fe_Model_DrawTable_Qualify_games_member user1 = new Fe_Model_DrawTable_Qualify_games_member();
                        user1.userimage = team1.teamImage;
                        user1.username = team1.TEAMNAME;

                        users.Add(user1);
                    }
                    else
                    {
                        if (matches[i].PLAYER1.IndexOf(",") > 0)
                        {
                            //双打
                            WeMemberModel memL1 = WeMemberDll.instance.GetModel(matches[i].PLAYER1.Split(',')[0]);
                            Fe_Model_DrawTable_Qualify_games_member user1 = new Fe_Model_DrawTable_Qualify_games_member();
                            user1.userimage = memL1.EXT1;
                            user1.username = memL1.USERNAME;

                            users.Add(user1);

                            WeMemberModel memL2 = WeMemberDll.instance.GetModel(matches[i].PLAYER1.Split(',')[1]);
                            Fe_Model_DrawTable_Qualify_games_member user2 = new Fe_Model_DrawTable_Qualify_games_member();
                            user2.userimage = memL2.EXT1;
                            user2.username = memL2.USERNAME;

                            users.Add(user2);
                        }
                        else
                        {
                            //单打
                            WeMemberModel memL1 = WeMemberDll.instance.GetModel(matches[i].PLAYER1);
                            Fe_Model_DrawTable_Qualify_games_member user1 = new Fe_Model_DrawTable_Qualify_games_member();
                            user1.userimage = memL1.EXT1;
                            user1.username = memL1.USERNAME;

                            users.Add(user1);
                        }
                    }
                    leftplayer.users = users;
                    if (matches[i].STATE == 2)
                    {
                        try
                        {
                            leftplayer.score = Convert.ToInt32(matches[i].SCORE[0].ToString());
                        }
                        catch (Exception)
                        {
                            leftplayer.score = 0;
                        }
                       
                        //判断是否获胜
                        if (matches[i].PLAYER1 == matches[i].WINNER)
                        {
                            leftplayer.win = true;
                        }
                        else
                        {
                            leftplayer.win = false;
                        }
                    }
                    
                    #endregion
                    game1.Add(leftplayer);

                    #region 添加player2人员信息
                    Fe_Model_DrawTable_knockOut_Player rightplayer = new Fe_Model_DrawTable_knockOut_Player();

                    List<Fe_Model_DrawTable_Qualify_games_member> users2 = new List<Fe_Model_DrawTable_Qualify_games_member>();

                    if (tcModel.ContentType == "团体")
                    {
                        weTeamModel team1 = weTeamdll.instance.GetModel(matches[i].PLAYER2);
                        Fe_Model_DrawTable_Qualify_games_member user1 = new Fe_Model_DrawTable_Qualify_games_member();
                        user1.userimage = team1.teamImage;
                        user1.username = team1.TEAMNAME;

                        users2.Add(user1);
                    }
                    else
                    {
                        if (matches[i].PLAYER2.IndexOf(",") > 0)
                        {
                            //双打
                            WeMemberModel memR1 = WeMemberDll.instance.GetModel(matches[i].PLAYER2.Split(',')[0]);
                            Fe_Model_DrawTable_Qualify_games_member user1 = new Fe_Model_DrawTable_Qualify_games_member();
                            user1.userimage = memR1.EXT1;
                            user1.username = memR1.USERNAME;

                            users2.Add(user1);

                            WeMemberModel memR2 = WeMemberDll.instance.GetModel(matches[i].PLAYER2.Split(',')[1]);
                            Fe_Model_DrawTable_Qualify_games_member user2 = new Fe_Model_DrawTable_Qualify_games_member();
                            user2.userimage = memR2.EXT1;
                            user2.username = memR2.USERNAME;

                            users2.Add(user2);
                        }
                        else
                        {
                            //单打
                            WeMemberModel memL1 = WeMemberDll.instance.GetModel(matches[i].PLAYER2);
                            Fe_Model_DrawTable_Qualify_games_member user1 = new Fe_Model_DrawTable_Qualify_games_member();
                            user1.userimage = memL1.EXT1;
                            user1.username = memL1.USERNAME;

                            users2.Add(user1);
                        }
                    }
                    rightplayer.users = users2;
                    if (matches[i].STATE == 2)
                    {
                        try
                        {
                            rightplayer.score = Convert.ToInt32(matches[i].SCORE[1].ToString());
                        }
                        catch (Exception)
                        {
                            rightplayer.score = Convert.ToInt32(matches[i].SCORE[1].ToString());
                        }
                       
                        //判断是否获胜
                        if (matches[i].PLAYER2 == matches[i].WINNER)
                        {
                            rightplayer.win = true;
                        }
                        else
                        {
                            rightplayer.win = false;
                        }
                    }
                    
                    #endregion
                    game1.Add(rightplayer);
                    //构造一场比赛
                    //knockout.game = game1;

                    //将比赛添加pair
                    gamelist.Add(game1);
                    #endregion


                    #region 添加第二场比赛
                    Fe_Model_DrawTable_knockOut_game knockout2 = new Fe_Model_DrawTable_knockOut_game();
                    List<Fe_Model_DrawTable_knockOut_Player> game2 = new List<Fe_Model_DrawTable_knockOut_Player>();

                    #region 添加player1人员信息
                    Fe_Model_DrawTable_knockOut_Player leftplayer2 = new Fe_Model_DrawTable_knockOut_Player();

                    List<Fe_Model_DrawTable_Qualify_games_member> p2users = new List<Fe_Model_DrawTable_Qualify_games_member>();

                    if (tcModel.ContentType == "团体")
                    {
                        weTeamModel team1 = weTeamdll.instance.GetModel(matches[i+1].PLAYER1);
                        Fe_Model_DrawTable_Qualify_games_member user1 = new Fe_Model_DrawTable_Qualify_games_member();
                        user1.userimage = team1.teamImage;
                        user1.username = team1.TEAMNAME;

                        p2users.Add(user1);
                    }
                    else
                    {
                        if (matches[i + 1].PLAYER1.IndexOf(",") > 0)
                        {
                            //双打
                            WeMemberModel memL1 = WeMemberDll.instance.GetModel(matches[i + 1].PLAYER1.Split(',')[0]);
                            Fe_Model_DrawTable_Qualify_games_member user1 = new Fe_Model_DrawTable_Qualify_games_member();
                            user1.userimage = memL1.EXT1;
                            user1.username = memL1.USERNAME;

                            p2users.Add(user1);

                            WeMemberModel memL2 = WeMemberDll.instance.GetModel(matches[i + 1].PLAYER1.Split(',')[1]);
                            Fe_Model_DrawTable_Qualify_games_member user2 = new Fe_Model_DrawTable_Qualify_games_member();
                            user2.userimage = memL2.EXT1;
                            user2.username = memL2.USERNAME;

                            p2users.Add(user2);
                        }
                        else
                        {
                            //单打
                            WeMemberModel memL1 = WeMemberDll.instance.GetModel(matches[i + 1].PLAYER1);
                            Fe_Model_DrawTable_Qualify_games_member user1 = new Fe_Model_DrawTable_Qualify_games_member();
                            user1.userimage = memL1.EXT1;
                            user1.username = memL1.USERNAME;

                            p2users.Add(user1);
                        }
                    }
                    leftplayer2.users = p2users;
                    if (matches[i + 1].STATE == 2)
                    {
                        try
                        {
                            leftplayer2.score = Convert.ToInt32(matches[i + 1].SCORE[0].ToString());
                        }
                        catch (Exception)
                        {
                            leftplayer2.score = 0;
                        }
                        
                        //判断是否获胜
                        if (matches[i + 1].PLAYER1 == matches[i + 1].WINNER)
                        {
                            leftplayer2.win = true;
                        }
                        else
                        {
                            leftplayer2.win = false;
                        }
                    }
                    
                    #endregion
                    game2.Add(leftplayer2);

                    #region 添加player2人员信息
                    Fe_Model_DrawTable_knockOut_Player rightplayer2 = new Fe_Model_DrawTable_knockOut_Player();

                    List<Fe_Model_DrawTable_Qualify_games_member> p2users2 = new List<Fe_Model_DrawTable_Qualify_games_member>();
                    if (tcModel.ContentType == "团体")
                    {
                        weTeamModel team1 = weTeamdll.instance.GetModel(matches[i+1].PLAYER2);
                        Fe_Model_DrawTable_Qualify_games_member user1 = new Fe_Model_DrawTable_Qualify_games_member();
                        user1.userimage = team1.teamImage;
                        user1.username = team1.TEAMNAME;

                        p2users2.Add(user1);
                    }
                    else
                    {
                        if (matches[i + 1].PLAYER2.IndexOf(",") > 0)
                        {
                            //双打
                            WeMemberModel memR1 = WeMemberDll.instance.GetModel(matches[i + 1].PLAYER2.Split(',')[0]);
                            Fe_Model_DrawTable_Qualify_games_member user1 = new Fe_Model_DrawTable_Qualify_games_member();
                            user1.userimage = memR1.EXT1;
                            user1.username = memR1.USERNAME;

                            p2users2.Add(user1);

                            WeMemberModel memR2 = WeMemberDll.instance.GetModel(matches[i + 1].PLAYER2.Split(',')[1]);
                            Fe_Model_DrawTable_Qualify_games_member user2 = new Fe_Model_DrawTable_Qualify_games_member();
                            user2.userimage = memR2.EXT1;
                            user2.username = memR2.USERNAME;

                            p2users2.Add(user2);
                        }
                        else
                        {
                            //单打
                            WeMemberModel memL1 = WeMemberDll.instance.GetModel(matches[i + 1].PLAYER2);
                            Fe_Model_DrawTable_Qualify_games_member user1 = new Fe_Model_DrawTable_Qualify_games_member();
                            user1.userimage = memL1.EXT1;
                            user1.username = memL1.USERNAME;

                            p2users2.Add(user1);
                        }
                    }
                    rightplayer2.users = p2users2;

                    if (matches[i + 1].STATE == 2)
                    {
                        try
                        {
                            rightplayer2.score = Convert.ToInt32(matches[i + 1].SCORE[1].ToString());
                        }
                        catch (Exception)
                        {
                            rightplayer2.score = 0;
                        }
                        
                        //判断是否获胜
                        if (matches[i + 1].PLAYER2 == matches[i + 1].WINNER && matches[i + 1].WINNER!="")
                        {
                            rightplayer2.win = true;
                        }
                        else
                        {
                            rightplayer2.win = false;
                        }
                    }

                   
                    #endregion
                    game2.Add(rightplayer2);
                    //knockout2.game = game2;
                    gamelist.Add(game2);
                    #endregion
                    //pair.games = gamelist;
                    KnockOuts.Add(gamelist);
                }
            }
            else
            {
                //决赛
                Fe_Model_DrawTable_knockOut_game_pair pair = new Fe_Model_DrawTable_knockOut_game_pair();
                List<object> gamelist = new List<object>();
                #region 添加第一场比赛
                List<object> game1 = new List<object>();

                #region 添加player1人员信息
                Fe_Model_DrawTable_knockOut_Player leftplayer = new Fe_Model_DrawTable_knockOut_Player();

                List<Fe_Model_DrawTable_Qualify_games_member> users = new List<Fe_Model_DrawTable_Qualify_games_member>();
                if (matches[0].PLAYER1.IndexOf(",") > 0)
                {
                    //双打
                    WeMemberModel memL1 = WeMemberDll.instance.GetModel(matches[0].PLAYER1.Split(',')[0]);
                    Fe_Model_DrawTable_Qualify_games_member user1 = new Fe_Model_DrawTable_Qualify_games_member();
                    user1.userimage = memL1.EXT1;
                    user1.username = memL1.USERNAME;

                    users.Add(user1);

                    WeMemberModel memL2 = WeMemberDll.instance.GetModel(matches[0].PLAYER1.Split(',')[1]);
                    Fe_Model_DrawTable_Qualify_games_member user2 = new Fe_Model_DrawTable_Qualify_games_member();
                    user2.userimage = memL2.EXT1;
                    user2.username = memL2.USERNAME;

                    users.Add(user2);
                }
                else
                {
                    //单打
                    WeMemberModel memL1 = WeMemberDll.instance.GetModel(matches[0].PLAYER1);
                    Fe_Model_DrawTable_Qualify_games_member user1 = new Fe_Model_DrawTable_Qualify_games_member();
                    user1.userimage = memL1.EXT1;
                    user1.username = memL1.USERNAME;

                    users.Add(user1);
                }
                leftplayer.users = users;
                try
                {
                    leftplayer.score = Convert.ToInt32(matches[0].SCORE[0].ToString());
                }
                catch (Exception)
                {
                    leftplayer.score = 0;
                }
                
                //判断是否获胜                
                if (matches[0].PLAYER1 == matches[0].WINNER && matches[0].WINNER!="")
                {
                    leftplayer.win = true;
                }
                else
                {
                    leftplayer.win = false;
                }
                
                #endregion
                game1.Add(leftplayer);

                #region 添加player2人员信息
                Fe_Model_DrawTable_knockOut_Player rightplayer = new Fe_Model_DrawTable_knockOut_Player();

                List<Fe_Model_DrawTable_Qualify_games_member> users2 = new List<Fe_Model_DrawTable_Qualify_games_member>();
                if (matches[0].PLAYER2.IndexOf(",") > 0)
                {
                    //双打
                    WeMemberModel memR1 = WeMemberDll.instance.GetModel(matches[0].PLAYER2.Split(',')[0]);
                    Fe_Model_DrawTable_Qualify_games_member user1 = new Fe_Model_DrawTable_Qualify_games_member();
                    user1.userimage = memR1.EXT1;
                    user1.username = memR1.USERNAME;

                    users2.Add(user1);

                    WeMemberModel memR2 = WeMemberDll.instance.GetModel(matches[0].PLAYER2.Split(',')[1]);
                    Fe_Model_DrawTable_Qualify_games_member user2 = new Fe_Model_DrawTable_Qualify_games_member();
                    user2.userimage = memR2.EXT1;
                    user2.username = memR2.USERNAME;

                    users2.Add(user2);
                }
                else
                {
                    //单打
                    WeMemberModel memL1 = WeMemberDll.instance.GetModel(matches[0].PLAYER2);
                    Fe_Model_DrawTable_Qualify_games_member user1 = new Fe_Model_DrawTable_Qualify_games_member();
                    user1.userimage = memL1.EXT1;
                    user1.username = memL1.USERNAME;

                    users2.Add(user1);
                }
                rightplayer.users = users2;
                try
                {
                    rightplayer.score = Convert.ToInt32(matches[0].SCORE[1].ToString());
                }
                catch (Exception)
                {
                    rightplayer.score = 0;
                }
                
                //判断是否获胜
                if (matches[0].PLAYER2 == matches[0].WINNER && matches[0].WINNER!="")
                {
                    rightplayer.win = true;
                }
                else
                {
                    rightplayer.win = false;
                }                
                #endregion
                game1.Add(rightplayer);
                //构造一场比赛

                //将比赛添加pair
                gamelist.Add(game1);
                #endregion
                //pair.games = gamelist;
                KnockOuts.Add(gamelist);
            }

            return KnockOuts;
        }

        public List<Fe_Model_Filter> eventScheduleFilter(string _Id)
        {
            //添加场馆
            List<Fe_Model_Filter> itemlist = new List<Fe_Model_Filter>();

            //添加日期
            List<Dictionary<string, string>> dates = WeTourSceduleDll.instance.GetMatchdates(_Id);
            for(int i=0;i<dates.Count;i++)
            {
                Fe_Model_Filter date = new Fe_Model_Filter();
                Dictionary<string, string> item = dates[i];
                string _text = "";
                item.TryGetValue("text", out _text);
                date.text = _text;

                string _value = "";
                item.TryGetValue("value",out _value);
                date.value = _value;
                List<Dictionary<string, string>> locations = WeTourSceduleDll.instance.GetMatchCourts(_Id, _value);
                date.children = locations;
                itemlist.Add(date);
            }
            return itemlist;
        }

        

        #endregion
        #region 赛事报名
        public Fe_Model_eventGroups eventGroups(string _TourSys)
        {
            Fe_Model_eventGroups model = new Fe_Model_eventGroups();
            //根据赛事主键获取所有的组
            List<Fe_Model_eventGroups_Group> listG = new List<Fe_Model_eventGroups_Group>();
            List<WeTourContModel> grouplist = WeTourContentDll.instance.GetGroups(_TourSys);
            for (int i=0;i < grouplist.Count; i++)
            {
                Fe_Model_eventGroups_Group group = new Fe_Model_eventGroups_Group();
                group.id = (i + 1).ToString();
                group.name = grouplist[i].TourDate;

                List<Fe_Model_eventGroups_items> items = new List<Fe_Model_eventGroups_items>();
                //根据赛事，组，再获取对应的比赛项目
                List<WeTourContModel> Gitems = WeTourContentDll.instance.GetcontentbyGroup(_TourSys,group.name);

                //判断是否是团体赛，团体赛的groupid就是itemid
                WeTourModel tour = WeTourDll.instance.GetModelbySys(_TourSys);
                if (tour.MATCHCONTENT == "group")
                {
                    group.id = Gitems[0].id;
                    group.price = WeTourContentDll.instance.GetContentApplyFee(Gitems[0].id);
                }
                for (int j = 0; j < Gitems.Count; j++)
                {
                    Fe_Model_eventGroups_items item = new Fe_Model_eventGroups_items();
                    item.id = Gitems[j].id;
                    item.name = Gitems[j].ContentName;                    
                    if (Gitems[j].ContentType.IndexOf("双") > 0)
                    {
                        item.needPartner = true;
                    }
                    else
                    {
                        item.needPartner = false ;
                    }

                    //添加报名限制,要在管理系统中增加限制条件
                    Fe_Model_eventGroups_item_restriction res = new Fe_Model_eventGroups_item_restriction();
                    //年龄限制,
                    string ageres=Gitems[j].ext5;
                    try
                    {
                        string[] ages = ageres.Split('|');
                        res.minAmountAge = Convert.ToInt32(ages[0]);
                        res.maxAge = Convert.ToInt32(ages[1]);
                        res.minAge = Convert.ToInt32(ages[2]);
                    }
                    catch (Exception)
                    {
                        res.minAmountAge = 100;
                        res.maxAge = 100;
                        res.minAge = 0;
                    }
                   

                    if (Gitems[j].ContentType.IndexOf("男") >= 0)
                    {
                        res.gender = "male";
                        res.isMixedPair = false ;
                    }
                    else if (Gitems[j].ContentType.IndexOf("女") >= 0)
                    {
                        res.gender = "female";
                        res.isMixedPair = false;
                    }
                    else
                    {
                        res.gender = Gitems[j].ContentType;
                        res.isMixedPair = true;
                    }
                    try
                    {
                        item.price = Convert.ToDecimal(Gitems[i].ext3);//添加报名单价
                    }
                    catch (Exception)
                    {
                        item.price = 0;//添加报名单价
                    }
                    
                    item.restriction = res;
                    items.Add(item);
                }

                    group.items = items;
                listG.Add(group);
            }
            model.groups = listG;
            return model;
        }

        public List<Fe_Model_registeredUsers> registeredUsers(string _itemId)
        {
            List<Fe_Model_registeredUsers> list = new List<Fe_Model_registeredUsers>();
            List<WeTourApplyModel> applyList = WeTourApplyDll.instance.getContentApplyByid(_itemId);
            bool isdoub = false ;
            WeTourContModel cont = WeTourContentDll.instance.GetModelbyId(_itemId);
            if (cont.ContentType.IndexOf("双") > 0)
            {
                isdoub = true;
            }
            foreach (WeTourApplyModel model in applyList)
            {
                Fe_Model_registeredUsers register = new Fe_Model_registeredUsers();
                if (isdoub)
                {
                    //按照双打添加
                    string[] names = new string[2];
                    names[0] = model.MemberName;
                    names[1] = model.ParName;
                    register.name = names;
                    string[] thmbImgs = new string[2];
                    thmbImgs[0] = model.MemberImg;
                    thmbImgs[1]=model.ParImg;
                    register.thumbImgUrl = thmbImgs;
                    register.registerDatestr = RenderDate(model.APPLYDATE);
                }
                else
                { 
                    //按照单打添加
                    string[] names = new string[1];
                    names[0] = model.MemberName;
                    register.name = names;
                    string[] thmbImgs = new string[1];
                    thmbImgs[0] = model.MemberImg;

                    register.thumbImgUrl = thmbImgs;
                    register.registerDatestr = RenderDate(model.APPLYDATE);
                }
                list.Add(register);
            }
            return list;
        }

        /// <summary>
        /// 获得结果
        /// </summary>
        /// <param name="_eventId"></param>
        /// <returns></returns>
        public List<Fe_Model_DrawResult> getDrawResult(string _eventId)
        {
            List<Fe_Model_DrawResult> list = new List<Fe_Model_DrawResult>();
            List<WeTourContModel> contlist = WeTourContentDll.instance.GetContentlist(_eventId);
            for (int i = 0; i < contlist.Count; i++)
            {
                Fe_Model_DrawResult model = new Fe_Model_DrawResult();
                model.name = contlist[i].TourDate+"|"+ contlist[i].ContentName;//项目名称
                //add rounds of matches
                List<Fe_Model_RoundMatches> roundM_list = new List<Fe_Model_RoundMatches>();
                List<WeMatchModel> listR = WeMatchDll.instance.GetContentRounds(contlist[i].id);
                foreach (WeMatchModel round in listR)
                {
                    Fe_Model_RoundMatches roundM = new Fe_Model_RoundMatches();
                    //render roundname
                    int cap = Convert.ToInt32(contlist[i].SignQty);
                    int rou = Convert.ToInt32(round.ROUND);
                    roundM.roundName = WeTourDll.instance.RenderRound(rou, cap);
                    roundM.roundmatches = GetRoundMatch(contlist[i].id, rou);
                    roundM_list.Add(roundM);
                }
                model.round = roundM_list;
                list.Add(model);
            }
            return list;
        }

        /// <summary>
        /// 按照前端要求修改
        /// </summary>
        /// <param name="_eventId"></param>
        /// <returns></returns>
        public List<Dictionary<string,object>> getDrawResult2(string _eventId)
        {
            List<Dictionary<string,object>> list = new List<Dictionary<string,object>>();
            List<WeTourContModel> contlist = WeTourContentDll.instance.GetContentlist(_eventId);
            for (int i = 0; i < contlist.Count; i++)
            {
                Dictionary<string,object> model = new Dictionary<string,object>();
                string ContName = contlist[i].TourDate + "|" + contlist[i].ContentName;//项目名称
                model.Add("name",ContName);

                //add rounds of matches
                List<Fe_Model_RoundMatches> roundM_list = new List<Fe_Model_RoundMatches>();
                List<WeMatchModel> listR = WeMatchDll.instance.GetContentRounds(contlist[i].id);
                foreach (WeMatchModel round in listR)
                {
                    Fe_Model_RoundMatches roundM = new Fe_Model_RoundMatches();
                    //render roundname
                    int cap = Convert.ToInt32(contlist[i].SignQty);
                    int rou = Convert.ToInt32(round.ROUND);
                    roundM.roundName = WeTourDll.instance.RenderRound(rou, cap);
                    List<Fe_Model_MatchMain> match_list = GetRoundMatch(contlist[i].id, rou);
                    //构造比赛
                    List<object> cpmatch = new List<object>();
                    foreach (Fe_Model_MatchMain match in match_list)
                    {
                        List<Dictionary<string, object>> matches = new List<Dictionary<string, object>>();
                        Dictionary<string, object> matchinfo = new Dictionary<string, object>();
                        matchinfo.Add("member", match.player1);
                        matchinfo.Add("score", match.score1);
                        matches.Add(matchinfo);

                        Dictionary<string, object> matchinfo2 = new Dictionary<string, object>();
                        matchinfo2.Add("member", match.player2);
                        matchinfo2.Add("score", match.score2);
                        matches.Add(matchinfo2);

                        cpmatch.Add(matches);
                    }

                    model.Add(roundM.roundName, cpmatch);
                }
                list.Add(model);
            }
            return list;
        }

        private List<Fe_Model_MatchMain> GetRoundMatch(string _ContentId, int Round)
        {
            List<Fe_Model_MatchMain> Roundmatch_list = new List<Fe_Model_MatchMain>();
            List<WeMatchModel> match_list = WeMatchDll.instance.GetFinishedMatchlistbyContRound(_ContentId, Round.ToString());
            foreach (WeMatchModel match in match_list)
            {
                Fe_Model_MatchMain roundmatch = new Fe_Model_MatchMain();
                roundmatch.score1 = Convert.ToInt32(match.SCORE[0].ToString());
                roundmatch.score2 = Convert.ToInt32(match.SCORE[1].ToString());

                #region add player
                if (match.PLAYER1.IndexOf(",") > 0)
                {
                    //double
                    List<Fe_Model_member> member_list_1 = new List<Fe_Model_member>();   
                    WeMemberModel p1l = WeMemberDll.instance.GetModel(match.PLAYER1.Split(',')[0]);
                    Fe_Model_member member_1_L = new Fe_Model_member();
                    member_1_L.memberName = p1l.USERNAME;
                    member_1_L.memberEnglishName = p1l.NAME;
                    member_1_L.memberThumb = p1l.EXT1;
                    member_list_1.Add(member_1_L);

                    WeMemberModel p1R = WeMemberDll.instance.GetModel(match.PLAYER1.Split(',')[1]);
                    Fe_Model_member member_1_R = new Fe_Model_member();
                    member_1_R.memberName = p1R.USERNAME;
                    member_1_R.memberEnglishName = p1R.NAME;
                    member_1_R.memberThumb = p1R.EXT1;
                    member_list_1.Add(member_1_R);

                    roundmatch.player1 = member_list_1;

                    //ADD PLYAER2 
                    List<Fe_Model_member> member_list_2 = new List<Fe_Model_member>();
                    WeMemberModel p2l = WeMemberDll.instance.GetModel(match.PLAYER2.Split(',')[0]);
                    Fe_Model_member member_2_L = new Fe_Model_member();
                    member_2_L.memberName = p2l.USERNAME;
                    member_2_L.memberEnglishName = p2l.NAME;
                    member_2_L.memberThumb = p2l.EXT1;
                    member_list_2.Add(member_2_L);

                    WeMemberModel p2R = WeMemberDll.instance.GetModel(match.PLAYER2.Split(',')[1]);
                    Fe_Model_member member_2_R = new Fe_Model_member();
                    member_1_R.memberName = p2R.USERNAME;
                    member_1_R.memberEnglishName = p2R.NAME;
                    member_1_R.memberThumb = p2R.EXT1;
                    member_list_2.Add(member_1_R);

                    roundmatch.player2 = member_list_2;
                }
                else
                { 
                    //single
                    List<Fe_Model_member> member_list_1 = new List<Fe_Model_member>();
                    WeMemberModel p1l = WeMemberDll.instance.GetModel(match.PLAYER1);
                    Fe_Model_member member_1_L = new Fe_Model_member();
                    member_1_L.memberName = p1l.USERNAME;
                    member_1_L.memberEnglishName = p1l.NAME;
                    member_1_L.memberThumb = p1l.EXT1;
                    member_list_1.Add(member_1_L);
                   
                    roundmatch.player1 = member_list_1;

                    //ADD PLYAER2 
                    List<Fe_Model_member> member_list_2 = new List<Fe_Model_member>();
                    WeMemberModel p2l = WeMemberDll.instance.GetModel(match.PLAYER2);
                    Fe_Model_member member_2_L = new Fe_Model_member();
                    member_2_L.memberName = p2l.USERNAME;
                    member_2_L.memberEnglishName = p2l.NAME;
                    member_2_L.memberThumb = p2l.EXT1;
                    member_list_2.Add(member_2_L);

                    roundmatch.player2 = member_list_2;
                }
                #endregion

                Roundmatch_list.Add(roundmatch);
            }
            return Roundmatch_list;
        }

        #endregion

        #region 辅助方法
        public string RenderDate(string _DateTime) {
            string RenDate = "";
            DateTime a = Convert.ToDateTime(_DateTime);
            TimeSpan ts1 = new TimeSpan(a.Ticks);
            TimeSpan ts2 = new TimeSpan(DateTime.Now.Ticks);
            TimeSpan ts = ts1.Subtract(ts2).Duration();//时间差距的绝对值

            if (ts.Days > 0)
            {
                RenDate = ts.Days.ToString() + "天前";
            }
            else if (ts.Hours > 0)
            {
                RenDate = ts.Hours.ToString() + "小时前";
            }
            else if (ts.Minutes > 0)
            {
                RenDate = ts.Minutes.ToString() + "分钟前";
            }
            else
            {
                RenDate = ts.Seconds.ToString() + "秒前";
            }
            return RenDate;
        }
        #endregion

        #region 赛事排名AUG08-
        public List<Dictionary<string, string>> Get_RankingFilter()
        {
            List<Dictionary<string, string>> getFilters = new List<Dictionary<string, string>>();

            return getFilters;
        }

        #endregion

        #region 我的赛事（Aug14
        public Fe_fetchMyMatch GetMyMatch(string _userId)
        {
            Fe_fetchMyMatch model = new Fe_fetchMyMatch();
            model.single_ytdwl = WeTour_MyMatchs.instance.Get_YTDWL(_userId,"单打");
            model.single_totalwl = WeTour_MyMatchs.instance.Get_TotalWL(_userId, "单打");
            model.couple_ytdwl = WeTour_MyMatchs.instance.Get_YTDWL(_userId, "双打");
            model.couple_totalwl = WeTour_MyMatchs.instance.Get_TotalWL(_userId, "双打");
            List<Fe_fetchMyMatch_singleMatch> singleMatch = null;
            List<Fe_fetchMyMatch_singleMatch> coupleMatch = null;
            WeTour_MyMatchs.instance.GetMyMatches(_userId, out singleMatch, out coupleMatch);
            model.singleMatch = singleMatch;
            model.coupleMatch = coupleMatch;
            return model;
        }

        public Fe_fetchMyPractice GetMyPractice(string _userId)
        {
            Fe_fetchMyPractice model = new Fe_fetchMyPractice();
            model.ytdwl = Biz_MyPractice.instance.Get_YTD_Practice(_userId);
            model.totalwl = Biz_MyPractice.instance.Get_Total_Practice(_userId);
            model.singlePractice = Biz_MyPractice.instance.Get_Single_Practices(_userId);
            model.couplePractice = Biz_MyPractice.instance.Get_Couple_Practices(_userId);
            return model;
        }
        #endregion

        #region 比赛详情
        /// <summary>
        /// 比赛详情，9月29
        /// </summary>
        /// <param name="_sys"></param>
        /// <returns></returns>
        //public Fe_Model_MatchInfo Fe_Match_GetInfo(string _sys)
        //{
        //    Fe_Model_MatchInfo model = new Fe_Model_MatchInfo();
        //    WeMatchModel match = WeMatchDll.instance.GetModel(_sys);//比赛实体
        //    WeTourModel tour = WeTourDll.instance.GetModelbySys(match.TOURSYS);//赛事实体
        //    model.status = Convert.ToInt32(match.STATE);//比赛状态
        //    model.eventName = tour.NAME;//赛事信息
        //    //获取项目信息
        //    WeTourContModel cont = WeTourContentDll.instance.GetModelbyId(match.ContentID);
        //    model.matchName = cont.TourDate+"|"+cont.ContentName;
        //    model.matchName +="|"+ WeTourDll.instance.RenderRound(Convert.ToInt32(match.ROUND), Convert.ToInt32(cont.SignQty));
        //    if (match.ROUND == 0)
        //    { 
        //        //小组赛，则增加小组组别
        //        model.matchName +="|第"+ match.etc1+"组";
        //    }

        //    model.time = match.MATCHDATE;//比赛日期
        //    //获取比赛地点
        //    model.location= CourtDll.Get_Instance().GetCourtFullName(match.COURTID);

        //    model.gameTime = "30";//比赛用时

        //    #region 添加team
        //    //获取比赛双方的比分以及人员信息
        //    List<Dictionary<string, object>> teams = new List<Dictionary<string, object>>();
        //    //添加player1
            
        //    Dictionary<string, object> team1 = new Dictionary<string, object>();
        //    Dictionary<string, object> team2 = new Dictionary<string, object>();
        //    if (match.STATE == 2 && match.PLAYER1 == match.WINNER)
        //    {
        //        team1.Add("win", true);
        //        team2.Add("win", false);
        //    }
        //    else if (match.STATE == 2 && match.PLAYER2 == match.WINNER)
        //    {
        //        team1.Add("win", false);
        //        team2.Add("win", true);
        //    }
        //    else
        //    {
        //        team1.Add("win", false);
        //        team2.Add("win", false);
        //    }

        //    //比分
        //    if (match.SCORE.ToString().Length == 2)
        //    { 
        //        team1.Add("score",Convert.ToInt32(match.SCORE.Substring(0,1)));
        //        team2.Add("score",Convert.ToInt32(match.SCORE.Substring(1,1)));
        //    }

        //    //成员
        //    team1.Add("user",getUserInfo(match.PLAYER1));
        //    team2.Add("user",getUserInfo(match.PLAYER2));
           
        //    teams.Add(team1);
        //    teams.Add(team2);
        //    #endregion
        //    model.teams = teams;

        //    #region 添加盘分详情
        //    List<Dictionary<string, object>> sets = new List<Dictionary<string, object>>();
            
        //    //获得盘分
        //    #region  盘分
        //    List<SetModel> setmodellist = SetDll.Get_Instance().GetModelList(_sys);
        //    foreach (SetModel set_model in setmodellist)
        //    {
        //        Dictionary<string, object> set = new Dictionary<string, object>();

        //        //获取当盘比分
        //        set.Add("scores", GetSetScore(_sys, set_model.SYS));
        //        //添加盘分详情
        //        set.Add("games", GetSetGames(_sys,set_model.SYS));
                
        //        sets.Add(set);
        //    }
        //    #endregion
            
        //    model.sets = sets;

        //    #endregion


        //    return model;
        //}

        /// <summary>
        /// 修改比赛详情，10月10日，根据10月9日开会讨论的json结构
        /// 
        /// </summary>
        /// <param name="_sys"></param>
        /// <returns></returns>
        public Fe_Model_MatchInfo Fe_Match_GetInfo2(string _sys)
        {
            Fe_Model_MatchInfo model = new Fe_Model_MatchInfo();
            WeMatchModel match = WeMatchDll.instance.GetModel(_sys);//比赛实体
            WeTourModel tour = WeTourDll.instance.GetModelbySys(match.TOURSYS);//赛事实体
            model.status = Convert.ToInt32(match.STATE);//比赛状态
            model.eventName = tour.NAME;//赛事信息
            //获取项目信息
            WeTourContModel cont = WeTourContentDll.instance.GetModelbyId(match.ContentID);
            model.matchName = cont.TourDate + " | " + cont.ContentName;
            model.matchName += " | " + WeTourDll.instance.RenderRound(Convert.ToInt32(match.ROUND), Convert.ToInt32(cont.SignQty));
            if (match.ROUND == 0)
            {
                //小组赛，则增加小组组别
                model.matchName += " | 第" + match.etc1 + "组";
            }

            model.datetime = match.MATCHDATE;//比赛日期
            //获取比赛地点
            model.location = CourtDll.Get_Instance().GetCourtFullName(match.COURTID);

            model.datetime = "30";//比赛用时（计算第一分到最后一分的时间）

            #region 添加team
            //获取比赛双方的比分以及人员信息
            List<Dictionary<string, object>> teams = new List<Dictionary<string, object>>();
            //添加player1
            Dictionary<string, object> team1 = new Dictionary<string, object>();
            Dictionary<string, object> team2 = new Dictionary<string, object>();
            if (match.STATE == 2 && match.PLAYER1 == match.WINNER)
            {
                team1.Add("win", true);
                team2.Add("win", false);
            }
            else if (match.STATE == 2 && match.PLAYER2 == match.WINNER)
            {
                team1.Add("win", false);
                team2.Add("win", true);
            }
            else
            {
                team1.Add("win", false);
                team2.Add("win", false);
            }

            //比分
            if (match.SCORE.ToString().Length == 2)
            {
                team1.Add("score", Convert.ToInt32(match.SCORE.Substring(0, 1)));
                team2.Add("score", Convert.ToInt32(match.SCORE.Substring(1, 1)));
            }

            //成员
            team1.Add("users", getUserInfo(match.PLAYER1));
            team2.Add("users", getUserInfo(match.PLAYER2));

            teams.Add(team1);
            teams.Add(team2);
            #endregion
            model.teams = teams;

            //根据比赛获取比赛的盘
            List<SetModel> setmodellist = SetDll.Get_Instance().GetModelList(_sys);
            #region 添加盘比分
            List<object> set_list = new List<object>();
            foreach (SetModel set_model in setmodellist)
            {
                //获取当盘比分,并添加到数组
                set_list.Add(GetSetScore(_sys, set_model.SYS));
            }
            #endregion
            model.sets = set_list;

            

            #region 添加盘分详情 scoreDetails
            List<Dictionary<string, object>> sets = new List<Dictionary<string, object>>();

            //获得盘分
            #region  盘分            
            for ( int i=0;i<setmodellist.Count;i++)
            {
                SetModel set_model = setmodellist[i];
                Dictionary<string, object> set = new Dictionary<string, object>();
                //添加盘分详情
                set.Add("name","第"+(i+1)+"盘");
                set.Add("games", GetSetGames(_sys, set_model.SYS));

                sets.Add(set);
            }
            #endregion

            model.scoreDetails = sets;

            #endregion

            


            return model;
        }

        /// <summary>
        /// 根据盘主键和赛事主键，获取盘分情况
        /// </summary>
        /// <param name="matchsys"></param>
        /// <param name="_setSys"></param>
        /// <returns></returns>
        private List<Dictionary<string, object>> GetSetScore(string matchsys,string _setSys)
        {
            List<Dictionary<string, object>> setScores = new List<Dictionary<string, object>>();

            Dictionary<string, object> score1 = new Dictionary<string, object>();
            Dictionary<string, object> score2 = new Dictionary<string, object>();
            WeMatchModel match = WeMatchDll.instance.GetModel(matchsys);
            //获取player1的获胜局数
            int p1w = GameDll.Get_Instance().GetGameWonQty(_setSys, match.PLAYER1);
            //获取player2的获胜局数
            int p2w = GameDll.Get_Instance().GetGameWonQty(_setSys, match.PLAYER2);
            score1.Add("score", p1w);
            score2.Add("score", p2w);

            if (p1w > p2w)
            {
                score1.Add("win", true);
                score2.Add("win", false);
            }
            else if (p2w > p1w)
            {
                score1.Add("win", false);
                score2.Add("win", true);
            }
            else
            {
                score1.Add("win", false);
                score2.Add("win", false);
            }
            setScores.Add(score1);
            setScores.Add(score2);

            return setScores;
        }

        /// <summary>
        /// 获得一盘比赛的局详情
        /// </summary>
        /// <param name="_setSys"></param>
        /// <returns></returns>
        private List<object> GetSetGames(string matchsys, string _setSys)
        {
            List<object> game_list = new List<object>();
            WeMatchModel match = WeMatchDll.instance.GetModel(matchsys);

            //根据盘获取所有的局,并按照先后顺序排列
            List<GameModel> gameModelList = GameDll.Get_Instance().GetModellist(_setSys);
            //初始化双份局分
            int g1s = 0;
            int g2s = 0;
            foreach (GameModel game in gameModelList)
            {
                List<Dictionary<string, object>> gameDoub_list = new List<Dictionary<string, object>>();
                //局信息
                Dictionary<string, object> game1info = new Dictionary<string, object>();
                Dictionary<string, object> game2info = new Dictionary<string, object>();
                //发球
                if (game.MSERVER == match.PLAYER1)
                {
                    game1info.Add("first", true);
                    game2info.Add("first", false);
                }
                else if (game.MSERVER == match.PLAYER2)
                {
                    game1info.Add("first", false);
                    game2info.Add("first", true);
                }

                //得分
                if (game.WINNER == match.PLAYER1)
                {
                    g1s += 1;
                }
                else
                {
                    g2s += 1;
                }
                game1info.Add("score", g1s);
                game2info.Add("score", g2s);

                //添加得分详情
                List<Dictionary<string, object>> _p1sd = new List<Dictionary<string, object>>();
                List<Dictionary<string, object>> _p2sd = new List<Dictionary<string, object>>();
                GetscoreDetails(matchsys, game.SYS, out _p1sd, out _p2sd);
                game1info.Add("details", _p1sd);
                game2info.Add("details", _p2sd);

                gameDoub_list.Add(game1info);
                gameDoub_list.Add(game2info);

                game_list.Add(gameDoub_list);
            }

            return game_list;
        }

        /// <summary>
        /// 获取比分情况
        /// </summary>
        /// <param name="_matchsys"></param>
        /// <param name="_gamesys"></param>
        /// <param name="p1sd"></param>
        /// <param name="p2sd"></param>
        private void GetscoreDetails(string _matchsys, string _gamesys,out List<Dictionary<string,object>> p1sd,out List<Dictionary<string,object>> p2sd)
        {
            WeMatchModel match = WeMatchDll.instance.GetModel(_matchsys);
            List<Dictionary<string, object>> _p1sd = new List<Dictionary<string, object>>();
            List<Dictionary<string, object>> _p2sd = new List<Dictionary<string, object>>();

            List<PointsModel> point_list = PointsDll.Get_Instance().GetModellist(_gamesys);

            int p1s = 0;
            int p2s = 0;
            if (point_list.Count > 0)
            {
                //只显示除最后一分以外的分数，最后一分默认为本局获胜方
                for (int i = 0; i < point_list.Count - 1; i++)
                {
                    PointsModel point = point_list[i];

                    Dictionary<string, object> p1 = new Dictionary<string, object>();
                    Dictionary<string, object> p2 = new Dictionary<string, object>();
                    //添加这一分是否是破发点
                    bool IsBreak = false;
                    if (point.ISBREAKPOINT == 1)
                    {
                        IsBreak = false;
                    }

                    p1.Add("isBreakPoint",false);
                    p2.Add("isBreakPoint", false);

                    if (point.WINNER == match.PLAYER1)
                    {
                        p1s += 1;
                        p1.Add("win", true);
                        p2.Add("win", false);                        
                    }
                    else
                    {
                        p2s += 1;
                        p1.Add("win", false);
                        p2.Add("win", true);
                    }

                    //render score
                    string p1ss = "";
                    string p2ss = "";
                    bool isbreak=false;
                    GameModel game=GameDll.Get_Instance().GetModel(_gamesys);
                    if(game.ISTIEBREAK==1)
                    {
                        isbreak=true;
                    }
                    //渲染比分
                    PointsDll.Get_Instance().RenderPoint(p1s,p2s,isbreak,out p1ss,out p2ss);
                    p1.Add("score", p1ss);
                    p2.Add("score", p2ss);

                    _p1sd.Add(p1);
                    _p2sd.Add(p2);
                }
            }

            p1sd = _p1sd;
            p2sd = _p2sd;
        }

        /// <summary>
        /// 获取比赛详情的技术统计
        /// </summary>
        /// <param name="_matchId"></param>
        /// <returns></returns>
        public List<Dictionary<string, object>> GetMatchStatics(string _matchId)
        {
            List<Dictionary<string, object>> static_list = new List<Dictionary<string, object>>();
            //获取比赛技术统计
            List<TennisStats> modelist = StatsDll.Get_Instance().GetMatchStats(1, _matchId);
            foreach (TennisStats TeS in modelist)
            {
                Dictionary<string, object> stat = new Dictionary<string, object>();
                stat.Add("title",TeS.INDEX);
                stat.Add("team1", TeS.P1);
                stat.Add("team2", TeS.P2);

                static_list.Add(stat);
            }
            return static_list;
        }

        /// <summary>
        /// 获得用户信息
        /// </summary>
        /// <param name="_Usersys"></param>
        /// <returns></returns>
        public List<Fe_fetchMyMatch_signle_game_team_users> getUserInfo(string _Usersys)
        {
            List<Fe_fetchMyMatch_signle_game_team_users> user_list = new List<Fe_fetchMyMatch_signle_game_team_users>();
            if (!string.IsNullOrEmpty(_Usersys))
            {
                if (_Usersys.IndexOf(",") > 0)
                {
                    //双打
                    Fe_fetchMyMatch_signle_game_team_users user = new Fe_fetchMyMatch_signle_game_team_users();                    
                    WeMemberModel mem = WeMemberDll.instance.GetModel(_Usersys.Split(',')[0]);
                    user.username = mem.USERNAME;
                    user.userimage = mem.EXT1;
                    user_list.Add(user);

                    Fe_fetchMyMatch_signle_game_team_users user1 = new Fe_fetchMyMatch_signle_game_team_users();
                    WeMemberModel mem1 = WeMemberDll.instance.GetModel(_Usersys.Split(',')[1]);
                    user1.username = mem1.USERNAME;
                    user1.userimage = mem1.EXT1;
                    user_list.Add(user1);
                }
                else
                {
                    Fe_fetchMyMatch_signle_game_team_users user = new Fe_fetchMyMatch_signle_game_team_users();
                    //单打
                    WeMemberModel mem = WeMemberDll.instance.GetModel(_Usersys);
                    user.username = mem.USERNAME;
                    user.userimage = mem.EXT1;
                    user_list.Add(user);
                }
            }
            return user_list;
        }
        #endregion
    }
}
