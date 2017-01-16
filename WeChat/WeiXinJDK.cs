using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WeChat
{
    public class WeiXinJDK
    {

        public static WeiXinJDK instance = new WeiXinJDK(); 

        /// <summary>
        /// 获得签名
        /// </summary>
        /// <param name="jsapoi_ticket"></param>
        /// <param name="noncestr"></param>
        /// <param name="timestamp"></param>
        /// <param name="url"></param>
        /// <returns></returns>
        public string sign(string jsapoi_ticket, string noncestr, int timestamp, string url)
        {
            //签名算法
            string str = "jsapi_ticket=" + jsapoi_ticket + "&noncestr=" + noncestr + "&timestamp=" + timestamp.ToString() + "&url=" + url;
            return System.Web.Security.FormsAuthentication.HashPasswordForStoringInConfigFile(str, "SHA1").ToLower();
        }

        /// <summary>
        /// 创建随机数
        /// </summary>
        /// <returns></returns>
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

        /// <summary>
        /// 创建时间戳
        /// </summary>
        /// <returns></returns>
        public int createTimeStamp()
        {
            System.DateTime startTime = TimeZone.CurrentTimeZone.ToLocalTime(new System.DateTime(1970, 1, 1));
            return (int)(System.DateTime.Now - startTime).TotalSeconds;
        }
    }
}