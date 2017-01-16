using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using WeTour;

namespace WeUmpire
{
    public class SetDll
    {
        public SetDll() { }
        private static SetDll _Instance;
        public static SetDll Get_Instance()
        {
            if (_Instance == null)
            {
                _Instance = new SetDll();
            }
            return _Instance;
        }

        /// <summary>
        /// get a model
        /// </summary>
        /// <param name="_Sys"></param>
        /// <returns></returns>
        public SetModel GetModel(string _Sys)
        {
            SetModel model = new SetModel();
            DataTable dt = SetDac.SelectList(string.Format(" and sys='{0}'",_Sys));
            if (dt.Rows.Count > 0)
            {
                model = JsonHelper.ParseDtInfo<SetModel>(dt);
            }
            return model;
        }

        /// <summary>
        /// 获取比赛正在进行的盘实体
        /// </summary>
        /// <param name="_Sys"></param>
        /// <returns></returns>
        public SetModel GetActiveModel(string _Sys)
        {
            SetModel model = new SetModel();
            DataTable dt = SetDac.SelectList(string.Format(" and matchsys='{0}' and state=1", _Sys));
            if (dt.Rows.Count > 0)
            {
                model = JsonHelper.ParseDtInfo<SetModel>(dt);
            }
            return model;
        }

        /// <summary>
        /// 根据比赛主键和盘顺序来获得盘实体
        /// </summary>
        /// <param name="_Matchsys"></param>
        /// <param name="_Sorder"></param>
        /// <returns></returns>
        public SetModel GetSingleModel(string _Matchsys, int _Sorder)
        {
            SetModel model = new SetModel();
            DataTable dt = SetDac.SelectList(string.Format(" and matchsys='{0}' and sorder='{1}'", _Matchsys, _Sorder));
            if (dt.Rows.Count > 0)
            {
                model = JsonHelper.ParseDtInfo<SetModel>(dt);
            }
            return model;
        }

        /// <summary>
        /// 根据比赛主键获得盘实体清单
        /// </summary>
        /// <param name="_Matchsys"></param>
        /// <returns></returns>
        public List<SetModel> GetModelList(string _Matchsys)
        {
            List<SetModel> ModelList = new List<SetModel>();
            DataTable dt = SetDac.SelectList(string.Format(" and matchsys='{0}'",_Matchsys));
            if (dt.Rows.Count > 0)
            {
                ModelList = JsonHelper.ParseDtModelList<List<SetModel>>(dt);
            }
            return ModelList;
        }

        /// <summary>
        /// add a new record
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public string Add(SetModel model)
        {
            string Setsys = "";
            model.SYS = Guid.NewGuid().ToString("N").ToUpper();
            if (SetDac.Insert(model))
            {
                Setsys = model.SYS;
            }
            return Setsys;
        }

        /// <summary>
        /// 修改盘状态
        /// </summary>
        /// <param name="_Sys"></param>
        /// <param name="_State"></param>
        /// <returns></returns>
        public bool UpdateState(string _Sys, string _Winner, int _State)
        {
            if (SetDac.UpdateState(_Sys, _Winner, _State))
            {
                return  true;
            }
            else
            {
                return  false;
            }
        }

        /// <summary>
        /// 根据比赛主键获得盘分，2016-1-5，liutao
        /// </summary>
        /// <param name="_Matchsys"></param>
        /// <returns></returns>
        public List<SetGameWon> GetSetScore(string _Matchsys,string _Player)
        {
            List<SetGameWon> list = new List<SetGameWon>();
            WeMatchModel mmodel = WeMatchDll.instance.GetModel(_Matchsys);            
            int _Set = 0;
            switch (mmodel.MATCHTYPE)
            { 
                case 0:
                    _Set = 1;//一盘比赛
                    break;
                case 1:
                    _Set = 3;//三盘比赛
                    break;
                case 2:
                    _Set = 5;//5盘比赛
                    break;
            }

            //获取获胜的盘分
            for (int i = 1; i <= _Set; i++)
            {
                SetGameWon model = new SetGameWon();
                model.SetOrder = i.ToString();
                SetModel smodel = GetSingleModel(_Matchsys, i);
                DataTable dt1 = GameDac.SelectList(string.Format(" and setsys='{0}' and winner='{1}'", smodel.SYS, _Player));
                model.WinGames = dt1.Rows.Count.ToString();
                list.Add(model);
            }
            return list;
        }

        /// <summary>
        /// 根据比赛主键获得盘数
        /// </summary>
        /// <param name="_Matchsys"></param>
        /// <returns></returns>
        public int SetNumByMatch(string _Matchsys)
        {
            int _SetNum = 0;
            DataTable dt = SetDac.SelectList(string.Format(" and matchsys='{0}'",_Matchsys));
            _SetNum = dt.Rows.Count;
            return _SetNum;
        }

        /// <summary>
        /// 根据比赛主键,盘顺序号，获得该盘的比分
        /// </summary>
        /// <param name="_MatchSys"></param>
        /// <param name="_Sorder"></param>
        /// <returns></returns>
        public string SetScore(string _MatchSys, int _Sorder)
        {
            string _SetScore = "";
            WeMatchModel mmodel = WeMatchDll.instance.GetModel(_MatchSys);
            SetModel smodel = GetSingleModel(_MatchSys, _Sorder);
            DataTable dt1 = GameDac.SelectList(string.Format(" and setsys='{0}' and winner='{1}'",smodel.SYS,mmodel.PLAYER1 ));
            DataTable dt2 = GameDac.SelectList(string.Format(" and setsys='{0}' and winner='{1}'", smodel.SYS, mmodel.PLAYER2));
                        
            _SetScore = dt1.Rows.Count.ToString()+dt2.Rows.Count.ToString();
            //判断是否抢七局
            if (IsSetHasTieBreak(smodel.SYS)!="")
            {
                _SetScore += IsSetHasTieBreak(smodel.SYS);
            }
            return _SetScore;
        }

        /// <summary>
        /// 判断这一盘是否包含抢七局
        /// </summary>
        /// <param name="_SetSys"></param>
        /// <returns></returns>
        public string IsSetHasTieBreak(string _SetSys)
        {
            string _TieScore = "";
            List<GameModel> gameList = GameDll.Get_Instance().GetModellist(_SetSys);
            SetModel smodel = SetDll.Get_Instance().GetModel(_SetSys);
            WeMatchModel mmodel = WeMatchDll.instance.GetModel(smodel.MATCHSYS);
            foreach (GameModel model in gameList)
            {
                if (model.ISTIEBREAK == 1)
                {
                    int p1 = 0;
                    string _Loser = "";
                    if (model.WINNER == mmodel.PLAYER1)
                    {
                        _Loser = mmodel.PLAYER2;
                    }
                    else
                    {
                        _Loser = mmodel.PLAYER1;
                    }

                    DataTable dt1 = PointsDac.SelectList(string.Format(" and gamesys='{0}' and winner='{1}'", model.SYS, _Loser));
                    p1 = dt1.Rows.Count;
                   

                    _TieScore = "(" + p1.ToString() + ")";
                }
            }
            return _TieScore;
        }

        /// <sumdmary>
        /// 根据盘主键来判断新的一局是否为抢七局
        /// </summary>
        /// <param name="_SetSys"></param>
        /// <returns></returns>
        public bool IsTieBreak(string _SetSys)
        {
            int p1 = 0;
            int p2 = 0;
            SetModel smodel = SetDll.Get_Instance().GetModel(_SetSys);
            WeMatchModel mmodel = WeMatchDll.instance.GetModel(smodel.MATCHSYS);
            DataTable dt1 = GameDac.SelectList(string.Format(" and setsys='{0}' and winner='{1}' and state=2",_SetSys,mmodel.PLAYER1));
            p1 = dt1.Rows.Count;
            DataTable dt2 = GameDac.SelectList(string.Format(" and setsys='{0}' and winner='{1}'  and state=2", _SetSys, mmodel.PLAYER2));
            p2 = dt2.Rows.Count;

            int GameQ = 6;
            if (!string.IsNullOrEmpty(mmodel.GameQty))
            {
                GameQ = Convert.ToInt32(mmodel.GameQty);//从比赛实体中获取比赛的盘数量，是6局比赛，还是4局比赛，还是8局比赛（华侨城决赛就是打8局）
            }
            if (p1 == GameQ && p2 == GameQ)
            {
                //新的一局为抢七局
                return true;
            }
            else
            {
                return false;
            }
        }

        /// <summary>
        /// 根据比赛主键和盘序号获得盘主键
        /// </summary>
        /// <param name="_Matchsys"></param>
        /// <param name="_Order"></param>
        /// <returns></returns>
        public string GetSetSysByOrder(string _Matchsys, int _Order)
        {
            SetModel model = new SetModel();
            DataTable dt = SetDac.SelectList(string.Format(" and matchsys='{0}' and sorder='{1}' and state=2",_Matchsys,_Order));
            if (dt.Rows.Count > 0)
            {
                model = JsonHelper.ParseDtInfo<SetModel>(dt);
            }
            return model.SYS;
        }

        #region Match Umpire
        /// <summary>
        /// 根据最新盘的获胜方，对比比赛的盘数量，判断比赛是否结束
        /// </summary>
        /// <returns></returns>
        public bool IsMatchOver(string _MatchSys, string _Winner)
        {
            WeMatchModel mmodel =WeMatchDll.instance.GetModel(_MatchSys);

            int _AsetNum = 0;
            DataTable dt = SetDac.SelectList(string.Format(" and matchsys='{0}' and state=2 and winner ='{1}'", _MatchSys, _Winner));
            _AsetNum = dt.Rows.Count;

            int _SetNum = 0;
            switch (mmodel.MATCHTYPE)
            {
                case 0:
                    _SetNum = 1;
                    break;
                case 1:
                    _SetNum = 2;
                    break;
                case 2:
                    _SetNum = 3;
                    break;
            }

            if (_SetNum == _AsetNum)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        /// <summary>
        /// 获取比赛比分
        /// </summary>
        /// <param name="_Matchsys"></param>
        /// <returns></returns>
        public string GetMatchScore(string _Matchsys)
        {
            string _MatchScore = "";
            WeMatchModel mmodel = WeMatchDll.instance.GetModel(_Matchsys);
            int _SetNum = 0;
            switch (mmodel.MATCHTYPE)
            {
                case 0:
                    _SetNum = 1;
                    break;
                case 1:
                    _SetNum = 3;
                    break;
                case 2:
                    _SetNum = 5;
                    break;
            }
            //获得比分
            for (int i = 1; i <= _SetNum; i++)
            {
                _MatchScore += SetScore(_Matchsys, i) + ",";
            }

            return _MatchScore.Substring(0, _MatchScore.Length - 1);
        }
        #endregion
    }
}
