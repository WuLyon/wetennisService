using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace WeTennisService
{
    public class WeChatJDK
    {
        public string sign(string jsapoi_ticket, string noncestr, int timestamp, string url)
        {
            //签名算法
            string str = "jsapi_ticket=" + jsapoi_ticket + "&noncestr=" + noncestr + "&timestamp=" + timestamp.ToString() + "&url=" + url;
            return System.Web.Security.FormsAuthentication.HashPasswordForStoringInConfigFile(str, "SHA1").ToLower();
        }

        public string createNonceStr()
        {
            string str = "1234567890abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ";
            Random r = new Random();
            string result = string.Empty;

            //生成一个16位的随机字符
            for (int i = 0; i < 16; i++)
            {
                int m = r.Next(0, str.Length);
                string s = str.Substring(m, 1);
                result += s;
            }
            return result;
        }

        public int createTimeStamp()
        {
            System.DateTime startTime = TimeZone.CurrentTimeZone.ToLocalTime(new System.DateTime(1970, 1, 1));
            return (int)(System.DateTime.Now - startTime).TotalSeconds;
        }
    }
}
