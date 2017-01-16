using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.IO;
using Time;

namespace WeTennisService.WebService
{
    /// <summary>
    /// PicUpload2 的摘要说明
    /// </summary>
    public class PicUpload2 : IHttpHandler
    {

        public void ProcessRequest(HttpContext context)
        {
            context.Response.ContentType = "text/plain";
            //context.Response.AddHeader("Access-Control-Allow-Origin", "http://wetennis.cn"); //添加允许跨域访问的请求头文件
            string typename = context.Request.QueryString["typename"];
            switch (typename)
            { 
                case "UploadImg":
                    UploadImg(context);
                    break;
                    //上传图片评选的图片
                case "UploadPicSel":
                    UploadPicSel(context);
                    break;

                case "UploadPicSelCORS":
                    UploadPicSelCORS(context);
                    break;

                case "UploadPicRtnUrl":
                    UploadPicRtnUrl(context);
                    break;
            }
        }
        /// <summary>
        /// 上传图片，并返回图片的地址
        /// </summary>
        /// <param name="context"></param>
        void UploadImg(HttpContext context)
        {
            HttpFileCollection files = context.Request.Files;
            for (int i = 0; i < files.Count; i++)
            { 
                //上传单个文件
                HttpPostedFile file = files[i];
                if (file != null)
                {
                    
                    //获得文件后缀
                    string tail=Path.GetExtension(file.FileName);
                    //重新设置文件名
                    string newName = Guid.NewGuid().ToString() + tail;

                    //设置文件路径
                    string ImagePath = @"D:\\Resource\images\upload\"+newName;

                    //外链地址
                    string outurl = @"http://wetennis.cn:86\upload\" + newName;

                    //验证上传文件大小
                    if (file.ContentLength > 2048000)
                    {
                        context.Response.Write("超出文件大小限制，请处理图片，文件上限为2M");
                        context.Response.End();
                    }

                    file.SaveAs(ImagePath);

                    //返回图片地址
                    context.Response.Write(outurl);
                    context.Response.End();
                }
            }
        }

        void UploadPicSel(HttpContext context)
        {
            //处理回调函数
            context.Response.ContentType = "application/json;charset=utf-8";
            string jsonCallBackFunName = context.Request.Params["jsoncallback"].ToString();

            //接收参数
            string Imgstr = context.Request.Params["imgstr"];
            string Memsys = context.Request.Params["Memsys"];
            string ImgName = context.Request.Params["ImgName"];
            string ImgDesc = context.Request.Params["ImgDesc"];

            //检查是否能够上传作品


            //保存图片
            string ImgUrl = "";
            
            //保存作品信息
                //添加时光
            TimeModel model = new TimeModel();
            model.MEMSYS = Memsys;
            model.TYPE = "0";//定义为非比赛的时光
            model.DESCRIPTION = ImgDesc;
            model.EXT1 = ImgName;//备用字段来存储作品名称
            string Timesys = TimeDAL.instance.InsertNew(model);

                //添加时光照片
            TimePicsModel tpmodel = new TimePicsModel();
            tpmodel.TIMESYS = Timesys;
            tpmodel.PICURL = ImgUrl;
            TimePicDal.instance.InsertNewTimePic(tpmodel);

            //处理查询结果
            context.Response.Write(string.Format("{0}({1})", jsonCallBackFunName, "{\"message\":\"ok\"}"));
            context.Response.End();
        }

        void UploadPicSelCORS(HttpContext context)
        {
            context.Response.AddHeader("Access-Control-Allow-Origin", "http://wetennis.cn"); //添加允许跨域访问的请求头文件

            //接收参数
            string Imgstr = context.Request.Form["imgstr"];
            string Memsys = context.Request.Form["Memsys"];
            string ImgName = context.Request.Form["ImgName"];
            string ImgDesc = context.Request.Form["ImgDesc"];

            //检查是否能够上传作品


            //保存图片
            
            string FileName = Guid.NewGuid().ToString().ToUpper();
            string fileName = FileName + ".png";
            string SaveUrl = @"D:\\Resource\images\upload\"+fileName;
            string ImgUrl = "http://wetennis.cn:86/upload/"+fileName;
            try
            {
                System.IO.FileStream fs = new System.IO.FileStream(SaveUrl, System.IO.FileMode.Create);
                System.IO.BinaryWriter bw = new System.IO.BinaryWriter(fs);
                if (!string.IsNullOrEmpty(Imgstr) && File.Exists(SaveUrl))
                {
                    Imgstr = Imgstr.Replace(" ", "+");
                    WriteLog("imgstr", Imgstr);
                    byte[] data = Convert.FromBase64String(Imgstr);//将base64 转换为byte数组
                    bw.Write(data);//将二进制存储为图片
                }
            }
            catch (Exception e){
                WriteLog("imguploaderror", e.ToString());
                ImgUrl = "www.baidu.com";
            }

            //保存作品信息
            //添加时光
            TimeModel model = new TimeModel();
            model.MEMSYS = Memsys;
            model.TYPE = "0";//定义为非比赛的时光
            model.DESCRIPTION = ImgDesc;
            model.EXT1 = "2";//定义为华侨城图片评选的时光
            model.EXT2 = ImgName;//备用字段来存储作品名称
            string Timesys = TimeDAL.instance.InsertNew(model);

            //添加时光照片
            TimePicsModel tpmodel = new TimePicsModel();
            tpmodel.TIMESYS = Timesys;
            tpmodel.PICURL = ImgUrl;
            TimePicDal.instance.InsertNewTimePic(tpmodel);
            context.Response.Write(Timesys);
        }

        /// <summary>
        /// 上传图片，返回图片url
        /// </summary>
        /// <param name="context"></param>
        void UploadPicRtnUrl(HttpContext context)
        {
            context.Response.AddHeader("Access-Control-Allow-Origin", "*"); //添加允许跨域访问的请求头文件

            //接收参数
            string Imgstr = context.Request.Params["imgstr"];
            
            //保存图片
            string FileName = Guid.NewGuid().ToString().ToUpper();
            string fileName = FileName + ".jpg";
            string SaveUrl = @"D:\\Resource\images\upload\" + fileName;
            string ImgUrl = "http://wetennis.cn:86/upload/" + fileName;
            try
            {
                System.IO.FileStream fs = new System.IO.FileStream(SaveUrl, System.IO.FileMode.Create);
                System.IO.BinaryWriter bw = new System.IO.BinaryWriter(fs);
                if (!string.IsNullOrEmpty(Imgstr) && File.Exists(SaveUrl))
                {
                    Imgstr = Imgstr.Replace(" ", "+");                   
                    byte[] data = Convert.FromBase64String(Imgstr);//将base64 转换为byte数组
                    bw.Write(data);//将二进制存储为图片
                }
            }
            catch (Exception e)
            {
                WriteLog("imguploaderror", e.ToString());
                ImgUrl = "www.baidu.com";
            }

            context.Response.Write(ImgUrl);
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