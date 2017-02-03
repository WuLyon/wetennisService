using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;

namespace WeTour
{
    public class Biz_TourDate
    {
        public static Biz_TourDate instance = new Biz_TourDate();

#region CRUD
        /// <summary>
        /// 添加赛事日期
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public bool AddTourDate(Model_TourDate model)
        {
            string sql = string.Format("Insert into Tour_Dates (TourSys,TourDate) values ('{0}','{1}')",model.TourSys,model.TourDate);
            int a = DbHelperSQL.ExecuteSql(sql);
            if (a > 0)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        /// <summary>
        /// 删除日期
        /// </summary>
        /// <param name="_Id"></param>
        /// <returns></returns>
        public bool DeleteTourDate(string _Id)
        {
            string sql = "delete Tour_Dates where id='"+_Id+"'";
            int a = DbHelperSQL.ExecuteSql(sql);
            if (a > 0)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        /// <summary>
        /// 添加赛事轮次信息
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public bool AddTourDateRound(Model_TourDateRound model)
        {
            string sql = string.Format("Insert into Tour_DateRounds (TourSys,TourDate,ContentId,Round) values ('{0}','{1}','{2}','{3}')", model.TourSys, model.TourDate,model.ContentId,model.Round);
            int a = DbHelperSQL.ExecuteSql(sql);
            if (a > 0)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        private bool DeleteTourDateRound(string _TourSys, string _TourDate)
        {
            string sql = "delete Tour_DateRounds where TourSys='" + _TourSys + "' and TourDate='" + _TourDate + "'";
            int a = DbHelperSQL.ExecuteSql(sql);
            if (a > 0)
            {
                return true;
            }
            else
            {
                return false;
            }
        }
#endregion

        #region 获取数据
        /// <summary>
        /// 获取赛事日期
        /// </summary>
        /// <param name="_TourSys"></param>
        /// <returns></returns>
        public List<Model_TourDate> GetTourDate(string _TourSys)
        {
            List<Model_TourDate> list = new List<Model_TourDate>();
            string sql = "select * from Tour_Dates where TourSys='"+_TourSys+"'";
            DataTable dt = DbHelperSQL.Query(sql).Tables[0];
            if (dt.Rows.Count > 0)
            {
                list = JsonHelper.ParseDtModelList<List<Model_TourDate>>(dt);

                //为赛事加载比赛数量
                foreach (Model_TourDate model in list)
                {
                    model.TourMatchQty = GetMatchQtybyTourdate(_TourSys, model.TourDate).ToString();
                }
            }
            return list;
        }

        /// <summary>
        /// 获取比赛日期的比赛数量
        /// </summary>
        /// <param name="_TourSys"></param>
        /// <param name="_TourDate"></param>
        /// <returns></returns>
        private int GetMatchQtybyTourdate(string _TourSys, string _TourDate)
        {
            int DateQty = 0;
            List<Model_TourDateRound> list = GetTourDateAssRound(_TourSys, _TourDate);
            foreach (Model_TourDateRound model in list)
            {
                int a=Convert.ToInt32(model.Round) ;
                if (a> 0)
                {
                    DateQty += WeMatchDll.instance.CountMatchQtybyContRound(model.ContentId, model.Round);
                }
                else
                {
                    DateQty += WeMatchDll.instance.CountGroupMatchQtybyContRound(model.ContentId, (-a).ToString());
                }
            }
            return DateQty;
        }

        /// <summary>
        /// 获取日期已绑定的项目轮次
        /// </summary>
        /// <param name="_TourSys"></param>
        /// <param name="_Date"></param>
        /// <returns></returns>
        public List<Model_TourDateRound> GetTourDateAssRound(string _TourSys, string _Date)
        {
            List<Model_TourDateRound> list = new List<Model_TourDateRound>();
            string sql = "select * from Tour_DateRounds where TourSys='" + _TourSys + "' and TourDate='"+_Date+"'";
            DataTable dt = DbHelperSQL.Query(sql).Tables[0];
            if (dt.Rows.Count > 0)
            {
                list = JsonHelper.ParseDtModelList<List<Model_TourDateRound>>(dt);
            }
            else
            { 
                //未指定当天的轮次，则默认为赛事的所有轮次
                List<WeTourContModel> cont_list = WeTourContentDll.instance.GetContentlist(_TourSys);//根据赛事sys，获取所有的项目。
                foreach (WeTourContModel cont in cont_list)
                {
                   
                    //添加round
                    List<WeMatchModel> match_list = WeMatchDll.instance.GetDistinctRoundbyCont(cont.id);
                    foreach (WeMatchModel match in match_list)
                    {
                        Model_TourDateRound tourDateRound = new Model_TourDateRound();
                        tourDateRound.TourSys = _TourSys;
                        tourDateRound.TourDate = cont.TourDate;
                        tourDateRound.ContentId = cont.id;
                        tourDateRound.Round = match.ROUND.ToString();
                        tourDateRound.MatchDate = _Date;
                        list.Add(tourDateRound);
                    }
                }
            }
            return list;
        }

        

        private bool IsRoundChoosen(string _Contid, string _Round)
        {
            string sql = "select * from Tour_DateRounds where ContentId='" + _Contid + "' and Round='" + _Round + "'";
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

        private bool IsRoundChoosenInDate(string _Contid, string _Round, string _Date)
        {
            string sql = "select * from Tour_DateRounds where ContentId='" + _Contid + "' and Round='" + _Round + "' and TourDate='" + _Date + "'";
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


        public List<Model_DistriTDR> GetDisTourDate(string _TourSys, string _Date)
        {
            List<Model_DistriTDR> list = new List<Model_DistriTDR>();
            List<WeTourContModel> contL = WeTourContentDll.instance.GetContentlist(_TourSys);
            foreach (WeTourContModel cont in contL)
            {
                Model_DistriTDR model = new Model_DistriTDR();
                model.contId = cont.id;
                model.contName = cont.ContentName;

                //添加比赛数量
                model.MatchQty = WeMatchDll.instance.CountMatchQtybyCont(model.contId).ToString();

                //为每个项目添加轮次
                List<Model_DistricontRound> rlist = new List<Model_DistricontRound>();
                //从比赛信息表中查询轮次情况
                string sql = "select distinct(round) from wtf_match where contentid='"+cont.id+"'";
                DataTable dt = DbHelperSQL.Query(sql).Tables[0];
                foreach (DataRow dr in dt.Rows)
                {
                   
                    
                    //判断如果是小组赛，则要添加多轮小组赛
                    if (dr[0].ToString() == "0")
                    {
                        //如果是小组赛，需要将小组赛的轮次分开。查看小组赛的轮次
                        //查看小组赛最都的轮次
                        string sql2 = "select distinct(etc2) from wtf_match where contentid='" + cont.id + "' and round=0";
                        DataTable dt2 = DbHelperSQL.Query(sql2).Tables[0];
                        for (int j = 0; j < dt2.Rows.Count; j++)
                        {
                            Model_DistricontRound round = new Model_DistricontRound();
                            round.roundNum = "-" + dt2.Rows[j][0].ToString();
                            round.roundName = "小组赛第" + dt2.Rows[j][0].ToString() + "轮";
                            round.MatchQty = WeMatchDll.instance.CountGroupMatchQtybyContRound(model.contId, dt2.Rows[j][0].ToString()).ToString();

                            //
                            if (IsRoundChoosen(cont.id, round.roundNum))
                            {
                                //already been choosen
                                if (IsRoundChoosenInDate(cont.id, round.roundNum, _Date))
                                {
                                    //in this date
                                    round.isCheck = "1";
                                    round.isEnable = "0";
                                }
                                else
                                {
                                    // in another day
                                    round.isCheck = "1";
                                    round.isEnable = "1";
                                }
                            }
                            else
                            {
                                round.isCheck = "0";
                                round.isEnable = "0";
                            }
                            rlist.Add(round);
                        }
                    }
                    else
                    {
                        Model_DistricontRound round = new Model_DistricontRound();
                        //不是小组赛
                        round.roundNum = dr[0].ToString();
                        round.roundName = WeTourDll.instance.RenderRound(Convert.ToInt32(round.roundNum), Convert.ToInt32(cont.SignQty));
                        round.MatchQty = WeMatchDll.instance.CountMatchQtybyContRound(model.contId, round.roundNum).ToString();

                        //
                        if (IsRoundChoosen(cont.id, round.roundNum))
                        {
                            //already been choosen
                            if (IsRoundChoosenInDate(cont.id, round.roundNum, _Date))
                            {
                                //in this date
                                round.isCheck = "1";
                                round.isEnable = "0";
                            }
                            else
                            {
                                // in another day
                                round.isCheck = "1";
                                round.isEnable = "1";
                            }
                        }
                        else
                        {
                            round.isCheck = "0";
                            round.isEnable = "0";
                        }
                        rlist.Add(round);
                    }
                }
                model.contRound = rlist;

                list.Add(model);
            }
            return list;
        }

        /// <summary>
        /// 修改日期轮次分布
        /// </summary>
        /// <param name="_TourSys"></param>
        /// <param name="_Date"></param>
        /// <param name="list"></param>
        public void UpdateTourSignRound(string _TourSys, string _Date,List<Model_DistriTDR> list)
        {
            try
            {
                DeleteTourDateRound(_TourSys, _Date);
                foreach (Model_DistriTDR item in list)
                {
                    foreach (Model_DistricontRound contr in item.contRound)
                    {
                        if (contr.isCheck == "1" && contr.isEnable == "0")
                        {
                            Model_TourDateRound model = new Model_TourDateRound();
                            model.TourDate = _Date;
                            model.TourSys = _TourSys;
                            model.ContentId = item.contId;
                            model.Round = contr.roundNum;

                            AddTourDateRound(model);
                            //update match for match date
                            WeMatchDll.instance.AssignMatchDate(_TourSys, _Date, contr.roundNum);
                        }
                    }
                }
            }
            catch (Exception e)
            {
                DbHelperSQL.WriteLog("UpdateTourSignRound", e.ToString());
            }
        }

        #endregion
    }
}
