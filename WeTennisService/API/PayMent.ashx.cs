using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using OrderLib;
using System.IO;
using WeChat;

namespace WeTennisService.API
{
    /// <summary>
    /// PayMent 的摘要说明
    /// </summary>
    public class PayMent : IHttpHandler
    {

        public void ProcessRequest(HttpContext context)
        {
             //设定Request Header
            context.Response.AddHeader("Access-Control-Allow-Origin", "*");
            context.Response.ContentType = "application/json;charset=UTF-8";

            string typename = "";
            try
            {
                typename = context.Request.QueryString["typename"].ToString();
            }
            catch {
                typename = context.Request.Params["typename"].ToString();
            }

            //获取jsonstring.
            var jsonstr = string.Empty;
            context.Request.InputStream.Position = 0;
            using (var inputStream = new StreamReader(context.Request.InputStream))
            {
                jsonstr = inputStream.ReadToEnd();
            }    

            switch (typename)
            {
                case "GetAccessToken":
                    GetAccessToken(context);
                    break;

                case "GetOrderInfo":
                    GetOrderInfo(context,jsonstr);
                    break;

                case "Charrity":
                    Charrity(context);
                    break;

                case "ConfirmPayment":
                    ConfirmPayment(context);
                    break;
            }
        }

        /// <summary>
        /// 获取用户信息
        /// </summary>
        /// <param name="context"></param>
        void GetAccessToken(HttpContext context)
        {
            Model_Return ret = new Model_Return();
            try
            {
                string _code = context.Request.Params["code"];
                ret.code = 0;
                ret.errorMsg = "";
                ret.data = Biz_WeLogin.instance.GetOpenId(_code);
            }
            catch (Exception e)
            {
                ret.code = 1;
                ret.errorMsg = e.ToString();
                ret.data = "";                
            }
            context.Response.Write(JsonHelper.ToJson(ret));
        }

        /// <summary>
        /// 获得订单信息
        /// </summary>
        /// <param name="context"></param>
        void GetOrderInfo(HttpContext context,string _jsonStr)
        {
            Model_Return ret = new Model_Return();
            try
            {
                string _orderNum = context.Request.Params["orderNum"];
                string _code = context.Request.Params["code"];
                //获取参数
                ret.code = 0;
                ret.errorMsg = "";

                //统一支付接口
                Fe_Model_OrderInfo model = new Fe_Model_OrderInfo();
                model.jsBridge= Biz_WePay.instance.GetBridge(context,_orderNum,_code);
                //添加订单详情
                OrderModel order = OrderBll.Instance("wtf").getOrderModelbySys(_orderNum);
                Model_OrderInfo oi = new Model_OrderInfo();
                oi.orderNum = _orderNum;
                oi.description = order.DESCRIPTION;
                oi.totalFee = order.SHOULDPAY;
                model.OrderInfo = oi;
                ret.data =model;
            }
            catch (Exception e)
            {
                ret.code = 1;
                ret.errorMsg = e.ToString();
                ret.data = "";
            }
            context.Response.Write(JsonHelper.ToJson(ret));
        }

        void Charrity(HttpContext context)
        {
            Model_Return ret = new Model_Return();
            try
            {
                ret.code = 0;
                ret.errorMsg = "";
                //创建订单，并返回订单信息
                OrderModel order = new OrderModel();
                order.DESCRIPTION = "charrity one cent捐款1分钱";
                order.CUST_SYS = "city02";
                order.SHOULDPAY="0.01";
                ret.data = OrderBll.Instance("wtf").AddOrderMain(order);
            }
            catch (Exception e)
            {
                ret.code = 1;
                ret.errorMsg = e.ToString();
                ret.data = "";
            }
            context.Response.Write(JsonHelper.ToJson(ret));
        }

        void ConfirmPayment(HttpContext context)
        {
            Model_Return ret = new Model_Return();
            try
            {
                string _orderNum = context.Request.Form["orderNum"];
                OrderBll.Instance("wtf").UpdateOrderStatus(_orderNum, "2", "订单支付成功");
                OrderBll.Instance("wtf").UpdateBusinessOrder(_orderNum);
                ret.code = 0;
                ret.errorMsg = "";
                ret.data = "订单修改成功";
            }
            catch (Exception e)
            {
                ret.code = 1;
                ret.errorMsg = e.ToString();
                ret.data = "";
            }
            context.Response.Write(JsonHelper.ToJson(ret));
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