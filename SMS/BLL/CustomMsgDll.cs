using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using Member;

namespace SMS
{
    /// <summary>
    /// 自定义发送端新应用
    /// </summary>
    public class CustomMsgDll
    {
        public static CustomMsgDll instance = new CustomMsgDll();

        /// <summary>
        /// 给报了名但是未付款的朋友们发送催款通知
        /// </summary>
        /// <param name="_Toursys"></param>
        public void SendUnpaidReport(string _Toursys)
        {
            string MsgContent = "您好！本次比赛签表优先考虑已支付报名费的球员，为避免您的报名信息被忽略，请尽快缴纳报名费。";
            string sql = "select distinct(memberid) from wtf_tourapply where toursys='"+_Toursys+"' and status=1";
            DataTable dt = DbHelperSQL.Query(sql).Tables[0];
            if (dt.Rows.Count > 0)
            {
                for(int i = 0; i < dt.Rows.Count; i++)
                {
                    WeMemberModel model = WeMemberDll.instance.GetModel(dt.Rows[i][0].ToString());
                    if (model.TELEPHONE.Length == 11)
                    {
                        SMSdll.instance.BatchSendSMS(model.TELEPHONE, MsgContent);
                    }
                }
            }
        }
    }
}
