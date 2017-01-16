using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data;
using System.Net;
using System.IO;
using System.Xml;
using Newtonsoft.Json;
using SMS;

namespace WeChat
{
    /// <summary>
    /// 公共服务类
    /// </summary>
    public class zPublicClass
    {
        /// <summary>
        /// 读取JSON文件内容
        /// </summary>
        /// <param name="congTxt"></param>
        /// <param name="fi"></param>
        /// <param name="DTname"></param>
        /// <returns></returns>
        public DataTable getJSONfr(string congTxt, string fi, string DTname)
        {
            ReadTxt readTxtTocken = new ReadTxt();
            string json_str = readTxtTocken.ReadTxtF(congTxt, fi).ToString();
            DataSet dataSet = JsonConvert.DeserializeObject<DataSet>(json_str);//
            DataTable dataTable = dataSet.Tables[DTname];
            return dataTable;
        }

        /// <summary>
        /// 计算时间差
        /// </summary>
        /// <param name="dateBegin"></param>
        /// <param name="dateEnd"></param>
        /// <returns></returns>
        public int ExecDateDiff(DateTime dateBegin, DateTime dateEnd)
        {
            TimeSpan ts1 = new TimeSpan(dateBegin.Ticks);
            TimeSpan ts2 = new TimeSpan(dateEnd.Ticks);
            TimeSpan ts3 = ts1.Subtract(ts2).Duration();
            return (int)ts3.TotalSeconds;
        }

        /// <summary>
        /// 获得 ACCESS TOKEN
        /// </summary>
        /// <param name="urlPath"></param>
        /// <param name="millisecond"></param>
        /// <param name="zMethod"></param>
        /// <returns></returns>
        public string GetHTTPInfo(string urlPath, int millisecond, string zMethod)
        {            
            string str = null;
            HttpWebResponse response = null;
            StreamReader reader = null;
            try
            {
                HttpWebRequest request = (HttpWebRequest)WebRequest.Create(urlPath);
                request.Timeout = millisecond;
                request.Method = zMethod;
                response = (HttpWebResponse)request.GetResponse();
                reader = new StreamReader(response.GetResponseStream());
                str = reader.ReadToEnd();
            }
            catch
            {
                str = "";
            }
            finally
            {
                if (reader != null)
                {
                    reader.Close();
                    reader.Dispose();
                }
                if (response != null)
                {
                    response.Close();
                }
            }
            return str;
        }


        public string GetHttp_jsapi_ticket(string urlPath, int millisecond, string zMethod)
        {           
            string str = null;
            HttpWebResponse response = null;
            StreamReader reader = null;
            try
            {
                HttpWebRequest request = (HttpWebRequest)WebRequest.Create(urlPath);
                request.Timeout = millisecond;
                request.Method = zMethod;
                response = (HttpWebResponse)request.GetResponse();
                reader = new StreamReader(response.GetResponseStream());
                str = reader.ReadToEnd();
            }
            catch
            {
                str = "";
            }
            finally
            {
                if (reader != null)
                {
                    reader.Close();
                    reader.Dispose();
                }
                if (response != null)
                {
                    response.Close();
                }
            }
            return str;
        }

        public string Getjsapi_ticket(string url_access_token, string url_jsapi_ticket, int millisecond, string zMethod)
        {
            string str = null;
            string str_access_token = null;
            string str_jsapi_ticket = null;
            try
            {
                str_access_token = GetHTTPInfo(url_access_token, millisecond, zMethod);
                str_access_token = "{\"ds_dt\":[" + str_access_token + "]}";

                //Biz.Proc_StepBll.Get_Instance().WriteLog("获取accesstoken","","",str_access_token,"","","","");

                DataSet ds_access_token = JsonConvert.DeserializeObject<DataSet>(str_access_token);
                DataTable dt_access_token = ds_access_token.Tables["ds_dt"];
                url_jsapi_ticket = "https://api.weixin.qq.com/cgi-bin/ticket/getticket?access_token=" + dt_access_token.Rows[0]["access_token"].ToString()+"&type=jsapi";
                str_jsapi_ticket = "{\"jt_dt\":[" + GetHttp_jsapi_ticket(url_jsapi_ticket, millisecond, zMethod) + "]}";
                
                DataSet ds_jsapi_ticket = JsonConvert.DeserializeObject<DataSet>(str_jsapi_ticket);
                DataTable dt_jsapi_ticket = ds_jsapi_ticket.Tables["jt_dt"];
                str = dt_jsapi_ticket.Rows[0]["ticket"].ToString();
            }
            catch (Exception e) { str = e.ToString(); }

            return str;
        }

        public char WriteJSONfr(string filePath, string Files, string JSON_text)
        {
            ReadTxt Writetxt = new ReadTxt();
            try
            {
                Writetxt.WriteJsonText(filePath, Files, JSON_text);
                return 'Y';
            }
            catch
            {
                return 'N';
            }
        }
    }
}