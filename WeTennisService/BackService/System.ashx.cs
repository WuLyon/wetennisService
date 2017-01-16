using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WeTennisService.BackService
{
    /// <summary>
    /// System 的摘要说明
    /// </summary>
    public class System : IHttpHandler
    {

        public void ProcessRequest(HttpContext context)
        {
            context.Response.ContentType = "text/plain";
            string typename = context.Request.Params["typename"].ToString();
            switch (typename)
            { 
                case "GetInsert":
                    GetInsert(context);
                    break;

                case "GetEntity":
                    GetEntity(context);
                    break;
            }
        }

        void GetInsert(HttpContext context)
        {
            context.Response.AddHeader("Access-Control-Allow-Origin", "*");
            string _TableName=context.Request.Params["tablename"];
            string _TableField=context.Request.Params["field"];
           string sql= CRUDsqlstr.instance.GetInsert(_TableName, _TableField);
           context.Response.Write(sql);

        }

        void GetEntity(HttpContext context)
        {
            context.Response.AddHeader("Access-Control-Allow-Origin", "*");
            string _TableField = context.Request.Params["field"];
            string sql = CRUDsqlstr.instance.GetEntity( _TableField);
            context.Response.Write(sql);
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