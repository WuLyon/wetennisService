using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using System.Data.SqlClient;

namespace WeTour
{
    public static class PointsDac
    {
        public static DataTable SelectList(string strWhere)
        {
            string strSql = "select * from wtf_points where 1=1 " + strWhere;
            return DbHelperSQL.Query(strSql).Tables[0];
        }

        public static bool Insert(PointsModel model)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append(" Insert into wtf_points (");
            strSql.Append(" sys,gamesys,gorder,servetype,wintype,winner,isbreakpoint,AddDate) values (");
            strSql.Append("@sys,@gamesys,@gorder,@servetype,@wintype,@winner,@isbreakpoint,@AddDate)");
            SqlParameter[] parameters = { 
                        new SqlParameter("@sys",DbType.String),
                        new SqlParameter("@gamesys",DbType.String),
                        new SqlParameter("@gorder",DbType.String),
                        new SqlParameter("@servetype",DbType.DateTime),
                        new SqlParameter("@wintype",DbType.String),
                        new SqlParameter("@winner",DbType.String),
                        new SqlParameter("@isbreakpoint",DbType.Int32),
                        new SqlParameter("@AddDate",DbType.String)
                                      };
            parameters[0].Value = model.SYS;
            parameters[1].Value = model.GAMESYS;
            parameters[2].Value = model.GORDER;
            parameters[3].Value = model.SERVETYPE;
            parameters[4].Value = model.WINTYPE;
            parameters[5].Value = model.WINNER;
            parameters[6].Value = model.ISBREAKPOINT;
            parameters[7].Value = DateTime.Now.ToString();

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
        /// 根据局主键来获得局分
        /// </summary>
        /// <param name="_Gamesys"></param>
        /// <returns></returns>
        public static DataTable SelectScore(string _Gamesys, string _Winner)
        {
            string strSql = "select count(sys) score,winner from wtf_points where gamesys='" + _Gamesys + "' and winner='" + _Winner + "' group by winner";
            return DbHelperSQL.Query(strSql).Tables[0];
        }

        /// <summary>
        /// 根据局主键和比分顺序来删除比分
        /// </summary>
        /// <param name="_Gamesys"></param>
        /// <param name="_Order"></param>
        /// <returns></returns>
        public static bool DeletebyOrder(string _Gamesys, int _Order)
        {
            string strSql = "delete wtf_points where gamesys='" + _Gamesys + "' and gorder=" + _Order + "";
            int res = DbHelperSQL.ExecuteSql(strSql);
            if (res > 0)
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
