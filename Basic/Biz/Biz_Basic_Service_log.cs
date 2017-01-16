using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Basic
{
    public class Biz_Basic_Service_log
    {
        public static Biz_Basic_Service_log instance = new Biz_Basic_Service_log();

        public void AddNewLog(Model_Basic_Service_Log model)
        {
            StringBuilder sb = new StringBuilder();
            sb.Append("Insert into sys_service_log values (");
            sb.Append("'" + Guid.NewGuid().ToString("N") + "',");
            sb.Append("'" + model.ServiceName + "',");
            sb.Append("'" + model.URL + "',");
            sb.Append("'" + model.HostName + "',");
            sb.Append("'" + model.ResponseStr + "',");
            sb.Append("'" + DateTime.Now + "'");
            sb.Append(")");
            string sql = sb.ToString();
            int a = DbHelperSQL.ExecuteSql(sql);

        }
    }
}
