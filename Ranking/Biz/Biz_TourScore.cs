using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using WeTour;

namespace Ranking
{
    public class Biz_TourScore
    {
        public static Biz_TourScore instance = new Biz_TourScore();

        /// <summary>
        /// 赛事完成后计算获得积分情况
        /// </summary>
        /// <param name="_Toursys"></param>
        public void UpdateScore(string _Toursys)
        {
            WeTourModel tmodel = WeTourDll.instance.GetModelbySys(_Toursys);

            List<WeTourContModel> list = WeTourContentDll.instance.GetContentlist(_Toursys);

            if (list.Count > 0)
            {
                foreach (WeTourContModel model in list)
                {

                    UpdateScorebyContentId(model.id);

                }
            }

            //更新排名
            if (tmodel.CITYTYPE == "Club")
            {
                RankDll.instance.UpdateClubRanking(tmodel.MGRSYS);
            }
            else
            {
                RankDll.instance.UpdateWetennisRank();
            }
        }

        /// <summary>
        /// 根据组别计算得分
        /// 按照微网球算法
        /// </summary>
        /// <param name="_Contentid"></param>
        public void UpdateScorebyContentId(string _Contentid)
        {
            //删除积分
            string sql = "delete rank_points where contentid='" + _Contentid + "'";
            DbHelperSQL.ExecuteSql(sql);

            //
            WeTourContModel tcModel = WeTourContentDll.instance.GetModelbyId(_Contentid);
            WeTourModel tmodel = WeTourDll.instance.GetModelbyId(tcModel.Toursys);
            //计算组别分数占别，根据级别,参赛人数
            if (string.IsNullOrEmpty(tcModel.ext4))
            {
                tcModel.ext4 = "1";
            }
            double _Ratio = Convert.ToDouble(tcModel.ext4);
           

            #region AddPoint
            try
            {
                List<MatchModel> list = MatchDll.Get_Instance().GetMatchListbyCont(_Contentid);
                if (list.Count > 0)
                {
                    foreach (MatchModel model in list)
                    {
                        //积分类型，根据比赛内容，判断是单打还是双打

                        //render Round
                        int _Round = Convert.ToInt32(model.ROUND);
                        int _Cap = Convert.ToInt32(tcModel.SignQty);
                        string _RoundName = WeTourDll.instance.RenderRound(_Round, _Cap);
                        try
                        {
                            int RankScore = 0;

                            if (model.ROUND == 0)
                            {
                                #region 小组赛积分
                                if (model.iswithdraw != "1")
                                {
                                    //是小组赛，给获胜方增加积分
                                    RankScore = Biz_TourContentScore.instance.GetContRoundScore(model.ContentID,model.ROUND.ToString());//获得获胜一场小组赛所获得的积分
                                  
                                    MemberModel mmodel = MemberDll.Get_Instance().GetModel(model.WINNER);
                                    RPointDll.instance.AddRankPoint(tmodel.CITYTYPE, tmodel.MGRSYS, RankScore.ToString(), mmodel.GENDER, model.WINNER, "获得一场小组赛/资格赛胜利", model.ContentID, model.TOURSYS);

                                }
                                else
                                {
                                    //退赛，维护
                                    if (model.SCORE != "00")
                                    {
                                        //一方退赛
                                        int RankScore1 = Biz_TourContentScore.instance.GetContRoundScore(model.ContentID, model.ROUND.ToString());//赢得一场小组赛的积分
                                       
                                        //2015-6-10更新积分算法
                                        MemberModel mmodel = MemberDll.Get_Instance().GetModel(model.WINNER);
                                        RPointDll.instance.AddRankPoint(tmodel.CITYTYPE, tmodel.MGRSYS, RankScore1.ToString(), mmodel.GENDER, model.WINNER, "获得一场小组赛/资格赛胜利", model.ContentID, model.TOURSYS);

                                        int WithDrawScore = (-1)*Biz_TourContentScore.instance.GetContRoundScore(model.ContentID, model.ROUND.ToString());//赢得一场小组赛的积分
                                       
                                        //2015-6-10更新积分算法
                                        MemberModel mmodel1 = MemberDll.Get_Instance().GetModel(model.LOSER);
                                        RPointDll.instance.AddRankPoint(tmodel.CITYTYPE, tmodel.MGRSYS, WithDrawScore.ToString(), mmodel1.GENDER, model.LOSER, "退赛，扣除惩罚积分", model.ContentID, model.TOURSYS);
                                    }
                                    else
                                    {
                                        //双方退赛
                                        int WithDrawScore = Biz_TourContentScore.instance.GetContRoundScore(model.ContentID, model.ROUND.ToString());//
                                       
                                        //2015-6-10更新积分算法
                                        MemberModel mmodel1 = MemberDll.Get_Instance().GetModel(model.PLAYER1);
                                        RPointDll.instance.AddRankPoint(tmodel.CITYTYPE, tmodel.MGRSYS, WithDrawScore.ToString(), mmodel1.GENDER, model.PLAYER1, "退赛，扣除惩罚积分", model.ContentID, model.TOURSYS);

                                        //2015-6-10更新积分算法
                                        MemberModel mmodel2 = MemberDll.Get_Instance().GetModel(model.PLAYER2);
                                        RPointDll.instance.AddRankPoint(tmodel.CITYTYPE, tmodel.MGRSYS, WithDrawScore.ToString(), mmodel2.GENDER, model.PLAYER2, "退赛，扣除惩罚积分", model.ContentID, model.TOURSYS);
                                    }
                                }
                                #endregion
                            }
                            else
                            {
                                #region 淘汰赛
                                //淘汰赛，给输家增加奖励积分
                                //是否是最后一轮     

                                string MaxCond = Math.Log(Convert.ToInt32(tcModel.SignQty), 2).ToString();////获得设计签数
                                if (MaxCond == model.ROUND.ToString())
                                {
                                    //决赛
                                    RankScore = Biz_TourContentScore.instance.GetContRoundScore(model.ContentID, model.ROUND.ToString());
                                    
                                    //2015-6-10更新积分算法
                                    MemberModel mmodel = MemberDll.Get_Instance().GetModel(model.LOSER);
                                    RPointDll.instance.AddRankPoint(tmodel.CITYTYPE, tmodel.MGRSYS, (RankScore*0.6).ToString(), mmodel.GENDER, model.LOSER, "亚军", model.ContentID, model.TOURSYS);

                                    RankScore = Biz_TourContentScore.instance.GetContRoundScore(model.ContentID, model.ROUND.ToString());//给冠军添加积分   
                                  
                                    //2015-6-10更新积分算法
                                    MemberModel mmodel2 = MemberDll.Get_Instance().GetModel(model.WINNER);
                                    RPointDll.instance.AddRankPoint(tmodel.CITYTYPE, tmodel.MGRSYS, RankScore.ToString(), mmodel2.GENDER, model.WINNER, "冠军", model.ContentID, model.TOURSYS);
                                }
                                else
                                {
                                    //非决赛
                                    RankScore = Biz_TourContentScore.instance.GetContRoundScore(model.ContentID, model.ROUND.ToString());
                                    MemberModel mmodel2 = MemberDll.Get_Instance().GetModel(model.LOSER);
                                    RPointDll.instance.AddRankPoint(tmodel.CITYTYPE, tmodel.MGRSYS, RankScore.ToString(), mmodel2.GENDER, model.LOSER, _RoundName, model.ContentID, model.TOURSYS);
                                }
                                #endregion
                            }
                        }
                        catch (Exception e)
                        {
                            //SettingDll.Get_Instance().InsertVisitLog("4001", "添加积分", e.ToString(), "", "");
                        }
                    }
                }
            }
            catch
            {

            }
            #endregion

        }
    }
}
