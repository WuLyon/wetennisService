using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;

namespace WeTour
{
    public class GameDll
    {
        public GameDll() { }
        private static GameDll _Instance;
        public static GameDll Get_Instance()
        {
            if (_Instance == null)
            {
                _Instance = new GameDll();
            }
            return _Instance;
        }

        /// <summary>
        /// 获取实体
        /// </summary>
        /// <param name="_Sys"></param>
        /// <returns></returns>
        public GameModel GetModel(string _Sys)
        {
            GameModel model = new GameModel();
            DataTable dt = GameDac.SelectList(string.Format(" and sys='{0}'", _Sys));
            if (dt.Rows.Count > 0)
            {
                model = JsonHelper.ParseDtInfo<GameModel>(dt);
            }
            return model;
        }

        /// <summary>
        /// 获取正在进行的局实体
        /// </summary>
        /// <param name="_Sys"></param>
        /// <returns></returns>
        public GameModel GetActiveModel(string _Sys)
        {
            GameModel model = new GameModel();
            DataTable dt = GameDac.SelectList(string.Format(" and setsys='{0}' and state=1", _Sys));
            if (dt.Rows.Count > 0)
            {
                model = JsonHelper.ParseDtInfo<GameModel>(dt);
            }
            return model;
        }

        /// <summary>
        /// 添加记录
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public MessageInfo Insert(GameModel model)
        {
            MessageInfo mess = new MessageInfo();
            model.SYS = Guid.NewGuid().ToString("N").ToUpper();
            model.STATE = 1;
            if (GameDac.Insert(model))
            {
                mess.IsSucceed = true;
                mess.Message = "保存成功！";
                mess.SysNo = model.SYS;
            }
            else
            {
                mess.IsSucceed = false;
                mess.Message = "保存不成功！";
            }
            return mess;
        }

        /// <summary>
        /// 修改局状态
        /// </summary>
        /// <param name="_Sys"></param>
        /// <param name="_State"></param>
        /// <returns></returns>
        public MessageInfo UpdateState(string _Sys, string _Winner, int _State)
        {
            MessageInfo mess = new MessageInfo();
            if (GameDac.UpdateState(_Sys, _Winner, _State))
            {
                mess.IsSucceed = true;
                mess.Message = "保存成功！";
            }
            else
            {
                mess.IsSucceed = false;
                mess.Message = "保存不成功！";
            }
            return mess;
        }

        /// <summary>
        /// 根据盘主键
        /// </summary>
        /// <param name="_SetSys"></param>
        /// <returns></returns>
        public List<GameModel> GetModellist(string _SetSys)
        {
            List<GameModel> Modellist = new List<GameModel>();
            DataTable dt = GameDac.SelectList(string.Format(" and setsys='{0}' order by morder", _SetSys));
            if (dt.Rows.Count > 0)
            {
                Modellist = JsonHelper.ParseDtModelList<List<GameModel>>(dt);
            }
            return Modellist;
        }

        /// <summary>
        /// 根据当前局，增加新局
        /// </summary>
        /// <param name="_Sys"></param>
        /// <returns></returns>
        public MessageInfo AddAnotherGame(string _Sys, string _ChangeSide)
        {
            MessageInfo mess = new MessageInfo();
            GameModel model = GetModel(_Sys);
            SetModel smodel = SetDll.Get_Instance().GetModel(model.SETSYS);
            MatchModel mmodel = MatchDll.Get_Instance().GetModel(smodel.MATCHSYS);

            if (IsSetOver(_Sys))
            {
                //add umpire tips
                TrendDll.Get_Instance().InsertTrendComment(smodel.MATCHSYS, "sys", "本盘比赛结束");

                //修改本盘状态
                mess = SetDll.Get_Instance().UpdateState(model.SETSYS, model.WINNER, 2);

                string _Server = "";

                //判断比赛是否结束
                if (MatchDll.Get_Instance().IsMatchOver(smodel.MATCHSYS, model.WINNER))
                {
                    //add umpire tips
                    TrendDll.Get_Instance().InsertTrendComment(smodel.MATCHSYS, "sys", "全场比赛结束");

                    //修改比赛状态
                    mmodel.WINNER = model.WINNER;
                    if (model.WINNER == mmodel.PLAYER1)
                    {
                        mmodel.LOSER = mmodel.PLAYER2;
                    }
                    else
                    {
                        mmodel.LOSER = mmodel.PLAYER1;
                    }
                    mmodel.SCORE = MatchDll.Get_Instance().GetMatchScore(mmodel.SYS);
                    mmodel.STATE = 2;
                    mess = MatchDll.Get_Instance().Update(mmodel);

                    //添加下一轮比赛或更新
                    //mess = MatchDll.Get_Instance().AddNextRoundMatch(mmodel);

                    //添加比赛积分
                    //MatchDll.Get_Instance().AddRankPoints(mmodel.SYS);

                    //2014年12月17日
                    //MatchDll.Get_Instance().UpdateRegularMatch(mmodel.SYS, mmodel.WINNER, mmodel.LOSER, mmodel.ROUND.ToString());
                }
                else
                {
                    //增加新盘                
                    if (model.WINNER == mmodel.PLAYER1)
                    {
                        _Server = mmodel.PLAYER2;
                    }
                    else
                    {
                        _Server = mmodel.PLAYER1;
                    }
                    ServerDecided(_Server, smodel.MATCHSYS);

                    //一盘结束，修改比分
                    mmodel.SCORE = MatchDll.Get_Instance().GetMatchScore(mmodel.SYS);
                    mess = MatchDll.Get_Instance().Update(mmodel);
                }
            }
            else
            {
                GameModel nmodel = new GameModel();
                nmodel.SETSYS = model.SETSYS;
                nmodel.MORDER = model.MORDER + 1;
                nmodel.MSERVER = GetNewServer(_Sys);//根据局主键获得新局的发球者
                nmodel.IsDecidePoint = mmodel.ISDECIDE;//给是否金球制胜赋值，2014/9/10，liutao
                nmodel.LeftsidePlayer = _ChangeSide;//添加左侧球员，2015/05/15，liutao

                //判断是否为抢七局
                if (SetDll.Get_Instance().IsTieBreak(model.SETSYS))
                {
                    nmodel.ISTIEBREAK = 1;
                }
                else
                {
                    nmodel.ISTIEBREAK = 0;
                }
                mess = Insert(nmodel);

                //一局结束修改比分
                mmodel.SCORE = MatchDll.Get_Instance().GetMatchScore(mmodel.SYS);
                mess = MatchDll.Get_Instance().Update(mmodel);

            }
            return mess;
        }

        /// <summary>
        /// 根据盘主键获得局数
        /// </summary>
        /// <param name="Setsys"></param>
        /// <returns></returns>
        public int GetGameQtybySet(string Setsys)
        {
            DataTable dt = GameDac.SelectList(string.Format(" and setsys='{0}'", Setsys));
            return dt.Rows.Count;
        }

        /// <summary>
        /// 判断一盘是否结束
        /// </summary>
        /// <param name="_GameSys"></param>
        /// <returns></returns>
        private bool IsSetOver(string _GameSys)
        {
            GameModel gmodel = GetModel(_GameSys);
            SetModel smodel = SetDll.Get_Instance().GetModel(gmodel.SETSYS);
            MatchModel mmodel = MatchDll.Get_Instance().GetModel(smodel.MATCHSYS);
            DataTable dt1 = GameDac.SelectList(string.Format(" and setsys='{0}' and winner='{1}'", gmodel.SETSYS, gmodel.WINNER));
            DataTable dt2 = GameDac.SelectList(string.Format(" and setsys='{0}' and winner<>'{1}'", gmodel.SETSYS, gmodel.WINNER));

            int P1 = dt1.Rows.Count;
            int P2 = dt2.Rows.Count;

            if (gmodel.ISTIEBREAK == 1)
            {
                return true;
            }
            else
            {
                int GameQty = 6;
                try
                {
                    if (!string.IsNullOrEmpty(mmodel.GameQty))
                    {
                        GameQty = Convert.ToInt32(mmodel.GameQty);
                    }
                }
                catch
                { }
                //判断盘是否结束
                if (P1 >= GameQty)
                {
                    if (P1 - P2 >= 2)
                    {
                        //本盘结束
                        return true;
                    }
                    else
                    {
                        return false;
                    }
                }
                else
                {
                    return false;
                }
            }
        }

        /// <summary>
        /// 获取新局的发球者
        /// </summary>
        /// <param name="_GameSys"></param>
        /// <returns></returns>
        private string GetNewServer(string _GameSys)
        {
            string _NewServer = "";
            GameModel gmodel = GetModel(_GameSys);
            SetModel smodel = SetDll.Get_Instance().GetModel(gmodel.SETSYS);
            MatchModel mmodel = MatchDll.Get_Instance().GetModel(smodel.MATCHSYS);
            if (gmodel.MSERVER == mmodel.PLAYER1)
            {
                _NewServer = mmodel.PLAYER2;
            }
            else
            {
                _NewServer = mmodel.PLAYER1;
            }
            return _NewServer;
        }

        /// <summary>
        /// 根据局主键，获取比赛双方
        /// </summary>
        /// <param name="_GameSys"></param>
        /// <returns></returns>
        public List<PlayerModel> GetTwoPlayerbyGameSys(string _GameSys)
        {
            List<PlayerModel> pList = new List<PlayerModel>();
            GameModel gmodel = GetModel(_GameSys);
            SetModel smodel = SetDll.Get_Instance().GetModel(gmodel.SETSYS);
            MatchModel mmodel = MatchDll.Get_Instance().GetModel(smodel.MATCHSYS);
            PlayerModel pmodel1 = PlayerDll.Get_Instance().GetModel(mmodel.PLAYER1);
            PlayerModel pmodel2 = PlayerDll.Get_Instance().GetModel(mmodel.PLAYER2);
            //增加player1
            pList.Add(pmodel1);
            pList.Add(pmodel2);

            return pList;
        }

        /// <summary>
        /// 确定发球方，增加一盘，增加一局
        /// </summary>
        /// <param name="_Server"></param>
        /// <returns></returns>
        public MessageInfo ServerDecided(string _Server, string _MatchSys)
        {
            MessageInfo mess = new MessageInfo();

            //增加新的一盘
            SetModel smodel = new SetModel();
            smodel.MATCHSYS = _MatchSys;
            smodel.SORDER = SetDll.Get_Instance().SetNumByMatch(_MatchSys) + 1;
            smodel.STATE = 1;
            mess = SetDll.Get_Instance().Add(smodel);
            if (mess.IsSucceed)
            {
                //增加新的一局
                GameModel gmodel = new GameModel();
                gmodel.MSERVER = _Server;
                gmodel.SETSYS = mess.SysNo;
                gmodel.MORDER = SetDll.Get_Instance().SetNumByMatch(_MatchSys) + 1;
                gmodel.STATE = 1;
                //判断新局是否是金球制胜
                MatchModel mmodel = MatchDll.Get_Instance().GetModel(smodel.MATCHSYS);
                gmodel.IsDecidePoint = mmodel.ISDECIDE;

                mess = Insert(gmodel);

            }
            return mess;
        }

        /// <summary>
        /// 确定发球方，增加一盘，增加一局
        /// </summary>
        /// <param name="_Server"></param>
        /// <returns></returns>
        public MessageInfo ServerDecidedNew(string _Server, string _MatchSys, string _LeftSidePlayer)
        {
            MessageInfo mess = new MessageInfo();

            MatchModel mmodel = MatchDll.Get_Instance().GetModel(_MatchSys);
            if (mmodel.STATE == 0)
            {
                //比赛未开始，增加新盘，增加新局
                //增加新的一盘
                SetModel smodel = new SetModel();
                smodel.MATCHSYS = _MatchSys;
                smodel.SORDER = 1;
                smodel.STATE = 1;
                mess = SetDll.Get_Instance().Add(smodel);
                if (mess.IsSucceed)
                {
                    //增加新的一局
                    GameModel gmodel = new GameModel();
                    gmodel.MSERVER = _Server;
                    gmodel.SETSYS = mess.SysNo;
                    gmodel.MORDER = 1;
                    gmodel.STATE = 1;
                    gmodel.LeftsidePlayer = _LeftSidePlayer;

                    mess = Insert(gmodel);

                    //修改比赛状态，由未开始变为进行中                                        
                    mess = MatchDll.Get_Instance().UpdateState(_MatchSys, 1);
                }
            }
            return mess;
        }

        /// <summary>
        /// 根据比赛主键获得局比分
        /// </summary>
        /// <param name="_Matchsys"></param>
        /// <returns></returns>
        public string GetGameScore(string _Matchsys)
        {
            string _GameScore = "";
            int p1 = 0;
            int p2 = 0;
            MessageInfo mess = new MessageInfo();

            //获得比赛实体
            MatchModel mmodel = MatchDll.Get_Instance().GetModel(_Matchsys);

            //获取正在进行的盘
            SetModel smodel = SetDll.Get_Instance().GetActiveModel(_Matchsys);

            //获取当盘正在进行的局
            GameModel gmodel = GetActiveModel(smodel.SYS);

            //获取局分
            DataTable dt = PointsDac.SelectList(string.Format(" and gamesys='{0}' and winner='{1}'", gmodel.SYS, mmodel.PLAYER1));
            p1 = dt.Rows.Count;

            DataTable dt1 = PointsDac.SelectList(string.Format(" and gamesys='{0}' and winner='{1}'", gmodel.SYS, mmodel.PLAYER2));
            p2 = dt1.Rows.Count;

            if (gmodel.ISTIEBREAK == 1)
            {
                _GameScore = p1.ToString() + "," + p2.ToString();
            }
            else
            {
                _GameScore = RenderDouble(p1, p2);
            }
            return _GameScore;
        }

        /// <summary>
        /// 修饰局比分
        /// </summary>
        /// <param name="_P1"></param>
        /// <param name="_P2"></param>
        /// <returns></returns>
        private string RenderDouble(int _P1, int _P2)
        {
            string _Points = "";
            if (_P1 <= 3 && _P2 <= 3)
            {
                _Points = RenderRegular(_P1) + "," + RenderRegular(_P2);
            }
            else if ((_P1 >= 3 && _P2 >= 3) && _P1 > _P2)
            {
                _Points = "AD,40";
            }
            else if ((_P1 >= 3 && _P2 >= 3) && _P1 < _P2)
            {
                _Points = "40,AD";
            }
            else if ((_P1 >= 3 && _P2 >= 3) && _P1 == _P2)
            {
                _Points = "40,40";
            }
            return _Points;
        }

        /// <summary>
        /// 修饰局分
        /// </summary>
        /// <param name="_Pts"></param>
        /// <returns></returns>
        private string RenderRegular(int _Pts)
        {
            string _Rpoint = "";
            switch (_Pts)
            {
                case 0:
                    _Rpoint = "0";
                    break;
                case 1:
                    _Rpoint = "15";
                    break;
                case 2:
                    _Rpoint = "30";
                    break;

                case 3:
                    _Rpoint = "40";
                    break;
            }
            return _Rpoint;
        }

        /// <summary>
        /// 根据比赛主键，获取当前发球人
        /// </summary>
        /// <param name="_Matchsys"></param>
        /// <returns></returns>
        public string GetGameServer(string _Matchsys)
        {
            string _Server = "";
            //获得比赛实体
            MatchModel mmodel = MatchDll.Get_Instance().GetModel(_Matchsys);

            if (mmodel.STATE != 2)
            {
                _Server = "A";
                //获取正在进行的盘
                SetModel smodel = SetDll.Get_Instance().GetActiveModel(_Matchsys);

                //获取当盘正在进行的局
                GameModel gmodel = GetActiveModel(smodel.SYS);

                if (gmodel.MSERVER == mmodel.PLAYER1)
                {
                    _Server += "P1";
                }
                else
                {
                    _Server += "P2";
                }
            }
            else  //比赛结束
            {
                _Server = "B";
                if (mmodel.WINNER == mmodel.PLAYER1)
                {
                    _Server += "P1";
                }
                else
                {
                    _Server += "P2";
                }
            }
            return _Server;
        }

        /// <summary>
        /// 获取左侧球员
        /// </summary>
        /// <param name="_matchsys"></param>
        /// <returns></returns>
        public string GetLeftSidePlayer(string _matchsys)
        {
            string LeftSide = "";
            //获取正在进行的盘
            SetModel smodel = SetDll.Get_Instance().GetActiveModel(_matchsys);

            //获取当盘正在进行的局
            GameModel gmodel = GetActiveModel(smodel.SYS);
            LeftSide = gmodel.LeftsidePlayer;
            return LeftSide;
        }

        /// <summary>
        /// 修改抢七局状态
        /// </summary>
        /// <param name="_GameSys"></param>
        /// <returns></returns>
        public MessageInfo UpdateIsTieBreak(string _GameSys)
        {
            MessageInfo mess = new MessageInfo();
            if (GameDac.UpdateIsTieBreak(_GameSys))
            {
                mess.IsSucceed = true;
                mess.Message = "保存成功！";
            }
            else
            {
                mess.IsSucceed = false;
                mess.Message = "保存不成功！";
            }
            return mess;
        }

        /// <summary>
        /// 根据盘主键，运动员主键获取在一盘内的获胜局数
        /// 2016-9-30
        /// </summary>
        /// <param name="_Setsys"></param>
        /// <param name="_memsys"></param>
        /// <returns></returns>
        public int GetGameWonQty(string _Setsys, string _memsys)
        {
            string sql = "select * from wtf_game where setsys='"+_Setsys+"' and winner='"+_memsys+"'";
            DataTable dt = DbHelperSQL.Query(sql).Tables[0];
            return dt.Rows.Count;
        }

    }
}
