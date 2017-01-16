using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using SMS;
using Basic;

namespace WeTennisService.WebService
{
    /// <summary>
    /// SMS 的摘要说明
    /// </summary>
    public class SMS : IHttpHandler
    {

        public void ProcessRequest(HttpContext context)
        {
            context.Response.ContentType = "text/plain";
            string typename = "";
            try
            {
                typename = context.Request.QueryString["typename"].ToString();
            }
            catch {
                typename = context.Request.Params["typename"].ToString();
            }
            switch (typename)
            { 
                    //发送code
                case "GenerateCode":
                    GenerateCode(context);
                    break;

                case "GenerateCodeXML":
                    GenerateCodeXML(context);
                    break;
                case "GenerateCodeInter":
                    GenerateCodeInter(context);
                    break;

                case "ValidateCode":
                    ValidateCode(context);
                    break;

                case "ValidateCodeXML":
                    ValidateCodeXML(context);
                    break;

                case "ValidateCodeXML1":
                    ValidateCodeXML1(context);
                    break;

                #region SendInform
                case "SendInformSms":
                    SendInformSms(context);
                    break;
                #endregion

            }
        }

        #region Telephone
        /// <summary>
        /// 创建code并发送到手机
        /// </summary>
        /// <param name="context"></param>
        void GenerateCode(HttpContext context)
        {
            //处理回调函数
            context.Response.ContentType = "application/json;charset=utf-8";
            string jsonCallBackFunName = context.Request.Params["jsoncallback"].ToString();

            string _Telephone = context.Request.Params["tele"];
            WriteLog("GenerateCode", _Telephone);
            string code = TeleCheckDll.instance.GreateCode(_Telephone);
            string mes = "ok";
            if (code == "FALSE")
            {
                mes = "no";
            }

            //处理查询结果
            context.Response.Write(string.Format("{0}({1})", jsonCallBackFunName, "{\"message\":\""+mes+"\"}"));
            context.Response.End();
        }

        /// <summary>
        /// 生成验证码
        /// </summary>
        /// <param name="context"></param>
        void GenerateCodeXML(HttpContext context)
        {
            #region validate Origin
            string _Uid = context.Request.Params["userid"];
            string _Secret = context.Request.Params["secret"];
            string _Origin = Biz_Basic_UserOrigin.instance.GetUserOriginbyCredential(_Uid, _Secret, "GenerateCodeXML");
            if (_Origin != "")
            {
                context.Response.AddHeader("Access-Control-Allow-Origin", _Origin); //添加允许跨域访问的请求头文件
            }
            #endregion

            string _Telephone = context.Request.Params["tele"];
            string code = TeleCheckDll.instance.GreateCode(_Telephone);
            string mes = "ok";
            if (code == "FALSE")
            {
                mes = "no";
            }

            context.Response.Write(mes);
            context.Response.End();
        }

        /// <summary>
        /// 生成验证码
        /// </summary>
        /// <param name="context"></param>
        void GenerateCodeInter(HttpContext context)
        {
            
            context.Response.AddHeader("Access-Control-Allow-Origin", "*"); //添加允许跨域访问的请求头文件
          
            string _Telephone = context.Request.Params["tele"];
            string code = TeleCheckDll.instance.GreateCode(_Telephone);
            string mes = "0";
            if (code == "FALSE")
            {
                mes = "1";
            }

            context.Response.Write("{code:"+mes+"}");
            context.Response.End();
        }



        /// <summary>
        /// 验证code
        /// </summary>
        /// <param name="context"></param>
        void ValidateCode(HttpContext context)
        {
            //处理回调函数
            context.Response.ContentType = "application/json;charset=utf-8";
            string jsonCallBackFunName = context.Request.Params["jsoncallback"].ToString();

            string _Telephone = context.Request.Params["tele"];
            string _Code = context.Request.Params["code"];
            WriteLog("ValidateCode", _Telephone + "-" + _Code);
            string mes = "";
            if (TeleCheckDll.instance.ValidateCode(_Telephone, _Code))
            {                 
                mes="ok";
            }
            else
            {
                mes="no";
            }
            
            //处理查询结果
            context.Response.Write(string.Format("{0}({1})", jsonCallBackFunName, "{\"message\":\"" + mes + "\"}"));
            context.Response.End();
        }

        /// <summary>
        /// 验证code
        /// </summary>
        /// <param name="context"></param>
        void ValidateCodeXML(HttpContext context)
        {
            #region validate Origin
            string _Uid = context.Request.Params["userid"];
            string _Secret = context.Request.Params["secret"];
            string _Origin = Biz_Basic_UserOrigin.instance.GetUserOriginbyCredential(_Uid, _Secret, "ValidateCodeXML");
            if (_Origin != "")
            {
                context.Response.AddHeader("Access-Control-Allow-Origin", _Origin); //添加允许跨域访问的请求头文件
            }
            #endregion

            string _Telephone = context.Request.Params["tele"];
            string _Code = context.Request.Params["code"];
            
            string mes = "";
            if (TeleCheckDll.instance.ValidateCode(_Telephone, _Code))
            {
                mes = "ok";
            }
            else
            {
                mes = "no";
            }
            //处理查询结果
            context.Response.Write(mes);
            context.Response.End();
        }

        void ValidateCodeXML1(HttpContext context)
        {
            #region validate Origin
          
                context.Response.AddHeader("Access-Control-Allow-Origin", "*"); //添加允许跨域访问的请求头文件
            
            #endregion

            string _Telephone = context.Request.Params["tele"];
            string _Code = context.Request.Params["code"];

            string mes = "";
            if (TeleCheckDll.instance.ValidateCode(_Telephone, _Code))
            {
                mes = "ok";
            }
            else
            {
                mes = "no";
            }
            //处理查询结果
            context.Response.Write(mes);
            context.Response.End();
        }

        #endregion

        
        #region SendInformSms
        void SendInformSms(HttpContext context)
        {
            //处理回调函数
            context.Response.ContentType = "application/json;charset=utf-8";
            string jsonCallBackFunName = context.Request.Params["jsoncallback"].ToString();
            WriteLog("SendInformSms", "getin");
            SendReport.instance.SendReportToLastYear();

            //处理查询结果
            context.Response.Write(string.Format("{0}({1})", jsonCallBackFunName, "{\"message\":\" ok\"}"));
            context.Response.End();
        }
        #endregion

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