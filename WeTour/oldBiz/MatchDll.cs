using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;

namespace WeTour
{
    public class MatchDll
    {
        public MatchDll() { }
        private static MatchDll _Instance;
        public static MatchDll Get_Instance()
        {
            if (_Instance == null)
            {
                _Instance = new MatchDll();
            }
            return _Instance;
        }

        /// <summary>
        /// 获取实体
        /// </summary>
        /// <param name="_Sys"></param>
        /// <returns></returns>
        public MatchModel GetModel(string _Sys)
        {
            MatchModel model = new MatchModel();
            DataTable dt = MatchDac.SelectList(string.Format(" and sys='{0}'", _Sys));
            if (dt.Rows.Count > 0)
            {
                model = JsonHelper.ParseDtInfo<MatchModel>(dt);
            }
            return model;
        }

        /// <summary>
        /// 根据内容主键和比赛顺序来获得实体
        /// </summary>
        /// <param name="ContentId"></param>
        /// <param name="_MatchOrder"></param>
        /// <returns></returns>
        public MatchModel GetModelbyMatchOrder(string ContentId, string _MatchOrder)
        {
            MatchModel model = new MatchModel();
            DataTable dt = MatchDac.SelectList(string.Format(" and ContentId='{0}' and matchorder='{1}'", ContentId, _MatchOrder));
            if (dt.Rows.Count > 0)
            {
                model = JsonHelper.ParseDtInfo<MatchModel>(dt);
            }
            return model;
        }

        /// <summary>
        /// 添加记录
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public MessageInfo Insert(MatchModel model)
        {
            MessageInfo mess = new MessageInfo();
            model.SYS = Guid.NewGuid().ToString("N").ToUpper();
            model.STATE = 0;

            if (MatchDac.Insert(model))
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
        /// 更新信息
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public MessageInfo Update(MatchModel model)
        {
            MessageInfo mess = new MessageInfo();
            if (MatchDac.Update(model))
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
        /// 根据状态来查询比赛
        /// </summary>
        /// <param name="_State"></param>
        /// <param name="_Player"></param>
        /// <returns></returns>
        public List<MatchModel> GetMatchReport(int _State, string _Player)
        {
            List<MatchModel> list = new List<MatchModel>();
            DataTable dt = MatchDac.SelectMatchReport(string.Format(" and state={0} and player1name+player2name like '%{1}%' order by matchdate desc", _State, _Player));
            if (dt.Rows.Count > 0)
            {
                list = JsonHelper.ParseDtModelList<List<MatchModel>>(dt);
            }
            return list;
        }

        /// <summary>
        /// 修改比赛状态
        /// </summary>
        /// <param name="_Sys"></param>
        /// <param name="_State"></param>
        /// <returns></returns>
        public MessageInfo UpdateState(string _Sys, int _State)
        {
            MessageInfo mess = new MessageInfo();
            if (MatchDac.UpdateState(_Sys, _State))
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

        public MessageInfo UpdateUmpire(string _Sys, string _Umpire)
        {
            MessageInfo mess = new MessageInfo();
            if (MatchDac.UpdateUmpire(_Sys, _Umpire))
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
        /// 根据比赛主键，获得比赛双方成员
        /// </summary>
        /// <param name="_MatchSys"></param>
        /// <returns></returns>
        public string GetPlayers(string _MatchSys)
        {
            string _Players = "";
            MatchModel model = MatchDll.Get_Instance().GetModel(_MatchSys);
            MemberModel p1 = MemberDll.Get_Instance().GetModel(model.PLAYER1);
            _Players = p1.NAME + "," + p1.SYSNO;
            MemberModel p2 = MemberDll.Get_Instance().GetModel(model.PLAYER2);
            _Players += "," + p2.NAME + "," + p2.SYSNO;
            return _Players;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="_Matchsys"></param>
        /// <returns></returns>
        public DataTable GetPlayersByMatchSys(string _MatchSys)
        {
            DataTable dt = new DataTable();

            dt.Columns.Add("Psys");
            dt.Columns.Add("Pname");

            MatchModel model = MatchDll.Get_Instance().GetModel(_MatchSys);
            PlayerModel p1 = PlayerDll.Get_Instance().GetModel(model.PLAYER1);
            PlayerModel p2 = PlayerDll.Get_Instance().GetModel(model.PLAYER2);

            dt.Rows.Add(p1.SYS, p1.PNAME);
            dt.Rows.Add(p2.SYS, p2.PNAME);

            return dt;

        }

        /// <summary>
        /// 判断比赛是否结束
        /// </summary>
        /// <returns></returns>
        public bool IsMatchOver(string _MatchSys, string _Winner)
        {
            MatchModel mmodel = GetModel(_MatchSys);

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
            MatchModel mmodel = MatchDll.Get_Instance().GetModel(_Matchsys);
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
                _MatchScore += SetDll.Get_Instance().SetScore(_Matchsys, i) + ",";
            }

            return _MatchScore.Substring(0, _MatchScore.Length - 1);
        }

        /// <summary>
        /// 根据比赛主键获取完成的盘数
        /// </summary>
        /// <param name="_MatchSys"></param>
        /// <returns></returns>
        public int GetFinishSetbySys(string _MatchSys)
        {
            DataTable dt = SetDac.SelectList(string.Format(" and matchsys='{0}' and state=2", _MatchSys));
            return dt.Rows.Count;
        }

        /// <summary>
        /// 根据比赛实体，添加下一轮的比赛
        /// 2014-12-10,引入小组赛制
        /// </summary>
        /// <param name="mmodel"></param>
        /// <returns></returns>
        //public MessageInfo AddNextRoundMatch(MatchModel mmodel)
        //{
        //    MessageInfo mess = new MessageInfo();
        //    int winSO = GetThisRoundSign(mmodel.WINNER, mmodel.ROUND.ToString(), mmodel.TOURSYS);//获得获胜方在本轮的签位
        //    TourModel tmodel = TourDll.Get_Instance().GetModel(mmodel.TOURSYS);
        //    if (IsTourOver(mmodel.ROUND.ToString(), tmodel.CAPACITY))
        //    {
        //        mess.IsSucceed = false;
        //        mess.Message = "已经是最后一轮";
        //    }
        //    else
        //    {
        //        //添加下一轮签位和比赛。。
        //        int NextSign = Convert.ToInt32(GetNextRoundSign(winSO));//根据本轮签位获得下一轮的签位
        //        int NextRound = Convert.ToInt32(mmodel.ROUND.ToString()) + 1;
        //        if (TourDll.Get_Instance().AddNewSign(NextSign, mmodel.WINNER, mmodel.TOURSYS, NextRound, mmodel.PLAYTYPE))//添加一个签位
        //        {
        //            //查看对应的比赛是否已经存在

        //            if (IsMatchExist(mmodel.TOURSYS, NextSign, NextRound.ToString(), mmodel.PLAYTYPE) != "")
        //            {
        //                //更新比赛球员
        //                int vsSign = Convert.ToInt32(IsMatchExist(mmodel.TOURSYS, NextSign, NextRound.ToString(), mmodel.PLAYTYPE));
        //                string vsPlayer = "";
        //                string sql1 = string.Format("select * from wtf_match where toursys='{0}' and round='{1}'", mmodel.TOURSYS, NextRound.ToString());

        //                vsPlayer = TourDll.Get_Instance().GetPlayerbySign(vsSign.ToString(), NextRound.ToString(), mmodel.TOURSYS, mmodel.ContentID);
        //                sql1 += "and Player1='" + vsPlayer + "'";

        //                DataTable dt = DbHelperSQL.Query(sql1).Tables[0];
        //                string matchsys = "";
        //                if (dt.Rows.Count > 0)
        //                {
        //                    matchsys = dt.Rows[0]["sys"].ToString();
        //                }
        //                string sql2 = "";
        //                if (NextSign % 2 == 0)
        //                {
        //                    sql2 = "update wtf_match set player2='" + mmodel.WINNER + "' where sys='" + matchsys + "'";
        //                }
        //                else
        //                {
        //                    sql2 = "update wtf_match set player1='" + mmodel.WINNER + "' where sys='" + matchsys + "'";
        //                }
        //                int a = DbHelperSQL.ExecuteSql(sql2);
        //                if (a > 0)
        //                {
        //                    mess.IsSucceed = true;
        //                    mess.Message = "更新比赛对手成功";
        //                }
        //                else
        //                {
        //                    mess.IsSucceed = false;
        //                    mess.Message = "更新比赛对手失败";
        //                }
        //            }
        //            else
        //            {
        //                //添加比赛
        //                MatchModel NewModel = new MatchModel();
        //                NewModel.TOURSYS = mmodel.TOURSYS;
        //                NewModel.ROUND = NextRound;
        //                NewModel.MATCHDATE = mmodel.MATCHDATE;
        //                NewModel.MATCHTYPE = mmodel.MATCHTYPE;
        //                NewModel.GRADETYPE = mmodel.GRADETYPE;
        //                NewModel.ISDECIDE = mmodel.ISDECIDE;
        //                NewModel.PLAYTYPE = mmodel.PLAYTYPE;
        //                NewModel.PREDITTIME = GetLatestPredictTime(mmodel.TOURSYS, mmodel.COURTID);
        //                NewModel.STATE = 0;
        //                NewModel.COURTID = mmodel.COURTID;
        //                NewModel.ContentID = mmodel.ContentID;

        //                if (NextSign % 2 == 0)
        //                {
        //                    NewModel.PLAYER2 = mmodel.WINNER;
        //                }
        //                else
        //                {
        //                    NewModel.PLAYER1 = mmodel.WINNER;
        //                }
        //                mess = Insert(NewModel);
        //            }
        //        }
        //    }
        //    return mess;
        //}

        /// <summary>
        /// 更新下一场比赛的球员
        /// 2015-3-5,修改下一场比赛球员
        /// </summary>
        /// <param name="_Matchsys"></param>
        /// <param name="_Player"></param>
        private void UpdateNextMatchPlayer(string _Matchsys, string _Player, string _Position)
        {
            if (_Matchsys != "")
            {
                MatchModel model = GetModel(_Matchsys);
                string sql = "";

                if (_Position == "1")
                {
                    sql = "Update wtf_match set player1='" + _Player + "'";
                }
                else
                {
                    sql = "Update wtf_match set player2='" + _Player + "'";
                }
                sql += " where sys='" + _Matchsys + "'";
                int a = DbHelperSQL.ExecuteSql(sql);
            }
        }
        /// <summary>
        /// 在比赛结束时调用此方法，根据比赛情况决定是否添加下一场比赛
        /// 2014-12-10,引入小组赛制，整合原有方法AddNextRoundMatch
        /// </summary>
        /// <param name="Matchsys"></param>
        /// <returns></returns>
        //public MessageInfo AddNextMatch(string Matchsys)
        //{
        //    MessageInfo mess = new MessageInfo();
        //    MatchModel mmodel = GetModel(Matchsys);
        //    if (mmodel.ROUND != 0)
        //    {
        //        //淘汰赛
        //        //比赛指定下一场去向更新比赛
        //        if (!string.IsNullOrEmpty(mmodel.winto))
        //        {
        //            MatchModel Nmodel = GetModelbyMatchOrder(mmodel.ContentID, mmodel.winto);
        //            UpdateNextMatchPlayer(Nmodel.SYS, mmodel.WINNER, mmodel.etc3);

        //            //为下一轮比赛添加赔率
        //            try
        //            {
        //                //判断比赛选手是否已齐全
        //                string sql = "select * from wtf_match where sys='" + Nmodel.SYS + "' and Player1 is not null and player2 is not null";
        //                DataTable dt = DbHelperSQL.Query(sql).Tables[0];
        //                if (dt.Rows.Count > 0)
        //                {
        //                    //betDll.Get_Instance().InsertNewBet(Nmodel.SYS, "胜负");
        //                }
        //            }
        //            catch
        //            {

        //            }
        //        }
        //        if (!string.IsNullOrEmpty(mmodel.loseto))
        //        {
        //            MatchModel Nmodel = GetModelbyMatchOrder(mmodel.ContentID, mmodel.loseto);
        //            UpdateNextMatchPlayer(Nmodel.SYS, mmodel.LOSER, mmodel.etc3);
        //        }
        //    }
        //    else
        //    {
        //        //小组赛
        //        //小组赛，根据比赛回推小组赛组别
        //        int GroupId = GetGroupId(mmodel.PLAYER1, mmodel.ContentID);
        //        //查看比赛是否已全部完成
        //        if (IsGroupMatchesEnd(mmodel.ContentID, GroupId))
        //        {
        //            //小组赛比赛已全部完成
        //            string Quolifier = "";
        //            //根据小组赛类型来进行判断
        //            TourContentModel tcmodel = TourContentDll.Get_Instance().GetModelbyId(mmodel.ContentID);
        //            string _Matchorder = GetGroupWinto(mmodel.ContentID, GroupId.ToString());
        //            switch (tcmodel.GroupType)
        //            {
        //                case "2":
        //                    //资格赛
        //                    //AddNextRoundMatch(mmodel);
        //                    break;
        //                case "3":
        //                    //三人制小组赛                            
        //                    //获取小组排名
        //                    DataTable dt = TourContentDll.Get_Instance().SortGroup(mmodel.ContentID, GroupId.ToString());
        //                    Quolifier = dt.Rows[0]["membersys"].ToString();//获取头名memberid

        //                    MatchModel Nmodel = GetModelbyMatchOrder(mmodel.ContentID, _Matchorder);
        //                    UpdateNextMatchPlayer(Nmodel.SYS, Quolifier, mmodel.etc3);
        //                    break;
        //                case "4":
        //                    //四人制小组赛
        //                    DataTable dt1 = TourContentDll.Get_Instance().SortGroup(mmodel.ContentID, GroupId.ToString());
        //                    if (_Matchorder.IndexOf(",") > 0)
        //                    {
        //                        //有两人晋级
        //                        string[] MatchOrders = _Matchorder.Split(',');
        //                        Quolifier = dt1.Rows[0]["membersys"].ToString();//获取头名memberid
        //                        string Quolifier2 = dt1.Rows[1]["membersys"].ToString();//获取头名memberid
        //                        //小组头名进入对应组号的比赛player1
        //                        MatchModel Nmodel1 = GetModelbyMatchOrder(mmodel.ContentID, MatchOrders[0]);
        //                        UpdateNextMatchPlayer(Nmodel1.SYS, Quolifier, mmodel.etc3);

        //                        //小组次名进入另外一场比赛
        //                        MatchModel Nmodel2 = GetModelbyMatchOrder(mmodel.ContentID, MatchOrders[1]);
        //                        UpdateNextMatchPlayer(Nmodel2.SYS, Quolifier, mmodel.etc3);
        //                    }
        //                    else
        //                    {
        //                        //只有小组头名晋级
        //                        Quolifier = dt1.Rows[0]["membersys"].ToString();//获取头名memberid

        //                        //小组头名进入对应组号的比赛player1
        //                        MatchModel Nmodel1 = GetModelbyMatchOrder(mmodel.ContentID, _Matchorder);
        //                        UpdateNextMatchPlayer(Nmodel1.SYS, Quolifier, mmodel.etc3);
        //                    }
        //                    break;

        //                case "5":
        //                    //五人制小组赛
        //                    DataTable dt2 = TourContentDll.Get_Instance().SortGroup(mmodel.ContentID, GroupId.ToString());
        //                    Quolifier = dt2.Rows[0]["membersys"].ToString();//获取头名memberid

        //                    //小组头名进入对应组号的比赛player1
        //                    MatchModel Nmodel5 = GetModelbyMatchOrder(mmodel.ContentID, _Matchorder);
        //                    UpdateNextMatchPlayer(Nmodel5.SYS, Quolifier, mmodel.etc3);
        //                    break;
        //            }

        //        }
        //    }
        //    return mess;
        //}

        /// <summary>
        /// 根据比赛内容号获得对应子项的小组赛比赛数量
        /// </summary>
        /// <param name="_ContentID"></param>
        /// <returns></returns>
        private int GetGroupMatchQty(string _ContentID)
        {
            string sql = "select * from wtf_match where ContentId='" + _ContentID + "' and Round=0";
            DataTable dt = DbHelperSQL.Query(sql).Tables[0];
            return dt.Rows.Count;
        }

        #region 判断小组比赛辅助方法
        /// <summary>
        /// 获得小组组别号
        /// 小组赛完成后判断是否增加下一场比赛
        /// </summary>
        /// <param name="_Membersys"></param>
        /// <param name="ContentID"></param>
        /// <returns></returns>
        private int GetGroupId(string _Membersys, string ContentID)
        {
            int GroupId = 1;
            string sql = "select GroupId from wtf_TourSign where ContentID='" + ContentID + "' and membersys='" + _Membersys + "'";
            DataTable dt = DbHelperSQL.Query(sql).Tables[0];
            if (dt.Rows.Count > 0)
            {
                GroupId = Convert.ToInt32(dt.Rows[0][0].ToString());
            }
            return GroupId;
        }

        /// <summary>
        /// 获得小组winto的比赛顺序号
        /// 2015.3.4,小组赛可能允许一名晋级，也可以能允许两名队员晋级
        /// </summary>
        /// <param name="_ContentId"></param>
        /// <param name="_GroupId"></param>
        /// <returns></returns>
        private string GetGroupWinto(string _ContentId, string _GroupId)
        {
            string _Winto = "";
            string sql = "select distinct(winto) from wtf_match where ContentID='" + _ContentId + "' and Round=0 and etc1='" + _GroupId + "'";
            DataTable dt = DbHelperSQL.Query(sql).Tables[0];
            if (dt.Rows.Count > 0)
            {
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    _Winto += "," + dt.Rows[i]["winto"].ToString();
                }
            }
            _Winto = _Winto.TrimStart(',');
            return _Winto;
        }

        /// <summary>
        /// 判断小组比赛是否已经全部结束
        /// </summary>
        /// <param name="_ContentID"></param>
        /// <param name="GroupId"></param>
        /// <returns></returns>
        private bool IsGroupMatchesEnd(string _ContentID, int GroupId)
        {
            int UnfinishedMatch = 0;
            string sql = "select * from wtf_TourSign where ContentID='" + _ContentID + "' and GroupId='" + GroupId + "'";
            DataTable dt = DbHelperSQL.Query(sql).Tables[0];
            if (dt.Rows.Count > 0)
            {
                //分别查看小组成员的比赛是否已经完成
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    string sql1 = "select * from wtf_match  where contentid='" + _ContentID + "' and (player1='" + dt.Rows[i]["membersys"].ToString() + "' or player2='" + dt.Rows[i]["membersys"].ToString() + "') and state in (0,1) and Round=0";
                    DataTable dt1 = DbHelperSQL.Query(sql1).Tables[0];
                    UnfinishedMatch += dt1.Rows.Count;
                }
            }
            if (UnfinishedMatch > 0)
            {
                return false;
            }
            else
            {
                return true;
            }
        }

        /// <summary>
        /// 添加4人组比赛签位到赛事第一轮
        /// </summary>
        /// <param name="_Qualifier"></param>
        /// <param name="GroupId"></param>
        //private string AddSignforGroup4(string _Qualifier, int GroupId, string _Toursys, string _ContentId)
        //{
        //    string SignOrder = "";
        //    if (GroupId % 2 == 0)
        //    {
        //        SignOrder = (GroupId * 2 - 1).ToString() + "," + (GroupId * 2 - 2).ToString();
        //        //偶数组别
        //        //添加头名
        //        TourDll.Get_Instance().AddNewSign((GroupId * 2 - 1), _Qualifier.Split(',')[0], _Toursys, 1, _ContentId);

        //        //添加第二名
        //        TourDll.Get_Instance().AddNewSign((GroupId * 2 - 2), _Qualifier.Split(',')[1], _Toursys, 1, _ContentId);
        //    }
        //    else
        //    {
        //        //奇数组别
        //        SignOrder = (GroupId * 2 - 1).ToString() + "," + (GroupId * 2 + 2).ToString();
        //        //添加头名
        //        TourDll.Get_Instance().AddNewSign((GroupId * 2 - 1), _Qualifier.Split(',')[0], _Toursys, 1, _ContentId);
        //        //添加第二名
        //        TourDll.Get_Instance().AddNewSign((GroupId * 2 + 2), _Qualifier.Split(',')[1], _Toursys, 1, _ContentId);
        //    }
        //    return SignOrder;
        //}
        #endregion

        /// <summary>
        /// 获得某一轮比赛球员的签位
        /// </summary>
        /// <param name="_Membersys"></param>
        /// <param name="Round"></param>
        /// <returns></returns>
        private int GetThisRoundSign(string _Membersys, string _Round, string _TourSys)
        {
            string sql = string.Format("select * from wtf_toursign where membersys='{0}' and toursys='{1}' and round='{2}'", _Membersys, _TourSys, _Round);
            DataTable dt = DbHelperSQL.Query(sql).Tables[0];
            int a = Convert.ToInt32(dt.Rows[0]["signorder"].ToString());
            return a;
        }

        /// <summary>
        /// 根据当前的轮数，判断是否结束
        /// 如增加赛事子项目配置表,这里的轮数就要进行修改
        /// </summary>
        /// <param name="Round"></param>
        /// <returns></returns>
        private bool IsTourOver(string Round, string Sign)
        {
            //double a = Math.Sqrt(Convert.ToDouble(Sign));//根据赛事的签数，计算比赛的轮数。
            string a = TotalRound(Sign);
            if (Round == a)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        /// <summary>
        /// 根据赛事签位数，来计算总共的比赛轮数
        /// </summary>
        /// <param name="Sign"></param>
        /// <returns></returns>
        private string TotalRound(string Sign)
        {
            string _TR = "";
            switch (Sign)
            {
                case "8":
                    _TR = "3";
                    break;
                case "16":
                    _TR = "4";
                    break;
                case "32":
                    _TR = "5";
                    break;
                case "64":
                    _TR = "6";
                    break;
                case "128":
                    _TR = "7";
                    break;
            }
            return _TR;
        }



        /// <summary>
        /// 获得下一轮的签位
        /// </summary>
        /// <param name="_SignOrder"></param>
        /// <returns></returns>
        private string GetNextRoundSign(int _SignOrder)
        {
            decimal _Order = Convert.ToDecimal(_SignOrder);
            double _sign = Convert.ToDouble(_Order / 2);
            string a = Math.Ceiling(_sign).ToString();
            return a;
        }

        /// <summary>
        /// 根据下一轮签位查看比赛是否存在，存在，返回签位，不存在，则返回空值
        /// </summary>
        /// <param name="_TourSys"></param>
        /// <param name="Sign"></param>
        /// <returns></returns>
        private string IsMatchExist(string _TourSys, int Sign, string _Round, string _Type)
        {
            string OperSign = "";
            //根据下一轮组成一场比赛的签位是否已经填加即可
            if (Sign % 2 == 0)
            {
                //签位是偶数签，查看小一号签是否存在
                if (IsSignExist(_TourSys, Sign - 1, _Round, _Type))
                {
                    OperSign = (Sign - 1).ToString();
                }
            }
            else
            {
                //签位是奇数签，查看大一号签是否存在
                if (IsSignExist(_TourSys, Sign + 1, _Round, _Type))
                {
                    OperSign = (Sign + 1).ToString();
                }
            }
            return OperSign;
        }

        /// <summary>
        /// 判断sign是否存在
        /// </summary>
        /// <param name="_TourSys"></param>
        /// <param name="Sign"></param>
        /// <param name="_Round"></param>
        /// <returns></returns>
        private bool IsSignExist(string _TourSys, int Sign, string _Round, string _Type)
        {
            string sql = string.Format("select * from wtf_toursign where signorder='{0}' and toursys='{1}' and round='{2}' and ContentId='{3}'", Sign, _TourSys, _Round, _Type);
            DataTable dt = DbHelperSQL.Query(sql).Tables[0];
            if (dt.Rows.Count > 0)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        /// <summary>
        /// 获得最新的预计开始时间
        /// </summary>
        /// <param name="_TourSys"></param>
        /// <param name="_CourtID"></param>
        /// <returns></returns>
        public string GetLatestPredictTime(string _TourSys, string _CourtID)
        {
            string NewTime = "12:01";
            string sql = "select max(PreditTime) from wtf_match where toursys='" + _TourSys + "' and state=0 and courtid='" + _CourtID + "'";
            DataTable dt = DbHelperSQL.Query(sql).Tables[0];
            try
            {
                if (dt.Rows.Count > 0)
                {
                    //有未开始的比赛，加在所有比赛最后
                    DateTime _Last = Convert.ToDateTime(dt.Rows[0][0].ToString());
                    NewTime = _Last.AddHours(0.5).TimeOfDay.ToString();
                }
                else
                {
                    //查看有没有正在进行的比赛
                    string sql1 = "select max(PreditTime) from wtf_match where toursys='" + _TourSys + "' and state=1 and courtid='" + _CourtID + "'";
                    DataTable dt1 = DbHelperSQL.Query(sql).Tables[0];
                    if (dt1.Rows.Count > 0)
                    {
                        //有未开始的比赛，加在所有比赛最后
                        DateTime _Last = Convert.ToDateTime(dt.Rows[0][0].ToString());
                        NewTime = _Last.AddHours(0.5).TimeOfDay.ToString();
                    }
                }
            }
            catch
            {
            }
            return NewTime;
        }

        /// <summary>
        /// 增加比赛积分
        /// </summary>
        /// <param name="_MatchSys"></param>
        //public void AddRankPoints(string _MatchSys)
        //{
        //    MatchModel mmodel = MatchDll.Get_Instance().GetModel(_MatchSys);
        //    int _Round = Convert.ToInt32(mmodel.ROUND);//
        //    TourModel tmodel = TourDll.Get_Instance().GetModel(mmodel.TOURSYS);
        //    double _Cap = Convert.ToDouble(tmodel.CAPACITY);//
        //    int TotalRound = Convert.ToInt32(Math.Log(_Cap, 2));


        //    if (_Round == TotalRound)
        //    {
        //        //is the last round match
        //        if (mmodel.PLAYTYPE.IndexOf("双") > 0)
        //        {
        //            string[] _Player = mmodel.LOSER.Split('/');
        //            RankingDll.Get_Instance().AddRankPoint(GetRunerupbyCap(tmodel.CAPACITY).ToString(), "双打", _Player[0], mmodel.SYS, mmodel.ContentID);
        //            RankingDll.Get_Instance().AddRankPoint(GetRunerupbyCap(tmodel.CAPACITY).ToString(), "双打", _Player[1], mmodel.SYS, mmodel.ContentID);

        //            string[] _Winner = mmodel.WINNER.Split('/');
        //            RankingDll.Get_Instance().AddRankPoint(GetWinnerbyCap(tmodel.CAPACITY).ToString(), "双打", _Winner[0], mmodel.SYS, mmodel.ContentID);
        //            RankingDll.Get_Instance().AddRankPoint(GetWinnerbyCap(tmodel.CAPACITY).ToString(), "双打", _Winner[1], mmodel.SYS, mmodel.ContentID);
        //        }
        //        else
        //        {
        //            //
        //            RankingDll.Get_Instance().AddRankPoint(GetWinnerbyCap(tmodel.CAPACITY).ToString(), "单打", mmodel.WINNER, mmodel.SYS, mmodel.ContentID);
        //            RankingDll.Get_Instance().AddRankPoint(GetRunerupbyCap(tmodel.CAPACITY).ToString(), "单打", mmodel.LOSER, mmodel.SYS, mmodel.ContentID);
        //        }
        //    }
        //    else
        //    {
        //        //before final round
        //        if (mmodel.PLAYTYPE.IndexOf("双") > 0)
        //        {
        //            string[] _Player = mmodel.LOSER.Split('/');
        //            RankingDll.Get_Instance().AddRankPoint(GetPointByRoundCap(_Round).ToString(), "双打", _Player[0], mmodel.SYS, mmodel.ContentID);
        //            RankingDll.Get_Instance().AddRankPoint(GetPointByRoundCap(_Round).ToString(), "双打", _Player[1], mmodel.SYS, mmodel.ContentID);
        //        }
        //        else
        //        {
        //            //single match
        //            string Point = GetPointByRoundCap(_Round).ToString();
        //            RankingDll.Get_Instance().AddRankPoint(Point, "单打", mmodel.LOSER, mmodel.SYS, mmodel.ContentID);
        //        }
        //    }
        //}

        /// <summary>
        /// get Points by Round and Capacity
        /// 计算比赛积分
        /// </summary>
        /// <param name="_Round"></param>
        /// <param name="_Cap"></param>
        /// <returns></returns>
        private int GetPointByRoundCap(int _Round)
        {
            int _Score = 0;
            switch (_Round)
            {
                case 1:
                    _Score = 50;
                    break;
                case 2:
                    _Score = 100;
                    break;
                case 3:
                    _Score = 200;
                    break;
                case 4:
                    _Score = 300;
                    break;
            }
            return _Score;
        }

        /// <summary>
        /// Compute Winner Point
        /// 计算获胜者积分
        /// </summary>
        /// <param name="_Cap"></param>
        /// <returns></returns>
        private int GetWinnerbyCap(string _Cap)
        {
            int _Point = 0;
            switch (_Cap)
            {
                case "8":
                    _Point = 200;
                    break;
                case "16":
                    _Point = 500;
                    break;
                case "32":
                    _Point = 1000;
                    break;
            }
            return _Point;
        }

        /// <summary>
        /// Compute runner Up points
        /// 计算第二名积分
        /// </summary>
        /// <param name="_Cap"></param>
        /// <returns></returns>
        private int GetRunerupbyCap(string _Cap)
        {
            int _Point = 0;
            switch (_Cap)
            {
                case "8":
                    _Point = 150;
                    break;
                case "16":
                    _Point = 300;
                    break;
                case "32":
                    _Point = 500;
                    break;
            }
            return _Point;
        }


        #region 小组赛RoundRobin
        /// <summary>
        /// 获得球员胜场
        /// </summary>
        /// <param name="ContentId"></param>
        /// <param name="GroupId"></param>
        /// <param name="Membersys"></param>
        /// <returns></returns>
        public int GetGroupMatchWin(string ContentId, string GroupId, string Membersys)
        {
            string sql = "select * from wtf_match where ContentId='" + ContentId + "' and Round=0 and winner='" + Membersys + "'";
            DataTable dt = DbHelperSQL.Query(sql).Tables[0];
            return dt.Rows.Count;
        }

        /// <summary>
        /// 获得球员负场
        /// </summary>
        /// <param name="ContentId"></param>
        /// <param name="GroupId"></param>
        /// <param name="Membersys"></param>
        /// <returns></returns>
        public int GetGroupMatchloss(string ContentId, string GroupId, string Membersys)
        {
            string sql = "select * from wtf_match where ContentId='" + ContentId + "' and Round=0 and loser='" + Membersys + "'";
            DataTable dt = DbHelperSQL.Query(sql).Tables[0];
            return dt.Rows.Count;
        }

        /// <summary>
        /// 获得胜局
        /// </summary>
        /// <param name="ContentId"></param>
        /// <param name="GroupId"></param>
        /// <param name="Membersys"></param>
        /// <returns></returns>
        public int GetGroupGameWin(string ContentId, string GroupId, string Membersys)
        {
            int GameWin = 0;
            string sql = "select * from wtf_match where ContentId='" + ContentId + "' and Round=0 and winner='" + Membersys + "'";
            DataTable dt = DbHelperSQL.Query(sql).Tables[0];
            if (dt.Rows.Count > 0)
            {
                foreach (DataRow dr in dt.Rows)
                {
                    if (Membersys == dr["player1"].ToString())
                    {
                        GameWin += Convert.ToInt32(dr["score"].ToString().Substring(0, 1));
                    }
                    else
                    {
                        GameWin += Convert.ToInt32(dr["score"].ToString().Substring(1, 1));
                    }
                }
            }

            string sql1 = "select * from wtf_match where ContentId='" + ContentId + "' and Round=0 and loser='" + Membersys + "'";
            DataTable dt1 = DbHelperSQL.Query(sql1).Tables[0];
            if (dt1.Rows.Count > 0)
            {
                foreach (DataRow dr in dt1.Rows)
                {
                    if (Membersys == dr["player1"].ToString())
                    {
                        GameWin += Convert.ToInt32(dr["score"].ToString().Substring(0, 1));
                    }
                    else
                    {
                        GameWin += Convert.ToInt32(dr["score"].ToString().Substring(1, 1));
                    }
                }
            }
            return GameWin;
        }

        /// <summary>
        /// 获得负局
        /// </summary>
        /// <param name="ContentId"></param>
        /// <param name="GroupId"></param>
        /// <param name="Membersys"></param>
        /// <returns></returns>
        public int GetGroupGameLose(string ContentId, string GroupId, string Membersys)
        {
            int GameLose = 0;
            string sql = "select * from wtf_match where ContentId='" + ContentId + "' and Round=0 and winner='" + Membersys + "'";
            DataTable dt = DbHelperSQL.Query(sql).Tables[0];
            if (dt.Rows.Count > 0)
            {
                foreach (DataRow dr in dt.Rows)
                {
                    if (Membersys == dr["player1"].ToString())
                    {
                        GameLose += Convert.ToInt32(dr["score"].ToString().Substring(1, 1));
                    }
                    else
                    {
                        GameLose += Convert.ToInt32(dr["score"].ToString().Substring(0, 1));
                    }
                }
            }

            string sql1 = "select * from wtf_match where ContentId='" + ContentId + "' and Round=0 and loser='" + Membersys + "'";
            DataTable dt1 = DbHelperSQL.Query(sql1).Tables[0];
            if (dt1.Rows.Count > 0)
            {
                foreach (DataRow dr in dt1.Rows)
                {
                    if (Membersys == dr["player1"].ToString())
                    {
                        GameLose += Convert.ToInt32(dr["score"].ToString().Substring(1, 1));
                    }
                    else
                    {
                        GameLose += Convert.ToInt32(dr["score"].ToString().Substring(0, 1));
                    }
                }
            }
            return GameLose;
        }
        #endregion

        /// <summary>
        /// 维护球员退赛
        /// </summary>
        /// <param name="_matchSys"></param>
        /// <param name="WithPlayer"></param>
        /// <returns></returns>
        //public bool UpdateWithDrow(string _matchSys, string WithPlayer, string State)
        //{
        //    MatchModel Model = GetModel(_matchSys);
        //    string score = "";
        //    string winner = "";
        //    string loser = "";
        //    //添加比赛积分
        //    TourContentModel tcmode = TourContentDll.Get_Instance().GetModelbyId(Model.ContentID);
        //    string Type = "";
        //    if (tcmode.ContentType.IndexOf('双') > 0)
        //    {
        //        Type = "双打";
        //    }
        //    else
        //    {
        //        Type = "单打";
        //    }

        //    if (WithPlayer != "-1")
        //    {
        //        //单人退赛
        //        if (Model.PLAYER1 == WithPlayer)
        //        {
        //            //球员1退赛
        //            score = "06";
        //            winner = Model.PLAYER2;
        //            loser = Model.PLAYER1;
        //        }
        //        else
        //        {
        //            //球员2退赛
        //            score = "06";
        //            winner = Model.PLAYER1;
        //            loser = Model.PLAYER2;
        //        }

        //        ////获得比赛积分,根据比赛的进程，获得相应的比赛加分。
        //        //int RankScore = RankingDll.Get_Instance().GetMatchPoint(_matchSys, "Win");//赢得一场小组赛的积分
        //        //RankingDll.Get_Instance().AddRankPoint(RankScore.ToString(), Type, winner, "对方退赛，获得比赛积分",Model.ContentID);
        //        //if (State == "1")
        //        //{
        //        //    //扣除无故退赛球员的积分
        //        //    int WithDrawScore = RankingDll.Get_Instance().GetMatchPoint(_matchSys, "Withdraw");//赢得一场小组赛的积分
        //        //    RankingDll.Get_Instance().AddRankPoint(WithDrawScore.ToString(), Type, loser, "退赛，扣除惩罚积分",Model.ContentID);
        //        //}

        //        //Generate Another Match
        //    }
        //    else
        //    {
        //        ////修改比赛状态
        //        score = "00";

        //        ////扣除无故退赛球员的积分
        //        //int WithDrawScore = RankingDll.Get_Instance().GetMatchPoint(_matchSys, "Withdraw");//赢得一场小组赛的积分
        //        //RankingDll.Get_Instance().AddRankPoint(WithDrawScore.ToString(), Type, Model.PLAYER1, "退赛，扣除惩罚积分", Model.ContentID);
        //        //RankingDll.Get_Instance().AddRankPoint(WithDrawScore.ToString(), Type, Model.PLAYER2, "退赛，扣除惩罚积分", Model.ContentID);
        //    }

        //    //修改比赛状态
        //    string sql = string.Format("update wtf_match set state=2, winner='{0}',loser='{1}',score='{2}',IsWithDraw='{3}',matchtime='{5}' where sys='{4}'", winner, loser, score, "1", _matchSys, DateTime.Now.ToString());
        //    int a = DbHelperSQL.ExecuteSql(sql);
        //    if (a > 0)
        //    {
        //        //添加下一轮比赛
        //        AddNextMatch(_matchSys);
        //        betDll.Get_Instance().HandleMatchbet(_matchSys);
        //        return true;
        //    }
        //    else
        //    {
        //        return false;
        //    }

        //}

        /// <summary>
        /// 常规比赛结束后的操作，积分和增加以下场比赛,纪录比赛日期
        /// 2014-12-16，liutao
        /// 删除比赛积分的添加，改到在整个比赛结束后来修改积分。2015-05-11，liutao
        /// </summary>
        /// <param name="_MatchSys"></param>
        /// <param name="_Winer"></param>
        /// <param name="_Loser"></param>
        /// <param name="_Round"></param>
        //public void UpdateRegularMatch(string _MatchSys, string _Winer, string _Loser, string _Round)
        //{
        //    //添加下一轮比赛
        //    AddNextMatch(_MatchSys);

        //    //结算投注
        //    betDll.Get_Instance().HandleMatchbet(_MatchSys);
        //}

        /// <summary>
        /// 根据组别id获得该组别的所有比赛
        /// </summary>
        /// <param name="_Contid"></param>
        /// <returns></returns>
        public List<MatchModel> GetMatchListbyCont(string _Contid)
        {
            List<MatchModel> list = new List<MatchModel>();
            string sql = "select * from wtf_match where contentid='" + _Contid + "'";
            DataTable dt = DbHelperSQL.Query(sql).Tables[0];
            if (dt.Rows.Count > 0)
            {
                list = JsonHelper.ParseDtModelList<List<MatchModel>>(dt);
            }
            return list;
        }


        #region 团体赛
        /// <summary>
        /// 获得小组的比赛场次
        /// </summary>
        /// <param name="_Teamid"></param>
        /// <param name="_Contentid"></param>
        /// <returns></returns>
        public string GetGroupMatchQty(string _Teamid, string _Contentid)
        {
            string sql = "select * from wtf_groupmatch where contentid='" + _Contentid + "' and (Team1='" + _Teamid + "' or Team2='" + _Teamid + "') and status=2 and Round=0";
            DataTable dt = DbHelperSQL.Query(sql).Tables[0];
            return dt.Rows.Count.ToString();
        }

        /// <summary>
        /// 获得团体赛获胜场次
        /// </summary>
        /// <param name="_Teamid"></param>
        /// <param name="_Contentid"></param>
        /// <returns></returns>
        public int GetGroupMatchWin(string _Teamid, string _Contentid)
        {
            string sql = "select * from wtf_groupmatch where contentid='" + _Contentid + "' and winner='" + _Teamid + "' and status=2 and Round=0";
            DataTable dt = DbHelperSQL.Query(sql).Tables[0];
            return dt.Rows.Count;
        }

        /// <summary>
        /// 获得团体赛失利场次
        /// </summary>
        /// <param name="_Teamid"></param>
        /// <param name="_Contentid"></param>
        /// <returns></returns>
        public int GetGroupMatchLose(string _Teamid, string _Contentid)
        {
            string sql = "select * from wtf_groupmatch where contentid='" + _Contentid + "' and loser='" + _Teamid + "' and status=2 and Round=0";
            DataTable dt = DbHelperSQL.Query(sql).Tables[0];
            return dt.Rows.Count;
        }

        /// <summary>
        /// 获得小组在比分上的胜利
        /// </summary>
        /// <param name="_Teamid"></param>
        /// <param name="_Contentid"></param>
        /// <returns></returns>
        public int GetGroupMatchWinscore(string _Teamid, string _Contentid)
        {
            int GameWin = 0;
            string sql = "select * from wtf_groupmatch where contentid='" + _Contentid + "' and (team1='" + _Teamid + "' or team2='" + _Teamid + "') and status=2 and Round=0";
            DataTable dt = DbHelperSQL.Query(sql).Tables[0];
            if (dt.Rows.Count > 0)
            {
                foreach (DataRow dr in dt.Rows)
                {
                    string _Gamewin = "";
                    if (dr["Team1"].ToString() == _Teamid)
                    {
                        _Gamewin = dr["score"].ToString().Substring(0, 1);
                    }
                    else
                    {
                        _Gamewin = dr["score"].ToString().Substring(1, 1);
                    }
                    GameWin += Convert.ToInt32(_Gamewin);
                }
            }
            return GameWin;
        }

        /// <summary>
        /// 获得小组在比分上的胜利
        /// </summary>
        /// <param name="_Teamid"></param>
        /// <param name="_Contentid"></param>
        /// <returns></returns>
        public int GetGroupMatchLosescore(string _Teamid, string _Contentid)
        {
            int GameLose = 0;
            string sql = "select * from wtf_groupmatch where contentid='" + _Contentid + "' and (team1='" + _Teamid + "' or team2='" + _Teamid + "') and status=2 and Round=0";
            DataTable dt = DbHelperSQL.Query(sql).Tables[0];
            if (dt.Rows.Count > 0)
            {
                foreach (DataRow dr in dt.Rows)
                {
                    string _Gamewin = "";
                    if (dr["Team1"].ToString() == _Teamid)
                    {
                        _Gamewin = dr["score"].ToString().Substring(1, 1);
                    }
                    else
                    {
                        _Gamewin = dr["score"].ToString().Substring(0, 1);
                    }
                    GameLose += Convert.ToInt32(_Gamewin);
                }
            }
            return GameLose;
        }

        /// <summary>
        /// 根据个人比赛，修改团体赛的积分
        /// </summary>
        /// <param name="model"></param>
        //public void UpdateGroupMatch(MatchModel model)
        //{
        //    //获得团体赛实体
        //    MatchModel mmodel = TourDll.Get_Instance().GetGroupMatchById(model.etc5);

        //    string score = mmodel.SCORE;
        //    if (score == "未开赛")
        //    {
        //        score = "00";
        //    }

        //    int p1s = Convert.ToInt32(score.Substring(0, 1));
        //    int p2s = Convert.ToInt32(score.Substring(1, 1));
        //    //判断哪一队获胜，并修改团体比分
        //    if (model.WINNER == model.PLAYER1)
        //    {
        //        //1队获胜
        //        p1s += 1;
        //    }
        //    else
        //    {
        //        //2队获胜
        //        p2s += 1;
        //    }

        //    score = p1s.ToString() + p2s.ToString();

        //    //判断团体赛是否已经完成
        //    string status = "1";
        //    string winner = "";
        //    string loser = "";
        //    if (IsGroupMatchEnd(model.etc5))
        //    {
        //        //已完成
        //        status = "2";

        //        //判断获胜方
        //        if (p1s > p2s)
        //        {
        //            //
        //            winner = mmodel.PLAYER1;
        //            loser = mmodel.PLAYER2;
        //        }
        //        else
        //        {
        //            winner = mmodel.PLAYER2;
        //            loser = mmodel.PLAYER1;
        //        }
        //        string sql = "update wtf_groupmatch set winner='" + winner + "',loser='" + loser + "' where id='" + model.etc5 + "'";
        //        int a = DbHelperSQL.ExecuteSql(sql);
        //    }

        //    //修改团体赛的状态和比分
        //    string sql1 = "update wtf_groupmatch set status='" + status + "',score='" + score + "' where id='" + model.etc5 + "'";
        //    DbHelperSQL.ExecuteSql(sql1);

        //}

        /// <summary>
        /// 判断团体比赛是否已经完成
        /// </summary>
        /// <param name="_TeamMatchid"></param>
        /// <returns></returns>
        public bool IsGroupMatchEnd(string _TeamMatchid)
        {
            string sql = "select * from wtf_match where etc5='" + _TeamMatchid + "' and state!='2'";
            DataTable dt = DbHelperSQL.Query(sql).Tables[0];
            if (dt.Rows.Count > 0)
            {
                return false;
            }
            else
            {
                return true;
            }
        }
        #endregion


        /// <summary>
        /// 根据比赛主键来获得
        /// </summary>
        /// <param name="_Matchsys"></param>
        /// <returns></returns>
        public MatchModel GetMatchContents(string _matchsys)
        {
            MatchModel model = new MatchModel();

            //获得比赛双方的名字
            MatchModel mmodel = GetModel(_matchsys);
            if (mmodel.PLAYER1.IndexOf(",") > 0)
            {
                //双打比赛
                model.etc2 = MemberDll.Get_Instance().GetModel(mmodel.PLAYER1.Split(',')[0]).NAME + "," + MemberDll.Get_Instance().GetModel(mmodel.PLAYER1.Split(',')[1]).NAME;
                model.etc3 = MemberDll.Get_Instance().GetModel(mmodel.PLAYER2.Split(',')[0]).NAME + "," + MemberDll.Get_Instance().GetModel(mmodel.PLAYER2.Split(',')[1]).NAME;
            }
            else
            {
                //单打比赛
                model.etc2 = MemberDll.Get_Instance().GetModel(mmodel.PLAYER1).NAME;
                model.etc3 = MemberDll.Get_Instance().GetModel(mmodel.PLAYER2).NAME;
            }

            string setScore = SetDll.Get_Instance().GetSetScore(_matchsys);//获得盘比分
            if (mmodel.STATE == 2)
            {
                model.PLAYER1 = mmodel.SCORE.Substring(0, 1);
                model.PLAYER2 = mmodel.SCORE.Substring(1, 1);
            }
            else
            {
                model.PLAYER1 = setScore.Split(',')[0];
                model.PLAYER2 = setScore.Split(',')[1];
            }

            string gameScore = GameDll.Get_Instance().GetGameScore(_matchsys); //获取局比分
            model.PLAYER1NAME = gameScore.Split(',')[0];
            model.PLAYER2NAME = gameScore.Split(',')[1];

            model.etc1 = GameDll.Get_Instance().GetGameServer(_matchsys);//显示发起方和比赛胜方

            //获得比赛提醒
            model.etc5 = TrendDll.Get_Instance().getLatestTrendCom(_matchsys).COMMENTS;

            return model;
        }

        /// <summary>
        /// 根据比赛主键来获得参赛人员的信息
        /// </summary>
        /// <param name="_Matchsys"></param>
        /// <returns></returns>
        //public List<MemberModel> GetMemberlist(string _Matchsys)
        //{
        //    List<MemberModel> list = new List<MemberModel>();
        //    MatchModel model = MatchDll.Get_Instance().GetModel(_Matchsys);
        //    TourContentModel tcmodel = TourContentDll.Get_Instance().GetModelbyId(model.ContentID);
        //    if (tcmodel.ContentType.IndexOf("双") > 0)
        //    {
        //        //双打
        //        MemberModel model1 = MemberDll.Get_Instance().GetModel(model.PLAYER1.Split(',')[0]);
        //        list.Add(model1);

        //        model1 = MemberDll.Get_Instance().GetModel(model.PLAYER1.Split(',')[1]);
        //        list.Add(model1);

        //        model1 = MemberDll.Get_Instance().GetModel(model.PLAYER2.Split(',')[0]);
        //        list.Add(model1);

        //        model1 = MemberDll.Get_Instance().GetModel(model.PLAYER2.Split(',')[1]);
        //        list.Add(model1);
        //    }
        //    else
        //    {
        //        //单打
        //        MemberModel model1 = MemberDll.Get_Instance().GetModel(model.PLAYER1);
        //        list.Add(model1);

        //        MemberModel model2 = MemberDll.Get_Instance().GetModel(model.PLAYER2);
        //        list.Add(model2);
        //    }

        //    return list;
        //}

        /// <summary>
        /// 获得球员信息
        /// </summary>
        /// <param name="_Playersys"></param>
        /// <returns></returns>
        public string GetPlayersName(string _Playersys)
        {
            string _Pname = "";
            if (_Playersys.IndexOf(",") > 0)
            {
                _Pname = MemberDll.Get_Instance().GetModel(_Playersys.Split(',')[0]).USERNAME + "/" + MemberDll.Get_Instance().GetModel(_Playersys.Split(',')[1]).USERNAME;
            }
            else
            {
                _Pname = MemberDll.Get_Instance().GetModel(_Playersys).USERNAME;
            }
            return _Pname;
        }

        /// <summary>
        /// Get my ChanllengeMatch win qty
        /// </summary>
        /// <param name="_Memsys"></param>
        /// <returns></returns>
        private int GetMyChanWin(string _Memsys)
        {
            string sql = "select * from wtf_match where state=2 and TourSys is not null and winner='" + _Memsys + "'";
            return DbHelperSQL.Query(sql).Tables[0].Rows.Count;
        }

        /// <summary>
        /// Get my ChanllengeMatch win qty
        /// </summary>
        /// <param name="_Memsys"></param>
        /// <returns></returns>
        private int GetMyChanLose(string _Memsys)
        {
            string sql = "select * from wtf_match where state=2 and TourSys is not null and loser='" + _Memsys + "'";
            return DbHelperSQL.Query(sql).Tables[0].Rows.Count;
        }

        /// <summary>
        /// get chanllenge win rate
        /// </summary>
        /// <param name="_Memsys"></param>
        /// <returns></returns>
        public string GetMyChanWinRate(string _Memsys)
        {
            string wrate = "";
            int winQ = GetMyChanWin(_Memsys);
            int losQ = GetMyChanLose(_Memsys);

            wrate += Math.Round(Convert.ToDouble(winQ * 100 / (winQ + losQ)), 0).ToString();
            wrate += "% (" + winQ + "-" + losQ + ")";
            return wrate;
        }
    }
}
