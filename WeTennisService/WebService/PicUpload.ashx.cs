using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.IO;
using Time;
using System.Drawing;

namespace WeTennisService.WebService
{
    /// <summary>
    /// PicUpload 的摘要说明
    /// </summary>
    public class PicUpload : IHttpHandler
    {
        public void ProcessRequest(HttpContext context)
        {
            context.Response.ContentType = "text/plain;charset=utf-8";
            //通过添加responseheader来解决跨域访问的问题
            context.Response.AddHeader("Access-Control-Allow-Origin", "*");

            string PicType = context.Request.QueryString["pictype"].ToString();//上传图片的类型
            string TypeSys = context.Request.QueryString["typesys"].ToString();//图片对应的主键
            string Memsys = context.Request.QueryString["memsys"].ToString();//会员主键
            WriteLog("UploadPara", PicType + "-" + TypeSys + "-" + Memsys);
            //根据类型获得ImagePath
            string ImgPath = "";
            if (PicType == "Times")
            {
                //时光照片
                ImgPath = System.Configuration.ConfigurationManager.AppSettings["TimePicUrl"].ToString();
            }
            else if (PicType == "Head")
            {
                //头像图片
                ImgPath = System.Configuration.ConfigurationManager.AppSettings["HeadPicUrl"].ToString();
            }

            #region 创建时光
            if (TypeSys == "" || TypeSys == null)
            {
                //新建时光
                TimeModel model = new TimeModel();
                model.MEMSYS = Memsys;
                model.TYPE = "0";
                TypeSys = TimeDAL.instance.InsertNew(model);
            }
            #endregion

            string Result = "";
            string oldUrl = "";


            //获取当前Post过来的file集合对象,在这里我只获取了<input type='file' name='fileUp'/>的文件控件
            //多个文件上传
            HttpFileCollection files = context.Request.Files;
            WriteLog("fileQty", files.Count.ToString());
            for (int i = 0; i < files.Count; i++)
            {
                HttpPostedFile file = files[i];

                if (file != null)
                {
                    #region 上传图片
                    //当前文件上传的目录
                    string path = ImgPath;
                    //当前待上传的服务端路径
                    string tail = Path.GetExtension(file.FileName);
                    if (tail == "" || tail == null)
                    {
                        tail = ".jpg";
                    }
                    string NewName = Guid.NewGuid().ToString("N").ToUpper() + tail;
                    string imageUrl = path + NewName;

                    //验证文件的大小
                    if (file.ContentLength > 1048576)
                    {
                        //这里window.parent.uploadSuccess()是我在前端页面中写好的javascript function,此方法主要用于输出异常和上传成功后的图片地址 
                        context.Response.Write("<script>window.parent.uploadSuccess('你上传的文件不能大于1048576KB!请重新上传！');</script>");
                        context.Response.End();
                    }
                    //压缩后再上传
                    //MakeThumbnailByFile(file, imageUrl, 600, 0, "W");
                    //开始上传
                    file.SaveAs(imageUrl);
                    #endregion

                    #region 修改图片信息
                    //图片url数据库存储地址
                    string NewUrl = "/" + PicType + "/" + NewName;

                    if (PicType == "Times")
                    {
                        WriteLog("UpdateNewImg", NewUrl);
                        //存储时光图片
                        TimePicsModel model = new TimePicsModel();
                        model.TIMESYS = TypeSys;
                        model.PICURL = NewUrl;
                        TimePicDal.instance.InsertNewTimePic(model);
                    }
                    #endregion

                    Result = "ok";
                }
                else
                {
                    //上传失败
                    Result = "upload lose";
                }
            }


            if (Result == "ok")
            {
                //页面跳转回原来的url
                context.Response.AddHeader("Access-Control-Allow-Origin", "http://www.wetennis.cn:82");
                context.Response.Redirect(oldUrl);
                context.Response.AddHeader("Access-Control-Allow-Origin", "http://www.wetennis.cn:82");
            }
            else
            {
                context.Response.Write(Result);
                context.Response.End();
            }


        }

        void WriteLog(string _Desc, string value1)
        {
            string sql = "insert into sys_setting (Enum,Descript,value,value2) values ('1101','" + _Desc + "','" + DateTime.Now.ToString() + "','" + value1 + "')";
            int a = DbHelperSQL.ExecuteSql(sql);
        }

        /// <summary>
        /// 生成缩略图
        /// </summary>
        /// <param name="file">上传的文件</param>
        /// <param name="thumbnailPath">缩略图路径（物理路径）</param>
        /// <param name="width">缩略图宽度</param>
        /// <param name="height">缩略图高度</param>
        /// <param name="mode">生成缩略图的方式</param> 
        public static void MakeThumbnailByFile(HttpPostedFile file, string thumbnailPath, int width, int height, string mode)
        {
            Image originalImage = Image.FromStream(file.InputStream);//将文件转化为image

            int towidth = width;
            int toheight = height;

            int x = 0;
            int y = 0;
            int ow = originalImage.Width;
            int oh = originalImage.Height;

            switch (mode)
            {
                case "HW"://指定高宽缩放（可能变形） 
                    break;
                case "W"://指定宽，高按比例 
                    toheight = originalImage.Height * width / originalImage.Width;
                    break;
                case "H"://指定高，宽按比例
                    towidth = originalImage.Width * height / originalImage.Height;
                    break;
                case "Cut"://指定高宽裁减（不变形） 
                    if ((double)originalImage.Width / (double)originalImage.Height > (double)towidth / (double)toheight)
                    {
                        oh = originalImage.Height;
                        ow = originalImage.Height * towidth / toheight;
                        y = 0;
                        x = (originalImage.Width - ow) / 2;
                    }
                    else
                    {
                        ow = originalImage.Width;
                        oh = originalImage.Width * height / towidth;
                        x = 0;
                        y = (originalImage.Height - oh) / 2;
                    }
                    break;
                default:
                    break;
            }

            //新建一个bmp图片
            Image bitmap = new System.Drawing.Bitmap(towidth, toheight);

            //新建一个画板
            Graphics g = System.Drawing.Graphics.FromImage(bitmap);

            //设置高质量插值法
            g.InterpolationMode = System.Drawing.Drawing2D.InterpolationMode.High;

            //设置高质量,低速度呈现平滑程度
            g.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.HighQuality;

            //清空画布并以透明背景色填充
            g.Clear(Color.White);

            //在指定位置并且按指定大小绘制原图片的指定部分
            g.DrawImage(originalImage, new Rectangle(0, 0, towidth, toheight),
            new Rectangle(x, y, ow, oh),
            GraphicsUnit.Pixel);

            try
            {
                //以jpg格式保存缩略图
                bitmap.Save(thumbnailPath, System.Drawing.Imaging.ImageFormat.Jpeg);
            }
            catch (System.Exception e)
            {
                throw e;
            }
            finally
            {
                originalImage.Dispose();
                bitmap.Dispose();
                g.Dispose();
            }
        }


        /// <summary>
        /// 生成缩略图
        /// </summary>
        /// <param name="originalImagePath">源图路径（物理路径）</param>
        /// <param name="thumbnailPath">缩略图路径（物理路径）</param>
        /// <param name="width">缩略图宽度</param>
        /// <param name="height">缩略图高度</param>
        /// <param name="mode">生成缩略图的方式</param> 
        public static void MakeThumbnail(string originalImagePath, string thumbnailPath, int width, int height, string mode)
        {
            Image originalImage = Image.FromFile(originalImagePath);

            int towidth = width;
            int toheight = height;

            int x = 0;
            int y = 0;
            int ow = originalImage.Width;
            int oh = originalImage.Height;

            switch (mode)
            {
                case "HW"://指定高宽缩放（可能变形） 
                    break;
                case "W"://指定宽，高按比例 
                    toheight = originalImage.Height * width / originalImage.Width;
                    break;
                case "H"://指定高，宽按比例
                    towidth = originalImage.Width * height / originalImage.Height;
                    break;
                case "Cut"://指定高宽裁减（不变形） 
                    if ((double)originalImage.Width / (double)originalImage.Height > (double)towidth / (double)toheight)
                    {
                        oh = originalImage.Height;
                        ow = originalImage.Height * towidth / toheight;
                        y = 0;
                        x = (originalImage.Width - ow) / 2;
                    }
                    else
                    {
                        ow = originalImage.Width;
                        oh = originalImage.Width * height / towidth;
                        x = 0;
                        y = (originalImage.Height - oh) / 2;
                    }
                    break;
                default:
                    break;
            }

            //新建一个bmp图片
            Image bitmap = new System.Drawing.Bitmap(towidth, toheight);

            //新建一个画板
            Graphics g = System.Drawing.Graphics.FromImage(bitmap);

            //设置高质量插值法
            g.InterpolationMode = System.Drawing.Drawing2D.InterpolationMode.High;

            //设置高质量,低速度呈现平滑程度
            g.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.HighQuality;

            //清空画布并以透明背景色填充
            g.Clear(Color.White);

            //在指定位置并且按指定大小绘制原图片的指定部分
            g.DrawImage(originalImage, new Rectangle(0, 0, towidth, toheight),
            new Rectangle(x, y, ow, oh),
            GraphicsUnit.Pixel);

            try
            {
                //以jpg格式保存缩略图
                bitmap.Save(thumbnailPath + "t", System.Drawing.Imaging.ImageFormat.Jpeg);
            }
            catch (System.Exception e)
            {
                throw e;
            }
            finally
            {
                originalImage.Dispose();
                bitmap.Dispose();
                g.Dispose();
            }
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