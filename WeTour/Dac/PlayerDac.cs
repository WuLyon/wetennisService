using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using System.Data.SqlClient;

namespace WeTour
{
    public static class PlayerDac
    {
        /// <summary>
        /// 查询数据
        /// </summary>
        /// <param name="strWhere"></param>
        /// <returns></returns>
        public static DataTable SelectList(string strWhere)
        {
            string strSql = "select * from td_player where 1=1 " + strWhere;
            return DbHelperSQL.Query(strSql).Tables[0];
        }

        public static bool Insert(PlayerModel model)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append(" Insert into td_player (");
            strSql.Append(" sys,pname,address,birthday,turnedtennis) values (");
            strSql.Append("@sys,@pname,@address,@birthday,@turnedtennis)");
            SqlParameter[] parameters = { 
                        new SqlParameter("@sys",DbType.String),
                        new SqlParameter("@pname",DbType.String),
                        new SqlParameter("@address",DbType.String),
                        new SqlParameter("@birthday",DbType.DateTime),
                        new SqlParameter("@turnedtennis",DbType.String)
                                      };
            parameters[0].Value = model.SYS;
            parameters[1].Value = model.PNAME;
            parameters[2].Value = model.ADDRESS;
            parameters[3].Value = model.BIRTHDAY;
            parameters[4].Value = model.TURNEDTENNIS;

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
    }
}
