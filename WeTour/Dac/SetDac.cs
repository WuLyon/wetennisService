using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using System.Data.SqlClient;

namespace WeTour
{
    public static class SetDac
    {
        /// <summary>
        /// 查询数据
        /// </summary>
        /// <param name="strWhere"></param>
        /// <returns></returns>
        public static DataTable SelectList(string strWhere)
        {
            string strSql = "select * from wtf_set where 1=1"+strWhere;
            return DbHelperSQL.Query(strSql).Tables[0];
        }

        /// <summary>
        /// add a new record
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public static bool Insert(SetModel model)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append(" Insert into wtf_set (");
            strSql.Append(" sys,matchsys,sorder,winner,state) values (");
            strSql.Append("@sys,@matchsys,@sorder,@winner,@state)");
            SqlParameter[] parameters = { 
                        new SqlParameter("@sys",DbType.String),
                        new SqlParameter("@matchsys",DbType.String),
                        new SqlParameter("@sorder",DbType.Int32),
                        new SqlParameter("@winner",DbType.String),
                        new SqlParameter("@state",DbType.Int32)
                                      };
            parameters[0].Value = model.SYS;
            parameters[1].Value = model.MATCHSYS;
            parameters[2].Value = model.SORDER;
            parameters[3].Value = model.WINNER;
            parameters[4].Value = model.STATE;

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
        /// update game state
        /// </summary>
        /// <param name="_Sys"></param>
        /// <param name="_State"></param>
        /// <returns></returns>
        public static bool UpdateState(string _Sys, string _Winner, int _State)
        {
            string strSql = "Update wtf_set set state='" + _State + "',winner='" + _Winner + "' where sys='" + _Sys + "'";
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

