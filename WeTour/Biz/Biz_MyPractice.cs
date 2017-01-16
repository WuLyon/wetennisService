using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using Member;

namespace WeTour
{
    public class Biz_MyPractice
    {
        public static Biz_MyPractice instance = new Biz_MyPractice();

        #region 对外方法
        /// <summary>
        /// 获取YTD训练数统计
        /// </summary>
        /// <param name="_userId"></param>
        /// <returns></returns>
        public string Get_YTD_Practice(string _userId)
        {
            string _YTD = "";
            //计算获胜
            int win = 0;
            win = YTD_PracticeWin(_userId, "单打", "win");
            win += YTD_PracticeWin(_userId, "双打", "win");
            //计算失败
            int lose = 0;
            lose = YTD_PracticeWin(_userId, "单打", "lose");
            lose += YTD_PracticeWin(_userId, "双打", "lose");
            _YTD = win + "-" + lose;
            return _YTD;
        }

        /// <summary>
        /// 获取总训练数统计
        /// </summary>
        /// <param name="_userId"></param>
        /// <returns></returns>
        public string Get_Total_Practice(string _userId)
        {
            string _Total = "";
            //计算获胜
            int win = 0;
            win = Total_PracticeWin(_userId, "单打", "win");
            win += Total_PracticeWin(_userId, "双打", "win");
            //计算失败
            int lose = 0;
            lose = Total_PracticeWin(_userId, "单打", "lose");
            lose += Total_PracticeWin(_userId, "双打", "lose");

            _Total = win + "-" + lose;
            return _Total;
        }

        /// <summary>
        /// 获取单打练习赛
        /// </summary>
        /// <param name="_userId"></param>
        /// <returns></returns>
        public List<Fe_fetchMyMatch_single_game> Get_Single_Practices(string _userId)
        {
            List<Fe_fetchMyMatch_single_game> game_list = new List<Fe_fetchMyMatch_single_game>();
            string sql = string.Format("select * from wtf_match where toursys is null and state=2 and (player1='{0}' or player2='{0}')",_userId);
            DataTable dt = DbHelperSQL.Query(sql).Tables[0];
            for (int i = 0; i < dt.Rows.Count; i++)
            {
                Fe_fetchMyMatch_single_game game = new Fe_fetchMyMatch_single_game();
                game.gameTime = dt.Rows[i]["matchdate"].ToString();
                game.matches = "";
                                
                #region 添加队员
                List<Fe_fetchMyMatch_single_game_team> game_teams = new List<Fe_fetchMyMatch_single_game_team>();
                #region 添加player1
                Fe_fetchMyMatch_single_game_team team1 = new Fe_fetchMyMatch_single_game_team();
                if (dt.Rows[i]["player1"].ToString() == dt.Rows[i]["winner"].ToString())
                {
                    team1.win = true;
                }
                else
                {
                    team1.win = false;
                }
                //分数
                team1.score = Convert.ToInt32(dt.Rows[i]["score"].ToString().Substring(0, 1));

                //添加队员
                List<Fe_fetchMyMatch_signle_game_team_users> team1_user_list = new List<Fe_fetchMyMatch_signle_game_team_users>();
                Fe_fetchMyMatch_signle_game_team_users team1_user = new Fe_fetchMyMatch_signle_game_team_users();
                WeMemberModel mem = WeMemberDll.instance.GetModel(dt.Rows[i]["player1"].ToString());
                team1_user.username = mem.USERNAME;
                team1_user.userimage = mem.EXT1;
                team1_user_list.Add(team1_user);
                team1.users = team1_user_list;

                game_teams.Add(team1);
                #endregion

                #region 添加player2
                Fe_fetchMyMatch_single_game_team team2 = new Fe_fetchMyMatch_single_game_team();
                if (dt.Rows[i]["player2"].ToString() == dt.Rows[i]["winner"].ToString())
                {
                    team2.win = true;
                }
                else
                {
                    team2.win = false;
                }
                //分数
                team2.score = Convert.ToInt32(dt.Rows[i]["score"].ToString().Substring(1, 1));

                //添加队员
                List<Fe_fetchMyMatch_signle_game_team_users> team2_user_list = new List<Fe_fetchMyMatch_signle_game_team_users>();
                Fe_fetchMyMatch_signle_game_team_users team2_user = new Fe_fetchMyMatch_signle_game_team_users();
                WeMemberModel mem2 = WeMemberDll.instance.GetModel(dt.Rows[i]["player2"].ToString());
                team2_user.username = mem2.USERNAME;
                team2_user.userimage = mem2.EXT1;
                team2_user_list.Add(team2_user);
                team2.users = team2_user_list;

                game_teams.Add(team2);
                #endregion
                game.teams = game_teams;
                #endregion 

                game_list.Add(game);
            }
            return game_list;
        }

        public List<Fe_fetchMyMatch_single_game> Get_Couple_Practices(string _userId)
        {
            List<Fe_fetchMyMatch_single_game> game_list = new List<Fe_fetchMyMatch_single_game>();

            return game_list;
        }
        #endregion
              
        #region 计算胜负比
        /// <summary>
        /// 52周内
        /// </summary>
        /// <param name="_Memsys"></param>
        /// <param name="_Type"></param>
        /// <param name="_IsWin"></param>
        /// <returns></returns>
        private int YTD_PracticeWin(string _Memsys,string _Type,string _IsWin)
        {
            string onyear = DateTime.Now.AddYears(-1).ToString("yyyy-MM-dd");
            string sql = "select * from wtf_match where state=2 and Toursys is null  and matchdate>'" + onyear + "'";

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
            return dt.Rows.Count;
        }

        /// <summary>
        /// 总计比赛数量
        /// </summary>
        /// <param name="_Memsys"></param>
        /// <param name="_Type"></param>
        /// <param name="_IsWin"></param>
        /// <returns></returns>
        private int Total_PracticeWin(string _Memsys, string _Type, string _IsWin)
        {
            string sql = "select * from wtf_match where state=2 and Toursys is null";

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
            return dt.Rows.Count;
        }
        #endregion

        #region 获取比赛信息
        
        #endregion
    }
}
