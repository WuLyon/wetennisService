using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;

namespace SMS
{
    public class SMSdll
    {
        public static SMSdll instance = new SMSdll();

        public string CorpId = "LKSDK0003785";//公司编号
        public string Pwd = "ZHWW@TY85";//密码

        public int BatchSendSMS(string _Telephone, string _Content)
        {
            string _Result = "";
            int a = 0;
            try
            {
                SMG.LinkWS smsc = new SMG.LinkWS();
                a = smsc.BatchSend(CorpId, Pwd, _Telephone, _Content, "", "");
                if (a == 0)
                {
                    _Result = "发送成功进入审核阶段";
                }
                else if (a == 1)
                {
                    _Result = "直接发送成功";
                }
                else if (a == -1)
                {
                    _Result = "帐号未注册";
                }
                else if (a == -2)
                {
                    _Result = "其他错误！";
                }
                else if (a == -3)
                {
                    _Result = "帐号密码不匹配！";
                }
                else if (a == -4)
                {
                    _Result = "一次提交信息不能超过600个手机号码！";
                }
                else if (a == -5)
                {
                    _Result = "企业号帐户余额不足，请先充值再提交短信息！";
                }
                else if (a == -6)
                {
                    _Result = "定时发送时间不是有效时间格式！";
                }
                else if (a == -7)
                {
                    _Result = "禁止10小时之内向同一手机发送相同内容";
                }
                else if (a == -8)
                {
                    _Result = "发送内容需在3到250个字之间";
                }
                else if (a == -9)
                {
                    _Result = "发送号码为空";
                }
            }
            catch
            {
                _Result = "网络错误，无法连接到服务器";
            }

            //写到日志
            InsertSms(_Result, _Telephone, _Content);

            return a;
        }

        /// <summary>
        /// 将内容写到数据库
        /// </summary>
        /// <param name="_Result"></param>
        /// <param name="_Telephone"></param>
        /// <param name="_Content"></param>
        public void InsertSms(string _Result, string _Telephone, string _Content)
        {
            string sql = string.Format("insert into wtf_SendSms (Result,Telephone,Contents,SendDate) values ('{0}','{1}','{2}','{3}')", _Result, _Telephone, _Content, DateTime.Now.ToString());
            DbHelperSQL.ExecuteSql(sql);
        }
    }
}
