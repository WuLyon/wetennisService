using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace WeChat
{
    public class PayModel
    {

        /// <summary>
        /// 商户自己的订单号（必填）
        /// </summary>

        private string ordersn;
        public string OrderSN
        {
            get
            {
                if (string.IsNullOrEmpty(ordersn))
                    return DateTime.Now.ToString("yyyyMMddHHmmsss");
                return ordersn;
            }
            set
            {
                ordersn = value;
            }
        }

        /// <summary>
        /// 订单支付金额单位为分（必填）
        /// </summary>
        public int TotalFee { get; set; }

        /// <summary>
        /// 商品信息（必填，长度不能大于127字符）
        /// </summary>
        private string body;
        public string Body
        {
            get
            {
                if (string.IsNullOrEmpty(body))
                    return "购物";
                if (body.Length > 127)
                    return body.Substring(0, 120) + "...";
                return body;
            }
            set
            {
                body = value;
            }
        }


        /// <summary>
        /// 支付用户微信OpenId（必填）
        /// </summary>
        public string OpenId { get; set; }


        /// <summary>
        /// 用户自定义参数原样返回，不能有中文不然调用Notify页面会有错误。（长度不能大于127字符）
        /// </summary>
        /// 
        public string Attach { get; set; }


        /// <summary>
        /// 重写ToString函数，获取跳转到支付页面的url其中附带参数
        /// </summary>
        /// <returns></returns>
        public override string ToString()
        {
            StringBuilder sb = new StringBuilder();
            sb.Append("WeiPay.aspx?");
            sb.AppendFormat("&OrderSN={0}", OrderSN);
            sb.AppendFormat("&Body={0}", Body);
            sb.AppendFormat("&TotalFee={0}", TotalFee);
            sb.AppendFormat("&UserOpenId={0}", OpenId);
            sb.AppendFormat("&Attach={0}", Attach);

            return sb.ToString();
        }
    }

    public class Model_JSBridge {
        public string appId { get; set; }
        public string timestamp { get; set; }
        public string nonceStr { get; set; }
        public string packageStr { get; set; }
        public string signType { get; set; }
        public string paySign { get; set; }
    }
    public class Model_OrderInfo{
        public string orderNum{get;set;}
        public string description{get;set;}
        public string totalFee{get;set;}
    }

    public class Fe_Model_OrderInfo {
        public object OrderInfo { get; set; }
        public object jsBridge { get; set; }
    }
}
