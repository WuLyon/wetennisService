using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;

namespace Gym
{
    public class GymDll
    {
        public static GymDll instanc = new GymDll();

        /// <summary>
        /// Get Model by id
        /// </summary>
        /// <param name="_Id"></param>
        /// <returns></returns>
        public GymModel GetmodelbyId(string _Id)
        {
            GymModel model = new GymModel();
            string sql = "select * from wtf_gym where id='"+_Id+"'";
            DataTable dt = DbHelperSQL.Query(sql).Tables[0];
            if (dt.Rows.Count > 0)
            {
                dt.Rows[0]["GymInfo"] = "";
                model = JsonHelper.ParseDtInfo<GymModel>(dt);
            }
            return model;
        }

        /// <summary>
        /// Get Model by Sys
        /// </summary>
        /// <param name="_Id"></param>
        /// <returns></returns>
        public GymModel GetmodelbySys(string _Sysno)
        {
            GymModel model = new GymModel();
            string sql = "select * from wtf_gym where sys='" + _Sysno + "'";
            DataTable dt = DbHelperSQL.Query(sql).Tables[0];
            if (dt.Rows.Count > 0)
            {
                dt.Rows[0]["GymInfo"] = "";
                model = JsonHelper.ParseDtInfo<GymModel>(dt);
            }            
            return model;
        }

        /// <summary>
        /// Get Model by Ownersys
        /// owner can owned only one gym
        /// </summary>
        /// <param name="_Id"></param>
        /// <returns></returns>
        public GymModel GetmodelbyOwner(string _Sysno)
        {
            GymModel model = new GymModel();
            string sql = "select * from wtf_gym where Ownersys='" + _Sysno + "'";
            DataTable dt = DbHelperSQL.Query(sql).Tables[0];
            if (dt.Rows.Count > 0)
            {
                dt.Rows[0]["GymInfo"] = "";
                model = JsonHelper.ParseDtInfo<GymModel>(dt);
            }
            return model;
        }

        /// <summary>
        /// Get GymPrice Range
        /// </summary>
        /// <param name="_Sysno"></param>
        /// <returns></returns>
        public string GetGymPriceRange(string _Id)
        {
            string PriceRange = "";
            string sql1 = "select MIN(Convert(int,Price)) from Wtf_GymPrice where Gymid='" + _Id + "'";
            DataTable dt1 = DbHelperSQL.Query(sql1).Tables[0];
            PriceRange += dt1.Rows[0][0].ToString();

            string sql2 = "select MAX(Convert(int,Price)) from Wtf_GymPrice where Gymid='" + _Id + "'";
            DataTable dt2 = DbHelperSQL.Query(sql2).Tables[0];
            PriceRange += "-"+dt2.Rows[0][0].ToString();
            return PriceRange;
        }

        /// <summary>
        /// Get Gym Main info
        /// </summary>
        /// <param name="sysno"></param>
        /// <returns></returns>
        public string GetGymMain(string sysno)
        {
            string _GymInfo = "";
            string sql = "select GymInfo from wtf_gym where sys='" + sysno + "'";
            DataTable dt = DbHelperSQL.Query(sql).Tables[0];
            if (dt.Rows.Count > 0)
            {
                _GymInfo = dt.Rows[0][0].ToString();
            }
            return _GymInfo;
        }

        /// <summary>
        /// update gym main info
        /// </summary>
        /// <param name="_sysno"></param>
        /// <returns></returns>
        public bool UpdateGymMainInfo(string _sysno,string _html)
        {
            string sql = "update wtf_gym set GymInfo='"+_html+"' where sys ='"+_sysno+"'";
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
    }
}
