using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Text;
using System.Web.Script.Serialization;
using Member;
using SMS;
using System.Data;

namespace WeTennisService.WebService
{
    /// <summary>
    /// WeChat 的摘要说明
    /// </summary>
    public class WeChat : IHttpHandler
    {

        public void ProcessRequest(HttpContext context)
        {
            context.Response.ContentType = "text/plain";
            string typename = context.Request.QueryString["typename"];
            switch (typename)
            { 
                case "GetAccessToken":
                    GetAccessToken(context);
                    break;

                case "GetWxConfigDetails":
                    GetWxConfigDetails(context);
                    break;

                case "WeChatPay":

                    break;
            }
        }

        /// <summary>
        /// 通过code换取微信登陆人信息。
        /// </summary>
        /// <param name="context"></param>
        void GetAccessToken(HttpContext context)
        {
            //处理回调函数
            context.Response.ContentType = "application/json;charset=utf-8";
            string jsonCallBackFunName = context.Request.Params["jsoncallback"].ToString();

            string code = context.Request.Params["code"];
            //WriteLog("GetAccessToken", code);

            WeMemberModel model = new WeMemberModel();
            string json="";
            var client = new System.Net.WebClient();//定义从URI资源获取数据的方法。
            client.Encoding = System.Text.Encoding.UTF8;//设置字符编码格式为UTF8
            string url1 = "https://api.weixin.qq.com/sns/oauth2/access_token?appid=wxe90eba22e992ffba&secret=062613b1025b0c9abc4ffc4feec3aaba&code=" + code + "&grant_type=authorization_code";//定义获取access_token的链接。
            var data = client.DownloadString(url1);//将指定URI的资源以字符串的形式下载下来。
            
            var serializer = new JavaScriptSerializer();
            var obj = serializer.Deserialize<Dictionary<string, string>>(data);//将json字符串转换为实体对象。Dictionary表示键和值的集合。
           

            //获取access_Token;
            string accessToken;
            if (obj.TryGetValue("access_token",out accessToken))
            { 
                //获取openid
                string _OpenId;
                obj.TryGetValue("openid", out _OpenId);

                //判断openid是否已是绑定的微网球用户
                model = WeMemberDll.instance.GetModelbyOpenId(_OpenId);
              
                if (string.IsNullOrEmpty(model.SYSNO))
                {
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
                    //获得用户信息
                    string url4 = "https://api.weixin.qq.com/sns/userinfo?access_token=" + accessToken + "&openid=" + _OpenId + "&lang=zh_CN";
                    data = client.DownloadString(url4);//获取到用户信息，是json字符串格式
                    json = data;
                }
                else
                { 
                    //当前用户是绑定了微信的微网球会员
                    model.EXT10 = "1";//表示用户是绑定了微信的微网球会员，显示微网球的头像和用户名。
                    json = JsonHelper.ToJson(model);
                }
            }
            //处理查询结果
            context.Response.Write(string.Format("{0}({1})", jsonCallBackFunName, json));
            context.Response.End();
        }

        /// <summary>
        /// 获得微信的基本信息
        /// </summary>
        /// <param name="context"></param>
        void GetWxConfigDetails(HttpContext context)
        {           
              //处理回调函数
            context.Response.ContentType = "application/json;charset=utf-8";
            string jsonCallBackFunName = context.Request.Params["jsoncallback"].ToString();
            
            string str_json="";
            try
            {
                string url = context.Request["url"].ToString();
                
                string jsapi_ticket = string.Empty;
                string JSON_text = string.Empty;

                string appid = "wxe90eba22e992ffba";
                //获取accessToken
                string url_access_token = "https://api.weixin.qq.com/cgi-bin/token?grant_type=client_credential&appid=wxe90eba22e992ffba&secret=062613b1025b0c9abc4ffc4feec3aaba";

                string url_jsapi_ticket = "https://api.weixin.qq.com/cgi-bin/ticket/getticket?access_token=ACCESS_TOKEN&type=jsapi";
                int millisecond = 5000;
                string zMethod = "GET";

                //签名算法
                WeChatJDK wxjdk = new WeChatJDK();

                //noncestr 随机字符串
                string Noncestr = wxjdk.createNonceStr();
                
                //timestamp
                int timestamp = wxjdk.createTimeStamp();
                //WriteLog("GetWxConfigDetails", "timestamp " + timestamp);


                #region 由于ticket的获取次数有限制，故服务器端需要将获取的ticket存放在文件中
                //jsapi_ticket 签名算法(这个需要两小时去算一次)
                zPublicClass zpc = new zPublicClass();
                //从文件读取数据
                DataTable zpc_dt = zpc.getJSONfr(@"\Test", @"\jsapi_ticket.json.txt", "jt");
                //DataTable zpc_dt = zpc.getJSONsql("jt");
                //按条件取
                string filler = "\'" + appid + "\'";
                DataRow[] select_cont = zpc_dt.Select("AppID=" + filler);
                DataRow scr = select_cont[0];
                if (scr.IsNull(1))
                {
                    //没有取到数据,重新获取
                    //重新获取jsapi_ticket
                    jsapi_ticket = zpc.Getjsapi_ticket(url_access_token, url_jsapi_ticket, millisecond, zMethod);
                    //文件写入jsapi_ticket Json
                    JSON_text = "{\'jt\':";
                    JSON_text += "[{\'AppID\':\'" + appid + "\',";
                    JSON_text += "\'jsapi_ticket\':\'" + jsapi_ticket + "\',";
                    JSON_text += "\'zdatetime\':\'" + System.DateTime.Now.ToString() + "\',";
                    JSON_text += "}]";
                    JSON_text += "}";
                    char Ret_char = zpc.WriteJSONfr(@"\Test", @"\jsapi_ticket.json.txt", JSON_text);
                    //WriteLog("GetWxConfigDetails", "JSON_text " + JSON_text);
                }
                else
                {
                    //时间比较=当前时间-jsap_ticket写入数据库
                    int TSconds = zpc.ExecDateDiff(Convert.ToDateTime(scr["zdatetime"]), DateTime.Now);
                    if (TSconds < 7200 && !string.IsNullOrEmpty(scr["jsapi_ticket"].ToString()))
                    {
                        jsapi_ticket = scr["jsapi_ticket"].ToString();
                    }
                    else
                    {
                        //重新获取jsapi_ticket
                        jsapi_ticket = zpc.Getjsapi_ticket(url_access_token, url_jsapi_ticket, millisecond, zMethod);
                        //WriteLog("jsapi_ticket", jsapi_ticket);
                        //文件写入jsapi_ticket Json
                        JSON_text = "{\'jt\':";
                        JSON_text += "[{\'AppID\':\'" + appid + "\',";
                        JSON_text += "\'jsapi_ticket\':\'" + jsapi_ticket + "\',";
                        JSON_text += "\'zdatetime\':\'" + System.DateTime.Now.ToString() + "\',";
                        JSON_text += "}]";
                        JSON_text += "}";
                        char Ret_char = zpc.WriteJSONfr(@"\Test", @"\jsapi_ticket.json.txt", JSON_text);
                        //WriteLog("JSON_text", JSON_text);
                    }
                }

                #endregion

                //得到ticket后开始进行 签名算法开始
                string Signature = wxjdk.sign(jsapi_ticket, Noncestr, timestamp, url);
                WriteLog("Signature", Signature);

                //json
                List<Para_wxconfig> p_wxjdk = new List<Para_wxconfig>();

                Para_wxconfig wxjdk_str = new Para_wxconfig
                {
                    appid = appid,
                    timestamp = timestamp,
                    nonceStr = Noncestr,
                    signature = Signature,
                };           
           
                p_wxjdk.Add(wxjdk_str);
           
                //writejson
                str_json = JsonHelper.ToJson(wxjdk_str);
                //WriteLog("Signature", Signature);
            }
            catch (Exception e)
            {
                str_json = e.ToString();
                WriteLog("Error", e.ToString().Substring(0,60));
            }
                //处理查询结果
                context.Response.Write(string.Format("{0}({1})", jsonCallBackFunName, str_json));
                context.Response.End();
           
        }

        /// <summary>
        /// 微信支付
        /// </summary>
        /// <param name="context"></param>
        void WeChatPay(HttpContext context)
        {
            Model_Return ret = new Model_Return();

            try
            {

            }
            catch (Exception e)
            {
                
                throw;
            }
            context.Response.Write("");
        }

        void WriteLog(string _Desc, string value1)
        {
            string sql = "insert into sys_setting (Enum,Descript,value,value2) values ('1101','" + _Desc + "','" + DateTime.Now.ToString() + "','" + value1 + "')";
            int a = DbHelperSQL.ExecuteSql(sql);
        }


        public bool IsReusable
        {
            get
            {
                return false;
            }
        }
    }
}