using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;

namespace SMS
{
    public class SendReport
    {
        public static SendReport instance = new SendReport();

        public void SendReportToLastYear()
        { 
            string _Content="您好！2016年华侨城网球精英赛已开始报名，我们期待您的回归。请关注微网球公众号进行报名。";
            string tousys="E9E7986B-99CF-4483-82FE-304AE84309C0";
            string sql = "select distinct(Telephone) from wtf_member where sysno in (select memberid from wtf_tourapply where toursys='" + tousys + "') and len(Telephone)='11'";
            DataTable dt = DbHelperSQL.Query(sql).Tables[0];
            //SMSdll.instance.BatchSendSMS("13678162515", _Content+dt.Rows.Count);
            if (dt.Rows.Count > 0)
            {
                foreach (DataRow dr in dt.Rows)
                {
                    //发送通知
                    SMSdll.instance.BatchSendSMS(dr[0].ToString(), _Content);
                }
            }
        }
    }
}
