using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web.Script.Serialization;

namespace WeChat
{
    public class Biz_WeLogin
    {
        public static Biz_WeLogin instance = new Biz_WeLogin();

        //获取openid
        public string GetOpenId(string code)
        {
            
            var client = new System.Net.WebClient();//定义从URI资源获取数据的方法。
            client.Encoding = System.Text.Encoding.UTF8;//设置字符编码格式为UTF8
            string url1 = "https://api.weixin.qq.com/sns/oauth2/access_token?appid=wxe90eba22e992ffba&secret=062613b1025b0c9abc4ffc4feec3aaba&code=" + code + "&grant_type=authorization_code";//定义获取access_token的链接。
            var data = client.DownloadString(url1);//将指定URI的资源以字符串的形式下载下来。

            var serializer = new JavaScriptSerializer();
            var obj = serializer.Deserialize<Dictionary<string, string>>(data);//将json字符串转换为实体对象。Dictionary表示键和值的集合。
            string _OpenId="";
            //获取access_Token;
            string accessToken;
            if (obj.TryGetValue("access_token", out accessToken))
            {
                //获取openid                
                obj.TryGetValue("openid", out _OpenId);

                //获取refresh_Token;
                string refToken;
                obj.TryGetValue("refresh_token", out refToken);

                //检验获取的access_token是否有效，如无效，则重新获取。
                string url2 = "https://api.weixin.qq.com/sns/auth?access_token=" + accessToken + "&openid=" + _OpenId;
                data = client.DownloadString(url2);

                var checkobj = serializer.Deserialize<Dictionary<string, string>>(data);
                string erromsg;
                checkobj.TryGetValue("errmsg", out erromsg);//获得判断的结果
                if (erromsg != "ok")
                {
                    //access_token已经失效，需要重新获取。
                    string url3 = "https://api.weixin.qq.com/sns/oauth2/refresh_token?appid=wxe90eba22e992ffba&grant_type=refresh_token&refresh_token=" + refToken;
                    data = client.DownloadString(url3);
                    var refobj = serializer.Deserialize<Dictionary<string, string>>(data);
                    refobj.TryGetValue("access_token", out accessToken);//刷新access_Token
                }
            }
            return _OpenId;
        }
        
    }
}
