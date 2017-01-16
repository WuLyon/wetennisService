using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using System.Data.SqlClient;

namespace Club
{
    public static class ClubDac
    {
        /// <summary>
        /// 查询数据
        /// </summary>
        /// <param name="strWhere"></param>
        /// <returns></returns>
        public static DataTable SelectList(string strWhere)
        {
            string strSql = "select * from wtf_club where 1=1 " + strWhere;
            return DbHelperSQL.Query(strSql).Tables[0];
        }

        /// <summary>
        /// 添加数据
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public static bool Insert(ClubModel model)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("Insert into wtf_club (");
            strSql.Append(" sysno,ClubName,Province,City,region,proid,cityid,regid,ContactPerson,ContactTel,Description,status,clevel,ext1,ext2,ext3,UpdateDate) values (");
            strSql.Append("@sysno,@ClubName,@Province,@City,@region,@proid,@cityid,@regid,@ContactPerson,@ContactTel,@Description,@status,@clevel,@ext1,@ext2,@ext3,@UpdateDate)");

            SqlParameter[] parameters = { 
                        new SqlParameter("@sysno",DbType.String),
                        new SqlParameter("@ClubName",DbType.String),
                        new SqlParameter("@Province",DbType.String),
                        new SqlParameter("@City",DbType.String),
                        new SqlParameter("@region",DbType.String),
                        new SqlParameter("@proid",DbType.String),
                        new SqlParameter("@cityid",DbType.String),
                        new SqlParameter("@regid",DbType.String),
                        new SqlParameter("@ContactPerson",DbType.String),
                        new SqlParameter("@ContactTel",DbType.String),
                        new SqlParameter("@Description",DbType.String),
                        new SqlParameter("@status",DbType.String),
                        new SqlParameter("@clevel",DbType.String),
                        new SqlParameter("@ext1",DbType.String),
                        new SqlParameter("@ext2",DbType.String),                        
                        new SqlParameter("@ext3",DbType.String),
                        new SqlParameter("@UpdateDate",DbType.String)
                                        };
            parameters[0].Value = model.SYSNO;
            parameters[1].Value = model.CLUBNAME;
            parameters[2].Value = model.PROVINCE;
            parameters[3].Value = model.CITY;
            parameters[4].Value = model.REGION;
            parameters[5].Value = model.PROID;
            parameters[6].Value = model.CITYID;
            parameters[7].Value = model.REGID;
            parameters[8].Value = model.CONTACTPERSON;
            parameters[9].Value = model.CONTACTTEL;
            parameters[10].Value = model.DESCRIPTION;
            parameters[11].Value = model.STATUS;
            parameters[12].Value = model.CLEVEL;
            parameters[13].Value = model.EXT1;
            parameters[14].Value = model.EXT2;
            parameters[15].Value = model.EXT3;
            parameters[16].Value = DateTime.Now.ToString();

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
