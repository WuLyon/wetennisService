using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using WeTour;
using Member;
using Club;

namespace Ranking
{
    public class RankDll
    {
        public static RankDll instance = new RankDll();

        /// <summary>
        ///  Insert New Rank Record
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public bool InsertNewRank(RankModel model)
        {
            string sql = string.Format("insert into rank_rank (Memsys,RankType,TypeSys,Rank,IsSingle,Gender,GroupType,Ryear,RWeek,UpdateDate,status,RankPoint) values ('{0}','{1}','{2}','{3}','{4}','{5}','{6}','{7}','{8}','{9}','{10}','{11}')", model.MemSys, model.RankType, model.TypeSys, model.Rank, model.IsSingle, model.Gender, model.GroupType, model.Ryear, model.Rweek, DateTime.Now.ToString(), "1",model.RankPoint);
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

        #region Wetennis Ranking
        /// <summary>
        /// Update Ranking of Wetennis.cn 
        /// </summary>
        public void UpdateWetennisRank()
        { 
            //Update Past Ranking Data
            string sql = "update rank_rank set status=0 where ranktype=''";
            DbHelperSQL.ExecuteSql(sql);
            //Men's Single
            UpdateWetennisRankbyType("单打", "男");
            //Women's Single
            UpdateWetennisRankbyType("单打", "女");
            //Men's Double
            UpdateWetennisRankbyType("双打", "男");
            //Women's Double
            UpdateWetennisRankbyType("双打", "女");
        }

        /// <summary>
        /// update wetennis rank
        /// </summary>
        /// <param name="_IsSingle"></param>
        /// <param name="_Gender"></param>
        private void UpdateWetennisRankbyType(string _IsSingle, string _Gender)
        {            
            string sql1 = "select sum(points) as points,Memsys from rank_points where Issingle='" + _IsSingle + "' and pointType='' and Gender='" + _Gender + "' group by memsys order by sum(points) desc";
            DataTable dt1 = DbHelperSQL.Query(sql1).Tables[0];
            if (dt1.Rows.Count > 0)
            {
                for (int i = 0; i < dt1.Rows.Count; i++)
                {
                    RankModel model = new RankModel();
                    model.MemSys = dt1.Rows[i]["Memsys"].ToString();
                    model.RankPoint = dt1.Rows[i]["points"].ToString();
                    model.Rank = (i + 1).ToString();
                    model.IsSingle = _IsSingle;
                    model.Gender = _Gender;
                    model.Ryear = DateTime.Now.Year.ToString();
                    model.Rweek = getDayWeekthofYear(DateTime.Now.ToString());

                    InsertNewRank(model);
                }
            }
        }
        #endregion

        #region SocialPoint Ranking
        /// <summary>
        /// update Social Rank
        /// </summary>
        public void UpdateSocialRank()
        {
            //Update Past Ranking Data
            string sql = "update rank_rank set status=0 where ranktype='社区'";
            DbHelperSQL.ExecuteSql(sql);
            UpdateSocialRankbyPoint();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="_IsSingle"></param>
        /// <param name="_Gender"></param>
        private void UpdateSocialRankbyPoint()
        {
            string sql1 = "select sum(points) as points,Memsys from rank_points where  pointType='社区' group by memsys order by sum(points) desc";
            DataTable dt1 = DbHelperSQL.Query(sql1).Tables[0];
            if (dt1.Rows.Count > 0)
            {
                for (int i = 0; i < dt1.Rows.Count; i++)
                {
                    RankModel model = new RankModel();
                    model.MemSys = dt1.Rows[i]["Memsys"].ToString();
                    model.RankPoint = dt1.Rows[i]["points"].ToString();
                    model.Rank = (i + 1).ToString();
                    model.RankType = "社区";
                    model.Ryear = DateTime.Now.Year.ToString();
                    model.Rweek = getDayWeekthofYear(DateTime.Now.ToString());

                    InsertNewRank(model);
                }
            }
        }
        #endregion

        #region Club Ranking
        /// <summary>
        /// Update Club Ranking
        /// </summary>
        /// <param name="_ClubSys"></param>
        public void UpdateClubRanking(string _ClubSys)
        {
            //Update Past Ranking Data
            string sql = "update rank_rank set status=0 where ranktype='club' and typesys='"+_ClubSys+"'";
            DbHelperSQL.ExecuteSql(sql);

            //single
            UpdateClubRankbyType(_ClubSys, "单打");
            //double
            UpdateClubRankbyType(_ClubSys, "双打");

        }

        /// <summary>
        /// Update Ranking Of Club by type single or double
        /// </summary>
        /// <param name="_ClubSys"></param>
        private void UpdateClubRankbyType(string _ClubSys,string _IsSingle)
        {
            string sql1 = "select sum(points) as points,Memsys from rank_points  where Issingle='" + _IsSingle + "'  and Memsys in (select memsys from wtf_clubMember where Clubsys='" + _ClubSys + "')and (pointType='' or pointType='club' and TypeSys='" + _ClubSys + "') group by memsys order by sum(points) desc";
            DataTable dt1 = DbHelperSQL.Query(sql1).Tables[0];
            if (dt1.Rows.Count > 0)
            {
                for (int i = 0; i < dt1.Rows.Count; i++)
                {
                    RankModel model = new RankModel();
                    model.RankType = "club";
                    model.TypeSys = _ClubSys;
                    model.MemSys = dt1.Rows[i]["Memsys"].ToString();
                    model.RankPoint = dt1.Rows[i]["points"].ToString();
                    model.Rank = (i + 1).ToString();
                    model.IsSingle = _IsSingle;
                    model.Ryear = DateTime.Now.Year.ToString();
                    model.Rweek = getDayWeekthofYear(DateTime.Now.ToString());

                    InsertNewRank(model);
                }
            }
        }
        #endregion

        #region 更新联盟排名

        /// <summary>
        /// 更新联盟的排名
        /// </summary>
        /// <param name="_UnionSys"></param>
        public void UpdateUnionRanking(string _UnionSys)
        {
            //先删除此前的排名
            string DelSql = "delete rank_rank where RankType='Union' and TypeSys='" + _UnionSys + "'";
            int a = DbHelperSQL.ExecuteSql(DelSql);

            //根据UnionSys获取对应的赛事主键
            string sql = "select * from wtf_citytour where unionsys='" + _UnionSys + "'";
            DataTable dt = DbHelperSQL.Query(sql).Tables[0];
            string toursys = "";
            if (dt.Rows.Count > 0)
            {
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    toursys += "'" + dt.Rows[i]["sysno"].ToString() + "',";
                }
                toursys = toursys.TrimEnd(',');
            }
          

            //根据toursys 获取类型
            string sql1 = "select distinct(ContentType) from wtf_tourcontent where toursys in ("+toursys+")";
            DataTable dt1 = DbHelperSQL.Query(sql1).Tables[0];
            
            foreach (DataRow dr in dt1.Rows)
            {
                UpdateUnionRankbyType(_UnionSys, dr["ContentType"].ToString());
            }
            
        }

        /// <summary>
        /// 更新联盟排名
        /// </summary>
        /// <param name="_UnionSys"></param>
        /// <param name="_Type"></param>
        private void UpdateUnionRankbyType(string _UnionSys, string _Type)
        {           
            //根据UnionSys获取对应的赛事主键
            string sql = "select * from wtf_citytour where unionsys='"+_UnionSys+"'";
            DataTable dt = DbHelperSQL.Query(sql).Tables[0];
            string toursys = "";
            if (dt.Rows.Count > 0)
            {
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    toursys += "'"+dt.Rows[i]["sysno"].ToString() + "',";
                }
                toursys = toursys.TrimEnd(',');
            }

            //根据赛事主键，获取对应的积分
            string _IsSingle="";
            string _Gender="";
            _IsSingle = _Type.Substring(1, 1) + "打";
            _Gender = _Type.Substring(0, 1);
            string sql1 = "select sum(points) as points,Memsys from rank_points  where Issingle='" + _IsSingle + "' and Gender='" + _Gender + "' and toursys in (" + toursys + ")  group by memsys order by sum(points) desc";
            DataTable dt1 = DbHelperSQL.Query(sql1).Tables[0];
            if (dt1.Rows.Count > 0)
            {
                for (int i = 0; i < dt1.Rows.Count; i++)
                {
                    RankModel model = new RankModel();
                    model.RankType = "union";
                    model.TypeSys = _UnionSys;
                    model.MemSys = dt1.Rows[i]["Memsys"].ToString();
                    model.RankPoint = dt1.Rows[i]["points"].ToString();
                    model.Rank = (i + 1).ToString();
                    model.IsSingle = _IsSingle;
                    model.Gender = _Gender;
                    model.Ryear = DateTime.Now.Year.ToString();
                    model.Rweek = getDayWeekthofYear(DateTime.Now.ToString());
                    model.GroupType = "青年组";//添加组别
                    InsertNewRank(model);
                }
            }
        }

        #endregion

        #region ChengDu Tennis Association Ranking
        /// <summary>
        /// the all in one method to update ChengDu ranking
        /// </summary>
        public void UpdateChengDuAssRank()
        {
            //to disable previous rank data
            string sql = "update rank_rank set status=0 where ranktype='chengdu'";
            int a = DbHelperSQL.ExecuteSql(sql);

            //create new rank data
            UpdateChengDubyType("单打", "男");
            UpdateChengDubyType("双打", "男");
            UpdateChengDubyType("单打", "女");
            UpdateChengDubyType("双打", "女");
        }

        /// <summary>
        /// update chengdu rank by type
        /// </summary>
        /// <param name="_IsSingle"></param>
        /// <param name="_Gender"></param>
        public void UpdateChengDubyType(string _IsSingle,string _Gender)
        {
            string sql1 = "select sum(points) as points,Memsys from rank_points where Issingle='" + _IsSingle + "' and pointType='' and Gender='" + _Gender + "' and TourSys in (select sysno from wtf_CityTour where MgrSys='ChengDu' and CityType='') group by memsys order by sum(points) desc";
            DataTable dt1 = DbHelperSQL.Query(sql1).Tables[0];
            if (dt1.Rows.Count > 0)
            {
                for (int i = 0; i < dt1.Rows.Count; i++)
                {
                    RankModel model = new RankModel();
                    model.MemSys = dt1.Rows[i]["Memsys"].ToString();
                    model.RankPoint = dt1.Rows[i]["points"].ToString();
                    model.Rank = (i + 1).ToString();
                    model.IsSingle = _IsSingle;
                    model.Gender = _Gender;
                    model.Ryear = DateTime.Now.Year.ToString();
                    model.Rweek = getDayWeekthofYear(DateTime.Now.ToString());
                    model.RankType = "chengdu";
                    model.GroupType = GetGroupNamebyMemsys(dt1.Rows[i]["Memsys"].ToString());//组别

                    InsertNewRank(model);
                }
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="_Memsys"></param>
        /// <returns></returns>
        private string GetGroupNamebyMemsys(string _Memsys)
        {
            string GroupType = "Default";
            string sql = "select sysno from wtf_CityTour where MgrSys='ChengDu' and CityType=''";//get toursys
            DataTable dt = DbHelperSQL.Query(sql).Tables[0];
            if (dt.Rows.Count > 0)
            {
                string Toursys = dt.Rows[0][0].ToString();
                string sql1 = "select * from wtf_tourapply where toursys='" + Toursys + "' and (memberid='" + _Memsys + "' or paterner='" + _Memsys + "')";
                DataTable dt1 = DbHelperSQL.Query(sql1).Tables[0];
                if (dt1.Rows.Count > 0)
                { 
                    WeTourContModel wmodel=WeTourContentDll.instance.GetModelbyId(dt1.Rows[0]["ContentId"].ToString());
                    string ContentName = wmodel.ContentName;
                    if (ContentName.IndexOf("少年") >= 0)
                    {
                        GroupType = "少年组";
                    }

                    if (ContentName.IndexOf("青年") >= 0)
                    {
                        GroupType = "青年组";
                    }

                    if (ContentName.IndexOf("中年") > 0)
                    {
                        GroupType = "中年组";
                    }

                    if (ContentName.IndexOf("常青") >= 0)
                    {
                        GroupType = "常青组";
                    }

                    if (ContentName.IndexOf("13岁") >= 0)
                    {
                        GroupType = "少年组";
                    }
                }                
            }
            return GroupType;
        }
        #endregion 

        #region 获取周次 what week it is now?
        /// <summary>
        /// 根据time时间获取 改time 为今年的第几个星期
        /// </summary>
        /// <param name="time"></param>
        /// <returns></returns>
        private string getDayWeekthofYear(string strtime)
        {
            int weekth = 2;

            if (!string.IsNullOrEmpty(strtime))
            {
                DateTime time = DateTime.Now;
                bool ret = DateTime.TryParse(strtime, out time);

                int year = time.Year;

                DateTime timefirstDay = Convert.ToDateTime(year.ToString() + "/01/01");

                DateTime timeSecondWeekofMonday = getSecondWeekofMonday(timefirstDay);

                if (timeSecondWeekofMonday.CompareTo(time) > 0)
                {
                    weekth = 1;
                }
                else
                {
                    while (timeSecondWeekofMonday.AddDays(6).CompareTo(time) < 0)
                    {
                        weekth++;
                        timeSecondWeekofMonday = timeSecondWeekofMonday.AddDays(7);
                    }
                }
            }

            return weekth.ToString().PadLeft(2, '0');
        }

        /// <summary>
        /// 获取一年中 第二个礼拜的第一天(周一)
        /// </summary>
        /// <param name="timefirstDay"></param>
        /// <returns></returns>
        private DateTime getSecondWeekofMonday(DateTime timefirstDay)
        {
            DateTime timeret = DateTime.Now;

            if (timefirstDay.DayOfWeek == DayOfWeek.Monday)
            {
                timeret = timefirstDay.AddDays(7);
            }
            else if (timefirstDay.DayOfWeek == DayOfWeek.Tuesday)
            {
                timeret = timefirstDay.AddDays(6);
            }
            else if (timefirstDay.DayOfWeek == DayOfWeek.Wednesday)
            {
                timeret = timefirstDay.AddDays(5);
            }
            else if (timefirstDay.DayOfWeek == DayOfWeek.Thursday)
            {
                timeret = timefirstDay.AddDays(4);
            }
            else if (timefirstDay.DayOfWeek == DayOfWeek.Friday)
            {
                timeret = timefirstDay.AddDays(3);
            }
            else if (timefirstDay.DayOfWeek == DayOfWeek.Saturday)
            {
                timeret = timefirstDay.AddDays(2);
            }
            else if (timefirstDay.DayOfWeek == DayOfWeek.Sunday)
            {
                timeret = timefirstDay.AddDays(1);
            }

            return timeret;
        }

        # endregion

        #region Get Rank
        /// <summary>
        /// Get Wetennis Ranking
        /// </summary>
        /// <param name="_IsSingle"></param>
        /// <param name="_Gender"></param>
        /// <returns></returns>
        public List<RankModel> GetWetennisRankList(string _IsSingle,string _Gender)
        {
            List<RankModel> list = new List<RankModel>();
            string sql = string.Format("select * from rank_rank where status=1 and RankType='' and Issingle='" + _IsSingle + "' and Gender='" + _Gender + "' order by Convert(int,rank)");
            DataTable dt = DbHelperSQL.Query(sql).Tables[0];
            if (dt.Rows.Count > 0)
            {
                list = JsonHelper.ParseDtModelList<List<RankModel>>(dt); 
            }
            return list;
        }

        /// <summary>
        /// get Chengdu Tennis Association tennis rank
        /// </summary>
        /// <param name="_IsSingle"></param>
        /// <param name="_Gender"></param>
        /// <returns></returns>
        public List<RankModel> GetChengDuRankList(string _IsSingle, string _Gender)
        {
            List<RankModel> list = new List<RankModel>();
            string sql = string.Format("select * from rank_rank where status=1 and RankType='chengdu' and Issingle='" + _IsSingle + "' and Gender='" + _Gender + "' order by Convert(int,rank)");
            DataTable dt = DbHelperSQL.Query(sql).Tables[0];
            if (dt.Rows.Count > 0)
            {
                list = JsonHelper.ParseDtModelList<List<RankModel>>(dt);
            }
            return list;
        }



        /// <summary>
        /// Get Club Ranks
        /// </summary>
        /// <param name="_Clubsys"></param>
        /// <param name="_IsSingle"></param>
        /// <returns></returns>
        public List<RankModel> GetClubRankList(string _Clubsys,string _IsSingle)
        {
            List<RankModel> list = new List<RankModel>();
            string sql = string.Format("select * from rank_rank where status=1 and RankType='club' and typesys='" + _Clubsys + "' and Issingle='" + _IsSingle + "' order by Convert(int,rank)");
            DataTable dt = DbHelperSQL.Query(sql).Tables[0];
            if (dt.Rows.Count > 0)
            {
                list = JsonHelper.ParseDtModelList<List<RankModel>>(dt);
            }
            return list;
        }

        /// <summary>
        /// 获得社区积分排名
        /// </summary>
        /// <returns></returns>
        public List<RankModel> GetSocialRanklist()
        {
            List<RankModel> list = new List<RankModel>();
            string sql = "select * from rank_rank where status=1 and RankType='社区' order by Convert(int,rank)";
            DataTable dt = DbHelperSQL.Query(sql).Tables[0];
            if (dt.Rows.Count > 0)
            {
                list = JsonHelper.ParseDtModelList<List<RankModel>>(dt);
            }
            return list;
        }


        /// <summary>
        /// 获取微网球的排名
        /// </summary>
        /// <param name="_Memsys"></param>
        /// <param name="_IsSingle"></param>
        /// <param name="_Gender"></param>
        /// <returns></returns>
        public string GetWetennisRankbyMem(string _Memsys, string _IsSingle, string _Gender)
        {
            string Rank = "暂无排名";
            string sql = string.Format("select * from rank_rank where status=1 and RankType='' and Issingle='" + _IsSingle + "' and Gender='" + _Gender + "' and Memsys='" + _Memsys + "'");
            DataTable dt = DbHelperSQL.Query(sql).Tables[0];
            if (dt.Rows.Count > 0)
            {
                Rank = dt.Rows[0]["Rank"].ToString() + "(" + dt.Rows[0]["RankPoint"].ToString() + "分)";
            }
            return Rank;
        }

        /// <summary>
        /// 获取联盟排名的概略
        /// </summary>
        /// <param name="_UnionSys"></param>
        /// <returns></returns>
        public List<Model_GroupType> GetUnionRankGroups(string _UnionSys)
        {
            List<Model_GroupType> list = new List<Model_GroupType>();
            string sql = "select distinct(GroupType) from rank_rank where typesys='" + _UnionSys + "'";
            DataTable dt = DbHelperSQL.Query(sql).Tables[0];
            for (int i = 0; i < dt.Rows.Count; i++)
            {
                Model_GroupType model = new Model_GroupType();
                model.GroupName = dt.Rows[i][0].ToString();
                //添加contenttype
                string sql1 = "select distinct(Gender+IsSingle) from rank_rank where typesys='" + _UnionSys + "' and grouptype='" + model.GroupName + "'";
                DataTable dt1 = DbHelperSQL.Query(sql1).Tables[0];
                string[] Content = new string[dt1.Rows.Count];
                for(int j=0;j<dt1.Rows.Count;j++)
                {
                    Content[j] = dt1.Rows[j][0].ToString().Substring(0, 2);
                }
                model.ContType = Content;

                list.Add(model);
            }

            return list;
        }

        /// <summary>
        /// 获取组明细
        /// </summary>
        /// <param name="_UnionSys"></param>
        /// <returns></returns>
        public List<Model_GroupDetail> GetUnionGroupDetail(string _UnionSys)
        {
            List<Model_GroupDetail> list = new List<Model_GroupDetail>();
            List<Model_GroupType> listG = GetUnionRankGroups(_UnionSys);
            for (int i = 0; i < listG.Count; i++)
            {
                
                for (int j = 0; j < listG[i].ContType.Length; j++)
                {
                    Model_GroupDetail model = new Model_GroupDetail();
                    model.GroupName = listG[i].GroupName;
                    model.ContType = listG[i].ContType[j];
                    string _IsSingle = model.ContType.Substring(1,1)+"打";
                    string _Gender = model.ContType.Substring(0, 1);
                    model.MemRanking = GetUnionMemRanking(_UnionSys, model.GroupName, _IsSingle, _Gender);
                    list.Add(model);
                }
            }
                return list;
        }

        /// <summary>
        /// 获取排名
        /// </summary>
        /// <param name="_Union"></param>
        /// <returns></returns>
        public List<Model_MemRanking> GetUnionMemRanking(string _UnionSys, string _GroupType, string _IsSingle, string _Gender)
        {
            List<Model_MemRanking> list = new List<Model_MemRanking>();
            string sql = "select * from  rank_rank where TypeSys='" + _UnionSys + "' and GroupType='" + _GroupType + "' and IsSingle='" + _IsSingle + "' and Gender='" + _Gender + "'  order by Rank";
            DataTable dt = DbHelperSQL.Query(sql).Tables[0];
            for (int i = 0; i < dt.Rows.Count; i++)
            {
                Model_MemRanking model = new Model_MemRanking();
                model.Rank = dt.Rows[i]["Rank"].ToString();
                model.Points = dt.Rows[i]["RankPoint"].ToString();
                model.MemSys = dt.Rows[i]["Memsys"].ToString();
                //get member info
                WeMemberModel mem = WeMemberDll.instance.GetModel(model.MemSys);
                model.MemImg = mem.EXT1;
                model.Name = mem.NAME;
                model.UserName = mem.USERNAME;
                //get Club Name
                model.ClubName = ClubMemBiz.instance.GetMemberClub(model.MemSys);
                list.Add(model);
            }
            return list;
        }
        
        #endregion

        
    }
}
