using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using System.Data.SqlClient;
using Member;
using Gym;

namespace WeTour
{
    public class WeMatchDll
    {
        string ImgSer = System.Configuration.ConfigurationManager.AppSettings["ImageServer"].ToString();
        //string ImgSer = "";
        public static WeMatchDll instance = new WeMatchDll();

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sys"></param>
        /// <returns></returns>
        public WeMatchModel GetModel(string sys)
        {
            WeMatchModel model = new WeMatchModel();
            string sql = "select * from wtf_match where sys='"+sys+"'";
            DataTable dt = DbHelperSQL.Query(sql).Tables[0];
            if (dt.Rows.Count > 0)
            {
                model = JsonHelper.ParseDtInfo<WeMatchModel>(dt);
            }
            return model;
        }

        /// <summary>
        /// 根据球员查询比赛
        /// </summary>
        /// <param name="_ContentId"></param>
        /// <param name="_Player1"></param>
        /// <param name="_Player2"></param>
        /// <returns></returns>
        public WeMatchModel GetModelbyPlayer(string _ContentId,string _Player1,string _Player2)
        {
            WeMatchModel model = new WeMatchModel();
            string sql = string.Format("select * from wtf_match where ContentID='{0}' and (player1='{1}' and player2='{2}') or (player1='{2}' and player2='{1}')",_ContentId,_Player1,_Player2);
            DataTable dt = DbHelperSQL.Query(sql).Tables[0];
            if (dt.Rows.Count > 0)
            {
                model = JsonHelper.ParseDtInfo<WeMatchModel>(dt);
            }
            return model;
        }

        /// <summary>
        /// 修改比赛状态
        /// </summary>
        /// <param name="_Sys"></param>
        /// <param name="_State"></param>
        /// <returns></returns>
        public bool UpdateMatchStatus(string _Sys, string _State)
        {
            string sql = "Update wtf_match set state='"+_State+"' where sys='"+_Sys+"'";
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
        /// Get Match by Contentid
        /// </summary>
        /// <param name="_Contentid"></param>
        /// <returns></returns>
        public List<WeMatchModel> GetMatchlistbyCont(string _Contentid)
        {
             WeTourContModel wmodel=WeTourContentDll.instance.GetModelbyId(_Contentid);
            List<WeMatchModel> list = new List<WeMatchModel>();
            List<WeMatchModel> nlist = new List<WeMatchModel>();
            string sql = "select * from wtf_match where contentid='" + _Contentid + "' order by convert(int,matchorder) desc";
            DataTable dt = DbHelperSQL.Query(sql).Tables[0];
            if (dt.Rows.Count > 0)
            {
                list = JsonHelper.ParseDtModelList<List<WeMatchModel>>(dt);
            }
            if (list.Count > 0)
            {
                foreach (WeMatchModel model in list)
                {
                    nlist.Add(RenderMatch(model.SYS));
                }
            }
            return nlist;
        }

        /// <summary>
        /// add information to a match
        /// </summary>
        /// <param name="_Sys"></param>
        /// <returns></returns>
        public WeMatchModel RenderMatch(string _Sys)
        {
            WeMatchModel model = GetModel(_Sys);
            WeTourContModel wcmodel = WeTourContentDll.instance.GetModelbyId(model.ContentID);
            //添加人员姓名
            if (!string.IsNullOrEmpty(model.PLAYER1))
            {
                if (wcmodel.ContentType == "团体")
                {
                    model.Player1ComName = weTeamdll.instance.GetModel(model.PLAYER1).TEAMNAME;
                    model.Player1Lname = model.Player1ComName;
                }
                else
                {
                    //player1
                    if (model.PLAYER1.IndexOf(",") > 0)
                    {
                        string[] p1 = model.PLAYER1.Split(',');
                        WeMemberModel m1l = WeMemberDll.instance.GetModel(p1[0]);
                        model.Player1Lname = m1l.USERNAME;
                        if (m1l.EXT1.IndexOf("http") >= 0)
                        {
                            model.Player1Limage = m1l.EXT1;
                        }
                        else
                        {
                            model.Player1Limage = ImgSer + m1l.EXT1;
                        }

                        WeMemberModel m1r = WeMemberDll.instance.GetModel(p1[1]);
                        model.Player1Rname = m1r.USERNAME;
                        if (m1r.EXT1.IndexOf("http") >= 0)
                        {
                            model.Player1Rimage = m1r.EXT1;
                        }
                        else
                        {
                            model.Player1Rimage = ImgSer + m1r.EXT1;
                        }

                        model.Player1ComName = m1l.USERNAME + "/" + m1r.USERNAME;
                    }
                    else
                    {
                        WeMemberModel m1l = WeMemberDll.instance.GetModel(model.PLAYER1);
                        model.Player1Lname = m1l.USERNAME;
                        if (m1l.EXT1.IndexOf("http") >= 0)
                        {
                            model.Player1Limage = m1l.EXT1;
                        }
                        else
                        {
                            model.Player1Limage = ImgSer + m1l.EXT1;
                        }
                        model.Player1ComName = m1l.USERNAME;
                    }
                }
            }
            else
            {
                model.Player1Lname = "未知";
                model.Player1Limage = ImgSer + "/images/touxiang.jpg";
                model.Player1ComName = "未知";
            }
            if (!string.IsNullOrEmpty(model.PLAYER2))
            {
                if (wcmodel.ContentType == "团体")
                {
                    model.Player2ComName = weTeamdll.instance.GetModel(model.PLAYER2).TEAMNAME;
                    model.Player2Lname = model.Player2ComName;
                }
                else
                {
                    //player2
                    if (model.PLAYER2.IndexOf(",") > 0)
                    {
                        string[] p2 = model.PLAYER2.Split(',');
                        WeMemberModel m2l = WeMemberDll.instance.GetModel(p2[0]);
                        model.Player2Lname = m2l.USERNAME;
                        if (m2l.EXT1.IndexOf("http") >= 0)
                        {
                            model.Player2Limage = m2l.EXT1;
                        }
                        else
                        {
                            model.Player2Limage = ImgSer + m2l.EXT1;
                        }

                        WeMemberModel m2r = WeMemberDll.instance.GetModel(p2[1]);
                        model.Player2Rname = m2r.USERNAME;
                        if (m2r.EXT1.IndexOf("http") >= 0)
                        {
                            model.Player2Rimage = m2r.EXT1;
                        }
                        else
                        {
                            model.Player2Rimage = ImgSer + m2r.EXT1;
                        }

                        model.Player2ComName = m2l.USERNAME + "/" + m2r.USERNAME;
                    }
                    else
                    {
                        WeMemberModel m2l = WeMemberDll.instance.GetModel(model.PLAYER2);
                        model.Player2Lname = m2l.USERNAME;
                        if (m2l.EXT1.IndexOf("http") >= 0)
                        {
                            model.Player2Limage = m2l.EXT1;
                        }
                        else
                        {
                            model.Player2Limage = ImgSer + m2l.EXT1;
                        }
                        model.Player2ComName = m2l.USERNAME;
                    }
                }
            }
            else
            {
                model.Player2Lname = "未知";
                model.Player2Limage = ImgSer + "/images/touxiang.jpg";
                model.Player2ComName = "未知";
            }
            //render round
            WeTourContModel wmodel = WeTourContentDll.instance.GetModelbyId(model.ContentID);
            int SignQty = Convert.ToInt32(wmodel.SignQty);
            int Round = Convert.ToInt32(model.ROUND);
            model.RoundName = WeTourDll.instance.RenderRound(Round, SignQty);
            //render court
            CourtModel cmodel = CourtDll.Get_Instance().GetModelbyId(model.COURTID);
            model.CourtName = cmodel.COURTNAME;

            //render contentname
            model.ContentName = wmodel.ContentName;
            model.etc4 = wmodel.TourDate;//组别


            //render tourname
            model.TourName = WeTourDll.instance.GetModelbySys(model.TOURSYS).NAME;

            //render match status
            switch (model.STATE)
            { 
                case 0:
                    model.MatchStatus = "未开始";
                    break;
                case 1:
                    model.MatchStatus = "进行中";
                    break;
                case 2:
                    model.MatchStatus = "已完成";
                    break;
            }
            return model;
        }

        /// <summary>
        /// 获得项目指定轮次的比赛
        /// </summary>
        /// <param name="_Contentid"></param>
        /// <param name="_Round"></param>
        /// <returns></returns>
        public List<WeMatchModel> GetMatchlistbyContRound(string _Contentid, string _Round)
        {
            WeTourContModel wmodel = WeTourContentDll.instance.GetModelbyId(_Contentid);
            List<WeMatchModel> list = new List<WeMatchModel>();
            List<WeMatchModel> nlist = new List<WeMatchModel>();
            string sql = "select * from wtf_match where contentid='" + _Contentid + "' and Round='" + _Round + "' order by convert(int,matchorder) desc";
            DataTable dt = DbHelperSQL.Query(sql).Tables[0];
            if (dt.Rows.Count > 0)
            {
                list = JsonHelper.ParseDtModelList<List<WeMatchModel>>(dt);
            }
            if (list.Count > 0)
            {
                foreach (WeMatchModel model in list)
                {
                    nlist.Add(RenderMatch(model.SYS));
                }
            }
            return nlist;
        }

        public List<WeMatchModel> GetFinishedMatchlistbyContRound(string _Contentid, string _Round)
        {
            WeTourContModel wmodel = WeTourContentDll.instance.GetModelbyId(_Contentid);
            List<WeMatchModel> list = new List<WeMatchModel>();
            List<WeMatchModel> nlist = new List<WeMatchModel>();
            string sql = "select * from wtf_match where contentid='" + _Contentid + "' and Round='" + _Round + "' and state='2' order by convert(int,matchorder) desc";
            DataTable dt = DbHelperSQL.Query(sql).Tables[0];
            if (dt.Rows.Count > 0)
            {
                list = JsonHelper.ParseDtModelList<List<WeMatchModel>>(dt);
            }
            if (list.Count > 0)
            {
                foreach (WeMatchModel model in list)
                {
                    nlist.Add(RenderMatch(model.SYS));
                }
            }
            return nlist;
        }

        /// <summary>
        /// 获得比赛项目的轮次
        /// </summary>
        /// <param name="_ContentId"></param>
        /// <returns></returns>
        public List<WeMatchModel> GetContentRounds(string _ContentId)
        {
            WeTourContModel Contmodel=WeTourContentDll.instance.GetModelbyId(_ContentId);
            List<WeMatchModel> list = new List<WeMatchModel>();
            string sql = "select distinct(ROUND) from wtf_match where ContentID='" + _ContentId + "' order by CONVERT(int,round) desc";
            DataTable dt = DbHelperSQL.Query(sql).Tables[0];
            if (dt.Rows.Count > 0)
            {
                list = JsonHelper.ParseDtModelList<List<WeMatchModel>>(dt);
                foreach (WeMatchModel model in list)
                { 
                    int Round=Convert.ToInt32(model.ROUND);
                    int CapQty=Convert.ToInt32(Contmodel.SignQty);
                    model.RoundName = WeTourDll.instance.RenderRound(Round, CapQty);
                }
            }
            return list;
        }

        /// <summary>
        /// 升序排列
        /// </summary>
        /// <param name="_ContentId"></param>
        /// <returns></returns>
        public List<WeMatchModel> GetContentRoundsAsc(string _ContentId)
        {
            WeTourContModel Contmodel = WeTourContentDll.instance.GetModelbyId(_ContentId);
            List<WeMatchModel> list = new List<WeMatchModel>();
            string sql = "select distinct(ROUND) from wtf_match where ContentID='" + _ContentId + "' order by CONVERT(int,round)";
            DataTable dt = DbHelperSQL.Query(sql).Tables[0];
            if (dt.Rows.Count > 0)
            {
                list = JsonHelper.ParseDtModelList<List<WeMatchModel>>(dt);
                foreach (WeMatchModel model in list)
                {
                    int Round = Convert.ToInt32(model.ROUND);
                    int CapQty = Convert.ToInt32(Contmodel.SignQty);
                    model.RoundName = WeTourDll.instance.RenderRound(Round, CapQty);
                }
            }
            return list;
        }

        /// <summary>
        /// 根据比赛赛事主键，比赛日期，场地编号，获得比赛数据
        /// </summary>
        /// <param name="_Toursys"></param>
        /// <param name="_MatchDate"></param>
        /// <param name="_CourtId"></param>
        /// <returns></returns>
        public List<WeMatchModel> GetMatchesbyCourtDate(string _Toursys, string _MatchDate, string _CourtId)
        {
            List<WeMatchModel> list = new List<WeMatchModel>();
            List<WeMatchModel> nlist = new List<WeMatchModel>();
            string sql = "select * from wtf_match where toursys='" + _Toursys + "' and matchdate='"+_MatchDate+"' and courtId='"+_CourtId+"' order by CONVERT(int,place)";
            DataTable dt = DbHelperSQL.Query(sql).Tables[0];
            if (dt.Rows.Count > 0)
            {
                list = JsonHelper.ParseDtModelList<List<WeMatchModel>>(dt);
                foreach (WeMatchModel model in list)
                {
                    nlist.Add(RenderMatch(model.SYS));
                }
            }
            return nlist;
        }

        /// <summary>
        /// 获取场地的比赛主键
        /// </summary>
        /// <param name="_Toursys"></param>
        /// <param name="_MatchDate"></param>
        /// <param name="_CourtId"></param>
        /// <returns></returns>
        public Dictionary<string, object> GetMatchsys_byCourtDate(string _Toursys, string _MatchDate, string _CourtId)
        {
            Dictionary<string, object> item = new Dictionary<string, object>();
            //添加场地名称
            string courtName = CourtDll.Get_Instance().GetCourtFullName(_CourtId);
            item.Add("name", courtName);
            //添加比赛主键
            List<string> matchSys = new List<string>();
            string sql = "select * from wtf_match where toursys='" + _Toursys + "' and matchdate='" + _MatchDate + "' and courtId='" + _CourtId + "' order by CONVERT(int,place)";
            DataTable dt = DbHelperSQL.Query(sql).Tables[0];
            foreach (DataRow dr in dt.Rows)
            {
                matchSys.Add(dr["sys"].ToString());
            }
            item.Add("matches", matchSys);
            return item;
        }

        /// <summary>
        /// 根据赛事主键和具体日期获取比赛
        /// </summary>
        /// <param name="_Toursys"></param>
        /// <param name="_MatchDate"></param>
        /// <returns></returns>
        public List<WeMatchModel> GetMachesByDate(string _Toursys, string _MatchDate)
        {            
            List<WeMatchModel> list = new List<WeMatchModel>();
            List<WeMatchModel> nlist = new List<WeMatchModel>();
            string sql = "select * from wtf_match where toursys='" + _Toursys + "' and matchdate='" + _MatchDate + "'";
            DataTable dt = DbHelperSQL.Query(sql).Tables[0];
            if (dt.Rows.Count > 0)
            {
                list = JsonHelper.ParseDtModelList<List<WeMatchModel>>(dt);
                foreach (WeMatchModel model in list)
                {
                    nlist.Add(RenderMatch(model.SYS));
                }
            }
            return nlist;
        }

        /// <summary>
        /// 获取单独的courtid
        /// </summary>
        /// <param name="_TourSys"></param>
        /// <param name="_date"></param>
        /// <returns></returns>
        public List<string> GetDistinctCourt(string _TourSys, string _date)
        {
            List<string> court_list = new List<string>();
            string sql = "select distinct(COURTID) from wtf_match where toursys='" + _TourSys + "' and matchdate='" + _date + "' ";
            DataTable dt = DbHelperSQL.Query(sql).Tables[0];
            foreach (DataRow dr in dt.Rows)
            {
                court_list.Add(dr[0].ToString());
            }
            return court_list;
        }
       


        /// <summary>
        /// 根据赛事主键获得赛事已完成的比赛
        /// </summary>
        /// <param name="_Toursys"></param>
        /// <returns></returns>
        public List<WeMatchModel> GetContFinishMatches(string _ContentId)
        {
            List<WeMatchModel> list = new List<WeMatchModel>();
            List<WeMatchModel> nlist = new List<WeMatchModel>();
            string sql = "select * from wtf_match where contentid='" + _ContentId + "' and state=2 order by convert(int,matchorder) desc";
            DataTable dt= DbHelperSQL.Query(sql).Tables[0];
            if (dt.Rows.Count > 0)
            {
                list = JsonHelper.ParseDtModelList<List<WeMatchModel>>(dt);
                foreach (WeMatchModel model in list)
                {
                    nlist.Add(RenderMatch(model.SYS));
                }
            }
            return nlist;
        }

        /// <summary>
        /// 根据赛事主键获得赛事已完成的比赛
        /// </summary>
        /// <param name="_Toursys"></param>
        /// <returns></returns>
        public List<WeMatchModel> GetContMachthByStatus(string _ContentId,string _status)
        {
            List<WeMatchModel> list = new List<WeMatchModel>();
            List<WeMatchModel> nlist = new List<WeMatchModel>();
            string sql = "select * from wtf_match where contentid='" + _ContentId + "'";
            if (_status == "1" || _status == "2")
            {
                sql += "and state=" + _status + "";
            }
            else
            {
                sql += " and state in ('1','2')";
            }
            sql += " order by convert(int,matchorder) desc";

            DataTable dt = DbHelperSQL.Query(sql).Tables[0];
            if (dt.Rows.Count > 0)
            {
                list = JsonHelper.ParseDtModelList<List<WeMatchModel>>(dt);
                foreach (WeMatchModel model in list)
                {
                    nlist.Add(RenderMatch(model.SYS));
                }
            }
            return nlist;
        }


#region 获得赛事资源分配所需要的比赛
        /// <summary>
        /// 同一项目的不同组的比赛，并根据小组轮次来排序
        /// </summary>
        /// <param name="_contentid"></param>
        /// <param name="_Groups"></param>
        /// <returns></returns>
        public List<WeMatchModel> GetMatchesbyContGroup(string _contentid, string _Groups)
        {
            List<WeMatchModel> list = new List<WeMatchModel>();
            if (_Groups.IndexOf(",") > 0)
            {
                string[] gps = _Groups.Split(',');
                string gpss = "";
                for (int i = 0; i < gps.Length; i++)
                {
                    gpss += "'" + gps[i] + "',";
                }
                _Groups = gpss.TrimEnd(',');
            }
            else
            {
                _Groups = "'" + _Groups + "'";
            }

            string sql = "select * from wtf_match where contentid='" + _contentid + "' and state=0 and round=0 and etc1 in (" + _Groups + ") order by convert(int,etc2)";
            DataTable dt = DbHelperSQL.Query(sql).Tables[0];
            if (dt.Rows.Count > 0)
            {
                list = JsonHelper.ParseDtModelList<List<WeMatchModel>>(dt);
            }
            return list;
        }

        /// <summary>
        /// 获得一个或多个赛事子项的比赛，按照小组赛轮次来进行排序
        /// </summary>
        /// <param name="_Contents"></param>
        /// <returns></returns>
        public List<WeMatchModel> GetMatchesbyConts(string _Contents)
        {
            List<WeMatchModel> list = new List<WeMatchModel>();

            string sql = "select * from wtf_match where contentid in ("+_Contents+") and state=0 and round=0 order by convert(int,etc2) desc";
            DataTable dt = DbHelperSQL.Query(sql).Tables[0];
            if (dt.Rows.Count > 0)
            {
                list = JsonHelper.ParseDtModelList<List<WeMatchModel>>(dt);
            }
            return list;
        }

        /// <summary>
        /// 获取整个赛事的小组赛比赛用于场地资源分配
        /// </summary>
        /// <param name="_Toursys"></param>
        /// <returns></returns>
        public List<WeMatchModel> GetMatchesbyToursys4Dis(string _Toursys)
        {
            List<WeMatchModel> list = new List<WeMatchModel>();
            string sql = "select * from wtf_match where toursys='"+_Toursys+"' and state=0 and round=0 order by convert(int,etc2) desc";
            DataTable dt = DbHelperSQL.Query(sql).Tables[0];
            if (dt.Rows.Count > 0)
            {
                list = JsonHelper.ParseDtModelList<List<WeMatchModel>>(dt);
            }
            return list;
        }

#endregion 

        /// <summary>
        /// 修改比赛晋级
        /// </summary>
        /// <param name="_sys"></param>
        /// <param name="_winto"></param>
        /// <param name="_ext2"></param>
        /// <returns></returns>
        public bool UpdateMatchWinto(string _sys, string _winto, string _ext2)
        {
            string sql = string.Format("update wtf_match set winto='{0}',etc3='{1}' where sys='{2}'",_winto,_ext2,_sys);
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
        /// update winto logic for group matches
        /// </summary>
        /// <param name="_ContentId"></param>
        /// <param name="_Groupid"></param>
        /// <param name="_winto"></param>
        /// <param name="_ext2"></param>
        /// <returns></returns>
        public bool UpdateGroupMatchWinto(string _ContentId, string _Groupid, string _winto, string _ext2)
        {
            string sql = string.Format("update wtf_match set winto='{0}',etc3='{1}' where contentid='{2}' and etc1='{3}'", _winto, _ext2, _ContentId,_Groupid);
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

        public bool Insert(WeMatchModel model)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append(" Insert into wtf_match (");
            strSql.Append(" sys,player1,player2,matchdate,place,winner,loser,score,matchtype,gradetype,toursys,state,isdecide,round,playtype,predittime,courtid,ContentID,matchorder,etc1,etc2,etc3,etc4,etc5,gameQty) values (");
            strSql.Append("@sys,@player1,@player2,@matchdate,@place,@winner,@loser,@score,@matchtype,@gradetype,@toursys,@state,@isdecide,@round,@playtype,@predittime,@courtid,@ContentID,@matchorder,@etc1,@etc2,@etc3,@etc4,@etc5,@gameQty)");
            SqlParameter[] parameters = { 
                        new SqlParameter("@sys",DbType.String),
                        new SqlParameter("@player1",DbType.String),
                        new SqlParameter("@player2",DbType.String),
                        new SqlParameter("@matchdate",DbType.String),
                        new SqlParameter("@place",DbType.String),
                        new SqlParameter("@winner",DbType.String),
                        new SqlParameter("@loser",DbType.String),
                        new SqlParameter("@score",DbType.String),
                        new SqlParameter("@matchtype",DbType.Int32),
                        new SqlParameter("@gradetype",DbType.Int32),
                        new SqlParameter("@toursys",DbType.String),
                        new SqlParameter("@state",DbType.Int32),
                        new SqlParameter("@isdecide",DbType.Int32),
                        new SqlParameter("@round",DbType.Int32),
                        new SqlParameter("@playtype",DbType.String),
                        new SqlParameter("@predittime",DbType.String),
                        new SqlParameter("@courtid",DbType.String),
                        new SqlParameter("@ContentID",DbType.String),
                        new SqlParameter("@matchorder",DbType.String),
                        new SqlParameter("@etc1",DbType.String),
                        new SqlParameter("@etc2",DbType.String),
                        new SqlParameter("@etc3",DbType.String),
                        new SqlParameter("@etc4",DbType.String),
                        new SqlParameter("@etc5",DbType.String),
                        new SqlParameter("@gameQty",DbType.String)
                                      };
            parameters[0].Value = Guid.NewGuid().ToString("N").ToUpper();
            parameters[1].Value = model.PLAYER1;
            parameters[2].Value = model.PLAYER2;
            parameters[3].Value = model.MATCHDATE;
            parameters[4].Value = model.PLACE;
            parameters[5].Value = model.WINNER;
            parameters[6].Value = model.LOSER;
            parameters[7].Value = model.SCORE;
            parameters[8].Value = model.MATCHTYPE;
            parameters[9].Value = model.GRADETYPE;
            parameters[10].Value = model.TOURSYS;
            parameters[11].Value = model.STATE;
            parameters[12].Value = model.ISDECIDE;
            parameters[13].Value = model.ROUND;
            parameters[14].Value = model.PLAYTYPE;
            parameters[15].Value = model.PREDITTIME;
            parameters[16].Value = model.COURTID;
            parameters[17].Value = model.ContentID;
            parameters[18].Value = model.matchorder;
            parameters[19].Value = model.etc1;
            parameters[20].Value = model.etc2;
            parameters[21].Value = model.etc3;
            parameters[22].Value = model.etc4;
            parameters[23].Value = model.etc5;
            parameters[24].Value = model.GameQty;

            int res = DbHelperSQL.ExecuteSql(strSql.ToString(), parameters);
            if (res > 0)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        /// <summary>
        /// 修改比赛的图片
        /// </summary>
        /// <param name="sysno"></param>
        /// <param name="imgurl"></param>
        public void UpdateMatchImg(string sysno, string imgurl)
        {
            string sql = "update wtf_match set MatchImg='" + imgurl + "' where sys='" + sysno + "'";
            int a = DbHelperSQL.ExecuteSql(sql);
        }

        #region Get Match Data
        /// <summary>
        /// get tour total match qty
        /// </summary>
        /// <param name="_Toursys"></param>
        /// <returns></returns>
        public int CountMatchQtybyTour(string _Toursys)
        {
            string sql = "select * from wtf_match where toursys='" + _Toursys + "' and state=0 and player1!='1bf973ea-7d4c-41d1-aa05-8bf68b567d77' and player2!='1bf973ea-7d4c-41d1-aa05-8bf68b567d77'";
            DataTable dt = DbHelperSQL.Query(sql).Tables[0];
            return dt.Rows.Count;
        }

        /// <summary>
        /// get tourcontent match qty
        /// </summary>
        /// <param name="_ContentId"></param>
        /// <returns></returns>
        public int CountMatchQtybyCont(string _ContentId)
        {
            string sql = "select * from wtf_match where contentid='" + _ContentId + "' and state=0 and player1!='1bf973ea-7d4c-41d1-aa05-8bf68b567d77' and player2!='1bf973ea-7d4c-41d1-aa05-8bf68b567d77'";
            DataTable dt = DbHelperSQL.Query(sql).Tables[0];
            return dt.Rows.Count;
        }

        /// <summary>
        /// 获取比赛数量，根据项目和轮次
        /// </summary>
        /// <param name="_ContentId"></param>
        /// <returns></returns>
        public int CountMatchQtybyContRound(string _ContentId,string _Round)
        {
            string sql = "select * from wtf_match where contentid='" + _ContentId + "' and round='" + _Round + "' and  state=0 and player1!='1bf973ea-7d4c-41d1-aa05-8bf68b567d77' and player2!='1bf973ea-7d4c-41d1-aa05-8bf68b567d77'";

            DataTable dt = DbHelperSQL.Query(sql).Tables[0];
            return dt.Rows.Count;
        }

        /// <summary>
        /// 获取小组比赛，各轮次的比赛数量
        /// 比赛数量，根据项目和轮次
        /// </summary>
        /// <param name="_ContentId"></param>
        /// <returns></returns>
        public int CountGroupMatchQtybyContRound(string _ContentId, string _Round)
        {
            string sql = "select * from wtf_match where contentid='" + _ContentId + "' and round='0' and etc2='" + _Round + "' and  state=0 and player1!='1bf973ea-7d4c-41d1-aa05-8bf68b567d77' and player2!='1bf973ea-7d4c-41d1-aa05-8bf68b567d77'";

            DataTable dt = DbHelperSQL.Query(sql).Tables[0];
            return dt.Rows.Count;
        }


        #endregion

        #region Deal with Match
        /// <summary>
        /// update a match state
        /// </summary>
        /// <param name="_Sys"></param>
        /// <param name="_State"></param>
        /// <returns></returns>
        public bool UpdatematchState(string _Sys, string _State)
        {
            string sql = "update wtf_match set state='" + _State + "' where sys='" + _Sys + "'";
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
        /// update match result
        /// </summary>
        /// <param name="_Sys"></param>
        /// <param name="_Winner"></param>
        /// <param name="_Loser"></param>
        /// <param name="_Score"></param>
        /// <returns></returns>
        public bool UpdateMatchResult(string _Sys, string _Winner, string _Loser, string _Score)
        {
            string sql = "update wtf_match set winner='" + _Winner + "',loser='" + _Loser + "',score='" + _Score + "',state='2' where sys='" + _Sys + "'";
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
        /// get match model by contentid and match order
        /// </summary>
        /// <param name="_Contentid"></param>
        /// <param name="_matchOrder"></param>
        /// <returns></returns>
        public WeMatchModel GetMatchbyMatchOrder(string _Contentid, string _matchOrder)
        {
            WeMatchModel model = new WeMatchModel();
            string sql = "select * from wtf_match where contentid='"+_Contentid+"' and matchorder='"+_matchOrder+"'";
            DataTable dt = DbHelperSQL.Query(sql).Tables[0];
            if (dt.Rows.Count > 0)
            {
                model = JsonHelper.ParseDtInfo<WeMatchModel>(dt);
            }
            return model;
        }

        /// <summary>
        /// udpate player1
        /// </summary>
        /// <param name="_Sys"></param>
        /// <param name="_Playersys"></param>
        public void UpdatePlayer1(string _Sys, string _Playersys)
        {
            string sql = "update wtf_match set player1='"+_Playersys+"' where sys='" + _Sys + "'";
            int a = DbHelperSQL.ExecuteSql(sql);
        }

        /// <summary>
        /// update player2
        /// </summary>
        /// <param name="_Sys"></param>
        /// <param name="_Playersys"></param>
        public void UpdatePlayer2(string _Sys, string _Playersys)
        {
            string sql = "update wtf_match set player2='" + _Playersys + "' where sys='" + _Sys + "'";
            int a = DbHelperSQL.ExecuteSql(sql);
        }

        /// <summary>
        /// Get max group round of a tour
        /// </summary>
        /// <param name="_TourSys"></param>
        /// <returns></returns>
        public int GetGroupMaxRound(string _TourSys)
        {
            int Ground = 0;
            string sql = "select MAX(etc2) from wtf_match where toursys='"+_TourSys+"' and Round='0'";
            DataTable dt = DbHelperSQL.Query(sql).Tables[0];
            if (!string.IsNullOrEmpty(dt.Rows[0][0].ToString()))
            {
                Ground =Convert.ToInt16(dt.Rows[0][0].ToString());
            }
            return Ground;
        }

        /// <summary>
        /// Get group matches by round
        /// </summary>
        /// <param name="TourSys"></param>
        /// <param name="Round"></param>
        /// <returns></returns>
        public List<WeMatchModel> GetGroupMatchbyRound(string TourSys, string Round)
        {
            List<WeMatchModel> list = new List<WeMatchModel>();
            string sql = "select * from wtf_match where toursys='" + TourSys + "' and round='0' and etc2='" + Round + "' and player1!='1bf973ea-7d4c-41d1-aa05-8bf68b567d77' and player2!='1bf973ea-7d4c-41d1-aa05-8bf68b567d77'";
            DataTable dt = DbHelperSQL.Query(sql).Tables[0];
            if (dt.Rows.Count > 0)
            {
                list = JsonHelper.ParseDtModelList<List<WeMatchModel>>(dt);
            }
            return list;
        }

        /// <summary>
        /// get max Knock out match round
        /// </summary>
        /// <param name="_Toursys"></param>
        /// <returns></returns>
        public int GetMaxKnockRound(string _Toursys)
        {
            int Ground = 0;
            string sql = "select MAX(Round) from wtf_match where toursys='" + _Toursys + "'";
            DataTable dt = DbHelperSQL.Query(sql).Tables[0];
            if (dt.Rows.Count > 0)
            {
                Ground = Convert.ToInt16(dt.Rows[0][0].ToString());
            }
            return Ground;
        }

        /// <summary>
        /// get knock out match
        /// </summary>
        /// <param name="_Toursys"></param>
        /// <param name="_Round"></param>
        /// <returns></returns>
        public List<WeMatchModel> getKnockOutMatch(string _Toursys, string _Round)
        {
            List<WeMatchModel> list = new List<WeMatchModel>();
            string sql = "select * from wtf_match where toursys='" + _Toursys + "' and round='" + _Round + "' and player1!='1bf973ea-7d4c-41d1-aa05-8bf68b567d77' and player2!='1bf973ea-7d4c-41d1-aa05-8bf68b567d77'";
            DataTable dt = DbHelperSQL.Query(sql).Tables[0];
            if (dt.Rows.Count > 0)
            {
                list = JsonHelper.ParseDtModelList<List<WeMatchModel>>(dt);
            }
            return list;
        }

        /// <summary>
        /// Update Match Court Id
        /// </summary>
        /// <param name="_Sys"></param>
        /// <param name="_CourtId"></param>
        public void UpdateMatch_CourtId(string _Sys, string _CourtId)
        {
            string sql = "update wtf_match set courtid='"+_CourtId+"' where sys='"+_Sys+"'";
            int a = DbHelperSQL.ExecuteSql(sql);
        }

        /// <summary>
        /// Update Match Place by sys
        /// </summary>
        /// <param name="_Sys"></param>
        /// <param name="_Place"></param>
        public void UpdateMatch_Place(string _Sys, string _Place)
        {
            string sql = "update wtf_match set place='" + _Place + "' where sys='" + _Sys + "'";
            int a = DbHelperSQL.ExecuteSql(sql);
        }

        /// <summary>
        /// get court Qty
        /// </summary>
        /// <param name="_Toursys"></param>
        /// <returns></returns>
        public DataTable GetCourtQty(string _Toursys)
        {
            string sql = "select distinct(courtid) from wtf_match where toursys='"+_Toursys+"'";
            DataTable dt = DbHelperSQL.Query(sql).Tables[0];
            return dt;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="_Toursys"></param>
        /// <param name="_Courtid"></param>
        /// <returns></returns>
        public List<WeMatchModel> GetMatchbyCourtid(string _Toursys, string _Courtid)
        {
            List<WeMatchModel> list = new List<WeMatchModel>();
            string sql = "select * from wtf_match where toursys='"+_Toursys+"' and courtid='"+_Courtid+"' order by convert(int,matchorder)";
            DataTable dt = DbHelperSQL.Query(sql).Tables[0];
            if (dt.Rows.Count > 0)
            {
                list = JsonHelper.ParseDtModelList<List<WeMatchModel>>(dt);
            }
            return list;
        }

        /// <summary>
        /// 按照courtid查询比赛，查询的结果按照比赛顺序来排序
        /// </summary>
        /// <param name="_Toursys"></param>
        /// <param name="_Courtid"></param>
        /// <returns></returns>
        public List<WeMatchModel> GetMatchbyCourtidPlace(string _Toursys, string _Courtid)
        {
            List<WeMatchModel> list = new List<WeMatchModel>();
            string sql = "select * from wtf_match where toursys='" + _Toursys + "' and courtid='" + _Courtid + "' order by convert(int,place)";
            DataTable dt = DbHelperSQL.Query(sql).Tables[0];
            if (dt.Rows.Count > 0)
            {
                list = JsonHelper.ParseDtModelList<List<WeMatchModel>>(dt);
            }
            return list;
        }

        /// <summary>
        /// before close the tour ,check if all matches are closed
        /// </summary>
        /// <param name="_TourSys"></param>
        /// <returns></returns>
        public bool AreAllMatchFinish(string _TourSys)
        {
            string sql = "select * from wtf_match where toursys='" + _TourSys + "' and state='0'";
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

        #region Match Resource
        /// <summary>
        /// suggest resource needed
        /// </summary>
        /// <param name="_TourSys"></param>
        /// <returns></returns>
        public string GetSuggestResource(string _TourSys)
        {
            string _Suggest = "";
            int CourtQty = 0;
            int matchdate = 0;
            //Default strategy is minimum court qty
            int Total = CountMatchQtybyTour(_TourSys);
            float a = Total / 15;
            int CourtQbyM = Convert.ToInt32(Math.Ceiling(a));
            if (CourtQbyM == 0)
            {
                CourtQbyM = 1;
            }

            //Initiate court qty by tour content qty
            List<WeTourContModel> list = WeTourContentDll.instance.GetContentlist(_TourSys);
            int CourtQtybyCont = list.Count;
            if (CourtQtybyCont > CourtQbyM)
            {
                CourtQty = Convert.ToInt32(CourtQbyM);
                matchdate = 1;
            }
            else
            {
                CourtQty = CourtQtybyCont;
                decimal comdate = Math.Ceiling(Convert.ToDecimal(Total / (15 * CourtQty)));
                if (comdate == 0)
                {
                    comdate = 1;
                }

                matchdate = Convert.ToInt32(comdate);
            }
            _Suggest = "推荐方案：建议至少分配" + CourtQty + "片场地，安排"+matchdate+"个比赛日期";
            return _Suggest;
        }

        /// <summary>
        /// is match resource distributed
        /// </summary>
        /// <param name="_TourSys"></param>
        /// <returns></returns>
        public bool IsResourceDistied(string _TourSys)
        {
            string sql = "select * from wtf_match where toursys='"+_TourSys+"' and courtid is null";
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

        /// <summary>
        /// 获取项目的轮次
        /// </summary>
        /// <param name="_ContentId"></param>
        /// <returns></returns>
        public List<WeMatchModel> GetDistinctRoundbyCont(string _ContentId)
        {
            List<WeMatchModel> round_list = new List<WeMatchModel>();
            string sql = "select distinct(round) from wtf_match where contentId='"+_ContentId+"'";
            DataTable dt = DbHelperSQL.Query(sql).Tables[0];
            round_list = JsonHelper.ParseDtModelList<List<WeMatchModel>>(dt);
            return round_list;
        }

        /// <summary>
        /// 修改比赛资源
        /// 2016-09-14，刘涛，
        /// </summary>
        /// <param name="_Date"></param>
        /// <param name="_Court"></param>
        public void UpdateMatchResource(string _Sys,string _Date, string _Court)
        { 
            WeMatchModel match=WeMatchDll.instance.GetModel(_Sys);
            int i=CountMatchQtybyDateCourt(match.TOURSYS, _Date, _Court);
            string sql = string.Format("update wtf_match set matchdate='{0}',place='{1}',CourtId='{2}' where sys='{3}'",_Date,(i+1).ToString(),_Court,_Sys);
            int a = DbHelperSQL.ExecuteSql(sql);
        }

        public void Clear_Resource(string _TourSys)
        {
            string sql = "update wtf_match set matchdate='',place='',courtId='' where toursys='"+_TourSys+"'";
            int a = DbHelperSQL.ExecuteSql(sql);
        }
        #endregion

        #region Match Umpire

        /// <summary>
        /// 在页面上修改比分
        /// 2016-09-26
        /// </summary>
        /// <param name="model"></param>
        public void RecoreMatchScore(WeMatchModel model)
        {
            if (UpdateMatchScore(model))
            {
                AddNextMatch(model.SYS);
            }
        }

        public void RecoreMatchScore2(string sys,string p1s,string p2s)
        {
            //更新比分
            WeMatchModel match = WeMatchDll.instance.GetModel(sys);

            match.SCORE = p1s + p2s;
            int scoreP1 = Convert.ToInt32(p1s);
            int scoreP2 = Convert.ToInt32(p2s);
            if (scoreP1 > scoreP2)
            {
                match.WINNER = match.PLAYER1;
                match.LOSER = match.PLAYER2;
            }
            else
            {
                match.LOSER = match.PLAYER1;
                match.WINNER = match.PLAYER2;
            }

            match.STATE = 2;

            if (UpdateMatchScore(match))
            {
                AddNextMatch(match.SYS);
            }
        }

        /// <summary>
        /// 修改全场比赛的比分
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public bool UpdateMatchScore(WeMatchModel model)
        {
            string sql = string.Format("Update wtf_match set winner='{0}',loser='{1}',score='{2}',state='{3}' where sys='{4}'",model.WINNER,model.LOSER,model.SCORE,model.STATE,model.SYS);
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
        /// 比赛结束的时候，添加到下一轮比赛
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public void UpdateRegularMatch(WeMatchModel model)
        {
            AddNextMatch(model.SYS);
        }

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
                WeMatchModel model = GetModel(_Matchsys);
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

        public void AddNextMatch(string Matchsys)
        {
            WeMatchModel mmodel = GetModel(Matchsys);
            if (mmodel.ROUND != 0)
            {
                //淘汰赛
                //比赛指定下一场去向更新比赛
                if (!string.IsNullOrEmpty(mmodel.winto))
                {
                    WeMatchModel Nmodel = GetMatchbyMatchOrder(mmodel.ContentID, mmodel.winto);
                    UpdateNextMatchPlayer(Nmodel.SYS, mmodel.WINNER, mmodel.etc3);                    

                    //为下一轮比赛添加赔率
                    try
                    {
                        //判断比赛选手是否已齐全
                        string sql = "select * from wtf_match where sys='" + Nmodel.SYS + "' and Player1 is not null and player2 is not null";
                        DataTable dt = DbHelperSQL.Query(sql).Tables[0];
                        if (dt.Rows.Count > 0)
                        {
                            //betDll.Get_Instance().InsertNewBet(Nmodel.SYS, "胜负");
                        }
                    }
                    catch
                    {

                    }
                }
                //指定本场比赛的失利方到下一场比赛，如果已制定比赛losto
                if (!string.IsNullOrEmpty(mmodel.loseto))
                {
                    WeMatchModel Nmodel = GetMatchbyMatchOrder(mmodel.ContentID, mmodel.loseto);
                    UpdateNextMatchPlayer(Nmodel.SYS, mmodel.LOSER, mmodel.etc3);
                }
            }
            else
            {
                if (mmodel.PLAYTYPE == "1")
                {
                    //比赛是俱乐部临时比赛，不做晋级处理，只是增加积分 
                    WeTourModel tmodel = WeTourDll.instance.GetModelbySys(mmodel.TOURSYS);
                    string ptype = "";
                    if (mmodel.PLAYER1.IndexOf(",") > 0)
                    {
                        ptype = "双打";
                        //RPointDll.instance.AddRankPoint("Club", tmodel.MGRSYS, "5", "", mmodel.WINNER.Split(',')[0], "俱乐部挑战赛获胜积分奖励", mmodel.ContentID, mmodel.TOURSYS);
                        //RPointDll.instance.AddRankPoint("Club", tmodel.MGRSYS, "5", "", mmodel.WINNER.Split(',')[1], "俱乐部挑战赛获胜积分奖励", mmodel.ContentID, mmodel.TOURSYS);
                        //RPointDll.instance.AddRankPoint("Club", tmodel.MGRSYS, "1", "", mmodel.LOSER.Split(',')[0], "俱乐部挑战赛失败积分奖励", mmodel.ContentID, mmodel.TOURSYS);
                        //RPointDll.instance.AddRankPoint("Club", tmodel.MGRSYS, "1", "", mmodel.LOSER.Split(',')[1], "俱乐部挑战赛失败积分奖励", mmodel.ContentID, mmodel.TOURSYS);
                    }
                    else
                    {
                        ptype = "单打";
                        //RPointDll.instance.AddRankPoint("Club", tmodel.MGRSYS, "5", "", mmodel.WINNER, "俱乐部挑战赛获胜积分奖励", mmodel.ContentID, mmodel.TOURSYS);
                        //RPointDll.instance.AddRankPoint("Club", tmodel.MGRSYS, "1", "", mmodel.LOSER, "俱乐部挑战赛失败积分奖励", mmodel.ContentID, mmodel.TOURSYS);
                    }
                }
                else
                {
                    #region 处理小组赛
                    //小组赛
                    //小组赛，根据比赛回推小组赛组别
                    int GroupId = GetGroupId(mmodel.PLAYER1, mmodel.ContentID);
                    //查看比赛是否已全部完成
                    if (IsGroupMatchesEnd(mmodel.ContentID, GroupId))
                    {
                        //小组赛比赛已全部完成
                        string Quolifier = "";
                        //根据小组赛类型来进行判断
                        WeTourContModel tcmodel = WeTourContentDll.instance.GetModelbyId(mmodel.ContentID);
                        string _Matchorder = GetGroupWinto(mmodel.ContentID, GroupId.ToString());
                        switch (tcmodel.GroupType)
                        {
                            case "2":
                                //资格赛
                                //AddNextRoundMatch(mmodel);
                                break;
                            case "3":
                                //三人制小组赛                            
                                //获取小组排名
                                DataTable dt = WeContentSignsDll.instance.SortGroup(mmodel.ContentID, GroupId.ToString());
                                Quolifier = dt.Rows[0]["membersys"].ToString();//获取头名memberid

                                WeMatchModel Nmodel = GetMatchbyMatchOrder(mmodel.ContentID, _Matchorder);
                                UpdateNextMatchPlayer(Nmodel.SYS, Quolifier, mmodel.etc3);
                                break;
                            case "4":
                                //四人制小组赛
                                DataTable dt1 = WeContentSignsDll.instance.SortGroup(mmodel.ContentID, GroupId.ToString());
                                if (_Matchorder.IndexOf(",") > 0)
                                {
                                    //有两人晋级
                                    string[] MatchOrders = _Matchorder.Split(',');
                                    Quolifier = dt1.Rows[0]["membersys"].ToString();//获取头名memberid
                                    string Quolifier2 = dt1.Rows[1]["membersys"].ToString();//获取头名memberid
                                    //小组头名进入对应组号的比赛player1
                                    WeMatchModel Nmodel1 = GetMatchbyMatchOrder(mmodel.ContentID, MatchOrders[0]);
                                    UpdateNextMatchPlayer(Nmodel1.SYS, Quolifier, mmodel.etc3);

                                    //小组次名进入另外一场比赛
                                    WeMatchModel Nmodel2 = GetMatchbyMatchOrder(mmodel.ContentID, MatchOrders[1]);
                                    UpdateNextMatchPlayer(Nmodel2.SYS, Quolifier, mmodel.etc3);
                                }
                                else
                                {
                                    //只有小组头名晋级
                                    Quolifier = dt1.Rows[0]["membersys"].ToString();//获取头名memberid

                                    //小组头名进入对应组号的比赛player1
                                    WeMatchModel Nmodel1 = GetMatchbyMatchOrder(mmodel.ContentID, _Matchorder);
                                    UpdateNextMatchPlayer(Nmodel1.SYS, Quolifier, mmodel.etc3);
                                }
                                break;

                            case "5":
                                //五人制小组赛
                                DataTable dt2 = WeContentSignsDll.instance.SortGroup(mmodel.ContentID, GroupId.ToString());
                                Quolifier = dt2.Rows[0]["membersys"].ToString();//获取头名memberid

                                //小组头名进入对应组号的比赛player1
                                WeMatchModel Nmodel5 = GetMatchbyMatchOrder(mmodel.ContentID, _Matchorder);
                                UpdateNextMatchPlayer(Nmodel5.SYS, Quolifier, mmodel.etc3);
                                break;
                        }
                    }
                    #endregion
                }
            }
        }

        //public void UpdateMatchScore()

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
        #endregion

        #endregion


        #region 后台管理，比赛展示

        /// <summary>
        /// 根据赛事内容和比赛状态获得比赛列表
        /// </summary>
        /// <param name="Contentid"></param>
        /// <param name="_State"></param>
        /// <returns></returns>
        public List<Model_TgmMatch> GetMatchesByContentState(string Contentid)
        {
            List<Model_TgmMatch> list = new List<Model_TgmMatch>();
             WeTourContModel tcmodel = WeTourContentDll.instance.GetModelbyId(Contentid);
            //首先根据Contentid,获取所有的轮次
            string sql1 = "select distinct(round) from wtf_match where contentid='" + Contentid + "'";
            DataTable dt1 = DbHelperSQL.Query(sql1).Tables[0];
            if (dt1.Rows.Count > 0)
            {
                foreach (DataRow dr1 in dt1.Rows)
                {
                    Model_TgmMatch model = new Model_TgmMatch();
                    int RoundNum = Convert.ToInt32(dr1[0].ToString());
                    int Cap = Convert.ToInt32(tcmodel.SignQty);
                    model.RoundName = WeTourDll.instance.RenderRound(RoundNum, Cap);
                    List<Model_TgmRound> trs = new List<Model_TgmRound>();
                    //获取该轮次的比赛
                    string sql = "";
                    sql = "select * from wtf_match where ContentId='" + Contentid + "' and Round='" + dr1[0].ToString() + "'  order by matchorder desc";
                    
                    DataTable dt = DbHelperSQL.Query(sql).Tables[0];
                    if (dt.Rows.Count > 0)
                    {
                        foreach (DataRow dr in dt.Rows)
                        {
                            Model_TgmRound tr = new Model_TgmRound();
                            try
                            {
                                tr.matchOrder = dr["matchorder"].ToString();
                                //补全运动员姓名
                                string _P1Name = "";
                                string _P2Name = "";

                                if (tcmodel.ContentType == "团体")
                                {
                                    _P1Name = weTeamdll.instance.GetModel(dr["player1"].ToString()).TEAMNAME;
                                    _P2Name = weTeamdll.instance.GetModel(dr["player2"].ToString()).TEAMNAME;

                                }
                                else
                                {
                                    if (tcmodel.ContentType.IndexOf("双") > 0)
                                    {
                                        //double
                                        string[] p1 = dr["player1"].ToString().Split(',');
                                        _P1Name = WeMemberDll.instance.GetModel(p1[0]).USERNAME + "/" + WeMemberDll.instance.GetModel(p1[1]).USERNAME;
                                        string[] p2 = dr["player2"].ToString().Split(',');
                                        _P2Name = WeMemberDll.instance.GetModel(p2[0]).USERNAME + "/" + WeMemberDll.instance.GetModel(p2[1]).USERNAME;

                                    }
                                    else
                                    {
                                        //single
                                        _P1Name = WeMemberDll.instance.GetModel(dr["player1"].ToString()).USERNAME;
                                        _P2Name = WeMemberDll.instance.GetModel(dr["player2"].ToString()).USERNAME;
                                    }
                                }
                                if (!string.IsNullOrEmpty(_P1Name))
                                {
                                    tr.p1name = _P1Name;
                                }
                                else
                                {
                                    tr.p1name = "未知";
                                }
                                if (!string.IsNullOrEmpty(_P2Name))
                                {
                                    tr.p2name = _P2Name;
                                }
                                else
                                {
                                    tr.p2name = "未知";
                                }
                            }
                            catch
                            {
                                tr.p1name = "未知";
                                tr.p2name = "未知";
                            }
                            trs.Add(tr);
                        }
                    }
                    model.RoundMatch = trs;
                    list.Add(model);
                }//end of foreach

            }//end of if

            
            return list;
        }
        /// <summary>
        /// 获取赛事秩序册的实例列表
        /// </summary>
        /// <param name="_Toursys"></param>
        /// <returns></returns>
        public List<Model_CourtMatches> GetCourtMatchs(string _Toursys)
        {
            List<Model_CourtMatches> list = new List<Model_CourtMatches>();
            //获取比赛日期
            List<WeMatchModel> datelist = GetDateofMatches(_Toursys);
            foreach (WeMatchModel date in datelist)
            {
                Model_CourtMatches model = new Model_CourtMatches();
                //根据比赛日期获取比赛数量和日期内的比赛
                model.matchDate = date.MATCHDATE;
                model.dateMatchQty = CountMatchQtybyDate(_Toursys, date.MATCHDATE).ToString();
                List<Model_DateMatches> dateCoutlist = new List<Model_DateMatches>();
                List<WeMatchModel> courtlist = GetCourtIdsbyMatchDate(_Toursys, date.MATCHDATE);
                foreach (WeMatchModel dateCourt in courtlist)
                {
                    Model_DateMatches DateMatch = new Model_DateMatches();
                    DateMatch.courtName = CourtDll.Get_Instance().GetCourtNoByID(dateCourt.COURTID);
                    DateMatch.courtMatchQty = CountMatchQtybyDateCourt(_Toursys, date.MATCHDATE, dateCourt.COURTID).ToString();
                    //添加详细的比赛情况
                    List<Model_courtDateMatch> courtDateMatchList = new List<Model_courtDateMatch>();
                    List<WeMatchModel> courtDateMatches = GetMatchesbyCourtDate(_Toursys, date.MATCHDATE, dateCourt.COURTID);
                    foreach (WeMatchModel match in courtDateMatches)
                    {
                        Model_courtDateMatch courtDateMatch = new Model_courtDateMatch();
                        courtDateMatch.matchSys = match.SYS;
                        courtDateMatch.courtMatchOrder = match.PLACE;
                        courtDateMatch.matchOrder = match.matchorder;
                        string Desc = "";
                        WeTourContModel cont = WeTourContentDll.instance.GetModelbyId(match.ContentID);
                        Desc += cont.TourDate + cont.ContentName;
                        Desc += "-"+match.RoundName;
                        if (match.ROUND == 0)
                        {
                            Desc += "-第" + match.etc1 + "组";
                        }
                        courtDateMatch.matchDesc = Desc;
                        courtDateMatch.player1 = match.Player1ComName;
                        courtDateMatch.player2 = match.Player2ComName;                        

                        courtDateMatchList.Add(courtDateMatch);
                    }//end of foreach court matches
                    DateMatch.courtMatches = courtDateMatchList;
                    dateCoutlist.Add(DateMatch);
                }//end of foreach date courts
                model.dateMatches = dateCoutlist;
                list.Add(model);
            }//end of foreach dates
            return list;
        }

        /// <summary>
        /// 获取赛程列表
        /// </summary>
        /// <param name="_eventId"></param>
        /// <param name="_date"></param>
        /// <param name="_placeId"></param>
        /// <returns></returns>
        public List<Fe_Model_Schedule> GetEventSchedule(string _eventId, string _date, string _placeId)
        {
            List<Fe_Model_Schedule> schedule_list = new List<Fe_Model_Schedule>();
            //获取数据
            List<WeMatchModel> courtDateMatches = GetMatchesbyCourtDate(_eventId, _date, _placeId);
            foreach (WeMatchModel match in courtDateMatches)
            {
                Fe_Model_Schedule courtDateMatch = new Fe_Model_Schedule();
                courtDateMatch.matchId = match.SYS;
                courtDateMatch.matches = "第"+match.PLACE+"场";

                string Desc = "";
                WeTourContModel cont = WeTourContentDll.instance.GetModelbyId(match.ContentID);
                Desc += cont.TourDate +" | "+ cont.ContentName;
                Desc += " | " + match.RoundName;
                if (match.ROUND == 0)
                {
                    Desc += " | 第" + match.etc1 + "组";
                }
                courtDateMatch.group = Desc;

                courtDateMatch.gameTime = "";
                courtDateMatch.date = _date;
                courtDateMatch.location = _placeId;

                //添加team
                #region 添加team 内容
                List<Fe_Model_Schedule_team> team_list = new List<Fe_Model_Schedule_team>();
                Fe_Model_Schedule_team team1 = new Fe_Model_Schedule_team();
                #region 添加player1
                if (cont.ContentType == "团体")
                {
                    team1.users = GetGroupTeambySys(match.PLAYER1);//添加团体
                }
                else
                {
                    team1.users = GetUsersbySys(match.PLAYER1);//添加用户
                }

                //是否获胜
                if (match.PLAYER1 == match.WINNER && match.WINNER!="")
                {
                    team1.win = true;
                }
                else
                {
                    team1.win = false;
                }

                //获取分数
                try
                {
                    team1.score = Convert.ToInt32(match.SCORE.Substring(0, 1));
                }
                catch (Exception)
                {
                    team1.score = 0;
                }

                team1.winGameNumber = "";
                team1.currentScore = "";
                team1.currentScoreWin = false;
                team1.first = false;

                #endregion
                team_list.Add(team1);


                Fe_Model_Schedule_team team2 = new Fe_Model_Schedule_team();
                #region 添加player2
                if (cont.ContentType == "团体")
                {
                    team2.users = GetGroupTeambySys(match.PLAYER2);//添加团体
                }
                else
                {
                    team2.users = GetUsersbySys(match.PLAYER2);
                }
                //是否获胜
                if (match.PLAYER2 == match.WINNER && match.WINNER != "")
                {
                    team2.win = true;
                }
                else
                {
                    team2.win = false;
                }

                //获取分数
                try
                {
                    team2.score = Convert.ToInt32(match.SCORE.Substring(1, 1));
                }
                catch (Exception)
                {
                    team2.score = 0;
                }

                team2.winGameNumber = "";
                team2.currentScore = "";
                team2.currentScoreWin = false;
                team2.first = false;
                #endregion
                team_list.Add(team2);

                courtDateMatch.team = team_list;
                #endregion

                schedule_list.Add(courtDateMatch);
            }//end of foreach court matches
            return schedule_list;
        }

        /// <summary>
        /// 获取比赛结果
        /// </summary>
        /// <param name="ItemId"></param>
        /// <returns></returns>
        public List<Fe_Model_Result> GeteventResults(string ItemId,string status)
        {
            List<Fe_Model_Result> res_list = new List<Fe_Model_Result>();
            //获取项目已完成的比赛
            List<WeMatchModel> match_list = WeMatchDll.instance.GetContMachthByStatus(ItemId,status);
            foreach (WeMatchModel match in match_list)
            {
                Fe_Model_Result courtDateMatch = new Fe_Model_Result();
                courtDateMatch.matches = "第" + match.PLACE + "场";
                courtDateMatch.id = match.SYS;
                string Desc = "";
                WeTourContModel cont = WeTourContentDll.instance.GetModelbyId(match.ContentID);
                Desc += cont.TourDate + "|" + cont.ContentName;
                Desc += "|" + match.RoundName;
                if (match.ROUND == 0)
                {
                    Desc += "|第" + match.etc1 + "组";
                }
                courtDateMatch.group = Desc;

                courtDateMatch.gameTime = "";
                courtDateMatch.type = 1;
                courtDateMatch.status = 2;

                //添加team
                #region 添加team 内容
                List<Fe_Model_Schedule_team> team_list = new List<Fe_Model_Schedule_team>();
                Fe_Model_Schedule_team team1 = new Fe_Model_Schedule_team();
                #region 添加player1
                team1.users = GetUsersbySys(match.PLAYER1);//添加用户
                //是否获胜
                if (match.PLAYER1 == match.WINNER && match.WINNER != "")
                {
                    team1.win = true;
                }
                else
                {
                    team1.win = false;
                }

                //获取分数
                try
                {
                    team1.score = Convert.ToInt32(match.SCORE.Substring(0, 1));
                }
                catch (Exception)
                {
                    team1.score = 0;
                }

                team1.winGameNumber = "";
                team1.currentScore = "0";
                team1.currentScoreWin = false;
                team1.first = false;

                #endregion
                team_list.Add(team1);


                Fe_Model_Schedule_team team2 = new Fe_Model_Schedule_team();
                #region 添加player2
                team2.users = GetUsersbySys(match.PLAYER2);
                //是否获胜
                if (match.PLAYER2 == match.WINNER && match.WINNER != "")
                {
                    team2.win = true;
                }
                else
                {
                    team2.win = false;
                }

                //获取分数
                try
                {
                    team2.score = Convert.ToInt32(match.SCORE.Substring(1, 1));
                }
                catch (Exception)
                {
                    team2.score = 0;
                }

                team2.winGameNumber = "";
                team2.currentScore = "0";
                team2.currentScoreWin = false;
                team2.first = false;
                #endregion
                team_list.Add(team2);

                courtDateMatch.team = team_list;
                #endregion

                res_list.Add(courtDateMatch);
            }//end of foreach court matches
            return res_list;
        }

        private List<Dictionary<string, string>> GetGroupTeambySys(string _Sysno)
        {
            List<Dictionary<string, string>> user_list = new List<Dictionary<string, string>>();
            if (string.IsNullOrEmpty(_Sysno))
            {
                Dictionary<string, string> item = new Dictionary<string, string>();
                item.Add("username", "未知");
                item.Add("userimage", "");

                user_list.Add(item);
            }
            else
            {
                Dictionary<string, string> item = new Dictionary<string, string>();
                string teamname = weTeamdll.instance.GetModel(_Sysno).TEAMNAME;
                item.Add("username", teamname);
                item.Add("userimage", "");
                user_list.Add(item);
            }

            return user_list;
        }

        private List<Dictionary<string, string>> GetUsersbySys(string _Sysno)
        {
            List<Dictionary<string, string>> user_list = new List<Dictionary<string, string>>();
            if (string.IsNullOrEmpty(_Sysno))
            {
                Dictionary<string, string> item = new Dictionary<string, string>();
                item.Add("username", "未知");
                item.Add("userimage", "");

                user_list.Add(item);
            }
            else
            { 
                //判断单双打
                if (_Sysno.IndexOf(",") > 0)
                {
                    //双打
                }
                else
                { 
                    //单打
                    WeMemberModel mem = WeMemberDll.instance.GetModel(_Sysno);
                    Dictionary<string, string> item = new Dictionary<string, string>();
                    item.Add("username", mem.USERNAME);
                    item.Add("userimage", mem.EXT1);

                    user_list.Add(item);
                }
            }
            return user_list;
        }

        /// <summary>
        /// 获取比赛日期列表
        /// </summary>
        /// <param name="_Toursys"></param>
        /// <returns></returns>
        public List<WeMatchModel> GetDateofMatches(string _Toursys)
        {
            List<WeMatchModel> list = new List<WeMatchModel>();
            string sql = "select distinct(matchDate) from wtf_match where toursys='" + _Toursys + "'";
            DataTable dt = DbHelperSQL.Query(sql).Tables[0];
            if (dt.Rows.Count>0)
            {
                list = JsonHelper.ParseDtModelList<List<WeMatchModel>>(dt);
            }
            return list;
        }

        /// <summary>
        /// 获取比赛数量
        /// </summary>
        /// <param name="_TourSys"></param>
        /// <param name="_Matchdate"></param>
        /// <returns></returns>
        private int CountMatchQtybyDate(string _TourSys, string _Matchdate)
        {
            string sql = "select sys from wtf_match where toursys='" + _TourSys + "' and matchdate='" + _Matchdate + "'";
            DataTable dt = DbHelperSQL.Query(sql).Tables[0];
            return dt.Rows.Count;
        }

        /// <summary>
        /// 获取指定日期，指定场地的比赛数量
        /// </summary>
        /// <param name="_TourSys"></param>
        /// <param name="_MatchDate"></param>
        /// <param name="_CourtId"></param>
        /// <returns></returns>
        private int CountMatchQtybyDateCourt(string _TourSys, string _MatchDate, string _CourtId)
        {
            string sql = "select sys from wtf_match where toursys='" + _TourSys + "' and matchdate='" + _MatchDate + "' and courtid='"+_CourtId+"'";
            DataTable dt = DbHelperSQL.Query(sql).Tables[0];
            return dt.Rows.Count;
        }

        private List<WeMatchModel> GetCourtIdsbyMatchDate(string _TourSys, string _MatchDate)
        {
            List<WeMatchModel> list = new List<WeMatchModel>();
            string sql = "select distinct(courtid) from wtf_match where toursys='"+_TourSys+"' and matchdate='"+_MatchDate+"'";
            DataTable dt = DbHelperSQL.Query(sql).Tables[0];
            if (dt.Rows.Count > 0)
            {
                list = JsonHelper.ParseDtModelList<List<WeMatchModel>>(dt);
            }
                return list;
        }
        #endregion


        #region Lyon
        /// <summary>
        /// Assign match date when add match date
        /// </summary>
        /// <param name="toursys"></param>
        /// <param name="date"></param>
        /// <param name="round"></param>
        public bool AssignMatchDate(string _TourSys, string _Date, string _Round)
        {
            int round = -999999;
            int.TryParse(_Round,out round);
            StringBuilder sql = new StringBuilder();
            sql.Append("update wtf_match set matchdate='" + _Date + "' where toursys='" + _TourSys +"'");
            if (round < 0)
            {
                sql.Append(" and etc2='" + Math.Abs(round) + "' and round=0");
            }
            else
            {
                sql.Append(" and round='" + _Round + "'");
            }
            int a = DbHelperSQL.ExecuteSql(sql.ToString());
            return a > 0 ? true : false;
        }
        #endregion
    }
}
