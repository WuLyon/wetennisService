using System;
using System.Collections;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Xml.Linq;
using WeiPay;
using Newtonsoft.Json;
using System.Web.Script.Serialization;
using OrderLib;

namespace WeiPayWeb
{
    /**
   * 
   * 作用：模拟填写支付信息页面
   * 作者：邹学典
   * 编写日期：2014-12-25
   * 备注：请正确输入支付的信息，部分参数不能为空。具体请查看 WeiPay类库中PayModel类。
   * 
   * */
    public partial class Send : System.Web.UI.Page
    {
        private string UserOpenId = ""; //微信用户openid；

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!this.IsPostBack)
            {
                InitiateData();
                //获取当前用户的OpenId，如果可以通过系统获取用户Openid就不用调用该函数
                this.GetUserOpenId();
                this.lblOpenId.Text = this.UserOpenId;
            }

        }

        void InitiateData()
        {
            string OrderNo = Request.QueryString["orderid"].ToString();
            OrderModel model = OrderBll.Instance("wtf").getOrderModelbySys(OrderNo);

            this.txtBody.Text = model.DESCRIPTION;
            this.txtOrderSN.Text = OrderNo;
            this.txtOther.Text = model.TYPE;
            this.txtPrice.Text = model.ORDERMONEY;
        }

        /// <summary>
        /// 获取当前用户的微信 OpenId，如果知道用户的OpenId请不要使用该函数
        /// </summary>
        private void GetUserOpenId()
        {
            var client = new System.Net.WebClient();
            client.Encoding = System.Text.Encoding.UTF8;
            string OrderNo = Request.QueryString["orderid"].ToString();
            string code = Request.QueryString["code"];
            string backurl = PayConfig.SendUrl + "?orderid=" + OrderNo;
            LogUtil.WriteLog("PayConfig.AppId:" + PayConfig.AppId + " PayConfig.SendUrl:" + backurl);
            if (string.IsNullOrEmpty(code))
            {
                string code_url = string.Format("https://open.weixin.qq.com/connect/oauth2/authorize?appid={0}&redirect_uri={1}&orderid={2}&response_type=code&scope=snsapi_base&state=lk#wechat_redirect", PayConfig.AppId, backurl, OrderNo);
                Response.Redirect(code_url);
            }
            else
            {
                LogUtil.WriteLog(" ============ 开始 获取微信用户相关信息 =====================");

                #region 获取支付用户 OpenID
                string url = string.Format("https://api.weixin.qq.com/sns/oauth2/access_token?appid={0}&secret={1}&code={2}&grant_type=authorization_code", PayConfig.AppId, PayConfig.AppSecret, code);
                string returnStr = HttpUtil.Send("", url);
                LogUtil.WriteLog("Send 页面  returnStr 第一个：" + returnStr);

                var obj = JsonConvert.DeserializeObject<OpenModel>(returnStr);

                url = string.Format("https://api.weixin.qq.com/sns/oauth2/refresh_token?appid={0}&grant_type=refresh_token&refresh_token={1}", PayConfig.AppId, obj.refresh_token);
                returnStr = HttpUtil.Send("", url);
                obj = JsonConvert.DeserializeObject<OpenModel>(returnStr);

                LogUtil.WriteLog("Send 页面  access_token：" + obj.access_token);
                LogUtil.WriteLog("Send 页面  openid=" + obj.openid);

                url = string.Format("https://api.weixin.qq.com/sns/userinfo?access_token={0}&openid={1}", obj.access_token, obj.openid);
                returnStr = HttpUtil.Send("", url);
                LogUtil.WriteLog("Send 页面  returnStr：" + returnStr);

               this.UserOpenId = obj.openid;

               LogUtil.WriteLog(" ============ 结束 获取微信用户相关信息 =====================");
                #endregion
            }
        }


        /// <summary>
        /// 提交支付信息
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void BtnSave_Click(object sender, EventArgs e)
        {
            //设置支付数据
            PayModel model = new PayModel();
            model.OrderSN = this.txtOrderSN.Text;
            model.TotalFee = Convert.ToInt32((Convert.ToDecimal(this.txtPrice.Text)*100));
            model.Body = this.txtBody.Text;
            model.Attach = this.txtOther.Text; //不能有中文
            model.OpenId = this.lblOpenId.Text;

            //跳转到 WeiPay.aspx 页面，请设置函数中WeiPay.aspx的页面地址
            this.Response.Redirect(model.ToString());
        }
    }

  
}
