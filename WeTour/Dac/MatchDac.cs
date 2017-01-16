using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using System.Data.SqlClient;

namespace WeTour
{
    public static class MatchDac
    {
        public static DataTable SelectList(string strWhere)
        {
            string strSql = "select * from wtf_match where 1=1 " + strWhere;
            return DbHelperSQL.Query(strSql).Tables[0];
        }

        public static bool Insert(MatchModel model)
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
            parameters[0].Value = model.SYS;
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

        public static bool Update(MatchModel model)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append(" Update wtf_match set");
            strSql.Append(" winner=@winner,");
            strSql.Append(" loser=@loser,");
            strSql.Append(" score=@score,");
            strSql.Append(" state=@state");
            strSql.Append(" where sys=@sys");

            SqlParameter[] parameters = { 
                        new SqlParameter("@sys",DbType.String),                      
                        new SqlParameter("@winner",DbType.String),
                        new SqlParameter("@loser",DbType.String),
                        new SqlParameter("@score",DbType.String),
                        new SqlParameter("@state",DbType.Int32)
                                      };
            parameters[0].Value = model.SYS;
            parameters[1].Value = model.WINNER;
            parameters[2].Value = model.LOSER;
            parameters[3].Value = model.SCORE;
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
        /// 查询比赛报表
        /// </summary>
        /// <param name="strWhere"></param>
        /// <returns></returns>
        public static DataTable SelectMatchReport(string strWhere)
        {
            string strSql = "select * from Report_Tennis_MatchDetail where 1=1 " + strWhere;
            return DbHelperSQL.Query(strSql).Tables[0];
        }

        /// <summary>
        /// 修改比赛状态
        /// </summary>
        /// <param name="_Sys"></param>
        /// <param name="_State"></param>
        /// <returns></returns>
        public static bool UpdateState(string _Sys, int _State)
        {
            string strSql = "update wtf_match set state='" + _State + "' where sys='" + _Sys + "'";
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

        public static bool UpdateUmpire(string _Sys, string _Umpiresys)
        {
            string Time = DateTime.Now.ToString();
            string strSql = "update wtf_match set Umpire='" + _Umpiresys + "',actualTime='" + Time + "' where sys='" + _Sys + "'";
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
