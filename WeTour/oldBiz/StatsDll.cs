using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;

namespace WeTour
{
    public class StatsDll
    {
        private StatsDll() { }
        private static StatsDll _Instance;
        public static StatsDll Get_Instance()
        {
            if (_Instance == null)
            {
                _Instance = new StatsDll();
            }
            return _Instance;
        }

        /// <summary>
        /// 根据条件查看比赛或某一盘的技术统计
        /// </summary>
        /// <param name="_Type"></param>
        /// <param name="_Sys"></param>
        /// <returns></returns>
        public List<TennisStats> GetMatchStats(int _Type, string _Sys)
        {
            List<TennisStats> modellist = new List<TennisStats>();
            string _SetSys = "";

            if (_Type == 1 || _Type == 0)
            {
                //比赛数据
                modellist = GetStats(1, _Sys);
            }
            else
            {
                //某一盘的数据
                _SetSys = SetDll.Get_Instance().GetSetSysByOrder(_Sys, _Type - 1);
                modellist = GetStats(2, _SetSys);
            }

            return modellist;
        }

        /// <summary>
        /// 根据比赛主键或者盘主键来获取数据
        /// </summary>
        /// <param name="_Type"></param>1表示比赛数据，sys是比赛的主键，2表示盘数据，sys是盘的主键
        /// <param name="_Sys"></param>
        /// <returns></returns>
        public List<TennisStats> GetStats(int _Type, string _Sys)
        {
            List<TennisStats> modellist = new List<TennisStats>();

            if (_Type == 1)//统计整场比赛的
            {
                try
                {
                    //总得分率
                    modellist.Add(GetPointsWonbyMatch(_Sys));

                    //发球总得分率
                    modellist.Add(GetServeWonbyMatch(_Sys));

                    //接球总得分率
                    modellist.Add(GetReturnWonbyMatch(_Sys));



                    //添加ACE数
                    modellist.Add(GetAces(_Sys));

                    //添加双误数
                    modellist.Add(GetDoubleFaultsByMatch(_Sys));

                    //添加winner
                    modellist.Add(GetWinner(_Sys));

                    //添加非受迫性失误
                    modellist.Add(GetUF(_Sys));

                    //添加一发成功率
                    modellist.Add(GetFstServebyMatch(_Sys));

                    //添加一发得分率
                    modellist.Add(GetFstServeWonbyMatch(_Sys));

                    //添加二发得分率
                    modellist.Add(GetSndServeWonbyMatch(_Sys));

                    //添加破发点保发成功率
                    modellist.Add(GetBreakPointSavedWonbyMatch(_Sys));

                    //添加发球局数量
                    modellist.Add(GetServeGameByMatch(_Sys));

                    //添加一发回球得分率
                    modellist.Add(GetFstReturnWonbyMatch(_Sys));

                    //添加二发回球得分率
                    modellist.Add(GetSndReturnWonbyMatch(_Sys));

                    //破发成功率
                    modellist.Add(GetBreakPointWonbyMatch(_Sys));

                }
                catch
                {
                }

            }
            else //统计某一盘的比赛
            {
                //添加ACE数
                modellist.Add(GetAcesbySet(_Sys));

                //添加双误数
                modellist.Add(GetDoubleFaultsBySet(_Sys));

                //添加winner
                modellist.Add(GetWinnerbySet(_Sys));

                //添加非受迫性失误
                modellist.Add(GetUFbySet(_Sys));

                //添加一发成功率
                modellist.Add(GetFstServeBySet(_Sys));

                //添加一发得分率
                modellist.Add(GetFstServeWonBySet(_Sys));

                //添加二发得分率
                modellist.Add(GetSndServeWonBySet(_Sys));

                //添加破发点保发成功率
                modellist.Add(GetBreakPointSavedBySet(_Sys));

                //添加发球局数量
                modellist.Add(GetServeGameBySet(_Sys));

                //添加一发回球得分率
                modellist.Add(GetFstReturnWonBySet(_Sys));

                //添加二发回球得分率
                modellist.Add(GetSndReturnWonBySet(_Sys));

                //破发成功率
                modellist.Add(GetBreakPointWonBySet(_Sys));

                //发球总得分率
                modellist.Add(GetServeWonBySet(_Sys));

                //接球总得分率
                modellist.Add(GetReturnWonBySet(_Sys));

                //总得分率
                modellist.Add(GetPointsWonBySet(_Sys));
            }
            return modellist;
        }


        //------------------------统计ACE球数---------------------

        /// <summary>
        /// 获取整场比赛的winner
        /// </summary>
        /// <param name="_Type"></param>
        /// <param name="_Sys"></param>
        /// <returns></returns>
        private TennisStats GetAces(string _Matchsys)
        {
            int P1 = 0;
            int P2 = 0;
            TennisStats model = new TennisStats();
            model.INDEX = "ACES";

            MatchModel mmodel = MatchDll.Get_Instance().GetModel(_Matchsys);
            List<SetModel> SetList = SetDll.Get_Instance().GetModelList(_Matchsys);
            foreach (SetModel smodel in SetList)
            {
                List<GameModel> GameList = GameDll.Get_Instance().GetModellist(smodel.SYS);
                foreach (GameModel gmodel in GameList)
                {
                    if (gmodel.ISTIEBREAK == 0)
                    {
                        if (gmodel.MSERVER == mmodel.PLAYER1)
                        {
                            //统计P1的ACE球
                            P1 += GetGameAces(gmodel.SYS);
                        }
                        else
                        {
                            //统计P2的ACE球
                            P2 += GetGameAces(gmodel.SYS);
                        }
                    }
                    else
                    {
                        #region 统计抢七
                        List<PointsModel> listp = PointsDll.Get_Instance().GetModellist(gmodel.SYS);
                        if (listp.Count > 0)
                        {
                            foreach (PointsModel pmodel in listp)
                            {
                                if (pmodel.WINTYPE == 0)
                                {
                                    //是ACE球
                                    if (pmodel.GORDER == 1)
                                    {
                                        if (gmodel.MSERVER == mmodel.PLAYER1)
                                        {
                                            P1 += 1;
                                        }
                                        else
                                        {
                                            P2 += 1;
                                        }
                                    }
                                    else
                                    {
                                        //not the first point
                                        int x = 0;
                                        if (pmodel.GORDER % 2 == 0)
                                        {
                                            x = pmodel.GORDER / 2;
                                        }
                                        else
                                        {
                                            x = (pmodel.GORDER - 1) / 2;
                                        }

                                        if (x % 2 == 0)
                                        {
                                            //发球方是本局标记的发球方
                                            if (gmodel.MSERVER == mmodel.PLAYER1)
                                            {
                                                P1 += 1;
                                            }
                                            else
                                            {
                                                P2 += 1;
                                            }
                                        }
                                        else
                                        {
                                            //发球方不是本局标记的发球方
                                            if (gmodel.MSERVER == mmodel.PLAYER1)
                                            {
                                                P2 += 1;
                                            }
                                            else
                                            {
                                                P1 += 1;
                                            }
                                        }
                                    }
                                }
                            }
                        }
                        #endregion

                    }
                }
            }

            model.P1 = P1.ToString();
            model.P2 = P2.ToString();

            return model;
        }

        /// <summary>
        /// 获取一盘的ACE球
        /// </summary>
        /// <param name="_Setsys"></param>
        /// <returns></returns>
        private TennisStats GetAcesbySet(string _Setsys)
        {
            int P1 = 0;
            int P2 = 0;
            TennisStats model = new TennisStats();
            model.INDEX = "ACES";

            SetModel smodel = SetDll.Get_Instance().GetModel(_Setsys);
            MatchModel mmodel = MatchDll.Get_Instance().GetModel(smodel.MATCHSYS);
            List<GameModel> GameList = GameDll.Get_Instance().GetModellist(smodel.SYS);
            foreach (GameModel gmodel in GameList)
            {
                if (gmodel.MSERVER == mmodel.PLAYER1)
                {
                    //统计P1的ACE球
                    P1 += GetGameAces(gmodel.SYS);
                }
                else
                {
                    //统计P2的ACE球
                    P2 += GetGameAces(gmodel.SYS);
                }
            }

            model.P1 = P1.ToString();
            model.P2 = P2.ToString();

            return model;
        }

        /// <summary>
        /// 获取当局ACE球数量
        /// </summary>
        /// <param name="_Gamesys"></param>
        /// <param name="_Winner"></param>
        /// <returns></returns>
        private int GetGameAces(string _Gamesys)
        {
            DataTable dt = PointsDac.SelectList(string.Format(" and gamesys='{0}' and wintype='0'", _Gamesys));
            return dt.Rows.Count;
        }

        //------------------------统计WINNER球数---------------------

        /// <summary>
        /// 获取整场比赛的winner
        /// </summary>
        /// <param name="_Type"></param>
        /// <param name="_Sys"></param>
        /// <returns></returns>
        private TennisStats GetWinner(string _Matchsys)
        {
            int P1 = 0;
            int P2 = 0;
            TennisStats model = new TennisStats();
            model.INDEX = "制胜分";

            MatchModel mmodel = MatchDll.Get_Instance().GetModel(_Matchsys);
            List<SetModel> SetList = SetDll.Get_Instance().GetModelList(_Matchsys);
            foreach (SetModel smodel in SetList)
            {
                List<GameModel> GameList = GameDll.Get_Instance().GetModellist(smodel.SYS);
                foreach (GameModel gmodel in GameList)
                {
                    if (gmodel.MSERVER == mmodel.PLAYER1)
                    {
                        //统计P1的ACE球
                        P1 += GetGameWinner(gmodel.SYS);
                    }
                    else
                    {
                        //统计P2的ACE球
                        P2 += GetGameWinner(gmodel.SYS);
                    }
                }
            }

            model.P1 = P1.ToString();
            model.P2 = P2.ToString();

            return model;
        }

        /// <summary>
        /// 获取一盘的制胜分
        /// </summary>
        /// <param name="_Setsys"></param>
        /// <returns></returns>
        private TennisStats GetWinnerbySet(string _Setsys)
        {
            int P1 = 0;
            int P2 = 0;
            TennisStats model = new TennisStats();
            model.INDEX = "制胜分";

            SetModel smodel = SetDll.Get_Instance().GetModel(_Setsys);
            MatchModel mmodel = MatchDll.Get_Instance().GetModel(smodel.MATCHSYS);
            List<GameModel> GameList = GameDll.Get_Instance().GetModellist(smodel.SYS);
            foreach (GameModel gmodel in GameList)
            {
                if (gmodel.MSERVER == mmodel.PLAYER1)
                {
                    //统计P1的ACE球
                    P1 += GetGameWinner(gmodel.SYS);
                }
                else
                {
                    //统计P2的制胜分
                    P2 += GetGameWinner(gmodel.SYS);
                }
            }

            model.P1 = P1.ToString();
            model.P2 = P2.ToString();

            return model;
        }

        /// <summary>
        /// 获取当局制胜分数量
        /// </summary>
        /// <param name="_Gamesys"></param>
        /// <param name="_Winner"></param>
        /// <returns></returns>
        private int GetGameWinner(string _Gamesys)
        {
            DataTable dt = PointsDac.SelectList(string.Format(" and gamesys='{0}' and wintype='1'", _Gamesys));
            return dt.Rows.Count;
        }

        //------------------------统计非受迫失误---------------------

        /// <summary>
        /// 获取整场比赛的winner
        /// </summary>
        /// <param name="_Type"></param>
        /// <param name="_Sys"></param>
        /// <returns></returns>
        private TennisStats GetUF(string _Matchsys)
        {
            int P1 = 0;
            int P2 = 0;
            TennisStats model = new TennisStats();
            model.INDEX = "非受迫失误";

            MatchModel mmodel = MatchDll.Get_Instance().GetModel(_Matchsys);
            List<SetModel> SetList = SetDll.Get_Instance().GetModelList(_Matchsys);
            foreach (SetModel smodel in SetList)
            {
                List<GameModel> GameList = GameDll.Get_Instance().GetModellist(smodel.SYS);
                foreach (GameModel gmodel in GameList)
                {
                    //统计P2的非受迫失误
                    P2 += GetGameUF(gmodel.SYS, mmodel.PLAYER1);
                    //统计P1的非受迫失误
                    P1 += GetGameUF(gmodel.SYS, mmodel.PLAYER2);
                }
            }

            model.P1 = P1.ToString();
            model.P2 = P2.ToString();

            return model;
        }

        /// <summary>
        /// 获取一盘的非受迫失误数
        /// </summary>
        /// <param name="_Setsys"></param>
        /// <returns></returns>
        private TennisStats GetUFbySet(string _Setsys)
        {
            int P1 = 0;
            int P2 = 0;
            TennisStats model = new TennisStats();
            model.INDEX = "非受迫失误";

            SetModel smodel = SetDll.Get_Instance().GetModel(_Setsys);
            MatchModel mmodel = MatchDll.Get_Instance().GetModel(smodel.MATCHSYS);
            List<GameModel> GameList = GameDll.Get_Instance().GetModellist(smodel.SYS);
            foreach (GameModel gmodel in GameList)
            {
                //统计P2的非受迫失误
                P2 += GetGameUF(gmodel.SYS, mmodel.PLAYER1);
                //统计P1的非受迫失误
                P1 += GetGameUF(gmodel.SYS, mmodel.PLAYER2);
            }

            model.P1 = P1.ToString();
            model.P2 = P2.ToString();

            return model;
        }

        /// <summary>
        /// 获取当局非受迫性失误
        /// </summary>
        /// <param name="_Gamesys"></param>
        /// <param name="_Winner"></param>
        /// <returns></returns>
        private int GetGameUF(string _Gamesys, string _Winner)
        {
            DataTable dt = PointsDac.SelectList(string.Format(" and gamesys='{0}' and wintype='2' and winner='{1}'", _Gamesys, _Winner));
            return dt.Rows.Count;
        }

        //------------------统计双误数--------------------------//

        /// <summary>
        /// 获取当局的双误数量
        /// </summary>
        /// <param name="_Gamesys"></param>
        /// <returns></returns>
        private int GetGameDoubleFault(string _Gamesys)
        {
            DataTable dt = PointsDac.SelectList(string.Format(" and gamesys='{0}' and servetype='2'", _Gamesys));
            return dt.Rows.Count;
        }

        /// <summary>
        /// 根据比赛主键获取正常比赛的双误数量
        /// </summary>
        /// <param name="_Matchsys"></param>
        /// <returns></returns>
        private TennisStats GetDoubleFaultsByMatch(string _Matchsys)
        {
            int P1 = 0;
            int P2 = 0;
            TennisStats model = new TennisStats();
            model.INDEX = "双误";

            MatchModel mmodel = MatchDll.Get_Instance().GetModel(_Matchsys);
            List<SetModel> SetList = SetDll.Get_Instance().GetModelList(_Matchsys);
            foreach (SetModel smodel in SetList)
            {
                List<GameModel> GameList = GameDll.Get_Instance().GetModellist(smodel.SYS);
                foreach (GameModel gmodel in GameList)
                {
                    if (gmodel.ISTIEBREAK == 0)
                    {
                        if (gmodel.MSERVER == mmodel.PLAYER1)
                        {
                            //统计P1的ACE球
                            P1 += GetGameDoubleFault(gmodel.SYS);
                        }
                        else
                        {
                            //统计P2的ACE球
                            P2 += GetGameDoubleFault(gmodel.SYS);
                        }
                    }
                    else
                    {
                        #region 统计抢七
                        List<PointsModel> listp = PointsDll.Get_Instance().GetModellist(gmodel.SYS);
                        if (listp.Count > 0)
                        {
                            foreach (PointsModel pmodel in listp)
                            {
                                if (pmodel.SERVETYPE == 2)
                                {
                                    if (pmodel.GORDER == 1)
                                    {
                                        if (gmodel.MSERVER == mmodel.PLAYER1)
                                        {
                                            P1 += 1;
                                        }
                                        else
                                        {
                                            P2 += 1;
                                        }
                                    }
                                    else
                                    {
                                        //not the first point
                                        int x = 0;
                                        if (pmodel.GORDER % 2 == 0)
                                        {
                                            x = pmodel.GORDER / 2;
                                        }
                                        else
                                        {
                                            x = (pmodel.GORDER - 1) / 2;
                                        }

                                        if (x % 2 == 0)
                                        {
                                            //发球方是本局标记的发球方
                                            if (gmodel.MSERVER == mmodel.PLAYER1)
                                            {
                                                P1 += 1;
                                            }
                                            else
                                            {
                                                P2 += 1;
                                            }


                                        }
                                        else
                                        {
                                            //发球方不是本局标记的发球方
                                            if (gmodel.MSERVER == mmodel.PLAYER1)
                                            {
                                                P2 += 1;
                                            }
                                            else
                                            {
                                                P1 += 1;
                                            }
                                        }
                                    }
                                }
                            }
                        }
                        #endregion
                    }
                }
            }

            model.P1 = P1.ToString();
            model.P2 = P2.ToString();

            return model;
        }

        /// <summary>
        /// 获取一盘的双误数量
        /// </summary>
        /// <param name="_Setsys"></param>
        /// <returns></returns>
        private TennisStats GetDoubleFaultsBySet(string _Setsys)
        {
            int P1 = 0;
            int P2 = 0;
            TennisStats model = new TennisStats();
            model.INDEX = "双误";

            SetModel smodel = SetDll.Get_Instance().GetModel(_Setsys);
            MatchModel mmodel = MatchDll.Get_Instance().GetModel(smodel.MATCHSYS);
            List<GameModel> GameList = GameDll.Get_Instance().GetModellist(smodel.SYS);
            foreach (GameModel gmodel in GameList)
            {
                if (gmodel.MSERVER == mmodel.PLAYER1)
                {
                    //统计P1的ACE球
                    P1 += GetGameDoubleFault(gmodel.SYS);
                }
                else
                {
                    //统计P2的ACE球
                    P2 += GetGameDoubleFault(gmodel.SYS);
                }
            }

            model.P1 = P1.ToString();
            model.P2 = P2.ToString();

            return model;
        }


        //------------------统计一发成功率--------------------------//
        //--球员的发球局里面，所有发球数量为分母，所有的一发数量为分子
        /// <summary>
        /// 获取当局的分数
        /// </summary>
        /// <param name="_Gamesys"></param>
        /// <returns></returns>
        private int GetGameServe(string _Gamesys)
        {
            DataTable dt = PointsDac.SelectList(string.Format(" and gamesys='{0}'", _Gamesys));
            return dt.Rows.Count;
        }

        /// <summary>
        /// 统计一局里面一发的个数 
        /// </summary>
        /// <param name="_Gamesys"></param>
        /// <returns></returns>
        private int GetGameFstServe(string _Gamesys)
        {
            DataTable dt = PointsDac.SelectList(string.Format(" and gamesys='{0}' and servetype='0'", _Gamesys));
            return dt.Rows.Count;
        }

        /// <summary>
        /// 获取比赛的一发成功率
        /// </summary>
        /// <param name="_Matchsys"></param>
        /// <returns></returns>
        private TennisStats GetFstServebyMatch(string _Matchsys)
        {
            int P1 = 0;
            int FP1 = 0;
            int P2 = 0;
            int FP2 = 0;
            TennisStats model = new TennisStats();
            model.INDEX = "一发成功率";

            MatchModel mmodel = MatchDll.Get_Instance().GetModel(_Matchsys);
            List<SetModel> SetList = SetDll.Get_Instance().GetModelList(_Matchsys);
            foreach (SetModel smodel in SetList)
            {
                List<GameModel> GameList = GameDll.Get_Instance().GetModellist(smodel.SYS);
                foreach (GameModel gmodel in GameList)
                {
                    if (gmodel.ISTIEBREAK == 0)
                    {
                        if (gmodel.MSERVER == mmodel.PLAYER1)
                        {
                            //统计P1的发球数
                            P1 += GetGameServe(gmodel.SYS);
                            //统计P1的一发数
                            FP1 += GetGameFstServe(gmodel.SYS);
                        }
                        else
                        {
                            //统计P2的发球数
                            P2 += GetGameServe(gmodel.SYS);
                            //统计P2的一发数
                            FP2 += GetGameFstServe(gmodel.SYS);
                        }
                    }
                    else
                    {
                        #region 统计抢七
                        List<PointsModel> listp = PointsDll.Get_Instance().GetModellist(gmodel.SYS);
                        if (listp.Count > 0)
                        {
                            foreach (PointsModel pmodel in listp)
                            {

                                if (pmodel.GORDER == 1)
                                {
                                    if (gmodel.MSERVER == mmodel.PLAYER1)
                                    {
                                        P1 += 1;
                                        if (pmodel.SERVETYPE == 0)
                                        {
                                            FP1 += 1;
                                        }
                                    }
                                    else
                                    {
                                        P2 += 1;
                                        if (pmodel.SERVETYPE == 0)
                                        {
                                            FP2 += 1;
                                        }
                                    }
                                }
                                else
                                {
                                    //not the first point
                                    int x = 0;
                                    if (pmodel.GORDER % 2 == 0)
                                    {
                                        x = pmodel.GORDER / 2;
                                    }
                                    else
                                    {
                                        x = (pmodel.GORDER - 1) / 2;
                                    }

                                    if (x % 2 == 0)
                                    {
                                        //发球方是本局标记的发球方
                                        if (gmodel.MSERVER == mmodel.PLAYER1)
                                        {
                                            P1 += 1;
                                            if (pmodel.SERVETYPE == 0)
                                            {
                                                FP1 += 1;
                                            }
                                        }
                                        else
                                        {
                                            P2 += 1;
                                            if (pmodel.SERVETYPE == 0)
                                            {
                                                FP2 += 1;
                                            }
                                        }
                                    }
                                    else
                                    {
                                        //发球方不是本局标记的发球方
                                        if (gmodel.MSERVER == mmodel.PLAYER1)
                                        {
                                            P2 += 1;
                                            if (pmodel.SERVETYPE == 0)
                                            {
                                                FP2 += 1;
                                            }
                                        }
                                        else
                                        {
                                            P1 += 1;
                                            if (pmodel.SERVETYPE == 0)
                                            {
                                                FP1 += 1;
                                            }
                                        }
                                    }
                                }
                            }
                        }
                        #endregion

                    }
                }
            }

            if (P1 != 0)
            {
                model.P1 = FP1.ToString() + "/" + P1.ToString() + " (" + Math.Round(Convert.ToDouble(FP1 * 100 / P1), 0) + "%)";
            }
            else
            {
                model.P2 = "0/0 (100%)";
            }

            if (P2 != 0)
            {
                model.P2 = FP2.ToString() + "/" + P2.ToString() + " (" + Math.Round(Convert.ToDouble(FP2 * 100 / P2), 0) + "%)";
            }
            else
            {
                model.P2 = "0/0 (100%)";
            }
            return model;
        }

        /// <summary>
        /// 获取一盘的一发成功率
        /// </summary>
        /// <param name="_Setsys"></param>
        /// <returns></returns>
        private TennisStats GetFstServeBySet(string _Setsys)
        {
            int P1 = 0;
            int FP1 = 0;
            int P2 = 0;
            int FP2 = 0;
            TennisStats model = new TennisStats();
            model.INDEX = "一发成功率";

            SetModel smodel = SetDll.Get_Instance().GetModel(_Setsys);
            MatchModel mmodel = MatchDll.Get_Instance().GetModel(smodel.MATCHSYS);
            List<GameModel> GameList = GameDll.Get_Instance().GetModellist(smodel.SYS);
            foreach (GameModel gmodel in GameList)
            {
                if (gmodel.MSERVER == mmodel.PLAYER1)
                {
                    //统计P1的发球数
                    P1 += GetGameServe(gmodel.SYS);
                    //统计P1的一发数
                    FP1 += GetGameFstServe(gmodel.SYS);
                }
                else
                {
                    //统计P2的发球数
                    P2 += GetGameServe(gmodel.SYS);
                    //统计P2的一发数
                    FP2 += GetGameFstServe(gmodel.SYS);
                }
            }

            if (P1 != 0)
            {
                model.P1 = FP1.ToString() + "/" + P1.ToString() + " (" + Math.Round(Convert.ToDouble(FP1 * 100 / P1), 0) + "%)";
            }
            else
            {
                model.P2 = "0/0 (100%)";
            }

            if (P2 != 0)
            {
                model.P2 = FP2.ToString() + "/" + P2.ToString() + " (" + Math.Round(Convert.ToDouble(FP2 * 100 / P2), 0) + "%)";
            }
            else
            {
                model.P2 = "0/0 (100%)";
            }

            return model;
        }

        //------------------添加一发得分率---------------------//
        //-------------分母是所有一发数量，分子是得分数量----

        /// <summary>
        /// 获取当局的一发得分数量
        /// </summary>
        /// <param name="_Gamesys"></param>
        /// <returns></returns>
        private int GetFstServeWon(string _Gamesys, string _Server)
        {
            DataTable dt = PointsDac.SelectList(string.Format(" and gamesys='{0}' and servetype='0' and winner='{1}'", _Gamesys, _Server));
            return dt.Rows.Count;
        }

        /// <summary>
        /// 获取比赛的一发成功率
        /// </summary>
        /// <param name="_Matchsys"></param>
        /// <returns></returns>
        private TennisStats GetFstServeWonbyMatch(string _Matchsys)
        {
            int P1 = 0;
            int FP1 = 0;
            int P2 = 0;
            int FP2 = 0;
            TennisStats model = new TennisStats();
            model.INDEX = "一发得分率";

            MatchModel mmodel = MatchDll.Get_Instance().GetModel(_Matchsys);
            List<SetModel> SetList = SetDll.Get_Instance().GetModelList(_Matchsys);
            foreach (SetModel smodel in SetList)
            {
                List<GameModel> GameList = GameDll.Get_Instance().GetModellist(smodel.SYS);
                foreach (GameModel gmodel in GameList)
                {
                    if (gmodel.ISTIEBREAK == 0)
                    {
                        if (gmodel.MSERVER == mmodel.PLAYER1)
                        {
                            //统计P1的一发得分率
                            FP1 += GetFstServeWon(gmodel.SYS, mmodel.PLAYER1);
                            //统计P1的一发数
                            P1 += GetGameFstServe(gmodel.SYS);
                        }
                        else
                        {
                            //统计P2的一发得分率
                            FP2 += GetFstServeWon(gmodel.SYS, mmodel.PLAYER2);
                            //统计P2的一发数
                            P2 += GetGameFstServe(gmodel.SYS);
                        }
                    }
                    else
                    {
                        #region 统计抢七
                        List<PointsModel> listp = PointsDll.Get_Instance().GetModellist(gmodel.SYS);
                        if (listp.Count > 0)
                        {
                            foreach (PointsModel pmodel in listp)
                            {
                                if (pmodel.SERVETYPE == 0)
                                {
                                    if (pmodel.GORDER == 1)
                                    {
                                        if (gmodel.MSERVER == mmodel.PLAYER1)
                                        {
                                            P1 += 1;
                                        }
                                        else
                                        {
                                            P2 += 1;
                                        }

                                        if (pmodel.WINNER == mmodel.PLAYER1)
                                        {
                                            FP1 += 1;
                                        }
                                        else
                                        {
                                            FP2 += 1;
                                        }
                                    }
                                    else
                                    {
                                        //not the first point
                                        int x = 0;
                                        if (pmodel.GORDER % 2 == 0)
                                        {
                                            x = pmodel.GORDER / 2;
                                        }
                                        else
                                        {
                                            x = (pmodel.GORDER - 1) / 2;
                                        }

                                        if (x % 2 == 0)
                                        {
                                            //发球方是本局标记的发球方
                                            if (gmodel.MSERVER == mmodel.PLAYER1)
                                            {
                                                P1 += 1;
                                            }
                                            else
                                            {
                                                P2 += 1;
                                            }

                                            if (pmodel.WINNER == mmodel.PLAYER1)
                                            {
                                                FP1 += 1;
                                            }
                                            else
                                            {
                                                FP2 += 1;
                                            }
                                        }
                                        else
                                        {
                                            //发球方不是本局标记的发球方
                                            if (gmodel.MSERVER == mmodel.PLAYER1)
                                            {
                                                P2 += 1;
                                            }
                                            else
                                            {
                                                P1 += 1;
                                            }

                                            if (pmodel.WINNER == mmodel.PLAYER1)
                                            {
                                                FP2 += 1;
                                            }
                                            else
                                            {
                                                FP1 += 1;
                                            }
                                        }
                                    }
                                }
                            }
                        }
                        #endregion

                    }
                }
            }

            if (P1 != 0)
            {
                model.P1 = FP1.ToString() + "/" + P1.ToString() + " (" + Math.Round(Convert.ToDouble(FP1 * 100 / P1), 0) + "%)";
            }
            else
            {
                model.P2 = "0/0 (100%)";
            }

            if (P2 != 0)
            {
                model.P2 = FP2.ToString() + "/" + P2.ToString() + " (" + Math.Round(Convert.ToDouble(FP2 * 100 / P2), 0) + "%)";
            }
            else
            {
                model.P2 = "0/0 (100%)";
            }

            return model;
        }

        /// <summary>
        /// 获取一盘的一发成功率
        /// </summary>
        /// <param name="_Setsys"></param>
        /// <returns></returns>
        private TennisStats GetFstServeWonBySet(string _Setsys)
        {
            int P1 = 0;
            int FP1 = 0;
            int P2 = 0;
            int FP2 = 0;
            TennisStats model = new TennisStats();
            model.INDEX = "一发得分率";

            SetModel smodel = SetDll.Get_Instance().GetModel(_Setsys);
            MatchModel mmodel = MatchDll.Get_Instance().GetModel(smodel.MATCHSYS);
            List<GameModel> GameList = GameDll.Get_Instance().GetModellist(smodel.SYS);
            foreach (GameModel gmodel in GameList)
            {
                if (gmodel.MSERVER == mmodel.PLAYER1)
                {
                    //统计P1的一发得分数
                    FP1 += GetFstServeWon(gmodel.SYS, mmodel.PLAYER1);
                    //统计P1的一发数
                    P1 += GetGameFstServe(gmodel.SYS);
                }
                else
                {
                    //统计P2的一发得分率
                    FP2 += GetFstServeWon(gmodel.SYS, mmodel.PLAYER2);
                    //统计P2的一发数
                    P2 += GetGameFstServe(gmodel.SYS);
                }
            }

            if (P1 != 0)
            {
                model.P1 = FP1.ToString() + "/" + P1.ToString() + " (" + Math.Round(Convert.ToDouble(FP1 * 100 / P1), 0) + "%)";
            }
            else
            {
                model.P2 = "0/0 (100%)";
            }

            if (P2 != 0)
            {
                model.P2 = FP2.ToString() + "/" + P2.ToString() + " (" + Math.Round(Convert.ToDouble(FP2 * 100 / P2), 0) + "%)";
            }
            else
            {
                model.P2 = "0/0 (100%)";
            }
            return model;
        }

        //----------------统计二发得分率--------------------------//
        //---------------分母是二发数量，分子是二发得分数量-------------------//
        /// <summary>
        /// 获取当局二发个数
        /// </summary>
        /// <param name="_Gamesys"></param>
        /// <returns></returns>
        private int GetGameSndServe(string _Gamesys)
        {
            DataTable dt = PointsDac.SelectList(string.Format(" and gamesys='{0}' and servetype='1'", _Gamesys));
            return dt.Rows.Count;
        }

        /// <summary>
        /// 统计一局里面一发的个数 
        /// </summary>
        /// <param name="_Gamesys"></param>
        /// <returns></returns>
        private int GetGameSndServeWon(string _Gamesys, string _Server)
        {
            DataTable dt = PointsDac.SelectList(string.Format(" and gamesys='{0}' and servetype='1' and winner='{1}'", _Gamesys, _Server));
            return dt.Rows.Count;
        }

        /// <summary>
        /// 获取比赛的二发得分率
        /// </summary>
        /// <param name="_Matchsys"></param>
        /// <returns></returns>
        private TennisStats GetSndServeWonbyMatch(string _Matchsys)
        {
            int P1 = 0;
            int FP1 = 0;
            int P2 = 0;
            int FP2 = 0;
            TennisStats model = new TennisStats();
            model.INDEX = "二发得分率";

            MatchModel mmodel = MatchDll.Get_Instance().GetModel(_Matchsys);
            List<SetModel> SetList = SetDll.Get_Instance().GetModelList(_Matchsys);
            foreach (SetModel smodel in SetList)
            {
                List<GameModel> GameList = GameDll.Get_Instance().GetModellist(smodel.SYS);
                foreach (GameModel gmodel in GameList)
                {
                    if (gmodel.ISTIEBREAK == 0)
                    {
                        if (gmodel.MSERVER == mmodel.PLAYER1)
                        {
                            //统计P1的二发得分数
                            FP1 += GetGameSndServeWon(gmodel.SYS, mmodel.PLAYER1);
                            //统计P1的一发数
                            P1 += GetGameSndServe(gmodel.SYS);
                        }
                        else
                        {
                            //统计P2的一发得分率
                            FP2 += GetGameSndServeWon(gmodel.SYS, mmodel.PLAYER2);
                            //统计P2的一发数
                            P2 += GetGameSndServe(gmodel.SYS);
                        }
                    }
                    else
                    {
                        #region 统计抢七
                        List<PointsModel> listp = PointsDll.Get_Instance().GetModellist(gmodel.SYS);
                        if (listp.Count > 0)
                        {
                            foreach (PointsModel pmodel in listp)
                            {
                                if (pmodel.SERVETYPE == 1)
                                {
                                    if (pmodel.GORDER == 1)
                                    {
                                        if (gmodel.MSERVER == mmodel.PLAYER1)
                                        {
                                            P1 += 1;
                                        }
                                        else
                                        {
                                            P2 += 1;
                                        }

                                        if (pmodel.WINNER == mmodel.PLAYER1)
                                        {
                                            FP1 += 1;
                                        }
                                        else
                                        {
                                            FP2 += 1;
                                        }
                                    }
                                    else
                                    {
                                        //not the first point
                                        int x = 0;
                                        if (pmodel.GORDER % 2 == 0)
                                        {
                                            x = pmodel.GORDER / 2;
                                        }
                                        else
                                        {
                                            x = (pmodel.GORDER - 1) / 2;
                                        }

                                        if (x % 2 == 0)
                                        {
                                            //发球方是本局标记的发球方
                                            if (gmodel.MSERVER == mmodel.PLAYER1)
                                            {
                                                P1 += 1;
                                            }
                                            else
                                            {
                                                P2 += 1;
                                            }

                                            if (pmodel.WINNER == mmodel.PLAYER1)
                                            {
                                                FP1 += 1;
                                            }
                                            else
                                            {
                                                FP2 += 1;
                                            }
                                        }
                                        else
                                        {
                                            //发球方不是本局标记的发球方
                                            if (gmodel.MSERVER == mmodel.PLAYER1)
                                            {
                                                P2 += 1;
                                            }
                                            else
                                            {
                                                P1 += 1;
                                            }

                                            if (pmodel.WINNER == mmodel.PLAYER1)
                                            {
                                                FP2 += 1;
                                            }
                                            else
                                            {
                                                FP1 += 1;
                                            }
                                        }
                                    }
                                }
                            }
                        }
                        #endregion
                    }

                }
            }
            if (P1 != 0)
            {
                model.P1 = FP1.ToString() + "/" + P1.ToString() + " (" + Math.Round(Convert.ToDouble(FP1 * 100 / P1), 0) + "%)";
            }
            else
            {
                model.P1 = "0/0 (100%)";
            }

            if (P2 != 0)
            {
                model.P2 = FP2.ToString() + "/" + P2.ToString() + " (" + Math.Round(Convert.ToDouble(FP2 * 100 / P2), 0) + "%)";
            }
            else
            {
                model.P2 = "0/0 (100%)";
            }

            return model;
        }

        /// <summary>
        /// 获取一盘的二发得分率
        /// </summary>
        /// <param name="_Setsys"></param>
        /// <returns></returns>
        private TennisStats GetSndServeWonBySet(string _Setsys)
        {
            int P1 = 0;
            int FP1 = 0;
            int P2 = 0;
            int FP2 = 0;
            TennisStats model = new TennisStats();
            model.INDEX = "二发得分率";

            SetModel smodel = SetDll.Get_Instance().GetModel(_Setsys);
            MatchModel mmodel = MatchDll.Get_Instance().GetModel(smodel.MATCHSYS);
            List<GameModel> GameList = GameDll.Get_Instance().GetModellist(smodel.SYS);
            foreach (GameModel gmodel in GameList)
            {
                if (gmodel.MSERVER == mmodel.PLAYER1)
                {
                    //统计P1的二发得分数
                    FP1 += GetGameSndServeWon(gmodel.SYS, mmodel.PLAYER1);
                    //统计P1的一发数
                    P1 += GetGameSndServe(gmodel.SYS);
                }
                else
                {
                    //统计P2的一发得分率
                    FP2 += GetGameSndServeWon(gmodel.SYS, mmodel.PLAYER2);
                    //统计P2的一发数
                    P2 += GetGameSndServe(gmodel.SYS);
                }
            }

            if (P1 != 0)
            {
                model.P1 = FP1.ToString() + "/" + P1.ToString() + " (" + Math.Round(Convert.ToDouble(FP1 * 100 / P1), 0) + "%)";
            }
            else
            {
                model.P1 = "0/0 (100%)";
            }

            if (P2 != 0)
            {
                model.P2 = FP2.ToString() + "/" + P2.ToString() + " (" + Math.Round(Convert.ToDouble(FP2 * 100 / P2), 0) + "%)";
            }
            else
            {
                model.P2 = "0/0 (100%)";
            }

            return model;
        }

        //---------------统计破发点赢球率--------------//
        //------------分母是所有破发点分数，分子是这些分数当中的赢球数--------

        //----------统计发球局数----------------------//
        //-----------根据盘主键来获得所有局数当中发球者是响应球员的数量----------

        /// <summary>
        /// 根据盘主键来获取发球局数
        /// </summary>
        /// <param name="_Setsys"></param>
        /// <param name="_Server"></param>
        /// <returns></returns>
        private int GetServeGame(string _Setsys, string _Server)
        {
            DataTable dt = GameDac.SelectList(string.Format(" and setsys='{0}' and mserver='{1}' and state=2 and isbreakpoint=0", _Setsys, _Server));
            return dt.Rows.Count;
        }

        /// <summary>
        /// 根据 比赛主键获得发球局数
        /// </summary>
        /// <param name="_Matchsys"></param>
        /// <returns></returns>
        private TennisStats GetServeGameByMatch(string _Matchsys)
        {
            int P1 = 0;
            int P2 = 0;
            TennisStats model = new TennisStats();
            model.INDEX = "发球局数";

            MatchModel mmodel = MatchDll.Get_Instance().GetModel(_Matchsys);
            List<SetModel> SetList = SetDll.Get_Instance().GetModelList(_Matchsys);
            foreach (SetModel smodel in SetList)
            {
                //获得球员1的发球局数
                P1 += GetServeGame(smodel.SYS, mmodel.PLAYER1);

                //获得球员1的发球局数
                P2 += GetServeGame(smodel.SYS, mmodel.PLAYER2);

            }
            model.P1 = P1.ToString();
            model.P2 = P2.ToString();

            return model;
        }

        /// <summary>
        /// 根据盘主键获得发球局数量
        /// </summary>
        /// <param name="_Setsys"></param>
        /// <returns></returns>
        private TennisStats GetServeGameBySet(string _Setsys)
        {
            TennisStats model = new TennisStats();
            SetModel smodel = SetDll.Get_Instance().GetModel(_Setsys);
            MatchModel mmodel = MatchDll.Get_Instance().GetModel(smodel.MATCHSYS);
            model.INDEX = "发球局数";
            model.P1 = GetServeGame(_Setsys, mmodel.PLAYER1).ToString();
            model.P2 = GetServeGame(_Setsys, mmodel.PLAYER2).ToString();

            return model;
        }

        //------------一发回球得分率-------------------------//
        //-----------分母是对方球员的一发数量，分子是这些分数当中的得分数量----------------------//

        /// <summary>
        /// 获取比赛的一发回球得分率
        /// </summary>
        /// <param name="_Matchsys"></param>
        /// <returns></returns>
        private TennisStats GetFstReturnWonbyMatch(string _Matchsys)
        {
            int P1 = 0;
            int FP1 = 0;
            int P2 = 0;
            int FP2 = 0;
            TennisStats model = new TennisStats();
            model.INDEX = "一发回球得分率";

            MatchModel mmodel = MatchDll.Get_Instance().GetModel(_Matchsys);//获得比赛实体
            List<SetModel> SetList = SetDll.Get_Instance().GetModelList(_Matchsys);//根据比赛主键获得盘列表
            foreach (SetModel smodel in SetList)
            {
                List<GameModel> GameList = GameDll.Get_Instance().GetModellist(smodel.SYS);//根据盘主键，获得局列表
                foreach (GameModel gmodel in GameList)
                {
                    if (gmodel.ISTIEBREAK == 0)
                    {
                        if (gmodel.MSERVER == mmodel.PLAYER1)
                        {
                            //统计P2的一发回球的得分数
                            FP2 += GetFstServeWon(gmodel.SYS, mmodel.PLAYER2);
                            //统计P2的一发回球数量
                            P2 += GetGameFstServe(gmodel.SYS);
                        }
                        else
                        {
                            //统计P1的一发回球的得分数
                            FP1 += GetFstServeWon(gmodel.SYS, mmodel.PLAYER1);
                            //统计P1的一发回球数量
                            P1 += GetGameFstServe(gmodel.SYS);
                        }
                    }
                    else
                    {
                        #region 统计抢七
                        List<PointsModel> listp = PointsDll.Get_Instance().GetModellist(gmodel.SYS);
                        if (listp.Count > 0)
                        {
                            foreach (PointsModel pmodel in listp)
                            {
                                if (pmodel.SERVETYPE == 0)
                                {
                                    if (pmodel.GORDER == 1)
                                    {
                                        if (gmodel.MSERVER == mmodel.PLAYER1)
                                        {
                                            P2 += 1;
                                        }
                                        else
                                        {
                                            P1 += 1;
                                        }

                                        if (pmodel.WINNER == mmodel.PLAYER1)
                                        {
                                            FP1 += 1;
                                        }
                                        else
                                        {
                                            FP2 += 1;
                                        }
                                    }
                                    else
                                    {
                                        //not the first point
                                        int x = 0;
                                        if (pmodel.GORDER % 2 == 0)
                                        {
                                            x = pmodel.GORDER / 2;
                                        }
                                        else
                                        {
                                            x = (pmodel.GORDER - 1) / 2;
                                        }

                                        if (x % 2 == 0)
                                        {
                                            //发球方是本局标记的发球方
                                            if (gmodel.MSERVER == mmodel.PLAYER1)
                                            {
                                                P2 += 1;
                                            }
                                            else
                                            {
                                                P1 += 1;
                                            }

                                            if (pmodel.WINNER == mmodel.PLAYER1)
                                            {
                                                FP1 += 1;
                                            }
                                            else
                                            {
                                                FP2 += 1;
                                            }
                                        }
                                        else
                                        {
                                            //发球方不是本局标记的发球方
                                            if (gmodel.MSERVER == mmodel.PLAYER1)
                                            {
                                                P1 += 1;
                                            }
                                            else
                                            {
                                                P2 += 1;
                                            }

                                            if (pmodel.WINNER == mmodel.PLAYER1)
                                            {
                                                FP2 += 1;
                                            }
                                            else
                                            {
                                                FP1 += 1;
                                            }
                                        }
                                    }
                                }
                            }
                        }
                        #endregion
                    }
                }
            }
            if (P1 != 0)
            {
                model.P1 = FP1.ToString() + "/" + P1.ToString() + " (" + Math.Round(Convert.ToDouble(FP1 * 100 / P1), 0) + "%)";
            }
            else
            {
                model.P1 = "0/0 (100%)";
            }

            if (P2 != 0)
            {
                model.P2 = FP2.ToString() + "/" + P2.ToString() + " (" + Math.Round(Convert.ToDouble(FP2 * 100 / P2), 0) + "%)";
            }
            else
            {
                model.P2 = "0/0 (100%)";
            }

            return model;
        }

        /// <summary>
        /// 获取一盘的一发回球得分率
        /// </summary>
        /// <param name="_Setsys"></param>
        /// <returns></returns>
        private TennisStats GetFstReturnWonBySet(string _Setsys)
        {
            int P1 = 0;
            int FP1 = 0;
            int P2 = 0;
            int FP2 = 0;
            TennisStats model = new TennisStats();
            model.INDEX = "一发回球得分率";

            SetModel smodel = SetDll.Get_Instance().GetModel(_Setsys);
            MatchModel mmodel = MatchDll.Get_Instance().GetModel(smodel.MATCHSYS);
            List<GameModel> GameList = GameDll.Get_Instance().GetModellist(smodel.SYS);
            foreach (GameModel gmodel in GameList)
            {
                if (gmodel.MSERVER == mmodel.PLAYER1)
                {
                    //统计P2的一发回球得分数
                    FP2 += GetFstServeWon(gmodel.SYS, mmodel.PLAYER2);
                    //统计P2的一发回球数量
                    P2 += GetGameFstServe(gmodel.SYS);
                }
                else
                {
                    //统计P1的一发得分率
                    FP1 += GetFstServeWon(gmodel.SYS, mmodel.PLAYER1);
                    //统计P1的一发数
                    P1 += GetGameFstServe(gmodel.SYS);
                }
            }

            if (P1 != 0)
            {
                model.P1 = FP1.ToString() + "/" + P1.ToString() + " (" + Math.Round(Convert.ToDouble(FP1 * 100 / P1), 0) + "%)";
            }
            else
            {
                model.P1 = "0/0 (100%)";
            }

            if (P2 != 0)
            {
                model.P2 = FP2.ToString() + "/" + P2.ToString() + " (" + Math.Round(Convert.ToDouble(FP2 * 100 / P2), 0) + "%)";
            }
            else
            {
                model.P2 = "0/0 (100%)";
            }

            return model;
        }



        //------------一破发点保发成功率-------------------------//
        //-----------分母是自己发球局出现的破发点数量，分子是这些分数当中的得分数量----------------------//

        /// <summary>
        /// 根据盘主键来获取破发点数量
        /// </summary>
        /// <param name="_Setsys"></param>
        /// <param name="_Server"></param>
        /// <returns></returns>
        private int GetGameBreakPoint(string _Gamesys)
        {
            DataTable dt = PointsDac.SelectList(string.Format(" and gamesys='{0}' and isbreakpoint=1 ", _Gamesys));
            return dt.Rows.Count;
        }

        /// <summary>
        /// 获取破发点获胜分数
        /// </summary>
        /// <param name="_GameSys"></param>
        /// <param name="_Winner"></param>
        /// <returns></returns>
        private int GetGameBreakPointWon(string _GameSys, string _Winner)
        {
            DataTable dt = PointsDac.SelectList(string.Format(" and gamesys='{0}' and isbreakpoint=1 and winner='{1}'", _GameSys, _Winner));
            return dt.Rows.Count;
        }

        /// <summary>
        /// 获取比赛的破发点保发成功率
        /// </summary>
        /// <param name="_Matchsys"></param>
        /// <returns></returns>
        private TennisStats GetBreakPointSavedWonbyMatch(string _Matchsys)
        {
            int P1 = 0;
            int FP1 = 0;
            int P2 = 0;
            int FP2 = 0;
            TennisStats model = new TennisStats();
            model.INDEX = "破发点保发成功率";

            MatchModel mmodel = MatchDll.Get_Instance().GetModel(_Matchsys);
            List<SetModel> SetList = SetDll.Get_Instance().GetModelList(_Matchsys);
            foreach (SetModel smodel in SetList)
            {
                List<GameModel> GameList = GameDll.Get_Instance().GetModellist(smodel.SYS);
                foreach (GameModel gmodel in GameList)
                {
                    if (gmodel.ISTIEBREAK == 0)
                    {
                        if (gmodel.MSERVER == mmodel.PLAYER1)
                        {
                            //统计P1的破发点保发成功率
                            FP1 += GetGameBreakPointWon(gmodel.SYS, mmodel.PLAYER1);
                            //统计P1的破发点保发成功率
                            P1 += GetGameBreakPoint(gmodel.SYS);
                        }
                        else
                        {
                            //统计P2的破发点保发成功率
                            FP2 += GetGameBreakPointWon(gmodel.SYS, mmodel.PLAYER2);
                            //统计P2的破发点保发成功率
                            P2 += GetGameBreakPoint(gmodel.SYS);
                        }
                    }
                }
            }
            if (P1 != 0)
            {
                model.P1 = FP1.ToString() + "/" + P1.ToString() + " (" + Math.Round(Convert.ToDouble(FP1 * 100 / P1), 0) + "%)";
            }
            else
            {
                model.P1 = "0/0 (100%)";
            }

            if (P2 != 0)
            {
                model.P2 = FP2.ToString() + "/" + P2.ToString() + " (" + Math.Round(Convert.ToDouble(FP2 * 100 / P2), 0) + "%)";
            }
            else
            {
                model.P2 = "0/0 (100%)";
            }

            return model;
        }

        /// <summary>
        /// 获取一盘的破发点保发成功率
        /// </summary>
        /// <param name="_Setsys"></param>
        /// <returns></returns>
        private TennisStats GetBreakPointSavedBySet(string _Setsys)
        {
            int P1 = 0;
            int FP1 = 0;
            int P2 = 0;
            int FP2 = 0;
            TennisStats model = new TennisStats();
            model.INDEX = "破发点保发成功率";

            SetModel smodel = SetDll.Get_Instance().GetModel(_Setsys);
            MatchModel mmodel = MatchDll.Get_Instance().GetModel(smodel.MATCHSYS);
            List<GameModel> GameList = GameDll.Get_Instance().GetModellist(smodel.SYS);
            foreach (GameModel gmodel in GameList)
            {
                if (gmodel.MSERVER == mmodel.PLAYER1)
                {
                    //统计P1的破发点保发成功率
                    FP1 += GetGameBreakPointWon(gmodel.SYS, mmodel.PLAYER1);
                    //统计P1发球局的破发点数量
                    P1 += GetGameBreakPoint(gmodel.SYS);
                }
                else
                {
                    //统计P2的破发点保发成功率
                    FP2 += GetGameBreakPointWon(gmodel.SYS, mmodel.PLAYER2);
                    //统计P2的破发点数量
                    P2 += GetGameBreakPoint(gmodel.SYS);
                }
            }

            if (P1 != 0)
            {
                model.P1 = FP1.ToString() + "/" + P1.ToString() + " (" + Math.Round(Convert.ToDouble(FP1 * 100 / P1), 0) + "%)";
            }
            else
            {
                model.P1 = "0/0 (100%)";
            }

            if (P2 != 0)
            {
                model.P2 = FP2.ToString() + "/" + P2.ToString() + " (" + Math.Round(Convert.ToDouble(FP2 * 100 / P2), 0) + "%)";
            }
            else
            {
                model.P2 = "0/0 (100%)";
            }

            return model;
        }

        //------------二发回球得分率-------------------------//

        /// <summary>
        /// 获取比赛的二发回球得分率
        /// </summary>
        /// <param name="_Matchsys"></param>
        /// <returns></returns>
        private TennisStats GetSndReturnWonbyMatch(string _Matchsys)
        {
            int P1 = 0;
            int FP1 = 0;
            int P2 = 0;
            int FP2 = 0;
            TennisStats model = new TennisStats();
            model.INDEX = "二发回球得分率";

            MatchModel mmodel = MatchDll.Get_Instance().GetModel(_Matchsys);//获得比赛实体
            List<SetModel> SetList = SetDll.Get_Instance().GetModelList(_Matchsys);//根据比赛主键获得盘列表
            foreach (SetModel smodel in SetList)
            {
                List<GameModel> GameList = GameDll.Get_Instance().GetModellist(smodel.SYS);//根据盘主键，获得局列表
                foreach (GameModel gmodel in GameList)
                {
                    if (gmodel.ISTIEBREAK == 0)
                    {
                        if (gmodel.MSERVER == mmodel.PLAYER1)
                        {
                            //统计P2的二发回球的得分数
                            FP2 += GetGameSndServeWon(gmodel.SYS, mmodel.PLAYER2);
                            //统计P2的二发回球数量
                            P2 += GetGameSndServe(gmodel.SYS);
                        }
                        else
                        {
                            //统计P1的二发回球的得分数
                            FP1 += GetGameSndServeWon(gmodel.SYS, mmodel.PLAYER1);
                            //统计P1的二发回球数量
                            P1 += GetGameSndServe(gmodel.SYS);
                        }
                    }
                    else
                    {
                        #region 统计抢七
                        List<PointsModel> listp = PointsDll.Get_Instance().GetModellist(gmodel.SYS);
                        if (listp.Count > 0)
                        {
                            foreach (PointsModel pmodel in listp)
                            {
                                if (pmodel.SERVETYPE == 1)
                                {
                                    if (pmodel.GORDER == 1)
                                    {
                                        if (gmodel.MSERVER == mmodel.PLAYER1)
                                        {
                                            P2 += 1;
                                        }
                                        else
                                        {
                                            P1 += 1;
                                        }

                                        if (pmodel.WINNER == mmodel.PLAYER1)
                                        {
                                            FP1 += 1;
                                        }
                                        else
                                        {
                                            FP2 += 1;
                                        }
                                    }
                                    else
                                    {
                                        //not the first point
                                        int x = 0;
                                        if (pmodel.GORDER % 2 == 0)
                                        {
                                            x = pmodel.GORDER / 2;
                                        }
                                        else
                                        {
                                            x = (pmodel.GORDER - 1) / 2;
                                        }

                                        if (x % 2 == 0)
                                        {
                                            //发球方是本局标记的发球方
                                            if (gmodel.MSERVER == mmodel.PLAYER1)
                                            {
                                                P2 += 1;
                                            }
                                            else
                                            {
                                                P1 += 1;
                                            }

                                            if (pmodel.WINNER == mmodel.PLAYER1)
                                            {
                                                FP1 += 1;
                                            }
                                            else
                                            {
                                                FP2 += 1;
                                            }
                                        }
                                        else
                                        {
                                            //发球方不是本局标记的发球方
                                            if (gmodel.MSERVER == mmodel.PLAYER1)
                                            {
                                                P1 += 1;
                                            }
                                            else
                                            {
                                                P2 += 1;
                                            }

                                            if (pmodel.WINNER == mmodel.PLAYER1)
                                            {
                                                FP2 += 1;
                                            }
                                            else
                                            {
                                                FP1 += 1;
                                            }
                                        }
                                    }
                                }
                            }
                        }
                        #endregion
                    }
                }
            }
            if (P1 != 0)
            {
                model.P1 = FP1.ToString() + "/" + P1.ToString() + " (" + Math.Round(Convert.ToDouble(FP1 * 100 / P1), 0) + "%)";
            }
            else
            {
                model.P1 = "0/0 (100%)";
            }

            if (P2 != 0)
            {
                model.P2 = FP2.ToString() + "/" + P2.ToString() + " (" + Math.Round(Convert.ToDouble(FP2 * 100 / P2), 0) + "%)";
            }
            else
            {
                model.P2 = "0/0 (100%)";
            }

            return model;
        }

        /// <summary>
        /// 获取一盘的二发回球得分率
        /// </summary>
        /// <param name="_Setsys"></param>
        /// <returns></returns>
        private TennisStats GetSndReturnWonBySet(string _Setsys)
        {
            int P1 = 0;
            int FP1 = 0;
            int P2 = 0;
            int FP2 = 0;
            TennisStats model = new TennisStats();
            model.INDEX = "二发回球得分率";

            SetModel smodel = SetDll.Get_Instance().GetModel(_Setsys);
            MatchModel mmodel = MatchDll.Get_Instance().GetModel(smodel.MATCHSYS);
            List<GameModel> GameList = GameDll.Get_Instance().GetModellist(smodel.SYS);
            foreach (GameModel gmodel in GameList)
            {
                if (gmodel.MSERVER == mmodel.PLAYER1)
                {
                    //统计P2的二发回球得分数
                    FP2 += GetGameSndServeWon(gmodel.SYS, mmodel.PLAYER2);
                    //统计P2的二发回球数量
                    P2 += GetGameSndServe(gmodel.SYS);
                }
                else
                {
                    //统计P1的二发得分率
                    FP1 += GetGameSndServeWon(gmodel.SYS, mmodel.PLAYER1);
                    //统计P1的二发数
                    P1 += GetGameSndServe(gmodel.SYS);
                }
            }

            if (P1 != 0)
            {
                model.P1 = FP1.ToString() + "/" + P1.ToString() + " (" + Math.Round(Convert.ToDouble(FP1 * 100 / P1), 0) + "%)";
            }
            else
            {
                model.P1 = "0/0 (100%)";
            }

            if (P2 != 0)
            {
                model.P2 = FP2.ToString() + "/" + P2.ToString() + " (" + Math.Round(Convert.ToDouble(FP2 * 100 / P2), 0) + "%)";
            }
            else
            {
                model.P2 = "0/0 (100%)";
            }

            return model;
        }

        //------------一破发成功率-------------------------//

        /// <summary>
        /// 获取比赛的破发点保发成功率
        /// </summary>
        /// <param name="_Matchsys"></param>
        /// <returns></returns>
        private TennisStats GetBreakPointWonbyMatch(string _Matchsys)
        {
            int P1 = 0;
            int FP1 = 0;
            int P2 = 0;
            int FP2 = 0;
            TennisStats model = new TennisStats();
            model.INDEX = "破发成功率";

            MatchModel mmodel = MatchDll.Get_Instance().GetModel(_Matchsys);
            List<SetModel> SetList = SetDll.Get_Instance().GetModelList(_Matchsys);
            foreach (SetModel smodel in SetList)
            {
                List<GameModel> GameList = GameDll.Get_Instance().GetModellist(smodel.SYS);
                foreach (GameModel gmodel in GameList)
                {
                    if (gmodel.ISTIEBREAK == 0)
                    {
                        if (gmodel.MSERVER == mmodel.PLAYER1)
                        {
                            //统计P2的破发成功率
                            FP2 += GetGameBreakPointWon(gmodel.SYS, mmodel.PLAYER2);
                            //统计P2的逼出破发点数
                            P2 += GetGameBreakPoint(gmodel.SYS);
                        }
                        else
                        {
                            //统计P1的破发点保发成功率
                            FP1 += GetGameBreakPointWon(gmodel.SYS, mmodel.PLAYER1);
                            //统计P1的破发点保发成功率
                            P1 += GetGameBreakPoint(gmodel.SYS);
                        }
                    }
                }
            }
            if (P1 != 0)
            {
                model.P1 = FP1.ToString() + "/" + P1.ToString() + " (" + Math.Round(Convert.ToDouble(FP1 * 100 / P1), 0) + "%)";
            }
            else
            {
                model.P1 = "0/0 (100%)";
            }

            if (P2 != 0)
            {
                model.P2 = FP2.ToString() + "/" + P2.ToString() + " (" + Math.Round(Convert.ToDouble(FP2 * 100 / P2), 0) + "%)";
            }
            else
            {
                model.P2 = "0/0 (100%)";
            }

            return model;
        }

        /// <summary>
        /// 获取一盘的破发发成功率
        /// </summary>
        /// <param name="_Setsys"></param>
        /// <returns></returns>
        private TennisStats GetBreakPointWonBySet(string _Setsys)
        {
            int P1 = 0;
            int FP1 = 0;
            int P2 = 0;
            int FP2 = 0;
            TennisStats model = new TennisStats();
            model.INDEX = "破发成功率";

            SetModel smodel = SetDll.Get_Instance().GetModel(_Setsys);
            MatchModel mmodel = MatchDll.Get_Instance().GetModel(smodel.MATCHSYS);
            List<GameModel> GameList = GameDll.Get_Instance().GetModellist(smodel.SYS);
            foreach (GameModel gmodel in GameList)
            {
                if (gmodel.MSERVER == mmodel.PLAYER1)
                {
                    //统计P2的破发发成功率
                    FP2 += GetGameBreakPointWon(gmodel.SYS, mmodel.PLAYER2);
                    //统计P2的逼出的破发点数量
                    P2 += GetGameBreakPoint(gmodel.SYS);
                }
                else
                {
                    //统计P1的破发发成功率
                    FP1 += GetGameBreakPointWon(gmodel.SYS, mmodel.PLAYER2);
                    //统计P1的逼出的破发点数量
                    P1 += GetGameBreakPoint(gmodel.SYS);
                }
            }

            if (P1 != 0)
            {
                model.P1 = FP1.ToString() + "/" + P1.ToString() + " (" + Math.Round(Convert.ToDouble(FP1 * 100 / P1), 0) + "%)";
            }
            else
            {
                model.P1 = "0/0 (100%)";
            }

            if (P2 != 0)
            {
                model.P2 = FP2.ToString() + "/" + P2.ToString() + " (" + Math.Round(Convert.ToDouble(FP2 * 100 / P2), 0) + "%)";
            }
            else
            {
                model.P2 = "0/0 (100%)";
            }

            return model;
        }

        //------------------统计发球得分率--------------------------//

        /// <summary>
        /// 获取当局的得分数量
        /// </summary>
        /// <param name="_Gamesys"></param>
        /// <returns></returns>
        private int GetServeWon(string _Gamesys, string _Server)
        {
            DataTable dt = PointsDac.SelectList(string.Format(" and gamesys='{0}' and winner='{1}'", _Gamesys, _Server));
            return dt.Rows.Count;
        }

        /// <summary>
        /// 获取比赛的发球得分率
        /// </summary>
        /// <param name="_Matchsys"></param>
        /// <returns></returns>
        private TennisStats GetServeWonbyMatch(string _Matchsys)
        {
            int P1 = 0;
            int FP1 = 0;
            int P2 = 0;
            int FP2 = 0;
            TennisStats model = new TennisStats();
            model.INDEX = "发球总得分率";

            MatchModel mmodel = MatchDll.Get_Instance().GetModel(_Matchsys);
            List<SetModel> SetList = SetDll.Get_Instance().GetModelList(_Matchsys);
            foreach (SetModel smodel in SetList)
            {
                List<GameModel> GameList = GameDll.Get_Instance().GetModellist(smodel.SYS);
                foreach (GameModel gmodel in GameList)
                {
                    //非抢七局按照某一局的发球方来计算发球数，抢七局按照比分数据来计算
                    if (gmodel.ISTIEBREAK == 0)
                    {
                        //计算非抢七局
                        if (gmodel.MSERVER == mmodel.PLAYER1)
                        {
                            //统计P1的发球数
                            P1 += GetGameServe(gmodel.SYS);
                            //统计P1的发球得分数
                            FP1 += GetServeWon(gmodel.SYS, mmodel.PLAYER1);
                        }
                        else
                        {
                            //统计P2的发球数
                            P2 += GetGameServe(gmodel.SYS);
                            //统计P2的发球得分数
                            FP2 += GetServeWon(gmodel.SYS, mmodel.PLAYER2);
                        }
                    }
                    else
                    {
                        //计算抢七局
                        #region 统计抢七
                        List<PointsModel> listp = PointsDll.Get_Instance().GetModellist(gmodel.SYS);
                        if (listp.Count > 0)
                        {
                            foreach (PointsModel pmodel in listp)
                            {
                                if (pmodel.GORDER == 1)
                                {
                                    if (gmodel.MSERVER == mmodel.PLAYER1)
                                    {
                                        P1 += 1;
                                        if (pmodel.WINNER == mmodel.PLAYER1)
                                        {
                                            FP1 += 1;
                                        }
                                    }
                                    else
                                    {
                                        P2 += 1;
                                        if (pmodel.WINNER == mmodel.PLAYER2)
                                        {
                                            FP2 += 1;
                                        }
                                    }

                                }
                                else
                                {
                                    //not the first point
                                    int x = 0;
                                    if (pmodel.GORDER % 2 == 0)
                                    {
                                        x = pmodel.GORDER / 2;
                                    }
                                    else
                                    {
                                        x = (pmodel.GORDER - 1) / 2;
                                    }

                                    if (x % 2 == 0)
                                    {
                                        //发球方是本局标记的发球方
                                        if (gmodel.MSERVER == mmodel.PLAYER1)
                                        {
                                            P1 += 1;
                                            if (pmodel.WINNER == mmodel.PLAYER1)
                                            {
                                                FP1 += 1;
                                            }
                                        }
                                        else
                                        {
                                            P2 += 1;
                                            if (pmodel.WINNER == mmodel.PLAYER2)
                                            {
                                                FP2 += 1;
                                            }
                                        }
                                    }
                                    else
                                    {
                                        //发球方不是本局标记的发球方
                                        if (gmodel.MSERVER == mmodel.PLAYER1)
                                        {
                                            P2 += 1;
                                            if (pmodel.WINNER == mmodel.PLAYER2)
                                            {
                                                FP2 += 1;
                                            }
                                        }
                                        else
                                        {
                                            P1 += 1;
                                            if (pmodel.WINNER == mmodel.PLAYER1)
                                            {
                                                FP1 += 1;
                                            }
                                        }
                                    }
                                }
                            }
                        }
                        #endregion
                    }
                }
            }

            if (P1 != 0)
            {
                model.P1 = FP1.ToString() + "/" + P1.ToString() + " (" + Math.Round(Convert.ToDouble(FP1 * 100 / P1), 0) + "%)";
            }
            else
            {
                model.P2 = "0/0 (100%)";
            }

            if (P2 != 0)
            {
                model.P2 = FP2.ToString() + "/" + P2.ToString() + " (" + Math.Round(Convert.ToDouble(FP2 * 100 / P2), 0) + "%)";
            }
            else
            {
                model.P2 = "0/0 (100%)";
            }
            return model;
        }

        /// <summary>
        /// 获取一盘的发球得分率
        /// </summary>
        /// <param name="_Setsys"></param>
        /// <returns></returns>
        private TennisStats GetServeWonBySet(string _Setsys)
        {
            int P1 = 0;
            int FP1 = 0;
            int P2 = 0;
            int FP2 = 0;
            TennisStats model = new TennisStats();
            model.INDEX = "发球总得分率";

            SetModel smodel = SetDll.Get_Instance().GetModel(_Setsys);
            MatchModel mmodel = MatchDll.Get_Instance().GetModel(smodel.MATCHSYS);
            List<GameModel> GameList = GameDll.Get_Instance().GetModellist(smodel.SYS);
            foreach (GameModel gmodel in GameList)
            {
                if (gmodel.MSERVER == mmodel.PLAYER1)
                {
                    //统计P1的发球数
                    P1 += GetGameServe(gmodel.SYS);
                    //统计P1的发球得分数
                    FP1 += GetServeWon(gmodel.SYS, mmodel.PLAYER1);
                }
                else
                {
                    //统计P2的发球数
                    P2 += GetGameServe(gmodel.SYS);
                    //统计P2的发球得分数
                    FP2 += GetServeWon(gmodel.SYS, mmodel.PLAYER2);
                }
            }

            if (P1 != 0)
            {
                model.P1 = FP1.ToString() + "/" + P1.ToString() + " (" + Math.Round(Convert.ToDouble(FP1 * 100 / P1), 0) + "%)";
            }
            else
            {
                model.P2 = "0/0 (100%)";
            }

            if (P2 != 0)
            {
                model.P2 = FP2.ToString() + "/" + P2.ToString() + " (" + Math.Round(Convert.ToDouble(FP2 * 100 / P2), 0) + "%)";
            }
            else
            {
                model.P2 = "0/0 (100%)";
            }

            return model;
        }

        //------------------统计总得分率--------------------------//        
        /// <summary>
        /// 获取比赛的总得分率
        /// </summary>
        /// <param name="_Matchsys"></param>
        /// <returns></returns>
        private TennisStats GetPointsWonbyMatch(string _Matchsys)
        {
            int FP1 = 0;
            int FP2 = 0;
            TennisStats model = new TennisStats();
            model.INDEX = "总得分率";

            MatchModel mmodel = MatchDll.Get_Instance().GetModel(_Matchsys);
            List<SetModel> SetList = SetDll.Get_Instance().GetModelList(_Matchsys);
            foreach (SetModel smodel in SetList)
            {
                List<GameModel> GameList = GameDll.Get_Instance().GetModellist(smodel.SYS);
                foreach (GameModel gmodel in GameList)
                {
                    //统计P2的总得分数
                    FP2 += GetServeWon(gmodel.SYS, mmodel.PLAYER2);
                    //统计P1的总得分数
                    FP1 += GetServeWon(gmodel.SYS, mmodel.PLAYER1);
                }
            }
            model.P1 = FP1.ToString() + "/" + (FP1 + FP2).ToString() + " (" + Math.Round(Convert.ToDouble(FP1 * 100 / (FP1 + FP2)), 0) + "%)";
            model.P2 = FP2.ToString() + "/" + (FP1 + FP2).ToString() + " (" + Math.Round(Convert.ToDouble(FP2 * 100 / (FP1 + FP2)), 0) + "%)";
            return model;
        }

        /// <summary>
        /// 获取一盘的接球总得分率
        /// </summary>
        /// <param name="_Setsys"></param>
        /// <returns></returns>
        private TennisStats GetPointsWonBySet(string _Setsys)
        {
            int P1 = 0;
            int FP1 = 0;
            int P2 = 0;
            int FP2 = 0;
            TennisStats model = new TennisStats();
            model.INDEX = "总得分率";

            SetModel smodel = SetDll.Get_Instance().GetModel(_Setsys);
            MatchModel mmodel = MatchDll.Get_Instance().GetModel(smodel.MATCHSYS);
            List<GameModel> GameList = GameDll.Get_Instance().GetModellist(smodel.SYS);
            foreach (GameModel gmodel in GameList)
            {
                //统计P2的总得分数
                FP2 += GetServeWon(gmodel.SYS, mmodel.PLAYER2);
                //统计P1的总得分数
                FP1 += GetServeWon(gmodel.SYS, mmodel.PLAYER1);
            }
            model.P1 = FP1.ToString() + "/" + (FP1 + FP2).ToString() + " (" + Math.Round(Convert.ToDouble(FP1 * 100 / (FP1 + FP2)), 0) + "%)";
            model.P2 = FP2.ToString() + "/" + (FP1 + FP2).ToString() + " (" + Math.Round(Convert.ToDouble(FP2 * 100 / (FP1 + FP2)), 0) + "%)";
            return model;
        }

        //------------------统计接球得分率--------------------------//        
        /// <summary>
        /// 获取比赛的接球得分率
        /// </summary>
        /// <param name="_Matchsys"></param>
        /// <returns></returns>
        private TennisStats GetReturnWonbyMatch(string _Matchsys)
        {
            int P1 = 0;
            int FP1 = 0;
            int P2 = 0;
            int FP2 = 0;
            TennisStats model = new TennisStats();
            model.INDEX = "接球总得分率";

            MatchModel mmodel = MatchDll.Get_Instance().GetModel(_Matchsys);
            List<SetModel> SetList = SetDll.Get_Instance().GetModelList(_Matchsys);
            foreach (SetModel smodel in SetList)
            {
                List<GameModel> GameList = GameDll.Get_Instance().GetModellist(smodel.SYS);
                foreach (GameModel gmodel in GameList)
                {
                    if (gmodel.ISTIEBREAK == 0)
                    {
                        if (gmodel.MSERVER == mmodel.PLAYER1)
                        {
                            //统计P2的接发球数
                            P2 += GetGameServe(gmodel.SYS);
                            //统计P2的接发球得分数
                            FP2 += GetServeWon(gmodel.SYS, mmodel.PLAYER2);
                        }
                        else
                        {
                            //统计P1的接发球数
                            P1 += GetGameServe(gmodel.SYS);
                            //统计P2的接发球得分数
                            FP1 += GetServeWon(gmodel.SYS, mmodel.PLAYER1);
                        }
                    }
                    else
                    {
                        //统计抢七局
                        #region 统计抢七
                        List<PointsModel> listp = PointsDll.Get_Instance().GetModellist(gmodel.SYS);
                        if (listp.Count > 0)
                        {
                            foreach (PointsModel pmodel in listp)
                            {
                                if (pmodel.GORDER == 1)
                                {
                                    if (gmodel.MSERVER == mmodel.PLAYER1)
                                    {
                                        P2 += 1;
                                        if (pmodel.WINNER == mmodel.PLAYER2)
                                        {
                                            FP2 += 1;
                                        }
                                    }
                                    else
                                    {
                                        P1 += 1;
                                        if (pmodel.WINNER == mmodel.PLAYER1)
                                        {
                                            FP1 += 1;
                                        }
                                    }


                                }
                                else
                                {
                                    //not the first point
                                    int x = 0;
                                    if (pmodel.GORDER % 2 == 0)
                                    {
                                        x = pmodel.GORDER / 2;
                                    }
                                    else
                                    {
                                        x = (pmodel.GORDER - 1) / 2;
                                    }

                                    if (x % 2 == 0)
                                    {
                                        //发球方是本局标记的发球方
                                        if (gmodel.MSERVER == mmodel.PLAYER1)
                                        {
                                            P2 += 1;
                                            if (pmodel.WINNER == mmodel.PLAYER2)
                                            {
                                                FP2 += 1;
                                            }
                                        }
                                        else
                                        {
                                            P1 += 1;
                                            if (pmodel.WINNER == mmodel.PLAYER1)
                                            {
                                                FP1 += 1;
                                            }
                                        }


                                    }
                                    else
                                    {
                                        //发球方不是本局标记的发球方
                                        if (gmodel.MSERVER == mmodel.PLAYER1)
                                        {
                                            P1 += 1;
                                            if (pmodel.WINNER == mmodel.PLAYER1)
                                            {
                                                FP1 += 1;
                                            }
                                        }
                                        else
                                        {
                                            P2 += 1;
                                            if (pmodel.WINNER == mmodel.PLAYER2)
                                            {
                                                FP2 += 1;
                                            }
                                        }
                                    }
                                }
                            }
                        }
                        #endregion
                    }
                }
            }

            if (P1 != 0)
            {
                model.P1 = FP1.ToString() + "/" + P1.ToString() + " (" + Math.Round(Convert.ToDouble(FP1 * 100 / P1), 0) + "%)";
            }
            else
            {
                model.P2 = "0/0 (100%)";
            }

            if (P2 != 0)
            {
                model.P2 = FP2.ToString() + "/" + P2.ToString() + " (" + Math.Round(Convert.ToDouble(FP2 * 100 / P2), 0) + "%)";
            }
            else
            {
                model.P2 = "0/0 (100%)";
            }
            return model;
        }

        /// <summary>
        /// 获取一盘的接球总得分率
        /// </summary>
        /// <param name="_Setsys"></param>
        /// <returns></returns>
        private TennisStats GetReturnWonBySet(string _Setsys)
        {
            int P1 = 0;
            int FP1 = 0;
            int P2 = 0;
            int FP2 = 0;
            TennisStats model = new TennisStats();
            model.INDEX = "接球总得分率";

            SetModel smodel = SetDll.Get_Instance().GetModel(_Setsys);
            MatchModel mmodel = MatchDll.Get_Instance().GetModel(smodel.MATCHSYS);
            List<GameModel> GameList = GameDll.Get_Instance().GetModellist(smodel.SYS);
            foreach (GameModel gmodel in GameList)
            {
                if (gmodel.MSERVER == mmodel.PLAYER1)
                {
                    //统计P2的接球数
                    P2 += GetGameServe(gmodel.SYS);
                    //统计P2的接发球得分数
                    FP2 += GetServeWon(gmodel.SYS, mmodel.PLAYER2);
                }
                else
                {
                    //统计P1的接发球数
                    P1 += GetGameServe(gmodel.SYS);
                    //统计P2的接发球得分数
                    FP1 += GetServeWon(gmodel.SYS, mmodel.PLAYER1);
                }
            }

            if (P1 != 0)
            {
                model.P1 = FP1.ToString() + "/" + P1.ToString() + " (" + Math.Round(Convert.ToDouble(FP1 * 100 / P1), 0) + "%)";
            }
            else
            {
                model.P2 = "0/0 (100%)";
            }

            if (P2 != 0)
            {
                model.P2 = FP2.ToString() + "/" + P2.ToString() + " (" + Math.Round(Convert.ToDouble(FP2 * 100 / P2), 0) + "%)";
            }
            else
            {
                model.P2 = "0/0 (100%)";
            }

            return model;
        }
    }
}
