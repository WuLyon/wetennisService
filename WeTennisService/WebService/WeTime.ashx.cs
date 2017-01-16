using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Time;

namespace WeTennisService.WebService
{
    /// <summary>
    /// WeTime 的摘要说明
    /// </summary>
    public class WeTime : IHttpHandler
    {

        public void ProcessRequest(HttpContext context)
        {
            context.Response.ContentType = "text/plain";
            string typename = context.Request.QueryString["typename"].ToString();
            switch (typename) {
                case "LoadTimebyMemsys":
                    LoadTimebyMemsys(context);
                    break;

                case "AddTimebyDesc":
                    AddTimebyDesc(context);
                    break;

                case "GetTimeOfPic":
                    GetTimeOfPic(context);
                    break;
                case "GetPicContextList":
                    GetPicContextList(context);
                    break;
                case "CheckIfUploader":
                    CheckIfUploader(context);
                    break;

                case "DeletePicWork":
                    DeletePicWork(context);
                    break;
            }
        }

        /// <summary>
        /// 根据会员主键和是否公开来获取时光列表
        /// </summary>
        /// <param name="context"></param>
        void LoadTimebyMemsys(HttpContext context)
        {
            //处理回调函数
            context.Response.ContentType = "application/json;charset=utf-8";
            string jsonCallBackFunName = context.Request.Params["jsoncallback"].ToString();

            string Memsys = context.Request.Params["memsys"].ToString();
            string IsOpen=context.Request.Params["isopen"].ToString();
            
            WriteLog("LoadTimebyMemsys", Memsys + "-" + IsOpen);
            List<MyTimeModel> list = TimesBll.instance.GetMyTimeList(Memsys, IsOpen);
            string json = JsonHelper.ToJson(list);

            //处理查询结果
            context.Response.Write(string.Format("{0}({1})", jsonCallBackFunName, json));
            context.Response.End();
        }

        /// <summary>
        /// 添加时光，根据时光描述
        /// </summary>
        /// <param name="context"></param>
        void AddTimebyDesc(HttpContext context)
        {
            //处理回调函数
            context.Response.ContentType = "application/json;charset=utf-8";
            string jsonCallBackFunName = context.Request.Params["jsoncallback"].ToString();

            string MemSys = context.Request.Params["memsys"].ToString();
            string Type = context.Request.Params["type"].ToString();
            string Description = context.Request.Params["desc"].ToString();
            if (MemSys.IndexOf(",") > 0)
            {
                MemSys = MemSys.Split(',')[0];
            }
            WriteLog("AddTimebyDesc", MemSys + "-" + Type+"-"+Description);
          
            TimeModel model = new TimeModel();
            model.MEMSYS = MemSys;
            model.TYPE = Type;
            model.DESCRIPTION = Description;
            TimeDAL.instance.InsertNew(model);

            //处理查询结果
            context.Response.Write(string.Format("{0}({1})", jsonCallBackFunName, "{\"message\":\"ok\"}"));
            context.Response.End();
        }

        void GetTimeOfPic(HttpContext context)
        {
            //处理回调函数
            context.Response.ContentType = "application/json;charset=utf-8";
            string jsonCallBackFunName = context.Request.Params["jsoncallback"].ToString();

            string _TimeSys = context.Request.Params["sys"];
            string _Type = context.Request.Params["type"];

            TimeModel model = new TimeModel();
            if (_Type == "mem")
            {
                try
                {
                    model = TimeDAL.instance.GetModelByMemsys(_TimeSys);
                    WriteLog("memok", model.SYS);
                }
                catch (Exception e)
                {
                    WriteLog("memero", e.ToString());
                }
            }
            else
            {
                model = TimeDAL.instance.GetModlebySys(_TimeSys);
            }

            string json = JsonHelper.ToJson(model);

            //处理查询结果
            context.Response.Write(string.Format("{0}({1})", jsonCallBackFunName, json));
            context.Response.End();
        }

        /// <summary>
        /// 获取图片评选数量
        /// </summary>
        /// <param name="context"></param>
        void GetPicContextList(HttpContext context)
        {
            //处理回调函数
            context.Response.ContentType = "application/json;charset=utf-8";
            string jsonCallBackFunName = context.Request.Params["jsoncallback"].ToString();

            string _PageSize = context.Request.Params["pagesize"];
            string _Page = context.Request.Params["page"];
            string _Type = context.Request.Params["type"];
            List<TimeModel> list = TimeDAL.instance.GetPicContestList(Convert.ToInt32(_PageSize),Convert.ToInt32(_Page),_Type);

            string json = JsonHelper.ToJson(list);

            //处理查询结果
            context.Response.Write(string.Format("{0}({1})", jsonCallBackFunName, json));
            context.Response.End();
        }

        /// <summary>
        /// 判断当前登录人是否已发起过作品
        /// </summary>
        /// <param name="context"></param>
        void CheckIfUploader(HttpContext context)
        {
            //处理回调函数
            context.Response.ContentType = "application/json;charset=utf-8";
            string jsonCallBackFunName = context.Request.Params["jsoncallback"].ToString();

            string result;
            string memsys = context.Request.Params["memsys"];
            if (memsys.IndexOf(",") > 0)
            {
                memsys = memsys.Split(',')[0];
            }


            WriteLog("checkifUp", memsys);
            TimeModel model = TimeDAL.instance.GetModelByMemsys(memsys);
            if (string.IsNullOrEmpty(model.SYS))
            {
                result = "no";
            }
            else
            {
                result = "yes";
            }

            //处理查询结果
            context.Response.Write(string.Format("{0}({1})", jsonCallBackFunName, "{\"message\":\"" + result + "\"}"));
            context.Response.End();
        }

        void DeletePicWork(HttpContext context)
        {
            //处理回调函数
            context.Response.ContentType = "application/json;charset=utf-8";
            string jsonCallBackFunName = context.Request.Params["jsoncallback"].ToString();

            string sys = context.Request.Params["sys"];
            string result;
            if (TimeDAL.instance.DeletePicWork(sys))
            {
                result = "ok";
            }
            else
            {
                result = "no";
            }

            //处理查询结果
            context.Response.Write(string.Format("{0}({1})", jsonCallBackFunName, "{\"message\":\"" + result + "\"}"));
            context.Response.End();
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