using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using News;
using SMS;
using System.Text;
using Basic;

namespace WeTennisService.WebService
{
    /// <summary>
    /// WeNews 的摘要说明
    /// </summary>
    public class WeNews : IHttpHandler
    {

        public void ProcessRequest(HttpContext context)
        {
            context.Response.ContentType = "text/plain";
            
            string typename = context.Request.QueryString["typename"].ToString();
            WriteLog("typename", typename);
            switch (typename)
            {
                #region 加载内容和评论
                case "LoadNews":
                    LoadNews(context);
                    break;

                case "LoadNewsPagePar":
                    LoadNewsPagePar(context);
                    break;

                case "GetHtmlDesc":
                    GetHtmlDesc(context);
                    break;

                case "GetNewsHtmlDescXML":
                    GetNewsHtmlDescXML(context);
                    break;

                case "GetNewsInfo":
                    GetNewsInfo(context);
                    break;

                case "GetNewsInfobySys":
                    GetNewsInfobySys(context);
                    break;

                case "LoadHotComment":
                    LoadHotComment(context);
                    break;

                case "LoadAllComments":
                    LoadAllComments(context);
                    break;
                #endregion

                #region 处理新闻
                case "DeleteNews":
                    DeleteNews(context);
                    break;
                #endregion

                #region 评论
                case "AddComment":
                    AddComment(context);
                    break;
                case "AddPraise":
                    AddPraise(context);
                    break;

                case "GetPraiseQty":
                    GetPraiseQty(context);
                    break;
            
                #endregion

                #region CORS
                case "LoadNewsXml":
                    LoadNewsXml(context);
                    break;

                case "GetNewsInfoXML":
                    GetNewsInfoXML(context);
                    break;

                case "GetNewsInfobySysXML":
                    GetNewsInfobySysXML(context);
                    break;

                case "LoadAllCommentsXML":
                    LoadAllCommentsXML(context);
                    break;

                case "AddCommentXML":
                    AddCommentXML(context);
                    break;

                case "AddPraiseXML":
                    AddPraiseXML(context);
                    break;

                case "GetPraiseQtyXML":
                    GetPraiseQtyXML(context);
                    break;
                #endregion

                case "TetsConnect":
                    TetsConnect(context);
                    break;
            }
        }

        void TetsConnect(HttpContext context)
        {
            string _Uid = context.Request.Params["userid"];
            string _Secret = context.Request.Params["secret"];
            string _Origin = Biz_Basic_UserOrigin.instance.GetUserOriginbyCredential(_Uid, _Secret, "LoadNewsXml");
            context.Response.AddHeader("Access-Control-Allow-Origin", _Origin);

            List<NewsModel> list = NewsBiz.instance.GetNewsList("Pro", "10", "1", "0");
            context.Response.Write("_Origin:" + list.Count.ToString());
            context.Response.End();
        }


        #region 获取新闻的方法
        /// <summary>
        /// 加载新闻
        /// </summary>
        /// <param name="context"></param>
        void LoadNews(HttpContext context)
        {
            //处理回调函数
            context.Response.ContentType = "application/json;charset=utf-8";
            string jsonCallBackFunName = context.Request.Params["jsoncallback"].ToString();

            string _Type = context.Request.Params["type"].ToString();
            string _PageSize = context.Request.Params["pagesize"].ToString();
            string _Page = context.Request.Params["page"].ToString();
            string _Status = context.Request.Params["status"].ToString();
            WriteLog("LoadNews", _Type + "-" + _PageSize + "-" + _Page);
            List<NewsModel> list = NewsBiz.instance.GetNewsList(_Type, _PageSize, _Page, _Status);
            string json = JsonHelper.ToJson(list);

            //添加数量
            int NewsQty = NewsBiz.instance.GetNewsQty(_Type, _Status);
            int PageQty = NewsQty / Convert.ToInt32(_PageSize);
            string jsonp = "{\"qty\":\"" + NewsQty + "\",\"pageq\":\"" + PageQty + "\",\"content\":" + json + "}";

            //处理查询结果
            context.Response.Write(string.Format("{0}({1})", jsonCallBackFunName, jsonp));
            context.Response.End();
        }

        void LoadNewsXml(HttpContext context)
        {          
            string _Uid = context.Request.Params["userid"];
            string _Secret = context.Request.Params["secret"];
            string _Origin = Biz_Basic_UserOrigin.instance.GetUserOriginbyCredential(_Uid, _Secret, "LoadNewsXml");
            context.Response.AddHeader("Access-Control-Allow-Origin", _Origin); //添加允许跨域访问的请求头文件

            string _Type = context.Request.Params["type"].ToString();
            string _PageSize = context.Request.Params["pagesize"].ToString();
            string _Page = context.Request.Params["page"].ToString();
            string _Status = context.Request.Params["status"].ToString();
            List<NewsModel> list = NewsBiz.instance.GetNewsList(_Type, _PageSize, _Page, _Status);
            string json = JsonHelper.ToJson(list);
            string retjson = "{code:0,erromsg:\"\",data:"+json+"}";

            context.Response.Write(retjson);
            context.Response.End();

        }

        /// <summary>
        /// 获得新闻页面的页数
        /// </summary>
        /// <param name="context"></param>
        void LoadNewsPagePar(HttpContext context)
        {
            //处理回调函数
            context.Response.ContentType = "application/json;charset=utf-8";
            string jsonCallBackFunName = context.Request.Params["jsoncallback"].ToString();

            string _Type = context.Request.Params["type"].ToString();
            string _PageSize = context.Request.Params["pagesize"].ToString();
            string _Status = context.Request.Params["status"].ToString();

            //添加数量
            int NewsQty = NewsBiz.instance.GetNewsQty(_Type, _Status);
            int PageQty = NewsQty / Convert.ToInt32(_PageSize) + 1;
            string jsonp = "{\"qty\":\"" + NewsQty + "\",\"pageq\":\"" + PageQty + "\"}";

            //处理查询结果
            context.Response.Write(string.Format("{0}({1})", jsonCallBackFunName, jsonp));
            context.Response.End();
        }

        /// <summary>
        /// 获得描述的html
        /// </summary>
        /// <param name="context"></param>
        void GetHtmlDesc(HttpContext context)
        {
            //处理回调函数
            context.Response.ContentType = "application/json;charset=utf-8";
            string jsonCallBackFunName = context.Request.Params["jsoncallback"].ToString();

            string type = context.Request.Params["type"].ToString();
            string typesys = context.Request.Params["typesys"].ToString();
            WriteLog("GetHtmlDesc", type + "-" + typesys);
            string str = "";

            string html = DescBll.instance.GetContent(type, typesys);
            //WriteLog("GetHtmlDes-before", html);
            //使用base64编码
            html = System.Web.HttpUtility.UrlEncode(html);//编码前，先对中文进行编码
            //WriteLog("GetHtmlDes-mid", html);
            byte[] bytes = Encoding.Default.GetBytes(html);

            str = Convert.ToBase64String(bytes);
            //WriteLog("GetHtmlDesc-after", str);

            //处理查询结果
            context.Response.Write(string.Format("{0}({1})", jsonCallBackFunName, "{html:\"" + str + "\"}"));
            context.Response.End();
        }

        void GetNewsHtmlDescXML(HttpContext context)
        {
            context.Response.AddHeader("Access-Control-Allow-Origin", "*");

            string type = context.Request.Params["type"].ToString();
            string typesys = context.Request.Params["typesys"].ToString();
            WriteLog("GetHtmlDesc", type + "-" + typesys);

            string html = DescBll.instance.GetContent(type, typesys);            

            context.Response.Write(html);
            context.Response.End();
        }

        /// <summary>
        /// 获取新闻实体
        /// 输入：新闻编号
        /// 输出：新闻实体
        /// </summary>
        /// <param name="context"></param>
        void GetNewsInfo(HttpContext context)
        {
            //处理回调函数
            context.Response.ContentType = "application/json;charset=utf-8";
            string jsonCallBackFunName = context.Request.Params["jsoncallback"].ToString();

            string newsid = context.Request.Params["newsid"].ToString();
            WriteLog("GetNewsInfo", newsid);
            NewsModel model = NewsBiz.instance.GetModel(newsid);

            string json = JsonHelper.ToJson(model);

            //处理查询结果
            context.Response.Write(string.Format("{0}({1})", jsonCallBackFunName, json));
            context.Response.End();
        }

        void GetNewsInfoXML(HttpContext context)
        {
            #region validate Origin
            string _Uid = context.Request.Params["userid"];
            string _Secret = context.Request.Params["secret"];
            string _Origin = Biz_Basic_UserOrigin.instance.GetUserOriginbyCredential(_Uid, _Secret, "GetNewsInfoXML");
            if (_Origin != "")
            {
                context.Response.AddHeader("Access-Control-Allow-Origin", _Origin); //添加允许跨域访问的请求头文件
            }
            #endregion

            string newsid = context.Request.Params["newsid"].ToString();
            NewsModel model = NewsBiz.instance.GetModel(newsid);

            string json = JsonHelper.ToJson(model);

            context.Response.Write(json);
            context.Response.End();
        }

        void GetNewsInfobySysXML(HttpContext context)
        {
            #region validate Origin
            string _Uid = context.Request.Params["userid"];
            string _Secret = context.Request.Params["secret"];
            string _Origin = Biz_Basic_UserOrigin.instance.GetUserOriginbyCredential(_Uid, _Secret, "GetNewsInfobySysXML");
            if (_Origin != "")
            {
                context.Response.AddHeader("Access-Control-Allow-Origin", _Origin); //添加允许跨域访问的请求头文件
            }
            #endregion

            string newsid = context.Request.Params["sysno"].ToString();
            NewsModel model = NewsBiz.instance.GetModelbySys(newsid);

            string json = JsonHelper.ToJson(model);

            context.Response.Write(json);
            context.Response.End();
        }

        /// <summary>
        /// 获取新闻实体
        /// 输入：新闻主键
        /// 输出：新闻实体
        /// </summary>
        /// <param name="context"></param>
        void GetNewsInfobySys(HttpContext context)
        {
            //处理回调函数
            context.Response.ContentType = "application/json;charset=utf-8";
            string jsonCallBackFunName = context.Request.Params["jsoncallback"].ToString();

            string newsid = context.Request.Params["sysno"].ToString();
            WriteLog("GetNewsInfobySys", newsid);
            NewsModel model = NewsBiz.instance.GetModelbySys(newsid);

            string json = JsonHelper.ToJson(model);

            //处理查询结果
            context.Response.Write(string.Format("{0}({1})", jsonCallBackFunName, json));
            context.Response.End();
        }




        /// <summary>
        /// 加载热门评论
        /// 输入:1.type,默认为News,2.type sysno
        /// 输出：List<CommentModel>,评论的实体清单
        /// </summary>
        /// <param name="context"></param>
        void LoadHotComment(HttpContext context)
        {
            //处理回调函数
            context.Response.ContentType = "application/json;charset=utf-8";
            string jsonCallBackFunName = context.Request.Params["jsoncallback"].ToString();

            string type = context.Request.Params["type"].ToString();
            string typesys = context.Request.Params["typesys"].ToString();
            WriteLog("LoadHotComment", type + "-" + typesys);
            List<CommentModel> list = CommentBll.instance.GetHotComments(typesys, type);
            string json = JsonHelper.ToJson(list);

            //处理查询结果
            context.Response.Write(string.Format("{0}({1})", jsonCallBackFunName, json));
            context.Response.End();
        }

        /// <summary>
        /// 加载所有评论
        /// 输入:1.type,默认为News,2.type sysno
        /// 输出：List<CommentModel>,评论的实体清单
        /// </summary>
        /// <param name="context"></param>
        void LoadAllComments(HttpContext context)
        {
            //处理回调函数
            context.Response.ContentType = "application/json;charset=utf-8";
            string jsonCallBackFunName = context.Request.Params["jsoncallback"].ToString();

            string type = context.Request.Params["type"].ToString();
            string typesys = context.Request.Params["typesys"].ToString();
            WriteLog("LoadAllComments", type + "-" + typesys);
            List<CommentModel> list = CommentBll.instance.GetComList(typesys, type);
            string json = JsonHelper.ToJson(list);

            //处理查询结果
            context.Response.Write(string.Format("{0}({1})", jsonCallBackFunName, json));
            context.Response.End();
        }

        void LoadAllCommentsXML(HttpContext context)
        {
            #region validate Origin
            string _Uid = context.Request.Params["userid"];
            string _Secret = context.Request.Params["secret"];
            string _Origin = Biz_Basic_UserOrigin.instance.GetUserOriginbyCredential(_Uid, _Secret, "LoadAllCommentsXML");
            if (_Origin != "")
            {
                context.Response.AddHeader("Access-Control-Allow-Origin", _Origin); //添加允许跨域访问的请求头文件
            }
            #endregion

            string type = context.Request.Params["type"].ToString();
            string typesys = context.Request.Params["typesys"].ToString();
            List<CommentModel> list = CommentBll.instance.GetComList(typesys, type);
            string json = JsonHelper.ToJson(list);

            //处理查询结果
            context.Response.Write(json);
            context.Response.End();
        }


        #endregion

        #region 新闻处理
        /// <summary>
        /// 删除新闻
        /// 2016-2-20，liutao
        /// </summary>
        /// <param name="context"></param>
        void DeleteNews(HttpContext context)
        {
            //处理回调函数
            context.Response.ContentType = "application/json;charset=utf-8";
            string jsonCallBackFunName = context.Request.Params["jsoncallback"].ToString();

            string id = context.Request.Params["sysno"];
            WriteLog("DeleteNews", id);
            string result="";
            if (NewsBiz.instance.DeleteNews(id))
            {
                result = "ok";
            }
            else
            {
                result = "no";
            }

            //处理查询结果
            context.Response.Write(string.Format("{0}({1})", jsonCallBackFunName, "{\"message\":\""+result+"\"}"));
            context.Response.End();
        }

        /// <summary>
        /// 创建新闻
        /// </summary>
        /// <param name="context"></param>
        void CreateNews(HttpContext context)
        {
            //处理回调函数
            context.Response.ContentType = "application/json;charset=utf-8";
            string jsonCallBackFunName = context.Request.Params["jsoncallback"].ToString();

            string type = context.Request.Params["type"];
            string title = context.Request.Params["title"];
            string writer = context.Request.Params["writer"];
            string img = context.Request.Params["newsimg"];
            WriteLog("CreateNews", type + "-" + title + "-" + writer + "-" + img);
            NewsModel model = new NewsModel();
            model.TYPE = type;
            model.TITLE = title;
            model.WRITER = writer;
            model.IMGURL = img;
            model.SMALLURL = img;
            model.STATUS = "1";
            model.ISSUETIME = DateTime.Now.ToString();
            string sysno = NewsBiz.instance.InsertNews(model);
            
            //处理查询结果
            context.Response.Write(string.Format("{0}({1})", jsonCallBackFunName, "{\"message\":\"" + sysno + "\"}"));
            context.Response.End();
        }
        #endregion

        #region 添加评论
        /// <summary>
        /// 添加评论，可以对新闻，对比赛等内容进行评论，均使用该方法，通过type来进行区分
        /// </summary>
        /// <param name="context"></param>
        void AddComment(HttpContext context)
        {
            //处理回调函数
            context.Response.ContentType = "application/json;charset=utf-8";
            string jsonCallBackFunName = context.Request.Params["jsoncallback"].ToString();

            //业务处理
            string memsys = context.Request.Params["memsys"].ToString();
            string type = context.Request.Params["type"].ToString();
            string typesys = context.Request.Params["typesys"].ToString();
            string comments = context.Request.Params["comment"].ToString();
            WriteLog("AddComment", memsys + "-" + type + "-" + typesys + "-" + comments);
            CommentModel model = new CommentModel();
            model.MEMSYSNO = memsys;
            model.TYPE = type;
            model.TYPESYSNO = typesys;
            model.COMMENTS = comments;
            CommentBll.instance.InsertComment(model);

            //处理结果
            context.Response.Write(string.Format("{0}({1})", jsonCallBackFunName, "{message:\"ok\"}"));
            context.Response.End();
        }

        void AddCommentXML(HttpContext context)
        {
            #region validate Origin
            string _Uid = context.Request.Params["userid"];
            string _Secret = context.Request.Params["secret"];
            string _Origin = Biz_Basic_UserOrigin.instance.GetUserOriginbyCredential(_Uid, _Secret, "AddCommentXML");
            if (_Origin != "")
            {
                context.Response.AddHeader("Access-Control-Allow-Origin", _Origin); //添加允许跨域访问的请求头文件

                string memsys = context.Request.Params["memsys"].ToString();
                string type = context.Request.Params["type"].ToString();
                string typesys = context.Request.Params["typesys"].ToString();
                string comments = context.Request.Params["comment"].ToString();
                WriteLog("AddComment", memsys + "-" + type + "-" + typesys + "-" + comments);
                CommentModel model = new CommentModel();
                model.MEMSYSNO = memsys;
                model.TYPE = type;
                model.TYPESYSNO = typesys;
                model.COMMENTS = comments;
                CommentBll.instance.InsertComment(model);
            }
            #endregion

            context.Response.Write("ok");
            context.Response.End();
        }

        /// <summary>
        /// 点赞
        /// </summary>
        /// <param name="context"></param>
        void AddPraise(HttpContext context)
        {
            //处理回调函数
            context.Response.ContentType = "application/json;charset=utf-8";
            string jsonCallBackFunName = context.Request.Params["jsoncallback"].ToString();

            //业务处理
            string _type = context.Request.Params["type"].ToString();
            string _Typesys = context.Request.Params["typesys"].ToString();
            string _IsGood = context.Request.Params["isgood"].ToString();
            string _memsys = context.Request.Params["memsys"].ToString();

            if (_memsys.IndexOf(",") > 0)
            {
                _memsys=_memsys.Split(',')[0];
            }

            WriteLog("AddPraise", _type + "-" + _Typesys + "-" + _IsGood + "-" + _memsys);
            
            PraiseModel model = new PraiseModel();
            model.DTYPE = _type;
            model.TYPESYSNO = _Typesys;
            model.ISGOOD = _IsGood;
            model.MEMSYS = _memsys;
            bool Reust = true;
            if (_type == "pic")
            {
                Reust=PraiseBll.instance.AddPicPraiseInday(model);
            }
            else
            {
                Reust = PraiseBll.instance.AddPraise(model);
            }
            string Message = "";
            if (Reust)
            {
                Message = "ok";
            }
            else
            {
                Message = "no";
            }

            //处理结果
            context.Response.Write(string.Format("{0}({1})", jsonCallBackFunName, "{message:\"" + Message + "\"}"));
            context.Response.End();
        }

        void AddPraiseXML(HttpContext context)
        { 
              #region validate Origin
            string _Uid = context.Request.Params["userid"];
            string _Secret = context.Request.Params["secret"];
            string _Origin = Biz_Basic_UserOrigin.instance.GetUserOriginbyCredential(_Uid, _Secret, "AddPraiseXML");
            if (_Origin != "")
            {
                context.Response.AddHeader("Access-Control-Allow-Origin", _Origin); //添加允许跨域访问的请求头文件

                string _type = context.Request.Params["type"].ToString();
                string _Typesys = context.Request.Params["typesys"].ToString();
                string _IsGood = context.Request.Params["isgood"].ToString();
                string _memsys = context.Request.Params["memsys"].ToString();

                if (_memsys.IndexOf(",") > 0)
                {
                    _memsys = _memsys.Split(',')[0];
                }

                WriteLog("AddPraise", _type + "-" + _Typesys + "-" + _IsGood + "-" + _memsys);

                PraiseModel model = new PraiseModel();
                model.DTYPE = _type;
                model.TYPESYSNO = _Typesys;
                model.ISGOOD = _IsGood;
                model.MEMSYS = _memsys;
                bool Reust = true;
                if (_type == "pic")
                {
                    Reust = PraiseBll.instance.AddPicPraiseInday(model);
                }
                else
                {
                    Reust = PraiseBll.instance.AddPraise(model);
                }
                string Message = "";
                if (Reust)
                {
                    Message = "ok";
                }
                else
                {
                    Message = "no";
                }

            }
              #endregion

            context.Response.Write("ok");
            context.Response.End();
        }



        /// <summary>
        /// 获取点赞数量
        /// </summary>
        /// <param name="context"></param>
        void GetPraiseQty(HttpContext context)
        {
            //处理回调函数
            context.Response.ContentType = "application/json;charset=utf-8";
            string jsonCallBackFunName = context.Request.Params["jsoncallback"].ToString();

            string _Type = context.Request.Params["type"];
            string _TypeSys = context.Request.Params["typesys"];
            int PraiseQty = PraiseBll.instance.CountPraiseQty(_Type, _TypeSys, "1");

            //处理结果
            context.Response.Write(string.Format("{0}({1})", jsonCallBackFunName, "{message:\"" + PraiseQty + "\"}"));
            context.Response.End();
        }

        void GetPraiseQtyXML(HttpContext context)
        {

            #region validate Origin
            string _Uid = context.Request.Params["userid"];
            string _Secret = context.Request.Params["secret"];
            string _Origin = Biz_Basic_UserOrigin.instance.GetUserOriginbyCredential(_Uid, _Secret, "GetPraiseQtyXML");
            if (_Origin != "")
            {
                context.Response.AddHeader("Access-Control-Allow-Origin", _Origin); //添加允许跨域访问的请求头文件
                string _Type = context.Request.Params["type"];
                string _TypeSys = context.Request.Params["typesys"];
                int PraiseQty = PraiseBll.instance.CountPraiseQty(_Type, _TypeSys, "1");
                context.Response.Write(PraiseQty.ToString());
                context.Response.End();
            }
            #endregion

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