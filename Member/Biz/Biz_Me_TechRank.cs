using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;

namespace Member
{
    public class Biz_Me_TechRank
    {
        public static Biz_Me_TechRank instance = new Biz_Me_TechRank();

        public bool IsMemExist(string _userId)
        {
            string sql = "select * from Me_TechRank where memsys='"+_userId+"'";
            DataTable dt = DbHelperSQL.Query(sql).Tables[0];
            if (dt.Rows.Count > 0)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        public Model_Me_TechRank GetTechRank(string _userId)
        {
            Model_Me_TechRank model = new Model_Me_TechRank();
            string sql = "select Rank,isnull(TechRank,0) as TechRank,isnull(SelfTechRank,0) as SelfTechRank,isnull(OtherTechRank,0) as OtherTechRank from Me_TechRank where memsys='" + _userId + "'";
            DataTable dt = DbHelperSQL.Query(sql).Tables[0];
            model=JsonHelper.ParseDtInfo<Model_Me_TechRank>(dt);
            return model;
        }

        public bool InsertNew(Model_Me_TechRank model)
        {
            model.sysno = Guid.NewGuid().ToString("N");
            StringBuilder sb = new StringBuilder();
            sb.Append("Insert into Me_TechRank values (");
            sb.Append("'" + model.sysno + "',");
            sb.Append("'"+model.memsys+"',");
            sb.Append("'" + model.Rank + "',");
            sb.Append("'" + model.TechRank + "',");
            sb.Append("'" + model.SelfTechRank + "',");
            sb.Append("'" + model.OtherTechRank + "')");
            string sql = sb.ToString();
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

        public bool UpdateRank(Model_Me_TechRank model)
        {
            StringBuilder sb = new StringBuilder();
            sb.Append("Update Me_TechRank set ");
            sb.Append(" Rank='"+model.Rank+"',");
            sb.Append(" TechRank='"+model.TechRank+"',");
            sb.Append(" SelfTechRank='"+model.SelfTechRank+"',");
            sb.Append(" OtherTechRank='" + model.OtherTechRank + "'");
            sb.Append(" where memsys='"+model.memsys+"'");
            int a = DbHelperSQL.ExecuteSql(sb.ToString());
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
