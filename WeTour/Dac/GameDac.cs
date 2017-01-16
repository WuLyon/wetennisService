using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using System.Data.SqlClient;

namespace WeTour
{
    public static class GameDac
    {
        public static DataTable SelectList(string strWhere)
        {
            string strSql = "select * from wtf_game where 1=1 " + strWhere;
            return DbHelperSQL.Query(strSql).Tables[0];
        }

        public static bool Insert(GameModel model)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append(" Insert into wtf_game (");
            strSql.Append(" sys,setsys,morder,mserver,winner,state,istiebreak,IsDecidePoint,LeftsidePlayer,ext1,ext2,ext3,ext4,ext5) values (");
            strSql.Append("@sys,@setsys,@morder,@mserver,@winner,@state,@istiebreak,@IsDecidePoint,@LeftsidePlayer,@ext1,@ext2,@ext3,@ext4,@ext5)");
            SqlParameter[] parameters = { 
                        new SqlParameter("@sys",DbType.String),
                        new SqlParameter("@setsys",DbType.String),
                        new SqlParameter("@morder",DbType.String),
                        new SqlParameter("@mserver",DbType.DateTime),
                        new SqlParameter("@winner",DbType.String),
                        new SqlParameter("@state",DbType.String),
                        new SqlParameter("@istiebreak",DbType.String),
                        new SqlParameter("@IsDecidePoint",DbType.String),
                         new SqlParameter("@LeftsidePlayer",DbType.DateTime),
                        new SqlParameter("@ext1",DbType.String),
                        new SqlParameter("@ext2",DbType.String),
                        new SqlParameter("@ext3",DbType.String),
                        new SqlParameter("@ext4",DbType.String),
                        new SqlParameter("@ext5",DbType.String)
                                      };
            parameters[0].Value = model.SYS;
            parameters[1].Value = model.SETSYS;
            parameters[2].Value = model.MORDER;
            parameters[3].Value = model.MSERVER;
            parameters[4].Value = model.WINNER;
            parameters[5].Value = model.STATE;
            parameters[6].Value = model.ISTIEBREAK;
            parameters[7].Value = model.IsDecidePoint;
            parameters[8].Value = model.LeftsidePlayer;
            parameters[9].Value = model.ext1;
            parameters[10].Value = model.ext2;
            parameters[11].Value = model.ext3;
            parameters[12].Value = model.ext4;
            parameters[13].Value = model.ext5;

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
        public static bool UpdateState(string _Sys,string _Winner, int _State)
        {
            string strSql = "Update wtf_game set state='" + _State + "',winner='" + _Winner + "' where sys='" + _Sys + "'";
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

        /// <summary>
        /// 修改局主键
        /// </summary>
        /// <param name="_Gamesys"></param>
        /// <returns></returns>
        public static bool UpdateIsTieBreak(string _Gamesys)
        {
            string strSql = "Update wtf_game set istiebreak=1 where sys='" + _Gamesys + "'";
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

