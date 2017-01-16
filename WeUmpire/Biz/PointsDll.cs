using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using WeTour;
using SMS;
using Member;

namespace WeUmpire
{
    public class PointsDll
    {
        public PointsDll() { }
        private static PointsDll _Instance;
        public static PointsDll Get_Instance()
        {
            if (_Instance == null)
            {
                _Instance = new PointsDll();
            }
            return _Instance;
        }

        /// <summary>
        /// 获取实体
        /// </summary>
        /// <param name="_Sys"></param>
        /// <returns></returns>
        public PointsModel GetModel(string _Sys)
        {
            PointsModel model = new PointsModel();
            DataTable dt = PointsDac.SelectList(string.Format(" and sys='{0}'", _Sys));
            if (dt.Rows.Count > 0)
            {
                model = JsonHelper.ParseDtInfo<PointsModel>(dt);
            }
            return model;
        }

        /// <summary>
        /// 根据局主键获得局分
        /// </summary>
        /// <param name="_Gamesys"></param>
        /// <returns></returns>
        public List<PointsModel> GetModellist(string _Gamesys)
        {
            List<PointsModel> list = new List<PointsModel>();
            DataTable dt = PointsDac.SelectList(string.Format(" and gamesys='{0}'", _Gamesys));
            if (dt.Rows.Count > 0)
            {
                list = JsonHelper.ParseDtModelList<List<PointsModel>>(dt);
            }
            return list;
        }

        /// <summary>
        /// 添加记录
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public bool Insert(PointsModel model)
        {            
            model.SYS = Guid.NewGuid().ToString("N").ToUpper();
            if (PointsDac.Insert(model))
            {
               return true;
            }
            else
            {
                return false;
            }
        }
        
       /// <summary>
       /// 根据局主键和球员主键，获取球员在当局的分数
       /// </summary>
       /// <param name="_Gamesys"></param>
       /// <param name="_Winner"></param>
       /// <returns></returns>
        private int SelectScroebyWinner(string _Gamesys,string _Winner)
        {
            DataTable dt = PointsDac.SelectList(string.Format(" and gamesys='{0}' and winner='{1}'",_Gamesys,_Winner));
            return dt.Rows.Count;
        }

        /// <summary>
        /// 根据局主键获得当局总分
        /// </summary>
        /// <param name="_Gamesys"></param>
        /// <returns></returns>
        private int SelectTotalPoints(string _Gamesys)
        {
            DataTable dt = PointsDac.SelectList(string.Format(" and gamesys='{0}'",_Gamesys));
            return dt.Rows.Count;
        }

        /// <summary>
        /// 判断当局是否完成
        /// 根据配置表的数据来定
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        private bool IsGameEnd(PointsModel model)
        {
            int _WinnerScore = SelectScroebyWinner(model.GAMESYS, model.WINNER); //此分获胜的人局分
            int _TotalScore = SelectTotalPoints(model.GAMESYS);
            int _OtherScore = _TotalScore - _WinnerScore;//此分失败的人局分

            GameModel Gmodel = GameDll.Get_Instance().GetModel(model.GAMESYS);
            SetModel smodel = SetDll.Get_Instance().GetModel(Gmodel.SETSYS);
            WeMatchModel mmodel = WeMatchDll.instance.GetModel(smodel.MATCHSYS);
            if (Gmodel.ISTIEBREAK == 1)
            {
                //获胜方局分是否大于等于7分
                if (_WinnerScore >= 7)
                {
                    
                        //判断净胜分是否大于等于2分
                        if (_WinnerScore - _OtherScore >= 2)
                        {
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
            else
            {
                //获胜方局分是否大于等于4分
                if (_WinnerScore >= 4)
                {
                    if (mmodel.ISDECIDE == 1)
                    {
                        //金球制胜
                        //判断净胜分是否大于等于1分
                        if (_WinnerScore - _OtherScore >= 1)
                        {
                            return true;
                        }
                        else
                        {
                            return false;
                        }
                    }
                    else
                    {
                        //平局制胜
                        //判断净胜分是否大于等于2分
                        if (_WinnerScore - _OtherScore >= 2)
                        {
                            return true;
                        }
                        else
                        {
                            return false;
                        }
                    }
                }
                else
                {
                    return false;
                }
            }
            
        }

        /// <summary>
        /// 添加新的一分
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public void AddAnewRecord(UmpPointModel umpPtsModel)
        {
            PointsModel model = new PointsModel();
            
            //构造比分实体
            //获得比赛实体
            WeMatchModel mmodel = WeMatchDll.instance.RenderMatch(umpPtsModel.MatchSys);

            if (mmodel.STATE == 1)
            {
                //获取正在进行的盘
                SetModel smodel = SetDll.Get_Instance().GetActiveModel(umpPtsModel.MatchSys);

                //获取当盘正在进行的局
                GameModel gmodel = GameDll.Get_Instance().GetActiveModel(smodel.SYS);

                model.GAMESYS = gmodel.SYS;
                DataTable dt = PointsDac.SelectList(string.Format(" and gamesys='{0}'", model.GAMESYS));
                model.GORDER = dt.Rows.Count + 1;
                model.SERVETYPE = Convert.ToInt32(umpPtsModel.ServeType);
                if (umpPtsModel.WinType != "")
                {
                    model.WINTYPE = Convert.ToInt32(umpPtsModel.WinType);
                }
                //获取获胜方
                model.WINNER = umpPtsModel.Winner;
                string WinName = "";
                if (model.WINNER == mmodel.PLAYER1)
                {
                    WinName = mmodel.Player1ComName;
                }
                else
                {
                    WinName = mmodel.Player2ComName;
                }

                //Add Umpire tips
                WeTour.TrendDll.Get_Instance().InsertTrendComment(mmodel.SYS, "sys", WinName + "得分");

                //判断是否为破发点
                if (IsBreakPoint(gmodel.SYS))
                {
                    model.ISBREAKPOINT = 1;
                }
                else
                {
                    model.ISBREAKPOINT = 0;
                }

                Insert(model);

                IsDeuce(gmodel.SYS);//判断是否平分

                //判断下一分是否为破发点
                if (IsBreakPoint(gmodel.SYS))
                {

                    WeTour.TrendDll.Get_Instance().InsertTrendComment(mmodel.SYS, "sys", "破发点");

                }
                //判断下一分是否为局点
                if (IsGamePoint(gmodel.SYS))
                {
                    WeTour.TrendDll.Get_Instance().InsertTrendComment(mmodel.SYS, "sys", "局点");
                }

                //检查当前比分
                if (IsGameEnd(model))
                {
                    //Add Umpire tips
                    //whether change sides
                    int GameQty = GameDll.Get_Instance().GetGameQtybySet(smodel.SYS);
                    string _ChangeSide = gmodel.LeftsidePlayer;
                    if (GameQty % 2 == 1)
                    {
                        WeTour.TrendDll.Get_Instance().InsertTrendComment(mmodel.SYS, "sys", "本局比赛结束,交换场地,交换发球");
                        if (_ChangeSide == mmodel.PLAYER1)
                        {
                            _ChangeSide = mmodel.PLAYER2;
                        }
                        else
                        {
                            _ChangeSide = mmodel.PLAYER1;
                        }
                    }
                    else
                    {
                        WeTour.TrendDll.Get_Instance().InsertTrendComment(mmodel.SYS, "sys", "本局比赛结束,交换发球");
                    }

                    //this game is over
                    GameDll.Get_Instance().UpdateState(gmodel.SYS, model.WINNER, 2);
                    //add a new game                
                    GameDll.Get_Instance().AddAnotherGame(gmodel.SYS, _ChangeSide);
                }               
            }
        }
        
        /// <summary>
        /// 根据局主键判断是否是破发点
        /// </summary>
        /// <param name="_Gamesys"></param>
        /// <returns></returns>
        private bool IsBreakPoint(string _Gamesys)
        {
            GameModel gmodel = GameDll.Get_Instance().GetModel(_Gamesys);
            SetModel smodel = SetDll.Get_Instance().GetModel(gmodel.SETSYS);
            WeMatchModel mmodel = WeMatchDll.instance.GetModel(smodel.MATCHSYS);

            int _S = 0;//发球方分数
            int _R = 0;//接球方分数

            //获得发球方分数
            DataTable dt1 = PointsDac.SelectList(string.Format(" and gamesys='{0}' and winner='{1}'",_Gamesys,gmodel.MSERVER));
            _S = dt1.Rows.Count;

            string _Return = "";
            if (mmodel.PLAYER1 == gmodel.MSERVER)
            {
                _Return = mmodel.PLAYER2;
            }
            else
            {
                _Return = mmodel.PLAYER1;
            }

            DataTable dt2 = PointsDac.SelectList(string.Format(" and gamesys='{0}' and winner='{1}'",_Gamesys,_Return));
            _R = dt2.Rows.Count;

            if (_S < 3 && _R == 3)
            {
                return true;
            }
            else if (_S > 2 && _R - _S == 1)
            {
                return true;
            }
            else
            {
                return false;
            }

        }

        /// <summary>
        /// 根据局主键判断是否是破发点
        /// </summary>
        /// <param name="_Gamesys"></param>
        /// <returns></returns>
        private bool IsGamePoint(string _Gamesys)
        {
            GameModel gmodel = GameDll.Get_Instance().GetModel(_Gamesys);
            SetModel smodel = SetDll.Get_Instance().GetModel(gmodel.SETSYS);
            WeMatchModel mmodel = WeMatchDll.instance.GetModel(smodel.MATCHSYS);

            int _S = 0;//发球方分数
            int _R = 0;//接球方分数

            //获得发球方分数
            DataTable dt1 = PointsDac.SelectList(string.Format(" and gamesys='{0}' and winner='{1}'", _Gamesys, gmodel.MSERVER));
            _S = dt1.Rows.Count;

            string _Return = "";
            if (mmodel.PLAYER1 == gmodel.MSERVER)
            {
                _Return = mmodel.PLAYER2;
            }
            else
            {
                _Return = mmodel.PLAYER1;
            }

            DataTable dt2 = PointsDac.SelectList(string.Format(" and gamesys='{0}' and winner='{1}'", _Gamesys, _Return));
            _R = dt2.Rows.Count;

            if (_R < 3 && _S == 3)
            {
                return true;
            }
            else if (_R > 2 && _S - _R == 1)
            {
                return true;
            }
            else
            {
                return false;
            }

        }

        /// <summary>
        /// 根据局主键判断是否是破发点
        /// </summary>
        /// <param name="_Gamesys"></param>
        /// <returns></returns>
        private bool IsDeuce(string _Gamesys)
        {
            GameModel gmodel = GameDll.Get_Instance().GetModel(_Gamesys);
            SetModel smodel = SetDll.Get_Instance().GetModel(gmodel.SETSYS);
            WeMatchModel mmodel = WeMatchDll.instance.GetModel(smodel.MATCHSYS);

            int _S = 0;//发球方分数
            int _R = 0;//接球方分数

            //获得发球方分数
            DataTable dt1 = PointsDac.SelectList(string.Format(" and gamesys='{0}' and winner='{1}'", _Gamesys, gmodel.MSERVER));
            _S = dt1.Rows.Count;

            string _Return = "";
            if (mmodel.PLAYER1 == gmodel.MSERVER)
            {
                _Return = mmodel.PLAYER2;
            }
            else
            {
                _Return = mmodel.PLAYER1;
            }

            DataTable dt2 = PointsDac.SelectList(string.Format(" and gamesys='{0}' and winner='{1}'", _Gamesys, _Return));
            _R = dt2.Rows.Count;

            if (_S >= 3 && _R >= 3 && _S == _R)
            {
                //
                if (mmodel.ISDECIDE == 1)
                {
                    WeTour.TrendDll.Get_Instance().InsertTrendComment(mmodel.SYS, "sys", "请接发球方选择接发区");
                }

                return true;
            }
            else
            {
                return false;
            }
           
        }

        /// <summary>
        /// 撤销比分
        /// </summary>
        /// <param name="_json"></param>
        /// <returns></returns>
        public string CancelPoints(string _json)
        {
            string _CanResult = "";
            PointsModel model = new PointsModel();
            string[] _Points = _json.Split(',');

            //构造比分实体
            //获得比赛实体
            WeMatchModel mmodel = WeMatchDll.instance.GetModel(_Points[0]);

            if (mmodel.STATE == 1)
            {
                //获取正在进行的盘
                SetModel smodel = SetDll.Get_Instance().GetActiveModel(_Points[0]);

                //获取当盘正在进行的局
                GameModel gmodel = GameDll.Get_Instance().GetActiveModel(smodel.SYS);

                model.GAMESYS = gmodel.SYS;
                DataTable dt = PointsDac.SelectList(string.Format(" and gamesys='{0}'", model.GAMESYS));
                if (dt.Rows.Count > 0)
                {
                    if (PointsDac.DeletebyOrder(model.GAMESYS, dt.Rows.Count))
                    {
                        _CanResult = "撤销成功！";
                    }
                    else
                    {
                        _CanResult = "撤销失败！";
                    }
                }
                else
                {
                    _CanResult = "只能撤销本局的比分！";
                }                
            }
            else
            {
                _CanResult = "比赛已完成！";
            }
            return _CanResult;
        }
    }
}
