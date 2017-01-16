using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;

namespace WeTour
{
    public class ResGymDll
    {
        public static ResGymDll instance = new ResGymDll();

        /// <summary>
        /// 获得赛事场馆资源
        /// </summary>
        /// <param name="_Toursys"></param>
        /// <returns></returns>
        public List<ResTour_TourGymModel> GetTourGymList(string _Toursys)
        {
            List<ResTour_TourGymModel> list = new List<ResTour_TourGymModel>();
            string sql = "select * from ResGym_TourGym where toursys='"+_Toursys+"'";
            DataTable dt = DbHelperSQL.Query(sql).Tables[0];
            if (dt.Rows.Count > 0)
            {
                list = JsonHelper.ParseDtModelList<List<ResTour_TourGymModel>>(dt);
            }
            return list;
        }

        /// <summary>
        /// 根据赛事主键获得场地列表
        /// </summary>
        /// <param name="_Toursys"></param>
        /// <returns></returns>
        public List<ResTour_TourCourtModel> GetTourCourtList(string _Toursys,string _Gymid)
        {
            List<ResTour_TourCourtModel> list = new List<ResTour_TourCourtModel>();
            string sql = "select * from ResGym_TourCourt where toursys='"+_Toursys+"' and Gymid='"+_Gymid+"'";
            DataTable dt = DbHelperSQL.Query(sql).Tables[0];
            if (dt.Rows.Count > 0)
            {
                list = JsonHelper.ParseDtModelList<List<ResTour_TourCourtModel>>(dt);
            }
            return list;
        }

        /// <summary>
        /// 根据赛事主键，场地编号，获得场馆项目清单
        /// </summary>
        /// <param name="_Toursys"></param>
        /// <param name="_GymId"></param>
        /// <returns></returns>
        public List<ResTour_GymContentModel> GetGymContentList(string _Toursys)
        {
            List<ResTour_GymContentModel> list = new List<ResTour_GymContentModel>();
            string sql = "select * from ResGym_GymContent where toursys='"+_Toursys+"'";
            DataTable dt = DbHelperSQL.Query(sql).Tables[0];
            if (dt.Rows.Count > 0)
            {
                list = JsonHelper.ParseDtModelList<List<ResTour_GymContentModel>>(dt);
            }
            return list;
        }
        
    }
}
