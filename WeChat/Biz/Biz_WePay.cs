using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.Script.Serialization;
using System.Net;
using System.IO;
using System.Xml;
using OrderLib;


namespace WeChat
{
    public class Biz_WePay
    {
        public static Biz_WePay instance = new Biz_WePay();

        public Model_JSBridge GetBridge(HttpContext Context,string _OrderNum,string _code)
        {
            Model_JSBridge model = new Model_JSBridge();
            model.appId = PayConfig.AppId;
            model.nonceStr = TenpayUtil.getNoncestr();
            model.timestamp = TenpayUtil.getTimestamp();

            //获取package
            model.packageStr = "prepay_id=" + UnionPrePay(Context,_OrderNum, _code);
           
            //获取paySign
            var paySignReqHandler = new RequestHandler(Context);
            paySignReqHandler.setParameter("appId", PayConfig.AppId);
            paySignReqHandler.setParameter("timeStamp", model.timestamp);
            paySignReqHandler.setParameter("nonceStr", model.nonceStr);
            paySignReqHandler.setParameter("package", model.packageStr);
            paySignReqHandler.setParameter("signType", "MD5");
            model.paySign = paySignReqHandler.CreateMd5Sign("key", PayConfig.AppKey);
                       
            return model;
        }

        

        /// <summary>
        /// 根据条件获取prepay_id
        /// </summary>
        /// <returns></returns>
        public string UnionPrePay(HttpContext Context,string _orderNum,string _code)
        {
            //_code = "orxFyuLaszWrLqiS9qdK5zsshMzs";
            string _PrePay_id = "";
            //获取订单详情
            OrderModel order = OrderBll.Instance("wtf").getOrderModelbySys(_orderNum);
            string Fee = "1";
            try
            {
                decimal a=Convert.ToDecimal(order.SHOULDPAY) * 100;
                int b=Convert.ToInt32(a);
                Fee = b.ToString();
            }
            catch (Exception e)
            {
                Fee = "1";
            }

            //后台调用js接口获取返回参数
            var client =new System.Net.WebClient();//定义从URI资源获取数据的方法
            client.Encoding = System.Text.Encoding.UTF8;

            //配置url及参数
            string url = "https://api.mch.weixin.qq.com/pay/unifiedorder";

            //设置参数
            var packageReqHandler = new RequestHandler(Context);

            packageReqHandler.init();//初始化

            packageReqHandler.setParameter("body", order.DESCRIPTION); //商品信息 127字符
            packageReqHandler.setParameter("appid", PayConfig.AppId);
            packageReqHandler.setParameter("mch_id", PayConfig.MchId);
            packageReqHandler.setParameter("nonce_str",TenpayUtil.getNoncestr().ToLower());
            packageReqHandler.setParameter("notify_url", PayConfig.NotifyUrl);
            packageReqHandler.setParameter("openid", _code);//用户openid
            packageReqHandler.setParameter("out_trade_no", _orderNum); //商家订单号
            packageReqHandler.setParameter("spbill_create_ip", Context.Request.UserHostAddress); //用户的公网ip，不是商户服务器IP
            packageReqHandler.setParameter("total_fee", Fee); //商品金额,以分为单位(money * 100).ToString()
            packageReqHandler.setParameter("trade_type", "JSAPI");

            //定义sign，为了获取预支付ID的签名
            string Sign = packageReqHandler.CreateMd5Sign("key", PayConfig.AppKey);
            packageReqHandler.setParameter("sign", Sign);
            
            //发送POST请求
            string prepayXml = TenpayUtil.Send(packageReqHandler.parseXML(), url);

            //获取预支付ID
            var xdoc = new XmlDocument();
            xdoc.LoadXml(prepayXml);
            XmlNode xn = xdoc.SelectSingleNode("xml");
            XmlNodeList xnl = xn.ChildNodes;
            if (xnl.Count > 7)
            {
                _PrePay_id = xnl[7].InnerText;
               
            }   
            return _PrePay_id;
        }

        #region 辅助方法
        private string HttpPost(string Url, string postDataStr)
        {
            HttpWebRequest request = (HttpWebRequest)WebRequest.Create(Url);
            request.Method = "POST";
            request.ContentType = "application/x-www-form-urlencoded";
            request.ContentLength = Encoding.UTF8.GetByteCount(postDataStr);
            //request.CookieContainer = cookie;
            Stream myRequestStream = request.GetRequestStream();
            StreamWriter myStreamWriter = new StreamWriter(myRequestStream, Encoding.GetEncoding("gb2312"));
            myStreamWriter.Write(postDataStr);
            myStreamWriter.Close();
 
            HttpWebResponse response = (HttpWebResponse)request.GetResponse();
 
            //response.Cookies = cookie.GetCookies(response.ResponseUri);
            Stream myResponseStream = response.GetResponseStream();
            StreamReader myStreamReader = new StreamReader(myResponseStream, Encoding.GetEncoding("utf-8"));
            string retString = myStreamReader.ReadToEnd();
            myStreamReader.Close();
            myResponseStream.Close();
 
            return retString;
        }
 
        public string HttpGet(string Url, string postDataStr)
        {
            HttpWebRequest request = (HttpWebRequest)WebRequest.Create(Url + (postDataStr == "" ? "" : "?") + postDataStr);
            request.Method = "GET";
            request.ContentType = "text/html;charset=UTF-8";
 
            HttpWebResponse response = (HttpWebResponse)request.GetResponse();
            Stream myResponseStream = response.GetResponseStream();
            StreamReader myStreamReader = new StreamReader(myResponseStream, Encoding.GetEncoding("utf-8"));
            string retString = myStreamReader.ReadToEnd();
            myStreamReader.Close();
            myResponseStream.Close();
 
            return retString;
        }
        #endregion

    }
}
