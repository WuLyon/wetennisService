using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.IO;

namespace WeTennisService.WebService
{
    /// <summary>
    /// PicUploadBase 的摘要说明
    /// </summary>
    public class PicUploadBase : IHttpHandler
    {

        public void ProcessRequest(HttpContext context)
        {
            //处理回调函数
            context.Response.ContentType = "application/json;charset=utf-8";
            string jsonCallBackFunName = context.Request.Params["jsoncallback"].ToString();
            
            string FileName = Guid.NewGuid().ToString().ToUpper();
            string fileName = FileName + ".jpg";
            string path = @"D:\\Resource\images\Head\" + fileName;
            //string _Sysno = context.Request.QueryString["sysno"].ToString();
            //string _Type = context.Request.QueryString["type"].ToString();
            try
            {
                string base64 = context.Request.Params["base"];//读取base64编码
                System.IO.FileStream fs = new System.IO.FileStream(path, System.IO.FileMode.Create);
                System.IO.BinaryWriter bw = new System.IO.BinaryWriter(fs);
                if (!string.IsNullOrEmpty(base64) && File.Exists(path))
                {
                    bw.Write(Convert.FromBase64String(base64));//存储图片

                    
                }
                bw.Close();
                fs.Close();
            }
            catch (Exception e)
            {              
 
            }
            //处理查询结果
            context.Response.Write(string.Format("{0}({1})", jsonCallBackFunName, "{\"message\":\"ok\"}"));
            context.Response.End();
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