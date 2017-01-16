using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data;

namespace WeTour
{
    public static class MemberDac
    {
        public static DataTable SelectList(string strWhere)
        {
            string strSql = "select * from wtf_member where 1=1 " + strWhere;
            return DbHelperSQL.Query(strSql).Tables[0];
        }
    }
}