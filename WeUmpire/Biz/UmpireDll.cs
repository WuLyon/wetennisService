using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using WeTour;
using SMS;

namespace WeUmpire
{
    /// <summary>
    /// 计分板的定制功能
    /// </summary>
    public class UmpireDll
    {
        public static UmpireDll instance = new UmpireDll();
        /// <summary>
        /// 根据比赛主键获得比赛的比分显示情况
        /// </summary>
        /// <param name="_Sys"></param>
        /// <returns></returns>
        public UmpScoreModel GetMatchScores(string _Sys)
        {
            UmpScoreModel model = new UmpScoreModel();
            try
            {
                WeMatchModel mmodel = WeMatchDll.instance.GetModel(_Sys);

                //获得盘分，支持多盘，数组表示
                model.SetScore1 = SetDll.Get_Instance().GetSetScore(_Sys, mmodel.PLAYER1);//获得p1盘比分
                model.SetScore2 = SetDll.Get_Instance().GetSetScore(_Sys, mmodel.PLAYER2);//获得p2盘比分

                //获得当前盘的局数
                int setOrder=model.SetScore1.Count;
                model.GameWon1 = model.SetScore1[setOrder-1].WinGames;
                model.GameWon2 = model.SetScore2[setOrder - 1].WinGames;

                //获得局分
                string gameScore = GameDll.Get_Instance().GetGameScore(_Sys); //获取局比分
                model.GameScore1 = gameScore.Split(',')[0];
                model.GameScore2 = gameScore.Split(',')[1];

                //获得上一分的情况
                //string Comments=TrendDll.Get_Instance().getLatestTrendCom(_Sys).COMMENTS;
                //if (string.IsNullOrEmpty(Comments))
                //{
                //    Comments = "比赛开始，提交比分过程中可能会由于网速问题，系统反应较慢，请耐心等待";
                //}
                //model.LastPtInfo = Comments; 
            }
            catch
            { 
            }
            return model;
        }

        /// <summary>
        /// 根据比赛主键，获得当前局的发球人员和左手方球员，2016-1-5,liutao
        /// </summary>
        /// <param name="_Sys"></param>
        /// <returns></returns>
        public GameServerLeftModel UmpGameServerLeft(string _Sys)
        {
            GameServerLeftModel model= GameDll.Get_Instance().GetGameServer(_Sys);
            model.MatchInfo = WeMatchDll.instance.RenderMatch(_Sys);
            return model;
        }

        /// <summary>
        /// 批量提交赛事结果
        /// </summary>
        /// <param name="list"></param>
        public string SubmitMatchResults(List<UmpMatchResult> list)
        {
            string Result = "";
            if (list.Count > 0)
            {
                foreach (UmpMatchResult model in list)
                {
                    WeMatchModel mmodel = WeMatchDll.instance.GetModel(model.MatchSys);
                    int MatchSets = 0;
                    switch (mmodel.MATCHTYPE)
                    { 
                        case 0:
                            MatchSets = 1;
                            break;
                        case 1:
                            MatchSets = 3;
                            break;
                        case 2:
                            MatchSets = 5;
                            break;
                    }

                    int set1 = 0;
                    int set2 = 0;
                    string scorestr = "";
                    for (int i = 0; i < MatchSets; i++)
                    {
                        int p11 = Convert.ToInt32(model.SetScore[i].P1SetS);
                        int p12 = Convert.ToInt32(model.SetScore[i].P2SetS);
                        if (p11 > p12)
                        {
                            set1 += 1;
                        }
                        else
                        {
                            set2 += 1;
                        }
                        scorestr += p11.ToString() + p12.ToString() + " ";
                    }
                    if (set1 > set2)
                    {
                        mmodel.WINNER = mmodel.PLAYER1;
                        mmodel.LOSER = mmodel.PLAYER2;
                    }
                    else
                    {
                        mmodel.WINNER = mmodel.PLAYER2;
                        mmodel.LOSER = mmodel.PLAYER1;
                    }
                    mmodel.SCORE = scorestr;

                    //更新比分
                    WeMatchDll.instance.UpdateMatchResult(mmodel.SYS, mmodel.WINNER, mmodel.loseto, mmodel.SCORE);
                }
            }
            else
            {
                Result = "无计划";
            }
            return Result;
        }
    }
}
