using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using Member;

namespace WeTour
{
    public class WeTour_MyMatchs
    {
        public static WeTour_MyMatchs instance = new WeTour_MyMatchs();

        /// <summary>
        /// 一年以内的胜负比
        /// 刘涛，2016-8-14
        /// </summary>
        /// <param name="_userId"></param>
        /// <param name="_Type"></param>
        /// <returns></returns>
        public string Get_YTDWL(string _userId,string _Type)
        {
            string winqty = TotalYTDMatchWLQty(_userId, _Type, "win");
            string loseqty = TotalYTDMatchWLQty(_userId, _Type, "lose");
            string ytdwl = winqty+"-"+loseqty;
            return ytdwl;
        }

        /// <summary>
        /// 获取总体winlose
        /// 刘涛，2016-8-14
        /// </summary>
        /// <param name="_userId"></param>
        /// <param name="_Type"></param>
        /// <returns></returns>
        public string Get_TotalWL(string _userId,string _Type)
        {
            string winqty=TotalMatchWLQty(_userId,_Type,"win");
            string loseqty=TotalMatchWLQty(_userId,_Type,"lose");
            string totalwl = winqty+"-"+loseqty;
            return totalwl;
        }

        #region 计算胜负场次
        /// <summary>
        /// 获得会员总胜场,单打，或双打
        /// 刘涛，2016-08-14
        /// </summary>
        /// <param name="_Memsys"></param>
        /// <param name="_Type">单打，或双打</param>
        /// <param name="_IsWin">胜利或失败</param>
        /// <returns></returns>
        public string TotalMatchWLQty(string _Memsys, string _Type, string _IsWin)
        {
            string sql = "select * from wtf_match where state=2 and Toursys is not null";

            if (_Type == "单打")
            {
                if (_IsWin == "win")
                {
                    //获胜场次
                    sql += " and winner='" + _Memsys + "'";
                }
                else
                {
                    //失败场次
                    sql += " and loser='" + _Memsys + "'";
                }
            }
            else
            {
                //双打比赛
                if (_IsWin == "win")
                {
                    //获胜场次
                    sql += " and (winner like '" + _Memsys + ",%' or winner like '%," + _Memsys + "')";
                }
                else
                {
                    //失败场次
                    sql += " and (loser like '" + _Memsys + ",%' or loser like '%," + _Memsys + "')";
                }
            }

            DataTable dt = DbHelperSQL.Query(sql).Tables[0];
            return dt.Rows.Count.ToString();
        }

        /// <summary>
        /// 获得会员总胜场,单打，或双打
        /// 刘涛，2016-08-14
        /// </summary>
        /// <param name="_Memsys"></param>
        /// <param name="_Type">单打，或双打</param>
        /// <param name="_IsWin">胜利或失败</param>
        /// <returns></returns>
        public string TotalYTDMatchWLQty(string _Memsys, string _Type, string _IsWin)
        {
            string onyear = DateTime.Now.AddYears(-1).ToString("yyyy-MM-dd");
            string sql = "select * from wtf_match where state=2 and Toursys is not null  and matchdate>'" + onyear + "'";

            if (_Type == "单打")
            {
                if (_IsWin == "win")
                {
                    //获胜场次
                    sql += " and winner='" + _Memsys + "'";
                }
                else
                {
                    //失败场次
                    sql += " and loser='" + _Memsys + "'";
                }
            }
            else
            {
                //双打比赛
                if (_IsWin == "win")
                {
                    //获胜场次
                    sql += " and (winner like '" + _Memsys + ",%' or winner like '%," + _Memsys + "')";
                }
                else
                {
                    //失败场次
                    sql += " and (loser like '" + _Memsys + ",%' or loser like '%," + _Memsys + "')";
                }
            }

            DataTable dt = DbHelperSQL.Query(sql).Tables[0];
            return dt.Rows.Count.ToString();
        }
        #endregion


        #region 拉取我的比赛
        /// <summary>
        /// 根据用户sysno获取参加的赛事及比赛信息
        /// 刘涛，2016-08-22
        /// </summary>
        /// <param name="_userId"></param>
        /// <returns></returns>
        public void  GetMyMatches(string _userId, out List<Fe_fetchMyMatch_singleMatch> singleMatch,out List<Fe_fetchMyMatch_singleMatch> coupleMatch)
        {
            List<Fe_fetchMyMatch_singleMatch> MyMatch_sigle_list = new List<Fe_fetchMyMatch_singleMatch>();
            List<Fe_fetchMyMatch_singleMatch> MyMatch_couple_list = new List<Fe_fetchMyMatch_singleMatch>();
            string sql="select DISTINCT(a.contentid) from wtf_TourApply a  left join wtf_CityTour b on a.toursys=b.sysno where (a.memberid='"+_userId+"' or a.paterner='"+_userId+"') and a.status<>'99' and b.status in ('4','5')";
            DataTable dt = DbHelperSQL.Query(sql).Tables[0];
            for (int i = 0; i < dt.Rows.Count; i++)
            { 
                //逐个项目添加
                string _contId = dt.Rows[i][0].ToString();
                //判断状态是否满足
                WeTourContModel cont = WeTourContentDll.instance.GetModelbyId(_contId);
                WeTourModel tour = WeTourDll.instance.GetModelbySys(cont.Toursys);
                int tourStatus=Convert.ToInt32(tour.STATUS);
                if (tourStatus > 3)
                {
                    //只读取状态为4的赛事
                    Fe_fetchMyMatch_singleMatch myMatch = new Fe_fetchMyMatch_singleMatch();
                    myMatch.title = tour.NAME;//赛事名称

                    //添加起始日期，结束日期
                    string subt_date = "";
                    if (tour.STARTDATE == tour.ENDDATE)
                    {
                        subt_date = tour.STARTDATE;
                    }
                    else
                    {
                        subt_date = tour.STARTDATE + "-" + tour.ENDDATE;
                    }

                    myMatch.subtitle = subt_date+"|"+cont.TourDate+"|"+cont.ContentName;

                                       
                    if(cont.ContentType.IndexOf("双")<0)
                    {
                        //获取赛事积分
                        myMatch.pts = GetSocre_byTour(_userId,"单打",tour.SYSNO); 
                        //添加单打比赛                        
                        myMatch.games = GetMatches(_contId, "单打", _userId);
                        MyMatch_sigle_list.Add(myMatch);
                    }
                    else
                    {                       
                        //获取赛事积分
                        myMatch.pts = GetSocre_byTour(_userId, "双打", tour.SYSNO);
                        //添加双打比赛
                        myMatch.games = GetMatches(_contId, "双打", _userId);
                        MyMatch_couple_list.Add(myMatch);
                    }
                }                
            }
            //赋值
            singleMatch = MyMatch_sigle_list;
            coupleMatch = MyMatch_couple_list;
        }

        /// <summary>
        /// 获取项目比赛
        /// </summary>
        /// <param name="_contentId"></param>
        /// <returns></returns>
        private List<Fe_fetchMyMatch_single_game> GetMatches(string _contentId,string _type,string _userId)
        {
            List<Fe_fetchMyMatch_single_game> Match_list = new List<Fe_fetchMyMatch_single_game>();
            string sql = "";
            if (_type == "单打")
            {
                //单打比赛
                sql = "select * from wtf_match where contentid='" + _contentId + "' and (player1='"+_userId+"' or player2='"+_userId+"')"; 
            }
            else
            { 
                //双打比赛
                sql = "select * from wtf_match where contentid='" + _contentId + "' and (player1 like '%," + _userId + "' or player1 like '" + _userId + ",%' or player2 like '%," + _userId + "' or player2 like '"+_userId+",%')";
            }

            DataTable dt = DbHelperSQL.Query(sql).Tables[0];
            for (int i = 0; i < dt.Rows.Count; i++)
            {
                Fe_fetchMyMatch_single_game game = new Fe_fetchMyMatch_single_game();
                game.gameTime = dt.Rows[i]["ActualTime"].ToString();//比赛时间
                //获取轮次
                string roundstr = dt.Rows[i]["Round"].ToString();
                if (roundstr == "")
                {
                    roundstr = "0";
                }

                int round = Convert.ToInt32(roundstr);
                WeTourContModel cont = WeTourContentDll.instance.GetModelbyId(_contentId);
                string capstr = cont.SignQty;
                if (capstr == "")
                {
                    capstr = "0";
                }

                int cap = Convert.ToInt32(capstr);
                game.matches = WeTourDll.instance.RenderRound(round, cap);

                //添加比赛双方信息

                List<Fe_fetchMyMatch_single_game_team> sintems = new List<Fe_fetchMyMatch_single_game_team>();
                #region 添加单打比赛
                //添加player1
                Fe_fetchMyMatch_single_game_team team = new Fe_fetchMyMatch_single_game_team();
                //比赛结果
                if (dt.Rows[i]["player1"].ToString() == dt.Rows[i]["winner"].ToString())
                {
                    team.win = true;
                }
                else
                {
                    team.win = false;
                }

                //分数
                string _score = dt.Rows[i]["score"].ToString();
                if (_score != "")
                {
                    string a = _score.Substring(0, 1);
                    team.score = Convert.ToInt32(a);
                }
                else
                {
                    team.score = 0;
                }

                List<Fe_fetchMyMatch_signle_game_team_users> user_list = new List<Fe_fetchMyMatch_signle_game_team_users>();
                if (_type == "单打")
                {
                    //单打比赛                    
                    //用户
                    Fe_fetchMyMatch_signle_game_team_users _user = new Fe_fetchMyMatch_signle_game_team_users();
                    WeMemberModel mem = WeMemberDll.instance.GetModel(dt.Rows[i]["player1"].ToString());
                    _user.username = mem.NAME;
                    _user.userimage = mem.EXT1;
                    
                    user_list.Add(_user);                 
                }
                else
                {
                    //双打比赛
                    Fe_fetchMyMatch_signle_game_team_users _user = new Fe_fetchMyMatch_signle_game_team_users();
                    WeMemberModel mem = WeMemberDll.instance.GetModel(dt.Rows[i]["player1"].ToString().Split(',')[0]);
                    _user.username = mem.NAME;
                    _user.userimage = mem.EXT1;
                    user_list.Add(_user);

                    Fe_fetchMyMatch_signle_game_team_users _user1r = new Fe_fetchMyMatch_signle_game_team_users();
                    WeMemberModel mem1r = WeMemberDll.instance.GetModel(dt.Rows[i]["player1"].ToString().Split(',')[1]);
                    _user1r.username = mem1r.NAME;
                    _user1r.userimage = mem1r.EXT1;
                    user_list.Add(_user1r);
                }
                team.users = user_list;  
                sintems.Add(team);
               
                //添加player2
                Fe_fetchMyMatch_single_game_team team2 = new Fe_fetchMyMatch_single_game_team();
                //比赛结果
                if (dt.Rows[i]["player2"].ToString() == dt.Rows[i]["winner"].ToString())
                {
                    team2.win = true;
                }
                else
                {
                    team2.win = false;
                }

                //分数
                if (_score != "")
                {
                    string a = _score.Substring(1, 1);
                    team2.score = Convert.ToInt32(a);
                }
                else
                {
                    team2.score = 0;
                }

                List<Fe_fetchMyMatch_signle_game_team_users> user2_list = new List<Fe_fetchMyMatch_signle_game_team_users>();
                if (_type == "单打")
                {
                    //用户
                    Fe_fetchMyMatch_signle_game_team_users _user2 = new Fe_fetchMyMatch_signle_game_team_users();
                    WeMemberModel mem2 = WeMemberDll.instance.GetModel(dt.Rows[i]["player2"].ToString());
                    _user2.username = mem2.NAME;
                    _user2.userimage = mem2.EXT1;
                    user2_list.Add(_user2);                   
                }
                else
                {
                    //双打比赛
                    Fe_fetchMyMatch_signle_game_team_users _user = new Fe_fetchMyMatch_signle_game_team_users();
                    WeMemberModel mem = WeMemberDll.instance.GetModel(dt.Rows[i]["player2"].ToString().Split(',')[0]);
                    _user.username = mem.NAME;
                    _user.userimage = mem.EXT1;
                    user2_list.Add(_user);

                    Fe_fetchMyMatch_signle_game_team_users _user1r = new Fe_fetchMyMatch_signle_game_team_users();
                    WeMemberModel mem1r = WeMemberDll.instance.GetModel(dt.Rows[i]["player2"].ToString().Split(',')[1]);
                    _user1r.username = mem1r.NAME;
                    _user1r.userimage = mem1r.EXT1;
                    user2_list.Add(_user1r);
                }

                team2.users = user2_list;
                sintems.Add(team2);

                #endregion
                game.teams = sintems;


                Match_list.Add(game);
            }


            return Match_list;
        }

        /// <summary>
        /// 获取一个赛事获取的积分
        /// </summary>
        /// <param name="_Memsys"></param>
        /// <param name="_IsSingle"></param>
        /// <param name="_TourSys"></param>
        /// <returns></returns>
        public int GetSocre_byTour(string _Memsys, string _IsSingle, string _TourSys)
        {
            string _Point = "0";
            int P = 0;
            string sql = "select sum(points) as points from rank_points where  pointType='' and memsys='" + _Memsys + "' and IsSingle='" + _IsSingle + "' and TourSys='" + _TourSys + "'";
            DataTable dt = DbHelperSQL.Query(sql).Tables[0];
            _Point = dt.Rows[0][0].ToString();
            try
            {
                P = Convert.ToInt32(_Point);
            }
            catch
            {
                P = 0;
            }
            return P;
        }
        #endregion
    }
}
