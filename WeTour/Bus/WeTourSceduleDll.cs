using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using Gym;

namespace WeTour
{
    /// <summary>
    /// 赛事签表显示的数据获取
    /// </summary>
    public class WeTourSceduleDll
    {
        public static WeTourSceduleDll instance = new WeTourSceduleDll();

        public List<WeTourScheduleModel> GetTourSchedule(string _Toursys)
        {
            List<WeTourScheduleModel> list = new List<WeTourScheduleModel>();

            //获取赛事的天数
            string sql = "select distinct(matchdate) from wtf_match where toursys='" + _Toursys + "'";
            DataTable dt = DbHelperSQL.Query(sql).Tables[0];
            if (dt.Rows.Count > 0)
            {
                foreach (DataRow dr in dt.Rows)
                {
                    WeTourScheduleModel smodel = new WeTourScheduleModel();
                    smodel.MatchDate = dr[0].ToString();//添加比赛日期

                    //添加赛事
                    List<CourtMatchModel> clist = new List<CourtMatchModel>();
                    string sqlc = "select distinct(courtid) from wtf_match where toursys='" + _Toursys + "' and matchdate='"+dr[0].ToString()+"'";
                    DataTable dt1 = DbHelperSQL.Query(sqlc).Tables[0];
                    if (dt1.Rows.Count > 0)
                    {
                        foreach (DataRow dr1 in dt1.Rows)
                        {
                            CourtMatchModel model = new CourtMatchModel();
                            CourtModel courtM = CourtDll.Get_Instance().GetModelbyId(dr1[0].ToString());
                            model.CourtId = courtM.ID;
                            model.CourtName = courtM.COURTNAME;
                            GymModel gymM = GymDll.instanc.GetmodelbyId(courtM.GYMID);
                            model.Gymid = courtM.GYMID;
                            model.GymName = gymM.GYMNAME;

                            model.CourtMatches = WeMatchDll.instance.GetMatchesbyCourtDate(_Toursys, dr[0].ToString(), model.CourtId);
                            clist.Add(model);
                        }
                    }

                    smodel.CourtMatches = clist;//添加场地比赛列表
                    list.Add(smodel);
                }
            }
            return list;
        }

        /// <summary>
        /// 获取赛事的日期
        /// </summary>
        /// <param name="_TourSys"></param>
        /// <returns></returns>
        public List<Dictionary<string, string>> GetMatchdates(string _TourSys)
        {
            List<Dictionary<string, string>> tourdates = new List<Dictionary<string, string>>();
            string sql = "select distinct(matchdate) from wtf_match where toursys='" + _TourSys + "'";
            DataTable dt = DbHelperSQL.Query(sql).Tables[0];
            if (dt.Rows.Count > 0)
            {
                for(int i=0;i<dt.Rows.Count;i++)
                {
                    Dictionary<string, string> item = new Dictionary<string, string>();
                    if (!string.IsNullOrEmpty(dt.Rows[i][0].ToString()))
                    {
                        item.Add("text", dt.Rows[i][0].ToString());
                        item.Add("value", dt.Rows[i][0].ToString());
                        tourdates.Add(item);
                    }
                }
            }
            return tourdates;
        }

        /// <summary>
        /// 获得赛事场地
        /// </summary>
        /// <param name="_TourSys"></param>
        /// <returns></returns>
        public List<Dictionary<string, string>> GetMatchCourts(string _TourSys,string _MatchDate)
        {
            List<Dictionary<string, string>> tourCourts = new List<Dictionary<string, string>>();
            string sqlc = "select distinct(courtid) from wtf_match where toursys='" + _TourSys + "' and matchdate='" + _MatchDate + "' and courtid<>''";
            DataTable dt1 = DbHelperSQL.Query(sqlc).Tables[0];
            for (int i = 0; i < dt1.Rows.Count; i++)
            {
                Dictionary<string, string> item = new Dictionary<string, string>();
                item.Add("value",dt1.Rows[i][0].ToString());

                CourtModel courtM = CourtDll.Get_Instance().GetModelbyId(dt1.Rows[i][0].ToString());
                GymModel gymM = GymDll.instanc.GetmodelbyId(courtM.GYMID);
                item.Add("text", gymM.GYMNAME + "|" + courtM.COURTNAME);

                tourCourts.Add(item);
            }
            return tourCourts;
        }


        /// <summary>
        /// 获得赛事的比赛记录
        /// 2015-12-29 
        /// </summary>
        /// <param name="_Toursys"></param>
        /// <returns></returns>
        public List<WeTourResultModel> GetTourResult(string _Toursys)
        {
            List<WeTourResultModel> list = new List<WeTourResultModel>();
            List<WeTourContModel> contlist = WeTourContentDll.instance.GetContentlist(_Toursys);
            if (contlist.Count > 0)
            {
                foreach (WeTourContModel model in contlist)
                {
                    WeTourResultModel rmodel = new WeTourResultModel();
                    rmodel.ContentId = model.id;
                    rmodel.ContentName = model.ContentName;
                    try
                    {
                        rmodel.WeTourResult = WeMatchDll.instance.GetContFinishMatches(model.id);
                    }
                    catch { 
                        
                    }
                    list.Add(rmodel);
                }
            }
            return list;
        }
    }
}
